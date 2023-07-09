using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
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
        }

        string Result = string.Empty;

        private async void SystemInfo()
        {
            textBox1.Clear();
            Result = await handler.GetSystemInfo();
            AppendTextValue(Result.Split('&'));
        }

        private async void buttonLock_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result = await handler.SetUserPermission(true);
            AppendTextValue(Result.Split('&'));
        }

        private async void buttonUnLock_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result = await handler.SetUserPermission(false);
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

        private async void buttonAlco_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            Result = await alcoReader.GetRequestCmd(MyJson.CreateCmdTypeInfMessage("getLogInf"));
            string countRec = MyJson.GetCountMessage(Result);
            Result = await alcoReader.GetRequestCmd(MyJson.CreateLogMessage("250"));
            textBox1.AppendText(MyJson.GetStringResult(Result));
        }
    }
}
