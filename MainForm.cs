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

namespace AlcoBarrier
{
    public partial class MainForm : Form
    {
        public Thread svrThread = new Thread(TcpServer.Server);
        int count = 0;
        HttpHandler handler = new HttpHandler();   
        public MainForm()
        {
            InitializeComponent();
           
            //new Thread(() => TcpServer.Server()).Start();
        }

        private void buttonFunc_Click(object sender, EventArgs e)
        {
            textBox1.AppendText($"Press button {++count} \n");
            handler.GETInfo();
        }

        /* 
         * 1. Здесь добавить в трейде сервер 
         * 2. Реализовать отправку сообщения через клинет
         */
        
    }
}
