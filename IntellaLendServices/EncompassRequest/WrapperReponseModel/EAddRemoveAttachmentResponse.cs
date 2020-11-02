using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EAddRemoveAttachmentResponse
    {
        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
