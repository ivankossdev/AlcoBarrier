using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoBarrier
{
    public static class DateTimeHandler 
    {
        public static void Function()
        {
            string[] formats = { "ddMMyyyyHHmmss" };
            string[] dateStrings = { "16082013115216" };
            DateTime parsedDate;

            foreach (var dateString in dateStrings)
            {
                if (DateTime.TryParseExact(dateString, formats, null,
                                           System.Globalization.DateTimeStyles.AssumeUniversal |
                                           System.Globalization.DateTimeStyles.AdjustToUniversal,
                                           out parsedDate))
                    Console.WriteLine($"{dateString} --> {parsedDate:g}");
                else
                    Console.WriteLine($"Cannot convert {dateString}");
            }

        }
    }
}
