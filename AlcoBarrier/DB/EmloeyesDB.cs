using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace AlcoBarrier
{
    public class EmloeyesDB : BaseSQLite
    {
        public EmloeyesDB(string NameDataBase) : base(NameDataBase) { }

        public override string CreateDB()
        {
            // ИЗМЕНЕНО (этап 3): IF NOT EXISTS позволяет вызывать метод при каждом старте без ошибок.
            string SqlCommand = $"CREATE TABLE IF NOT EXISTS employees " +
                $"(idkey INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                $"name TEXT NOT NULL, " +
                $"iduser TEXT NOT NULL, " +
                $"code TEXT NOT NULL, " +
                $"hex TEXT NOT NULL, " +
                $"id TEXT NOT NULL, " +
                $"CardTemplate TEXT NOT NULL);";
            Write(SqlCommand);

            // ДОБАВЛЕНО (этап 3): индекс для быстрого поиска карты по коду.
            Write("CREATE INDEX IF NOT EXISTS idx_employees_code ON employees(code);");

            return $"База данных {NameDataBase} создана \n";
        }

        // ДОБАВЛЕНО (этап 3): атомарная замена всей таблицы сотрудников.
        public void ReplaceAll(List<string[]> users)
        {
            ExecuteInTransaction((connection) =>
            {
                using (var cmd = connection.CreateCommand())
                {
                    // 1. Удаляем старую таблицу
                    cmd.CommandText = "DROP TABLE IF EXISTS employees;";
                    cmd.ExecuteNonQuery();

                    // 2. Создаем новую
                    cmd.CommandText = $"CREATE TABLE employees " +
                        $"(idkey INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                        $"name TEXT NOT NULL, iduser TEXT NOT NULL, code TEXT NOT NULL, " +
                        $"hex TEXT NOT NULL, id TEXT NOT NULL, CardTemplate TEXT NOT NULL);";
                    cmd.ExecuteNonQuery();

                    // 3. Создаем индекс
                    cmd.CommandText = "CREATE INDEX idx_employees_code ON employees(code);";
                    cmd.ExecuteNonQuery();

                    // 4. Вставляем данные пачкой
                    cmd.CommandText = "INSERT INTO employees (name, iduser, code, hex, id, CardTemplate) " +
                                      "VALUES (@name, @iduser, @code, @hex, @id, @cardTemplate)";

                    var pName = cmd.Parameters.Add("@name", SqliteType.Text);
                    var pIdUser = cmd.Parameters.Add("@iduser", SqliteType.Text);
                    var pCode = cmd.Parameters.Add("@code", SqliteType.Text);
                    var pHex = cmd.Parameters.Add("@hex", SqliteType.Text);
                    var pId = cmd.Parameters.Add("@id", SqliteType.Text);
                    var pCardTemplate = cmd.Parameters.Add("@cardTemplate", SqliteType.Text);

                    foreach (var emp in users)
                    {
                        pName.Value = emp[0] ?? (object)DBNull.Value;
                        pIdUser.Value = emp[1] ?? (object)DBNull.Value;
                        pCode.Value = emp[2] ?? (object)DBNull.Value;
                        pHex.Value = emp[3] ?? (object)DBNull.Value;
                        pId.Value = emp[4] ?? (object)DBNull.Value;
                        pCardTemplate.Value = emp[5] ?? (object)DBNull.Value;

                        cmd.ExecuteNonQuery();
                    }
                }
            });
        }

        public string GetNameCard(string code)
        {
            // ИЗМЕНЕНО (этап 2): после нового контракта Read (0 строк -> null)
            string[] res = Read(
                "SELECT name FROM employees WHERE code = @code",
                ("@code", code));
            if (res == null) return null;
            return res[0];
        }

        public string[] GetUserParam(string code)
        {
            // ИЗМЕНЕНО (этап 2)
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