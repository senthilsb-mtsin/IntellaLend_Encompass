using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public partial class EncompassConfig
    {
        [Key]
        public long Id { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
