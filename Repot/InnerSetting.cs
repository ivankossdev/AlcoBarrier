using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repot
{
    public partial class InnerSetting : Form
    {
        public InnerSetting()
        {
            InitializeComponent();
            PrintDataForms();
        }

        AddressDB addresDB = new AddressDB("Points");

        void PrintDataForms()
        {
            string[] lines = addresDB.GetInnerSettings();

            textBoxInnerIP.Text = lines[0];
            textBoxAuthorization.Text = lines[1];
            textBoxApiKey.Text = lines[2];
        }

        private void buttonInnerOk_Click(object sender, EventArgs e)
        {
            if (textBoxInnerIP.Text != string.Empty && textBoxAuthorization.Text != string.Empty && textBoxApiKey.Text != string.Empty)
            {
                if (addresDB.GetCount() <= 0)
                    addresDB.WriteInnerRow(textBoxInnerIP.Text, textBoxAuthorization.Text, textBoxApiKey.Text);
                else
                    addresDB.UpdateInnerRow(textBoxInnerIP.Text, textBoxAuthorization.Text, textBoxApiKey.Text);

                textBoxInnerIP.Clear();
                textBoxAuthorization.Clear();
                textBoxApiKey.Clear();
                PrintDataForms();
            }
            else
                MessageBox.Show("Заполните все поля.");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
