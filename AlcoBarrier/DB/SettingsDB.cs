using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class SettingsDB : BaseSQLite
    {
        public SettingsDB(string NameDataBase) : base(NameDataBase)
        {

        }
        public string InnerTable { get; } = "setInner";
        public string AlcoTable { get; } = "setAlco";

        string SqlCommand { get; set; } = string.Empty;

        public override string CreateDB()
        {
            SqlCommand = $"CREATE TABLE {InnerTable} " +
                         $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpInnerage TEXT NOT NULL, " +
                         $"authorization TEXT NOT NULL, key TEXT NOT NULL, BlockHour TEXT NOT NULL, BlockMinute TEXT NOT NULL);";
            Write(SqlCommand);

            SqlCommand = $"CREATE TABLE {AlcoTable} " +
                         $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpAddress TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        // ДОБАВЛЕНО (этап 2): вайтлист имён таблиц. Имена таблиц нельзя параметризовать,
        // поэтому разрешаем только две известные.
        private void ValidateTable(string table)
        {
            if (table != InnerTable && table != AlcoTable)
                throw new ArgumentException($"Unknown table: {table}", nameof(table));
        }

        public void WriteSettingsInner(string Table, string IpInnerage, string Auth, string Key, string BlockHour, string BlockMinute)
        {
            ValidateTable(Table);
            // ИЗМЕНЕНО (этап 2): параметризованный INSERT. Trim() сохранён.
            Write(
                $"INSERT INTO {Table} (IpInnerage, authorization, key, BlockHour, BlockMinute) " +
                $"VALUES (@ip, @auth, @key, @hour, @minute)",
                ("@ip", IpInnerage.Trim()),
                ("@auth", Auth.Trim()),
                ("@key", Key.Trim()),
                ("@hour", BlockHour),
                ("@minute", BlockMinute));
        }

        public void ReWriteSettingsInner(string Table, string IpInnerage, string Auth, string Key, string BlockHour, string BlockMinute)
        {
            ValidateTable(Table);
            // ИЗМЕНЕНО (этап 2): параметризованный UPDATE.
            Write(
                $"UPDATE {Table} " +
                $"SET IpInnerage = @ip, authorization = @auth, key = @key, BlockHour = @hour, BlockMinute = @minute " +
                $"WHERE id = 1",
                ("@ip", IpInnerage),
                ("@auth", Auth),
                ("@key", Key),
                ("@hour", BlockHour),
                ("@minute", BlockMinute));
        }

        public int GetCountId(string Table)
        {
            // ИЗМЕНЕНО (этап 2): вайтлист имён таблиц.
            ValidateTable(Table);
            // !!! Поменять на ReadListArray !!!
            SqlCommand = $"SELECT id FROM {Table};";
            return ReadList(SqlCommand).Count;
        }

        public string[] GetSettingString(string Table)
        {
            // ИЗМЕНЕНО (этап 2): вайтлист имён таблиц.
            ValidateTable(Table);
            if (Table == AlcoTable)
            {
                SqlCommand = $"SELECT IpAddress FROM {Table};";
            }
            else
            {
                SqlCommand = $"SELECT IpInnerage, authorization, key, BlockHour, BlockMinute FROM {Table};";
            }

            return Read(SqlCommand);
        }

        public void WriteSettingsAlco(string Table, string IpAddressAlco)
        {
            ValidateTable(Table);
            // ИЗМЕНЕНО (этап 2): параметризованный INSERT.
            Write(
                $"INSERT INTO {Table} (IpAddress) VALUES (@ip)",
                ("@ip", IpAddressAlco));
        }

        public void ReWriteSettingsAlco(string Table, string IpAddressAlco)
        {
            ValidateTable(Table);
            // ИЗМЕНЕНО (этап 2): параметризованный UPDATE.
            Write(
                $"UPDATE {Table} SET IpAddress = @ip WHERE id = 1",
                ("@ip", IpAddressAlco));
        }

        public string[] GetSettingsTime(string Table)
        {
            // ИЗМЕНЕНО (этап 2): вайтлист имён таблиц.
            ValidateTable(Table);
            SqlCommand = $"SELECT BlockHour, BlockMinute FROM {Table};";
            return Read(SqlCommand);
        }
    }
}