using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class CustomAddressDetail
    {
        [Key]
        public Int64 ID { get; set; }
        public string Country { get; set; }
        public Int64 UserID { get; set; }
        public Int64 ZipCode { get; set; }
    
        //public virtual User User { get; set; }
    }
}
