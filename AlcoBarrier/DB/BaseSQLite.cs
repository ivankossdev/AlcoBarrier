﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public abstract class BaseSQLite
    {
        public string NameDataBase { get; }

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
                foreach (string User in data)
                {
                    var command = connection.CreateCommand();
                    command.CommandText = User;
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
                            Data[i] = reader.GetString(i);
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
                            Data += $"{reader.GetString(i)} ";

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

            for(int i = 0; i < LengthArray; i++)
            {
                result[i] = Data.GetString(i);
            }

            return result;
        }
    }
}