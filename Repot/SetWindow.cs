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

namespace Repot
{
    public partial class SetWindow : Form
    {
        public SetWindow()
        {
            InitializeComponent();

            ShowCheckPoints(reportDB.ReadPoints());
        }

        ReportDB reportDB = new ReportDB("testers")
        {
            path = Directory.GetCurrentDirectory(),
            SettingsTable = "alcopoint"
        };

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            reportDB.WritePoint(textBoxCheckPoint.Text, textBoxIP.Text);
            textBoxCheckPoint.Clear();
            textBoxIP.Clear();
            dataGridView1.Rows.Clear();
            ShowCheckPoints(reportDB.ReadPoints());
        }

        private void ShowCheckPoints(List<string[]> points)
        {
            foreach (string[] s in points)
            {
                dataGridView1.Rows.Add(s);
            }
        }
    }
}
