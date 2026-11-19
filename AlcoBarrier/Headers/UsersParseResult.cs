using System.Collections.Generic;

namespace AlcoBarrier
{
    /// <summary>
    /// Результат разбора ответа Innerage со списком сотрудников.
    /// ДОБАВЛЕНО (рефакторинг XmlHandler): позволяет отличить «сотрудников нет»
    /// от «ответ не разобран» и «структура не та», не теряя контракт пункта 1
    /// (при ошибке база employees не перезаписывается).
    /// </summary>
    public sealed class UsersParseResult
    {
        /// <summary>true, если ответ является корректным XML и был разобран.</summary>
        public bool XmlParsed { get; set; }

        /// <summary>true, если в ответе найден ожидаемый элемент Rows.</summary>
        public bool StructureFound { get; set; }

        /// <summary>Список карт: одна запись на карту.</summary>
        public IReadOnlyList<EmployeeCard> Cards { get; set; } = new List<EmployeeCard>();

        /// <summary>Пояснение к неполному результату (пропуски, дубли, отсутствие карт).</summary>
        public string Warning { get; set; }

        /// <summary>Причина, по которой XML не разобран.</summary>
        public string Error { get; set; }
    }
}
