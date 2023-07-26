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
            //string message = CreateDbForButton(new SettingsDB("settings"));
            //textBoxLog.AppendText($"{message} \n");
        }

        private void buttonCreateEventsDB_Click(object sender, EventArgs e)
        {
            string message = CreateDbForButton(new EventsDB("events"));
            textBoxLog.AppendText($"{message} \n");
        }

        private string CreateDbForButton(object obj)
        {
            string message = string.Empty;
            string CurrentDb = string.Empty;
            if (obj is SettingsDB setDb)
            {
                CurrentDb = Directory.GetCurrentDirectory() + "\\settings.db";
                if (!File.Exists(CurrentDb))
                {
                    message = setDb.CreateDB();
                }
                else
                {
                    message = "База данных уже созадана.";
                }
            }
            else if (obj is EventsDB eventsDb)
            {
                CurrentDb = Directory.GetCurrentDirectory() + "\\events.db";
                if (!File.Exists(CurrentDb))
                {
                    eventsDb.CreateDB();
                    message = "База данных событий создана";

                }
                else
                {
                    message = "База данных уже созадана.";
                }
            }
            else if (obj is EmloeyesDB employesDb)
            {
                CurrentDb = Directory.GetCurrentDirectory() + "\\employees.db";
                if (!File.Exists(CurrentDb))
                {
                    employesDb.CreateDB("user");
                    message = "База данных пользователей создана";

                }
                else
                {
                    message = "База данных уже созадана.";
                }
            }
            else
                message = "1234";

            return message;
        }
    }
}
