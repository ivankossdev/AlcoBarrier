using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Data.Sqlite;


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
                        id TEXT NOT NULL
                    );
                ";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void WriteDB(string name, string iduser, string code, string hex, string id)
        {
            using (var connection = new SqliteConnection("Data Source=employees.db"))
            {
                // Name-Карта_1 User ID-U2 Card code-37358 hex 1A00000025000000EE910000 id-4ec8ed35-8e89-4bc9-b1d2-856c440cf969

                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $" INSERT INTO user (name, iduser, code, hex, id) VALUES (\"{name}\", \"{iduser}\", \"{code}\", \"{hex}\", \"{id}\")";
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
                command.CommandText = $"SELECT name FROM user WHERE code=\"{code}\"";
               
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

        public static void WriteUsersDb(List<Dictionary<string, string>> data)
        {
            //using (var connection = new SqliteConnection("Data Source=employees.db"))
            //{
            //    await connection.OpenAsync();
            //    var command = connection.CreateCommand();
            //    command.CommandText = $"";

            //    command.ExecuteNonQuery();
            //    connection.Close();
            //}
        }
    }
}
