﻿using System;
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

            ShowCheckPoints(reportDB.ReadRows());
        }

        AddressDB reportDB = new AddressDB("Points")
        {
            Path = Directory.GetCurrentDirectory()
        };

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            reportDB.WriteRow(textBoxCheckPoint.Text, textBoxIP.Text);
            textBoxCheckPoint.Clear();
            textBoxIP.Clear();
            dataGridView1.Rows.Clear();
            ShowCheckPoints(reportDB.ReadRows());
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
                    reportDB.UpdateRow(dataGridView1[1, e.RowIndex].Value.ToString(),
                                       dataGridView1[2, e.RowIndex].Value.ToString(), 
                                       dataGridView1[0, e.RowIndex].Value.ToString());
                }
            }
            else if (e.ColumnIndex == 4)
            {
                reportDB.DeleteRow(dataGridView1[0, e.RowIndex].Value.ToString());
                dataGridView1.Rows.Clear();
                ShowCheckPoints(reportDB.ReadRows());
            }

        }

        private void checkBoxEdit_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxEdit.Checked)
            {
                dataGridView1.Columns[3].Visible = true;
                dataGridView1.Columns[4].Visible = true;
                dataGridView1.ReadOnly = false;
            }
            else
            {
                dataGridView1.ReadOnly = true;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
            }
        }
    }
}
