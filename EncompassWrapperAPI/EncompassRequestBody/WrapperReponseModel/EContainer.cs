using EncompassRequestBody.EResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EncompassRequestBody.WrapperReponseModel
{
    public class EContainer
    {
        [JsonProperty(PropertyName = "documentId")]
        public string DocumentId { get; set; }

        [JsonProperty(PropertyName = "titleWithIndex")]
        public string TitleWithIndex { get; set; }

        [JsonProperty(PropertyName = "applicationName")]
        public string ApplicationName { get; set; }

        [JsonProperty(PropertyName = "milestoneId")]
        public string MilestoneId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public List<EComments> Comments { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public List<EDocument> Attachments { get; set; }

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
