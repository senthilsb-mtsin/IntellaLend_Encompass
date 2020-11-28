using EncompassRequestBody.WrapperReponseModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.ERequestModel
{

    public class EAttachmentDownloadRequest
    {
        [JsonProperty(PropertyName = "attachments")]
        public List<string> Attachments { get; set; }
    }

    public class EExportAttachmentJob
    {
        [JsonProperty(PropertyName = "annotationSettings")]
        public VisibilitySettings AnnotationSettings { get; set; }

        [JsonProperty(PropertyName = "entities")]
        public List<Entity> Entities { get; set; }

        [JsonProperty(PropertyName = "source")]
        public Entity Source { get; set; }

    }

    public class VisibilitySettings
    {
        [JsonProperty(PropertyName = "visibility")]
        public List<string> Visibility { get; set; }
    }
}
