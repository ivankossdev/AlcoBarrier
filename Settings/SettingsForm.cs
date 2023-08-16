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
using AlcoBarrier;

namespace Settings
{
    public partial class SettingsForm : Form
    {
        SettingsDB setDb;
        public SettingsForm()
        {
            InitializeComponent();
            setDb = new SettingsDB("settings")
            {
                #if DEBUG
                Path = Path.GetFullPath("..\\..\\..\\AlcoBarrier\\bin\\Debug")
                #else
                Path = Path.GetFullPath("..\\AlcoBarrier\\bin\\Release")
                #endif
            };
            check_db();
        }

        private void buttonInnerOk_Click(object sender, EventArgs e)
        {
                if (textBoxInnerIP.Text != "" && textBoxAuthorization.Text != "" && textBoxApiKey.Text != "")
                {
                    if (setDb.GetCountId(setDb.InnerTable) <= 0)
                    {
                        setDb.WriteSettingsInner(setDb.InnerTable, textBoxInnerIP.Text, textBoxAuthorization.Text, 
                            textBoxApiKey.Text, numericUpDownHours.Value.ToString(), numericUpDownMinuts.Value.ToString());
                    }
                    else
                    {
                        setDb.ReWriteSettingsInner(setDb.InnerTable, textBoxInnerIP.Text, textBoxAuthorization.Text, 
                            textBoxApiKey.Text, numericUpDownHours.Value.ToString(), numericUpDownMinuts.Value.ToString());
                    }
                    textBoxInnerInfo.AppendText("Настройки сохранены. \n");
                }
                else
                    textBoxInnerInfo.AppendText("Заполните все поля. \n");

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void check_db()
        {
            if (!File.Exists(setDb.Path + "\\settings.db"))
            {
                textBoxInnerInfo.AppendText(setDb.CreateDB());
            }
            else
            {
                ReadParams(setDb.InnerTable);
                ReadParams(setDb.AlcoTable);
            }
        }

        private void ReadParams(string Table)
        {
            string[] Params = { };
            if (Table == setDb.InnerTable)
            {
                Params = setDb.GetSettingString(Table);
                textBoxInnerIP.Text = Params[0];
                textBoxAuthorization.Text = Params[1];
                textBoxApiKey.Text = Params[2];
                numericUpDownHours.Value = decimal.Parse(Params[3]);
                numericUpDownMinuts.Value = decimal.Parse(Params[4]);
                textBoxInnerInfo.AppendText("БД прочитана. \n");
            }
            else
            {
                textBoxAlcoInfo.AppendText("БД прочитана. \n");
                Params = setDb.GetSettingString(Table);
                textBoxAlcoIP.Text = Params[0];
            }

        }

        private void buttonAlcoOk_Click(object sender, EventArgs e)
        {
            if (textBoxAlcoIP.Text != "" )
            {
                if (setDb.GetCountId(setDb.AlcoTable) <= 0)
                {
                    setDb.WriteSettingsAlco(setDb.AlcoTable, textBoxAlcoIP.Text);
                }
                else
                {
                    setDb.ReWriteSettingsAlco(setDb.AlcoTable, textBoxAlcoIP.Text);
                }
                textBoxAlcoInfo.AppendText("Настройки сохранены. \n");
            }
            else
                textBoxAlcoInfo.AppendText("Заполнит поле IP адрес. \n");
        }

        private void buttonAlcoCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        EmloeyesDB emloeyesDB = new EmloeyesDB("employees") 
        {
            #if DEBUG
            Path = Path.GetFullPath("..\\..\\..\\AlcoBarrier\\bin\\Debug")
            #else
            Path = Path.GetFullPath("..\\AlcoBarrier\\bin\\Release")
            #endif
        };
        RequestInner requestInner;
        private async void buttonSynchro_Click(object sender, EventArgs e)
        {
            buttonSynchro.Enabled = false;
            await Task.Run(() => emloeyesDB.DeleteTable());
            await Task.Run(() => emloeyesDB.CreateDB());
            string[] setting = setDb.GetSettingString(setDb.InnerTable);
            requestInner = new RequestInner(setting[0], setting[1], setting[2]);
            foreach (var user in await requestInner.GetDictUsers())
            {
                await Task.Run(() => emloeyesDB.WriteRow(user));
            }
            buttonSynchro.Enabled = true;
        }
    }
}
