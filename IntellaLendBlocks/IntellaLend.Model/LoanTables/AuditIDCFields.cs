﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditIDCFields
    {
        [Key]
        public Int64 AuditID { get; set; }
        public string AuditDescription { get; set; }
        public DateTime? AuditDateTime { get; set; }
        public Int64 LoanID { get; set; }
        public string IDCBatchInstanceID { get; set; }
        public decimal? IDCOCRAccuracy { get; set; }
        public long IDCStatusID { get; set; }
        public decimal? IDCExtractionAccuracy { get; set; }
        public string IDCReviewerName { get; set; }
        public string IDCValidatorName { get; set; }
        public DateTime? IDCLevelOneCompletionDate { get; set; }
        public DateTime? IDCLevelTwoCompletionDate { get; set; }
        public DateTime? IDCCompletionDate { get; set; }
        public string IDCLevelOneDuration { get; set; }
        public string IDCLevelTwoDuration { get; set; }
        public long IDCFileRemovedStatus { get; set; }
        public bool? OCRAccuracyCalculated { get; set; }
        public decimal? ClassificationAccuracy { get; set; }
        public DateTime? Createdon { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string IDCBatchClassID { get; set; }
        public string IDCBatchClassName { get; set; }
    }

}
