using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public class HttpRequestAlcoReader
    {
        private HttpClient client;
        private readonly string ipAddress;
        public string res { get; private set; } = string.Empty;

        public HttpRequestAlcoReader(string ip)
        {
            ipAddress = ip;
            client = new HttpClient();
        }

        async public Task GetInfo()
        {
            string request = @"{""cmdType"":""getInf""}";

            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{ipAddress}:443/cmd", content);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                res = responseBody;

            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }
    }
}
