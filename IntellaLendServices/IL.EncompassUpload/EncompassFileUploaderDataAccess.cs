using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.EncompassUpload
{
    public class EncompassFileUploaderDataAccess
    {
        public string TenantSchema;
        private static string SystemSchema = "IL";
        public EncompassFileUploaderDataAccess(string tenantSchema)
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
        public List<ELoanAttachmentUpload> GetUploadWaitingandRetryLoans()
        {
            List<ELoanAttachmentUpload> UploadWaitingRetryLoans = new List<ELoanAttachmentUpload>();
            using (var db = new DBConnect(TenantSchema))
            {
                UploadWaitingRetryLoans = db.ELoanAttachmentUpload.AsNoTracking().Where(x => x.Status == EncompassUploadConstant.UPLOAD_WAITING || x.Status == EncompassUploadConstant.UPLOAD_RETRY).ToList();
                return UploadWaitingRetryLoans;
            }

        }

        //public List<ELoanAttachmentUpload> GetUploadRetryLoans()
        //{
        //    List<ELoanAttachmentUpload> UploadRetryLoans = new List<ELoanAttachmentUpload>();
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        UploadRetryLoans = db.ELoanAttachmentUpload.AsNoTracking().Where(x => x.Status == EncompassUploadConstant.UPLOAD_RETRY).ToList();
        //        return UploadRetryLoans;
        //    }

        //}

        public Batch GetBatchDetails(Int64 loanID)
        {
            Batch _Batch = new Batch();
            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == loanID).FirstOrDefault();
                if (_loanDetail != null)
                {
                    _Batch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);
                }
            }
            return _Batch;
        }

        public Int64 GetTypeOfUpload(Int64 loanID)
        {
            Int64 _TypeOfUpload = 0;
            using (var db = new DBConnect(SystemSchema))
            {
                ELoanAttachmentUpload _eLoanAttachmentUpload = db.ELoanAttachmentUpload.AsNoTracking().Where(l => l.LoanID == loanID).ToList().FirstOrDefault();
                if (_eLoanAttachmentUpload != null)
                {
                    _TypeOfUpload = _eLoanAttachmentUpload.TypeOfUpload;
                }

            }
            return _TypeOfUpload;


        }

        public List<EUploadStaging> SaveDocumentDetails(Int64 loanid, Int64 typeofupload, Int64 UploadStagID, Batch Batchobj)
        {
            List<EUploadStaging> uploadLoans = new List<EUploadStaging>();

            using (var db = new DBConnect(TenantSchema))
            {
                List<DocumentUpload> documents = new List<DocumentUpload>();
                if (typeofupload == EncompassLoanAttachmentDownloadConstant.TrailingDocuments)
                {

                    ELoanAttachmentUpload _attachUpload = db.ELoanAttachmentUpload.AsNoTracking().Where(x => x.ID == UploadStagID).FirstOrDefault();
                    if (_attachUpload != null)
                    {
                        documents = JsonConvert.DeserializeObject<List<DocumentUpload>>(_attachUpload.Documents);
                    }
                }
                else
                {
                    db.EUploadStaging.RemoveRange(db.EUploadStaging.Where(x => x.UploadStagingID == UploadStagID));
                    db.SaveChanges();
                    documents = Batchobj.Documents.Select(x => new DocumentUpload() { DocumentName = x.Type, Version = x.VersionNumber }).ToList();
                }

                foreach (var item in documents)
                {
                    db.EUploadStaging.Add(new EUploadStaging()
                    {
                        UploadStagingID = UploadStagID,
                        LoanID = loanid,
                        TypeOfUpload = typeofupload,
                        Document = item.DocumentName,
                        Version = item.Version,
                        Status = EncompassUploadStagingConstant.UPLOAD_STAGING_WAITING,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        EParkingSpot = string.Empty,
                        ErrorMsg = string.Empty
                    });

                    db.SaveChanges();

                }
                uploadLoans = db.EUploadStaging.AsNoTracking().Where(l => l.UploadStagingID == UploadStagID && l.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_WAITING && l.TypeOfUpload != EncompassLoanAttachmentDownloadConstant.RuleResult).ToList();

                return uploadLoans;
            }
        }

        public List<EUploadStaging> GetRetryLoans(Int64 ID, Int64 typeOfUpload)
        {
            List<EUploadStaging> RetryLoans = new List<EUploadStaging>();

            using (var db = new DBConnect(TenantSchema))
            {
                RetryLoans = db.EUploadStaging.AsNoTracking().Where(x => x.UploadStagingID == ID && x.TypeOfUpload == typeOfUpload && (x.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_RETRY || x.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_WAITING) && x.TypeOfUpload != EncompassLoanAttachmentDownloadConstant.RuleResult).ToList();
            }
            return RetryLoans;
        }

        public List<EUploadStaging> GetFailedLoans()
        {
            List<EUploadStaging> FailedLoan = new List<EUploadStaging>();
            using (var db = new DBConnect(TenantSchema))
            {

                FailedLoan = db.EUploadStaging.AsNoTracking().Where(x => x.Status == EncompassUploadConstant.UPLOAD_FAILED).ToList();

            }
            return FailedLoan;
        }
        public bool UpdateUploadStaging(Int64 uploadStagID, Int64 status, string EParkingSpot, string ErrMsg = "")
        {

            using (var db = new DBConnect(TenantSchema))
            {
                EUploadStaging _staging = db.EUploadStaging.AsNoTracking().Where(k => k.ID == uploadStagID).FirstOrDefault();
                if (_staging != null)
                {
                    _staging.EParkingSpot = _staging.EParkingSpot != string.Empty ? _staging.EParkingSpot : EParkingSpot;
                    _staging.Status = status;
                    _staging.ModifiedOn = DateTime.Now;
                    _staging.ErrorMsg = ErrMsg;
                    db.Entry(_staging).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }


        }

        public void RemoveUploadedStagingDetail(Int64 uploadStagID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EUploadStaging _staging = db.EUploadStaging.AsNoTracking().Where(k => k.ID == uploadStagID).FirstOrDefault();
                if (_staging != null)
                {
                    db.EUploadStaging.RemoveRange(db.EUploadStaging.Where(x => x.ID == uploadStagID));
                    db.SaveChanges();
                }
            }
        }

        public bool IsSuccessfullUpload(Int64 uploadStagID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EUploadStaging _staging = db.EUploadStaging.AsNoTracking().Where(k => k.UploadStagingID == uploadStagID).FirstOrDefault();
                if (_staging != null)
                {
                    return false;
                }
                return true;
            }
        }

        public void RemoveUploadStaging(Int64 uploadStagID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentUpload _eLoanAttachmentDownload = db.ELoanAttachmentUpload.AsNoTracking().Where(x => x.ID == uploadStagID).FirstOrDefault();

                if (_eLoanAttachmentDownload != null)
                {
                    db.ELoanAttachmentUpload.RemoveRange(db.ELoanAttachmentUpload.Where(x => x.ID == uploadStagID));
                    db.SaveChanges();
                }
            }
        }

        public string GetRuleEvaluateResult(Int64 loanID)
        {
            string checkListEvaluatedresult = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                LoanEvaluatedResult evalResult = db.LoanEvaluatedResult.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                if (evalResult != null)
                {
                    checkListEvaluatedresult = evalResult.EvaluatedResult;

                }

            }
            return checkListEvaluatedresult;
        }
        public string GetParkingSpotName(string DocName)
        {
            string EParkDocName = string.Empty;
            using (var db = new DBConnect(SystemSchema))
            {
                DocumentTypeMaster Doc = db.DocumentTypeMaster.AsNoTracking().Where(l => l.Name == DocName).FirstOrDefault();
                if (Doc != null)
                {
                    EncompassParkingSpot EParkDoc = db.EncompassParkingSpot.AsNoTracking().Where(x => x.DocumentTypeID == Doc.DocumentTypeID).FirstOrDefault();
                    if (EParkDoc != null)
                        return EParkDoc.ParkingSpotName;
                }
            }

            return EParkDocName;
        }


        public string GetDocNameFromTenant(Int64 DocumentTypeID)
        {
            string docname = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                docname = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == DocumentTypeID).FirstOrDefault().Name.ToString();

            }
            return docname;


        }

        public string GetStatusDescription(Int64 statusId)
        {
            string StatusDescription = string.Empty;
            using (var db = new DBConnect(SystemSchema))
            {
                StatusDescription = db.WorkFlowStatusMaster.AsNoTracking().Where(s => s.StatusID == statusId).FirstOrDefault().StatusDescription;

            }
            return StatusDescription;


        }


        public LoanInfo GetLoanSearchValue(Int64 Loanid)
        {


            LoanInfo LoanHeader = new LoanInfo();

            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == Loanid).FirstOrDefault();
                LoanSearch _search = db.LoanSearch.AsNoTracking().Where(d => d.LoanID == Loanid).FirstOrDefault();
                IDCFields _idcFields = db.IDCFields.AsNoTracking().Where(d => d.LoanID == Loanid).FirstOrDefault();

                LoanHeader.LoanNumber = _loan.LoanNumber;
                LoanHeader.ServiceType = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == _loan.ReviewTypeID).FirstOrDefault().ReviewTypeName;
                LoanHeader.LoanType = db.LoanTypeMaster.AsNoTracking().Where(r => r.LoanTypeID == _loan.LoanTypeID).FirstOrDefault().LoanTypeName;
                LoanHeader.LoanStatus = GetStatusDescription(_loan.Status);
                LoanHeader.LoanAmount = _search.LoanAmount;
                LoanHeader.PostCloser = _idcFields.PostCloser;
                LoanHeader.LoanOfficer = _idcFields.LoanOfficer;
                LoanHeader.UnderWriter = _idcFields.UnderWritter;
                LoanHeader.AuditMonthYear = _loan.AuditMonthYear;
                LoanHeader.AuditDueDate = _search.AuditDueDate;
                LoanHeader.BorrowerName = _search.BorrowerName;
                LoanHeader.ReceivedDate = _search.ReceivedDate;
                LoanHeader.InvestorLoanNumber = _search.InvestorLoanNumber;
                LoanHeader.PropertyAddress = _search.PropertyAddress;

            }
            return LoanHeader;
        }

        public Int32 GetRulePDFVersion(Int64 UploadID, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<EUploadStaging> _lsRuleResult = db.EUploadStaging.AsNoTracking().Where(x => x.LoanID == LoanID && x.TypeOfUpload == EncompassLoanAttachmentDownloadConstant.RuleResult).ToList();

                return _lsRuleResult.Count;
            }
        }

        public Int64 InsertRuleResultStaging(Int64 UploadID, Int64 LoanID, string DocName, Int32 Version)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                EUploadStaging _eLoanAttachmentDownload = new EUploadStaging()
                {
                    UploadStagingID = UploadID,
                    Document = DocName,
                    Version = Version,
                    LoanID = LoanID,
                    EParkingSpot = string.Empty,
                    Status = EncompassUploadStagingConstant.UPLOAD_STAGING_PROCESSING,
                    ErrorMsg = string.Empty,
                    ModifiedOn = DateTime.Now,
                    CreatedOn = DateTime.Now,
                    TypeOfUpload = EncompassLoanAttachmentDownloadConstant.RuleResult,
                };

                db.EUploadStaging.Add(_eLoanAttachmentDownload);
                db.SaveChanges();


                return _eLoanAttachmentDownload.ID;
            }
        }

        public void UpdateEncompassUploadStatus(Int64 UploadID, Int64 Uploadstatus, string ErrorMsg = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentUpload _eLoanAttachmentDownload = db.ELoanAttachmentUpload.AsNoTracking().Where(x => x.ID == UploadID).FirstOrDefault();

                if (Uploadstatus == EncompassUploadConstant.UPLOAD_RETRY)
                {
                    List<EUploadStaging> _lsEUploadStaging = db.EUploadStaging.AsNoTracking().Where(x => x.UploadStagingID == UploadID).ToList();
                    foreach (var item in _lsEUploadStaging)
                    {
                        if (Uploadstatus != EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE)
                        {
                            item.Status = EncompassUploadStagingConstant.UPLOAD_STAGING_RETRY;
                            item.ModifiedOn = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                }

                if (_eLoanAttachmentDownload != null)
                {
                    _eLoanAttachmentDownload.Status = Uploadstatus;
                    _eLoanAttachmentDownload.Error = ErrorMsg;
                    _eLoanAttachmentDownload.ModifiedOn = DateTime.Now;
                    db.Entry(_eLoanAttachmentDownload).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}

