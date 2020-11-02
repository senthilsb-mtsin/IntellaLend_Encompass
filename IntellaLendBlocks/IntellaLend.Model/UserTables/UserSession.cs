using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class UserSession
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public DateTime LastAccessedTime { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string HashValidator { get; set; }
    }
}
