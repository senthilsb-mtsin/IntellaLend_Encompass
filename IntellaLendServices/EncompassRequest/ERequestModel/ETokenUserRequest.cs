using Newtonsoft.Json;

namespace EncompassRequestBody.ERequestModel
{
    public class ETokenUserRequest : ETokenRequest
    {
        [JsonProperty(PropertyName = "UserName")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }
    }
}
