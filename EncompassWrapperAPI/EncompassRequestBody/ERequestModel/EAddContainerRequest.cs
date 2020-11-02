using Newtonsoft.Json;

namespace EncompassRequestBody.ERequestModel
{
    public class EAddContainerRequest
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "applicationId")]
        public string ApplicationId { get; set; }
    }
}
