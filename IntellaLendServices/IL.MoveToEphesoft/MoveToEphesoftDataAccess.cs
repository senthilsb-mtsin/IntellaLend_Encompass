using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.MoveToEphesoft
{
    class MoveToEphesoftDataAccess
    {
        #region Private Variables

        //private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        //public MoveToEphesoftDataAccess(string tenantSchema)
        //{
        //    TenantSchema = tenantSchema;
        //}

        #endregion

        #region Public Methods

        public static int GetEphesoftPendingBatchCount(string BatchStatus)
        {
            string[] _batchStatus = BatchStatus.Split(',');
            List<string> _bStatus = new List<string>();

            foreach (string _status in _batchStatus)
                _bStatus.Add($"'{_status.ToUpper()}'");

            string sql = "SELECT COUNT(1) AS BATCHCOUNT  FROM BATCH_INSTANCE BI WITH(NOLOCK) INNER JOIN [BATCH_CLASS] B WITH(NOLOCK) ON BI.BATCH_CLASS_ID=B.ID WHERE BATCH_STATUS IN (" + string.Join(",", _bStatus) + ")";
            System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["BATCHCOUNT"]);
            }
            return 0;
        }

        public static List<ExportProcessingQueue> GetWaitingProcessedBatchCount(Int32 _ephesoftEmptySlotCount)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Logger.WriteTraceLog($"Start GetWaitingProcessedBatchCount()");
                List<ExportProcessingQueue> ls = db.ExportProcessingQueue.AsNoTracking().Where(e => e.Status == 1).OrderBy(e => e.Priority).ThenBy(e => e.CreatedOn).Take(_ephesoftEmptySlotCount).ToList();
                Logger.WriteTraceLog($"Start GetWaitingProcessedBatchCount()");
                return ls;
            }
            return new List<ExportProcessingQueue>();
        }

        public static string GetEphesoftBatchInputFolder(string TenantSchema, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (_loan != null)
                {
                    ReviewTypeMaster rm = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == _loan.ReviewTypeID).FirstOrDefault();
                    if (rm != null)
                    {
                        return rm.BatchClassInputPath;
                    }
                }
            }

            return string.Empty;
        }

        public static void UpdateExportProcessingQueueError(Int64 _processQueueID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Logger.WriteTraceLog($"Start UpdateExportProcessingQueueError() _processQueueID : {_processQueueID}");
                ExportProcessingQueue _processQueue = db.ExportProcessingQueue.AsNoTracking().Where(e => e.ProcessingQueueID == _processQueueID).FirstOrDefault();

                if (_processQueue != null)
                {
                    if (!string.IsNullOrEmpty(_errorMessage))
                        _processQueue.ErrorMessage = _errorMessage;

                    _processQueue.Status = _status;
                    _processQueue.ModifiedOn = DateTime.Now;

                    db.Entry(_processQueue).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Logger.WriteTraceLog($"End UpdateExportProcessingQueueError() _processQueueID : {_processQueueID}");
            }
        }

        public static void UpdateLoanStatus(string _tenantSchema, Int64 _loanID, Int64 _status, Int64 _docID)
        {
            using (var db = new DBConnect(_tenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (_docID == 0)
                    {
                        Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == _loanID).FirstOrDefault();

                        if (_loan != null)
                        {
                            _loan.Status = _status;
                            _loan.ModifiedOn = DateTime.Now;

                            db.Entry(_loan).State = EntityState.Modified;
                            db.SaveChanges();
                            string[] auditDescss = AuditDataAccess.GetAuditDescription(_tenantSchema, _status == 7 ? AuditConfigConstant.EXPORT_TO_EPHESOFT_FAILED : AuditConfigConstant.EXPORTED_TO_EPHESOFT);
                            LoanAudit.InsertLoanAudit(db, _loan, auditDescss[0], auditDescss[1]);
                            //LoanAudit.InsertLoanAudit(db, _loan, _status == 7 ? "Exported to Ephesoft Failed" : "Exported to Ephesoft");

                            LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(m => m.LoanID == _loanID).FirstOrDefault();

                            if (_loanSearch != null)
                            {
                                _loanSearch.Status = _status;
                                _loanSearch.ModifiedOn = DateTime.Now;
                                db.Entry(_loanSearch).State = EntityState.Modified;
                                db.SaveChanges();

                                string[] auditLoanSearchDescs = AuditDataAccess.GetAuditDescription(_tenantSchema, _status == 7 ? AuditConfigConstant.EXPORT_TO_EPHESOFT_FAILED : AuditConfigConstant.EXPORTED_TO_EPHESOFT);
                                LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditLoanSearchDescs[0], auditLoanSearchDescs[1]);
                            }

                            tran.Commit();
                        }
                    }
                    else
                    {
                        AuditLoanMissingDoc _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == _loanID && l.DocID == _docID).FirstOrDefault();

                        if (_loan != null)
                        {
                            _loan.Status = StatusConstant.MOVED_TO_IDC;
                            _loan.ModifiedOn = DateTime.Now;
                            db.Entry(_loan).State = EntityState.Modified;
                            db.SaveChanges();
                            //string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                            //LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc), auditDescs[1].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc));


                            //LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(m => m.LoanID == _loanID).FirstOrDefault();

                            //if (_loanSearch != null)
                            //{
                            //    _loanSearch.Status = _status;
                            //    _loanSearch.ModifiedOn = DateTime.Now;
                            //    db.Entry(_loanSearch).State = EntityState.Modified;
                            //    db.SaveChanges();

                            //    string[] auditLoanSearchDescs = AuditDataAccess.GetAuditDescription(_tenantSchema, _status == 7 ? AuditConfigConstant.EXPORT_TO_EPHESOFT_FAILED : AuditConfigConstant.EXPORTED_TO_EPHESOFT);
                            //    LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditLoanSearchDescs[0], auditLoanSearchDescs[1]);
                            //}

                            tran.Commit();
                        }
                    }
                }
            }
        }

        public static void UpdateLoanStatus(string _tenantSchema, Int64 _loanID, Int64 _status, Int32 _errorCode, Int64 _docID)
        {
            using (var db = new DBConnect(_tenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (_docID == 0)
                    {
                        Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == _loanID).FirstOrDefault();

                        if (_loan != null)
                        {
                            _loan.SubStatus = _errorCode;
                            _loan.Status = _status;
                            _loan.ModifiedOn = DateTime.Now;

                            db.Entry(_loan).State = EntityState.Modified;
                            db.SaveChanges();

                            string audiDesc = _status == 7 ? "Exported to Ephesoft Failed" : "Exported to Ephesoft";
                            LoanAudit.InsertLoanAudit(db, _loan, audiDesc, audiDesc);

                            LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(m => m.LoanID == _loanID).FirstOrDefault();

                            if (_loanSearch != null)
                            {
                                _loanSearch.Status = _status;
                                _loanSearch.ModifiedOn = DateTime.Now;
                                db.Entry(_loanSearch).State = EntityState.Modified;
                                db.SaveChanges();

                                string auditLoanSearchDescs = _status == 7 ? "Exported to Ephesoft Failed" : "Exported to Ephesoft";
                                LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditLoanSearchDescs, auditLoanSearchDescs);
                            }

                            tran.Commit();
                        }
                    }
                    else
                    {
                        AuditLoanMissingDoc _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == _loanID && l.DocID == _docID).FirstOrDefault();

                        if (_loan != null)
                        {
                            _loan.Status = _status == 7 ? StatusConstant.IDCERROR : StatusConstant.MOVED_TO_IDC;
                            _loan.ModifiedOn = DateTime.Now;
                            db.Entry(_loan).State = EntityState.Modified;
                            db.SaveChanges();
                            //  string[] auditDescs = AuditDataAccess.GetAuditDescription(_tenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                            //  LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc), auditDescs[1].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc));



                            //LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(m => m.LoanID == _loanID).FirstOrDefault();
                            //if (_loanSearch != null)
                            //{
                            //    _loanSearch.Status = _status;
                            //    _loanSearch.ModifiedOn = DateTime.Now;
                            //    db.Entry(_loanSearch).State = EntityState.Modified;
                            //    db.SaveChanges();

                            //    string auditLoanSearchDescs = _status == 7 ? "Exported to Ephesoft Failed" : "Exported to Ephesoft";
                            //    LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditLoanSearchDescs, auditLoanSearchDescs);
                            //}
                            tran.Commit();
                        }
                    }
                }
            }

        }

        #endregion
    }
}
