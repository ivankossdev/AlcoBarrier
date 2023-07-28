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

        EmloeyesDB test;
        EventsDB events;
        MyJson myJson = new MyJson();
        RequestInner InnerageHandler;
        RequestAlcoReader AlcoReader;

        private void InitClass()
        {
            SettingsDB setDb = new SettingsDB("settings")
            {
                path = Directory.GetCurrentDirectory(),
                InnerTable = "setInner",
                AlcoTable = "setAlco"
            };

            string[] ParamsInner = setDb.GetSettingString(setDb.InnerTable);
            string[] IpAlcoTester = setDb.GetSettingString(setDb.AlcoTable);
            test = new EmloeyesDB("employees") { path = Directory.GetCurrentDirectory() };
            events = new EventsDB("events") { path = Directory.GetCurrentDirectory() };
            InnerageHandler = new RequestInner(ParamsInner[0], ParamsInner[1], ParamsInner[2]);
            AlcoReader = new RequestAlcoReader(IpAlcoTester[0]);
        }

        private async void SystemInfo()
        {
            Result = await InnerageHandler.GetSystemInfo();
            toolStripStatusLabel1.Text = Result;
        }

        private async void OnlineMessage()
        {
            string OldRecord = string.Empty;

            while(true)
            {
                Result = await AlcoReader.GetRequestCmd(myJson.CreateCmdTypeInfMessage("getLogInf"));
                
                string LastRecord = myJson.GetCountMessage(Result);

                if (OldRecord != LastRecord)
                {
                    OldRecord = LastRecord;
                    Result = await AlcoReader.GetRequestCmd(myJson.CreateLogMessage(LastRecord));
                    string[] rows = myJson.GetArrayResult(Result);
                    if (rows[0] != null  && rows[1] != null && rows[2] != null && rows[3] != null && rows[4] != null)
                    {
                        dataGridView1.Rows.Add(rows);
                        string[] data = await Task.Run<string[]>(() => test.GetUserParam(rows[4]));
                        await Task.Run(() => events.WriteEvent(data));
                    }
                }

                await Task.Delay(250);
            }
        }
   
        private  void timer1_Tick(object sender, EventArgs e)
        {
            foreach(string[] s in events.ReadEventList())
            {
                Console.WriteLine($"{s[3]} - {s[5]} - {s[6]}");
            }
            Console.WriteLine("-------------------------------");
            
            DateTimeHandler.Function();

            timer1.Stop();
        }

        private async void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string CardCode = dataGridView1[4, e.RowIndex].Value.ToString();

            string[] u = test.GetUserParam(CardCode);
            await InnerageHandler.BlockedUser(false, u);
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
    }
}
