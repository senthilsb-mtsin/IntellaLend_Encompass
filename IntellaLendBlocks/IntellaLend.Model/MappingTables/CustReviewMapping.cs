﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class CustReviewMapping
    {
        [Key]
        public Int64 CustReviewMappingID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public CustomerMaster CustomerMaster { get; set; }
        [NotMapped]
        public LoanTypeMaster LoanTypeMaster { get; set; }
        [NotMapped]
        public ReviewTypeMaster ReviewTypeMaster { get; set; }
    }
}
