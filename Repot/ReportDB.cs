using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlcoBarrier;

namespace Repot
{
    public class ReportDB : BaseSQLite
    {
        public ReportDB(string _NameDataBase) : base(_NameDataBase)
        {

        }

        public string SettingsTable { get; set; }
        string SqlCommand { get; set; } = string.Empty;

        public string CreateDB()
        {
            SqlCommand = $"CREATE TABLE {SettingsTable} (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, accesspoint TEXT NOT NULL, " +
                $"ipaddress TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        public void WritePoint(string point, string ip)
        {
            SqlCommand = $"INSERT INTO {SettingsTable} (accesspoint, ipaddress) " +
                         $"VALUES (\"{point}\", \"{ip}\")";
            Write(SqlCommand);
        }

        public List<string[]> ReadPoints()
        {
            SqlCommand = $"SELECT * FROM {SettingsTable}";
            return ReadListArray(SqlCommand);
        }

    }
}
