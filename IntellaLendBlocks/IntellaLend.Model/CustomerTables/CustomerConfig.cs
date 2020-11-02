using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CustomerConfig
    {
        [Key]
        public Int64 ConfigID { get; set; }
        public Int64 CustomerID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class CustomerConfigItem
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public Boolean Active { get; set; }
    }
}
