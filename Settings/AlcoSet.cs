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
            check_db();
        }

        private void buttonInnerOk_Click(object sender, EventArgs e)
        {
                if (textBoxInnerIP.Text != "" && textBoxAuthorization.Text != "" && textBoxApiKey.Text != "")
                {
                    if (setDb.GetCountId(setDb.InnerTable) <= 0)
                    {
                        setDb.WriteSettingsInner(setDb.InnerTable, textBoxInnerIP.Text, textBoxAuthorization.Text, textBoxApiKey.Text);
                    }
                    else
                    {
                        setDb.ReWriteSettingsInner(setDb.InnerTable, textBoxInnerIP.Text, textBoxAuthorization.Text, textBoxApiKey.Text);
                    } 
                    ClearTextBox();
                    ReadParams(setDb.InnerTable);
                    textBoxInfo.AppendText("Настройки сохранены. \n");
                }
                else
                    textBoxInfo.AppendText("Заполните все поля. \n");

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void check_db()
        {
            if (!File.Exists(setDb.path + "\\settings.db"))
            {
                textBoxInfo.AppendText(setDb.CreateDB());
            }
            else
            {
                ReadParams(setDb.InnerTable);
                textBoxInfo.AppendText("БД прочитана. \n");
            }
        }

        private void ReadParams(string Table)
        {
            string[] Params = setDb.GetSettingString(Table);
            textBoxInnerIP.Text = Params[0];
            textBoxAuthorization.Text = Params[1];
            textBoxApiKey.Text = Params[2];
        }

        private void ClearTextBox()
        {
            textBoxInnerIP.Clear();
            textBoxAuthorization.Clear();
            textBoxApiKey.Clear();
        }
    }
}
