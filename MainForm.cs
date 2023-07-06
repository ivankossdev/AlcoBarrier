using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        HttpRequestInner handler = new HttpRequestInner("192.168.0.123");
        HttpRequestAlcoReader alcoReader = new HttpRequestAlcoReader("192.168.0.125");
        public MainForm()
        {
            InitializeComponent();
            SystemInfo();
        }

        private async void SystemInfo()
        {
            textBox1.Clear();
            await handler.GetSystemInfo();
            AppendTextValue(handler.res.Split('&'));
        }

        private async void buttonLock_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.SetUserPermission(true);
            AppendTextValue(handler.res.Split('&'));
        }

        private async void buttonUnLock_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.SetUserPermission(false);
            AppendTextValue(handler.res.Split('&'));
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
            await handler.OpenTheDoor(true);
            AppendTextValue(handler.res.Split('&'));
        }

        private async void buttonClose_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.OpenTheDoor(false);
            AppendTextValue(handler.res.Split('&'));
        }

        private async void buttonAlco_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await alcoReader.GetInfo();
            textBox1.AppendText(alcoReader.res);
        }
    }
}
