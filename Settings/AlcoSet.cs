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
        
        public AlcoSet()
        {
            InitializeComponent();           
        }
        private static string GetPath()
        {
            // relise ..\\AlcoBarrier\\bin\\Debug\\setting.db
            return Path.GetFullPath("..\\..\\..\\AlcoBarrier\\bin\\Debug");
        }
        SettingsDB setDb = new SettingsDB("settings") { path = GetPath() };

        private void buttonInnerOk_Click(object sender, EventArgs e)
        {
            setDb.WriteData(textBoxInnerIP.Text);

        }
    }
}
