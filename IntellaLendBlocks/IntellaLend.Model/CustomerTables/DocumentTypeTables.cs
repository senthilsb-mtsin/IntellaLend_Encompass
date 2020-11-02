using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class DocumetTypeTables
    {
        [Key]
        public Int64 TableID { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public string TableName { get; set; }
        public string TableJson { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
