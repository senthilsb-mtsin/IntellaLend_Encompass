using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{


    public class RuleMaster
    {
        [Key]
        public Int64 RuleID { get; set; }
        public Int64 CheckListDetailID { get; set; }
        public string RuleDescription { get; set; }
        public bool Active { get; set; }
        public string RuleJson { get; set; }
        public string DocumentType { get; set; }
        public string ActiveDocumentType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string DocVersion { get; set; }

        //public virtual CheckListDetailMaster CheckListDetailMaster { get; set; }
    }
}
