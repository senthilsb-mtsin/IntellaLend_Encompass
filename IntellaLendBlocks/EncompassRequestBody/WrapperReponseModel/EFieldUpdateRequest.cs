using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EFieldUpdate
    {
        [JsonProperty(PropertyName = "fieldName")]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "stringValue")]
        public string StringValue { get; set; }
    }

    public class ECustomFieldUpdateRequest
    {
        [JsonProperty(PropertyName = "customFields")]
        public List<EFieldUpdate> CustomFields { get; set; }
    }
}
