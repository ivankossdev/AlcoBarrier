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
        public void CreateDB()
        {
            SqlCommand = $"CREATE TABLE events " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, iduser TEXT NOT NULL, code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, cardtamplate TEXT NOT NULL, date TEXT NOT NULL, blockdate TEXT NOT NULL);";
            Write(SqlCommand);
        }

        public void WriteEvent(string[] Table, DateTime BlockTime)
        {
            DateTime dateTime = DateTime.Now;
            SqlCommand = $"INSERT INTO events (iduser, code, hex, cardtamplate, date, blockdate) " +
                $"VALUES (\"{Table[0]}\", \"{Table[1]}\", \"{Table[2]}\", \"{Table[3]}\", \"{dateTime}\", \"{BlockTime}\")";
            Write(SqlCommand);
        }

        public List<string[]> ReadEventList()
        {
            SqlCommand = "SELECT * FROM events";
            return ReadListArray(SqlCommand);
        }

        public void DeleteString(string id) 
        {
            SqlCommand = $"DELETE FROM events WHERE id = {Int32.Parse(id)}";
            Write(SqlCommand);
        }
    }
}
