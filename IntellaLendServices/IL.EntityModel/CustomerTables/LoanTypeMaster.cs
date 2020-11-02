using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public class LoanTypeMaster
    {
       
        //public LoanTypeMaster()
        //{
        //    this.CustLoanReviewCheckStackMappings = new HashSet<CustLoanReviewCheckStackMapping>();
        //    this.Loans = new HashSet<Loan>();
        //    this.DocumentTypeMasters = new HashSet<DocumentTypeMaster>();
        //}
        [Key]
        public long LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public bool Active { get; set; }

        //public virtual List<CustReviewLoanMapping> CustReviewLoanMapping { get; set; }
        //public virtual List<CustReviewLoanDocMapping> CustReviewLoanDocMapping { get; set; }
        //public virtual List<CustReviewLoanCheckMapping> CustReviewLoanCheckMapping { get; set; }
        //public virtual List<CustReviewLoanStackMapping> CustReviewLoanStackMapping { get; set; }


        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }
    
        //public virtual ICollection<Loan> Loans { get; set; }

        //public virtual List<DocumentTypeMaster> DocumentTypeMasters { get; set; }
    }
}
