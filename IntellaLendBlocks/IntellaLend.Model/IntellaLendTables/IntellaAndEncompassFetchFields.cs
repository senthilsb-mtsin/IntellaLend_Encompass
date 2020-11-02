using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class IntellaAndEncompassFetchFields
    {
        [Key]
        public Int64 ID { get; set; }
        public string FieldType { get; set; }
        public string EncompassFieldID { get; set; }
        public string EncompassFieldDescription { get; set; }
        public string EncompassFieldValue { get; set; }
        public string IntellaMappingValue { get; set; }
        public string IntellaMappingColumn { get; set; }
        public string Notes { get; set; }
        public bool Active { get; set; }
        public bool IsSingleValue { get; set; }
        public string UpdateFieldValue { get; set; }
        public string EFetchFieldID { get; set; }
        public string TenantSchema { get; set; }
    }
}
