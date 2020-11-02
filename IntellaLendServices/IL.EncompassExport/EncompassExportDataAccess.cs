using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IL.EncompassExport
{
    public class EncompassExportDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public EncompassExportDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Properties

        public static string Server
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == ApplicationConfiguration.ENCOMPASS_SERVER).FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }

        public static string UserName
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == ApplicationConfiguration.ENCOMPASS_USERNAME).FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }

        public static string Password
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == ApplicationConfiguration.ENCOMPASS_PASSWORD).FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }

        #endregion

        #region Public Methods

        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.AsNoTracking().Where(m => m.Active == true).ToList();
            }
        }

        public static List<IntellaAndEncompassFetchFields> GetEncompassImportFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType.Contains(LOSFieldType.IMPORT) && m.Active).ToList();
            }
        }

        public static List<IntellaAndEncompassFetchFields> GetEncompassLookUpFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType.Contains(LOSFieldType.LOOKUP) && m.Active).ToList();
            }
        }

        public static List<IntellaAndEncompassFetchFields> GetEncompassSearchFields()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.IntellaAndEncompassFetchFields.AsNoTracking().Where(m => m.FieldType.Contains(LOSFieldType.LOANSEARCH) && m.Active).ToList();
            }
        }

        public List<string> GetEncompassExceptionLoans()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<EncompassDownloadExceptions> lsEnException =  db.EncompassDownloadExceptions.AsNoTracking().Where(m => m.Status == EncompassExceptionStatus.FAILED_LOAN).ToList();

                List<string> lsGUIDs = new List<string>();

                foreach (EncompassDownloadExceptions item in lsEnException)
                {
                    Guid _guid = item.EncompassGuid.GetValueOrDefault();

                    if (_guid != null)
                        lsGUIDs.Add(_guid.ToString().ToUpper());
                }

                return lsGUIDs;
            }
        }

        public static Dictionary<string, string> GetEncompassParkingSpot(bool IsConfiguredSpot, string UnMappedParkingSpotNameInIntellaLend)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                Dictionary<string, string> _enILMappings = new Dictionary<string, string>();

                List<EncompassParkingSpot> enParkingSpotLs = new List<EncompassParkingSpot>();

                if (IsConfiguredSpot)
                {
                    enParkingSpotLs = db.EncompassParkingSpot.AsNoTracking().Where(e => e.Active).ToList();
                }
                else
                {
                    enParkingSpotLs = db.EncompassParkingSpot.AsNoTracking().ToList();
                }

                foreach (EncompassParkingSpot item in enParkingSpotLs)
                {
                    if (item.DocumentTypeID != null)
                    {
                        DocumentTypeMaster dm = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                        if (dm != null)
                        {
                            _enILMappings[item.ParkingSpotName] = dm.Name;
                        }
                        else
                            _enILMappings[item.ParkingSpotName] = UnMappedParkingSpotNameInIntellaLend;
                    }
                    else
                        _enILMappings[item.ParkingSpotName] = UnMappedParkingSpotNameInIntellaLend;
                }

                return _enILMappings;
            }
        }

        public static bool IsConfiguredSpots
        {
            get
            {
                bool result = false;

                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey.ToUpper() == "IS_CONFIGURED_PARKING_SPOT").FirstOrDefault();

                    if (appConfig != null)
                        Boolean.TryParse(appConfig.ConfigValue, out result);
                }
                return result;
            }
        }

        public bool CheckCustomerReviewTypeMapping(Int64 CustomerID, Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewMapping.AsNoTracking().Any(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID);
            }
        }

        public bool CheckCustomerReviewLoanTypeMapping(Int64 CustomerID, Int64 ReviewTypeID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanMapping.AsNoTracking().Any(r => r.CustomerID == CustomerID && r.ReviewTypeID == ReviewTypeID && r.LoanTypeID == LoanTypeID);
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

        public void SetEncompassDocPages(Int64 LoanID, string EncompassDocPages)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanLOSFields rm = db.LoanLOSFields.AsNoTracking().Where(r => r.LoanID == LoanID).FirstOrDefault();

                if (rm != null)
                {
                    rm.EncompassDocPages = EncompassDocPages;
                    db.Entry(rm).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

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

        public Int64 GetUserID(string UserName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Dictionary<Int64, string> _userList = db.Users.AsNoTracking().ToDictionary(t => t.UserID, t => (t.FirstName + " " + t.LastName));

                var user = _userList.Where(d => d.Value.ToLower() == UserName.ToLower()).FirstOrDefault();

                if (!string.IsNullOrEmpty(user.Value))
                    return user.Key;
                else
                    return 0;
            }
        }


        public string GetReviewTypeName(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster rm = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == ReviewTypeID).FirstOrDefault();

                if (rm != null)
                    return rm.ReviewTypeName;

                return string.Empty;
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


        public Int64 InsertLoan(Loan _loan, LoanLOSFields _loanLOSField, LoanSearch loanSearch)
        {
            Int64 LoanID = 0;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {

                    db.Loan.Add(_loan);
                    db.SaveChanges();

                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_ENCOMPASS);
                    LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], auditDescs[1]);

                    if (_loan.LoanID != 0)
                    {
                        _loanLOSField.LoanID = _loan.LoanID;
                        db.LoanLOSFields.Add(_loanLOSField);
                        db.SaveChanges();


                        loanSearch.LoanID = _loan.LoanID;
                        db.LoanSearch.Add(loanSearch);
                        db.SaveChanges();

                        auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.SEARCH_FIELDS_INSERTED);
                        LoanAudit.InsertLoanSearchAudit(db, loanSearch, auditDescs[0], auditDescs[1]);

                        LoanID = _loan.LoanID;
                    }

                    tran.Commit();
                }

                return LoanID;
            }
        }

        public void UpdateEncompassExceptionLoan(string LoanGUID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Guid _loanGuid = new Guid(LoanGUID);

                EncompassDownloadExceptions _exDownload = db.EncompassDownloadExceptions.AsNoTracking().Where(e => e.EncompassGuid == _loanGuid).FirstOrDefault();

                if (_exDownload != null)
                {
                    //_exDownload.ExceptionMessage = ex.Message;
                    //_exDownload.ExceptionStackTrace = ex.StackTrace;
                    _exDownload.ModifiedOn = DateTime.Now;
                    _exDownload.Status = EncompassExceptionStatus.SUCCESSFUL_RETRY;
                    db.Entry(_exDownload).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        public void InsertEncompassExceptionLoan(string LoanNumber, string LoanGUID, Exception ex)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Guid _loanGuid = new Guid(LoanGUID);

                EncompassDownloadExceptions _exDownload = db.EncompassDownloadExceptions.AsNoTracking().Where(e => e.EncompassGuid == _loanGuid).FirstOrDefault();

                if (_exDownload != null)
                {
                    _exDownload.EncompassLoanNumber = LoanNumber;
                    _exDownload.ExceptionMessage = ex.Message;
                    _exDownload.ExceptionStackTrace = ex.StackTrace;
                    _exDownload.ModifiedOn = DateTime.Now;
                    //_exDownload.RetryCount = _exDownload.RetryCount + 1;
                    _exDownload.Status = EncompassExceptionStatus.FAILED_LOAN;
                    db.Entry(_exDownload).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {

                    _exDownload = new EncompassDownloadExceptions()
                    {
                        EncompassGuid = new Guid(LoanGUID),
                        EncompassLoanNumber = LoanNumber,
                        ExceptionMessage = ex.Message,
                        ExceptionStackTrace = ex.StackTrace,
                        RetryCount = 0,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        Status = EncompassExceptionStatus.FAILED_LOAN
                    };

                    db.EncompassDownloadExceptions.Add(_exDownload);
                }
                db.SaveChanges();
            }
        }

        public void RemoveInsertLoan(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (db.Loan.Where(l => l.LoanID == LoanID).FirstOrDefault() != null)
                    {
                        db.AuditLoan.RemoveRange(db.AuditLoan.Where(l => l.LoanID == LoanID));
                        db.SaveChanges();

                        db.LoanLOSFields.RemoveRange(db.LoanLOSFields.Where(l => l.LoanID == LoanID));
                        db.SaveChanges();

                        db.LoanSearch.RemoveRange(db.LoanSearch.Where(l => l.LoanID == LoanID));
                        db.SaveChanges();

                        db.AuditLoanSearch.RemoveRange(db.AuditLoanSearch.Where(l => l.LoanID == LoanID));
                        db.SaveChanges();

                        db.Loan.RemoveRange(db.Loan.Where(l => l.LoanID == LoanID));
                        db.SaveChanges();
                    }

                    tran.Commit();
                }
            }
        }

        public void UpdateLoan(string TableName, string DestinationColumn, string DestinationValue)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                db.Database.ExecuteSqlCommand($"Update [{TenantSchema}].[{TableName}] set {DestinationColumn} = '{DestinationValue}' where studentid = 1");
            }
        }


        #endregion

    }
}
