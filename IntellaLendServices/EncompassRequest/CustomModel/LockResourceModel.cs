using Newtonsoft.Json;

namespace EncompassRequestBody.CustomModel
{
    public class LockResourceModel
    {
        [JsonProperty(PropertyName = "status")]
        public bool Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
