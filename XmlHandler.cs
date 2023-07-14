using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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

            innerage.LoadXml(xml);

            var cards = innerage.GetElementsByTagName(name);

            foreach (XmlElement x in cards)
            {
                foreach (XmlNode childnode in x.ChildNodes)
                {
                    body += $"{childnode.InnerText.Trim()}&";
                }
            }
            return body;
        }

        public List<string> GetUsers(string xml)
        {
            innerage.LoadXml(xml);

            XmlNodeList users = innerage.GetElementsByTagName("Rows");

            string Name = string.Empty, Address = string.Empty, 
                   CardCode = string.Empty, Id = string.Empty,
                   CardData = string.Empty;

            List<string> Info = new List<string>();

            foreach (XmlElement x in users)
            {
                foreach (XmlNode user in x.ChildNodes)
                {
                    foreach(XmlNode data in user.ChildNodes)
                    {
                       if (data.Name == "Name")
                        {
                            Name = data.InnerText;
                        }
                       if (data.Name == "Address")
                        {
                            Address = data.InnerText;
                        } 
                       foreach(XmlNode cards in data.ChildNodes)
                        {
                           foreach(XmlNode card in cards.ChildNodes)
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
                    // Console.WriteLine($"Name-{Name} User ID-{Address} Card code-{CardCode} hex {CardData} id-{Id}");
                    Info.Add($"Name-{Name} User ID-{Address} Card code-{CardCode} hex {CardData} id-{Id}");
                    Name = string.Empty; Address = string.Empty; CardCode = string.Empty; CardData = string.Empty;
                }
            }

            return Info;
        }
    }
}
