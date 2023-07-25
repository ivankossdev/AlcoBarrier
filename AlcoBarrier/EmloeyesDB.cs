using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class EmloeyesDB : BaseSQLite
    {
        public EmloeyesDB(string NameDataBase) : base(NameDataBase) {}
        
        public void CreateDB(string Table)
        {
            string SqlCommand = $"CREATE TABLE {Table} " +
                $"(idkey INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"name TEXT NOT NULL, " +
                $"iduser TEXT NOT NULL, " +
                $"code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, " +
                $"id TEXT NOT NULL, " +
                $"CardTemplate TEXT NOT NULL);";
            Write(SqlCommand);
        }

        public string GetNameCard(string code)
        {
            string[] res = Read($"SELECT name FROM user WHERE code LIKE \"%{code}%\"");
            return res[0];
        }

        public string[] GetUserParam(string code)
        {
            string[] res = Read($"SELECT code, hex, iduser, CardTemplate  FROM user WHERE code LIKE \"%{code}%\"");
            return res;
        }
    }
}
