using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class LOSDocument
    {

        [Key]
        public Int64 LOSDocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentDisplayName { get; set; }
        public string Version { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime ModifiedOn { get; set; }
    }


}
