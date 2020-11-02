using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanReverification
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 ReverificationID { get; set; }
        public string ReverificationFields { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsCoverLetterRequired { get; set; }
        public string RequiredDocuments { get; set; }
        public string ReverificationName { get; set; }
    }
}
