using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanLOSFields
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public string Closer { get; set; }
        public string LoanOfficer { get; set; }
        public string PostCloser { get; set; }
        public string Underwriter { get; set; }
        public string EmailCloser { get; set; }
        public string EmailLoanOfficer { get; set; }
        public string EmailPostCloser { get; set; }
        public string EmailUnderwriter { get; set; }
        public string EncompassDocPages { get; set; }
    }
}
