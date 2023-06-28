using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    class HttpHandler
    {
        async public Task GETInfo()
        {
            var client = new HttpClient();
            //var result = await client.GetAsync("http://192.168.15.52/cgi-bin/param.cgi?userName=Admin&password=1234&action=get&type=%20deviceInfo");
            // Console.WriteLine(result.StatusCode);
            var result = await client.GetStringAsync("http://192.168.15.52/cgi-bin/param.cgi?userName=Admin&password=1234&action=get&type=%20deviceInfo");
            Console.WriteLine(result);
        }
    }
}
