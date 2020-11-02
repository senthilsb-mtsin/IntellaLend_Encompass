using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class DocumentTables
    {
        [Key]
        public Int64 ID { get; set; }
        public string DocumentTypeName { get; set; }
        public string Tables { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class DocTableJOBJECT
    {
        public string TableName { get; set; }
        public List<ColumnDesignation> Columns { get; set; }
    }

    public class ColumnDesignation
    {
        public string ColumnName { get; set; }
        public string DestinationField { get; set; }
    }

}
