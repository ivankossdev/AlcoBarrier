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

namespace AlcoBarrier
{
    public partial class MainForm : Form
    {
        RequestInner InnerageHandler = new RequestInner("192.168.0.123");
        RequestAlcoReader alcoReader = new RequestAlcoReader("192.168.0.125");
        
        public MainForm()
        {
            InitializeComponent();
            SystemInfo();
            OnlineMessage();
            buttonTestDb.Enabled = false;
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

        private async void buttonOpen_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result = await InnerageHandler.OpenTheDoor(true);
            AppendTextValue(Result.Split('&'));
        }

        private async void buttonClose_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result = await InnerageHandler.OpenTheDoor(false);
            AppendTextValue(Result.Split('&'));
        }

        private async void OnlineMessage()
        {
            string OldRecord = string.Empty;

            while(true)
            {
                Result = await alcoReader.GetRequestCmd(MyJson.CreateCmdTypeInfMessage("getLogInf"));
                
                string LastRecord = MyJson.GetCountMessage(Result);

                if (OldRecord != LastRecord)
                {
                    OldRecord = LastRecord;
                    Result = await alcoReader.GetRequestCmd(MyJson.CreateLogMessage(LastRecord));
                    string[] rows = MyJson.GetArrayResult(Result);
                    if (rows[0] != null  && rows[1] != null && rows[2] != null && rows[3] != null)
                    {
                        dataGridView1.Rows.Add(rows);
                    }
                    
                }

                await Task.Delay(250);
            }
        }

        private async void buttonTestDb_Click(object sender, EventArgs e)
        {
            SqLiteHandler.CreateDB();
            List<string> Users = await InnerageHandler.GetDictUsers();
            await Task.Run(() => SqLiteHandler.WriteUsersDb(Users));
            buttonTestDb.Enabled = false;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.AppendText("!!! Timer !!!\n");
            string CardCode = dataGridView1[4, 0].Value.ToString();

            string[] u = SqLiteHandler.GetUserParam(CardCode);

            await InnerageHandler.BlockedUser(true, u);
            dataGridView1.Rows.RemoveAt(0);
            //timer1.Stop();
        }

        private async void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string CardCode = dataGridView1[4, e.RowIndex].Value.ToString();

            string[] u = SqLiteHandler.GetUserParam(CardCode);

            await InnerageHandler.BlockedUser(false, u);
        }
    }
}
