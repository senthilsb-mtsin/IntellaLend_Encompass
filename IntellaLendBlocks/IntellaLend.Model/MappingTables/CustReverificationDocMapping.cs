using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CustReverificationDocMapping
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReverificationID { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
