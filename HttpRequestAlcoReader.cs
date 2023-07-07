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
        public string Res { get; private set; } = string.Empty;
        public string CountRecords { get; private set; } = string.Empty;
        public string LastRecord { get; private set; } = string.Empty;

        public HttpRequestAlcoReader(string ip)
        {
            ipAddress = ip;
            client = new HttpClient();
        }

        async public Task GetRequestCmd(string request, string cmd)
        {
            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{ipAddress}:443/cmd", content);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                switch (cmd)
                {
                    case "getInf":
                        Res = GetNode(responseBody); 
                        break;
                    case "getLogInf":
                        CountRecords = GetNode(responseBody);
                        break;
                    case "getLog":
                        LastRecord = GetNode(responseBody);
                        break;
                    default:
                        Res = "Not information!!!";
                        break;
                }


            }
            catch (HttpRequestException e)
            {
                Res = $"Message :{e.Message}";
            }
        }

        private string GetNode(string responseBody)
        {
            JsonNode forecastNode = JsonNode.Parse(responseBody);
            var options = new JsonSerializerOptions { WriteIndented = true };
            return forecastNode.ToJsonString(options);
        }

    }
}
