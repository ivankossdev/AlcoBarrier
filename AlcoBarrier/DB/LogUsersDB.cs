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
            // ИЗМЕНЕНО (этап 2): параметризованный INSERT.
            Write(
                "INSERT INTO loguser (date, time, promille, user, cardnum) " +
                "VALUES (@date, @time, @promille, @user, @cardnum)",
                ("@date", rows[0]),
                ("@time", rows[1]),
                ("@promille", rows[2]),
                ("@user", rows[3]),
                ("@cardnum", rows[4]));
        }

        public List<string[]> ReadRows()
        {
            return ReadListArray($"SELECT date, time, promille, cardnum, user FROM loguser;");
        }

        public List<string[]> SortByDate(string date)
        {
            // ИЗМЕНЕНО (этап 2): параметризованный WHERE.
            return ReadListArray(
                "SELECT date, time, promille, cardnum, user FROM loguser WHERE date = @date",
                ("@date", date));
        }

        public List<string[]> SortByNumCardAndDate(string NumCard, string Date)
        {
            // ИЗМЕНЕНО (этап 2): параметризованный LIKE + WHERE.
            return ReadListArray(
                "SELECT date, time, promille, cardnum, user FROM loguser WHERE cardnum LIKE @cardnum AND date = @date",
                ("@cardnum", "%" + NumCard + "%"),
                ("@date", Date));
        }

        public List<string[]> SortByNumCard(string NumCard)
        {
            // ИЗМЕНЕНО (этап 2): параметризованный LIKE.
            return ReadListArray(
                "SELECT date, time, promille, cardnum, user FROM loguser WHERE cardnum LIKE @cardnum",
                ("@cardnum", "%" + NumCard + "%"));
        }

    }
}