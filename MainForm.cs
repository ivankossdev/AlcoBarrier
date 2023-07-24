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
        RequestInner InnerageHandler = new RequestInner("192.168.0.123");
        RequestAlcoReader alcoReader = new RequestAlcoReader("192.168.0.125");
        EmloeyesDB test = new EmloeyesDB("employees");
        EventsDB events = new EventsDB("events");
        MyJson myJson = new MyJson();
        

        public MainForm()
        {
            InitializeComponent();
            SystemInfo();
            
            if(check_databases("employees", "events", "settings"))
            {
                OnlineMessage();
            }
            else
            {
                timer1.Stop();
                using (EditDataBases editdatabases = new EditDataBases())
                {
                    editdatabases.ShowDialog();
                }
            }
        }

        string Result = string.Empty;

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
                Result = await alcoReader.GetRequestCmd(myJson.CreateCmdTypeInfMessage("getLogInf"));
                
                string LastRecord = myJson.GetCountMessage(Result);

                if (OldRecord != LastRecord)
                {
                    OldRecord = LastRecord;
                    Result = await alcoReader.GetRequestCmd(myJson.CreateLogMessage(LastRecord));
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
            foreach(string s in events.ReadEvent())
            {
                Console.WriteLine(s);
            }

            //timer1.Stop();
        }

        private async void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string CardCode = dataGridView1[4, e.RowIndex].Value.ToString();

            string[] u = test.GetUserParam(CardCode);
            await InnerageHandler.BlockedUser(false, u);
        }

        private void MenuEditDataBase_Click(object sender, EventArgs e)
        {
            using (EditDataBases editdatabases = new EditDataBases())
            {
                editdatabases.ShowDialog();
            }
                
        }

        private void MenuConnection_Click(object sender, EventArgs e)
        {
            using(EditConnection editconnection = new EditConnection())
            {
                editconnection.ShowDialog();
            }
        }

        private bool check_databases(params string[] databases)
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
