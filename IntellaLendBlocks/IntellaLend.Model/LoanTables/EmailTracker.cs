﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace IntellaLend.Model
{
    public class EmailTracker
    {
        [Key]
        public long ID { get; set; }
        public string To { get; set; }
        public string SendBy { get; set; }
        public string Subject { get; set; }
        public string Attachments { get; set; }
        public string Body { get; set; }
        public long UserID { get; set; }
        public int Delivered { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long LoanID { get; set; }
        public string Parameters { get; set; }
        public Int32 TemplateID { get; set; }
        public string ErrorMessage { get; set; }
        public string AttachmentsName { get; set; }
    }
    public class EmailtrackerModel
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
