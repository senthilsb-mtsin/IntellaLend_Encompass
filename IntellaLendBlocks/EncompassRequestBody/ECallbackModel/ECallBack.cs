using Newtonsoft.Json;

namespace EncompassRequestBody.ECallbackModel
{
    public class ECallBack
    {
        [JsonProperty(PropertyName = "eventId")]
        public string EventID { get; set; }

        [JsonProperty(PropertyName = "eventTime")]
        public string EventTime { get; set; }

        [JsonProperty(PropertyName = "eventType")]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "meta")]
        public EventMetaData Meta { get; set; }
    }

    public class EventMetaData
    {
        [JsonProperty(PropertyName = "resourceType")]
        public string ResourceType { get; set; }

        [JsonProperty(PropertyName = "resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty(PropertyName = "instanceId")]
        public string InstanceId { get; set; }

        [JsonProperty(PropertyName = "resourceRef")]
        public string ResourceRef { get; set; }
    }
}
