using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcoBarrier
{
    public partial class EditConnection : Form
    {
        public EditConnection()
        {
            InitializeComponent();
            Console.WriteLine(SetDB.NameDataBase);
        }

        SettingsDB SetDB = new SettingsDB("settings");
        private void buttonOK_Click(object sender, EventArgs e)
        {
            
            SetDB.WriteData($"{textBoxIR.Text}", $"{textBoxAlco.Text}");
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
