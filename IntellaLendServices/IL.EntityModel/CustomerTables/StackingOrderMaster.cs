using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{
    
    public class StackingOrderMaster
    {
        
        //public StackingOrderMaster()
        //{
        //    this.CustLoanReviewCheckStackMappings = new HashSet<CustLoanReviewCheckStackMapping>();
        //    this.StackingOrderDetailMasters = new HashSet<StackingOrderDetailMaster>();
        //}
    
        [Key]
        public long StackingOrderID { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }

       // public virtual List<StackingOrderDetailMaster> StackingOrderDetailMasters { get; set; }
    }
}
