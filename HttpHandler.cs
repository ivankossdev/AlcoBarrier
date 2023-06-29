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
        }

        public string res { get; private set; } = string.Empty;

        async public Task GetInfo(string ip)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ip}/cgi-bin/param.cgi?userName=Admin&password=1234&action=get&type=%20deviceInfo");

                response.EnsureSuccessStatusCode();
                
                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{responseBody} \n";

            }
            catch (HttpRequestException e)
            {
                res = $"\nException Caught!\nMessage :{e.Message}";
            }
        }
    }
}
