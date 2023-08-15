using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlcoBarrier;

namespace AlcoBarrier
{
    public class ReportDB : BaseSQLite
    {
        public ReportDB(string _NameDataBase) : base(_NameDataBase)
        {
        }

        string SqlCommand { get; set; } = string.Empty;
        public override string CreateDB()
        {
            SqlCommand = $"CREATE TABLE report (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"date TEXT NOT NULL, time TEXT NOT NULL, promille TEXT NOT NULL, numcard TEXT NOT NULL, fio TEXT);";
            Write(SqlCommand);
            return $"База данных {NameDataBase} создана \n";
        }

        public void DeleteTable()
        {
            SqlCommand = "DROP TABLE IF EXISTS report";
            Write(SqlCommand);
        }

        EmloeyesDB emloeyesDB = new EmloeyesDB("employees"); 
        public void WriteRows(List<string[]> Memory)
        {
            foreach (string[] Row in Memory)
            {   
                Write($"INSERT INTO report (date, time, promille, numcard, fio) " +
                    $"VALUES (\"{Row[0]}\", \"{Row[1]}\", \"{Row[2]}\", \"{Row[3]}\", \" {emloeyesDB.GetNameCard(Row[3])}\");");
            }
        }

        public void WriteFIO(string fio, string numcard)
        {
            Write($"UPDATE report SET fio = \"{fio}\" WHERE numcard = \"{numcard}\";");
        }

        public List<string[]> ReadRows()
        {
            return ReadListArray($"SELECT date, time, promille, numcard, fio FROM report;");
        }

        public List<string[]> SortByDate(string date)
        {
            return ReadListArray($"SELECT date, time, promille, numcard, fio FROM report WHERE date = \"{date}\"");
        }

        public List<string[]> SortByNumCard(string NumCard)
        {
            return ReadListArray($"SELECT date, time, promille, numcard, fio FROM report WHERE numcard LIKE \"%{NumCard}%\"");
        }

        public List<string[]> SortByNumCardAndDate(string NumCard, string Date)
        {
            return ReadListArray($"SELECT date, time, promille, numcard, fio FROM report WHERE numcard LIKE \"%{NumCard}%\" AND date = \"{Date}\"");
        }
    }
}
