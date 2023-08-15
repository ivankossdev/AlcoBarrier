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
        [Name("Дата")]
        public string date { get; set; }

        [Name("Время")]
        public string time { get; set; }

        [Name("Промилли")]
        public string promille { get; set; }

        [Name("Номер карты")]
        public string numcard { get; set; }

        [Name("ФИО")]
        public string fio { get; set; }
    }
}
