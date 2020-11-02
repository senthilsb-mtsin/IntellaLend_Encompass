using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class PurgeStagingDetails
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 BatchID { get; set; }
        public long LoanID { get; set; }
        public long Status { get; set; }
        public string ErrMsg { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
