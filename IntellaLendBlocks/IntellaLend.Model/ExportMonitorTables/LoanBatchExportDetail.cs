using System;
using System.ComponentModel.DataAnnotations;
namespace IntellaLend.Model
{
    public class LoanJobExportDetail
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 JobID { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanDocumentConfig { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Status { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}
