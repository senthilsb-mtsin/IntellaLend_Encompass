using Newtonsoft.Json;

namespace EncompassRequestBody.ERequestModel
{
    public class EAddRemoveAttachment
    {
        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }
    }
}
