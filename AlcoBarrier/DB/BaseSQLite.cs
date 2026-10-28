using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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

        // ДОБАВЛЕНО (этап 2): перегрузка Write с параметрами.
        protected void Write(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                {
                    command.ExecuteNonQuery();
                }
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
                            Data[i] = SafeGetString(reader, i);
                        }
                    }
                    result = Data;
                }
                connection.Close();
            }

            return result;
        }

        // ДОБАВЛЕНО (этап 2): перегрузка Read с параметрами.
        protected string[] Read(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            string[] result = null;
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    string[] Data = new string[lenghtArray];
                    while (reader.Read())
                    {
                        for (int i = 0; i < lenghtArray; i++)
                        {
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

        // ДОБАВЛЕНО (этап 2): перегрузка ReadList с параметрами.
        protected List<string> ReadList(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            List<string> result = new List<string>();
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    string Data = string.Empty;
                    while (reader.Read())
                    {
                        for (int i = 0; i < lenghtArray; i++)
                        {
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

        // ДОБАВЛЕНО (этап 2): перегрузка ReadListArray с параметрами.
        protected List<string[]> ReadListArray(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            List<string[]> result = new List<string[]>();
            using (var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db"))
            {
                connection.Open();
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                using (var reader = command.ExecuteReader())
                {
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
                result[i] = SafeGetString(Data, i);
            }

            return result;
        }

        // ДОБАВЛЕНО (этап 1): NULL -> null, число -> строка через InvariantCulture, текст -> как есть.
        private static string SafeGetString(SqliteDataReader reader, int index)
        {
            return reader.IsDBNull(index)
                ? null
                : Convert.ToString(reader.GetValue(index), CultureInfo.InvariantCulture);
        }

        // ДОБАВЛЕНО (этап 2): общий конструктор команды с параметрами.
        // null-значение параметра -> DBNull.Value.
        private static SqliteCommand BuildCommand(SqliteConnection connection, string sqlCommand, (string Name, object Value)[] parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = sqlCommand;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    command.Parameters.AddWithValue(p.Name, p.Value ?? (object)DBNull.Value);
                }
            }
            return command;
        }
    }
}