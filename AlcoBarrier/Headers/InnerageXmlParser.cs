using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AlcoBarrier
{
    /// <summary>
    /// Разбор XML-ответов Innerage.
    /// ДОБАВЛЕНО (рефакторинг XmlHandler): класс без состояния — каждый вызов работает
    /// со своим XmlDocument, поэтому его безопасно вызывать из фоновых потоков.
    ///
    /// Безопасность: DTD запрещены, внешние сущности не разрешаются, размер документа
    /// ограничен. Раньше XmlDocument.LoadXml позволял XXE (чтение file:///…)
    /// и раскрытие entity-бомб.
    /// </summary>
    internal static class InnerageXmlParser
    {
        private const long MaxDocumentCharacters = 20L * 1024 * 1024;

        private static readonly XmlReaderSettings SafeSettings = new XmlReaderSettings
        {
            DtdProcessing = DtdProcessing.Prohibit,
            XmlResolver = null,
            MaxCharactersInDocument = MaxDocumentCharacters,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            CheckCharacters = true,
            CloseInput = true
        };

        /// <summary>
        /// Текст первого элемента с указанным локальным именем (без namespace-префикса).
        /// Возвращает пустую строку, если ответ не разобран или узел не найден.
        /// </summary>
        public static string GetElementText(string xml, string elementName)
        {
            if (string.IsNullOrWhiteSpace(elementName))
                return string.Empty;

            if (!TryLoad(xml, out XmlDocument document, out string error))
            {
                Logger.Warning($"Innerage XML ({elementName}): {error}");
                return string.Empty;
            }

            XmlElement element = FindFirstElement(document, elementName);
            if (element == null)
                return string.Empty;

            // Склеиваем текст вложенных элементов через пробел (как раньше, но без
            // хвостового пробела и с обрезкой). Если вложенных элементов нет —
            // отдаём собственный текст узла.
            var parts = new List<string>();
            foreach (XmlNode child in element.ChildNodes)
            {
                if (child is XmlElement childElement)
                {
                    string text = childElement.InnerText.Trim();
                    if (text.Length > 0)
                        parts.Add(text);
                }
            }

            if (parts.Count == 0)
                return element.InnerText.Trim();

            return string.Join(" ", parts);
        }

        /// <summary>
        /// Разбирает ответ Innerage в список карт: одна запись на карту.
        /// Пользователи без карт пропускаются, значения обрезаются, комментарии
        /// и служебные узлы игнорируются.
        /// </summary>
        public static UsersParseResult ParseUserCards(string xml)
        {
            var result = new UsersParseResult();

            if (!TryLoad(xml, out XmlDocument document, out string error))
            {
                result.Error = error;
                return result;
            }

            result.XmlParsed = true;

            XmlElement rows = FindFirstElement(document, "Rows");
            if (rows == null)
            {
                result.Warning = "в ответе нет элемента Rows";
                return result;
            }

            result.StructureFound = true;

            var cards = new List<EmployeeCard>();
            var seenCardNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int usersSeen = 0;
            int cardsWithoutNumber = 0;
            int duplicateCards = 0;

            foreach (XmlNode userNode in rows.ChildNodes)
            {
                if (!(userNode is XmlElement user))
                    continue;

                usersSeen++;

                string userName = GetChildText(user, "Name");
                string userAddress = GetChildText(user, "Address");

                foreach (XmlElement cardElement in FindDescendants(user, "Card"))
                {
                    string cardNumber = GetChildText(cardElement, "Name");
                    if (cardNumber.Length == 0)
                    {
                        cardsWithoutNumber++;
                        continue;
                    }

                    if (!seenCardNumbers.Add(cardNumber))
                    {
                        duplicateCards++;
                        continue;
                    }

                    cards.Add(new EmployeeCard
                    {
                        UserName = userName,
                        UserAddress = userAddress,
                        CardNumber = cardNumber,
                        CardData = GetChildText(cardElement, "CardData"),
                        CardId = GetChildText(cardElement, "ID"),
                        CardTemplate = GetCardTemplate(cardElement)
                    });
                }
            }

            result.Cards = cards;

            if (cards.Count == 0)
            {
                result.Warning = "Rows найден, но карт не найдено";
            }
            else
            {
                var notes = new List<string>();
                if (cardsWithoutNumber > 0)
                    notes.Add($"карт без номера: {cardsWithoutNumber}");
                if (duplicateCards > 0)
                    notes.Add($"дублей номера карты: {duplicateCards}");

                if (notes.Count > 0)
                    result.Warning = string.Join(", ", notes) + $"; пользователей в ответе: {usersSeen}";
            }

            return result;
        }

        private static string GetCardTemplate(XmlElement cardElement)
        {
            // Ref ищется на любой глубине внутри Card: и <CardType><Ref/></CardType>,
            // и <Ref/> прямо в Card.
            foreach (XmlElement refElement in FindDescendants(cardElement, "Ref"))
            {
                string type = GetAttribute(refElement, "Type");
                if (!string.Equals(type, "CardTemplate", StringComparison.OrdinalIgnoreCase))
                    continue;

                string id = GetAttribute(refElement, "ID");
                if (id.Length > 0)
                    return id;
            }

            return string.Empty;
        }

        private static string GetChildText(XmlElement parent, string localName)
        {
            foreach (XmlNode child in parent.ChildNodes)
            {
                if (child is XmlElement element && NameEquals(element, localName))
                    return element.InnerText.Trim();
            }

            return string.Empty;
        }

        private static string GetAttribute(XmlElement element, string localName)
        {
            if (element.Attributes == null)
                return string.Empty;

            foreach (XmlAttribute attribute in element.Attributes)
            {
                if (string.Equals(attribute.LocalName, localName, StringComparison.OrdinalIgnoreCase))
                    return (attribute.Value ?? string.Empty).Trim();
            }

            return string.Empty;
        }

        private static IEnumerable<XmlElement> FindDescendants(XmlElement root, string localName)
        {
            foreach (XmlNode node in root.GetElementsByTagName("*"))
            {
                if (node is XmlElement element && NameEquals(element, localName))
                    yield return element;
            }
        }

        private static XmlElement FindFirstElement(XmlDocument document, string localName)
        {
            foreach (XmlNode node in document.GetElementsByTagName("*"))
            {
                if (node is XmlElement element && NameEquals(element, localName))
                    return element;
            }

            return null;
        }

        private static bool NameEquals(XmlElement element, string localName)
        {
            return string.Equals(element.LocalName, localName, StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryLoad(string xml, out XmlDocument document, out string error)
        {
            document = null;
            error = null;

            if (string.IsNullOrWhiteSpace(xml))
            {
                error = "пустой ответ";
                return false;
            }

            try
            {
                using (var stringReader = new StringReader(xml))
                using (var reader = XmlReader.Create(stringReader, SafeSettings))
                {
                    var loaded = new XmlDocument { XmlResolver = null, PreserveWhitespace = false };
                    loaded.Load(reader);
                    document = loaded;
                    return true;
                }
            }
            catch (XmlException ex)
            {
                error = "некорректный XML: " + ex.Message;
                return false;
            }
        }
    }
}
