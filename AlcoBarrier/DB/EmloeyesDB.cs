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
            // ИЗМЕНЕНО (этап 2): после нового контракта Read (0 строк -> null)
            // проверка res[0] == null больше не нужна: name объявлен NOT NULL,
            // поэтому если строка есть — значение не null.
            string[] res = Read(
                "SELECT name FROM employees WHERE code = @code",
                ("@code", code));
            if (res == null) return null;
            return res[0];
        }

        public string[] GetUserParam(string code)
        {
            // ИЗМЕНЕНО (этап 2): то же упрощение, что и в GetNameCard.
            // iduser/code/hex/CardTemplate — все NOT NULL, проверка res[0] == null
            // после нового контракта Read избыточна.
            string[] res = Read(
                "SELECT iduser, code, hex, CardTemplate FROM employees WHERE code = @code",
                ("@code", code));
            if (res == null) return null;
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