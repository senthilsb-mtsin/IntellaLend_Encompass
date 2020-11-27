using Newtonsoft.Json;

namespace EncompassRequestBody.EResponseModel
{
    public class EFieldResponse
    {
        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

    }
}
