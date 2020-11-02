using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class UpdateCustomFieldRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGuid { get; set; }

        [JsonProperty(PropertyName = "fields")]
        public Dictionary<string, string> Fields { get; set; }
    }
}
