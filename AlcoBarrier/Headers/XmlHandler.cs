using System;
using System.Collections.Generic;
using System.Xml;

namespace AlcoBarrier
{
    internal class XmlHandler
    {
        XmlDocument innerage;

        public XmlHandler()
        {
            innerage = new XmlDocument();
        }

        public string GetXmlElement(string xml, string name)
        {
            string body = string.Empty;

            // ИЗМЕНЕНО (этап 4): try/catch XmlException — при невалидном XML
            // (например, текст ошибки от Innerage) возвращаем пустую строку,
            // а не роняем наружу.
            try
            {
                innerage.LoadXml(xml);
            }
            catch (XmlException)
            {
                return body;
            }

            var cards = innerage.GetElementsByTagName(name);

            foreach (XmlElement elemnt in cards)
            {
                // ИЗМЕНЕНО (этап 4): защита от ChildNodes.Count < 2.
                // Раньше при меньшем числе узлов было ArgumentOutOfRangeException на [x].
                if (elemnt.ChildNodes.Count < 2) continue;

                for (int x = 0; x < 2; x++)
                {
                    body += $"{elemnt.ChildNodes[x].InnerText} ";
                }
            }
            return body;
        }

        /// <summary>
        /// Возвращает список сотрудников или null, если ответ Innerage не удалось разобрать
        /// (пустой ответ, текст/HTML ошибки вместо XML и т.п.).
        /// Пустой список означает корректно разобранный ответ, в котором сотрудников нет.
        /// </summary>
        public List<string[]> GetUsersArray(string xml)
        {
            List<string[]> Users = new List<string[]>();

            // ИЗМЕНЕНО (этап 4): LoadXml вызывается ДО GetElementsByTagName.
            // Раньше GetElementsByTagName("Rows") вызывался на пустом документе
            // (до LoadXml), из-за чего результат всегда был пустым списком,
            // и employees никогда не наполнялась.
            // Плюс try/catch XmlException.
            //
            // ИЗМЕНЕНО (пункт 1): неразобранный ответ возвращает null, а не пустой список.
            // Иначе сетевая ошибка или 401 от Innerage («Message :...») выглядели бы
            // как валидный ответ без сотрудников, и employees можно было бы обнулить.
            if (string.IsNullOrWhiteSpace(xml))
                return null;

            try
            {
                innerage.LoadXml(xml);
            }
            catch (XmlException)
            {
                return null;
            }

            XmlNodeList users = innerage.GetElementsByTagName("Rows");

            foreach (XmlElement x in users)
            {
                foreach (XmlNode user in x.ChildNodes)
                {
                    // ИЗМЕНЕНО (пункт 6): переменные объявляются внутри итерации пользователя,
                    // поэтому данные предыдущего пользователя не могут перейти в следующего.
                    // Раньше они объявлялись один раз снаружи, а в блоке очистки забывали Id —
                    // и при отсутствии тега <ID> в карту попадал идентификатор предыдущей карты.
                    string Name = string.Empty, Address = string.Empty,
                           CardCode = string.Empty, Id = string.Empty,
                           CardData = string.Empty, CardTemplate = string.Empty;

                    foreach (XmlNode data in user.ChildNodes)
                    {
                        if (data.Name == "Name")
                        {
                            Name = data.InnerText;
                        }
                        if (data.Name == "Address")
                        {
                            Address = data.InnerText;
                        }
                        foreach (XmlNode cards in data.ChildNodes)
                        {
                            foreach (XmlNode card in cards.ChildNodes)
                            {
                                if (card.Name == "Name")
                                {
                                    CardCode = card.InnerText;
                                }
                                if (card.Name == "ID")
                                {
                                    Id = card.InnerText;
                                }
                                if (card.Name == "CardData")
                                {
                                    CardData = card.InnerText;
                                }
                                foreach (XmlNode RefType in card.ChildNodes)
                                {
                                    if (RefType.Name == "Ref")
                                    {
                                        // ИЗМЕНЕНО (этап 4): null-safe обращение к атрибутам —
                                        // при отсутствии "Type"/"ID" больше не будет NRE.
                                        var typeAttr = RefType.Attributes?.GetNamedItem("Type");
                                        var idAttr = RefType.Attributes?.GetNamedItem("ID");
                                        if (typeAttr != null && typeAttr.Value == "CardTemplate" && idAttr != null)
                                        {
                                            CardTemplate = idAttr.Value;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Users.Add(CreateArray(Name, Address, CardCode, CardData, Id, CardTemplate));
                }
            }
            return Users;
        }

        private string[] CreateArray(string Name, string Address, string CardCode, string CardData, string Id, string CardTemplate)
        {
            string[] strings = {
                Name,
                Address,
                CardCode,
                CardData,
                Id,
                CardTemplate
            };
            return strings;
        }
    }
}