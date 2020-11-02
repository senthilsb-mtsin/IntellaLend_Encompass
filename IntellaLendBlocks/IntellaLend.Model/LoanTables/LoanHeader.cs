using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.Model
{
    public class LoanHeader
    {
        public string LoanNumber { get; set; }
        public string BorrowerName { get; set; }
        public Decimal LoanAmount { get; set; }
        public DateTime AuditMonthYear { get; set; }
        public string PropertyAddress { get; set; }
        public string InvestorLoanNumber { get; set; }
        public string PostCloser { get; set; }
        public string LoanOfficer { get; set; }
        public string UnderWriter { get; set; }
        public DateTime AuditDueDate { get; set; }
    }
}
