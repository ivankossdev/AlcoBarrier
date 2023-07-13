using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AlcoBarrier
{
    public static class MyJson
    {
        public static string CreateLogMessage(string memoryAddr)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = "getLog",
                ["Position"] = "fromAddr",
                ["MemAddr"] = memoryAddr
            };

            return forecastObject.ToJsonString();
        }

        public static string CreateCmdTypeInfMessage(string command)
        {
            try
            {
                var forecastObject = new JsonObject
                {
                    ["cmdType"] = command
                };

                return forecastObject.ToJsonString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return "Not Result";
        }

        public static string GetCountMessage(string jsonString)
        {
            string Result = string.Empty;
            try
            {
                JsonNode jsonNode = JsonNode.Parse(jsonString);
                Result = jsonNode["LastRecord"]["MemAddr"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Result;
        }

        public static string GetDataMemory(string jsonString)
        {
            string Result = string.Empty;
            try
            {
                JsonNode jsonNode = JsonNode.Parse(jsonString);
                return jsonNode["Records"][0].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Result;
        }

        public static string GetStringResult(string jsonString)
        {
            string Message = string.Empty;

            string Code = string.Empty;
            JsonNode jsonNode;
            try
            {
                jsonNode = JsonNode.Parse(jsonString);
                if (jsonNode["getLog"] == null)
                {
                    Code = jsonNode["Records"][0]["Code"].ToString();

                    if (Code == "4" || Code == "5")
                    {
                        Message = $"{jsonNode["Records"][0]["Date"]} " +
                                  $"{jsonNode["Records"][0]["Time"]} " +
                                  $"Концентрация {jsonNode["Records"][0]["Result"]} мг/л " +
                                  $"Карточка {jsonNode["Records"][0]["WiegandLSB"]} ";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Message;
        }

        /*
         * V 1. Асинхронная функция постоянного опроса памяти алкотестера. 
         * 2. Конвертирование кода карты.
         */
    }
}
