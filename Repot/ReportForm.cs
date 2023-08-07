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
            if (!File.Exists(reportDB.path + "\\" + reportDB.NameDataBase + ".db"))
            {
                reportDB.CreateDB();
            }

        }

        ReportDB reportDB = new ReportDB("testers")
        {
            path = Directory.GetCurrentDirectory(),
            SettingsTable = "alcopoint"
        };
 
        string DateSearch = string.Empty;

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateSearch = monthCalendar1.SelectionRange.Start.ToString();
        }

        /*
        1. Прочитать память тестера (JSON)
            1.1 $ curl  -H 'Content-Type: application/json' --data '{"cmdType":"getLogInf"}' http://192.168.0.125:443/cmd +++++
            1.2 $ curl  -H 'Content-Type: application/json' --data '{"cmdType":"getLog","Position":"toLast", "QTY":"951"}' http://192.168.0.125:443/cmd ++++
        2. Записать в БД (SQLite) данные памяти тестера п.1
        3. Сортировка по выбору даты вывод на таблицу (по нажатию кнопки)
         */

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
            foreach (string[] s in await Task.Run<List<string[]>>(() => reportDB.ReadPoints()))
            {
                toolStripComboBox1.Items.Add(s[2]);
            }
        }

        private async void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            string ip = toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex].ToString();
            MyJson myJson = new MyJson();
            RequestAlcoReader requestAlcoReader = new RequestAlcoReader(ip);
            string Result =  await requestAlcoReader.GetRequestCmd(myJson.CmdTypeHeader("getLogInf"));
            List<string[]> Memory = myJson.RecordsMemoryList(await requestAlcoReader.GetRequestCmd(myJson.CmdTypeHeaderAllMemory(Result)));
            foreach (string[] s in Memory)
            {
               dataGridView1.Rows.Add(s);
            }
        }
    }
}
