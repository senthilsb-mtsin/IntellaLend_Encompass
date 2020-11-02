using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class UserAddressDetail
    {
        [Key]
        public Int64 ID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Int64 UserID { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    
        //public virtual User User { get; set; }
    }
}
