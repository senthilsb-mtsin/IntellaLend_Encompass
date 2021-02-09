using System.Collections.Generic;

namespace IntellaLend.Model
{
    public class EWebhookEventCreation
    {
        public List<string> events { get; set; }
        public string endpoint { get; set; }
        public string resource { get; set; }
        public string signingkey { get; set; }
        public EWebHookFilter filters { get; set; }

    }

    public class EWebHookFilter
    {
        public List<string> attributes { get; set; }
    }
    public class EWebHookDeleteEventSubscription
    {
        public string subscriptionId { get; set; }
    }
}