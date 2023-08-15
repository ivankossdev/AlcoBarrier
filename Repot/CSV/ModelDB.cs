using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repot
{
    internal class ModelDB
    {
        [Name("индефикатор")]
        public string id { get; set; }
        [Name("Значение")]
        public string val { get; set; }
    }
}
