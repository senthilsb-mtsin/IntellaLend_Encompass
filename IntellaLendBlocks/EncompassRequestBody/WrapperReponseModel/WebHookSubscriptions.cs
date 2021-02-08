using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class WebHookSubscriptions
    {
        [JsonProperty(PropertyName = "subscriptionId")]
        public string SubscriptionID { get; set; }

        [JsonProperty(PropertyName = "endpoint")]
        public string Endpoint { get; set; }

        [JsonProperty(PropertyName = "signingkey")]
        public string Signingkey { get; set; }

        [JsonProperty(PropertyName = "instanceId")]
        public string InstanceID { get; set; }

        [JsonProperty(PropertyName = "resource")]
        public string Resource { get; set; }
    }
}
