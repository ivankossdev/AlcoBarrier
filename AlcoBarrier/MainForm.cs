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

        public MainForm()
        {
            InitializeComponent();

            EnsureDatabases();

            this.FormClosing += MainForm_FormClosing;

            InitClass();

            _ = InitializeAsync(_cts.Token);
        }

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
                Console.WriteLine($"SystemInfo error: {ex}");
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
                Console.WriteLine($"SychroInnerDB error: {ex}");
            }

            if (!token.IsCancellationRequested)
                _ = OnlineMessageAsync(token);
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

        /// <summary>
        /// ИЗМЕНЕНО (этап F3): backoff при недоступном AlcoReader.
        /// - consecutiveFailures инкрементируется, если GetCountMessage вернул пусто
        ///   (это означает, что GetNode вернул "Message :..." или иной невалидный ответ).
        /// - oldRecord обновляется только при успешном ответе — это заодно исправляет
        ///   пре-существующий баг "дубликат лога после реконнекта": раньше при offline
        ///   oldRecord обнулялся на "", и после восстановления связи запись обрабатывалась повторно.
        /// - Задержка между итерациями растёт: 250 → 500 → 1000 → 3000 → 5000 мс.
        /// </summary>
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
                        // (F3) пусто = ошибка (offline, таймаут, невалидный JSON).
                        // oldRecord НЕ трогаем — сохраняем последнее валидное значение.
                        consecutiveFailures++;
                    }
                    else
                    {
                        if (consecutiveFailures > 0)
                        {
                            // (F3) восстановление связи — одна строка на переход, не спам.
                            Console.WriteLine($"AlcoReader connection restored after {consecutiveFailures} failure(s)");
                        }
                        consecutiveFailures = 0;

                        if (oldRecord != lastRecord)
                        {
                            oldRecord = lastRecord;
                            result = await AlcoReader.GetRequestCmd(myJson.CreateLogMessage(lastRecord));
                            string[] rows = myJson.GetArrayResult(result);
                            if (rows[0] != null && rows[1] != null && rows[2] != null && rows[3] != null && rows[4] != null)
                            {
                                AddRowToGrid(rows);

                                await Task.Run(() => logUsersDB.WriteRow(rows), token);
                                string[] data = await Task.Run(() => emploeyesDB.GetUserParam(rows[4]), token);
                                await Task.Run(() => events.WriteEvent(data, CreateBlockTime(SetHour, SetMinute)), token);

                                foreach (string[] s in events.ReadEventList())
                                {
                                    if (token.IsCancellationRequested) return;
                                    await requestInner.BlockedUser(false, s, token);
                                }
                            }
                        }
                    }

                    // (F3) задержка с backoff.
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
                Console.WriteLine($"OnlineMessage error: {ex}");
            }
        }

        /// <summary>
        /// ДОБАВЛЕНО (этап F3): ступенчатая задержка между итерациями опроса.
        ///   онлайн (0)      : 250 мс  — как раньше
        ///   1–2 фейла       : 500 мс
        ///   3–5 фейлов      : 1000 мс
        ///   6–10 фейлов     : 3000 мс
        ///   11+ фейлов      : 5000 мс  — потолок, не растёт дальше
        /// </summary>
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

        private DateTime CreateBlockTime(string H, string M)
        {
            int.TryParse(H, out int hours);
            int.TryParse(M, out int minutes);
            return DateTime.Now.AddHours(hours).AddMinutes(minutes);
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            if (Interlocked.CompareExchange(ref _timerBusy, 1, 0) != 0)
                return;

            try
            {
                foreach (string[] s in events.ReadEventList())
                {
                    if (_cts.IsCancellationRequested) return;
                    if (DateTime.Parse(s[6]) < DateTime.Now)
                    {
                        await requestInner.BlockedUser(true, s, _cts.Token);
                        events.DeleteString(s[0]);
                    }
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
                Console.WriteLine($"timer1_Tick error: {ex}");
            }
            finally
            {
                Interlocked.Exchange(ref _timerBusy, 0);
            }
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
            await Task.Run(() => emploeyesDB.DeleteTable(), token);
            await Task.Run(() => emploeyesDB.CreateDB(), token);
            foreach (var user in await requestInner.GetDictUsers(token))
            {
                if (token.IsCancellationRequested) return;
                await Task.Run(() => emploeyesDB.WriteRow(user), token);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cts.Cancel();

            requestInner?.Dispose();
            AlcoReader?.Dispose();
        }
    }
}