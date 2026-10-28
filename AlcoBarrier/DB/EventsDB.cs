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
            SqlCommand = $"CREATE TABLE events " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, iduser TEXT NOT NULL, code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, cardtamplate TEXT NOT NULL, date TEXT NOT NULL, blockdate TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
        }

        public void WriteEvent(string[] Table, DateTime BlockTime)
        {
            DateTime dateTime = DateTime.Now;
            // ИЗМЕНЕНО (этап 2): параметризованный INSERT.
            // ВНИМАНИЕ: для date/blockdate сохранён прежний .ToString() (CurrentCulture),
            // чтобы формат хранения и DateTime.Parse в MainForm не изменились.
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
            SqlCommand = "SELECT * FROM events";
            return ReadListArray(SqlCommand);
        }

        public void DeleteString(string id)
        {
            // ИЗМЕНЕНО (этап 2): DELETE через параметр.
            // Int32.Parse оставлен намеренно — чтобы тип исключения на невалидном id не менялся.
            int parsedId = Int32.Parse(id);
            Write("DELETE FROM events WHERE id = @id", ("@id", parsedId));
        }
    }
}