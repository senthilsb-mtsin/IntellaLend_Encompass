using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class LockRequest
    {
        [JsonProperty(PropertyName = "resource")]
        public Entity Resource { get; set; }

        [JsonProperty(PropertyName = "lockType")]
        public string LockType { get; set; }
    }

    public class Entity
    {
        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }
    }

    public class LockLoanResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
    }


}
