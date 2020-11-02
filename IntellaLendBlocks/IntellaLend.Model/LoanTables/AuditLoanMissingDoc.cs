using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditLoanMissingDoc
    {
        [Key]
        public Int64 AuditID { get; set; }
        public string AuditDescription { get; set; }
        public string SystemAuditDescription { get; set; }
        public DateTime AuditDateTime { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 Status { get; set; }
        public string FileName { get; set; }
        public string IDCBatchInstanceID { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 EDownloadStagingID { get; set; }
    }
}
