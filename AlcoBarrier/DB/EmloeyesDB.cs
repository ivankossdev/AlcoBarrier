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
        // ИЗМЕНЕНО (пункт 1): защита от обнуления — null или пустой список не трогает таблицу.
        // DROP TABLE в этом методе выполняется безвозвратно, поэтому пустой набор данных
        // (сбой сети, пустой ответ Innerage) не должен приводить к потере базы.
        // ИЗМЕНЕНО (рефакторинг XmlHandler): на вход приходят типизированные карты,
        // а не позиционный string[6] — перепутать колонки больше нельзя.
        public void ReplaceAll(IReadOnlyList<EmployeeCard> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                Logger.Warning("ReplaceAll: пустой список карт, таблица employees не изменена");
                return;
            }

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

                    foreach (EmployeeCard card in cards)
                    {
                        pName.Value = card.UserName ?? string.Empty;
                        pIdUser.Value = card.UserAddress ?? string.Empty;
                        pCode.Value = card.CardNumber ?? string.Empty;
                        pHex.Value = card.CardData ?? string.Empty;
                        pId.Value = card.CardId ?? string.Empty;
                        pCardTemplate.Value = card.CardTemplate ?? string.Empty;

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

        // ИЗМЕНЕНО (пункт 7): удалён WriteRow(string[] emp) — позиционный массив
        // на 6 элементов, в который легко передать несовместимый набор (например,
        // 4 элемента из GetUserParam) и испортить строку. В приложении employees
        // наполняется только через ReplaceAll.
        //
        // ИЗМЕНЕНО (пункт 7): удалён DeleteTable() — не использовался;
        // полная замена таблицы уже есть в ReplaceAll (в одной транзакции).
    }
}