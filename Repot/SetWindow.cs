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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex == 3)
            {
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine(dataGridView1[i, e.RowIndex].Value.ToString());
                    }
                }
            }
            else if (e.ColumnIndex == 4)
            {
                reportDB.DeleteRow(dataGridView1[0, e.RowIndex].Value.ToString());
                dataGridView1.Rows.Clear();
                ShowCheckPoints(reportDB.ReadPoints());
            }

        }

        private void checkBoxEdit_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxEdit.Checked)
            {
                dataGridView1.Columns[3].Visible = true;
                dataGridView1.Columns[4].Visible = true;
            }
            else
            {
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
            }
        }
    }
}
