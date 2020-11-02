using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class PasswordPolicy
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 PasswordExpiryDays { get; set; }
        public bool StoreOldPassword { get; set; }
        public Int64 NoOfOldPassword { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
