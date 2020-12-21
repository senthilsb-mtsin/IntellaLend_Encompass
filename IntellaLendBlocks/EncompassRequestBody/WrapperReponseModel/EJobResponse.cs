using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperReponseModel
{

    public class EDownloadURLResponse
    {
        [JsonProperty(PropertyName = "attachments")]
        public List<EDownloadFile> Attachments { get; set; }
    }

    public class EDownloadFile
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "pages")]
        public List<EAttachmentPages> Pages { get; set; }

        [JsonProperty(PropertyName = "originalUrls")]
        public List<string> OriginalUrls { get; set; }
    }

    public class EAttachmentPageURL
    {
        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

    }

    public class EAttachmentPages : EAttachmentPageURL
    {
        [JsonProperty(PropertyName = "thumbnail")]
        public EAttachmentPageURL Thumbnail { get; set; }
    }

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
        public int PageCount { get; set; }

        [JsonProperty(PropertyName = "fileSize")]
        public long FileSize { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }
    }
}
