using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Nodes;

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
                        Message[0] = record["Date"]?.ToString();
                        Message[1] = record["Time"]?.ToString();
                        Message[2] = $"{record["Result"]} мг/л";

                        // ИЗМЕНЕНО (пункт 4): не угадываем формат кода карты, а проверяем
                        // несколько кандидатов в employees. Раньше номер карты жёстко
                        // брался как WiegandLSB & 0xFFFF, поэтому при 32-битном коде,
                        // битах чётности (настройка Н2.19) или отсутствии карты получался
                        // «0», и все замеры выглядели как Unknown card: 0.
                        List<string> candidates = GetCardCodeCandidates(record);

                        if (candidates.Count > 0)
                        {
                            Message[4] = candidates[0];

                            foreach (string candidate in candidates)
                            {
                                string name = employee.GetNameCard(candidate);
                                if (name != null)
                                {
                                    Message[4] = candidate;
                                    Message[3] = name;
                                    break;
                                }
                            }

                            if (Message[3] == null)
                            {
                                Logger.Warning($"Wiegand LSB={record["WiegandLSB"]}, MSB={record["WiegandMSB"]}: " +
                                               $"сотрудник не найден ни по одному коду ({string.Join(", ", candidates)})");
                            }
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

        /// <summary>
        /// Возможные номера карты из Wiegand-кода события.
        /// По протоколу (таблица 12): WiegandLSB — младшие 32 бита кода, WiegandMSB — старшие 32 бита.
        /// Пустой список означает, что устройство не передало код карты.
        /// </summary>
        private static List<string> GetCardCodeCandidates(JsonNode record)
        {
            List<string> candidates = new List<string>();

            if (!TryParseWiegandCode(record["WiegandLSB"]?.ToString(), out ulong lsb))
                return candidates;

            ulong msb = 0;
            TryParseWiegandCode(record["WiegandMSB"]?.ToString(), out msb);

            ulong code = lsb | (msb << 32);
            if (code == 0)
                return candidates;

            // Wiegand-26/34 без бит чётности (Н2.19 = «Да»): номер карты — младшие 16 бит.
            AddCandidate(candidates, (ushort)(code & 0xFFFF));

            // Если биты чётности остались в коде (Н2.19 = «Нет»), номер карты сдвинут на один бит.
            AddCandidate(candidates, (ushort)((code >> 1) & 0xFFFF));

            // Код целиком — на случай, если в СКУД заведён полный идентификатор карты.
            AddCandidate(candidates, code);

            return candidates;
        }

        private static void AddCandidate(List<string> candidates, ulong value)
        {
            if (value == 0)
                return;

            string text = value.ToString(CultureInfo.InvariantCulture);
            if (!candidates.Contains(text))
                candidates.Add(text);
        }

        private static bool TryParseWiegandCode(string text, out ulong value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text))
                return false;

            text = text.Trim();

            if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                return ulong.TryParse(text.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);

            if (ulong.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                return true;

            // Страховка: код мог прийти знаковым 32-битным числом.
            if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out long signedValue))
            {
                value = unchecked((ulong)signedValue);
                return true;
            }

            // Страховка: код может прийти hex-строкой без префикса.
            return ulong.TryParse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out value);
        }
    }
}