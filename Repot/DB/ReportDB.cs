using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlcoBarrier;

namespace Repot
{
    internal class ReportDB : BaseSQLite
    {
        public ReportDB(string _NameDataBase) : base(_NameDataBase)
        {
        }

        string SqlCommand { get; set; } = string.Empty;
        public override string CreateDB()
        {
            SqlCommand = $"CREATE TABLE report (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"date TEXT NOT NULL, time TEXT NOT NULL, promille TEXT NOT NULL, numcard TEXT NOT NULL);";
            Write(SqlCommand);
            return $"База данных {NameDataBase} создана \n";
        }

        public void DeleteTable()
        {
            SqlCommand = "DROP TABLE IF EXISTS report";
            Write(SqlCommand);
        }

        public void WriteRows(List<string[]> Memory)
        {
            foreach (string[] Row in Memory)
            {
                Write($"INSERT INTO report (date, time, promille, numcard) VALUES (\"{Row[0]}\", \"{Row[1]}\", \"{Row[2]}\", \"{Row[3]}\");");
            }
        }

        public List<string[]> ReadRows()
        {
            return ReadListArray($"SELECT date, time, promille, numcard FROM report;");
        }

        public List<string[]> SortByDate(string date)
        {
            return ReadListArray($"SELECT date, time, promille, numcard FROM report WHERE date = \"{date}\"");
        }

        public List<string[]> SortByNumCard(string NumCard)
        {
            return ReadListArray($"SELECT date, time, promille, numcard FROM report WHERE numcard LIKE \"%{NumCard}%\"");
        }

        public List<string[]> SortByNumCardAndDate(string NumCard, string Date)
        {
            return ReadListArray($"SELECT date, time, promille, numcard FROM report WHERE numcard LIKE \"%{NumCard}%\" AND date = \"{Date}\"");
        }
    }
}
