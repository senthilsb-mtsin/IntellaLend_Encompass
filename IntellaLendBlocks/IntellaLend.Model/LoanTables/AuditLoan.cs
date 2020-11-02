using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditLoan
    {
        [Key]
        public Int64 AuditID { get; set; }
        public string AuditDescription { get; set; }
        public string SystemAuditDescription { get; set; }
        public DateTime AuditDateTime { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 UploadedUserID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 LoggedUserID { get; set; }
        public Int64 Status { get; set; }
        public int SubStatus { get; set; }
        public DateTime? LoanCreatedOn { get; set; }
        public DateTime? LoanLastModifiedOn { get; set; }
        public string FileName { get; set; }
        public Int64 CustomerID { get; set; }
        public string LoanNumber { get; set; }
        public Int64 LastAccessedUserID { get; set; }
        public string Notes { get; set; }
        public string EphesoftBatchInstanceID { get; set; }
        public decimal EphesoftOCRAccuracy { get; set; }
        // public bool FromBox { get; set; }
        public Int64 EphesoftStatusID { get; set; }
        public DateTime AuditMonthYear { get; set; }
        public Int64? PageCount { get; set; }
        public int UploadType { get; set; }
        public DateTime? AuditCompletedDate { get; set; }
    }
}
