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

        public static string[] GetArrayResult(string jsonString)
        {
            string[] Message = new string[4];

            JsonNode jsonNode;
            try
            {
                jsonNode = JsonNode.Parse(jsonString);
                if (jsonNode["getLog"] == null)
                {
                    string Code = jsonNode["Records"][0]["Code"].ToString();
                   

                    if (Code == "4" || Code == "5")
                    {
                        string CardName = ConvertCodeCard(jsonNode["Records"][0]["WiegandLSB"].ToString());
                        Message[0] = jsonNode["Records"][0]["Date"].ToString();
                        Message[1] = jsonNode["Records"][0]["Time"].ToString();
                        Message[2] = $"{jsonNode["Records"][0]["Result"]} мг/л";
                        Message[3] = SqLiteHandler.GetNameCard(CardName);
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

            //return $"{IntCode >> 16}-{IntCode & 0xFFFF}";
            return $"{IntCode & 0xFFFF}";
        }
    }
}
