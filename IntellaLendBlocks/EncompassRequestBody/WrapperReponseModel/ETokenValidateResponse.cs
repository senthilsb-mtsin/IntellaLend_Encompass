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
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }
}
