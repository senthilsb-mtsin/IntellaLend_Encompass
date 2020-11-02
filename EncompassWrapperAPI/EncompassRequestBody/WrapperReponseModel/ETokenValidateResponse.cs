using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class ETokenValidateResponse
    {
        [JsonProperty(PropertyName = "validToken")]
        public bool ValidToken { get; set; }
    }

    public class EToken
    {
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "tokenType")]
        public string TokenType { get; set; }
    }
}
