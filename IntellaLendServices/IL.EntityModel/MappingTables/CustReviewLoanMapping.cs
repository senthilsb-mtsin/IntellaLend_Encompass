using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public class CustReviewLoanMapping
    {
        [Key]
        public long ID { get; set; }
        public long CustomerID { get; set; }        
        public long ReviewTypeID { get; set; }
        public long LoanTypeID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual CustomerMaster CustomerMaster { get; set; }        
        public virtual ReviewTypeMaster ReviewTypeMaster { get; set; }
        public virtual LoanTypeMaster LoanTypeMaster { get; set; }
    }
}
