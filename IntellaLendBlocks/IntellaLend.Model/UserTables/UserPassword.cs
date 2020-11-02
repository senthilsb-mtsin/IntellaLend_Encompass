using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class UserPassword
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public String Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

    }
}

