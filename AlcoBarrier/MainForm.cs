﻿using System;
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

        public MainForm()
        {
            InitializeComponent();
            InitClass();
            SystemInfo();
            if (Check_Databases("employees", "events", "settings"))
            {
                OnlineMessage();
            }
        }

        string Result = string.Empty;

        EmloeyesDB emploeyesDB;
        EventsDB events;
        MyJson myJson = new MyJson();
        RequestInner InnerageHandler;
        RequestAlcoReader AlcoReader;
        string SetHour = string.Empty;
        string SetMinute = string.Empty;

        /* 
        1. Сделать логику записи событий в бд по проходу
        2. Сделать синхронизацию бд иннера по времени или запросу
        3. Сделать выгрузку событий бд в бд отчета. 
         */

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
            InnerageHandler = new RequestInner(ParamsInner[0], ParamsInner[1], ParamsInner[2]);
            AlcoReader = new RequestAlcoReader(IpAlcoTester[0]);
            string[] HourMin = setDb.GetSettingsTime(setDb.InnerTable);
            SetHour = HourMin[0];
            SetMinute = HourMin[1];
        }

        /// <summary>
        /// 01
        /// Показывает системную информацию при подключении
        /// к алкотестеру и иннеру
        /// </summary>
        private async void SystemInfo()
        {
            Result = await InnerageHandler.GetSystemInfo();
            toolStripStatusLabel1.Text = Result;

            Result = await AlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getInf"));
            toolStripStatusLabel2.Text = myJson.GetInfoAlcoBarrier(Result);
        }

        /// <summary>
        /// 02
        /// Выводит в таблицу данные алкотестера и пользователя
        /// </summary>
        private async void OnlineMessage()
        {
            string OldRecord = string.Empty;

            while(true)
            {
                Result = await AlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getLogInf"));
                
                string LastRecord = myJson.GetCountMessage(Result);

                if (OldRecord != LastRecord)
                {
                    OldRecord = LastRecord;
                    Result = await AlcoReader.GetRequestCmd(myJson.CreateLogMessage(LastRecord));
                    string[] rows = myJson.GetArrayResult(Result);
                    if (rows[0] != null  && rows[1] != null && rows[2] != null && rows[3] != null && rows[4] != null)
                    {
                        dataGridView1.Rows.Add(rows);
                        string[] data = await Task.Run<string[]>(() => emploeyesDB.GetUserParam(rows[4]));
                        await Task.Run(() => events.WriteEvent(data, CreateBlockTime(SetHour, SetMinute)));

                        foreach (string[] s in events.ReadEventList())
                        {
                            {
                                await InnerageHandler.BlockedUser(false, s);
                            }
                        }
                    }
                }

                await Task.Delay(250);
            }
        }

        /// <summary>
        /// 03 
        /// Задает время через сколько будет заблокирован пользователь после прохода
        /// </summary>
        /// <param name="H"></param>
        /// <param name="M"></param>
        /// <returns></returns>
        private DateTime CreateBlockTime(string H, string M)
        {
            DateTime DtModyfy = DateTime.Now;
            DtModyfy = DtModyfy.AddHours(Int32.Parse(H));
            DtModyfy = DtModyfy.AddMinutes(Int32.Parse(M));

            return DtModyfy;
        }

        /// <summary>
        /// 04
        /// Проверяет когда нужно заблокировать пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void timer1_Tick(object sender, EventArgs e)
        {
            foreach(string[] s in events.ReadEventList())
            {
                if (DateTime.Parse(s[6]) < DateTime.Now)
                {
                    await InnerageHandler.BlockedUser(true, s);
                    events.DeleteString(s[0]);
                }
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
        /// 05
        /// Проверяет наличае базы данных 
        /// </summary>
        /// <param name="databases"></param>
        /// <returns></returns>
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
    }
}
