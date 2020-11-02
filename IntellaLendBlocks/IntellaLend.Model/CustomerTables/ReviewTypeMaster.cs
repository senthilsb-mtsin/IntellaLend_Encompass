using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{

    public class ReviewTypeMaster
    {

        //public ReviewTypeMaster()
        //{
        //    this.CustLoanReviewCheckStackMappings = new HashSet<CustLoanReviewCheckStackMapping>();
        //    this.Loans = new HashSet<Loan>();
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 ReviewTypeID { get; set; }
        public string ReviewTypeName { get; set; }
        public Int32 Type { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? ReviewTypePriority { get; set; }
        public string BatchClassInputPath { get; set; }
        public string SearchCriteria { get; set; }
        public Int64? UserRoleID { get; set; }

        //public virtual List<CustReviewMapping> CustReviewMapping { get; set; }
        //public virtual List<CustReviewLoanMapping> CustReviewLoanMapping { get; set; }
        //public virtual List<CustReviewLoanDocMapping> CustReviewLoanDocMapping { get; set; }
        //public virtual List<CustReviewLoanCheckMapping> CustReviewLoanCheckMapping { get; set; }
        //public virtual List<CustReviewLoanStackMapping> CustReviewLoanStackMapping { get; set; }

        //public virtual ICollection<CustLoanReviewCheckStackMapping> CustLoanReviewCheckStackMappings { get; set; }

        //public virtual ICollection<Loan> Loans { get; set; }
    }
}
