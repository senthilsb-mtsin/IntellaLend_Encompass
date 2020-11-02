using Newtonsoft.Json;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class AddContainerRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "documentName")]
        public string DocumentName { get; set; }
    }
}
