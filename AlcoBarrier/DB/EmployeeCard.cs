namespace AlcoBarrier
{
    /// <summary>
    /// Одна карта сотрудника, полученная из Innerage.
    /// ДОБАВЛЕНО (рефакторинг XmlHandler): типизированная замена позиционному string[6].
    /// </summary>
    public sealed class EmployeeCard
    {
        /// <summary>Имя пользователя (тег Name на уровне User).</summary>
        public string UserName { get; set; }

        /// <summary>Адрес/идентификатор пользователя (тег Address) — колонка employees.iduser.</summary>
        public string UserAddress { get; set; }

        /// <summary>Номер карты (тег Name на уровне Card) — employees.code.</summary>
        public string CardNumber { get; set; }

        /// <summary>Данные карты (тег CardData) — employees.hex.</summary>
        public string CardData { get; set; }

        /// <summary>Идентификатор карты (тег ID) — employees.id.</summary>
        public string CardId { get; set; }

        /// <summary>Шаблон карты (Ref Type="CardTemplate") — employees.CardTemplate.</summary>
        public string CardTemplate { get; set; }
    }
}
