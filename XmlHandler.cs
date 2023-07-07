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

            innerage.LoadXml(xml);
            var cards = innerage.GetElementsByTagName(name);
            foreach (XmlElement x in cards)
            {
                foreach (XmlNode childnode in x.ChildNodes)
                {
                    body += $"{childnode.Name} {childnode.InnerText.Trim()}&";
                }
            }
            return body;
        }
    }
}
