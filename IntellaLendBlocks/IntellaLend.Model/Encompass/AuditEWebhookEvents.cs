using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model.Encompass
{
    public class AuditEWebhookEvents
    {
        [Key]
        public Int64 ID { get; set; }
        public string Response { get; set; }
        public Int32 EventType { get; set; }
        public bool IsTrailing { get; set; }
        public bool Processed { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
