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

namespace IL.Ephesoft.Export
{
    class EphesoftExportDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public EphesoftExportDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods

        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<TenantMaster> _tenantMs = db.TenantMaster.ToList();

                List<AppConfig> _appConfig = db.AppConfig.ToList();

                List<Int64> _ints = new List<long>();

                foreach (TenantMaster a in _tenantMs)
                {
                    foreach (AppConfig app in _appConfig)
                    {
                        if (a.ID == app.ID)
                            _ints.Add(a.ID);
                    }
                }

                _ints = _tenantMs.Where(a => _appConfig.Any(b => b.ID == a.ID)).Select(t => t.ID).ToList();
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }

        public static Int64 AddExportProcessingQueue(string _tenantSchema, string fileName, Int32 priority)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Logger.WriteTraceLog($"Start AddExportProcessingQueue()");
                ExportProcessingQueue _processQueue = new ExportProcessingQueue()
                {
                    TenantSchema = _tenantSchema,
                    OriginPath = fileName,
                    DestinationPath = string.Empty,
                    Priority = priority,
                    PickUpDateTime = DateTime.Now,
                    Status = 0,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ErrorMessage = string.Empty
                };

                db.ExportProcessingQueue.Add(_processQueue);
                db.SaveChanges();
                Logger.WriteTraceLog($"End AddExportProcessingQueue() _processQueue : {_processQueue.ProcessingQueueID}");
                return _processQueue.ProcessingQueueID;
            }

            return 0;
        }

        public static Int32 GetWaitingProcessedBatchCount()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Logger.WriteTraceLog($"Start GetWaitingProcessedBatchCount()");
                List<ExportProcessingQueue> _ls = db.ExportProcessingQueue.AsNoTracking().Where(e => e.Status == 1).ToList();
                Logger.WriteTraceLog($"End GetWaitingProcessedBatchCount()");
                return _ls.Count;
            }
        }

        public static void UpdateExportProcessingQueueStatus(Int64 _processQueueID, Int32 _status, string _errorMessage = "")
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Logger.WriteTraceLog($"Start UpdateExportProcessingQueueStatus() _processQueueID: {_processQueueID} ");
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
                Logger.WriteTraceLog($"End UpdateExportProcessingQueueStatus() _processQueueID: {_processQueueID} ");
            }
        }


        public static void UpdateExportProcessingQueue(Int64 _processQueueID, string _destFolderPath)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Logger.WriteTraceLog($"Start UpdateExportProcessingQueue : {_processQueueID}");
                ExportProcessingQueue _processQueue = db.ExportProcessingQueue.AsNoTracking().Where(e => e.ProcessingQueueID == _processQueueID).FirstOrDefault();
                if (_processQueue != null)
                {
                    DateTime _date = DateTime.Now;

                    _processQueue.EndDateTime = _date;
                    _processQueue.ModifiedOn = _date;
                    _processQueue.Status = 1;
                    _processQueue.DestinationPath = _destFolderPath;

                    TimeSpan time = TimeSpan.FromMilliseconds(_date.Subtract(_processQueue.PickUpDateTime).TotalMilliseconds);
                    string _duration = $"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";
                    _processQueue.Duration = _duration;

                    db.Entry(_processQueue).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Logger.WriteTraceLog($"End UpdateExportProcessingQueue : {_processQueueID}");
            }
        }

        public Boolean CheckCustomerConfigKey()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var customerconfigkeyList = db.CustomerConfig.AsNoTracking().Where(m => (m.CustomerID == 0) && (m.ConfigKey == "Delete_Loan_Source") && (m.ConfigValue.ToLower().Equals("true")) && (m.Active)).ToList();
                if (customerconfigkeyList.Count > 0)
                    return true;
            }
            return false;
        }

        public string GetEphesoftBatchInputFolderWithLoanID(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster rm = (from l in db.Loan.AsNoTracking()
                                       join r in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals r.ReviewTypeID
                                       where l.LoanID == LoanID
                                       select r).FirstOrDefault();
                if (rm != null)
                {
                    return rm.BatchClassInputPath;
                }
            }

            return string.Empty;
        }


        public string GetEphesoftBatchInputFolder(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster rm = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                if (rm != null)
                {
                    return rm.BatchClassInputPath;
                }
            }

            return string.Empty;
        }

        public void UpdateLoanStatus(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    var item = db.Loan.AsNoTracking().Where(m => m.LoanID == LoanID).FirstOrDefault();
                    item.Status = StatusConstant.PENDING_IDC;
                    item.ModifiedOn = DateTime.Now;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EXPORTED_TO_EPHESOFT);
                    LoanAudit.InsertLoanAudit(db, item, auditDescs[0], auditDescs[1]);

                    trans.Commit();
                }
            }
        }

        public void SetLoanPDFPageCount(Int64 LoanID, Int64 pageCount)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    var item = db.Loan.AsNoTracking().Where(m => m.LoanID == LoanID).FirstOrDefault();
                    if (item != null)
                    {
                        item.PageCount = pageCount;
                        item.ModifiedOn = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        LoanAudit.InsertLoanAudit(db, item, "Page Count Updated", "");
                    }
                    trans.Commit();
                }
            }
        }

        public void UpdateLoanStatus(Int64 LoanID, Int64 StatusID, Int64 DocID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    if (DocID == 0)
                    {
                        var item = db.Loan.AsNoTracking().Where(m => m.LoanID == LoanID).FirstOrDefault();
                        item.Status = StatusID;
                        item.ModifiedOn = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(m => m.LoanID == LoanID).FirstOrDefault();

                        if (_loanSearch != null)
                        {
                            _loanSearch.Status = StatusID;
                            _loanSearch.ModifiedOn = DateTime.Now;
                            db.Entry(_loanSearch).State = EntityState.Modified;
                            db.SaveChanges();

                            string[] auditLoanSearchDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EXPORT_TO_QUEUE_FAILED);
                            LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditLoanSearchDescs[0], auditLoanSearchDescs[1]);
                        }

                        string[] auditDescss = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EXPORT_TO_QUEUE_FAILED);
                        LoanAudit.InsertLoanAudit(db, item, auditDescss[0], auditDescss[1]);
                        //LoanAudit.InsertLoanAudit(db, item, "Exported to Queue Failed");
                    }
                    else
                    {
                        AuditLoanMissingDoc _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocID == DocID).FirstOrDefault();

                        if (_loan != null)
                        {
                            _loan.Status = StatusID == StatusConstant.IDC_ERROR ? StatusConstant.IDCERROR : StatusConstant.MOVED_TO_IDC;
                            _loan.ModifiedOn = DateTime.Now;
                            db.Entry(_loan).State = EntityState.Modified;
                            db.SaveChanges();

                        }
                    }
                    trans.Commit();
                }
            }
        }

        public void UpdateLoanStatus(Int64 LoanID, Int64 StatusID, Int32 ErrorCode)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    var item = db.Loan.AsNoTracking().Where(m => m.LoanID == LoanID).FirstOrDefault();
                    if (item != null)
                    {
                        item.SubStatus = ErrorCode;
                        item.Status = StatusID;
                        item.ModifiedOn = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        LoanAudit.InsertLoanAudit(db, item, "Exported to Queue Failed", "");
                    }
                    trans.Commit();
                }
            }
        }


        public int GetEphesoftPendingBatchCount(string batchID, string BatchStatus)
        {
            string[] _batchStatus = BatchStatus.Split(',');
            List<string> _bStatus = new List<string>();

            foreach (string _status in _batchStatus)
                _bStatus.Add($"'{_status.ToUpper()}'");

            //string sql = "SELECT COUNT(1) AS BATCHCOUNT  FROM BATCH_INSTANCE BI WITH(NOLOCK) INNER JOIN [BATCH_CLASS] B WITH(NOLOCK) ON BI.BATCH_CLASS_ID=B.ID WHERE B.IDENTIFIER= '" + batchID + "' AND BATCH_STATUS IN (" + string.Join(",", _bStatus) + ")";
            string sql = "SELECT COUNT(1) AS BATCHCOUNT  FROM BATCH_INSTANCE BI WITH(NOLOCK) INNER JOIN [BATCH_CLASS] B WITH(NOLOCK) ON BI.BATCH_CLASS_ID=B.ID WHERE BATCH_STATUS IN (" + string.Join(",", _bStatus) + ")";
            System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["BATCHCOUNT"]);
            }
            return 0;
        }


        public List<ExportLoan> GetLoansToBeExported(Int32 loanFetchCount)
        {
            List<ExportLoan> _loans = new List<ExportLoan>();

            using (var db = new DBConnect(TenantSchema))
            {
                var loanPending = db.Loan.AsNoTracking().Where(x => x.Status == StatusConstant.READY_FOR_IDC).ToList();

                //var item = new ExportLoan();

                var _adhocLoans = (from loan in loanPending
                                   join RTM in db.ReviewTypeMaster.AsNoTracking() on loan.ReviewTypeID equals RTM.ReviewTypeID
                                   where RTM.ReviewTypePriority != null && RTM.ReviewTypePriority != 0 && loan.UploadType == UploadConstant.ADHOC   //!loan.FromBox
                                   select new ExportLoan()
                                   {
                                       LoanID = loan.LoanID,
                                       Priority = RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority,
                                       CreatedOn = loan.CreatedOn,
                                       FileName = loan.FileName,
                                       TenantSchema = TenantSchema,
                                       Status = loan.Status
                                   }).ToList();


                var _boxLoans = (from loan in loanPending
                                 join b in db.BoxDownloadQueue.AsNoTracking() on loan.LoanID equals b.LoanID
                                 where loan.ReviewTypeID != 0 && loan.UploadType == UploadConstant.BOX  //!loan.FromBox
                                 group loan by new { loan.LoanID, loan.CreatedOn, loan.FileName, loan.Status, b.Priority } into gcs
                                 select new ExportLoan()
                                 {
                                     LoanID = gcs.Key.LoanID,
                                     Priority = gcs.Key.Priority,
                                     CreatedOn = gcs.Key.CreatedOn,
                                     FileName = gcs.Key.FileName,
                                     TenantSchema = TenantSchema,
                                     Status = gcs.Key.Status
                                 }).ToList();

                var _encompassLoans = (from loan in loanPending
                                       join RTM in db.ReviewTypeMaster.AsNoTracking() on loan.ReviewTypeID equals RTM.ReviewTypeID
                                       where RTM.ReviewTypePriority != null && RTM.ReviewTypePriority != 0 && loan.UploadType == UploadConstant.ENCOMPASS   //!loan.FromBox
                                       select new ExportLoan()
                                       {
                                           LoanID = loan.LoanID,
                                           Priority = RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority,
                                           CreatedOn = loan.CreatedOn,
                                           FileName = loan.FileName,
                                           TenantSchema = TenantSchema,
                                           Status = loan.Status
                                       }).ToList();

                var _losLoans = (from loan in loanPending
                                 join RTM in db.ReviewTypeMaster.AsNoTracking() on loan.ReviewTypeID equals RTM.ReviewTypeID
                                 where RTM.ReviewTypePriority != null && RTM.ReviewTypePriority != 0 && loan.UploadType == UploadConstant.LOS   //!loan.FromBox
                                 select new ExportLoan()
                                 {
                                     LoanID = loan.LoanID,
                                     Priority = RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority,
                                     CreatedOn = loan.CreatedOn,
                                     FileName = loan.FileName,
                                     TenantSchema = TenantSchema,
                                     Status = loan.Status
                                 }).ToList();

                _loans = _boxLoans.Union(_adhocLoans).Union(_encompassLoans).Union(_losLoans).Distinct().OrderBy(m => m.Priority).ThenBy(m => m.CreatedOn).Take(loanFetchCount).ToList();

                if (_loans == null)
                {
                    _loans = (from loan in loanPending
                              join RTM in db.ReviewTypeMaster.AsNoTracking() on loan.ReviewTypeID equals RTM.ReviewTypeID
                              where (RTM.ReviewTypePriority == 0 || RTM.ReviewTypePriority == null)
                              select new ExportLoan()
                              {
                                  LoanID = loan.LoanID,
                                  Priority = RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority,
                                  CreatedOn = loan.CreatedOn,
                                  FileName = loan.FileName,
                                  TenantSchema = TenantSchema,
                                  Status = loan.Status
                              }).OrderBy(m => m.CreatedOn).Take(loanFetchCount).ToList();
                }
            }

            return _loans;
        }

        public ExportLoan GetLoanDocumentToExport()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var loanPending = db.Loan.AsNoTracking().Where(x => x.Status == StatusConstant.READY_FOR_IDC).ToList();

                var item = new ExportLoan();
                item = (from loan in loanPending
                        join RTM in db.ReviewTypeMaster.AsNoTracking() on loan.ReviewTypeID equals RTM.ReviewTypeID
                        where RTM.ReviewTypePriority != 0 && RTM.ReviewTypePriority != null
                        select new ExportLoan()
                        {
                            LoanID = loan.LoanID,
                            Priority = RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority,
                            CreatedOn = loan.CreatedOn,
                            FileName = loan.FileName,
                            TenantSchema = TenantSchema,
                            Status = loan.Status
                        }).OrderBy(m => m.CreatedOn).ThenBy(m => m.Priority).FirstOrDefault();

                if (item == null)
                {
                    item = (from loan in loanPending
                            join RTM in db.ReviewTypeMaster.AsNoTracking() on loan.ReviewTypeID equals RTM.ReviewTypeID
                            where (RTM.ReviewTypePriority == 0 || RTM.ReviewTypePriority == null)
                            select new ExportLoan()
                            {
                                LoanID = loan.LoanID,
                                Priority = RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority,
                                CreatedOn = loan.CreatedOn,
                                FileName = loan.FileName,
                                TenantSchema = TenantSchema,
                                Status = loan.Status
                            }).OrderBy(m => m.CreatedOn).FirstOrDefault();
                }

                return item;
            }
        }

        #endregion Public Methods
    }

    public class ExportLoan
    {
        public Int64 LoanID { get; set; }
        public Int64? Priority { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string FileName { get; set; }
        public string TenantSchema { get; set; }
        public Int64 Status { get; set; }
    }
}
