using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class SystemLoanTypeMaster
    {
        [Key]
        public Int64 LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public Int32 Type { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? LoanTypePriority { get; set; }
    }
}
