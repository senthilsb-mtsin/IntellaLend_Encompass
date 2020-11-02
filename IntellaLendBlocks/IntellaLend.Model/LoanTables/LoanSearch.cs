using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class LoanSearch
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string LoanNumber { get; set; }
        public string BorrowerName { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public Int64 Status { get; set; }
        public decimal LoanAmount { get; set; }
        public string SSN { get; set; }
        public Int64 CustomerID { get; set; }
        public DateTime? AuditDueDate { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string PropertyAddress { get; set; }
        public string InvestorLoanNumber { get; set; }
        [NotMapped]
        public LoanTypeMaster LoanTypeMaster { get; set; }
        //public virtual WorkFlowStatusMaster WorkFlowStatusMaster { get; set; }
    }
}
