using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AlcoBarrier
{
    public static class MyJson
    {
        public static string GetLogMessage(string memoryAddr)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = "getLog",
                [ "Position"] = "fromAddr",
                ["MemAddr"] = memoryAddr
            };

            return forecastObject.ToJsonString();
        }

        public static string GetCmdTypeInfMessage(string command)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = command
            };

            return forecastObject.ToJsonString();
        }
    }
}
