using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class FieldUpdateRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "fieldSchemas")]
        public object FieldSchemas { get; set; }

    }

    public class FieldGetRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "fieldIDs")]
        public string[] FieldIDs { get; set; }

    }
}
