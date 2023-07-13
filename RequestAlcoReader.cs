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
    public class RequestAlcoReader
    {
        private readonly HttpClient client;
        private readonly string IpAddress;

        public RequestAlcoReader(string ip)
        {
            IpAddress = ip;
            client = new HttpClient();
        }

        async public Task<string> GetRequestCmd(string request)
        {
            string responseBody;
            try
            {
                byte[] messageToBytes = Encoding.UTF8.GetBytes(request);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{IpAddress}:443/cmd", content);

                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                responseBody = $"Message :{e.Message}";
            }

            return GetNode(responseBody);
        }

        private string GetNode(string responseBody)
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

    }
}
