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
            File.Delete("employees.db");
            using (var connection = new SqliteConnection("Data Source=employees.db"))
            {
                // Name-Карта_1 User ID-U2 Card code-37358 hex 1A00000025000000EE910000 id-4ec8ed35-8e89-4bc9-b1d2-856c440cf969

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
        public static void WriteDB()
        {
            
        }
    }
}
