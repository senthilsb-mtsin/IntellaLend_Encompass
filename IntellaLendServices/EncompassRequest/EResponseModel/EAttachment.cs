using Newtonsoft.Json;
using System;
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
        public Int64 AttachmentType { get; set; }

        [JsonProperty(PropertyName = "fileSize")]
        public Int64 FileSize { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "isRemoved")]
        public bool IsRemoved { get; set; }

        [JsonProperty(PropertyName = "rotation")]
        public Int64 Rotation { get; set; }

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



    //public class UploadAttachment
    //{
    //    [JsonProperty(PropertyName = "entityId")]
    //    public string EntityId { get; set; }

    //    [JsonProperty(PropertyName = "entityType")]
    //    public string EntityType { get; set; }
    //}

    public class EComment
    {
        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "commentId")]
        public string CommentId { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdByName")]
        public string CreatedByName { get; set; }

        [JsonProperty(PropertyName = "reviewedBy")]
        public string ReviewedBy { get; set; }
    }

    public class EDocAttachment
    {
        [JsonProperty(PropertyName = "entityID")]
        public string EntityID { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "entityUri")]
        public string EntityUri { get; set; }

    }

    //    [
    //  {
    //    "documentId": "string",
    //    "titleWithIndex": "string",
    //    "applicationName": "string",
    //    "milestoneId": "string",
    //    "title": "string",
    //    "status": "string",
    //    "comments": [
    //      {
    //        "comments": "string",
    //        "commentId": "string",
    //        "dateCreated": "2020-04-16T11:02:11.855Z",
    //        "createdBy": "string",
    //        "createdByName": "string",
    //        "reviewedBy": "string"
    //      }
    //    ],
    //    "attachments": [
    //      {
    //        "entityID": "string",
    //        "entityType": "string",
    //        "entityName": "string",
    //        "entityUri": "string"
    //      }
    //    ]
    //  }
    //]

    public class ELoanDocument
    {
        [JsonProperty(PropertyName = "documentId")]
        public string DocumentID { get; set; }

        [JsonProperty(PropertyName = "titleWithIndex")]
        public string TitleWithIndex { get; set; }

        [JsonProperty(PropertyName = "applicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty(PropertyName = "milestoneId")]
        public string MilestoneID { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public List<EComment> Comments { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public List<EDocAttachment> Attachments { get; set; }

    }
}
