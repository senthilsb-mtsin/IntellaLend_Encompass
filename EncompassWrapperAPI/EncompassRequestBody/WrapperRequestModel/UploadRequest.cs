using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
        public IFormFile File { get; set; }
    }
}
