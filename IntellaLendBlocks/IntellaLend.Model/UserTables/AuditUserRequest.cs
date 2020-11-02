using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditUserRequest
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public string UserRequestedURL { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
