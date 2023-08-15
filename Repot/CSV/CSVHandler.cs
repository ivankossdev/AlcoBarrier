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
        public static void Writer(string Path)
        {
            var records = new List<ModelDB>();

            using (var writer = new StreamWriter(Path, false, Encoding.GetEncoding("windows-1251")))
            {
               
               for(int i = 0; i < 10; i++)
                {   
                    records.Add(new ModelDB { id = i, date = $"Значение {i}", time = "Время 12.00", promille = "0.00", numcard = "1111", fio = "Card 1" });
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
