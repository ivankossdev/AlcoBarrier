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
        // ДОБАВЛЕНО (этап 5.1): отмена всех фоновых циклов при закрытии формы.
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        // ДОБАВЛЕНО (этап 5.3): защита от реентрантности timer1_Tick.
        private int _timerBusy;

        public MainForm()
        {
            InitializeComponent();

            EnsureDatabases();

            // ДОБАВЛЕНО (этап 5.1): останавливаем циклы при закрытии формы.
            this.FormClosing += MainForm_FormClosing;

            InitClass();

            // ИЗМЕНЕНО (этап 5.2): единая асинхронная инициализация вместо
            // разрозненных async void вызовов из конструктора.
            // Порядок гарантирован: SystemInfo -> SychroInnerDB -> OnlineMessage.
            _ = InitializeAsync(_cts.Token);
        }

        /// <summary>
        /// ДОБАВЛЕНО (этап 3.5): создаёт отсутствующие файлы БД.
        /// Существующие файлы не трогает.
        /// </summary>
        private void EnsureDatabases()
        {
            string basePath = Directory.GetCurrentDirectory();

            if (!File.Exists($"{basePath}\\employees.db"))
                new EmloeyesDB("employees").CreateDB();

            if (!File.Exists($"{basePath}\\events.db"))
                new EventsDB("events").CreateDB();

            if (!File.Exists($"{basePath}\\settings.db"))
                new SettingsDB("settings").CreateDB();

            if (!File.Exists($"{basePath}\\loguser.db"))
                new LogUsersDB("loguser").CreateDB();
        }

        EmloeyesDB emploeyesDB;
        EventsDB events;
        LogUsersDB logUsersDB;
        MyJson myJson = new MyJson();
        RequestInner requestInner;
        RequestAlcoReader AlcoReader;
        string SetHour = string.Empty;
        string SetMinute = string.Empty;

        // УДАЛЕНО (этап 5.5): общее поле `string Result` — источник гонок между
        // асинхронными методами. Теперь в каждом методе локальная переменная.

        /// <summary>
        /// Иницыализация классов
        /// </summary>
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
        }

        /// <summary>
        /// ДОБАВЛЕНО (этап 5.2): единая точка асинхронной инициализации.
        /// Каждый шаг в своём try/catch, чтобы сбой одного не убивал остальные.
        /// </summary>
        private async Task InitializeAsync(CancellationToken token)
        {
            try
            {
                await SystemInfoAsync(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SystemInfo error: {ex}");
            }

            if (!Check_Databases("employees", "events", "settings", "loguser"))
                return;

            try
            {
                // ИЗМЕНЕНО (этап 5.2): дожидаемся синхронизации employees до запуска
                // OnlineMessage. Раньше SychroInnerDB пересоздавала таблицу
                // параллельно с чтением из неё в OnlineMessage.
                await SychroInnerDBAsync(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SychroInnerDB error: {ex}");
            }

            if (!token.IsCancellationRequested)
                _ = OnlineMessageAsync(token);
        }

        /// <summary>
        /// Показывает системную информацию при подключении
        /// к алкотестеру и иннеру.
        /// ИЗМЕНЕНО (этап 5.1/5.5): async Task + локальная result вместо поля.
        /// </summary>
        private async Task SystemInfoAsync(CancellationToken token)
        {
            string result = await requestInner.GetSystemInfo();
            if (token.IsCancellationRequested) return;
            toolStripStatusLabel1.Text = result;

            result = await AlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getInf"));
            if (token.IsCancellationRequested) return;
            toolStripStatusLabel2.Text = myJson.GetInfoAlcoBarrier(result);
        }

        /// <summary>
        /// Выводит в таблицу данные алкотестера и пользователя.
        /// ИЗМЕНЕНО (этап 5.1/5.4/5.5): async Task, CancellationToken,
        /// локальная result, безопасное добавление в DataGridView.
        /// </summary>
        private async Task OnlineMessageAsync(CancellationToken token)
        {
            string oldRecord = string.Empty;

            try
            {
                // ИЗМЕНЕНО (этап 5.1): while (!token.IsCancellationRequested) вместо while(true).
                while (!token.IsCancellationRequested)
                {
                    string result = await AlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getLogInf"));
                    if (token.IsCancellationRequested) return;

                    string lastRecord = myJson.GetCountMessage(result);

                    if (oldRecord != lastRecord)
                    {
                        oldRecord = lastRecord;
                        result = await AlcoReader.GetRequestCmd(myJson.CreateLogMessage(lastRecord));
                        string[] rows = myJson.GetArrayResult(result);
                        if (rows[0] != null && rows[1] != null && rows[2] != null && rows[3] != null && rows[4] != null)
                        {
                            // ИЗМЕНЕНО (этап 5.4): добавление строки — через AddRowToGrid
                            // с проверкой InvokeRequired.
                            AddRowToGrid(rows);

                            await Task.Run(() => logUsersDB.WriteRow(rows), token);
                            string[] data = await Task.Run(() => emploeyesDB.GetUserParam(rows[4]), token);
                            await Task.Run(() => events.WriteEvent(data, CreateBlockTime(SetHour, SetMinute)), token);

                            foreach (string[] s in events.ReadEventList())
                            {
                                if (token.IsCancellationRequested) return;
                                await requestInner.BlockedUser(false, s);
                            }
                        }
                    }

                    // ИЗМЕНЕНО (этап 5.1): отменяемая задержка.
                    await Task.Delay(250, token);
                }
            }
            catch (OperationCanceledException)
            {
                // Нормальная остановка при закрытии формы.
            }
            catch (Exception ex)
            {
                // ДОБАВЛЕНО (этап 5.1): раньше необработанное исключение молча убивало цикл.
                Console.WriteLine($"OnlineMessage error: {ex}");
            }
        }

        /// <summary>
        /// ДОБАВЛЕНО (этап 5.4): гарантирует добавление строки из UI-потока.
        /// </summary>
        private void AddRowToGrid(string[] rows)
        {
            if (InvokeRequired)
                Invoke(new Action(() => dataGridView1.Rows.Add(rows)));
            else
                dataGridView1.Rows.Add(rows);
        }

        /// <summary>
        /// Задает время через сколько будет заблокирован пользователь после прохода
        /// </summary>
        private DateTime CreateBlockTime(string H, string M)
        {
            DateTime DtModyfy = DateTime.Now;
            DtModyfy = DtModyfy.AddHours(Int32.Parse(H));
            DtModyfy = DtModyfy.AddMinutes(Int32.Parse(M));

            return DtModyfy;
        }

        /// <summary>
        /// Проверяет когда нужно заблокировать пользователя.
        /// ИЗМЕНЕНО (этап 5.3): защита от реентрантности через _timerBusy;
        /// добавлен try/catch, чтобы исключение не убивало таймер.
        /// Остаётся async void, т.к. это обработчик события WinForms.
        /// </summary>
        private async void timer1_Tick(object sender, EventArgs e)
        {
            // Предыдущий тик ещё выполняется — пропускаем.
            if (Interlocked.CompareExchange(ref _timerBusy, 1, 0) != 0)
                return;

            try
            {
                foreach (string[] s in events.ReadEventList())
                {
                    if (_cts.IsCancellationRequested) return;
                    if (DateTime.Parse(s[6]) < DateTime.Now)
                    {
                        await requestInner.BlockedUser(true, s);
                        events.DeleteString(s[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"timer1_Tick error: {ex}");
            }
            finally
            {
                Interlocked.Exchange(ref _timerBusy, 0);
            }
        }

        private void ConvertDate(string date)
        {
            string[] DateArray = date.Split(new char[] { '.', ' ', ':' });
            foreach (string Date in DateArray)
            {
                Console.WriteLine(Date);
            }
        }

        /// <summary>
        /// Проверяет наличае базы данных 
        /// </summary>
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

        /// <summary>
        /// Синхронизирует БД бользователей иннера с БД employees.db.
        /// ИЗМЕНЕНО (этап 5.1/5.2): async Task + CancellationToken, чтобы
        /// можно было дождаться завершения из InitializeAsync.
        /// </summary>
        private async Task SychroInnerDBAsync(CancellationToken token)
        {
            await Task.Run(() => emploeyesDB.DeleteTable(), token);
            await Task.Run(() => emploeyesDB.CreateDB(), token);
            foreach (var user in await requestInner.GetDictUsers())
            {
                if (token.IsCancellationRequested) return;
                await Task.Run(() => emploeyesDB.WriteRow(user), token);
            }
        }

        /// <summary>
        /// ДОБАВЛЕНО (этап 5.1): останавливаем фоновые циклы при закрытии формы.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cts.Cancel();
        }
    }
}