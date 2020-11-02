using Newtonsoft.Json;
using System.Collections.Generic;

namespace EncompassRequestBody.EResponseModel
{
    public class EAttachment
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "attachmentId")]
        public string AttachmentID { get; set; }

        [JsonProperty(PropertyName = "document")]
        public EDocument Document { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdByName")]
        public string CreatedByName { get; set; }

        [JsonProperty(PropertyName = "attachmentType")]
        public long AttachmentType { get; set; }

        [JsonProperty(PropertyName = "fileSize")]
        public long FileSize { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "isRemoved")]
        public bool IsRemoved { get; set; }

        [JsonProperty(PropertyName = "rotation")]
        public long Rotation { get; set; }

        [JsonProperty(PropertyName = "pages")]
        public List<EPages> Pages { get; set; }

    }

    public class EPages
    {
        [JsonProperty(PropertyName = "imageKey")]
        public string ImageKey { get; set; }

        [JsonProperty(PropertyName = "zipKey")]
        public string ZipKey { get; set; }

        [JsonProperty(PropertyName = "width")]
        public string Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }

        [JsonProperty(PropertyName = "horizontalResolution")]
        public string HorizontalResolution { get; set; }

        [JsonProperty(PropertyName = "verticalResolution")]
        public string VerticalResolution { get; set; }

        [JsonProperty(PropertyName = "rotation")]
        public string Rotation { get; set; }

        [JsonProperty(PropertyName = "fileSize")]
        public string FileSize { get; set; }

        [JsonProperty(PropertyName = "thumbnail")]
        public EThumbnail Thumbnail { get; set; }

        [JsonProperty(PropertyName = "mediaUrl")]
        public string MediaURL { get; set; }
    }

    public class EThumbnail
    {
        [JsonProperty(PropertyName = "imageKey")]
        public string ImageKey { get; set; }

        [JsonProperty(PropertyName = "zipKey")]
        public string ZipKey { get; set; }

        [JsonProperty(PropertyName = "width")]
        public string Width { get; set; }

        [JsonProperty(PropertyName = "height")]
        public string Height { get; set; }

        [JsonProperty(PropertyName = "horizontalResolution")]
        public string HorizontalResolution { get; set; }

        [JsonProperty(PropertyName = "verticalResolution")]
        public string VerticalResolution { get; set; }

        [JsonProperty(PropertyName = "mediaUrl")]
        public string MediaURL { get; set; }
    }

    public class EDocument
    {
        [JsonProperty(PropertyName = "entityId")]
        public string EntityID { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "entityUri")]
        public string EntityUri { get; set; }
    }

    public class DownloadAttachment
    {
        public string loanGuid { get; set; }
        public string attachmentID { get; set; }
    }
}
