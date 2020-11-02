using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public  class CheckListMaster
    {
        
        [Key]
        public long CheckListID { get; set; }
        public string CheckListName { get; set; }
        public bool Active { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }
        //public virtual List<CheckListDetailMaster> CheckListDetailMasters { get; set; }
    }
}
