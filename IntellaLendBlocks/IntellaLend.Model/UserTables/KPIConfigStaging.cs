using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class KPIConfigStaging
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 GroupID { get; set; }
        public int ConfigType { get; set; }
        public Int64 Goal { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
