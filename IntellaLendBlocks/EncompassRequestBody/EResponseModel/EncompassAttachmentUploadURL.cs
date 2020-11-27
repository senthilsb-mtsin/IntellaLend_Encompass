using Newtonsoft.Json;

namespace EncompassRequestBody.EResponseModel
{
    public class EncompassAttachmentUploadURL
    {
        [JsonProperty(PropertyName = "mediaUrl")]
        public string MediaUrl { get; set; }
    }
}
