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
        RequestInner handler = new RequestInner("192.168.0.123");
        RequestAlcoReader alcoReader = new RequestAlcoReader("192.168.0.125");
        
        public MainForm()
        {
            InitializeComponent();
            SystemInfo();
            OnlineMessage();
        }

        string Result = string.Empty;

        private async void SystemInfo()
        {
            textBox1.Clear();
            Result = await handler.GetSystemInfo();
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
            Result = await handler.OpenTheDoor(true);
            AppendTextValue(Result.Split('&'));
        }

        private async void buttonClose_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result = await handler.OpenTheDoor(false);
            AppendTextValue(Result.Split('&'));
        }

        private async void OnlineMessage()
        {
            int count = 0;
            string OldRecord = string.Empty;

            while(true)
            {
                Result = await alcoReader.GetRequestCmd(MyJson.CreateCmdTypeInfMessage("getLogInf"));
                
                string LastRecord = MyJson.GetCountMessage(Result);

                if (OldRecord != LastRecord)
                {
                    OldRecord = LastRecord;
                    Result = await alcoReader.GetRequestCmd(MyJson.CreateLogMessage(LastRecord));
                    //await Console.Out.WriteLineAsync(Result);
                    textBox1.AppendText($"{MyJson.GetStringResult(Result)} \n");
                    count++;
                    if (count > 10)
                    {
                        textBox1.Clear();
                        count = 0;
                    }
                }

                await Task.Delay(250);
            }
        }
    }
}
