using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcoBarrier;
using System.IO;

namespace Repot
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();

            if (!File.Exists(addressDB.Path + "\\" + addressDB.NameDataBase + ".db"))
            {
                MessageBox.Show(addressDB.CreateDB());
            }
            else if (!File.Exists(reportDB.Path + "\\" + reportDB.NameDataBase + ".db"))
            {
                MessageBox.Show(reportDB.CreateDB());
            }

            PrintFromDataBase(Task.Run<List<string[]>>(() => reportDB.ReadRows()));

        }

        AddressDB addressDB = new AddressDB("Points");

        ReportDB reportDB = new ReportDB("Report");

        DateTime DateSearch;

        private async void PrintFromDataBase(Task<List<string[]>> data)
        {
            toolStripMenuItem4.Enabled = false;
            pictureBox1.Visible = true;
            dataGridView1.Rows.Clear();
            foreach (var item in await data)
            {
                dataGridView1.Rows.Add(item);
            }

            toolStripMenuItem4.Enabled = true;
            pictureBox1.Visible = false;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateSearch = monthCalendar1.SelectionRange.Start;
        }

        private void toolAdd_Click(object sender, EventArgs e)
        {
            SetWindow setWindow = new SetWindow();
            setWindow.ShowDialog();
        }

        private async void toolReadMem_MouseHover(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Clear();
            foreach (string[] s in await Task.Run<List<string[]>>(() => addressDB.ReadRows()))
            {
                toolStripComboBox1.Items.Add(s[2]);
            }
        }

        string ip = string.Empty;
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            int index = toolStripComboBox1.SelectedIndex;

            if (index != -1)
            {
                ip = toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex].ToString();
                reportDB.DeleteTable();
                reportDB.CreateDB();
                PrintRows(ip);
            }
            else if (ip == string.Empty)
                MessageBox.Show("Выбирете IP адрес алкотестера");
            else
            {
                reportDB.DeleteTable();
                reportDB.CreateDB();
                PrintRows(ip);
            }
        }

        private async void PrintRows(string ip)
        {
            MyJson myJson = new MyJson();
            RequestAlcoReader requestAlcoReader = new RequestAlcoReader(ip);
            string Result = await requestAlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getLogInf"));
            toolStripMenuItem4.Enabled = false;
            pictureBox1.Visible = true;
            List<string[]> Memory = myJson.RecordsMemoryList(await requestAlcoReader.GetRequestCmd(myJson.CmdTypeHeaderAllMemory(Result)));
            foreach (string[] memory in Memory)
            {
                dataGridView1.Rows.Add(memory);
            }
            toolStripMenuItem4.Enabled = true;
            pictureBox1.Visible = false;
            await Task.Run(() => reportDB.WriteRows(Memory));
            MessageBox.Show("Данные прочитаны.");
            Memory.Clear();
        }

        private void buttonSortByDate_Click(object sender, EventArgs e)
        {
            if (DateSearch != DateTime.MinValue)
            {
                string date = $"{DateSearch:u}".Split(' ')[0];
                PrintFromDataBase(Task.Run<List<string[]>>(() => reportDB.SortByDate(date)));
            }
        }

        private void buttonAllRecords_Click(object sender, EventArgs e)
        {
            PrintFromDataBase(Task.Run<List<string[]>>(() => reportDB.ReadRows()));
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            Console.WriteLine(textBoxFirstName.Text);
            textBoxFirstName.Clear();
        }
    }
}
