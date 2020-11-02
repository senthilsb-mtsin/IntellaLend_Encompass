using System;
using System.ComponentModel.DataAnnotations;


namespace IntellaLend.Model
{
    public class ImportStagings
    {
        [Key]
        public int Id { get; set; }
        public string TenantSchema { get; set; }
        public long LoanId { get; set; }
        public long ReviewTypeID { get; set; }
        public long? Priority { get; set; }
        public DateTime? LoanCreatedDate { get; set; }
        public string Path { get; set; }
        public long Status { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool LoanPicked { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }

}
