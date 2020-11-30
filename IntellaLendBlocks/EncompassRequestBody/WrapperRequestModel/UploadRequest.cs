using Newtonsoft.Json;
using System.Web;

namespace EncompassRequestBody.WrapperRequestModel
{
    public class UploadRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "fileNameWithExtension")]
        public string FileNameWithExtension { get; set; }

        [JsonProperty(PropertyName = "file")]
        public HttpPostedFile File { get; set; }
    }
}
