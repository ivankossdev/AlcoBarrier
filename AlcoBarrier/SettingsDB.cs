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
        public string InnerTable { get; set; }
        public string AlcoTable { get; set; }
        string SqlCommand { get; set; } = string.Empty;

        public string CreateDB()
        {
            SqlCommand = $"CREATE TABLE {InnerTable} " +
                         $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpInnerage TEXT NOT NULL, authorization TEXT NOT NULL, key TEXT NOT NULL);";
            Write(SqlCommand);

            SqlCommand = $"CREATE TABLE {AlcoTable} " +
                         $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, IpAddress TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteSettingsInner(string Table, string IpInnerage, string Auth, string Key)
        {
            SqlCommand = $"INSERT INTO {Table} (IpInnerage, authorization, key) VALUES (\"{IpInnerage}\", \"{Auth}\", \"{Key}\");";
            Write(SqlCommand);
        }

        public void WriteSettingsAlco(string Table, string IpAddressAlco)
        {
            SqlCommand = $"INSERT INTO {Table} (IpAddress) VALUES (\"{IpAddressAlco}\");";
            Write(SqlCommand);
        }
    }
}
