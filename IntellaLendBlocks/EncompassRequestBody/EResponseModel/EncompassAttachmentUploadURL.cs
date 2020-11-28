using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EncompassRequestBody.EResponseModel
{
    public class EncompassAttachmentUploadURL
    {
        [JsonProperty(PropertyName = "mediaUrl")]
        public string MediaUrl { get; set; }
    }

    public class EAttachmentUploadResponse
    {
        [JsonProperty(PropertyName = "attachmentId")]
        public string AttachmentID { get; set; }

        [JsonProperty(PropertyName = "authorizationHeader")]
        public string AuthorizationHeader { get; set; }

        [JsonProperty(PropertyName = "uploadUrl")]
        public string UploadUrl { get; set; }

        [JsonProperty(PropertyName = "multiChunkRequired")]
        public bool MultiChunkRequired { get; set; }

        [JsonProperty(PropertyName = "expiresAt")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty(PropertyName = "multiChunk")]
        public EUploadChunk MultiChunk { get; set; }


    }

    public class EUploadChunk
    {
        [JsonProperty(PropertyName = "chunkList")]
        public List<EUploadChunkEntites> ChunkList { get; set; }

        [JsonProperty(PropertyName = "commitUrl")]
        public string CommitUrl { get; set; }
    }

    public class EUploadChunkEntites
    {
        [JsonProperty(PropertyName = "uploadUrl")]
        public string UploadUrl { get; set; }

        [JsonProperty(PropertyName = "size")]
        public Int32 Size { get; set; }
    }
}
