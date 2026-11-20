using System;
using System.Collections.Generic;
using System.Globalization;

namespace AlcoBarrier
{
    public class SettingsDB : BaseSQLite
    {
        /// <summary>Интервал периодической синхронизации по умолчанию, минуты.</summary>
        public const int DefaultSyncMinutes = 60;

        public SettingsDB(string NameDataBase) : base(NameDataBase)
        {

        }
        public string InnerTable { get; } = "setInner";
        public string AlcoTable { get; } = "setAlco";

        string SqlCommand { get; set; } = string.Empty;

        public override string CreateDB()
        {
            // ИЗМЕНЕНО (этап 3): IF NOT EXISTS.
            // ИЗМЕНЕНО (P0.5): колонка SyncMinutes — интервал периодической синхронизации.
            SqlCommand = $"CREATE TABLE IF NOT EXISTS {InnerTable} " +
                         $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpInnerage TEXT NOT NULL, " +
                         $"authorization TEXT NOT NULL, key TEXT NOT NULL, BlockHour TEXT NOT NULL, BlockMinute TEXT NOT NULL, " +
                         $"SyncMinutes TEXT NOT NULL DEFAULT '{DefaultSyncMinutes}');";
            Write(SqlCommand);

            SqlCommand = $"CREATE TABLE IF NOT EXISTS {AlcoTable} " +
                         $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpAddress TEXT NOT NULL);";
            Write(SqlCommand);

            // ИЗМЕНЕНО (P0.5): миграция уже существующих баз — добавляем колонку,
            // если её ещё нет (CREATE TABLE IF NOT EXISTS её не добавит).
            EnsureSyncMinutesColumn();

            return $"База данных {NameDataBase} создана \n";
        }

        /// <summary>
        /// Добавляет колонку SyncMinutes в уже существующую таблицу setInner.
        /// Для новых баз колонка создаётся сразу в CREATE TABLE.
        /// </summary>
        private void EnsureSyncMinutesColumn()
        {
            List<string[]> columns = ReadListArray($"PRAGMA table_info({InnerTable});");
            foreach (string[] column in columns)
            {
                if (column.Length > 1 && string.Equals(column[1], "SyncMinutes", StringComparison.OrdinalIgnoreCase))
                    return;
            }

            Write($"ALTER TABLE {InnerTable} ADD COLUMN SyncMinutes TEXT NOT NULL DEFAULT '{DefaultSyncMinutes}';");
            Logger.Info($"settings.db: добавлена колонка SyncMinutes (по умолчанию {DefaultSyncMinutes} мин)");
        }

        private void ValidateTable(string table)
        {
            if (table != InnerTable && table != AlcoTable)
                throw new ArgumentException($"Unknown table: {table}", nameof(table));
        }

        public void WriteSettingsInner(string Table, string IpInnerage, string Auth, string Key, string BlockHour, string BlockMinute, string SyncMinutes = null)
        {
            ValidateTable(Table);
            Write(
                $"INSERT INTO {Table} (IpInnerage, authorization, key, BlockHour, BlockMinute, SyncMinutes) " +
                $"VALUES (@ip, @auth, @key, @hour, @minute, @sync)",
                ("@ip", IpInnerage.Trim()),
                ("@auth", Auth.Trim()),
                ("@key", Key.Trim()),
                ("@hour", BlockHour),
                ("@minute", BlockMinute),
                ("@sync", NormalizeSyncMinutes(SyncMinutes)));
        }

        public void ReWriteSettingsInner(string Table, string IpInnerage, string Auth, string Key, string BlockHour, string BlockMinute, string SyncMinutes = null)
        {
            ValidateTable(Table);
            Write(
                $"UPDATE {Table} " +
                $"SET IpInnerage = @ip, authorization = @auth, key = @key, BlockHour = @hour, BlockMinute = @minute, " +
                $"SyncMinutes = @sync WHERE id = 1",
                ("@ip", IpInnerage),
                ("@auth", Auth),
                ("@key", Key),
                ("@hour", BlockHour),
                ("@minute", BlockMinute),
                ("@sync", NormalizeSyncMinutes(SyncMinutes)));
        }

        private static string NormalizeSyncMinutes(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? DefaultSyncMinutes.ToString(CultureInfo.InvariantCulture)
                : value.Trim();
        }

        /// <summary>
        /// Интервал периодической синхронизации из настроек, минуты.
        /// При отсутствии/некорректном значении возвращает DefaultSyncMinutes.
        /// Значение &lt;= 0 означает «только при старте» (периодическая синхронизация отключена).
        /// </summary>
        public int GetSyncMinutes()
        {
            string[] res = Read($"SELECT SyncMinutes FROM {InnerTable} LIMIT 1;");
            if (res == null || res[0] == null)
                return DefaultSyncMinutes;

            if (!int.TryParse(res[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int minutes))
                return DefaultSyncMinutes;

            return minutes;
        }

        public int GetCountId(string Table)
        {
            ValidateTable(Table);
            string[] res = Read($"SELECT COUNT(*) FROM {Table};");
            if (res == null || res[0] == null) return 0;
            return int.Parse(res[0]);
        }

        public string[] GetSettingString(string Table)
        {
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
            Write(
                $"INSERT INTO {Table} (IpAddress) VALUES (@ip)",
                ("@ip", IpAddressAlco));
        }

        public void ReWriteSettingsAlco(string Table, string IpAddressAlco)
        {
            ValidateTable(Table);
            Write(
                $"UPDATE {Table} SET IpAddress = @ip WHERE id = 1",
                ("@ip", IpAddressAlco));
        }

        public string[] GetSettingsTime(string Table)
        {
            ValidateTable(Table);
            SqlCommand = $"SELECT BlockHour, BlockMinute FROM {Table};";
            return Read(SqlCommand);
        }
    }
}