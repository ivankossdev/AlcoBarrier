using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AlcoBarrier
{
    public partial class MainForm : Form
    {
        HttpHandler handler = new HttpHandler("192.168.0.123");   
        public MainForm()
        {
            InitializeComponent();
        }

        private async void buttonFunc_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.GetInfo();
            textBox1.AppendText(handler.res);
            await handler.GetDoors();

        }

        private async void buttonLock_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.UserPermission(true);
            textBox1.AppendText(handler.res);
        }

        private async void buttonUnLock_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.UserPermission(false);
            textBox1.AppendText(handler.res);
        }
    }
}
