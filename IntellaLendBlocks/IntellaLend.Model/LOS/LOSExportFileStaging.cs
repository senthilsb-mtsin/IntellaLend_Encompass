using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LOSExportFileStaging
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public string FileName { get; set; }
        public string FileJson { get; set; }
        public Int32 FileType { get; set; }
        public string ErrorMsg { get; set; }
        public Int32 Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int64 TrailingAuditID { get; set; }

    }
}
