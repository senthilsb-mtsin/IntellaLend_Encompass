using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class TokenValidateRequest
    {
        [JsonProperty(PropertyName = "AccessToken")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "ClientID")]
        public string ClientID { get; set; }

        [JsonProperty(PropertyName = "ClientSecret")]
        public string ClientSecret { get; set; }

    }
}
