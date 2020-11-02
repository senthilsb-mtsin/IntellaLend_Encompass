using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public string ErrorCode { get; set; }

    }
}
