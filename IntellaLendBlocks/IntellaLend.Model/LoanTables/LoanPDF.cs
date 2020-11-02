using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanPDF
    {
        [Key]
        public Int64 LoanPDFID { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanPDFPath { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? LoanPDFGUID { get; set; }
    }
}
