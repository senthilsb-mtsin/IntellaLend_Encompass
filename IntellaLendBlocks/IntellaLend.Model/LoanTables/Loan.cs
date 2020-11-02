using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntellaLend.Model
{
    public class Loan
    {
        //public Loan()
        //{
        //    this.LoanDetails = new HashSet<LoanDetail>();
        //    this.LoanImages = new HashSet<LoanImage>();
        //}

        [Key]
        public Int64 LoanID { get; set; }
        public string LoanNumber { get; set; }
        public string CompleteNotes { get; set; }
        public Int64 UploadedUserID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 LoggedUserID { get; set; }
        public Int64 Status { get; set; }
        public int SubStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string FileName { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LastAccessedUserID { get; set; }
        public string Notes { get; set; }
        public Int64 CompletedUserID { get; set; }
        public Int64 CompletedUserRoleID { get; set; }
        //public bool FromBox { get; set; }
        public DateTime AuditMonthYear { get; set; }
        public DateTime? QCIQStartDate { get; set; }
        public string LoanDuration { get; set; }
        public Int64? PageCount { get; set; }
        public Int64 Priority { get; set; }
        public int UploadType { get; set; }
        public Int64 AssignedUserID { get; set; }
        public string EphesoftBatchInstanceID { get; set; }
        public DateTime? AuditCompletedDate { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid? LoanGUID { get; set; }
        public Guid? EnCompassLoanGUID { get; set; }
        [NotMapped]
        public LoanDetail LoanDetails { get; set; }
        [NotMapped]
        public LoanImage LoanImages { get; set; }
        [NotMapped]
        public LoanSearch LoanSearch { get; set; }
        [NotMapped]
        public List<AuditLoan> AuditLoan { get; set; }
        //public virtual CustomerMaster CustomerDetails { get; set; }
        //public virtual User User { get; set; }
        //public virtual ReviewTypeMaster ReviewTypeMaster { get; set; }
        //public virtual LoanTypeMaster LoanTypeMaster { get; set; }
        //public virtual User LoggedUser { get; set; }
    }

    public class ChecklistObject
    {
        public string CheckListName { get; set; }
        public string Expression { get; set; }
        public string Formula { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
    }

    public class QCIQLoanDeails
    {
        public string ConnectionString { get; set; }
        public string MasterConnectionString { get; set; }
        public string MasterSQLScript { get; set; }
        public string SQLScript { get; set; }
        public string CustomerName { get; set; }
        public string ReviewTypeName { get; set; }
    }
    public class GetLoanBatch
    {
        public Int64 LoanId { get; set; }
        public string IDCBatchInstanceID { get; set; }
    }
}
