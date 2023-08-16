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
        public static void Writer(string Path, List<ModelDB> records)
        {
            using (var writer = new StreamWriter(Path, false, Encoding.GetEncoding("windows-1251")))
            {
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
