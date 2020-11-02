using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class AuditUserKpiGoalConfig
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 UserID { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public Int64 Goal { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Int64? UserGroupID { get; set; }
    }
}


