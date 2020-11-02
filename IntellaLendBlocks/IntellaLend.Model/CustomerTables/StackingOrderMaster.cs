using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{

    public class StackingOrderMaster
    {

        //public StackingOrderMaster()
        //{
        //    this.CustLoanReviewCheckStackMappings = new HashSet<CustLoanReviewCheckStackMapping>();
        //    this.StackingOrderDetailMasters = new HashSet<StackingOrderDetailMaster>();
        //}

        [Key]
        public Int64 StackingOrderID { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }
        [NotMapped]
        public List<StackingOrderDetailMaster> StackingOrderDetailMasters { get; set; }

        [NotMapped]
        public StackingOrderGroupmasters StackingOrderGroupmasters { get; set; }
    }
}
