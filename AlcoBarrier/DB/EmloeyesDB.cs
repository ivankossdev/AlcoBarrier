using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class EmloeyesDB : BaseSQLite
    {
        public EmloeyesDB(string NameDataBase) : base(NameDataBase) {  }
        
        public override string CreateDB()
        {
            string SqlCommand = $"CREATE TABLE employees " +
                $"(idkey INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"name TEXT NOT NULL, " +
                $"iduser TEXT NOT NULL, " +
                $"code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, " +
                $"id TEXT NOT NULL, " +
                $"CardTemplate TEXT NOT NULL);";
            Write(SqlCommand);

            return $"База данных {NameDataBase} создана \n";
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
