using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlcoBarrier;

namespace Repot
{
    internal class ReportDB : BaseSQLite
    {
        public ReportDB(string _NameDataBase) : base(_NameDataBase)
        {
        }

        string SqlCommand { get; set; } = string.Empty;
        public override string CreateDB()
        {
            SqlCommand = $"CREATE TABLE report (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT);";
            Write(SqlCommand);
            return $"База данных {NameDataBase} создана \n";
        }
    }
}
