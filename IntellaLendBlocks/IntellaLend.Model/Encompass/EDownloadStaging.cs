using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EDownloadStaging
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 DownloadStagingID { get; set; }
        public Guid ELoanGUID { get; set; }
        public string Step { get; set; }
        public string AttachmentGUID { get; set; }
        public string EAttachmentName { get; set; }
        public Int64 TypeOfDownload { get; set; }
        public Int64 Status { get; set; }
        public string Error { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
