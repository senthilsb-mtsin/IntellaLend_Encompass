using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditLoanSearch
    {
        [Key]
        public Int64 AuditID { get; set; }
        public string AuditDescription { get; set; }
        public string SystemAuditDescription { get; set; }
        public DateTime AuditDateTime { get; set; }
        public Int64 LoanSearchID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string LoanNumber { get; set; }
        public string BorrowerName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public Int64 Status { get; set; }
        public decimal LoanAmount { get; set; }
        public string SSN { get; set; }
        public Int64 CustomerID { get; set; }
        public string PropertyAddress { get; set; }
        public string InvestorLoanNumber { get; set; }

    }
}
