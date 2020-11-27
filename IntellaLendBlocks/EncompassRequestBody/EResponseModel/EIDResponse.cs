using Newtonsoft.Json;

namespace EncompassRequestBody.EResponseModel
{
    public class EIDResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
