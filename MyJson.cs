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
        public static string CreateLogMessage(string memoryAddr)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = "getLog",
                [ "Position"] = "fromAddr",
                ["MemAddr"] = memoryAddr
            };

            return forecastObject.ToJsonString();
        }

        public static string CreateCmdTypeInfMessage(string command)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = command
            };

            return forecastObject.ToJsonString();
        }

        public static string GetCountMessage(string jsonString)
        {
            JsonNode jsonNode = JsonNode.Parse(jsonString);
            return jsonNode["LastRecord"]["MemAddr"].ToString();
        }

        public static string GetDataMemory(string jsonString)
        {
            JsonNode jsonNode = JsonNode.Parse(jsonString);

            return jsonNode["Records"][0].ToString();
        }

        public static string GetStringResult(string jsonString)
        {
            JsonNode jsonNode = JsonNode.Parse(jsonString);
            string Code = jsonNode["Records"][0]["Code"].ToString();
            string Message = string.Empty;

            if (Code == "4" || Code == "5")
            {
                Message = $"{jsonNode["Records"][0]["Date"]} " +
                          $"{jsonNode["Records"][0]["Time"]} " +
                          $"Промилли {jsonNode["Records"][0]["Result"]} " +
                          $"{jsonNode["Records"][0]["UnitEN"]}"; 
            }
            
            return Message;
        }

        public static float GetPpmResult(string jsonString)
        {
            JsonNode jsonNode = JsonNode.Parse(jsonString);
            string Code = jsonNode["Records"][0]["Code"].ToString();
            float ppm = 0;

            if (Code == "4" || Code == "5")
            {
                ppm = float.Parse(jsonNode["Records"][0]["Result"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            }
            return ppm;
        }
    }
}
