using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace AlcoBarrier
{
    class HttpHandler
    {
        private HttpClient client;

        private readonly string ipAddress;

        public HttpHandler(string ip) 
        {
            ipAddress = ip;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW5zdGFsbGVyOmluc3RhbGxlcg==");
            client.DefaultRequestHeaders.Add("API-KEY", "q5D2I5B/1Xr4ZlEA5yQuDw==");
        }

        public string res { get; private set; } = string.Empty;

        async public Task GetInfo()
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ipAddress}/restApi/v2/SystemInfo");

                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{MyXml(responseBody)}";
                
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }

        async public Task GetDoors()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ipAddress}/restApi/v2/User/Door");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{responseBody}";
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        } 

        async public Task UserPermission(bool deny)
        {
            string userPermission = $"<User Address=\"U2\">\r\n\t<Permissions>\r\n    " +
                $"\t<UserPermission ID=\"300ef1f9-f296-4814-abfc-9b8b016e7c3b\">\r\n      " +
                $"\t\t<Deny>{deny}</Deny>\r\n        </UserPermission>\r\n  </Permissions>\r\n</User>";

            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(userPermission);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{ipAddress}/restApi/v2/User/AddOrUpdate?IncludeObjectInResult=True", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{MyXml(responseBody)}";
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }

        private string MyXml(string xml)
        {
            XmlDocument innerage = new XmlDocument();
            //string path = $"{Directory.GetCurrentDirectory()}/Book.xml";
            //doc.Load(path);

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

            var cards = innerage.GetElementsByTagName("Cards");
            foreach (XmlElement x in cards)
            {
                
                foreach (XmlNode childnode in x.ChildNodes)
                {
                    Console.WriteLine(childnode.InnerXml);
                }
            }
            //Console.WriteLine(xml);
            return body;
        }
    }
}
