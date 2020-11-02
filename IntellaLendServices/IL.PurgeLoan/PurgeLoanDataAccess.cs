using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
namespace IL.PurgeLoan
{
    public class PurgeLoanDataAccess
    {
        #region Private Variables

        //private static int MaxRetryCount = 0;

        public string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public PurgeLoanDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods 

        public static List<TenantMaster> GetAllTenants()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }

        public List<PurgeStaging> GetAllBatchIDS()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.PurgeStaging.AsNoTracking().Where(ps => ps.Status == StatusConstant.PURGE_WAITING || ps.Status == StatusConstant.EXPORT_WAITING).ToList();
            }
        }

        public List<GetLoanBatch> GetAllAuditCompleteBatchIDS()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return (from L in db.Loan.AsNoTracking()
                        join idf in db.IDCFields.AsNoTracking() on L.LoanID equals idf.LoanID into idcfld
                        from IDC in idcfld.DefaultIfEmpty()
                        where L.Status == StatusConstant.COMPLETE && L.Status == StatusConstant.DELETE_FILE_READY
                        && IDC.IDCBatchInstanceID != ""
                        select new GetLoanBatch
                        {
                            LoanId = L.LoanID,
                            IDCBatchInstanceID = IDC.IDCBatchInstanceID,
                        }).ToList();

                // return db.Loan.AsNoTracking().Where(ln => ln.Status == StatusConstant.COMPLETE && ln.Status == StatusConstant.DELETE_FILE_READY && ln.EphesoftBatchInstanceID != "").ToList();
            }
        }

        public bool LoanPurged(Int64 _loanID, Int64 loanStatus)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == _loanID).FirstOrDefault();
                if (loan != null)
                {
                    loan.Status = loanStatus;
                    loan.ModifiedOn = DateTime.Now;
                    db.Entry(loan).State = EntityState.Modified;
                    db.SaveChanges();
                    if (loanStatus == StatusConstant.LOAN_EXPORTED)
                    {
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_EXPORTED);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);
                    }

                }
                return true;
            }
            return false;
        }

        public bool LoanSearchPurged(Int64 _loanID, Int64 loanStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == _loanID).FirstOrDefault();
                if (_loanSearch != null)
                {
                    _loanSearch.Status = loanStatus;
                    _loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loanSearch).State = EntityState.Modified;
                    db.SaveChanges();
                    if (loanStatus == StatusConstant.LOAN_EXPORTED)
                    {
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_EXPORTED);
                        LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditDescs[0], auditDescs[1]);
                    }
                }
                return true;
            }
            return false;
        }

        public string GetReviewTypeName(long reviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster _reviewType = db.ReviewTypeMaster.AsNoTracking().Where(rt => rt.ReviewTypeID == reviewTypeID).FirstOrDefault();
                if (_reviewType != null)
                    return _reviewType.ReviewTypeName;
                else
                    return string.Empty;
            }
        }

        public string GetLoanTypeName(long loanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(lt => lt.LoanTypeID == loanTypeID).FirstOrDefault();
                if (_loanType != null)
                    return _loanType.LoanTypeName;
                else
                    return string.Empty;
            }
        }

        public string GetCustomerName(long customerID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerMaster _custMaster = db.CustomerMaster.AsNoTracking().Where(cm => cm.CustomerID == customerID).FirstOrDefault();

                if (_custMaster != null)
                    return _custMaster.CustomerName;
                else
                    return string.Empty;
            }
        }

        public LoanSearch GetLoanSearchDetails(long loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.LoanSearch.AsNoTracking().Where(lsd => lsd.LoanID == loanID).FirstOrDefault();
            }
        }

        public string GetPDFPath(long loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF _loanPDF = db.LoanPDF.AsNoTracking().Where(lp => lp.LoanID == loanID).FirstOrDefault();

                if (_loanPDF != null)
                    return _loanPDF.LoanPDFPath;
                else
                    return string.Empty;
            }
        }

        public string GetExportPath(long customerID, string exportConfigKey)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerConfig _custConfig = db.CustomerConfig.AsNoTracking().Where(cc => cc.CustomerID == customerID && cc.ConfigKey == exportConfigKey).FirstOrDefault();

                if (_custConfig != null)
                    return _custConfig.ConfigValue;
                else
                    return string.Empty;
            }
        }

        public byte[] GetPDFBytes(long loanID)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);

            return _imageWrapper.GetLoanPDF(loanID);
        }

        public void DeleteLoanPDF(long loanID)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
            _imageWrapper.DeleteLoanStackingOrder(loanID);
            DeleteLoanPDFEntry(loanID);
        }

        public void DeleteLoanPDFEntry(long loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF _pdf = db.LoanPDF.AsNoTracking().Where(li => li.LoanID == loanID).FirstOrDefault();

                if (_pdf != null)
                {
                    db.Entry(_pdf).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        public bool DeleteLoanImages(long loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                //using(var trans = db.Database.BeginTransaction())
                //{
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                List<LoanImage> loanImgs = db.LoanImage.AsNoTracking().Where(li => li.LoanID == loanID).ToList();
                foreach (LoanImage img in loanImgs)
                    _imageWrapper.DeleteLoanImage(loanID, img.ImageGUID);

                //trans.Commit();
                return true;
                //}
            }
            return false;
        }

        public void SetExceptionMessage(long batchID, long loanID, long purgeFailed, string message)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    PurgeStagingDetails psDetails = db.PurgeStagingDetails.AsNoTracking().Where(psd => psd.BatchID == batchID && psd.LoanID == loanID).FirstOrDefault();
                    PurgeStaging prgStg = db.PurgeStaging.AsNoTracking().Where(pStg => pStg.BatchID == batchID).FirstOrDefault();

                    if (psDetails != null)
                    {
                        psDetails.Status = purgeFailed;
                        psDetails.ErrMsg = message;
                        psDetails.ModifiedOn = DateTime.Now;
                        db.Entry(psDetails).State = EntityState.Modified;
                        db.SaveChanges();
                        prgStg.Status = purgeFailed;
                        prgStg.ModifiedOn = DateTime.Now;
                        db.Entry(prgStg).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    trans.Commit();
                }
            }
        }

        public bool PSDLoanPurged(long batchID, Int64 loanStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                PurgeStaging pstg = db.PurgeStaging.AsNoTracking().Where(ps => ps.BatchID == batchID).FirstOrDefault();
                if (pstg != null)
                {
                    pstg.Status = loanStatus;
                    pstg.ModifiedOn = DateTime.Now;
                    db.Entry(pstg).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool PSDLoanPurged(Int64 batchID, Int64 LoanID, Int64 loanStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                PurgeStagingDetails psDtls = db.PurgeStagingDetails.AsNoTracking().Where(ps => ps.BatchID == batchID && ps.LoanID == LoanID).FirstOrDefault();
                if (psDtls != null)
                {

                    psDtls.Status = loanStatus;
                    psDtls.ModifiedOn = DateTime.Now;
                    psDtls.ErrMsg = String.Empty;
                    db.Entry(psDtls).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public List<PurgeStagingDetails> GetBatchDetails(Int64 _batchID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.PurgeStagingDetails.AsNoTracking().Where(psd => psd.BatchID == _batchID && psd.Status == StatusConstant.PURGE_WAITING || psd.Status == StatusConstant.EXPORT_WAITING).ToList();
            }
        }

        public Loan GetLoan(Int64 _loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Loan.AsNoTracking().Where(lo => lo.LoanID == _loanID).FirstOrDefault();
            }
        }

        public bool UpdateEphesoftFolderStatus(long _loanID, Int64 _status)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    Loan _loans = db.Loan.AsNoTracking().Where(lo => lo.LoanID == _loanID).FirstOrDefault();
                    if (_loans != null)
                    {
                        var _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == _loanID).FirstOrDefault();
                        if (_idcfield != null)
                        {
                            _idcfield.IDCFileRemovedStatus = _status;
                            _idcfield.ModifiedOn = DateTime.Now;
                            db.Entry(_idcfield).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.IDCFields.Add(new IDCFields
                            {
                                IDCFileRemovedStatus = _status,
                                OCRAccuracyCalculated = false,
                                Createdon = DateTime.Now
                            });
                            db.SaveChanges();
                        }
                        if (_status == StatusConstant.DELETE_FILE_PENDING)
                        {
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EPHESOFT_OUTPUT_FOLDER_MOVED);
                            LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0], auditDescs[1]);
                        }
                        else if (_status == StatusConstant.DELETE_FILE_SUCCESS)
                        {
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EPHESOFT_OUTPUT_FOLDER_DELETED);
                            LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0], auditDescs[1]);
                        }
                        else
                        {
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.ERROR_DELETING_EPHESOFT_OUTPUT);
                            LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0], auditDescs[1]);
                        }
                    }
                    trans.Commit();
                    return true;
                }
            }
            return false;
        }
        public Int64 PurgeLoanRecords(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Int64 returnVal = 0;
                SqlParameter loanid = new SqlParameter("@LOANID", LoanID);
                SqlParameter returnid = new SqlParameter("@SUCCESS", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                var obj = db.Database.ExecuteSqlCommand($"EXEC [{TenantSchema}].[LOAN_PURGE] @LOANID, @SUCCESS OUTPUT", loanid, returnid);
                Int64.TryParse(Convert.ToString(returnid.Value), out returnVal);
                return returnVal;
            }
        }

        public string GetPDFFooterName()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                string _footerName = db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == 0 && c.ConfigKey == ConfigConstant.PDFFOOTER).Select(cu => cu.ConfigValue).FirstOrDefault();
                return _footerName;
            }

        }
        #endregion
    }
}
