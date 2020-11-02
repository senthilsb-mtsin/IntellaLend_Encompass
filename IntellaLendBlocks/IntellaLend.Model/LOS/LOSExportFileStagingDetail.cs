using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class LOSExportFileStagingDetail
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 LOSExportFileStagingID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
        public Int64 Version { get; set; }
        public Int64 FileType { get; set; }
        public string FileName { get; set; }
        public string ErrorMsg { get; set; }
        public Int32 Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public List<Int32> Pages { get; set; }
    }
}
