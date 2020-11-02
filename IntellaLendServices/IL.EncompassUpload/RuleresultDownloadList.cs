using System;

namespace IL.EncompassUpload
{
    public class LoanInfo
    {
        public string LoanNumber { get; set; }
        public string LoanType { get; set; }
        public string LoanStatus { get; set; }
        public decimal LoanAmount { get; set; }
        public string BorrowerName { get; set; }
        public string ServiceType { get; set; }
        public DateTime AuditMonthYear { get; set; }
        public string PropertyAddress { get; set; }
        public string InvestorLoanNumber { get; set; }
        public string PostCloser { get; set; }
        public string LoanOfficer { get; set; }
        public string UnderWriter { get; set; }
        public DateTime? AuditDueDate { get; set; }
        public DateTime? ReceivedDate { get; set; }

    }

    public class RuleResult

    {
        public string CheckListName { get; set; }
        public string Formula { get; set; }
        public bool Expression { get; set; }
        public string Result { get; set; }
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
        public string RoleType { get; set; }
        public string SequenceID { get; set; }
        public string Category { get; set; }
        public string RuleType { get; set; }
    }
    
    

    

}
