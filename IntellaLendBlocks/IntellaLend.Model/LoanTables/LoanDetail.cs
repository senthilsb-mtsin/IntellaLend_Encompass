using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LoanDetail
    {
        [Key]
        public Int64 LoanDetailID { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanObject { get; set; }
        public string ManualQuestioners { get; set; }
        public Int64 TotalDocCount { get; set; }
        public Int64 MissingDocCount { get; set; }
        public Int64 MissingCriticalDocCount { get; set; }
        public Int64 MissingNonCriticalDocCount { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
