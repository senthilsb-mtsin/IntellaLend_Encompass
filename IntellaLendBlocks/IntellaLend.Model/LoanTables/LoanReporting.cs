using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanReporting
    {
        [Key]
        public Int64 LoanReportingID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 ReportID { get; set; }
        public bool AddToReport { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
