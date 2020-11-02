using System;
using System.Collections.Generic;

namespace IntellaLend.Model
{
    public class LoanStipulationDetails
    {
        public Int64 ID { get; set; }
        public Int64 StipulationCategoryID { get; set; }
        public string StipulationDescription { get; set; }
        public Int64 StipulationStatus { get; set; }
        public string StipulationNotes { get; set; }
    }

    public class LoanInvestorStipulationDetails
    {
        public Int64 StipulationID { get; set; }
        public Int64 StipulationCategoryID { get; set; }
        public string StipulationCategoryName { get; set; }
        public List<LoanStipulationDetails> loanStipulation { get; set; }
    }
}
