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
        public EditDataBases()
        {
            InitializeComponent();
        }

        private void buttonSyncBDInnerage_Click(object sender, EventArgs e)
        {
            textBoxLog.AppendText("БД Innerage\n");
        }

        private void buttonCreateSettings_Click(object sender, EventArgs e)
        {
            string message = CreateDbForButton(new SettingsDB("Settings"));
            textBoxLog.AppendText($"{message} \n");
        }

        private void buttonCreateEventsDB_Click(object sender, EventArgs e)
        {
            textBoxLog.AppendText("БД События\n");
        }

        private string CreateDbForButton(object obj)
        {

            string message = string.Empty;
            if (obj is SettingsDB setDb)
            {
                string CurrentDb = Directory.GetCurrentDirectory() + "\\Settings.db";
                if (!File.Exists(CurrentDb))
                {
                    message = setDb.CreateDB();
                }
                else
                {
                    message = "База данных настроек уже создана.";
                }
            }


            return message;
        }
    }
}
