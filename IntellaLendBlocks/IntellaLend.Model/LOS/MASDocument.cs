using System.Collections.Generic;

namespace IntellaLend.Model
{

    public class MASDocument
    {
        public string DocumentID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentDesc { get; set; }
    }

    public partial class MASBaseData
    {
        public string LenderCode { get; set; }
        public string LenderName { get; set; }
        public string LoanID { get; set; }
        public string LoanType { get; set; }
        public string ServiceType { get; set; }
        public int Priority { get; set; }
        public string AuditPeriod { get; set; }
        public string AuditDueDate { get; set; }
        public string IDCBatchID { get; set; }
        public string IDCBatchName { get; set; }
        public string IDCBC { get; set; }
        public string IDCBCName { get; set; }
        public string Event { get; set; }
        public string EventDescription { get; set; }
        public string URL { get; set; }
    }

    public partial class MASData : MASBaseData
    {
        public string ReviewBy { get; set; }
        public string ReviewOn { get; set; }
        public string ReviewDuration { get; set; }
        public List<MASDocument> Documents { get; set; }

    }
}
