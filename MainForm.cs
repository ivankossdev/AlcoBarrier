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
        RequestInner InnerageHandler = new RequestInner("192.168.0.123");
        RequestAlcoReader alcoReader = new RequestAlcoReader("192.168.0.125");
        Emloeyes test = new Emloeyes("employees");
        MyJson myJson = new MyJson();
        
        public MainForm()
        {
            InitializeComponent();
            SystemInfo();
            OnlineMessage();
            test.GetNameCard("37358");
        }

        string Result = string.Empty;

        private async void SystemInfo()
        {
            textBox1.Clear();
            Result = await InnerageHandler.GetSystemInfo();
            AppendTextValue(Result.Split('&'));
        }

        private void AppendTextValue(in string[] lines)
        {
            foreach (string line in lines)
            {
                textBox1.AppendText($"{line}\n");
            }
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
                    if (rows[0] != null  && rows[1] != null && rows[2] != null && rows[3] != null)
                    {
                        dataGridView1.Rows.Add(rows);
                    }
                    
                }

                await Task.Delay(250);
            }
        }
    
        DateTime t = DateTime.Now;
        private  void timer1_Tick(object sender, EventArgs e)
        {
            var v = DateTime.Now - t;
            
            textBox1.AppendText($"{v.Seconds}\n");
            //if(dataGridView1.Rows.Count > 0)
            //{
            //    string CardCode = dataGridView1[4, 0].Value.ToString();
            //    string[] u = SqLiteHandler.GetUserParam(CardCode);
            //    await InnerageHandler.BlockedUser(true, u);
            //    dataGridView1.Rows.RemoveAt(0);
            //}

            //timer1.Stop();
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string CardCode = dataGridView1[4, e.RowIndex].Value.ToString();

            string[] u = test.GetUserParam(CardCode);
            foreach(string s in u)
            {
                Console.WriteLine(s);
            }
            //await InnerageHandler.BlockedUser(false, u);
        }
    }
}
