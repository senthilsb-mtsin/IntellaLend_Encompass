using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class StackingOrderDetailMaster
    {
        [Key]
        public Int64 StackingOrderDetailID { get; set; }
        public Int64? StackingOrderGroupID { get; set; }
        public Int64 StackingOrderID { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public Int64 SequenceID { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        //public Array StackingGroupDocuments { get; set; }
        // public string StackingOrderGroupName { get; set; }
        //public List<StackingOrderGroupmasters> StackingOrderGroupmasters { get; set; }

        //public virtual StackingOrderMaster StackingOrderMaster { get; set; }
    }

    public class StackingOrderDocumentFieldMaster
    {
        public Int64 StackingOrderDetailID { get; set; }
        public Int64 StackingOrderID { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public Int64 SequenceID { get; set; }
        public List<DocumentFieldMaster> DocFieldList { get; set; }
    }

}
