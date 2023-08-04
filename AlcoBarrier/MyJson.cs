using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace AlcoBarrier
{
    public class MyJson
    {
        EmloeyesDB employee = new EmloeyesDB("employees") { path = Directory.GetCurrentDirectory() };
        public string CreateLogMessage(string memoryAddr)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = "getLog",
                ["Position"] = "fromAddr",
                ["MemAddr"] = memoryAddr
            };

            return forecastObject.ToJsonString();
        }

        public string CmdTypeHeader(string command)
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

        public string GetCountMessage(string jsonString)
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

        private string GetQTY(string jsonString)
        {

            string Result = string.Empty;
            try
            {
                JsonNode jsonNode = JsonNode.Parse(jsonString);
                Result = jsonNode["QTY"].ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Result;
        }

        private string CreateAllRecordMessage(string qty)
        {
            var forecastObject = new JsonObject
            {
                ["cmdType"] = "getLog",
                ["Position"] = "toLast",
                ["QTY"] = GetQTY(qty),
            };

            return forecastObject.ToJsonString();
        }

        public string GetDataMemory(string qty)
        {

            string Result = string.Empty;
            try
            {
                JsonNode jsonNode = JsonNode.Parse(CreateAllRecordMessage(qty));
                return jsonNode.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Result;
        }

        public string[] GetArrayResult(string jsonString)
        {
            string[] Message = new string[5];

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
                        Message[3] = employee.GetNameCard(CardName);
                        Message[4] = CardName;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return Message;
        }

        public string GetInfoAlcoBarrier(string jsonString)
        {
            JsonNode jsonNode;
            string Result = string.Empty;
            try
            {
                jsonNode = JsonNode.Parse(jsonString);
                if(jsonNode["EthBlock"] != null)
                {
                    Result = $"AlcoBarrier Host: {jsonNode["EthBlock"]["HostName"]}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Result;
        }

        private string ConvertCodeCard(string code)
        {
            int IntCode = Convert.ToInt32(code);

            //return $"{IntCode >> 16}-{IntCode & 0xFFFF}";
            return $"{IntCode & 0xFFFF}";
        }
    }
}
