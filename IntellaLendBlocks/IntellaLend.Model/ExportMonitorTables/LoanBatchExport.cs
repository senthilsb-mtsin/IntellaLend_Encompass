using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace IntellaLend.Model
{
    public class LoanJobExport
    {
        [Key]
        public Int64 JobID { get; set; }
        public string JobName { get; set; }
        public Int64 LoanCount { get; set; }
        public string ExportPath { get; set; }
        public Boolean CoverLetter { get; set; }
        public Boolean TableOfContent { get; set; }
        public string Password { get; set; }
        public string CoverLetterContent { get; set; }
        public Int64 Status { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorStackTrace { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Boolean PasswordProtected { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ExportedBy { get; set; }
    }
    public class Exportmodel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [DefaultDateTimeValueAttribute("ToDate")]
        public string ToDateStr { get; set; }
        [DefaultDateTimeValueAttribute("FromDate")]
        public string FromDateStr { get; set; }
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
                    System.Reflection.PropertyInfo property = validationContext.ObjectType.GetProperty(memberName);
                    DateTime defaultValue = DateTime.ParseExact((string)value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    property.SetValue(validationContext.ObjectInstance, defaultValue);
                }
                return ValidationResult.Success;
            }
        }
    }

    public class BatchLoanDetail
    {
        public Int64 LoanID { get; set; }
        public string Documents { get; set; }
    }
}
