using Newtonsoft.Json;

namespace EncompassRequestBody.ERequestModel
{
    public class EAddContainerRequest
    {
        [JsonProperty(PropertyName = "title")]
        public string title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
    }

    public class ERemoveContainerRequest
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
    }
}
