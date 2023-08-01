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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
    }
}
