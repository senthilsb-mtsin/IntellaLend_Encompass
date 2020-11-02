using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class ELoanAttachmentDownload
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public Guid ELoanGUID { get; set; }
        public Int64 Status { get; set; }
        public string Error { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 TypeOfDownload { get; set; }
        public string ELoanNumber { get; set; }
    }
}
