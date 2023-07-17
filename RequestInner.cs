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
    class RequestInner
    {
        private HttpClient client;
        private XmlHandler xmlHandler;

        private readonly string IpAddress;

        public RequestInner(string ip)
        {
            IpAddress = ip;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW5zdGFsbGVyOmluc3RhbGxlcg==");
            client.DefaultRequestHeaders.Add("API-KEY", "q5D2I5B/1Xr4ZlEA5yQuDw==");
            xmlHandler = new XmlHandler();
        }

        async public Task<string> GetSystemInfo()
        {
            string MyResult;
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/SystemInfo");

                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                MyResult = $"{xmlHandler.GetXmlElement(responseBody, "SystemInfo")}";

            }
            catch (HttpRequestException e)
            {
                MyResult = $"Message :{e.Message}";
            }
            return MyResult ;
        }

        async public Task<string> SetUserPermission(bool deny)
        {
            /* После обновления меняется UserPermission ID то есть перед изменением доступа запрашивать индефикатор  */
            string userPermission = $"<User Address=\"U2\">\r\n\t<Permissions>\r\n    " +
                                    $"\t<UserPermission ID=\"5e83c06a-84e5-487f-b8c3-33bf34f215a9\">\r\n      " +
                $"\t\t<Deny>{deny}</Deny>\r\n        </UserPermission>\r\n  </Permissions>\r\n</User>";

            string MyResult = string.Empty;

            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(userPermission);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}/restApi/v2/User/AddOrUpdate?IncludeObjectInResult=True", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                MyResult = $"{xmlHandler.GetXmlElement(responseBody, "Card")}";
                
            }
            catch (HttpRequestException e)
            {
                MyResult = $"Message :{e.Message}";
            }
            return MyResult;
        }

        async public Task<string> OpenTheDoor(bool open)
        {
            string MyResult = string.Empty;
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

                HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}/restApi/v2/BasicStatus/XML_Control", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                MyResult = $"{xmlHandler.GetXmlElement(responseBody, "CommandProgress")}";

            }
            catch (HttpRequestException e)
            {
                MyResult = $"Message :{e.Message}";
            }

            return MyResult;
        }

        async public Task<List<string>> GetAllUsers()
        {
            string MyResult;
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                MyResult= responseBody;

            }
            catch (HttpRequestException e)
            {
                MyResult = $"Message :{e.Message}";
            }

            return xmlHandler.GetUsersString(MyResult);
        }

        async public Task<List<string>> GetDictUsers()
        {
            string MyResult;
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                MyResult = responseBody;

            }
            catch (HttpRequestException e)
            {
                MyResult = $"Message :{e.Message}";
            }

            return xmlHandler.GetUsersArray(MyResult);
        }
    }
}
