using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace AlcoBarrier
{
    public partial class MainForm : Form
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _timerBusy;

        // ИЗМЕНЕНО (пункт 5): все обращения к events.db идут через этот семафор и
        // выполняются в пуле потоков — чтобы не блокировать UI и не пересекаться
        // между таймером и циклом опроса.
        private readonly SemaphoreSlim _eventsLock = new SemaphoreSlim(1, 1);

        // ИЗМЕНЕНО (пункт 2): состояние повторов блокировки по каждому событию events.id.
        private readonly Dictionary<string, BlockRetryState> _blockRetries = new Dictionary<string, BlockRetryState>();

        // ИЗМЕНЕНО (пункт 4): о том, что устройство не передаёт код карты, предупреждаем
        // один раз за запуск, чтобы не заливать лог каждым замером.
        private bool _cardNotReportedLogged;

        // ИЗМЕНЕНО (P0.5): периодическая синхронизация сотрудников.
        // Интервал берётся из settings.db (SyncMinutes), см. SettingsDB.GetSyncMinutes.
        private readonly System.Windows.Forms.Timer _syncTimer = new System.Windows.Forms.Timer();
        private int _syncBusy;
        private int _syncMinutes;

        public MainForm()
        {
            InitializeComponent();

            EnsureDatabases();
            EnsureSettings();

            this.FormClosing += MainForm_FormClosing;

            InitClass();

            _ = InitializeAsync(_cts.Token);
        }

        private void EnsureDatabases()
        {
            // ИЗМЕНЕНО (этап 3): методы CreateDB теперь используют CREATE TABLE IF NOT EXISTS.
            // Мы можем вызывать их безусловно при каждом старте. Это также гарантирует,
            // что новые индексы будут созданы в уже существующих базах данных.
            new EmloeyesDB("employees").CreateDB();
            new EventsDB("events").CreateDB();
            new SettingsDB("settings").CreateDB();
            new LogUsersDB("loguser").CreateDB();
        }

        private void EnsureSettings()
        {
            SettingsDB setDb = new SettingsDB("settings")
            {
                Path = Directory.GetCurrentDirectory()
            };

            if (setDb.GetCountId(setDb.InnerTable) == 0)
            {
                setDb.WriteSettingsInner(setDb.InnerTable, "127.0.0.1", "", "", "0", "5");
                Logger.Info("settings.db: setInner initialized with defaults (ip=127.0.0.1, block=0h 5m)");
            }

            if (setDb.GetCountId(setDb.AlcoTable) == 0)
            {
                setDb.WriteSettingsAlco(setDb.AlcoTable, "127.0.0.1");
                Logger.Info("settings.db: setAlco initialized with defaults (ip=127.0.0.1)");
            }
        }

        EmloeyesDB emploeyesDB;
        EventsDB events;
        LogUsersDB logUsersDB;
        MyJson myJson = new MyJson();
        RequestInner requestInner;
        RequestAlcoReader AlcoReader;
        string SetHour = string.Empty;
        string SetMinute = string.Empty;

        private void InitClass()
        {
            SettingsDB setDb = new SettingsDB("settings")
            {
                Path = Directory.GetCurrentDirectory()
            };

            string[] ParamsInner = setDb.GetSettingString(setDb.InnerTable);
            string[] IpAlcoTester = setDb.GetSettingString(setDb.AlcoTable);
            emploeyesDB = new EmloeyesDB("employees");
            events = new EventsDB("events");
            logUsersDB = new LogUsersDB("loguser");
            requestInner = new RequestInner(ParamsInner[0], ParamsInner[1], ParamsInner[2]);
            AlcoReader = new RequestAlcoReader(IpAlcoTester[0]);
            string[] HourMin = setDb.GetSettingsTime(setDb.InnerTable);
            SetHour = HourMin[0];
            SetMinute = HourMin[1];

            // ИЗМЕНЕНО (P0.5): интервал периодической синхронизации.
            _syncMinutes = setDb.GetSyncMinutes();
        }

        private async Task InitializeAsync(CancellationToken token)
        {
            try
            {
                await SystemInfoAsync(token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error("SystemInfo error", ex);
            }

            if (token.IsCancellationRequested) return;

            if (!Check_Databases("employees", "events", "settings", "loguser"))
                return;

            try
            {
                await SychroInnerDBAsync(token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error("SychroInnerDB error", ex);
            }

            if (!token.IsCancellationRequested)
            {
                // ИЗМЕНЕНО (P0.5): после стартовой синхронизации включаем периодическую.
                StartSyncTimer(_syncMinutes);
                _ = OnlineMessageAsync(token);
            }
        }

        /// <summary>
        /// Запускает периодическую синхронизацию сотрудников.
        /// ИЗМЕНЕНО (P0.5): интервал берётся из settings.db (SyncMinutes).
        /// Значение &lt;= 0 отключает периодическую синхронизацию — остаётся только стартовая.
        /// </summary>
        private void StartSyncTimer(int minutes)
        {
            if (minutes <= 0)
            {
                Logger.Info("SychroInnerDB: периодическая синхронизация отключена (SyncMinutes <= 0)");
                return;
            }

            const int maxMinutes = 24 * 60;
            int intervalMinutes = Math.Min(minutes, maxMinutes);

            _syncTimer.Interval = intervalMinutes * 60 * 1000;
            _syncTimer.Tick += SyncTimer_Tick;
            _syncTimer.Start();

            Logger.Info($"SychroInnerDB: периодическая синхронизация каждые {intervalMinutes} мин");
        }

        private async void SyncTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                await SychroInnerDBAsync(_cts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error("SyncTimer error", ex);
            }
        }

        private async Task SystemInfoAsync(CancellationToken token)
        {
            string result = await requestInner.GetSystemInfo(token);
            if (token.IsCancellationRequested) return;
            toolStripStatusLabel1.Text = result;

            result = await AlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getInf"));
            if (token.IsCancellationRequested) return;
            toolStripStatusLabel2.Text = myJson.GetInfoAlcoBarrier(result);
        }

        private async Task OnlineMessageAsync(CancellationToken token)
        {
            string oldRecord = string.Empty;
            int consecutiveFailures = 0;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    string result = await AlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getLogInf"));
                    if (token.IsCancellationRequested) return;

                    string lastRecord = myJson.GetCountMessage(result);

                    if (string.IsNullOrEmpty(lastRecord))
                    {
                        consecutiveFailures++;
                        if (consecutiveFailures == 1)
                        {
                            Logger.Warning("AlcoReader connection lost");
                        }
                    }
                    else
                    {
                        if (consecutiveFailures > 0)
                        {
                            Logger.Info($"AlcoReader connection restored after {consecutiveFailures} failure(s)");
                        }
                        consecutiveFailures = 0;

                        if (oldRecord != lastRecord)
                        {
                            oldRecord = lastRecord;
                            result = await AlcoReader.GetRequestCmd(myJson.CreateLogMessage(lastRecord));

                            // ИЗМЕНЕНО (пункт 5): разбор ответа и поиск карты в employees —
                            // это синхронные обращения к SQLite, выносим их с UI-потока.
                            string[] rows = await Task.Run(() => myJson.GetArrayResult(result), token);

                            if (rows[0] != null && rows[1] != null && rows[2] != null)
                            {
                                AddRowToGrid(rows);
                                await Task.Run(() => logUsersDB.WriteRow(rows), token);

                                // ИЗМЕНЕНО (пункт 4): «карта не передана» и «карта неизвестна» —
                                // разные ситуации. Раньше отсутствие Wiegand-кода превращалось
                                // в cardnum "0" и предупреждение «Unknown card: 0».
                                if (rows[4] == null)
                                {
                                    LogCardNotReported();
                                }
                                else
                                {
                                    string[] data = await Task.Run(() => emploeyesDB.GetUserParam(rows[4]), token);
                                    if (data != null)
                                    {
                                        // ИЗМЕНЕНО (пункт 5): запись события и чтение списка — под
                                        // тем же семафором, что и таймер, и вне UI-потока.
                                        List<string[]> pendingEvents = await RunEventsAsync(
                                            () =>
                                            {
                                                events.WriteEvent(data, CreateBlockTime(SetHour, SetMinute));
                                                return events.ReadEventList();
                                            },
                                            token);

                                        foreach (string[] s in pendingEvents)
                                        {
                                            if (token.IsCancellationRequested) return;
                                            await requestInner.BlockedUser(false, s, token);
                                        }
                                    }
                                    else
                                    {
                                        Logger.Warning($"Unknown card: {rows[4]} (logged to loguser, no block)");
                                    }
                                }
                            }
                        }
                    }

                    await Task.Delay(GetPollDelayMs(consecutiveFailures), token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error("OnlineMessage error", ex);
            }
        }

        private static int GetPollDelayMs(int consecutiveFailures)
        {
            if (consecutiveFailures == 0) return 250;
            if (consecutiveFailures <= 2) return 500;
            if (consecutiveFailures <= 5) return 1000;
            if (consecutiveFailures <= 10) return 3000;
            return 5000;
        }

        private void AddRowToGrid(string[] rows)
        {
            if (InvokeRequired)
                Invoke(new Action(() => dataGridView1.Rows.Add(rows)));
            else
                dataGridView1.Rows.Add(rows);
        }

        // ИЗМЕНЕНО (пункт 4): устройство по умолчанию не записывает Wiegand-коды
        // в память событий, поэтому замер может прийти без номера карты. Это не ошибка
        // сопоставления («Unknown card»), а отсутствие данных от блока сопряжения.
        private void LogCardNotReported()
        {
            if (_cardNotReportedLogged) return;

            _cardNotReportedLogged = true;
            Logger.Warning("Алкотестер не передаёт код карты (WiegandLSB отсутствует или равен 0): " +
                           "замеры сохраняются, но привязать их к сотрудникам и выполнить блокировку нельзя. " +
                           "Проверьте, что блок сопряжения записывает Wiegand-коды в память событий " +
                           "(по умолчанию запись запрещена) и что считыватель подключён к Wiegand-входу.");
        }

        private DateTime CreateBlockTime(string H, string M)
        {
            int.TryParse(H, out int hours);
            int.TryParse(M, out int minutes);
            return DateTime.Now.AddHours(hours).AddMinutes(minutes);
        }

        // ИЗМЕНЕНО (пункт 5): SQLite синхронный, поэтому операции с events
        // (ReadEventList, WriteEvent, DeleteString) выполняются в пуле потоков и
        // строго по одной. Раньше они вызывались прямо в UI-потоке и могли
        // подвешивать интерфейс, а таймер и цикл опроса работали с таблицей
        // одновременно.
        private async Task RunEventsAsync(Action action, CancellationToken token)
        {
            await _eventsLock.WaitAsync(token);
            try
            {
                await Task.Run(action, token);
            }
            finally
            {
                _eventsLock.Release();
            }
        }

        private async Task<T> RunEventsAsync<T>(Func<T> func, CancellationToken token)
        {
            await _eventsLock.WaitAsync(token);
            try
            {
                return await Task.Run(func, token);
            }
            finally
            {
                _eventsLock.Release();
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (Interlocked.CompareExchange(ref _timerBusy, 1, 0) != 0)
                return;

            try
            {
                DateTime now = DateTime.Now;
                HashSet<string> aliveIds = new HashSet<string>(StringComparer.Ordinal);

                // ИЗМЕНЕНО (пункт 5): чтение списка событий — в пуле потоков.
                List<string[]> scheduledEvents = await RunEventsAsync(() => events.ReadEventList(), _cts.Token);

                foreach (string[] s in scheduledEvents)
                {
                    if (_cts.IsCancellationRequested) return;

                    aliveIds.Add(s[0]);

                    DateTime blockTime = DateTime.Parse(s[6], CultureInfo.InvariantCulture);
                    if (blockTime >= now) continue;

                    // ИЗМЕНЕНО (пункт 2): запись из events удаляется только после того,
                    // как Innerage подтвердил блокировку. Раньше DeleteString вызывался
                    // безусловно, и при недоступном Innerage задача терялась навсегда.
                    if (!IsBlockAttemptDue(s[0], now)) continue;

                    if (await requestInner.BlockedUser(true, s, _cts.Token))
                    {
                        // ИЗМЕНЕНО (пункт 5): удаление — тоже вне UI-потока и под семафором.
                        await RunEventsAsync(() => events.DeleteString(s[0]), _cts.Token);
                        _blockRetries.Remove(s[0]);
                    }
                    else
                    {
                        // Событие остаётся в events и будет повторено с нарастающей паузой.
                        ScheduleBlockRetry(s[0]);
                    }
                }

                PruneBlockRetries(aliveIds);
            }
            catch (OperationCanceledException)
            {
            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                Logger.Error("timer1_Tick error", ex);
            }
            finally
            {
                Interlocked.Exchange(ref _timerBusy, 0);
            }
        }

        // Пункт 2: состояние повторов для конкретного события. Хранится в памяти,
        // поэтому после перезапуска приложения события из events пробуются сразу.
        private sealed class BlockRetryState
        {
            public int Failures { get; set; }
            public DateTime NextAttempt { get; set; }
        }

        private bool IsBlockAttemptDue(string id, DateTime now)
        {
            return !_blockRetries.TryGetValue(id, out BlockRetryState state) || state.NextAttempt <= now;
        }

        private void ScheduleBlockRetry(string id)
        {
            if (!_blockRetries.TryGetValue(id, out BlockRetryState state))
            {
                state = new BlockRetryState();
                _blockRetries[id] = state;
            }

            state.Failures++;

            // Пауза 5, 10, 20, 40, 80, 160, 300, 300... секунд: не долбим недоступный
            // Innerage каждые 5 секунд и не заливаем лог одинаковыми предупреждениями.
            int delaySeconds = Math.Min(5 * (int)Math.Pow(2, Math.Min(state.Failures - 1, 6)), 300);
            state.NextAttempt = DateTime.Now.AddSeconds(delaySeconds);

            if (state.Failures == 1)
            {
                Logger.Warning($"Innerage: блокировка события id={id} не выполнена, событие оставлено в events, " +
                               $"повтор через {delaySeconds} с");
            }
        }

        private void PruneBlockRetries(HashSet<string> aliveIds)
        {
            if (_blockRetries.Count == 0) return;

            foreach (string id in _blockRetries.Keys.Where(k => !aliveIds.Contains(k)).ToList())
                _blockRetries.Remove(id);
        }

        private bool Check_Databases(params string[] databases)
        {
            foreach (string database in databases)
            {
                string db = Directory.GetCurrentDirectory() + $"\\{database}.db";
                if (!File.Exists(db))
                {
                    return false;
                }
            }
            return true;
        }

        private async Task SychroInnerDBAsync(CancellationToken token)
        {
            // ИЗМЕНЕНО (P0.5): синхронизация вызывается и из инициализации, и из таймера —
            // одновременно выполняется только одна, чтобы не дублировать ReplaceAll.
            if (Interlocked.CompareExchange(ref _syncBusy, 1, 0) != 0)
                return;

            try
            {
                // ИЗМЕНЕНО (этап 3): получаем список карт и атомарно заменяем таблицу
                // в рамках одной транзакции. Если связь оборвется на середине — база останется старой.
                UsersParseResult parsed = await requestInner.GetDictUsers(token);
                if (token.IsCancellationRequested) return;

                // ИЗМЕНЕНО (пункт 1): неудачный или пустой ответ Innerage больше не обнуляет employees.
                // ИЗМЕНЕНО (рефакторинг XmlHandler): различаем сетевую ошибку, неразобранный XML,
                // отсутствие ожидаемой структуры и честно пустой список карт — в каждом случае
                // база остаётся прежней, но в лог пишется своя причина.
                if (parsed == null)
                {
                    Logger.Error("SychroInnerDB: не удалось получить ответ от Innerage, " +
                                 "локальная база employees оставлена без изменений");
                    return;
                }

                if (!parsed.XmlParsed)
                {
                    Logger.Error($"SychroInnerDB: ответ Innerage не разобран ({parsed.Error}), " +
                                 $"локальная база employees оставлена без изменений");
                    return;
                }

                if (!parsed.StructureFound)
                {
                    Logger.Warning($"SychroInnerDB: в ответе Innerage нет ожидаемой структуры ({parsed.Warning}), " +
                                   $"локальная база employees оставлена без изменений");
                    return;
                }

                if (parsed.Cards.Count == 0)
                {
                    Logger.Warning($"SychroInnerDB: Innerage не вернул ни одной карты ({parsed.Warning}), " +
                                   $"локальная база employees оставлена без изменений");
                    return;
                }

                if (!string.IsNullOrEmpty(parsed.Warning))
                    Logger.Warning($"SychroInnerDB: {parsed.Warning}");

                await Task.Run(() => emploeyesDB.ReplaceAll(parsed.Cards), token);

                Logger.Info($"SychroInnerDB: employees синхронизирована, карт: {parsed.Cards.Count}");
            }
            finally
            {
                Interlocked.Exchange(ref _syncBusy, 0);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _syncTimer.Stop();
            _syncTimer.Dispose();

            _cts.Cancel();

            requestInner?.Dispose();
            AlcoReader?.Dispose();
        }
    }
}