using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
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

        async public Task BlockedUser(bool set, string[] UserData)
        {
            string MyResult = string.Empty;

            try
            {
                string command = string.Empty;

                string Active = "Active";

                if (set)
                {
                    Active = "Unused";
                }
                command = $"<User Address=\"{UserData[2]}\">\r\n" +
                          $"<Cards>\r\n" +
                          $"<Card>\r\n<Name>{UserData[0]}</Name>\r\n" +
                          $"<State>{Active}</State>\r\n" +
                          $"<CardType>\r\n<Ref Type=\"CardTemplate\" PartitionID=\"0\" ID=\"{UserData[3]}\" />\r\n" +
                          $"</CardType>\r\n<CardNumber>{UserData[0]}</CardNumber>\r\n<CardNumberNumeric>{UserData[0]}</CardNumberNumeric>\r\n" +
                          $"<IssueNumber>0</IssueNumber>\r\n" +
                          $"<CardData>{UserData[1]}</CardData>\r\n" +
                          $"<ExternalCredentials />\r\n<CloudCredentialType>None</CloudCredentialType>\r\n</Card>\r\n</Cards>\r\n</User>";

                await Console.Out.WriteLineAsync(command);

                byte[] messageToBytes = Encoding.UTF8.GetBytes(command);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}/restApi/v2/User/AddOrUpdate", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                MyResult = $"{xmlHandler.GetXmlElement(responseBody, "CommandProgress")}";

            }
            catch (HttpRequestException e)
            {
                MyResult = $"Message :{e.Message}";
            }
            await Console.Out.WriteLineAsync(MyResult);
        }
    }
}


// Unused
// Active

/*
 <User Address="U2">
  <Cards>
    <Card>
      <Name>37358</Name>
      <State>Active</State>
      <CardType>
        <Ref Type="CardTemplate" PartitionID="0" ID="TM38" />
      </CardType>
      <CardNumber>37358</CardNumber>
      <CardNumberNumeric>37358</CardNumberNumeric>
      <IssueNumber>0</IssueNumber>
      <CardData>1A00000025000000EE910000</CardData>
      <ExternalCredentials />
      <CloudCredentialType>None</CloudCredentialType>
    </Card>
   </Cards>
 </User> 
*/
