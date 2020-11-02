using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class ETokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }
}
