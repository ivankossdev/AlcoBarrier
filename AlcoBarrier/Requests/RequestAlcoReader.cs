using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    // ИЗМЕНЕНО (этап C1): реализует IDisposable, HttpClient с таймаутом,
    // HttpResponseMessage в using. Убран неиспользуемый using Newtonsoft.Json.
    public class RequestAlcoReader : IDisposable
    {
        private readonly HttpClient client;
        private readonly string IpAddress;
        private bool _disposed;

        public RequestAlcoReader(string ip)
        {
            IpAddress = ip;
            client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        async public Task<string> GetRequestCmd(string request)
        {
            string responseBody;
            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(request);

                using (var content = new ByteArrayContent(messageToBytes))
                using (HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}:443/cmd", content))
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                responseBody = $"Message :{e.Message}";
            }


            // Временный вывод сообщения в файл
            Logger.Info($"AlcoReader raw response: {responseBody}");

            return GetNode(responseBody);
        }

        protected string GetNode(string responseBody)
        {
            try
            {
                JsonNode forecastNode = JsonNode.Parse(responseBody);
                var options = new JsonSerializerOptions { WriteIndented = true };
                return forecastNode.ToJsonString(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return responseBody;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            client.Dispose();
        }
    }
}