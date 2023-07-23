using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    internal class SettingsDB : BaseSQLite
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

            return "База данных настройки создана";
        }
    }
}
