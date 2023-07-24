using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcoBarrier
{
    public partial class EditDataBases : Form
    {
        SettingsDB SetDb = new SettingsDB("Settings");
        public EditDataBases()
        {
            InitializeComponent();
            
            string CurrentDb = Directory.GetCurrentDirectory() + "\\Settings.db";
            if (!File.Exists(CurrentDb))
            {

                buttonCreateSettings.Enabled = true;
            }
            else
            {
                buttonCreateSettings.Enabled = false;
            }
        }

        private void buttonSyncBDInnerage_Click(object sender, EventArgs e)
        {
            textBoxLog.AppendText("БД Innerage\n");
        }

        private void buttonCreateSettings_Click(object sender, EventArgs e)
        {
            string message = SetDb.CreateDB();
            textBoxLog.AppendText($"{message} \n");
            buttonCreateSettings.Enabled = false;
        }

        private void buttonCreateEventsDB_Click(object sender, EventArgs e)
        {
            textBoxLog.AppendText("БД События\n");
        }
    }
}
