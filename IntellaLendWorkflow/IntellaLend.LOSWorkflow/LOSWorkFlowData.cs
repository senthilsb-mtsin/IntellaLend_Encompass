using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.LOSWorkflow
{
    class LOSWorkFlowData
    {
        private static string TenantSchema { get; set; }
        private static string SystemSchema = "IL";

        public LOSWorkFlowData(string TableSchema)
        {
            TenantSchema = TableSchema;
        }

        public Object GetQCIQLookupDetails(Int64 loanID)
        {
            object returnItem = null;
            using (var db = new DBConnect(TenantSchema))
            {
                returnItem = (from l in db.Loan.AsNoTracking()
                              join conn in db.QCIQConnectionString.AsNoTracking().DefaultIfEmpty() on l.ReviewTypeID equals conn.ReviewTypeID
                              join connm in db.QCIQConnectionString.AsNoTracking().DefaultIfEmpty() on 0 equals connm.ReviewTypeID
                              join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                              join rev in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals rev.ReviewTypeID
                              where l.LoanID == loanID
                              select new
                              {
                                  ConnectionString = conn.ConnectIonString,
                                  MasterConnectionString = connm.ConnectIonString,
                                  MasterSQLScript = connm.SQLScript,
                                  SQLScript = conn.SQLScript,
                                  CustomerName = cus.CustomerName,
                                  ReviewTypeName = rev.ReviewTypeName

                              }).FirstOrDefault();
            }

            return returnItem;
        }

        public bool QCIQEnabled()
        {

            bool result = false;
            using (var db = new DBConnect(SystemSchema))
            {
                AppConfig config = db.AppConfig.AsNoTracking().Where(c => c.ConfigKey == ConfigConstant.QCIQSTARTDATEENABLED).FirstOrDefault();
                bool.TryParse(config.ConfigValue, out result);
            }
            return result;

        }

        public Dictionary<string, object> GetLoanNumber(Int64 loanID)
        {
            Loan loan = null;
            string loanNumber = string.Empty;
            Dictionary<string, object> result = new Dictionary<string, object> { { "loanNumber", "" }, { "startDate", DateTime.Now } };
            using (var db = new DBConnect(TenantSchema))
            {

                loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
            }
            if (loan != null)
            {
                result["loanNumber"] = loan.LoanNumber;
                result["startDate"] = loan.CreatedOn;
            }

            return result;
        }

        public System.Data.DataSet GetQCIQData(string connectIonString, string sqlScript)
        {
            return DynamicDataAccess.ExecuteSQLDataSet(connectIonString, sqlScript);
        }

        public void UpdateQCIQStartDate(DateTime? QCIQStartDate, Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                _loan.QCIQStartDate = QCIQStartDate;

                db.Entry(_loan).State = EntityState.Modified;
                db.SaveChanges();
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPDATED_LOAN_TYPE_FROM_QCIQ);
                LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], "QCIQ Start Date Updated");

            }
        }

        public void UpdateLoanCompleteUserDetails(Int64 LoanID, Int64 completeUserRoleID, Int64 completeUserID, string CompleteNotes)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                _loan.CompletedUserID = completeUserID;
                _loan.CompletedUserRoleID = completeUserRoleID;
                _loan.CompleteNotes = CompleteNotes;
                _loan.Status = StatusConstant.LOS_LOAN_STAGED;
                _loan.ModifiedOn = DateTime.Now;
                _loan.AuditCompletedDate = DateTime.Now;
                db.Entry(_loan).State = EntityState.Modified;
                db.SaveChanges();

                LoanAudit.InsertLoanAudit(db, _loan, "Waiting for LOS Export", "Waiting for LOS Export");

                LoanSearch loanSearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (loanSearch != null)
                {
                    loanSearch.Status = _loan.Status;
                    loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(loanSearch).State = EntityState.Modified;
                    db.SaveChanges();

                    string[] auditDescsearch = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_STATUS_MODIFIED);

                    LoanAudit.InsertLoanSearchAudit(db, loanSearch, auditDescsearch[0], auditDescsearch[1]);
                }
            }


        }

        public void InsertLOSLoanExport(Int64 LoanID, string loanNumber)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSExportFileStaging _lStage = new LOSExportFileStaging()
                {
                    LoanID = LoanID,
                    FileType = LOSExportFileTypeConstant.LOS_LOAN_EXPORT,
                    Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                    FileName = $"{loanNumber}_IL_Export_{DateTime.Now.ToString("yyyyMMddhhmmssfff")}.json",
                    FileJson = string.Empty,
                    ErrorMsg = string.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                };

                db.LOSExportFileStaging.Add(_lStage);
                db.SaveChanges();
            }
        }
    }
}
