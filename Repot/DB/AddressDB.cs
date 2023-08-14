using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AlcoBarrier;

namespace Repot
{
    public class AddressDB : BaseSQLite
    {
        /// <summary>
        /// Класс настройки таблицы настоек подключения 
        /// </summary>
        /// <param name="_NameDataBase"> Имя файла таблицы базы данных </param>
        public AddressDB(string _NameDataBase) : base(_NameDataBase)
        {

        }
        string SqlCommand { get; set; } = string.Empty;

        public override string CreateDB()
        {
            SqlCommand = $"CREATE TABLE alcopoint (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, accesspoint TEXT NOT NULL, " +
                $"ipaddress TEXT NOT NULL);";
            Write(SqlCommand);

            SqlCommand = $"CREATE TABLE innerserver (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ipaddress TEXT NOT NULL, " +
                $"authorization TEXT NOT NULL, apikey TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        #region AlcoSettings
        /// <summary>
        /// Настройки подключения к алкобарьеру 
        /// </summary>
        /// <param name="point"> Место установки </param>
        /// <param name="ip"> IP адрес </param>
        public void WriteRow(string point, string ip)
        {
            SqlCommand = $"INSERT INTO alcopoint (accesspoint, ipaddress) " +
                         $"VALUES (\"{point.Trim(TrimChars)}\", \"{ip.Trim(TrimChars)}\")";
            Write(SqlCommand);
        }

        public List<string[]> ReadRows()
        {
            SqlCommand = $"SELECT * FROM alcopoint";
            return ReadListArray(SqlCommand);
        }

        public void DeleteRow(string id)
        {
            SqlCommand = $"DELETE FROM alcopoint WHERE id = {Int32.Parse(id)}";
            Write(SqlCommand);
        }

        public void UpdateRow(string accesspoint, string ipaddress, string id)
        {
            SqlCommand = $"UPDATE alcopoint " +
                         $"SET accesspoint = \"{accesspoint.Trim(TrimChars)}\", " +
                         $"ipaddress = \"{ipaddress.Trim(TrimChars)}\" WHERE id = {Int32.Parse(id)};";
            Write(SqlCommand);
        }
        #endregion

        #region InnerSettings
        /// <summary>
        /// Настройки покдлючения к серверу Inner Range
        /// </summary>
        /// <param name="ipaddress"> IP адрес сервера </param>
        /// <param name="authorization"> Данные авторизации</param>
        /// <param name="apikey"> Ключ авторизации </param>
        public void WriteInnerRow(string ipaddress, string authorization, string apikey)
        {
            SqlCommand = $"INSERT INTO innerserver (ipaddress, authorization, apikey) " +
             $"VALUES (\"{ipaddress}\", \"{authorization}\", \"{apikey}\")";
            Write(SqlCommand);
        }

        public void UpdateInnerRow(string ipaddress, string authorization, string apikey)
        {
            SqlCommand = $"UPDATE innerserver SET ipaddress = \"{ipaddress}\", authorization = \"{authorization}\", apikey = \"{apikey}\" WHERE id = 1;";
            Write(SqlCommand);
        }

        public int GetCount()
        {
            return ReadList("SELECT * FROM innerserver;").Count;
        }

        public string[] GetInnerSettings()
        {
            return Read("SELECT ipaddress, authorization, apikey FROM innerserver WHERE id = 1;");
        }
        #endregion
    }
}
