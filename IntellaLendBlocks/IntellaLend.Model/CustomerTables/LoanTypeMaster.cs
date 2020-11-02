using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public Int32 Type { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? LoanTypePriority { get; set; }

        //public virtual List<CustReviewLoanMapping> CustReviewLoanMapping { get; set; }
        //public virtual List<CustReviewLoanDocMapping> CustReviewLoanDocMapping { get; set; }
        //public virtual List<CustReviewLoanCheckMapping> CustReviewLoanCheckMapping { get; set; }
        //public virtual List<CustReviewLoanStackMapping> CustReviewLoanStackMapping { get; set; }


        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }

        //public virtual ICollection<Loan> Loans { get; set; }

        //public virtual List<DocumentTypeMaster> DocumentTypeMasters { get; set; }
    }
}
