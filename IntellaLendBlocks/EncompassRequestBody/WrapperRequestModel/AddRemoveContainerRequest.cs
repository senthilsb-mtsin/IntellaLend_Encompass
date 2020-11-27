using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class AddContainerRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "documents")]
        public List<EAddDocument> Documents { get; set; }


    }

    public class EAddDocument
    {
        [JsonProperty(PropertyName = "documentName")]
        public string DocumentName { get; set; }

        [JsonProperty(PropertyName = "documentDescription")]
        public string DocumentDescription { get; set; }
    }

    public class RemoveContainerRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "documents")]
        public List<ERemoveDocument> Documents { get; set; }
    }

    public class ERemoveDocument
    {
        [JsonProperty(PropertyName = "documentID")]
        public string DocumentID { get; set; }
    }
}
