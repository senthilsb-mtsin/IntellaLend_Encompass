using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LOSLoanDetails
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LOSDocumentID { get; set; }
        public Int64 LoanID { get; set; }
        public string LOSDetailJSON { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
