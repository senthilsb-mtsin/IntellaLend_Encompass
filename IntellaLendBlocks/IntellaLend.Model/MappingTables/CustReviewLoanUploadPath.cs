using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CustReviewLoanUploadPath
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public string BoxUploadPath { get; set; }
        public string UploadPath { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
