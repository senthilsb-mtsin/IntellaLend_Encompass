using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class RuleDocumentTables
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 DocumentID { get; set; }
        public string TableName { get; set; }
        public string TableColumnName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
