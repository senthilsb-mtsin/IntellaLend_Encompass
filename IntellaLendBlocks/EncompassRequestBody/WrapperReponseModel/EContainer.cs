using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EContainer
    {
        [JsonProperty(PropertyName = "id")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "statusDate")]
        public DateTime StatusDate { get; set; }

        [JsonProperty(PropertyName = "receivedDate")]
        public DateTime ReceivedDate { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "isProtected")]
        public bool IsProtected { get; set; }

        [JsonProperty(PropertyName = "isMarkedRemoved")]
        public bool IsMarkedRemoved { get; set; }

        [JsonProperty(PropertyName = "daysTillExpire")]
        public Int32 DaysTillExpire { get; set; }

        [JsonProperty(PropertyName = "milestone")]
        public EDocumentMileStone Milestone { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public List<EDocumentAttachment> Attachments { get; set; }

    }

    public class EDocumentAttachment
    {

        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "isActive")]
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

    }

    public class EDocumentMileStone
    {

        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

    }

    public class EComments
    {
        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "commentId")]
        public string CommentId { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdByName")]
        public string CreatedByName { get; set; }

        [JsonProperty(PropertyName = "reviewedBy")]
        public string ReviewedBy { get; set; }
    }
}
