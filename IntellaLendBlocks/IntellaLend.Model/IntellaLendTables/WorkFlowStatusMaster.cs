using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class WorkFlowStatusMaster
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 StatusID { get; set; }
        public string StatusDescription { get; set; }
    }
}
