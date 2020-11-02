using System;
using System.Collections.Generic;

namespace IntellaLend.Model
{
    public class LoanAuditReportPDF
    {
        public List<ChecklistPDFTable> ChecklistDetails { get; set; }
        public Int64 LoanQCIndex { get; set; }
        public Int64 TotalCriticalDefectCount { get; set; }
        public Int64 FailedCriticalDefectCount { get; set; }
        public Int64 MissingDocumentCount { get; set; }
        public Int64 TotalDocumentCount { get; set; }
        public Int64 FailedRuleCount { get; set; }
        public Int64 TotalRuleCount { get; set; }
        public string ChecklistName { get; set; }
        public string completeNotes { get; set; }
        public string LenderName { get; set; }
        public string InvestorName { get; set; }
        public string LoanType { get; set; }
        public string LoanNumber { get; set; }
        public string LoanAmount { get; set; }
        public string PropertyAddress { get; set; }
    }

    public class ChecklistPDFTable
    {
        public string ChecklistCategory { get; set; }
        public string ChecklistDetailName { get; set; }
        public string ChecklistName { get; set; }
        public bool Result { get; set; }
        public Int32 Order { get; set; }
    }
}
