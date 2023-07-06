using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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

        async public Task GetRequest(string request, string firstNode, string nextNode)
        {
            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{ipAddress}:443/cmd", content);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                
                JsonNode forecastNode = JsonNode.Parse(responseBody);
                var options = new JsonSerializerOptions { WriteIndented = true };
                res = forecastNode.ToJsonString(options);
                JsonNode hostname = forecastNode[firstNode][nextNode];
                Console.WriteLine(hostname.ToJsonString().Trim('"'));
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }
    }
}
