using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    internal class Events : BaseSQLite
    {
        public Events(string NameDataBase) : base(NameDataBase)
        {
        }

        public void CreateDB(string Table)
        {
            string SqlCommand = $"CREATE TABLE {Table} " +
                $"(id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, iduser TEXT NOT NULL, code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, cardtamplate TEXT NOT NULL, date TEXT NOT NULL);";
            Write(SqlCommand);
        }

        public void WriteEvent(string[] Table)
        {
            string SqlCommand = $"INSERT INTO Events (iduser, code, hex, cardtamplate, date) " +
                $"VALUES (\"{Table[0]}\", \"{Table[1]}\", \"{Table[2]}\", \"{Table[3]}\", \"{DateTime.Now}\")";
            Write(SqlCommand);
        }
    }
}
