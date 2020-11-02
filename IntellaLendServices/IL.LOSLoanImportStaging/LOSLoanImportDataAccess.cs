using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.LOSLoanImport
{
    public class LOSLoanImportDataAccess
    {
        public static string SystemSchema = "IL";
        public string TenantSchema;

        public LOSLoanImportDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }
        public static List<TenantMaster> GetAllTenants()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
            }
        }

        public CustomerMaster CheckCustomerExists(string _lenderCode)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustomerMaster.AsNoTracking().Where(m => m.CustomerCode == _lenderCode).FirstOrDefault();
            }
        }

        public ReviewTypeMaster CheckReviewTypeExists(string reviewType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.ReviewTypeMaster.AsNoTracking().Where(m => m.ReviewTypeName == reviewType).FirstOrDefault();
            }
        }

        public LoanTypeMaster CheckLoanTypeExists(string loanType)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.LoanTypeMaster.AsNoTracking().Where(m => m.LoanTypeName == loanType).FirstOrDefault();
            }
        }

        public bool CheckMappingExists(Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanMapping.AsNoTracking().Where(m => m.CustomerID == _customerID && m.ReviewTypeID == _reviewTypeID && m.LoanTypeID == _loanTypeID && m.Active == true).Any();
            }
        }

        public Int64 UpdateImportStagingTable(string _fileName, Int64 _customerID, Int64 _reviewTypeID, Int64 _loanTypeID, Int64 _loanID, bool _validJSON, Int32 _status, string _errorMSG = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSImportStaging _import = new LOSImportStaging()
                {
                    FileName = _fileName,
                    CustomerID = _customerID,
                    ReviewTypeID = _reviewTypeID,
                    LoanTypeID = _loanTypeID,
                    LoanID = _loanID,
                    ValidJson = _validJSON,
                    Status = _status,
                    ErrorMsg = _errorMSG,
                    Createdon = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                db.LOSImportStaging.Add(_import);
                db.SaveChanges();

                return _import.ID;
            }
        }

        public void UpdateLoanTable(Int64 _loanID, Int64 _status)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == _loanID).FirstOrDefault();

                if (_loan != null)
                {
                    _loan.Status = _status;
                    _loan.ModifiedOn = DateTime.Now;
                    db.Entry(_loan).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == _loanID).FirstOrDefault();
                    if (_loanSearch != null)
                    {
                        _loanSearch.Status = _status;
                        _loanSearch.ModifiedOn = DateTime.Now;
                        db.Entry(_loanSearch).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }

        public void UpdateImportStagingTable(Int64 _stagingID, Int64 _loanID, Int32 _status, string _errorMSG = "")
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSImportStaging _import = db.LOSImportStaging.AsNoTracking().Where(x => x.ID == _stagingID).FirstOrDefault();

                if (_import != null)
                {
                    _import.LoanID = _loanID;
                    _import.Status = _status;
                    _import.ErrorMsg = _errorMSG;
                    _import.ModifiedOn = DateTime.Now;
                    db.Entry(_import).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public List<LOSDocumentFields> GetFannieMaeTemplate(string FannieMaeVersion)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Int64 _doc = GetFannieMaeDocumentID(FannieMaeVersion);
                if (_doc > 0)
                    return db.LOSDocumentFields.AsNoTracking().Where(x => x.LOSDocumentID == _doc).ToList();
                else
                    return new List<LOSDocumentFields>();
            }
        }

        public Int64 CheckLoanExists(string _loanNumber)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanNumber == _loanNumber && x.UploadType == UploadConstant.LOS && x.Status != StatusConstant.LOAN_DELETED).FirstOrDefault();
                if (_loan != null)
                    return _loan.LoanID;
                else
                    return 0;
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


        public Int64 GetFannieMaeDocumentID(string FannieMaeVersion)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSDocument _doc = db.LOSDocument.AsNoTracking().Where(x => x.Version == FannieMaeVersion).FirstOrDefault();
                if (_doc != null)
                    return _doc.LOSDocumentID;
                else
                    return 0;
            }
        }

        public void InsertFannieMaeDetails(Int64 _loanID, Int64 _LOSDocumentID, string _fannieMaeJSON)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSLoanDetails _detail = new LOSLoanDetails()
                {
                    LoanID = _loanID,
                    LOSDetailJSON = _fannieMaeJSON,
                    LOSDocumentID = _LOSDocumentID,
                    Createdon = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                db.LOSLoanDetails.Add(_detail);
                db.SaveChanges();
            }
        }

        public Int64 CreateLoan(Loan loan)
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
                    loan.Status = StatusConstant.PENDING_LOS_DOWNLOAD;
                    if (loan.Priority == 0)
                    {
                        ReviewTypeMaster reviewTypeMaster = db.ReviewTypeMaster.AsNoTracking().Where(a => a.ReviewTypeID == loan.ReviewTypeID).FirstOrDefault();
                        loan.Priority = reviewTypeMaster != null && reviewTypeMaster.ReviewTypePriority.HasValue ? reviewTypeMaster.ReviewTypePriority.Value : 0;
                    }
                    db.Loan.Add(loan);
                    db.SaveChanges();

                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_LOS);
                    LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);

                    LoanSearch _loansearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == loan.LoanID).FirstOrDefault();
                    if (_loansearch == null)
                    {
                        _loansearch = new LoanSearch();
                        _loansearch.LoanTypeID = loan.LoanTypeID;
                        _loansearch.LoanNumber = loan.LoanNumber;
                        _loansearch.CustomerID = loan.CustomerID;
                        _loansearch.Status = loan.Status;
                        _loansearch.ReceivedDate = loan.CreatedOn;
                        _loansearch.ModifiedOn = DateTime.Now;
                        _loansearch.AuditDueDate = Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
                        _loansearch.CreatedOn = DateTime.Now;
                        _loansearch.LoanID = loan.LoanID;
                        db.LoanSearch.Add(_loansearch);
                        db.SaveChanges();
                    }

                    tran.Commit();

                    return loan.LoanID;
                }
            }
            return 0;
        }
    }
}
