using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CustomerImportStagingDetail
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerImportStagingID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string ServiceType { get; set; }
        public string LoanType { get; set; }
        public Int32 Status { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
