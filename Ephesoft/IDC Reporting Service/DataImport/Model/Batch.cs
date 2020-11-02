using System;
using System.ComponentModel.DataAnnotations;

namespace DataImport
{
    public partial class BATCH
    {
        [Key]
        public int BATCH_ID { get; set; }
        public string BATCH_INSTANCEID { get; set; }
        public string BATCH_NAME { get; set; }
        public Nullable<System.DateTime> CREATEDATE { get; set; }
        public Nullable<System.DateTime> LASTMODIFIEDDATE { get; set; }
        public string BATCHCLASS_ID { get; set; }
        public string BATCHCLASS_NAME { get; set; }
        public string STATUS { get; set; }
        public string REVIEW_OPERATOR { get; set; }
        public string VALIDATION_OPERATOR { get; set; }
        public string CLASSIFICATION_TYPE { get; set; }
    }
}
