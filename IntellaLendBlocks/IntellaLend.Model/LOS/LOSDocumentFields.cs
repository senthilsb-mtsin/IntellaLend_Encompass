using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LOSDocumentFields
    {

        [Key]
        public Int64 LOSDocumentFieldID { get; set; }
        public Int64 LOSDocumentID { get; set; }
        public string FieldID { get; set; }
        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }
        public Int64 FieldPosition { get; set; }
        public Int64 FieldLength { get; set; }
        public string FieldOccurrences { get; set; }
        public string FieldValueCode { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime ModifiedOn { get; set; }
    }


}
