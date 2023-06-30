using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    class HttpHandler
    {
        private HttpClient client;

        private string ipAddress = string.Empty;

        public HttpHandler(string ip) 
        {
            ipAddress = ip;
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW5zdGFsbGVyOmluc3RhbGxlcg==");
            client.DefaultRequestHeaders.Add("API-KEY", "q5D2I5B/1Xr4ZlEA5yQuDw==");
        }

        public string res { get; private set; } = string.Empty;

        async public Task GetInfo()
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ipAddress}/restApi/v2/SystemInfo");

                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{responseBody}";
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }
    }
}
