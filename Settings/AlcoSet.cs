﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcoBarrier;

namespace Settings
{
    public partial class AlcoSet : Form
    {
        SettingsDB setDb;
        public AlcoSet()
        {
            InitializeComponent();
            setDb = new SettingsDB("settings")
            {
                path = Path.GetFullPath("..\\..\\..\\AlcoBarrier\\bin\\Debug"),
                InnerTable = "setInner",
                AlcoTable = "setAlco"
            };
        }

        private void buttonInnerOk_Click(object sender, EventArgs e)
        {
            if(textBoxInnerIP.Text != "" && textBoxAuthorization.Text != "" && textBoxApiKey.Text != "")
                setDb.WriteSettingsInner(setDb.InnerTable, textBoxInnerIP.Text, textBoxAuthorization.Text, textBoxApiKey.Text);
            else
                textBoxInfo.AppendText("Заполните все поля. \n");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonCreateDB_Click(object sender, EventArgs e)
        {
            textBoxInfo.AppendText(setDb.CreateDB()); 
        }
    }
}
