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
            // ИЗМЕНЕНО (этап C6-fix): Read при пустом результате возвращает
            // массив из null'ов, а не null. Проверяем res[0] явно.
            string[] res = Read(
                "SELECT name FROM employees WHERE code = @code",
                ("@code", code));
            if (res == null || res[0] == null) return null;
            return res[0];
        }

        public string[] GetUserParam(string code)
        {
            // ИЗМЕНЕНО (этап C6-fix): было `if (res == null) return null;` —
            // не срабатывало, т.к. Read возвращает массив [null, null, ...].
            // Теперь проверяем res[0] == null — это означает "строк нет".
            // Без этой проверки WriteEvent(data) получал массив из null'ов
            // и падал на events.iduser NOT NULL.
            string[] res = Read(
                "SELECT iduser, code, hex, CardTemplate FROM employees WHERE code = @code",
                ("@code", code));
            if (res == null || res[0] == null) return null;
            return res;
        }

        public void WriteRow(string[] emp)
        {
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