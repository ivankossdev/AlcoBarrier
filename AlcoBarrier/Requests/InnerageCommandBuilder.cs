using System.IO;
using System.Text;
using System.Xml;

namespace AlcoBarrier
{
    /// <summary>
    /// Сборка XML-команд для REST API Innerage.
    /// ДОБАВЛЕНО (P0.3): раньше команда склеивалась из строк, и символы &amp;, &lt;, &quot;
    /// в данных карты ломали XML (или позволяли дописать свои узлы). Теперь значения
    /// экранируются XmlWriter-ом, а структура собирается по элементам.
    /// </summary>
    internal static class InnerageCommandBuilder
    {
        /// <summary>
        /// Команда AddOrUpdate для перевода карты в состояние Active (block = false)
        /// или Unused (block = true).
        /// </summary>
        public static string BuildCardStateCommand(
            bool block,
            string address,
            string cardCode,
            string cardData,
            string cardTemplate)
        {
            string state = block ? "Unused" : "Active";

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                NewLineHandling = NewLineHandling.None,
                CheckCharacters = true,
                CloseOutput = false
            };

            var builder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(builder, settings))
            {
                writer.WriteStartElement("User");
                writer.WriteAttributeString("Address", address ?? string.Empty);

                writer.WriteStartElement("Cards");
                writer.WriteStartElement("Card");

                writer.WriteElementString("Name", cardCode ?? string.Empty);
                writer.WriteElementString("State", state);

                writer.WriteStartElement("CardType");
                writer.WriteStartElement("Ref");
                writer.WriteAttributeString("Type", "CardTemplate");
                writer.WriteAttributeString("PartitionID", "0");
                writer.WriteAttributeString("ID", cardTemplate ?? string.Empty);
                writer.WriteEndElement(); // Ref
                writer.WriteEndElement(); // CardType

                writer.WriteElementString("CardNumber", cardCode ?? string.Empty);
                writer.WriteElementString("CardNumberNumeric", cardCode ?? string.Empty);
                writer.WriteElementString("IssueNumber", "0");
                writer.WriteElementString("CardData", cardData ?? string.Empty);
                writer.WriteStartElement("ExternalCredentials");
                writer.WriteEndElement();
                writer.WriteElementString("CloudCredentialType", "None");

                writer.WriteEndElement(); // Card
                writer.WriteEndElement(); // Cards
                writer.WriteEndElement(); // User
            }

            return builder.ToString();
        }
    }
}
