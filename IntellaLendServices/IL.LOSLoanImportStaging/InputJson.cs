using System;

namespace IL.LOSLoanImport
{
    public class InputJson
    {
        public Int64 LenderCode { get; set; }
        public string LenderName { get; set; }
        public string LoanID { get; set; }
        public string LoanType { get; set; }
        public string ReviewType { get; set; }
        public Int64 Priority { get; set; }
        public string AuditPeriod { get; set; }
        public string FNMFile { get; set; }
        public string[] LoanFiles { get; set; }
    }
}
