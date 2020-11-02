﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditLoanDetail
    {
        [Key]
        public Int64 AuditID { get; set; }
        public string AuditDescription { get; set; }
        public string SystemAuditDescription { get; set; }
        public DateTime AuditDateTime { get; set; }
        public Int64 LoanDetailID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 UpdatedUserID { get; set; }
        public string LoanObject { get; set; }
        public string ManualQuestioners { get; set; }
        public Int64 TotalDocCount { get; set; }
        public Int64 MissingDocCount { get; set; }
        public Int64 MissingCriticalDocCount { get; set; }
        public Int64 MissingNonCriticalDocCount { get; set; }
    }
}
