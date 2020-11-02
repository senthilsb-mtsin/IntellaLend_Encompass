using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{

    public class BoxDownloadQueue
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 Priority { get; set; }
        public string BoxEntityID { get; set; }
        public string BoxFilePath { get; set; }
        public string BoxFileName { get; set; }
        public Int64? FileSize { get; set; }
        public int Status { get; set; }
        public int? RetryCount { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

    }
}
