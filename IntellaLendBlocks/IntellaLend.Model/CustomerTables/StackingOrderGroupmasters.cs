using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class StackingOrderGroupmasters
    {
        [Key]
        public Int64 StackingOrderGroupID { get; set; }
        public string StackingOrderGroupName { get; set; }
        public Int64 StackingOrderID { get; set; }
        //public Int64 DocumentTypeID { get; set; }
        public string GroupSortField { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
    public class GetStackOrder
    {
        public bool isGroup { get; set; }
        public Int64 ID { get; set; }
        public string Name { get; set; }
        public string StackingOrderFieldName { get; set; }
    }
}
