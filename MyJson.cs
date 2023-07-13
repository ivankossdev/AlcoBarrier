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
                    
                    string CardCode = jsonNode["Records"][0]["WiegandLSB"].ToString();

                    if (Code == "4" || Code == "5")
                    {
                        Message = $"{jsonNode["Records"][0]["Date"]} " +
                                  $"{jsonNode["Records"][0]["Time"]} " +
                                  $"Концентрация {jsonNode["Records"][0]["Result"]} мг/л " +
                                  $"Карточка {ConvertCodeCard(CardCode)} ";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Message;
        }

        private static string ConvertCodeCard(string code)
        {
            int IntCode = Convert.ToInt32(code);
            
            return $"{IntCode >> 16}-{IntCode & 0xFFFF}";
        }
    }
}
