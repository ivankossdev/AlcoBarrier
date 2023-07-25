using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class SettingsDB : BaseSQLite
    {
        public SettingsDB(string NameDataBase) : base(NameDataBase)
        {

        }
        string SqlCommand { get; set; } = string.Empty;

        public string CreateDB()
        {
            // Добавить проверку наличия БД
            SqlCommand = $"CREATE TABLE settings " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpInnerage TEXT NOT NULL, IpAlco TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана";
        }

        public void WriteData(string IpInnerage)
        {
            SqlCommand = $"INSERT INTO settings (IpInnerage, IpAlco) VALUES (\"{IpInnerage}\", \"Test\");";
            Write(SqlCommand);
        }
    }
}
