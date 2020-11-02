using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class LoanStipulation
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 StipulationCategoryID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string StipulationDescription { get; set; }
        public Int64 StipulationStatus { get; set; }
        public string Notes { get; set; }
        public DateTime AuditMonthYear { get; set; }
        public DateTime? RecievedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public string StipulationCategoryName { get; set; }
        [NotMapped]
        public LoanInvestorStipulationDetails _loanInvestor { get; set; }
    }
}
