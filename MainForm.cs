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
        public MainForm()
        {
            InitializeComponent();
            //new Thread(() => TcpServer.Server()).Start();

           
            svrThread.Name = "ServerThread";
            svrThread.Start();
            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           

            
            Console.WriteLine("Close app");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (svrThread.IsAlive)
            {
                Console.WriteLine("Close window\n");
                svrThread.Abort();
            }
        }
    }
}
