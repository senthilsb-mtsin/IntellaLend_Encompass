using Newtonsoft.Json;

namespace EncompassRequestBody.CustomModel
{
    public class HeaderToken
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "token_tenant")]
        public string TokenTenant { get; set; }
    }
}
