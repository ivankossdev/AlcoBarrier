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

namespace Repot
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        string DateSearch = string.Empty;

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateSearch = monthCalendar1.SelectionRange.Start.ToString();
            
        }

        /*
        1. Прочитать память тестера (JSON)
        2. Записать в БД (SQLite) данные памяти тестера п.1
        3. Сортировка по выбору даты вывод на таблицу (по нажатию кнопки)
        4. 
        5.
         */
        private void button1_Click(object sender, EventArgs e)
        {
            if (DateSearch != string.Empty)
            {
                Console.WriteLine(DateSearch);
            }
            
        }
    }
}
