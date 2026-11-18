using System;
using System.Collections.Generic;
using System.Globalization;
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
            // ИЗМЕНЕНО (этап 3): IF NOT EXISTS.
            SqlCommand = $"CREATE TABLE IF NOT EXISTS events " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, iduser TEXT NOT NULL, code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, cardtamplate TEXT NOT NULL, date TEXT NOT NULL, blockdate TEXT NOT NULL);";
            Write(SqlCommand);

            // ДОБАВЛЕНО (этап 3): индекс для быстрого поиска по iduser.
            Write("CREATE INDEX IF NOT EXISTS idx_events_iduser ON events(iduser);");

            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteEvent(string[] Table, DateTime BlockTime)
        {
            DateTime dateTime = DateTime.Now;
            Write(
                "INSERT INTO events (iduser, code, hex, cardtamplate, date, blockdate) " +
                "VALUES (@iduser, @code, @hex, @cardtamplate, @date, @blockdate)",
                ("@iduser", Table[0]),
                ("@code", Table[1]),
                ("@hex", Table[2]),
                ("@cardtamplate", Table[3]),
                ("@date", dateTime.ToString(CultureInfo.InvariantCulture)),
                ("@blockdate", BlockTime.ToString(CultureInfo.InvariantCulture)));
        }

        public List<string[]> ReadEventList()
        {
            SqlCommand = "SELECT id, code, hex, iduser, cardtamplate, date, blockdate FROM events";
            return ReadListArray(SqlCommand);
        }

        public void DeleteString(string id)
        {
            if (!int.TryParse(id, out int parsedId))
            {
                Logger.Warning($"DeleteString: non-numeric id '{id}', skip");
                return;
            }

            Write("DELETE FROM events WHERE id = @id", ("@id", parsedId));
        }
    }
}