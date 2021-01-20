using EncompassRequestBody.EResponseModel;
using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLend.Model.Encompass;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.EncompassFileDownloader
{
    public class EncompassFileDownloaderDataAccess
    {

        public string TenantSchema;
        private static string SystemSchema = "IL";

        public EncompassFileDownloaderDataAccess(string tenantSchema)
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

        public List<EDownloadStaging> SetDownloadSteps(List<EAttachment> _eAttachments, Int64 _downloadID, string _eLoanGUID)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                foreach (EAttachment item in _eAttachments)
                {
                    EDownloadStaging _stage = new EDownloadStaging()
                    {
                        DownloadStagingID = _downloadID,
                        ELoanGUID = new Guid(_eLoanGUID),
                        EAttachmentName = item.Title,
                        AttachmentGUID = item.AttachmentID,
                        TypeOfDownload = EncompassLoanAttachmentDownloadConstant.Loan,
                        Status = EncompassDownloadStepStatusConstant.Waiting,
                        Step = EncompassDownloadStepConstant.LoanAttachment,
                        Error = string.Empty,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    db.EDownloadStaging.Add(_stage);
                    db.SaveChanges();
                }

                //EDownloadStaging _fieldUpdateStage = db.EDownloadStaging.AsNoTracking().Where(x => x.DownloadStagingID == _downloadID && x.Step == EncompassDownloadStepConstant.UpdateField && (x.Status == EncompassDownloadStepStatusConstant.Waiting || x.Status == EncompassDownloadStepStatusConstant.Error)).FirstOrDefault();
                //if (_fieldUpdateStage == null)
                //{
                //    EDownloadStaging _stage = new EDownloadStaging()
                //    {
                //        DownloadStagingID = _downloadID,
                //        ELoanGUID = new Guid(_eLoanGUID),
                //        EAttachmentName = "Custom Flag Field Update",
                //        TypeOfDownload = EncompassLoanAttachmentDownloadConstant.Loan,
                //        Status = EncompassDownloadStepStatusConstant.Waiting,
                //        Step = EncompassDownloadStepConstant.UpdateField,
                //        Error = string.Empty,
                //        CreatedOn = DateTime.Now,
                //        ModifiedOn = DateTime.Now,
                //        AttachmentGUID = ""
                //    };

                //    db.EDownloadStaging.Add(_stage);
                //    db.SaveChanges();
                //}

                List<EDownloadStaging> _staged = db.EDownloadStaging.AsNoTracking().Where(x => x.DownloadStagingID == _downloadID).ToList();

                return _staged;
            }
        }

        public Int64 InsertLoan(Loan _loan, LoanSearch _loanSearch, LoanLOSFields _loanLOSField, Int64 DownloadID)
        {
            Int64 AttachmentId = 0;

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Guid encompassGUID = _loan.EnCompassLoanGUID.GetValueOrDefault();
                    ELoanAttachmentDownload _exDownload = db.ELoanAttachmentDownload.AsNoTracking().Where(e => (e.ID == DownloadID)).FirstOrDefault();
                    if (_exDownload != null && _exDownload.LoanID > 0)
                    {
                        //_exDownload.Error = "";
                        //_exDownload.ModifiedOn = DateTime.Now;
                        //_exDownload.Status = EncompassStatusConstant.DOWNLOAD_PENDING;
                        //db.Entry(_exDownload).State = System.Data.Entity.EntityState.Modified;
                        //db.SaveChanges();

                        AttachmentId = _exDownload.ID;
                        //loanupdate
                        Loan _dbloan = db.Loan.AsNoTracking().Where(m => m.LoanID == _exDownload.LoanID).FirstOrDefault();
                        if (_dbloan != null)
                        {
                            _dbloan.CustomerID = _loan.CustomerID;
                            _dbloan.ReviewTypeID = _loan.ReviewTypeID;
                            _dbloan.LoanTypeID = _loan.LoanTypeID;
                            _dbloan.LoanNumber = _loan.LoanNumber;
                            _dbloan.Status = _loan.Status;
                            _dbloan.LoanGUID = _loan.LoanGUID;
                            _dbloan.Priority = _loan.Priority;
                            _dbloan.SubStatus = _loan.SubStatus;
                            _dbloan.UploadedUserID = _loan.UploadedUserID;
                            _dbloan.LoggedUserID = _loan.LoggedUserID;
                            _dbloan.FileName = _loan.FileName;
                            _dbloan.UploadType = _loan.UploadType;
                            _dbloan.LastAccessedUserID = _loan.LastAccessedUserID;
                            _dbloan.AuditMonthYear = _loan.AuditMonthYear;
                            _dbloan.AssignedUserID = _loan.AssignedUserID;
                            _dbloan.EnCompassLoanGUID = _loan.EnCompassLoanGUID;
                            _dbloan.ModifiedOn = DateTime.Now;

                            db.Entry(_dbloan).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            string[] auditDescs1 = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_STATUS_MODIFIED);
                            LoanAudit.InsertLoanAudit(db, _dbloan, auditDescs1[0], auditDescs1[1]);
                            _loan.LoanID = _dbloan.LoanID;
                        }

                        //loansearch update

                        LoanSearch loanSearch = db.LoanSearch.Where(x => x.LoanID == _exDownload.LoanID).FirstOrDefault();
                        if (loanSearch != null)
                        {
                            loanSearch.LoanID = _loan.LoanID;
                            loanSearch.LoanTypeID = _loanSearch.LoanTypeID;
                            loanSearch.LoanNumber = _loanSearch.LoanNumber;
                            loanSearch.BorrowerName = _loanSearch.BorrowerName;
                            loanSearch.ReceivedDate = _loanSearch.CreatedOn;
                            loanSearch.Status = _loanSearch.Status;
                            loanSearch.SSN = _loanSearch.SSN;
                            loanSearch.CustomerID = _loan.CustomerID;
                            loanSearch.ModifiedOn = DateTime.Now;
                            db.Entry(loanSearch).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.SEARCH_FIELDS_INSERTED);
                            LoanAudit.InsertLoanSearchAudit(db, loanSearch != null ? loanSearch : loanSearch, auditDescs[0], auditDescs[1]);
                        }
                        else
                        {
                            _loanSearch.LoanID = _loan.LoanID;
                            db.LoanSearch.Add(_loanSearch);
                            db.SaveChanges();
                        }

                        //LoanLOSFields update
                        LoanLOSFields rm = db.LoanLOSFields.AsNoTracking().Where(r => r.LoanID == _exDownload.LoanID).FirstOrDefault();
                        if (rm != null)
                        {
                            rm.Closer = _loanLOSField.Closer;
                            rm.LoanOfficer = _loanLOSField.LoanOfficer;
                            rm.PostCloser = _loanLOSField.PostCloser;
                            rm.Underwriter = _loanLOSField.Underwriter;
                            rm.EmailCloser = _loanLOSField.EmailCloser;
                            rm.EmailLoanOfficer = _loanLOSField.EmailLoanOfficer;
                            rm.EmailPostCloser = _loanLOSField.EmailPostCloser;
                            rm.EmailUnderwriter = _loanLOSField.EmailUnderwriter;
                            db.Entry(rm).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            _loanLOSField.LoanID = _loan.LoanID;
                            db.LoanLOSFields.Add(_loanLOSField);
                            db.SaveChanges();
                        }

                    }
                    else
                    {
                        db.Loan.Add(_loan);
                        db.SaveChanges();

                        _exDownload.LoanID = _loan.LoanID;
                        _exDownload.ModifiedOn = DateTime.Now;
                        db.Entry(_exDownload).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_ENCOMPASS);
                        LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], auditDescs[1]);

                        if (_loan.LoanID != 0)
                        {
                            //_exDownload = new ELoanAttachmentDownload()
                            //{
                            //    LoanID = _loan.LoanID,
                            //    ELoanGUID = _loan.EnCompassLoanGUID.GetValueOrDefault(),
                            //    Status = EncompassStatusConstant.DOWNLOAD_PENDING,
                            //    CreatedOn = DateTime.Now,
                            //    ModifiedOn = DateTime.Now,
                            //    TypeOfDownload = EncompassLoanAttachmentDownloadConstant.Loan,
                            //    Error = ""
                            //};

                            //db.ELoanAttachmentDownload.Add(_exDownload);
                            //db.SaveChanges();
                            AttachmentId = _exDownload.ID;

                            _loanLOSField.LoanID = _loan.LoanID;
                            db.LoanLOSFields.Add(_loanLOSField);

                            _loanSearch.LoanID = _loan.LoanID;
                            db.LoanSearch.Add(_loanSearch);

                            string[] auditDescSearch = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.SEARCH_FIELDS_INSERTED);
                            LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditDescSearch[0], auditDescSearch[1]);

                            db.SaveChanges();
                        }
                    }
                    tran.Commit();
                }
            }

            return AttachmentId;
        }




        //public List<string> GetLoans()
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //       return db.ELoanAttachmentDownload.AsNoTracking().Where(x => (x.Status == EncompassStatusConstant.DOWNLOAD_RETRY) && (x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan) ).Select(s => s.ELoanGUID.ToString()).ToList(); 
        //    }
        //}

        public void UpdateDownloadStaging(Int64 _downloadStagingID, Int64 _status, string errMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EDownloadStaging _fieldUpdate = db.EDownloadStaging.AsNoTracking().Where(x => x.ID == _downloadStagingID && x.Step == EncompassDownloadStepConstant.UpdateField).FirstOrDefault();

                if (_fieldUpdate != null)
                {
                    _fieldUpdate.Status = _status;
                    _fieldUpdate.ModifiedOn = DateTime.Now;
                    _fieldUpdate.Error = errMsg;
                    db.Entry(_fieldUpdate).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public Int64 InsertEDownload(Guid _eLoanGUID, Int64 _status, string errMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentDownload _eAttachment = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.ELoanGUID == _eLoanGUID && x.Status == EncompassStatusConstant.DOWNLOAD_RETRY && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan).FirstOrDefault();

                if (_eAttachment != null)
                {
                    _eAttachment.Status = _status;
                    _eAttachment.Error = errMsg;
                    _eAttachment.ModifiedOn = DateTime.Now;
                    db.Entry(_eAttachment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {

                    _eAttachment = new ELoanAttachmentDownload()
                    {
                        LoanID = 0,
                        ELoanGUID = _eLoanGUID,
                        Status = EncompassStatusConstant.DOWNLOAD_PENDING,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        TypeOfDownload = EncompassLoanAttachmentDownloadConstant.Loan,
                        Error = ""
                    };
                    db.ELoanAttachmentDownload.Add(_eAttachment);
                    db.SaveChanges();
                }

                return _eAttachment.ID;
            }
        }

        public void UpdateEDownloadStatus(Int64 _downloadID, Int64 _status, string LoanNumber, string errMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentDownload _eAttachment = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.ID == _downloadID && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan).FirstOrDefault();

                if (_eAttachment != null)
                {
                    Loan _loan = null;
                    if (_eAttachment.LoanID > 0)
                        _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == _eAttachment.LoanID).FirstOrDefault();

                    if (_loan != null)
                    {
                        Int64 loanStatus = StatusConstant.READY_FOR_IDC;
                        string AuditMsg = "Attachment Downloaded From Encompass";

                        if (_status == EncompassStatusConstant.DOWNLOAD_FAILED)
                        {
                            loanStatus = StatusConstant.FAILED_ENCOMPASS_DOWNLOAD;
                            AuditMsg = "Attachment Download Failed From Encompass";
                        }

                        _loan.Status = loanStatus;
                        _loan.ModifiedOn = DateTime.Now;
                        db.Entry(_loan).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        LoanAudit.InsertLoanAudit(db, _loan, AuditMsg, AuditMsg);

                        LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == _eAttachment.LoanID).FirstOrDefault();
                        if (_loanSearch != null)
                        {
                            _loanSearch.Status = loanStatus;
                            _loanSearch.ModifiedOn = DateTime.Now;
                            db.Entry(_loanSearch).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            LoanAudit.InsertLoanSearchAudit(db, _loanSearch, AuditMsg, AuditMsg);
                        }
                    }

                    _eAttachment.Status = _status;
                    _eAttachment.ELoanNumber = LoanNumber;
                    _eAttachment.ModifiedOn = DateTime.Now;
                    _eAttachment.Error = errMsg;
                    db.Entry(_eAttachment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void DeleteEDownloadStatus(Int64 _downloadID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentDownload _eAttachment = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.ID == _downloadID && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan).FirstOrDefault();

                if (_eAttachment != null)
                {
                    Loan _loan = null;
                    if (_eAttachment.LoanID > 0)
                        _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == _eAttachment.LoanID).FirstOrDefault();

                    if (_loan != null)
                    {
                        db.Entry(_loan).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();

                        db.AuditLoan.RemoveRange(db.AuditLoan.Where(x => x.LoanID == _loan.LoanID));
                        db.SaveChanges();

                        LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == _eAttachment.LoanID).FirstOrDefault();
                        if (_loanSearch != null)
                        {
                            db.Entry(_loanSearch).State = System.Data.Entity.EntityState.Deleted;
                            db.SaveChanges();

                            db.AuditLoanSearch.RemoveRange(db.AuditLoanSearch.Where(x => x.LoanID == _loan.LoanID));
                            db.SaveChanges();
                        }

                        LoanLOSFields _loanLOSFields = db.LoanLOSFields.AsNoTracking().Where(x => x.LoanID == _eAttachment.LoanID).FirstOrDefault();
                        if (_loanLOSFields != null)
                        {
                            db.Entry(_loanLOSFields).State = System.Data.Entity.EntityState.Deleted;
                            db.SaveChanges();
                        }
                    }

                    db.EDownloadStaging.RemoveRange(db.EDownloadStaging.Where(x => x.DownloadStagingID == _eAttachment.ID));
                    db.SaveChanges();

                    db.Entry(_eAttachment).State = System.Data.Entity.EntityState.Deleted;
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
                    //_loan.ModifiedOn = DateTime.Now;
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

        //code added by mani
        public List<IntellaAndEncompassFetchFields> GetEncompassImportFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType.Contains(LOSFieldType.IMPORT) && m.Active && m.TenantSchema == TenantSchema).ToList();
            }
        }

        public List<IntellaAndEncompassFetchFields> GetEncompassSearchFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType.Contains(LOSFieldType.LOANSEARCH) && m.Active && m.TenantSchema == TenantSchema).ToList();
            }
        }

        public List<string> GetEncompassExceptionLoans()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.Status == EncompassStatusConstant.DOWNLOAD_FAILED && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan).Select(x => x.ELoanGUID.ToString()).ToList();
            }
        }


        public List<IntellaAndEncompassFetchFields> GetEncompassLookUpFields(string tenantSchema)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType.Contains(LOSFieldType.LOOKUP) && m.Active && m.TenantSchema.ToLower().Equals(tenantSchema.ToLower())).ToList();
            }
        }

        public Int64 GetCustomerID(string CustomerName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerMaster cm = db.CustomerMaster.AsNoTracking().Where(r => r.CustomerName == CustomerName).FirstOrDefault();

                if (cm != null)
                    return cm.CustomerID;

                return 0;
            }
        }

        public bool CheckCustomerReviewTypeMapping(Int64 CustomerID, Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewMapping.AsNoTracking().Any(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID);
            }
        }
        public string GetCustomerByID(Int64 CustomerID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerMaster cm = db.CustomerMaster.AsNoTracking().Where(r => r.CustomerID == CustomerID).FirstOrDefault();

                if (cm != null)
                    return cm.CustomerName;

                return string.Empty;
            }
        }
        public Int64 GetLoanTypeID(string LoanTypeName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanTypeMaster lt = db.LoanTypeMaster.AsNoTracking().Where(r => r.LoanTypeName == LoanTypeName).FirstOrDefault();

                if (lt != null)
                    return lt.LoanTypeID;

                return 0;
            }
        }
        public bool CheckCustomerReviewLoanTypeMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanMapping.AsNoTracking().Any(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID);
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

        public List<EWebhookEvents> GetWebHooksEvent()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.EWebhookEvents.AsNoTracking().Where(r => r.EventType == EWebHookEventsLogConstant.DOCUMENT_LOG && r.Status == EWebHookStatusConstant.EWEB_HOOK_STAGED).ToList();
            }
        }
    }
}
