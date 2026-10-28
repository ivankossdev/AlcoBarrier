using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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

        async public Task<string> GetSystemInfo(CancellationToken token)
        {
            string MyResult;
            try
            {
                // (F2) ранний выход, если отмена уже запрошена.
                token.ThrowIfCancellationRequested();

                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/SystemInfo", token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = $"{xmlHandler.GetXmlElement(responseBody, "SystemInfo")}";
                }
            }
            // (F2) отмена пробрасывается наверх — MainForm ловит OperationCanceledException.
            // Таймаут HttpClient (TaskCanceledException при token.IsCancellationRequested == false)
            // попадает в следующий catch и превращается в "Message :...".
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }
            return MyResult;
        }

        async public Task<List<string>> GetAllUsers(CancellationToken token)
        {
            string MyResult;
            try
            {
                token.ThrowIfCancellationRequested();

                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True", token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = responseBody;
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }

            return xmlHandler.GetUsersString(MyResult);
        }

        async public Task<List<string[]>> GetDictUsers(CancellationToken token)
        {
            string MyResult;
            try
            {
                // (F2) ранний выход.
                token.ThrowIfCancellationRequested();

                using (HttpResponseMessage response = await client.GetAsync($"http://{IpAddress}/restApi/v2/User/User?FullObject=True", token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = responseBody;
                }
            }
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception e)
            {
                MyResult = $"Message :{e.Message}";
            }

            return xmlHandler.GetUsersArray(MyResult);
        }

        async public Task BlockedUser(bool set, string[] UserData, CancellationToken token)
        {
            string MyResult = string.Empty;

            try
            {
                // (F1) ранний выход — самое важное: при закрытии формы запрос
                // PostAsync больше не будет ждать 10 сек таймаута.
                token.ThrowIfCancellationRequested();

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

                // (F1) CancellationToken прокинут в PostAsync.
                using (var content = new ByteArrayContent(messageToBytes))
                using (HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}/restApi/v2/User/AddOrUpdate", content, token))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    MyResult = $"{xmlHandler.GetXmlElement(responseBody, "CommandProgress")}";
                }
            }
            // (F1) отмена пробрасывается — MainForm ловит OperationCanceledException.
            catch (OperationCanceledException) when (token.IsCancellationRequested)
            {
                throw;
            }
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