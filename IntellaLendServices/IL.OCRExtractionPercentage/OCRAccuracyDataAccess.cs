using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
namespace IL.OCRExtractionPercentage
{
    public class OCRAccuracyDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public OCRAccuracyDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods

        public static List<TenantMaster> GetTenantList()
        {
            try
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<GetLoanBatch> GetWaitingOCRPercentageLoans()
        {
            try
            {
                List<GetLoanBatch> _lsLoans = new List<GetLoanBatch>();
                using (var db = new DBConnect(TenantSchema))
                {
                    _lsLoans = (from l in db.Loan.AsNoTracking()
                                join idf in db.IDCFields.AsNoTracking() on l.LoanID equals idf.LoanID
                                where idf.OCRAccuracyCalculated == false && idf.IDCBatchInstanceID != null && idf.IDCBatchInstanceID != "" && l.Status != StatusConstant.LOAN_DELETED
                                select new GetLoanBatch
                                {
                                    LoanId = l.LoanID,
                                    IDCBatchInstanceID = idf.IDCBatchInstanceID
                                }).OrderByDescending(l => l.LoanId).ToList();
                }

                return _lsLoans;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLoanOCRPercentageStatus(Int64 LoanID)
        {
            try
            {
                using (var db = new DBConnect(TenantSchema))
                {
                    using (var tran = db.Database.BeginTransaction())
                    {
                        Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                        if (_loan != null)
                        {
                            var _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                            if (_idcfield != null)
                            {
                                _idcfield.OCRAccuracyCalculated = true;
                                _idcfield.ModifiedOn = DateTime.Now;
                                db.Entry(_idcfield).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.IDCFields.Add(new IDCFields
                                {
                                    OCRAccuracyCalculated = true,
                                    Createdon = DateTime.Now
                                });
                                db.SaveChanges();
                            }
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.OCR_Accuracy_Calculated);
                            LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], auditDescs[1]);
                            LoanAudit.InsertLoanIDCFieldAudit(db, _idcfield, auditDescs[0], auditDescs[1]);

                            tran.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateLoanEphesoftAccuracy(Int64 LoanID, decimal EphesoftOCRAccuracy)
        {
            try
            {
                using (var db = new DBConnect(TenantSchema))
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (_loan != null)
                    {
                        var _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                        if (_idcfield != null)
                        {
                            _idcfield.IDCOCRAccuracy = EphesoftOCRAccuracy;
                            _idcfield.ModifiedOn = DateTime.Now;
                            db.Entry(_idcfield).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.IDCFields.Add(new IDCFields
                            {
                                IDCOCRAccuracy = EphesoftOCRAccuracy,
                                OCRAccuracyCalculated = false,
                                Createdon = DateTime.Now,
                                ModifiedOn = DateTime.Now,
                            });
                            db.SaveChanges();
                        }
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EPHESOFTOCRACCURACY_UPDATED);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], auditDescs[1]);
                        LoanAudit.InsertLoanIDCFieldAudit(db, _idcfield, auditDescs[0], auditDescs[1]);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool UpdateLoanClassificationAccuracy(Int64 LoanID, decimal classificationAccuracy)
        {
            try
            {
                using (var db = new DBConnect(TenantSchema))
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (_loan != null)
                    {
                        var _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                        if (_idcfield != null)
                        {
                            _idcfield.ClassificationAccuracy = classificationAccuracy;
                            _loan.ModifiedOn = DateTime.Now;
                            db.Entry(_idcfield).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.IDCFields.Add(new IDCFields
                            {
                                ClassificationAccuracy = classificationAccuracy,
                                OCRAccuracyCalculated = false,
                                Createdon = DateTime.Now
                            });
                            db.SaveChanges();
                        }
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.ClassificationAccuracy_Updated);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], auditDescs[1]);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool UpdateLoanDocumentAccuracy(Int64 LoanID, System.Data.DataTable DocumentPercentageTable)
        {
            try
            {
                using (var db = new DBConnect(TenantSchema))
                {
                    LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (_loanDetail != null)
                    {
                        Batch loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);

                        foreach (DataRow row in DocumentPercentageTable.Rows)
                        {
                            Documents _doc = loanBatch.Documents.Where(d => d.Identifier == Convert.ToString(row["DOCID"])).FirstOrDefault();
                            if (_doc != null)
                            {
                                _doc.DocumentExtractionAccuracy = Convert.ToString(row["DOCUMENT_PERCENTAGE"]);
                            }
                        }

                        _loanDetail.LoanObject = JsonConvert.SerializeObject(loanBatch);
                        _loanDetail.ModifiedOn = DateTime.Now;
                        db.Entry(_loanDetail).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EPHESOFTOCRACCURACY_UPDATED);
                        LoanAudit.InsertLoanDetailsAudit(db, _loanDetail, 0, auditDescs[0], auditDescs[1]);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static DataSet GetOCRAccuracy(string sql)
        {
            try
            {
                return new DataAccess2("IntellaLendReportingDB").ExecuteDataSet(sql);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        #endregion
    }
}
