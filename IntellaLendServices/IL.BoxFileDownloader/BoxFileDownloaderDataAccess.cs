using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.BoxFileDownloader
{
    public class BoxFileDownloaderDataAccess
    {

        #region Private Variables

        private static int MaxRetryCount = 0;

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public BoxFileDownloaderDataAccess(string tenantSchema,int maxRetryCount)
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
                return db.TenantMaster.Where(m => m.Active==true).ToList();
            }
        }

        public List<BoxDownloadQueue> GetFilesToDownload(int status)
        {
            var itemList = new List<BoxDownloadQueue>();
            using (var db = new DBConnect(TenantSchema))
            {
                var itemToDownload = db.BoxDownloadQueue.AsNoTracking().Where(m => m.Status == status && m.Priority != 0).OrderBy(m => m.Priority).ThenBy(m => m.CreatedOn).FirstOrDefault();
                if (itemToDownload == null || itemToDownload.ID == 0)
                    itemToDownload = db.BoxDownloadQueue.AsNoTracking().Where(m => m.Status == status && m.Priority == 0).OrderBy(m => m.CreatedOn).FirstOrDefault();

                if(itemToDownload != null && itemToDownload.ID != 0)
                { 
                    itemList = db.BoxDownloadQueue.AsNoTracking().Where(m => m.LoanID == itemToDownload.LoanID).ToList();
                }
            }
            return itemList;
        }

        public void UpdateFileDownloadStatus(Int64 id, int status, string FileName, string errorMsg="")
        {

            int finalStatus = status;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {                   
                    
                    var fileItem = db.BoxDownloadQueue.AsNoTracking().Where(m => m.ID == id).FirstOrDefault();
                    var itemList = db.BoxDownloadQueue.AsNoTracking().Where(m => m.LoanID == fileItem.LoanID).ToList();

                    foreach (var item in itemList)
                    {

                        if (status == BoxDownloadStatusConstant.DOWNLOAD_FAILED)
                        {
                            item.RetryCount = (item.RetryCount == null ? 0 : item.RetryCount) + 1;
                            if (item.RetryCount <= MaxRetryCount)
                            {
                                item.Status = BoxDownloadStatusConstant.DOWNLOAD_PENDING;
                                finalStatus = BoxDownloadStatusConstant.DOWNLOAD_PENDING;
                            }
                            else
                            {
                                item.Status = status;
                            }
                        }
                        else
                        {
                            item.Status = status;
                        }

                        item.ErrorMsg = errorMsg;
                        item.ModifiedOn = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                   

                    if (finalStatus == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS || finalStatus == BoxDownloadStatusConstant.DOWNLOAD_FAILED)
                    {
                        var loan = db.Loan.AsNoTracking().Where(m => m.LoanID == fileItem.LoanID).FirstOrDefault();
                        loan.FileName = FileName;
                        loan.Status = finalStatus == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS ? StatusConstant.READY_FOR_IDC : StatusConstant.FAILED_BOX_DOWNLOAD;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();

                        if (loan.Status == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS)
                        {
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.BOX_FILE_DOWNLOADED);
                            LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);
                        }
                        else
                        {
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.BOX_FILE_DOWNLOAD_FAILED);
                            LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);
                        }
                    }

                    trans.Commit();
                }
            }
        }

        #endregion

    }
}
