﻿using System;
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

        public void WriteSettingsInner(string Table, string IpInnerage, string Auth, string Key, string BlockHour, string BlockMinute)
        {
            SqlCommand = $"INSERT INTO {Table} (IpInnerage, authorization, key, BlockHour, BlockMinute) " +
                         $"VALUES (\"{IpInnerage.Trim()}\", \"{Auth.Trim()}\", \"{Key.Trim()}\", \"{BlockHour}\", \"{BlockMinute}\");";
            Write(SqlCommand);
        }
        
        public void ReWriteSettingsInner(string Table, string IpInnerage, string Auth, string Key, string BlockHour, string BlockMinute)
        {
            SqlCommand = $"UPDATE {Table} " +
                $"SET IpInnerage = \"{IpInnerage}\", authorization = \"{Auth}\", " +
                $"key = \"{Key}\", BlockHour = \"{BlockHour}\", BlockMinute = \"{BlockMinute}\" WHERE id = 1;";
            Write(SqlCommand);
        }

        public int GetCountId(string Table)
        {
            // !!! Поменять на ReadListArray !!!
            SqlCommand = $"SELECT id FROM {Table};";
            return ReadList(SqlCommand).Count;
        }

        public string[] GetSettingString(string Table)
        {
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
            SqlCommand = $"INSERT INTO {Table} (IpAddress) VALUES (\"{IpAddressAlco}\");";
            Write(SqlCommand);
        }

        public void ReWriteSettingsAlco(string Table, string IpAddressAlco)
        {
            SqlCommand = $"UPDATE {Table} SET IpAddress = \"{IpAddressAlco}\" WHERE id = 1;";
            Write(SqlCommand);
        }

        public string[] GetSettingsTime(string Table)
        {
            SqlCommand = $"SELECT BlockHour, BlockMinute FROM {Table};";
            return Read(SqlCommand);
        }
    }
}