using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanChecklistAudit
    {
        [Key]
        public long ID { get; set; }
        public long LoanID { get; set; }
        public long CustomerID { get; set; }
        public long ReviewTypeID { get; set; }
        public long LoanTypeID { get; set; }
        public long ChecklistGroupID { get; set; }
        public long ChecklistDetailID { get; set; }
        public long RuleID { get; set; }
        public string ChecklistName { get; set; }
        public string ChecklistDescription { get; set; }
        public string RuleFormula { get; set; }
        public string Evaluation { get; set; }
        public Boolean Result { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
