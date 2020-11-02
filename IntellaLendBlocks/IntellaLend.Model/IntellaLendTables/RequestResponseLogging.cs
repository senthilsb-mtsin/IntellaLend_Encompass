using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class RequestResponseLogging
    {
        [Key]
        public Int64 ID { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public string LogXML { get; set; }
    }
}
