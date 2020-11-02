using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{

    public class CheckListDetailMaster
    {
        [Key]
        public Int64 CheckListDetailID { get; set; }
        public Int64 CheckListID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64? UserID { get; set; }
        public Int32 Rule_Type { get; set; }
        public Int64 SequenceID { get; set; }
        public string Category { get; set; }
        public bool Modified { get; set; }
        public Int64 SystemID { get; set; }
        public Int64 LOSFieldToEvalRule { get; set; }
        public string LOSValueToEvalRule { get; set; }
        public Int32? LosIsMatched { get; set; }

        [NotMapped]
        public string LOSFieldDescription { get; set; }

        [NotMapped]
        public CheckListMaster CheckListMaster { get; set; }
        [NotMapped]
        public RuleMaster RuleMasters { get; set; }
        [NotMapped]
        public User UserMaster { get; set; }
    }

    public class ChecklistItemSequence
    {
        public Int64 CheckListDetailID { get; set; }
        public Int64 SequenceID { get; set; }
    }
}
