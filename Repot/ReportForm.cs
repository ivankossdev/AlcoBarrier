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
            else if (!File.Exists(emloeyesDB.Path + "\\" + reportDB.NameDataBase + ".db"))
            {
                MessageBox.Show(emloeyesDB.CreateDB());
            }

            PrintDataAlcoMemory(Task.Run<List<string[]>>(() => reportDB.ReadRows()));

        }

        AddressDB addressDB = new AddressDB("Points");

        ReportDB reportDB = new ReportDB("Report");

        EmloeyesDB emloeyesDB = new EmloeyesDB("employees");

        DateTime DateSearch;

        private async void PrintDataAlcoMemory(Task<List<string[]>> data)
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
                GetDataAlcoMemory(ip);
            }
            else if (ip == string.Empty)
                MessageBox.Show("Выбирете IP адрес алкотестера");
            else
            {
                reportDB.DeleteTable();
                reportDB.CreateDB();
                GetDataAlcoMemory(ip);
            }
        }

        private async void GetDataAlcoMemory(string ip)
        {
            MyJson myJson = new MyJson();
            RequestAlcoReader requestAlcoReader = new RequestAlcoReader(ip);
            string Result = await requestAlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getLogInf"));
            toolStripMenuItem4.Enabled = false;
            pictureBox1.Visible = true;
            toolStripMenuItem3.Enabled = false;
            List<string[]> Memory = myJson.RecordsMemoryList(await requestAlcoReader.GetRequestCmd(myJson.CmdTypeHeaderAllMemory(Result)));
            await Task.Run(() => reportDB.WriteRows(Memory));
            MessageBox.Show("Данные прочитаны.");
            toolStripMenuItem3.Enabled = true;
            PrintDataAlcoMemory(Task.Run<List<string[]>>(() => reportDB.ReadRows()));
            Memory.Clear();
        }

        private void buttonSortByDate_Click(object sender, EventArgs e)
        {
            if (DateSearch != DateTime.MinValue)
            {
                string date = $"{DateSearch:u}".Split(' ')[0];
                PrintDataAlcoMemory(Task.Run<List<string[]>>(() => reportDB.SortByDate(date)));
            }
        }

        private void buttonAllRecords_Click(object sender, EventArgs e)
        {
            PrintDataAlcoMemory(Task.Run<List<string[]>>(() => reportDB.ReadRows()));
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            string FirstName = textBoxNumCard.Text;

            if (DateSearch != DateTime.MinValue && FirstName != string.Empty)
            {
                string date = $"{DateSearch:u}".Split(' ')[0];
                PrintDataAlcoMemory(Task.Run<List<string[]>>(() => reportDB.SortByNumCardAndDate(FirstName, date)));
            }
            else if (textBoxNumCard.Text != string.Empty)
            {
                PrintDataAlcoMemory(Task.Run<List<string[]>>(() => reportDB.SortByNumCard(FirstName)));
            }
            textBoxNumCard.Clear();
        }

        InnerSetting innerSetting = new InnerSetting();
        private void toolStripAddInnerServer_Click(object sender, EventArgs e)
        {
            innerSetting.ShowDialog();
        }

        RequestInner requestInner;
        private async void toolStripSynchroServer_Click(object sender, EventArgs e)
        {
            await Task.Run(() => emloeyesDB.DeleteTable());
            await Task.Run(() => emloeyesDB.CreateDB());
            string[] settings = await Task.Run(() => addressDB.GetInnerSettings()); 
            requestInner = new RequestInner(settings[0], settings[1], settings[2]);
            foreach (var user in await requestInner.GetDictUsers())
            {
                await Task.Run(() => emloeyesDB.WriteRow(user)); 
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            DataGridHandler dataGridHandler = new DataGridHandler();
            saveFileDialog1.ShowDialog();
            CSVHandler.Writer(saveFileDialog1.FileName, dataGridHandler.GetData(dataGridView1));
        }
    }
}
