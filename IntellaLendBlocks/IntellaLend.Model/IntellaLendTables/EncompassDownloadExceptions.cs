using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace IntellaLend.Model
{
    public class EncompassDownloadExceptions
    {
        [Key]
        public Int64 EncompassExceptionID { get; set; }
        public string EncompassLoanNumber { get; set; }
        public Guid? EncompassGuid { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public Int64 Status { get; set; }
        public Int64 RetryCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class EncompassExceptionModel
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
}
