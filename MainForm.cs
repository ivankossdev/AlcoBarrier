using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            Server();
            
        }

        private async void Server()
        {
            var tcpListener = TcpListener.Create(10500);
            tcpListener.Start();
            for (; ; )
            {
                Console.WriteLine("[Server] waiting for clients...");
                using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                {
                    try
                    {
                        Console.WriteLine("[Server] Client has connected");
                        using (var networkStream = tcpClient.GetStream())
                        using (var reader = new StreamReader(networkStream))
                        using (var writer = new StreamWriter(networkStream) { AutoFlush = true })
                        {
                            var buffer = new byte[4096];
                            Console.WriteLine("[Server] Reading from client");

                            //for (int i = 0; i < 5; i++)
                            //{
                            //    await writer.WriteLineAsync("I am the server! HAHAHA!");
                            //    Console.WriteLine("[Server] Response has been written");
                            //    await Task.Delay(TimeSpan.FromSeconds(1));
                            //}
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("[Server] client connection lost");
                    }
                }
            }
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
            string LastRecord = MyJson.GetCountMessage(Result);
            Result = await alcoReader.GetRequestCmd(MyJson.CreateLogMessage(LastRecord));
            textBox1.AppendText(Result);
            //if (MyJson.GetPpmResult(Result) > 0)
            //{
            //    await handler.SetUserPermission(true);
            //}
            //else
            //{
            //    await handler.SetUserPermission(false);
            //}
        }
    }
}
