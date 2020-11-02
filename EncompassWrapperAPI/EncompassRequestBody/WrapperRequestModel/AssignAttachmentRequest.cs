using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class AssignAttachmentRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "documentGUID")]
        public string DocumentGUID { get; set; }

        [JsonProperty(PropertyName = "attachmentGUIDs")]
        public List<string> AttachmentGUIDs { get; set; }
    }
}
