using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class CustLoanDocMapping
    {
     

        [Key]
        public Int64 ID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public bool Active { get; set; }
        public Int32 DocumentLevel { get; set; }
        public string Condition { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public CustomerMaster CustomerMaster { get; set; }
        [NotMapped]
        public LoanTypeMaster LoanTypeMaster { get; set; }
        [NotMapped]
        public DocumentTypeMaster DocumentTypeMaster { get; set; }
    }

    public class DocMappingDetails
    {
        public Int64 DocumentTypeID { get; set; }
        public Int32 DocumentLevel { get; set; }
        public string Name { get; set; }
        public string Condition { get; set; }
        public List<DocumentFieldMaster> DocumentFieldMasters { get; set; }
        public List<DocumetTypeTables> DocumetTypeTables { get; set; }
        public List<RuleDocumentTables> RuleDocumentTables { get; set; }


    }


}
