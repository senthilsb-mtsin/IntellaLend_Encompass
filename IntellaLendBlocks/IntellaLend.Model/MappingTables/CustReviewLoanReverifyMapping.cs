using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class CustReviewLoanReverifyMapping
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 ReverificationID { get; set; }
        public Int64 TemplateID { get; set; }
        public string TemplateFields { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }


        [NotMapped]
        public SystemReverificationMasters SystemReverificationMasters { get; set; }

        [NotMapped]
        public ReverificationMaster ReverificationMasters { get; set; }
    }
}
