using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Repot
{
    internal static class CSVHandler
    {
        public static void Writer()
        {
            var records = new List<ModelDB>();

            using (var writer = new StreamWriter($"{Directory.GetCurrentDirectory()}\\file.csv", false, Encoding.GetEncoding("windows-1251")))
            {
               
               for(int i = 0; i < 10; i++)
                {   
                    records.Add(new ModelDB { id = $"Строка {i}", val = $"Значение {i}" });
                }
                var config = new CsvConfiguration(CultureInfo.GetCultureInfo("ru-Ru"))
                {
                    Delimiter = ";",
                };

                using (var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(records);
                };
            }

        }
    }


}
