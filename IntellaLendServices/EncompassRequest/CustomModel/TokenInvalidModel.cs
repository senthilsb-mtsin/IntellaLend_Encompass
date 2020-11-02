using Newtonsoft.Json;

namespace EncompassRequestBody.CustomModel
{
    public class TokenInvalidModel
    {
        [JsonProperty(PropertyName = "error_description")]
        public string ErrorDescription { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
