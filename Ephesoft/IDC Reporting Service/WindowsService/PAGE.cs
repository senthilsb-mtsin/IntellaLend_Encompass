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
    
    public partial class PAGE
    {
        public int PK { get; set; }
        public string PAGEID { get; set; }
        public Nullable<decimal> PAGE_CONFIDENCE { get; set; }
        public string LEARNED_FILENAME { get; set; }
        public string PAGE_POSITION { get; set; }
        public string DOCID { get; set; }
        public string DOCTYPE { get; set; }
        public Nullable<decimal> DOC_CONFIDENCE { get; set; }
        public Nullable<decimal> CONFIDENCE_THRESHOLD { get; set; }
        public Nullable<bool> VALID { get; set; }
        public Nullable<bool> REVIEWED { get; set; }
        public string BATCH_INSTANCEID { get; set; }
        public string PATTERN { get; set; }
    }
}
