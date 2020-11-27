using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EWebhookSubscription
    {
        [Key]
        public Int64 ID { get; set; }
		public Int32 EventType { get; set; }
		public string SubscriptionID { get; set; }
		public DateTime CreatedOn { get; set; }
    }
}
