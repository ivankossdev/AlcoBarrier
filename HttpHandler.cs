using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlcoBarrier
{
    class HttpHandler
    {
        private HttpClient client;

        private readonly string ipAddress = string.Empty;

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
                res = $"{responseBody}\n\n";
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        }

        async public Task GetDoors()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{ipAddress}/restApi/v2/User/Door");

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                res = $"{responseBody}";
            }
            catch (HttpRequestException e)
            {
                res = $"Message :{e.Message}";
            }
        } 

        async public Task UserPermission(bool deny)
        {
            string userPermission = $"<User Address=\"U2\">\r\n\t<Permissions>\r\n    " +
                $"\t<UserPermission ID=\"300ef1f9-f296-4814-abfc-9b8b016e7c3b\">\r\n      " +
                $"\t\t<Deny>{deny}</Deny>\r\n        </UserPermission>\r\n  </Permissions>\r\n</User>";

            try
            {
                byte[] messageToBytes = System.Text.Encoding.UTF8.GetBytes(userPermission);
                var content = new ByteArrayContent(messageToBytes);

                HttpResponseMessage response = await client.PostAsync($"http://{ipAddress}/restApi/v2/User/AddOrUpdate?IncludeObjectInResult=True", content);

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
