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
    public class AuditUserSession
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserSessionID { get; set; }
        public Int64 UserID { get; set; }
        public string HashValidator { get; set; }
        public DateTime LastAccessedTime { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime SessionCreatedTime { get; set; }
        public string IPAddress { get; set; }
        public string Device { get; set; }
        public string HostName { get; set; }
        public string AgentType { get; set; }
        public string AccessedURL { get; set; }

    }
}
