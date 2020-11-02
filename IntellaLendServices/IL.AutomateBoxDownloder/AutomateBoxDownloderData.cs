using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.AutomateBoxDownloder
{
    public class AutomateBoxDownloderData
    {

        #region Private Variables

        private static int MaxRetryCount = 0;

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public AutomateBoxDownloderData(string tenantSchema, int maxRetryCount)
        {
            TenantSchema = tenantSchema;
            MaxRetryCount = maxRetryCount;
        }

        #endregion

        #region Public Methods

        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }

        public List<LoanTapeDefinition> GetLoanTapeDefinition()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.LoanTapeDefinitions.AsNoTracking().ToList();
            }
        }

        public void RemoveLoanEntries(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                if (db.Loan.Where(l => l.LoanID == LoanID).FirstOrDefault() != null)
                {
                    db.AuditLoan.RemoveRange(db.AuditLoan.Where(l => l.LoanID == LoanID));
                    db.SaveChanges();

                    db.LoanLOSFields.RemoveRange(db.LoanLOSFields.Where(l => l.LoanID == LoanID));
                    db.SaveChanges();

                    db.LoanSearch.RemoveRange(db.LoanSearch.Where(l => l.LoanID == LoanID));
                    db.SaveChanges();

                    db.AuditLoanSearch.RemoveRange(db.AuditLoanSearch.Where(l => l.LoanID == LoanID));
                    db.SaveChanges();

                    db.Loan.RemoveRange(db.Loan.Where(l => l.LoanID == LoanID));
                    db.SaveChanges();
                }
            }
        }

        //public List<CustomerMaster> GetBoxCustomers()
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        return db.CustomerMaster.Where(m => m.BoxFolderName != "" && m.Active == true).ToList();
        //    }
        //}

        public bool AddBoxFileUploadDetails(Loan loan)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    loan.SubStatus = 0;
                    loan.LoggedUserID = 0;
                    loan.FileName = string.Empty;
                    loan.CreatedOn = DateTime.Now;
                    loan.ModifiedOn = DateTime.Now;
                    loan.Status = StatusConstant.PENDING_BOX_DOWNLOAD;
                    db.Loan.Add(loan);
                    db.SaveChanges();

                    IDCFields _idcObj = new IDCFields() { LoanID = loan.LoanID, Createdon = DateTime.Now, ModifiedOn = DateTime.Now, OCRAccuracyCalculated = false };

                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_BOX);
                    LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);

                    LoanSearch _loansearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == loan.LoanID).FirstOrDefault();
                    if (_loansearch == null)
                    {
                        _loansearch = new LoanSearch();
                        _loansearch.AuditDueDate = loan.AuditMonthYear;
                        _loansearch.CreatedOn = DateTime.Now;
                        _loansearch.LoanID = loan.LoanID;
                        db.LoanSearch.Add(_loansearch);
                        db.SaveChanges();
                    }
                    else
                    {
                        _loansearch.AuditDueDate = loan.AuditMonthYear;
                        _loansearch.ModifiedOn = DateTime.Now;
                        _loansearch.LoanID = loan.LoanID;
                        db.Entry(_loansearch).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    tran.Commit();
                }
            }
            return true;
        }

        public List<CustReviewLoanUploadPath> GetBoxFolderPath()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanUploadPath.Where(m => m.BoxUploadPath != "" && m.Active == true).ToList();
            }
        }

        public void RevertLoanInsert(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (LoanID > 0)
                    {
                        db.AuditLoanDetail.RemoveRange(db.AuditLoanDetail.Where(x => x.LoanID == LoanID));
                        db.AuditLoanMissingDoc.RemoveRange(db.AuditLoanMissingDoc.Where(x => x.LoanID == LoanID));
                        db.AuditLoanSearch.RemoveRange(db.AuditLoanSearch.Where(x => x.LoanID == LoanID));
                        db.LoanDetail.RemoveRange(db.LoanDetail.Where(x => x.LoanID == LoanID));
                        db.LoanPDF.RemoveRange(db.LoanPDF.Where(x => x.LoanID == LoanID));
                        db.LoanReverification.RemoveRange(db.LoanReverification.Where(x => x.LoanID == LoanID));
                        db.LoanSearch.RemoveRange(db.LoanSearch.Where(x => x.LoanID == LoanID));
                        db.LoanImage.RemoveRange(db.LoanImage.Where(x => x.LoanID == LoanID));
                        db.Loan.RemoveRange(db.Loan.Where(x => x.LoanID == LoanID));
                        db.SaveChanges();
                    }
                    tran.Commit();
                }
            }
        }

        public List<LoanTypeMaster> GetCustomerLoanTypes(Int64 CustomerID, Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanTypeMaster> lsLoanTypes = new List<LoanTypeMaster>();
                List<CustReviewLoanMapping> lMappings = db.CustReviewLoanMapping.Where(m => m.CustomerID == CustomerID && m.ReviewTypeID == ReviewTypeID && m.Active).ToList();

                foreach (CustReviewLoanMapping item in lMappings)
                {
                    LoanTypeMaster lm = db.LoanTypeMaster.Where(m => m.LoanTypeID == item.LoanTypeID).FirstOrDefault();

                    if (lm != null)
                        lsLoanTypes.Add(lm);
                }
                return lsLoanTypes;
            }
        }

        public Loan InsertLoan(Loan loan)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.Loan.Add(loan);
                    db.SaveChanges();

                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_BOX);
                    LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);

                    LoanSearch _loansearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == loan.LoanID).FirstOrDefault();
                    if (_loansearch == null)
                    {
                        _loansearch.AuditDueDate = loan.AuditMonthYear;
                        _loansearch.CreatedOn = DateTime.Now;
                        _loansearch.LoanID = loan.LoanID;
                        db.LoanSearch.Add(_loansearch);
                        db.SaveChanges();
                    }
                    else
                    {
                        _loansearch.AuditDueDate = loan.AuditMonthYear;
                        _loansearch.ModifiedOn = DateTime.Now;
                        _loansearch.LoanID = loan.LoanID;
                        db.Entry(_loansearch).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_BOX);
                    LoanAudit.InsertLoanSearchAudit(db, _loansearch, auditDescs[0], auditDescs[1]);

                    tran.Commit();

                    return loan;
                }
            }
        }

        public Int64 GetReviewTypePriority(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster rm = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == ReviewTypeID).FirstOrDefault();

                if (rm != null)
                    return rm.ReviewTypePriority.Value;

                return 4;
            }
        }

        #endregion
    }
}
