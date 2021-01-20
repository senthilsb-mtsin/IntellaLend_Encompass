using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.LoanStatusUpdate
{
    public class LoanStatusUpdateDataAccess
    {
        public static string SystemSchema = "IL";
        public static string TenantSchema;

        public LoanStatusUpdateDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }
        public static List<TenantMaster> GetAllTenants()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }

        public List<PendingOCRBatches> GetPendingIDCLoans()
        {
            List<PendingOCRBatches> _loans = null;
            using (var db = new DBConnect(TenantSchema))
            {
                List<Loan> _loan = db.Loan.AsNoTracking().Where(m => m.Status == StatusConstant.PENDING_IDC && m.SubStatus != StatusConstant.IDC_DELETED && m.SubStatus != StatusConstant.EXTRACTION_COMPLETED).ToList();
                List<AuditLoanMissingDoc> _aLoan = db.AuditLoanMissingDoc.AsNoTracking().Where(m => m.Status != StatusConstant.IDC_DELETED && m.Status != StatusConstant.EXTRACTION_COMPLETED && m.Status != StatusConstant.IDC_COMPLETE).ToList();
                _loans = new List<PendingOCRBatches>();
                foreach (var item in _loan)
                {
                    string _batchIdentifier = db.IDCFields.AsNoTracking().Where(i => i.LoanID == item.LoanID).Select(l => l.IDCBatchInstanceID).FirstOrDefault();
                    _loans.Add(new PendingOCRBatches()
                    {
                        LoanID = item.LoanID,
                        BatchInstancesID = (!(string.IsNullOrEmpty(_batchIdentifier))) ? _batchIdentifier : string.Empty,
                        DocID = 0
                    });
                }

                foreach (var item in _aLoan)
                {

                    if (item.DocID == 0 && item.EDownloadStagingID != 0)
                    {
                        string[] data = item.FileName.Split('_');
                        if (data.Length > 2)
                            item.DocID = Convert.ToInt64(data[2].Split('.')[0]);
                    }

                    _loans.Add(new PendingOCRBatches()
                    {
                        LoanID = item.LoanID,
                        BatchInstancesID = item.IDCBatchInstanceID,
                        DocID = item.DocID
                    });
                }

                return _loans;
            }
        }

        public string GetBatchIdentifier(long loanId)
        {
            string _batchIdentifier = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                _batchIdentifier = db.IDCFields.AsNoTracking().Where(i => i.LoanID == loanId).Select(l => l.IDCBatchInstanceID).FirstOrDefault();
                if (!(string.IsNullOrEmpty(_batchIdentifier)))
                    return _batchIdentifier;
                else
                    return "";
            }
        }

        public bool CheckFileNotCreated(Int64 LoanID, Int32 FileType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSExportFileStaging _stage = db.LOSExportFileStaging.AsNoTracking().Where(l => l.LoanID == LoanID && l.FileType == FileType).FirstOrDefault();

                return _stage == null;
            }
        }

        public void UpdateLoanSubStatus(long loanId, int subStatus, string loanStatusDesc, Int64 DocID, ref bool insert)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                if (DocID == 0)
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanId).FirstOrDefault();

                    if (_loan != null && _loan.SubStatus != subStatus)
                    {
                        _loan.SubStatus = subStatus;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc), auditDescs[1].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc));
                        insert = true;
                    }
                }
                else
                {
                    string fileName = $"{TenantSchema.ToUpper()}_{loanId.ToString()}_{DocID.ToString()}";

                    AuditLoanMissingDoc _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loanId && l.DocID == DocID).FirstOrDefault();

                    if (_loan == null)
                        _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loanId && l.FileName.Contains(fileName)).FirstOrDefault();

                    if (_loan != null && _loan.Status != subStatus)
                    {
                        _loan.Status = subStatus;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                        insert = true;
                        //string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                        //LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc), auditDescs[1].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc));

                    }
                }
            }

        }


        public void UpdateLoanStatus(Int64 LoanID, Int64 status, Int32 errorCode, string loanStatusDesc, Int64 DocID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                if (DocID == 0)
                {
                    Loan _loan = db.Loan.Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (_loan != null && _loan.SubStatus != errorCode)
                    {
                        _loan.SubStatus = errorCode;
                        _loan.Status = status;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc), auditDescs[1].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc));
                    }
                }
                else
                {
                    AuditLoanMissingDoc _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocID == DocID).FirstOrDefault();

                    if (_loan != null && _loan.Status != errorCode)
                    {
                        _loan.Status = errorCode;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

    }
}
