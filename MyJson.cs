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
                          $"Концентрация {jsonNode["Records"][0]["Result"]} мг/л " +
                          $"Карточка {jsonNode["Records"][0]["WiegandLSB"]} "; 
            }
            
            return Message;
        }

        /*
         * 1. Асинхронная функция постоянного опроса памяти алкотестера.
         * 2. Конвертирование кода карты.
         */
    }
}
