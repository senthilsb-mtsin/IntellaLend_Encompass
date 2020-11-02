using System;
using System.ComponentModel.DataAnnotations;

namespace IntellaLend.Model
{
    public class InvestorStipulation
    {
        [Key]
        public Int64 StipulationID { get; set; }
        public string StipulationCategory { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
