using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace IL.EncompassTrailingFileDownloader
{
    public class LoanCompleteRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string UserName { get; set; }
    }


    class EncompassTrailingFileDownloaderDataAccess
    {
        public string TenantSchema;
        private static string SystemSchema = "IL";
        private string baseUri = ConfigurationManager.AppSettings["BASE_URI"]; //http://localhost:52169/api/

        public EncompassTrailingFileDownloaderDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
            }
        }

        public List<IntellaAndEncompassFetchFields> GetEncompassLookUpFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType == LOSFieldType.LOOKUP && m.Active && m.TenantSchema == TenantSchema).ToList();
            }
        }

        public IntellaAndEncompassFetchFields GetEncompassImportFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => (m.FieldType.Contains(LOSFieldType.IMPORT)) && (m.EncompassFieldID == EncompassFieldConstant.SERVICE_TYPE) && m.Active && m.TenantSchema == TenantSchema).FirstOrDefault();
            }
        }

        public Int64 GetReviewTypeID(string ReviewTypeName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster rm = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeName == ReviewTypeName).FirstOrDefault();

                if (rm != null)
                    return rm.ReviewTypeID;

                return 0;
            }
        }

        public Int64 GetLoanId(string eLoanGuid)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(x => x.EnCompassLoanGUID.ToString() == eLoanGuid).FirstOrDefault();
                if (_loan != null)
                    return _loan.LoanID;

                return 0;
            }
        }

        public void UpdateEDownloadStatus(Int64 _downloadID, string ExceptionMsg)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentDownload _eAttachment = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.ID == _downloadID && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.TrailingDocuments).FirstOrDefault();

                if (_eAttachment != null)
                {
                    _eAttachment.Error = ExceptionMsg;
                    _eAttachment.Status = EncompassStatusConstant.DOWNLOAD_RETRY;
                    _eAttachment.ModifiedOn = DateTime.Now;
                    db.Entry(_eAttachment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }


        public void UpdateEDownloadStatus(Int64 _loanID, Int64 _elAttachmentID, Int64 _status, string errMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {

                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == _loanID).FirstOrDefault();

                ELoanAttachmentDownload _eAttachment = db.ELoanAttachmentDownload.AsNoTracking().Where(x => (x.ID == _elAttachmentID) && (x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.TrailingDocuments)).FirstOrDefault();

                if (_loan != null && _eAttachment != null)
                {
                    _eAttachment.Status = _status;
                    _eAttachment.ModifiedOn = DateTime.Now;
                    _eAttachment.Error = errMsg;

                    // db.ELoanAttachmentDownload.Add(_eAttachment);
                    //need to add errormsg
                    db.Entry(_eAttachment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }
        public void UpdateDownloadSteps(Int64 stagingID, Int64 status, string errorMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EDownloadStaging _staging = db.EDownloadStaging.AsNoTracking().Where(x => x.ID == stagingID).FirstOrDefault();
                if (_staging != null)
                {
                    _staging.Status = status;
                    _staging.Error = errorMsg;
                    _staging.ModifiedOn = DateTime.Now;
                    db.Entry(_staging).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        public void UpdateStatusEwebHookEvents(Int64 _id, Int32 status)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EWebhookEvents _loan = db.EWebhookEvents.AsNoTracking().Where(x => x.ID == _id).FirstOrDefault();
                if (_loan != null)
                {
                    _loan.Status = status;
                    _loan.ModifiedOn = DateTime.Now;
                    db.Entry(_loan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }



        public void DeleteStaginDetails(Guid? eLoanGuid)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                EDownloadStaging _stage = db.EDownloadStaging.AsNoTracking().Where(x => x.ELoanGUID == eLoanGuid).FirstOrDefault();
                if (_stage != null)
                {
                    db.Entry(_stage).State = System.Data.Entity.EntityState.Deleted;

                }
                ELoanAttachmentDownload _eloans = db.ELoanAttachmentDownload.AsNoTracking().Where(l => l.ELoanGUID == eLoanGuid).FirstOrDefault();
                if (_eloans != null)
                {
                    db.Entry(_eloans).State = System.Data.Entity.EntityState.Deleted;

                }
                db.SaveChanges();


            }
        }
        public void DeleteWebHookEvents(Int64 ID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EWebhookEvents _eWebevents = db.EWebhookEvents.AsNoTracking().Where(x => x.ID == ID).FirstOrDefault();
                if (_eWebevents != null)
                {
                    db.Entry(_eWebevents).State = System.Data.Entity.EntityState.Deleted;

                }

                db.SaveChanges();
            }
        }
        public List<EDownloadStaging> SetDownloadSteps(List<EAttachment> _eAttachments, Int64 _downloadID, string _eLoanGUID)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                foreach (EAttachment item in _eAttachments)
                {
                    EDownloadStaging _stage = db.EDownloadStaging.AsNoTracking().Where(x => x.DownloadStagingID == _downloadID && x.AttachmentGUID == item.AttachmentID).FirstOrDefault();

                    if (_stage != null)
                    {
                        _stage.Status = EncompassDownloadStepStatusConstant.Waiting;
                        _stage.ModifiedOn = DateTime.Now;
                        db.Entry(_stage).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        _stage = new EDownloadStaging()
                        {
                            DownloadStagingID = _downloadID,
                            ELoanGUID = new Guid(_eLoanGUID),
                            EAttachmentName = item.Title,
                            AttachmentGUID = item.AttachmentID,
                            TypeOfDownload = EncompassLoanAttachmentDownloadConstant.TrailingDocuments,
                            Status = EncompassDownloadStepStatusConstant.Waiting,
                            Step = EncompassDownloadStepConstant.LoanAttachment,
                            Error = string.Empty,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now
                        };

                        db.EDownloadStaging.Add(_stage);
                    }
                    db.SaveChanges();
                }

                List<EDownloadStaging> _staged = db.EDownloadStaging.AsNoTracking().Where(x => x.DownloadStagingID == _downloadID).ToList();

                return _staged;
            }
        }

        public ELoanAttachmentDownload AddOrUpdateEDownloadStatus(Int64 _loanID, Int64 _downloadID, Int64 _status, bool _retryFlag, string errMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {

                ELoanAttachmentDownload _eAttachment = null;
                if (_retryFlag)
                {
                    _eAttachment = db.ELoanAttachmentDownload.AsNoTracking().Where(x => (x.ID == _downloadID) && (x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.TrailingDocuments)).FirstOrDefault();
                    if (_eAttachment != null)
                    {
                        _eAttachment.Status = _status;
                        _eAttachment.ModifiedOn = DateTime.Now;
                        _eAttachment.Error = errMsg;

                        db.Entry(_eAttachment).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                else
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == _loanID).FirstOrDefault();

                    _eAttachment = new ELoanAttachmentDownload()
                    {
                        LoanID = _loanID,
                        ELoanGUID = _loan.EnCompassLoanGUID.GetValueOrDefault(),
                        ELoanNumber = _loan.LoanNumber,
                        Status = EncompassStatusConstant.DOWNLOAD_PENDING,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        TypeOfDownload = EncompassLoanAttachmentDownloadConstant.TrailingDocuments,
                        Error = ""
                        // need to add EloanNumber
                    };

                    db.ELoanAttachmentDownload.Add(_eAttachment);
                }
                db.SaveChanges();

                return _eAttachment;

            }
        }


        public void MissingDocFileUpload(Dictionary<string, object> AuditLoan)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MISSING_DOCUMENT_UPLOADED);

                LoanAudit.InsertLoanMissingDocAudit(db, AuditLoan, StatusConstant.MOVED_TO_IDC, auditDescs[0], auditDescs[1]);
            }
        }

        public int GetAuditLoanMissingDocCount(Int64 loanid)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<AuditLoanMissingDoc> auditLoanMissingDocCount = db.AuditLoanMissingDoc.AsNoTracking().Where(x => x.LoanID == loanid).ToList();
                return auditLoanMissingDocCount.Count;
            }

        }

        public void updateLoanCompleteStatus(Int64 loanid, string tenantSchema)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                //need to change this code
                List<AuditLoanMissingDoc> auditLoanMissingDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(x => (x.LoanID == loanid) && (x.Status != StatusConstant.OCR_COMPLETED)).ToList();

                if (auditLoanMissingDoc.Count == 0)
                {
                    //need to call api functionality to auditcomplete

                    LoanCompleteRequest loanReq = new LoanCompleteRequest();
                    loanReq.LoanID = loanid;
                    loanReq.TableSchema = tenantSchema;
                    loanReq.UserName = "";
                    using (HttpClient client = new HttpClient())
                    {
                        string json = JsonConvert.SerializeObject(loanReq);
                        client.BaseAddress = new Uri(this.baseUri);
                        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                        try
                        {
                            var response = client.PostAsync("EncompassInterface/LoanComplete", requestContent).Result;
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var resultBody = response.Content.ReadAsStringAsync();
                                var jsonres = resultBody.Result;
                            }
                        }
                        catch (Exception ex)
                        {
                            MTSExceptionHandler.HandleException(ref ex);
                            throw ex;
                        }
                    }

                }
            }
        }
        public List<EWebhookEvents> GetEncompassWebHookLoans()
        {

            try
            {
                List<EWebhookEvents> _lsLoans = new List<EWebhookEvents>();
                using (var db = new DBConnect(TenantSchema))
                {
                    _lsLoans = db.EWebhookEvents.AsNoTracking().Where(x => x.EventType == EWebHookEventsLogConstant.DOCUMENT_LOG && x.Status == EWebHookStatusConstant.EWEB_HOOK_STAGED).ToList();

                }
                return _lsLoans;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                throw ex;
            }


        }
        public List<LoanDownload> GetDBLoans(Guid? EnCompassLoanGUID)
        {
            try
            {
                List<LoanDownload> _lsLoans = new List<LoanDownload>();
                using (var db = new DBConnect(TenantSchema))
                {
                    //use left join 
                    _lsLoans = (from l in db.Loan.AsNoTracking()
                                where ((l.UploadType == UploadConstant.ENCOMPASS) && (
                                l.Status == StatusConstant.PENDING_IDC ||
                                l.Status == StatusConstant.IDC_COMPLETE ||
                                l.Status == StatusConstant.READY_FOR_IDC ||
                                l.Status == StatusConstant.IDC_ERROR ||
                                l.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE ||
                                l.Status == StatusConstant.PENDING_AUDIT) && l.LoanGUID == EnCompassLoanGUID)
                                select new LoanDownload()
                                {
                                    LoanID = l.LoanID,
                                    EnCompassLoanGUID = l.EnCompassLoanGUID,
                                    DownloadID = 0,
                                    RetryFlag = false
                                }).OrderByDescending(l => l.LoanID).ToList();
                }
                return _lsLoans;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                throw ex;
            }
        }


        public List<LoanDownload> GetRetryTrailingLoans()
        {
            try
            {
                List<LoanDownload> _lsLoans = new List<LoanDownload>();
                using (var db = new DBConnect(TenantSchema))
                {
                    //use left join 
                    _lsLoans = (from x in db.ELoanAttachmentDownload.AsNoTracking()
                                where
                                (x.Status == EncompassStatusConstant.DOWNLOAD_RETRY) &&
                                (x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.TrailingDocuments)
                                select new LoanDownload()
                                {
                                    LoanID = x.LoanID,
                                    EnCompassLoanGUID = x.ELoanGUID,
                                    DownloadID = x.ID,
                                    RetryFlag = true
                                }).OrderByDescending(l => l.LoanID).ToList();
                }
                return _lsLoans;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                throw ex;
            }
        }
    }
}
