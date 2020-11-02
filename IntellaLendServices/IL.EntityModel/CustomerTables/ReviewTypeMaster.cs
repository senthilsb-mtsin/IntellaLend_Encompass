using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.EntityModel
{

    public class ReviewTypeMaster
    {
       
        //public ReviewTypeMaster()
        //{
        //    this.CustLoanReviewCheckStackMappings = new HashSet<CustLoanReviewCheckStackMapping>();
        //    this.Loans = new HashSet<Loan>();
        //}
        [Key]
        public long ReviewTypeID { get; set; }
        public string ReviewTypeName { get; set; }
        public bool Active { get; set; }

        //public virtual List<CustReviewMapping> CustReviewMapping { get; set; }
        //public virtual List<CustReviewLoanMapping> CustReviewLoanMapping { get; set; }
        //public virtual List<CustReviewLoanDocMapping> CustReviewLoanDocMapping { get; set; }
        //public virtual List<CustReviewLoanCheckMapping> CustReviewLoanCheckMapping { get; set; }
        //public virtual List<CustReviewLoanStackMapping> CustReviewLoanStackMapping { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }
       
        //public virtual ICollection<Loan> Loans { get; set; }
    }
}
