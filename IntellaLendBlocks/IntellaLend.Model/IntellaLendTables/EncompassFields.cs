using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class EncompassFields
    {

        public Int64 ID { get; set; }
        public string FieldID { get; set; }
        public string FieldDesc { get; set; }
        public string FieldType { get; set; }
        public string DocumentType { get; set; }

    }
    public class LosFieldDetails
    {
        public string FieldID { get; set; }
        public string FieldDesc { get; set; }
    }

    public class EnDocumentType
    {
        public string DocumentTypeName { get; set; }
        public List<Int32> Pages { get; set; }
    }

    public class EncompassDocFields
    {
        public Int64 ID { get; set; }
        public string FieldId { get; set; }
        public string FieldIDDescription { get; set; }
    }


    public class LoanTapeDocFields
    {
        public string FieldId { get; set; }
        public string FieldIDDescription { get; set; }
    }

    public class LoanTapeFieldDefinition
    {
        public string FieldID { get; set; }
        public string DataStream { get; set; }
    }
    public class LOSLoanTapeFields
    {

        public Int64 ID { get; set; }
        public string FieldID { get; set; }
        public string FieldDesc { get; set; }
        public string FieldType { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class LoanTapeDefinition
    {
        [Key]
        public Int64 ID { get; set; }
        public string FieldID { get; set; }
        public string DataStream { get; set; }
        public string Position { get; set; }
        public string FieldLength { get; set; }
        public string FieldInformation { get; set; }
        public string Occurrences { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
