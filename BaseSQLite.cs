using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class BaseSQLite
    {
        private readonly string _db  = string.Empty;

        public BaseSQLite(string NameDataBase)
        {
            _db = NameDataBase;
        }

        private protected void Write(string sqlCommand)
        {
            using (var connection = new SqliteConnection($"Data Source={_db}.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        private protected void Write(List<string> data)
        {
            using (var connection = new SqliteConnection($"Data Source={_db}.db"))
            {
                connection.Open();
                foreach (string User in data)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = User;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private protected string[] Read(string sqlCommand) 
        {
            string[] result = null;
            using (var connection = new SqliteConnection($"Data Source={_db}.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    string[] Data = new string[lenghtArray];
                    while (reader.Read())
                    {
                        for (int i = 0; i < lenghtArray; i++)
                        {
                            Data[i] = reader.GetString(i);
                        }
                    }
                    result = Data;
                }
                connection.Close();
            }

            return result;
        }
    }
}
