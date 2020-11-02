using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditDescriptionConfig
    {
        [Key]
        public Int64 AuditDescriptionID { get; set; }
        public string Description { get; set; }
        public string SystemDescription { get; set; }
        public string ConcatenateFields { get; set; }
        public bool Active { get; set; }
        public Int64 ConstantID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
