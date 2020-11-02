using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.LoanJobsExport
{
    class LoanJobExportDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public LoanJobExportDataAccess(string tenantSchema)
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


        public string GetLoanNumber(Int64 LoanID)
        {
            string _loanNumber = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                _loanNumber = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).Select(ln => ln.LoanNumber).FirstOrDefault();
                _loanNumber = string.IsNullOrEmpty(_loanNumber) ? Convert.ToString(LoanID) : _loanNumber;
            }
            return _loanNumber;
        }
        public byte[] GetLoanPDF(Int64 LoanID)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
            return _imageWrapper.GetLoanPDF(LoanID);
        }
        public List<LoanJobExport> GetLoanBatches()
        {
            List<LoanJobExport> _loanBatches = null;
            using (var db = new DBConnect(TenantSchema))
            {
                _loanBatches = db.LoanJobExport.AsNoTracking().Where(lb => lb.Status == StatusConstant.JOB_WAITING).ToList();
            }
            return _loanBatches;
        }

        public List<LoanJobExportDetail> GetLoanBatchExportDetails(Int64 jobId)
        {
            List<LoanJobExportDetail> _loanBatch = null;
            using (var db = new DBConnect(TenantSchema))
            {
                _loanBatch = db.LoanJobExportDetail.AsNoTracking().Where(lb => lb.JobID == jobId && lb.Status == 0).ToList();
            }
            return _loanBatch;
        }

        //public int CheckAllLoansExported(Int64 jobId)
        //{
        //    List<LoanJobExportDetail> _loanBatch = null;
        //    int loanPendingCount = 1;
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        loanPendingCount = db.LoanJobExportDetail.AsNoTracking().Where(lb => lb.JobID == jobId &&  lb.Status == -1).ToList().Count();
        //    }
        //    return loanPendingCount;
        //}
        public int CheckAllLoansExported(Int64 jobId)
        {
            int loanExportCount = 0;
            using (var db = new DBConnect(TenantSchema))
            {
                //Status : 1 = Exported Successfully
                //Status : -1 = Exported Failed
                loanExportCount = db.LoanJobExportDetail.AsNoTracking().Where(lb => lb.JobID == jobId && lb.Status == 1).ToList().Count();
            }
            return loanExportCount;
        }
        public string GetLoanDetailsObject(Int64 loanId)
        {
            string _loanObject = null;
            using (var db = new DBConnect(TenantSchema))
            {
                _loanObject = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == loanId).Select(a => a.LoanObject).FirstOrDefault();
            }
            return _loanObject;
        }

        public string GetDocDetailsObject(Int64 LoanID)
        {
            string obj = null;
            using (var db = new DBConnect(TenantSchema))
            {
                obj = db.LoanJobExportDetail.AsNoTracking().Where(ld => ld.LoanID == LoanID).Select(a => a.LoanDocumentConfig).FirstOrDefault();
            }
            return obj;
        }

        public void UpdateLoanExportDetailStatus(Int64 LoanId, Int64 jobId, string fileName, string filePath, int Status)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanJobExportDetail _loanJobExportDetail = db.LoanJobExportDetail.AsNoTracking().Where(ld => ld.LoanID == LoanId && ld.JobID == jobId).FirstOrDefault();
                if (_loanJobExportDetail != null)
                {
                    _loanJobExportDetail.Status = Status;
                    _loanJobExportDetail.FileName = fileName;
                    _loanJobExportDetail.FilePath = filePath;
                    _loanJobExportDetail.ModifiedOn = DateTime.Now;
                    db.Entry(_loanJobExportDetail).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public void UpdateLoanJobExport(Int64 _jobId, Int64 _loanStatus, string jobExportPath)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanJobExport _loanJobexport = db.LoanJobExport.AsNoTracking().Where(ld => ld.JobID == _jobId).FirstOrDefault();
                if (_loanJobexport != null && _loanStatus == StatusConstant.PROCESSING_JOB)
                {
                    _loanJobexport.Status = _loanStatus;
                    _loanJobexport.ModifiedOn = DateTime.Now;
                    db.Entry(_loanJobexport).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (_loanJobexport != null && _loanStatus == StatusConstant.JOB_EXPORTED)
                {
                    _loanJobexport.Status = _loanStatus;
                    _loanJobexport.ExportPath = jobExportPath;
                    _loanJobexport.ErrorMsg = string.Empty;
                    _loanJobexport.ErrorStackTrace = string.Empty;
                    _loanJobexport.ModifiedOn = DateTime.Now;
                    db.Entry(_loanJobexport).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public void UpdateLoanBatchExportErrorStatus(Int64 _jobId, Int64 _loanStatus, Exception ex)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanJobExport _loanJobExport = db.LoanJobExport.AsNoTracking().Where(ld => ld.JobID == _jobId).FirstOrDefault();
                if (_loanJobExport != null)
                {
                    _loanJobExport.Status = _loanStatus;
                    _loanJobExport.ModifiedOn = DateTime.Now;
                    _loanJobExport.ErrorMsg = ex.Message;
                    _loanJobExport.ErrorStackTrace = ex.StackTrace;
                    db.Entry(_loanJobExport).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateLoanBatchExportErrorStatus(Int64 _jobId, Int64 _loanStatus, string exMsg)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanJobExport _loanJobExport = db.LoanJobExport.AsNoTracking().Where(ld => ld.JobID == _jobId).FirstOrDefault();
                if (_loanJobExport != null)
                {
                    _loanJobExport.Status = _loanStatus;
                    _loanJobExport.ModifiedOn = DateTime.Now;
                    _loanJobExport.ErrorMsg = exMsg;
                    _loanJobExport.ErrorStackTrace = string.Empty;
                    db.Entry(_loanJobExport).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateLoanBatchExportErrorStatus(Int64 _jobId, Int64 _loanStatus, string exMsg, string exportPath)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanJobExport _loanJobExport = db.LoanJobExport.AsNoTracking().Where(ld => ld.JobID == _jobId).FirstOrDefault();
                if (_loanJobExport != null)
                {
                    _loanJobExport.Status = _loanStatus;
                    _loanJobExport.ExportPath = exportPath;
                    _loanJobExport.ModifiedOn = DateTime.Now;
                    _loanJobExport.ErrorMsg = exMsg;
                    _loanJobExport.ErrorStackTrace = string.Empty;
                    db.Entry(_loanJobExport).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void UpdateLoanExportDetailStatus(Int64 LoanId, Int64 jobId, Exception ex)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanJobExportDetail _loanJobExportDetail = db.LoanJobExportDetail.AsNoTracking().Where(ld => ld.LoanID == LoanId && ld.JobID == jobId).FirstOrDefault();
                if (_loanJobExportDetail != null)
                {
                    _loanJobExportDetail.Status = StatusConstant.FILE_CREATION_FAILED;
                    //_loanJobExportDetail.FilePath = filePath;
                    _loanJobExportDetail.ErrorMsg = ex.Message;
                    _loanJobExportDetail.ModifiedOn = DateTime.Now;
                    db.Entry(_loanJobExportDetail).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        //public List<int> GetLoanID()
        //{
        //    List < int > loanID= null;
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        loanID=db.LoanBatchExportDetail.
        //    }
        //    return loanID;
        //}
        public string GetPDFFooterName()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                string _footerName = db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == 0 && c.ConfigKey == ConfigConstant.PDFFOOTER && c.Active == true).Select(cu => cu.ConfigValue).FirstOrDefault();
                return _footerName;
            }

        }
        #endregion
    }
}
