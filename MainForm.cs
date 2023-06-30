﻿using System;
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
        HttpHandler handler = new HttpHandler();   
        public MainForm()
        {
            InitializeComponent();
        }

        private async void buttonFunc_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            await handler.GetInfo("192.168.0.123");
            textBox1.AppendText(handler.res);
        }

        /* 
         * 1. Здесь добавить в трейде сервер 
         * 2. Реализовать отправку сообщения через клинет
         */
        
    }
}
