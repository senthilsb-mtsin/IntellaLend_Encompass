using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Reflection;

namespace IntellaLend.Model
{
    public class ReportModel
    {
        public Int64 CustomerID { get; set; }
        public Int64 DataEntryType { get; set; }
        public Int64 UserID { get; set; }
        public Int64 RoleID { get; set; }
        public DateTime AuditMontYear { get; set; }
        public string BarType { get; set; }
        public Int64 LoanID { get; set; }
        public int OCRReportType { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        [DefaultDateTimeValueAttribute("ToDate")]
        public string ToOCRDate { get; set; }
        [DefaultDateTimeValueAttribute("FromDate")]
        public string FromOCRDate { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public bool IsAuditMonthSearch { get; set; }
        [DefaultDateTimeValueAttribute("ToDate")]
        public string ToLoanInvestor { get; set; }
        [DefaultDateTimeValueAttribute("FromDate")]
        public string FromLoanInvestor { get; set; }
        public Int64 UserGroupID { get; set; }
        public bool Flag { get; set; }
        public string CategoryName { get; set; }
        public Int64 StipulationType { get; set; }
        public int RuleStatus { get; set; }

        public sealed class DefaultDateTimeValueAttribute : ValidationAttribute
        {
            private string memberName;
            public DefaultDateTimeValueAttribute(string DestinationMemberName)
            {
                memberName = DestinationMemberName;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null && value.GetType().Name.ToLower() == Type.GetType("System.String").Name.ToLower() && !string.IsNullOrEmpty((string)value) && !string.IsNullOrEmpty(memberName))
                {
                    PropertyInfo property = validationContext.ObjectType.GetProperty(memberName);
                    DateTime defaultValue = DateTime.ParseExact((string)value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    property.SetValue(validationContext.ObjectInstance, defaultValue);
                }
                return ValidationResult.Success;
            }
        }
    }

    public class ReportResultModel
    {
        public Int64 LoanID { get; set; }
        public string CustomerName { get; set; }
        public string LoanTypeName { get; set; }
        public string AuditMonthYear { get; set; }
        public string LoanNumber { get; set; }
        public string DataEntryName { get; set; }
        public bool OCRAccuracyCalculated { get; set; }
        public string OCRAccuracy { get; set; }
        public string ServiceTypeName { get; set; }
        public string StatusDescription { get; set; }
        public Int64 StatusID { get; set; }
        public Int64 NoofDaysAged { get; set; }
        public string ClassificationAccuracy { get; set; }
        public DateTime? ReviewCompletionDate { get; set; }
        public DateTime? ValidationCompletionDate { get; set; }
        public DateTime? AuditCompletionDate { get; set; }
        public string EphesoftReviewerName { get; set; }
        public string EphesoftValidatorName { get; set; }
        public string AuditorName { get; set; }
        public string EphesoftBatchInstanceID { get; set; }
        public string IDCReviewDuration { get; set; }
        public string IDCValidationDuration { get; set; }
        public string LoanDuration { get; set; }
        public Int64? PageCount { get; set; }
        public string MaxIDCReviewDuration { get; set; }
        public string MaxIDCValidationDuration { get; set; }
        public string MaxEphesoftReviewerName { get; set; }
        public string MaxEphesoftValidatorName { get; set; }
        public string LoanOfficer { get; set; }
        public string UnderWritter { get; set; }
        public string Closer { get; set; }
        public string PostCloser { get; set; }
        public string Investor { get; set; }
        public string MissingDocumentNames { get; set; }
        public Int64 FailedRulesCount { get; set; }
        public Int64 CriticalRulesCount { get; set; }
        public Int64 AssignUserID { get; set; }
        public string CategoryName { get; set; }
        public decimal LoanAmount { get; set; }
        public string BorrowerName { get; set; }
        public DateTime? LoanAuditCompleteDate { get; set; }
        public DateTime? ReceivedDate { get; set; }

    }

    public class DBReportResultModel
    {
        [Key]
        public Int64 LoanID { get; set; }
        public string CustomerName { get; set; }
        public string LoanTypeName { get; set; }
        public string LoanNumber { get; set; }
        public string OCRAccuracy { get; set; }
        public string ServiceTypeName { get; set; }
        public string ClassificationAccuracy { get; set; }
        public DateTime? ReviewCompletionDate { get; set; }
        public DateTime? ValidationCompletionDate { get; set; }
        public DateTime? AuditCompletionDate { get; set; }
        public string EphesoftReviewerName { get; set; }
        public string EphesoftValidatorName { get; set; }
        public string AuditorName { get; set; }
        public string EphesoftBatchInstanceID { get; set; }
        public string IDCReviewDuration { get; set; }
        public string IDCValidationDuration { get; set; }
        public string LoanDuration { get; set; }
        public bool OCRAccuracyCalculated { get; set; }
        public Int64? PageCount { get; set; }
        [NotMapped]
        public string MaxIDCReviewDuration { get; set; }
        [NotMapped]
        public string MaxIDCValidationDuration { get; set; }
        [NotMapped]
        public string MaxEphesoftReviewerName { get; set; }
        [NotMapped]
        public string MaxEphesoftValidatorName { get; set; }

    }


    public class ReportMasterData
    {
        public Int64 ReportMasterID { get; set; }
        public string ReportName { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public string ReviewTypeName { get; set; }
    }

    public class ReportMaster
    {
        [Key]
        public Int64 ReportMasterID { get; set; }
        public string ReportName { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class ReportConfig
    {
        [Key]
        public Int64 ReportID { get; set; }
        public Int64? ReportMasterID { get; set; }
        public string DocumentName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }



}
