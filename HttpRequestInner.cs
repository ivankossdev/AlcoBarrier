using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
    class HttpRequestInner
    {
        private HttpClient client;
        private XmlHandler xmlHandler;

        private readonly string ipAddress;

        public HttpRequestInner(string ip) 
        {
            ipAddress = ip;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW5zdGFsbGVyOmluc3RhbGxlcg==");
            client.DefaultRequestHeaders.Add("API-KEY", "q5D2I5B/1Xr4ZlEA5yQuDw==");
            xmlHandler = new XmlHandler(); 
        }

        public string res { get; private set; } = string.Empty;

        async public Task GetSystemInfo()
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ipAddress}/restApi/v2/SystemInfo");

                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{xmlHandler.GetXmlElement(responseBody, "SystemInfo")}";

            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }

        async public Task SetUserPermission(bool deny)
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
                res = $"{xmlHandler.GetXmlElement(responseBody, "Card")}";
                
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }

        async public Task OpenTheDoor(bool open)
        {
            try
            {
                string command = string.Empty;

                if (open)
                {
                    command = $"<DoorAction ID=\"35b753bd-b7ea-4630-b29a-2efc2478714e\">\r\n  " +
                        $"<ID>35b753bd-b7ea-4630-b29a-2efc2478714e</ID>\r\n  " +
                        $"<OnAssert>2</OnAssert>\r\n  " +
                        $"<OnDeAssert>1</OnDeAssert>\r\n  " +
                        $"<InvertQualifier>False</InvertQualifier>\r\n  " +
                        $"<WaitUntilComplete>False</WaitUntilComplete>\r\n  " +
                        $"<Entity>\r\n    " +
                        $"<Ref Type=\"Door\" ID=\"5072171692982273\" />\r\n  " +
                        $"</Entity>\r\n  " +
                        $"<UnlockTimeTicks>0</UnlockTimeTicks>\r\n  " +
                        $"<Genre>0</Genre>\r\n  " +
                        $"<DisarmAreas>False</DisarmAreas>\r\n  " +
                        $"<IgnoreInterlocks>False</IgnoreInterlocks>\r\n" +
                        $"</DoorAction>";
                }
                else
                {
                    command = $"<DoorAction ID=\"35b753bd-b7ea-4630-b29a-2efc2478714e\">\r\n  " +
                        $"<ID>35b753bd-b7ea-4630-b29a-2efc2478714e</ID>\r\n  " +
                        $"<OnAssert>1</OnAssert>\r\n  " +
                        $"<OnDeAssert>2</OnDeAssert>\r\n  " +
                        $"<InvertQualifier>False</InvertQualifier>\r\n  " +
                        $"<WaitUntilComplete>False</WaitUntilComplete>\r\n  " +
                        $"<Entity>\r\n    " +
                        $"<Ref Type=\"Door\" ID=\"5072171692982273\" />\r\n  " +
                        $"</Entity>\r\n  " +
                        $"<UnlockTimeTicks>0</UnlockTimeTicks>\r\n  " +
                        $"<Genre>0</Genre>\r\n  " +
                        $"<DisarmAreas>False</DisarmAreas>\r\n  " +
                        $"<IgnoreInterlocks>False</IgnoreInterlocks>\r\n" +
                        $"</DoorAction>";
                }
                byte[] messageToBytes = Encoding.UTF8.GetBytes(command);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{ipAddress}/restApi/v2/BasicStatus/XML_Control", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{xmlHandler.GetXmlElement(responseBody, "CommandProgress")}";

            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }
    }
}
