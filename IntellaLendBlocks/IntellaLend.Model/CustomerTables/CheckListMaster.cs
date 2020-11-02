using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{

    public class CheckListMaster
    {

        [Key]
        public Int64 CheckListID { get; set; }
        public string CheckListName { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? Sync { get; set; }
        [NotMapped]
        public List<CheckListDetailMaster> CheckListDetailMasters { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }
        //public virtual List<CheckListDetailMaster> CheckListDetailMasters { get; set; }
        //public virtual List<CheckListDetailMaster> CheckListDetailMasters { get; set; }
    }

    public class CheckListResult
    {
        public List<ManualQuestioner> loanQuestioner { get; set; }
        public List<Dictionary<string, string>> allChecklist { get; set; }
    }
}
