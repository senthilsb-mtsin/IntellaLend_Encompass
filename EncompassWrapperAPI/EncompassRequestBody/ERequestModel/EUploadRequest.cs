using Newtonsoft.Json;

namespace EncompassRequestBody.ERequestModel
{
    public class EUploadRequest
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "fileWithExtension")]
        public string FileWithExtension { get; set; }

        [JsonProperty(PropertyName = "createReason")]
        public int CreateReason { get; set; }
    }
}
