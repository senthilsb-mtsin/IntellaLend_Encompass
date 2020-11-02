using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class SystemReviewTypeMaster
    {
        [Key]
        public Int64 ReviewTypeID { get; set; }
        public string ReviewTypeName { get; set; }
        public Int32 Type { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? ReviewTypePriority { get; set; }
        public string BatchClassInputPath { get; set; }
        public string SearchCriteria { get; set; }
        public Int64? UserRoleID { get; set; }
    }
}
