//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTSIDCReportService
{
    using System;
    using System.Collections.Generic;
    
    public partial class BATCH
    {
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
