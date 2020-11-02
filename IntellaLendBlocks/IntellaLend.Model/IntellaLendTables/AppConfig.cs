using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AppConfig
    {
        [Key]
        public Int64 ID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }
}
