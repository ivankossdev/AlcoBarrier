using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace AlcoBarrier
{
    internal class XmlHandler
    {
        XmlDocument innerage;

        public XmlHandler()
        {
            innerage = new XmlDocument();
        }

        public XmlHandler(string fileName)
        {
            innerage = new XmlDocument();
            string path = $"{Directory.GetCurrentDirectory()}/{fileName}";
            innerage.Load(path);
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

        public List<string> GetUsersString(string xml)
        {
            // ИЗМЕНЕНО (этап 4): try/catch XmlException.
            try
            {
                innerage.LoadXml(xml);
            }
            catch (XmlException)
            {
                return new List<string>();
            }

            XmlNodeList users = innerage.GetElementsByTagName("Rows");

            string Name = string.Empty, Address = string.Empty,
                   CardCode = string.Empty, Id = string.Empty,
                   CardData = string.Empty;

            List<string> Info = new List<string>();

            foreach (XmlElement x in users)
            {
                foreach (XmlNode user in x.ChildNodes)
                {
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
                            }
                        }
                    }

                    Info.Add($"Name-{Name} User ID-{Address} Card code-{CardCode} hex {CardData} id-{Id}");
                    Name = string.Empty; Address = string.Empty; CardCode = string.Empty; CardData = string.Empty;
                }
            }

            return Info;
        }

        public List<string[]> GetUsersArray(string xml)
        {
            List<string[]> Users = new List<string[]>();

            // ИЗМЕНЕНО (этап 4): LoadXml вызывается ДО GetElementsByTagName.
            // Раньше GetElementsByTagName("Rows") вызывался на пустом документе
            // (до LoadXml), из-за чего результат всегда был пустым списком,
            // и employees никогда не наполнялась.
            // Плюс try/catch XmlException.
            try
            {
                innerage.LoadXml(xml);
            }
            catch (XmlException)
            {
                return Users;
            }

            XmlNodeList users = innerage.GetElementsByTagName("Rows");

            string Name = string.Empty, Address = string.Empty,
                   CardCode = string.Empty, Id = string.Empty,
                   CardData = string.Empty, CardTemplate = string.Empty;

            foreach (XmlElement x in users)
            {
                foreach (XmlNode user in x.ChildNodes)
                {
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

                    Name = string.Empty; Address = string.Empty; CardCode = string.Empty; CardData = string.Empty;
                    CardTemplate = string.Empty;
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