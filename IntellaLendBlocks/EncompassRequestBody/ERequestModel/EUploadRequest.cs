using Newtonsoft.Json;
using System;

namespace EncompassRequestBody.ERequestModel
{
    public class EUploadRequest
    {
        [JsonProperty(PropertyName = "file")]
        public EFileEntities File { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }


    }

    public class EFileEntities
    {
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "size")]
        public Int64 Size { get; set; }
    }
}
