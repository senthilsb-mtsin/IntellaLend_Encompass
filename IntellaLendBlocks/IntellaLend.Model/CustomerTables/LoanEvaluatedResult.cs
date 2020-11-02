using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanEvaluatedResult
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public string EvaluatedResult { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
