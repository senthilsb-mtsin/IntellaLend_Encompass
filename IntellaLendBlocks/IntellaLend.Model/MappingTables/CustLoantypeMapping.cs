using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.Model
{
    public class CustLoantypeMapping
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public bool DocumentTypeSync { get; set; }
        public bool LoanTypeSync { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class CustLoantypeMappingDetails
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string LoanTypeName { get; set; }
        public bool DocumentTypeSync { get; set; }
        public bool LoanTypeSync { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
