using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditKpiGoalConfig
    {
        [Key]
        public Int64 AuditGoalID { get; set; }
        public Int64 UserGroupID { get; set; }
        public Int64 RoleID { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public Int64 Goal { get; set; }
        public int ConfigType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
