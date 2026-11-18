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
        public string Path { get; set; } = Directory.GetCurrentDirectory();

        public abstract string CreateDB();

        public BaseSQLite(string _NameDataBase)
        {
            NameDataBase = _NameDataBase;
        }

        // ДОБАВЛЕНО (этап 3): централизованное создание подключения.
        // Включает WAL (для параллельного чтения/записи) и таймаут (защита от Database is locked).
        private SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection($"Data Source={Path}\\{NameDataBase}.db;Default Timeout=5");
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA journal_mode=WAL; PRAGMA busy_timeout=5000;";
                command.ExecuteNonQuery();
            }
            return connection;
        }

        // ДОБАВЛЕНО (этап 3): выполнение группы команд в одной транзакции.
        protected void ExecuteInTransaction(Action<SqliteConnection> action)
        {
            using (var connection = GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    action(connection);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected void Write(string sqlCommand)
        {
            using (var connection = GetConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;
                command.ExecuteNonQuery();
            }
        }

        protected void Write(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            using (var connection = GetConnection())
            {
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        protected void Write(List<string> data)
        {
            using (var connection = GetConnection())
            {
                foreach (string _data in data)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = _data;
                    command.ExecuteNonQuery();
                }
            }
        }

        protected string[] Read(string sqlCommand)
        {
            string[] result = null;
            using (var connection = GetConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int lenghtArray = reader.FieldCount;
                        string[] Data = new string[lenghtArray];
                        for (int i = 0; i < lenghtArray; i++)
                        {
                            Data[i] = SafeGetString(reader, i);
                        }

                        if (reader.Read())
                        {
                            Logger.Warning($"Read: query returned more than one row, using first. SQL: {sqlCommand}");
                        }
                        result = Data;
                    }
                }
            }
            return result;
        }

        protected string[] Read(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            string[] result = null;
            using (var connection = GetConnection())
            {
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int lenghtArray = reader.FieldCount;
                        string[] Data = new string[lenghtArray];
                        for (int i = 0; i < lenghtArray; i++)
                        {
                            Data[i] = SafeGetString(reader, i);
                        }

                        if (reader.Read())
                        {
                            Logger.Warning($"Read: query returned more than one row, using first. SQL: {sqlCommand}");
                        }
                        result = Data;
                    }
                }
            }
            return result;
        }

        protected List<string> ReadList(string sqlCommand)
        {
            List<string> result = new List<string>();
            using (var connection = GetConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    var sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.Clear();
                        for (int i = 0; i < lenghtArray; i++)
                        {
                            sb.Append(SafeGetString(reader, i)).Append(' ');
                        }
                        result.Add(sb.ToString());
                    }
                }
            }
            return result;
        }

        protected List<string> ReadList(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            List<string> result = new List<string>();
            using (var connection = GetConnection())
            {
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                using (var reader = command.ExecuteReader())
                {
                    int lenghtArray = reader.FieldCount;
                    var sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.Clear();
                        for (int i = 0; i < lenghtArray; i++)
                        {
                            sb.Append(SafeGetString(reader, i)).Append(' ');
                        }
                        result.Add(sb.ToString());
                    }
                }
            }
            return result;
        }

        protected List<string[]> ReadListArray(string sqlCommand)
        {
            List<string[]> result = new List<string[]>();
            using (var connection = GetConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = sqlCommand;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(ArrayString(reader));
                    }
                }
            }
            return result;
        }

        protected List<string[]> ReadListArray(string sqlCommand, params (string Name, object Value)[] parameters)
        {
            List<string[]> result = new List<string[]>();
            using (var connection = GetConnection())
            {
                using (var command = BuildCommand(connection, sqlCommand, parameters))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(ArrayString(reader));
                    }
                }
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

        private static string SafeGetString(SqliteDataReader reader, int index)
        {
            return reader.IsDBNull(index)
                ? null
                : Convert.ToString(reader.GetValue(index), CultureInfo.InvariantCulture);
        }

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