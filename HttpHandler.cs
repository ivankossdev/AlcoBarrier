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

        public HttpHandler() 
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic aW5zdGFsbGVyOmluc3RhbGxlcg==");
            client.DefaultRequestHeaders.Add("API-KEY", "q5D2I5B/1Xr4ZlEA5yQuDw==");
        }

        public string res { get; private set; } = string.Empty;

        // 192.168.0.233 admin:Admin1234!

        /*
         "Accept", "application/xml"
         "Content-Type", "application/xml"
         "Authorization", "Basic aW5zdGFsbGVyOmluc3RhbGxlcg=="
         "API-KEY", "q5D2I5B/1Xr4ZlEA5yQuDw=="
         http://{ip}/restApi/v2/SystemInfo
         http://{ip}/cgi-bin/param.cgi?userName=Admin&password=1234&action=get&type=%20deviceInfo
         */

        async public Task GetInfo(string ip)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ip}/restApi/v2/SystemInfo");

                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                string[] x = responseBody.Split('\n');
                res = $"{responseBody}";
                //3, 5, 6, 8
                
            }
            catch (HttpRequestException e)
            {
                res = $"\nException Caught!\nMessage :{e.Message}";
            }
        }
    }
}
