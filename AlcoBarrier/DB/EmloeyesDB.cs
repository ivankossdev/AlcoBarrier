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
        public EmloeyesDB(string NameDataBase) : base(NameDataBase) { }

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
            // (этап 2) параметризованный LIKE; (этап 3) порядок колонок здесь не важен.
            string[] res = Read(
                "SELECT name FROM employees WHERE code LIKE @code",
                ("@code", "%" + code + "%"));
            return res[0];
        }

        public string[] GetUserParam(string code)
        {
            // ИЗМЕНЕНО (этап 3): порядок колонок приведён к порядку записи в events
            // (iduser, code, hex, CardTemplate).
            // Раньше было (code, hex, iduser, CardTemplate), из-за чего EventsDB.WriteEvent
            // писал значения не в те колонки, а RequestInner.BlockedUser читал их по неверным индексам.
            string[] res = Read(
                "SELECT iduser, code, hex, CardTemplate FROM employees WHERE code LIKE @code",
                ("@code", "%" + code + "%"));
            return res;
        }
        public void WriteRow(string[] emp)
        {
            // (этап 2) параметризованный INSERT.
            Write(
                "INSERT INTO employees (name, iduser, code, hex, id, CardTemplate) " +
                "VALUES (@name, @iduser, @code, @hex, @id, @cardTemplate)",
                ("@name", emp[0]),
                ("@iduser", emp[1]),
                ("@code", emp[2]),
                ("@hex", emp[3]),
                ("@id", emp[4]),
                ("@cardTemplate", emp[5]));
        }

        public void DeleteTable()
        {
            Write("DROP TABLE IF EXISTS employees");
        }
    }
}