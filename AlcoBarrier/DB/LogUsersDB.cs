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
            // ИЗМЕНЕНО (этап 3): IF NOT EXISTS и индексы.
            string SqlCommand = $"CREATE TABLE IF NOT EXISTS loguser " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"date TEXT NOT NULL, " +
                $"time TEXT NOT NULL, " +
                $"promille TEXT NOT NULL, " +
                $"user TEXT," +
                $"cardnum TEXT NOT NULL);";
            Write(SqlCommand);

            Write("CREATE INDEX IF NOT EXISTS idx_loguser_date ON loguser(date);");
            Write("CREATE INDEX IF NOT EXISTS idx_loguser_cardnum ON loguser(cardnum);");

            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteRow(string[] rows)
        {
            // ИЗМЕНЕНО (пункт 4): замер может прийти без кода карты (устройство не передало
            // Wiegand-код). Колонка cardnum объявлена NOT NULL, поэтому, как и раньше,
            // для таких записей сохраняем "0".
            Write(
                "INSERT INTO loguser (date, time, promille, user, cardnum) " +
                "VALUES (@date, @time, @promille, @user, @cardnum)",
                ("@date", rows[0]),
                ("@time", rows[1]),
                ("@promille", rows[2]),
                ("@user", rows[3]),
                ("@cardnum", rows[4] ?? "0"));
        }

        public List<string[]> ReadRows()
        {
            return ReadListArray($"SELECT date, time, promille, cardnum, user FROM loguser;");
        }

        public List<string[]> SortByDate(string date)
        {
            return ReadListArray(
                "SELECT date, time, promille, cardnum, user FROM loguser WHERE date = @date",
                ("@date", date));
        }

        public List<string[]> SortByNumCardAndDate(string NumCard, string Date)
        {
            return ReadListArray(
                "SELECT date, time, promille, cardnum, user FROM loguser WHERE cardnum LIKE @cardnum AND date = @date",
                ("@cardnum", "%" + NumCard + "%"),
                ("@date", Date));
        }

        public List<string[]> SortByNumCard(string NumCard)
        {
            return ReadListArray(
                "SELECT date, time, promille, cardnum, user FROM loguser WHERE cardnum LIKE @cardnum",
                ("@cardnum", "%" + NumCard + "%"));
        }
    }
}