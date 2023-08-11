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

        }

        AddressDB addressDB = new AddressDB("Points");

        ReportDB reportDB = new ReportDB("Report");

        string DateSearch = string.Empty;

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateSearch = monthCalendar1.SelectionRange.Start.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DateSearch != string.Empty)
            {
                Console.WriteLine(DateSearch);
            }
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
                PrintRows(ip);
            }
            else if (ip == string.Empty)
                MessageBox.Show("Выбирете IP адрес алкотестера");
            else
                PrintRows(ip);

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
            await Console.Out.WriteLineAsync("OK");
            Memory.Clear();
        }
    }
}
