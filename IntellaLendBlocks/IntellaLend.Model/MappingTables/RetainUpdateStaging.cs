using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class RetainUpdateStaging
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 SyncLevel { get; set; }
        public Int64? Synchronized { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
    public class RetainUpdateStagingDetails
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64? Synchronized { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public String ErrorMsg { get; set; }


    }
}
