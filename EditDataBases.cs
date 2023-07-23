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
            textBoxLog.AppendText("БД настройки\n");
        }

        private void buttonCreateEventsDB_Click(object sender, EventArgs e)
        {
            textBoxLog.AppendText("БД События\n");
        }
    }
}
