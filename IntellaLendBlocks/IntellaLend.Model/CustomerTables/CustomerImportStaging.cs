using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CustomerImportStaging
    {
        [Key]
        public Int64 ID { get; set; }
        public string FilePath { get; set; }
        public Int64 ImportCount { get; set; }
        public Int32 Status { get; set; }
        public Int64 AssignType { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
