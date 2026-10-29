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

        public string CmdTypeHeaderAllMemory(string qty)
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

        public List<string[]> RecordsMemoryList(string data)
        {
            JsonNode jsonNode = JsonNode.Parse(data);
            int count = jsonNode["Records"].AsArray().Count;

            List<string[]> Records = new List<string[]>();

            for (int i = 0; i < count; i++)
            {
                string Code = jsonNode["Records"][i]["Code"].ToString();
                if (Code == "4" || Code == "5")
                {
                    string CardCode = ConvertCodeCard(jsonNode["Records"][i]["WiegandLSB"].ToString());
                    Records.Add(RecordsArray(jsonNode["Records"][i]["Date"].ToString(),
                                jsonNode["Records"][i]["Time"].ToString(),
                                $"{jsonNode["Records"][i]["Result"]} мг/л",
                                CardCode));
                }
            }
            return Records;
        }

        private string[] RecordsArray(string Date, string Time, string Result, string CardCode)
        {
            string[] Rec = new string[]
            {
                Date, Time, Result, CardCode
            };

            return Rec;
        }

        EmloeyesDB employee = new EmloeyesDB("employees");
        public string[] GetArrayResult(string jsonString)
        {
            string[] Message = new string[5];

            JsonNode jsonNode;
            try
            {
                jsonNode = JsonNode.Parse(jsonString);
                if (jsonNode == null) return Message;

                if (jsonNode["getLog"] == null)
                {
                    var records = jsonNode["Records"]?.AsArray();
                    if (records == null || records.Count == 0) return Message;

                    var record = records[0];

                    string Code = record["Code"]?.ToString();
                    if (Code == "4" || Code == "5")
                    {
                        string CardName = ConvertCodeCard(record["WiegandLSB"]?.ToString());
                        Message[0] = record["Date"]?.ToString();
                        Message[1] = record["Time"]?.ToString();
                        Message[2] = $"{record["Result"]} мг/л";

                        // ИЗМЕНЕНО (этап C6): GetNameCard теперь сам возвращает null,
                        // если карты нет в employees (без NRE). try/catch NRE больше не нужен.
                        // Message[3] может остаться null — MainForm это обрабатывает.
                        if (!string.IsNullOrEmpty(CardName))
                        {
                            Message[4] = CardName;
                            Message[3] = employee.GetNameCard(CardName);
                        }
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
                if (jsonNode["EthBlock"] != null)
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
            if (!int.TryParse(code, out int IntCode))
                return code;

            return $"{IntCode & 0xFFFF}";
        }
    }
}