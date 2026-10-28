using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
// ДОБАВЛЕНО (этап 1): для CultureInfo.InvariantCulture в SafeGetString
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public abstract class BaseSQLite
    {
        public string NameDataBase { get; }

        public char[] TrimChars { get; } = { ';', '>', '<', '\'', '\\', '{', '}', ':', '-', ' ', '%', '$' };

        public abstract string CreateDB();

        public BaseSQLite(string _NameDataBase)
        {
            NameDataBase = _NameDataBase;

        }
        public string Path { get; set; } = Directory.GetCurrentDirectory();

        protected void Write(string sqlCommand)
        {
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        protected void Write(List<string> data)
        {
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                foreach (string _data in data)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = _data;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        protected string[] Read(string sqlCommand)
        {
            string[] result = null;
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
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
                            // ИЗМЕНЕНО (этап 1): было reader.GetString(i) — падало на INTEGER/NULL.
                            // Теперь безопасное чтение через SafeGetString.
                            Data[i] = SafeGetString(reader, i);
                        }
                    }
                    result = Data;
                }
                connection.Close();
            }

            return result;
        }

        protected List<string> ReadList(string sqlCommand)
        {
            List<string> result = new List<string>();

            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    string Data = string.Empty;
                    while (reader.Read())
                    {
                        for (int i = 0; i < lenghtArray; i++)
                        {
                            // ИЗМЕНЕНО (этап 1): было reader.GetString(i) — падало на INTEGER/NULL.
                            Data += $"{SafeGetString(reader, i)} ";

                        }
                        result.Add(Data);
                        Data = string.Empty;
                    }
                }
                connection.Close();
            }
            return result;
        }

        protected List<string[]> ReadListArray(string sqlCommand)
        {
            List<string[]> result = new List<string[]>();

            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    while (reader.Read())
                    {
                        result.Add(ArrayString(reader));
                    }
                }
                connection.Close();
            }
            return result;
        }

        private string[] ArrayString(SqliteDataReader Data)
        {
            int LengthArray = Data.FieldCount;
            string[] result = new string[LengthArray];

            for (int i = 0; i < LengthArray; i++)
            {
                // ИЗМЕНЕНО (этап 1): было Data.GetString(i) — падало на INTEGER/NULL.
                // Именно этот метод вызывается из ReadListArray (EventsDB.ReadEventList и т.п.).
                result[i] = SafeGetString(Data, i);
            }

            return result;
        }

        // ДОБАВЛЕНО (этап 1): единая точка безопасного чтения значения из reader.
        // NULL -> null, число -> строка через InvariantCulture, текст -> как есть.
        private static string SafeGetString(SqliteDataReader reader, int index)
        {
            return reader.IsDBNull(index)
                ? null
                : Convert.ToString(reader.GetValue(index), CultureInfo.InvariantCulture);
        }
    }
}