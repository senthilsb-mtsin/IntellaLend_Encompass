using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LOSImportStaging
    {

        [Key]
        public Int64 ID { get; set; }
        public string FileName { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanID { get; set; }
        public bool ValidJson { get; set; }
        public string ErrorMsg { get; set; }
        public Int32 Status { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime ModifiedOn { get; set; }
    }


}
