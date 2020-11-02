using Newtonsoft.Json;
using System;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EJobResponse
    {
        [JsonProperty(PropertyName = "jobId")]
        public string JobId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "file")]
        public EFile File { get; set; }
    }

    public class EFile
    {
        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

        [JsonProperty(PropertyName = "entityUri")]
        public string EntityUri { get; set; }

        [JsonProperty(PropertyName = "authorizationHeader")]
        public string AuthorizationHeader { get; set; }

        [JsonProperty(PropertyName = "pageCount")]
        public Int32 PageCount { get; set; }

        [JsonProperty(PropertyName = "fileSize")]
        public Int64 FileSize { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }
    }
}
