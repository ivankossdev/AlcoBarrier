using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class LogUsersDB : BaseSQLite
    {
        public LogUsersDB(string _NameDataBase) : base(_NameDataBase)
        {
        }

        public override string CreateDB()
        {
            string SqlCommand = $"CREATE TABLE loguser (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"date TEXT NOT NULL, " +
                $"time NEXT NOT NULL, " +
                $"promille TEXT NOT NULL, " +
                $"user TEXT," +
                $"cardnum TEXT NOT NULL);";
            Write(SqlCommand);
            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteRow(string[] rows)
        {
            Write($"INSERT INTO loguser (date, time, promille, user, cardnum) " +
                  $"VALUES (\"{rows[0]}\", \"{rows[1]}\",  \"{rows[2]}\",  \"{rows[3]}\",  \"{rows[4]}\");");
        }

        public List<string[]> ReadRows()
        {
            return ReadListArray($"SELECT date, time, promille, cardnum, user FROM loguser;");
        }

        public List<string[]> SortByDate(string date)
        {
            return ReadListArray($"SELECT date, time, promille, cardnum, user FROM loguser WHERE date = \"{date}\"");
        }

        public List<string[]> SortByNumCardAndDate(string NumCard, string Date)
        {
            return ReadListArray($"SELECT date, time, promille, cardnum, user FROM loguser WHERE cardnum LIKE \"%{NumCard}%\" AND date = \"{Date}\"");
        }

        public List<string[]> SortByNumCard(string NumCard)
        {
            return ReadListArray($"SELECT date, time, promille, cardnum, user FROM loguser WHERE cardnum LIKE \"%{NumCard}%\"");
        }

    }
}
