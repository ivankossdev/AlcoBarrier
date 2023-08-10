using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlcoBarrier;

namespace Repot
{
    public class AddressDB : BaseSQLite
    {
        public AddressDB(string _NameDataBase) : base(_NameDataBase)
        {

        }
        string SqlCommand { get; set; } = string.Empty;

        public override string CreateDB()
        {
            SqlCommand = $"CREATE TABLE alcopoint (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, accesspoint TEXT NOT NULL, " +
                $"ipaddress TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteRow(string point, string ip)
        {
            SqlCommand = $"INSERT INTO alcopoint (accesspoint, ipaddress) " +
                         $"VALUES (\"{point}\", \"{ip}\")";
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
                         $"SET accesspoint = \"{accesspoint}\", ipaddress = \"{ipaddress}\" WHERE id = {Int32.Parse(id)};";
            Write(SqlCommand);
        }
    }
}
