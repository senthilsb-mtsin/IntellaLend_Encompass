using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EUploadResponse
    {
        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "attachmentGUID")]
        public string AttachmentGUID { get; set; }


    }
}
