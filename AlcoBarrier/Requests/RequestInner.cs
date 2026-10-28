using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class RequestInner : IDisposable
    {
        private HttpClient client;
        private XmlHandler xmlHandler;

        private readonly string IpAddress;
        private bool _disposed;

        public RequestInner(string ip, string auth, string key)
        {
            IpAddress = ip;
            client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            client.DefaultRequestHeaders.Add("Authorization", auth);
            client.DefaultRequestHeaders.Add("API-KEY", key);
            xmlHandler = new XmlHandler();
        }

        async public Task<string> GetSystemInfo()
        {
            string MyResult;
            try
            {
                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/SystemInfo"))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = $"{xmlHandler.GetXmlElement(responseBody, "SystemInfo")}";
                }
            }
            // ИЗМЕНЕНО (этап C1-fix): HttpRequestException -> Exception.
            // Раньше TaskCanceledException (таймаут HttpClient) улетал наверх.
            catch (Exception e)
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
                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True"))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = responseBody;
                }
            }
            // ИЗМЕНЕНО (этап C1-fix): HttpRequestException -> Exception.
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }

            return xmlHandler.GetUsersString(MyResult);
        }

        async public Task<List<string[]>> GetDictUsers()
        {
            string MyResult;
            try
            {
                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True"))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = responseBody;
                }
            }
            // ИЗМЕНЕНО (этап C1-fix): HttpRequestException -> Exception.
            catch (Exception e)
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
                command = $"<User Address=\"{UserData[3]}\">\r\n" +
                          $"<Cards>\r\n" +
                          $"<Card>\r\n<Name>{UserData[1]}</Name>\r\n" +
                          $"<State>{Active}</State>\r\n" +
                          $"<CardType>\r\n<Ref Type=\"CardTemplate\" PartitionID=\"0\" ID=\"{UserData[4]}\" />\r\n" +
                          $"</CardType>\r\n<CardNumber>{UserData[1]}</CardNumber>\r\n<CardNumberNumeric>{UserData[1]}</CardNumberNumeric>\r\n" +
                          $"<IssueNumber>0</IssueNumber>\r\n" +
                          $"<CardData>{UserData[2]}</CardData>\r\n" +
                          $"<ExternalCredentials />\r\n<CloudCredentialType>None</CloudCredentialType>\r\n</Card>\r\n</Cards>\r\n</User>\r\n";

                byte[] messageToBytes = Encoding.UTF8.GetBytes(command);

                using (var content = new ByteArrayContent(messageToBytes))
                using (HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}/restApi/v2/User/AddOrUpdate", content))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = $"{xmlHandler.GetXmlElement(responseBody, "CommandProgress")}";
                }
            }
            // ИЗМЕНЕНО (этап C1-fix): HttpRequestException -> Exception.
            // Это самое важное место: раньше TaskCanceledException здесь убивал
            // цикл OnlineMessageAsync (catch — вне while) и опрос прекращался навсегда.
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }

            await Console.Out.WriteLineAsync(MyResult);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            client.Dispose();
        }
    }
}