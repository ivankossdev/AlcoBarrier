using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    internal class EventsDB : BaseSQLite
    {
        public EventsDB(string NameDataBase) : base(NameDataBase)
        {

        }

        string SqlCommand { get; set; } = string.Empty;
        public override string CreateDB()
        {
            // TODO (этап 3): имя колонки "cardtamplate" — опечатка (cardtemplate).
            // Оставлено как есть, чтобы не ломать существующую БД. Переименование = отдельная миграция.
            SqlCommand = $"CREATE TABLE events " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, iduser TEXT NOT NULL, code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, cardtamplate TEXT NOT NULL, date TEXT NOT NULL, blockdate TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteEvent(string[] Table, DateTime BlockTime)
        {
            DateTime dateTime = DateTime.Now;
            // (этап 2) параметризованный INSERT.
            // (этап 3) Table приходит из EmloeyesDB.GetUserParam в порядке
            // (iduser, code, hex, CardTemplate) — теперь совпадает с порядком колонок.
            Write(
                "INSERT INTO events (iduser, code, hex, cardtamplate, date, blockdate) " +
                "VALUES (@iduser, @code, @hex, @cardtamplate, @date, @blockdate)",
                ("@iduser", Table[0]),
                ("@code", Table[1]),
                ("@hex", Table[2]),
                ("@cardtamplate", Table[3]),
                ("@date", dateTime.ToString()),
                ("@blockdate", BlockTime.ToString()));
        }

        public List<string[]> ReadEventList()
        {
            // ИЗМЕНЕНО (этап 3): явный список колонок в порядке, ожидаемом RequestInner.BlockedUser
            // и MainForm:
            //   [0]=id           -> EventsDB.DeleteString(s[0])
            //   [1]=code         -> <Name>, <CardNumber>, <CardNumberNumeric>
            //   [2]=hex          -> <CardData>
            //   [3]=iduser       -> <User Address="...">
            //   [4]=cardtamplate -> <Ref ... ID="...">
            //   [5]=date
            //   [6]=blockdate    -> MainForm.timer1_Tick (DateTime.Parse(s[6]))
            // Раньше был SELECT * (id, iduser, code, hex, ...) — поля уходили перепутанными.
            SqlCommand = "SELECT id, code, hex, iduser, cardtamplate, date, blockdate FROM events";
            return ReadListArray(SqlCommand);
        }

        public void DeleteString(string id)
        {
            // (этап 2) параметризованный DELETE; Int32.Parse сохранён.
            int parsedId = Int32.Parse(id);
            Write("DELETE FROM events WHERE id = @id", ("@id", parsedId));
        }
    }
}