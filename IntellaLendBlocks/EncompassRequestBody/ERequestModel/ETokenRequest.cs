using Newtonsoft.Json;

namespace EncompassRequestBody.ERequestModel
{
    public class ETokenRequest
    {
        [JsonProperty(PropertyName = "ClientID")]
        public string ClientID { get; set; }

        [JsonProperty(PropertyName = "ClientSecret")]
        public string ClientSecret { get; set; }

        [JsonProperty(PropertyName = "GrantType")]
        public string GrantType { get; set; }

        [JsonProperty(PropertyName = "Scope")]
        public string Scope { get; set; }

        [JsonProperty(PropertyName = "InstanceID")]
        public string InstanceID { get; set; }
    }
}
