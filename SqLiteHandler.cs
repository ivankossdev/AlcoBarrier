using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;


namespace AlcoBarrier
{
    public static class SqLiteHandler
    {
        public static void CreateDB()
        {
            using (var connection = new SqliteConnection("Data Source=employees.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    CREATE TABLE user (
                        idkey INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        name TEXT NOT NULL,
                        iduser TEXT NOT NULL,
                        code TEXT NOT NULL,
                        hex TEXT NOT NULL,
                        id TEXT NOT NULL,
                        CardTemplate TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static string GetNameCard(string code)
        {
            string CardName = string.Empty;

            using (var connection = new SqliteConnection("Data Source=employees.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT name FROM user WHERE code LIKE \"%{code}%\"";
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CardName = reader.GetString(0);
                    }
                }
                connection.Close();
            }

            return CardName;
        }

        public static void WriteUsersDb(List<string> data)
        {
            using (var connection = new SqliteConnection("Data Source=employees.db"))
            {
                connection.Open();
                foreach (string User in data)
                {
                    var command = connection.CreateCommand();
                    string[] usersData = User.Split('&');
                    command.CommandText = $" INSERT INTO user (name, iduser, code, hex, id, CardTemplate) " +
                        $"VALUES (\"{usersData[0]}\", \"{usersData[1]}\", \"{usersData[2]}\", \"{usersData[3]}\", \"{usersData[4]}\", \"{usersData[5]}\")";
                    command.ExecuteNonQuery();
                }
                
                connection.Close();
            }
        }
    }
}
