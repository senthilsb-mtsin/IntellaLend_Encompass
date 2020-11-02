using System;
using System.ComponentModel.DataAnnotations;

namespace DataImport
{
    public partial class PAGE
    {
        [Key]
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
