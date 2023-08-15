using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repot
{
    internal class DataGridHandler
    {
        public List<ModelDB> GetData(DataGridView table)
        {
            var Records = new List<ModelDB>();

            for(int i = 0; i < table.Rows.Count; i++)
            {
                Records.Add(new ModelDB { date = table[0, i].Value.ToString(), 
                                          time = table[1, i].Value.ToString(),
                                          promille = table[2, i].Value.ToString(),
                                          numcard = table[3, i].Value.ToString(),
                                          fio = table[4, i].Value.ToString()
                });
                
            }
            return Records;
        }
    }
}
