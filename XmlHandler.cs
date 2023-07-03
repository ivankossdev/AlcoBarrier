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

        public string PrintXmlData(string xml)
        {

            string body = string.Empty;

            innerage.LoadXml(xml);

            XmlElement node = innerage.DocumentElement;

            if (node != null)
            {
                foreach (XmlElement xnode in node)
                {
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        body += $"{childnode.InnerText} \n";
                    }
                }
            }

            return body;
        }
        //var cards = innerage.GetElementsByTagName("Cards");
        //foreach (XmlElement x in cards)
        //{

        //    foreach (XmlNode childnode in x.ChildNodes)
        //    {
        //        Console.WriteLine(childnode.InnerXml);
        //    }
        //}
        //Console.WriteLine(xml);
    }
}
