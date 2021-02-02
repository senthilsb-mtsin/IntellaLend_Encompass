using EncompassConsoleConnector;
using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using IntellaLend.Model.Encompass;
using IntellaLend.RuleEngine;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace IntellaLend.EntityDataHandler
{
    public class
        LoanDataAccess
    {

        private string TenantSchema;
        private static string SystemSchema = "IL";
        #region Constructor

        public LoanDataAccess()
        { }

        public LoanDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods

        public object GetLoans(
            DateTime FromDate,
            DateTime ToDate,
            Int64 CurrentUserID,
            string LoanNumber,
            Int64 LoanType,
            string BorrowerName,
            string LoanAmount,
            Int64 ReviewStatus,
            DateTime? AuditMonthYear,
            Int64 ReviewType,
            Int64 Customer,
            string PropertyAddress,
            string InvestorLoanNumber,
            string PostCloser,
            string LoanOfficer,
            string UnderWriter,
            bool isWorkFlow,
            DateTime AuditDueDate,
            string[] SelectedLoanStatus
            )
        {
            List<LoanSearchReport> loans = new List<LoanSearchReport>();
            object loan;
            using (var db = new DBConnect(TenantSchema))
            {

                ToDate = ToDate.AddDays(1);
                //DateTime.MinValue
                DateTime? auditMonth = null;
                decimal loanAmount = 0;
                //if (AuditMonthYear != null)
                //    auditMonth = Convert.ToDateTime(AuditMonthYear);
                List<WorkFlowStatusMaster> wfMaster = new IntellaLendDataAccess().GetWorkFlowMaster();
                if (!string.IsNullOrEmpty(LoanAmount))
                    Decimal.TryParse(LoanAmount, out loanAmount);
                if (FromDate.Year == DateTime.MinValue.Year && ToDate.Year == DateTime.MinValue.Year && LoanNumber == "" && BorrowerName == "" &&
                   LoanAmount == "" && LoanType == 0 && ReviewStatus == 0 && AuditMonthYear == null && ReviewType == 0 && Customer == 0 && PropertyAddress == "" && InvestorLoanNumber == "" && isWorkFlow && AuditDueDate.Year == DateTime.MinValue.Year)
                {
                    loans = (from L in db.Loan.AsNoTracking()
                             join search in db.LoanSearch.AsNoTracking() on L.LoanID equals search.LoanID
                             join LTM in db.LoanTypeMaster.AsNoTracking() on L.LoanTypeID equals LTM.LoanTypeID
                             join CUST in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CUST.CustomerID
                             join loanlosFields in db.LoanLOSFields.AsNoTracking() on L.LoanID equals loanlosFields.LoanID into loanField
                             from Los in loanField.DefaultIfEmpty()
                             join field in db.IDCFields.AsNoTracking() on L.LoanID equals field.LoanID into idcfield
                             from IDC in idcfield.DefaultIfEmpty()
                             where ((search.Status != StatusConstant.IDC_COMPLETE && L.AssignedUserID == CurrentUserID)
                                 )
                             select new LoanSearchReport()
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = search.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 EphesoftBatchInstanceID = string.IsNullOrEmpty(IDC.IDCBatchInstanceID) ? string.Empty : IDC.IDCBatchInstanceID, //L.EphesoftBatchInstanceID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = ((search.Status == StatusConstant.PENDING_IDC) && (L.SubStatus == 0)) ? search.Status : (search.Status == StatusConstant.PENDING_IDC) ? L.SubStatus : search.Status,
                                 LoanAmount = search.LoanAmount,
                                 LoanTypeName = LTM.LoanTypeName,
                                 BorrowerName = search.BorrowerName,
                                 StatusDescription = "",
                                 LoggedUserID = L.LoggedUserID,
                                 ServiceTypeName = db.ReviewTypeMaster.Where(r => r.ReviewTypeID == L.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                                 AuditMonthYearDate = L.AuditMonthYear,
                                 AuditMonthYear = "",
                                 Customer = CUST.CustomerName,
                                 AuditDueDate = search.AuditDueDate,
                                 AssignedUserID = L.AssignedUserID,
                                 UploadType = L.UploadType
                             }
                          ).OrderByDescending(c => c.ReceivedDate).Take(100).ToList();
                }
                else if (FromDate.Year == DateTime.MinValue.Year && ToDate.Year == DateTime.MinValue.Year && LoanNumber == "" && BorrowerName == "" &&
                    LoanAmount == "" && LoanType == 0 && ReviewStatus == 0 && SelectedLoanStatus.Length == 0 && AuditMonthYear == null && ReviewType == 0 && Customer == 0 && PropertyAddress == "" && InvestorLoanNumber == "" && PostCloser == "" && LoanOfficer == "" && UnderWriter == "" && AuditDueDate.Year == DateTime.MinValue.Year)
                {
                    loans = (from L in db.Loan.AsNoTracking()
                             join search in db.LoanSearch.AsNoTracking() on L.LoanID equals search.LoanID
                             join LTM in db.LoanTypeMaster.AsNoTracking() on L.LoanTypeID equals LTM.LoanTypeID
                             join CUST in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CUST.CustomerID
                             join users in db.Users.AsNoTracking() on L.AssignedUserID equals users.UserID into user
                             from USER in user.DefaultIfEmpty()
                             join loanlosFields in db.LoanLOSFields.AsNoTracking() on L.LoanID equals loanlosFields.LoanID into loanField
                             from Los in loanField.DefaultIfEmpty()
                             join field in db.IDCFields.AsNoTracking() on L.LoanID equals field.LoanID into idcfield
                             from IDC in idcfield.DefaultIfEmpty()
                                 //where ((search.Status != StatusConstant.IDC_COMPLETE && search.Status != StatusConstant.PENDING_IDC)
                             where ((search.Status != StatusConstant.IDC_COMPLETE)
                                 )
                             select new LoanSearchReport()
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = search.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 EphesoftBatchInstanceID = string.IsNullOrEmpty(IDC.IDCBatchInstanceID) ? string.Empty : IDC.IDCBatchInstanceID, //L.EphesoftBatchInstanceID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = ((search.Status == StatusConstant.PENDING_IDC) && (L.SubStatus == 0)) ? search.Status : (search.Status == StatusConstant.PENDING_IDC) ? L.SubStatus : search.Status,
                                 LoanAmount = search.LoanAmount,
                                 LoanTypeName = LTM.LoanTypeName,
                                 BorrowerName = search.BorrowerName,
                                 StatusDescription = "",
                                 LoggedUserID = L.LoggedUserID,
                                 ServiceTypeName = db.ReviewTypeMaster.Where(r => r.ReviewTypeID == L.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                                 AuditMonthYearDate = L.AuditMonthYear,
                                 AuditMonthYear = "",
                                 Customer = CUST.CustomerName,
                                 AssignedUserID = L.AssignedUserID > 0 ? L.AssignedUserID : 0,
                                 AssignedUser = USER.UserID > 0 ? USER.FirstName + " " + USER.LastName : "UnAsssigned",
                                 AuditDueDate = search.AuditDueDate,
                                 UploadType = L.UploadType
                             }
                          ).OrderByDescending(c => c.ReceivedDate).Take(100).ToList();
                }
                else
                {
                    loans = (from L in db.Loan.AsNoTracking()
                             join search in db.LoanSearch.AsNoTracking() on L.LoanID equals search.LoanID
                             join LTM in db.LoanTypeMaster.AsNoTracking() on L.LoanTypeID equals LTM.LoanTypeID
                             join users in db.Users.AsNoTracking() on L.AssignedUserID equals users.UserID into user
                             from USER in user.DefaultIfEmpty()
                             join loanlosFields in db.LoanLOSFields.AsNoTracking() on L.LoanID equals loanlosFields.LoanID into loanField
                             from Los in loanField.DefaultIfEmpty()
                             join CUST in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CUST.CustomerID
                             join field in db.IDCFields.AsNoTracking() on L.LoanID equals field.LoanID into idcfield
                             from IDC in idcfield.DefaultIfEmpty()
                             where ((FromDate == DateTime.MinValue || (L.CreatedOn >= FromDate && L.CreatedOn < ToDate))
                                    //&& (ReviewStatus == 0 || L.Status == ReviewStatus)
                                    && (LoanType == 0 || L.LoanTypeID == LoanType)
                                    && (ReviewType == 0 || L.ReviewTypeID == ReviewType)
                                    && (Customer == 0 || L.CustomerID == Customer)
                                    && (AuditMonthYear == null || L.AuditMonthYear == AuditMonthYear)
                                    && (string.IsNullOrEmpty(LoanNumber) || search.LoanNumber.ToUpper() == LoanNumber.ToUpper())
                                    && (string.IsNullOrEmpty(BorrowerName) || search.BorrowerName.ToUpper() == BorrowerName.ToUpper())
                                    && (loanAmount == 0 || search.LoanAmount == loanAmount)
                                    && (string.IsNullOrEmpty(PropertyAddress) || search.PropertyAddress.ToUpper() == PropertyAddress.ToUpper())
                                    && (string.IsNullOrEmpty(InvestorLoanNumber) || search.InvestorLoanNumber.ToUpper() == InvestorLoanNumber.ToUpper())
                                    && (string.IsNullOrEmpty(PostCloser) || Los.PostCloser.ToUpper() == PostCloser.ToUpper())
                                    && (string.IsNullOrEmpty(LoanOfficer) || Los.LoanOfficer.ToUpper() == LoanOfficer.ToUpper())
                                    && (string.IsNullOrEmpty(UnderWriter) || Los.Underwriter.ToUpper() == UnderWriter.ToUpper())
                                    && (AuditDueDate == DateTime.MinValue || (AuditDueDate == search.AuditDueDate))
                                    )
                             select new LoanSearchReport()
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = search.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 EphesoftBatchInstanceID = string.IsNullOrEmpty(IDC.IDCBatchInstanceID) ? string.Empty : IDC.IDCBatchInstanceID, //L.EphesoftBatchInstanceID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = ((search.Status == StatusConstant.PENDING_IDC) && (L.SubStatus == 0)) ? search.Status : (search.Status == StatusConstant.PENDING_IDC) ? L.SubStatus : search.Status,
                                 LoanAmount = search.LoanAmount,
                                 LoanTypeName = LTM.LoanTypeName,
                                 BorrowerName = search.BorrowerName,
                                 StatusDescription = "",
                                 LoggedUserID = L.LoggedUserID,
                                 ServiceTypeName = db.ReviewTypeMaster.Where(r => r.ReviewTypeID == L.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                                 AuditMonthYearDate = L.AuditMonthYear,
                                 AuditMonthYear = "",
                                 Customer = CUST.CustomerName,
                                 AssignedUserID = L.AssignedUserID > 0 ? L.AssignedUserID : 0,
                                 AssignedUser = USER.UserID > 0 ? USER.FirstName + USER.LastName : "UnAsssigned",
                                 AuditDueDate = search.AuditDueDate,
                                 UploadType = L.UploadType
                             }
                         ).ToList();

                    if (SelectedLoanStatus.Length > 0)
                    {
                        loans = loans.Where(ln => SelectedLoanStatus.Any(sls => ln.Status == Convert.ToInt64(sls))).ToList();
                    }


                }


                foreach (var item in loans.ToList())
                    item.AuditMonthYear = GetDateString(item.AuditMonthYearDate);



                loans = (from l in loans.AsEnumerable()
                         join wm in wfMaster on l.Status equals wm.StatusID
                         //where l.Status != StatusConstant.PENDING_IDC && l.Status != StatusConstant.IDC_COMPLETE
                         where l.Status != StatusConstant.IDC_COMPLETE
                         select new LoanSearchReport()
                         {
                             LoanID = l.LoanID,
                             LoanNumber = l.LoanNumber,
                             LoanTypeID = l.LoanTypeID,
                             EphesoftBatchInstanceID = l.EphesoftBatchInstanceID,
                             ReceivedDate = l.ReceivedDate,
                             Status = l.Status,
                             LoanAmount = l.LoanAmount,
                             LoanTypeName = l.LoanTypeName,
                             BorrowerName = l.BorrowerName,
                             StatusDescription = wm.StatusDescription,
                             LoggedUserID = l.LoggedUserID,
                             ServiceTypeName = l.ServiceTypeName,
                             AuditMonthYear = l.AuditMonthYear,
                             AuditMonthYearDate = l.AuditMonthYearDate,
                             Customer = l.Customer,
                             AssignedUserID = l.AssignedUserID,
                             AssignedUser = l.AssignedUser,
                             AuditDueDate = l.AuditDueDate,
                             UploadType = l.UploadType
                         }).ToList();

                loan = (from l in loans.AsEnumerable()
                        join u in db.Users on l.LoggedUserID equals u.UserID into lu
                        from ul in lu.DefaultIfEmpty()
                        select new
                        {
                            LoanID = l.LoanID,
                            LoanNumber = l.LoanNumber,
                            LoanTypeID = l.LoanTypeID,
                            EphesoftBatchInstanceID = l.EphesoftBatchInstanceID,
                            ReceivedDate = l.ReceivedDate,
                            Status = l.Status,
                            LoanAmount = l.LoanAmount,
                            LoanTypeName = l.LoanTypeName,
                            BorrowerName = l.BorrowerName,
                            StatusDescription = l.StatusDescription,
                            LoggedUserID = l.LoggedUserID,
                            LoggerUserFirstName = ul?.FirstName ?? String.Empty,
                            LoggerUserLastName = ul?.LastName ?? String.Empty,
                            CurrentUserID = CurrentUserID,
                            ServiceTypeName = l.ServiceTypeName,
                            AuditMonthYear = l.AuditMonthYear,
                            Customer = l.Customer,
                            AssignedUserID = l.AssignedUserID,
                            AssignedUser = l.AssignedUser,
                            AuditDueDate = l.AuditDueDate,
                            UploadType = l.UploadType
                        }).ToList().OrderBy(c => c.Customer).ToList();


            }

            return loan;
        }
        public object GetLoanInfo(string _loanGUID)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                Guid loanGUID = new Guid(_loanGUID);

                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanGUID == loanGUID).FirstOrDefault();
                if (_loan != null)
                {
                    LoanSearch _search = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == _loan.LoanID).FirstOrDefault();
                    return new
                    {
                        LoanID = _loan.LoanID,
                        LoanAmount = _search.LoanAmount,
                        BorrowerName = _search.BorrowerName,
                        ServiceTypeName = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == _loan.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                        AuditMonthYear = _loan.AuditMonthYear.ToString("MMMM yyyy"),
                        ReceivedDate = _search.ReceivedDate,
                        LoanNumber = _loan.LoanNumber,
                        LoanTypeName = db.LoanTypeMaster.AsNoTracking().Where(x => x.LoanTypeID == _loan.LoanTypeID).FirstOrDefault().LoanTypeName,
                        StatusDescription = StatusConstant.GetStatusDescription(_loan.Status),
                        AuditDueDate = _search.AuditDueDate,
                        Customer = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == _loan.CustomerID).FirstOrDefault().CustomerName,
                        EphesoftBatchInstanceID = db.IDCFields.AsNoTracking().Where(x => x.LoanID == _loan.LoanID).FirstOrDefault().IDCBatchInstanceID,
                        AssignedUser = "",
                        AssignedUserID = _loan.AssignedUserID,
                        StatusID = _loan.Status,
                        CurrentUserID = _loan.LoggedUserID,
                        LoanTypeID = _loan.LoanTypeID,
                        LoggedUserID = _loan.LoggedUserID,
                        LoggerUserFirstName = "",
                        LoggerUserLastName = "",
                        Message = ""

                    };
                }
                else return null;
            }
        }
        public object GetPurgeStagingDetails(long batchID)
        {
            object getPurgeStaging;
            using (var db = new DBConnect(TenantSchema))
            {
                getPurgeStaging = (from psd in db.PurgeStagingDetails
                                   join lo in db.Loan on psd.LoanID equals lo.LoanID
                                   join lt in db.LoanTypeMaster on lo.LoanTypeID equals lt.LoanTypeID
                                   join cs in db.CustomerMaster on lo.CustomerID equals cs.CustomerID
                                   join ls in db.LoanSearch on lo.LoanID equals ls.LoanID
                                   where psd.BatchID == batchID
                                   select new
                                   {
                                       LoanNumber = lo.LoanNumber,
                                       LoanType = lt.LoanTypeName,
                                       CustomerName = cs.CustomerName,
                                       BorrowerName = ls.BorrowerName,
                                       Status = psd.Status,
                                       ErrorMSG = psd.ErrMsg
                                   }).ToList();
                return getPurgeStaging;
            }
        }

        public bool PurgeStagingDetails(long loanID, Int64 batchID, Int64 loanStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.PurgeStagingDetails.Add(new PurgeStagingDetails()
                    {
                        BatchID = batchID,
                        LoanID = loanID,
                        Status = loanStatus,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                    db.SaveChanges();
                    trans.Commit();
                    return true;
                }
            }
            return false;
        }

        public void RetryPurgeStaging(DBConnect db, long bID, Int64 _Status)
        {

            PurgeStaging pSt = db.PurgeStaging.AsNoTracking().Where(pstg => pstg.BatchID == bID).FirstOrDefault();
            pSt.Status = _Status == StatusConstant.PURGE_FAILED ? StatusConstant.PURGE_WAITING : StatusConstant.EXPORT_WAITING;
            pSt.ModifiedOn = DateTime.Now;
            db.Entry(pSt).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void RetryLoanPurge(DBConnect db, long loanID, Int64 _Status)
        {
            Loan loan = db.Loan.AsNoTracking().Where(lo => lo.LoanID == loanID).FirstOrDefault();
            loan.Status = _Status == StatusConstant.PURGE_FAILED ? StatusConstant.PURGE_WAITING : StatusConstant.EXPORT_WAITING;
            loan.ModifiedOn = DateTime.Now;
            db.Entry(loan).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void RetryLoanSearchPurge(DBConnect db, long loanID, Int64 _Status)
        {
            LoanSearch loan = db.LoanSearch.AsNoTracking().Where(lo => lo.LoanID == loanID).FirstOrDefault();
            loan.Status = _Status == StatusConstant.PURGE_FAILED ? StatusConstant.PURGE_WAITING : StatusConstant.EXPORT_WAITING;
            loan.ModifiedOn = DateTime.Now;
            db.Entry(loan).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool RetryPurge(long[] batchIDs)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    foreach (Int64 batchID in batchIDs)
                    {
                        List<PurgeStagingDetails> lsPurgeStDt = db.PurgeStagingDetails.AsNoTracking().Where(psd => psd.BatchID == batchID && (psd.Status == StatusConstant.PURGE_FAILED || psd.Status == StatusConstant.EXPORT_FAILED)).ToList();

                        foreach (PurgeStagingDetails purgeDetail in lsPurgeStDt)
                        {
                            RetryPurgeStagingDetails(db, batchID, purgeDetail.LoanID, purgeDetail.Status);
                            RetryLoanSearchPurge(db, purgeDetail.LoanID, purgeDetail.Status);
                            RetryLoanPurge(db, purgeDetail.LoanID, purgeDetail.Status);
                            RetryPurgeStaging(db, batchID, purgeDetail.Status);
                        }

                    }
                    trans.Commit();
                    return true;
                }
            }
            return false;
        }

        public void RetryPurgeStagingDetails(DBConnect db, long batchId, long loanID, Int64 _Status)
        {
            PurgeStagingDetails purgeStDt = db.PurgeStagingDetails.AsNoTracking().Where(psd => psd.LoanID == loanID).FirstOrDefault();
            purgeStDt.Status = _Status == StatusConstant.PURGE_FAILED ? StatusConstant.PURGE_WAITING : StatusConstant.EXPORT_WAITING;
            purgeStDt.ModifiedOn = DateTime.Now;
            db.Entry(purgeStDt).State = EntityState.Modified;
            db.SaveChanges();
        }

        public object GetPurgeMonitor(DateTime fromDate, DateTime toDate, long workFlowStatus)
        {
            List<PurgeStaging> purgeMonitorDatas = null;
            using (var db = new DBConnect(TenantSchema))
            {
                toDate = toDate.AddDays(1);
                if (workFlowStatus != 0)
                {
                    purgeMonitorDatas = db.PurgeStaging.AsNoTracking().Where(ps => ps.CreatedOn >= fromDate && ps.CreatedOn <= toDate && ps.Status == workFlowStatus).ToList();
                }
                else
                {
                    purgeMonitorDatas = db.PurgeStaging.AsNoTracking().Where(ps => ps.CreatedOn >= fromDate && ps.CreatedOn <= toDate).ToList();
                }

            }
            return purgeMonitorDatas;
        }

        public object GetMissingDocStatus(Int64 LoanID)
        {
            List<AuditLoanMissingDoc> _auditMissingDocs = new List<AuditLoanMissingDoc>();
            using (var db = new DBConnect(TenantSchema))
            {
                _auditMissingDocs = db.AuditLoanMissingDoc.AsNoTracking().Where(a => a.LoanID == LoanID).ToList();
            }
            return _auditMissingDocs;
        }

        public object GetMissingDocVersion(Int64 LoanID, Int64 DocID)
        {
            string loanDocVersionNumber = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                loanDocVersionNumber = db.LoanImage.AsNoTracking().Where(a => a.LoanID == LoanID && a.DocumentTypeID == DocID).OrderByDescending(l => l.Version).Select(l => l.Version).FirstOrDefault();
            }
            return new { VersionNumber = loanDocVersionNumber };
        }


        public bool UpdatePurgeWaiting(long loanID, string userName, Int64 loanStatus)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    bool searchPurgeWaiting = false;
                    bool loanPurgeWaiting = false;
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                    LoanSearch loansearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == loanID).FirstOrDefault();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.PURGE_WAITING_BY_USER);
                    string[] auditExportDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.EXPORT_WAITING_BY_USER);
                    if (loan != null)
                    {
                        loan.Status = loanStatus;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        if (loanStatus == StatusConstant.PURGE_WAITING)
                        {
                            LoanAudit.InsertLoanAudit(db, loan, auditDescs[0].Replace(AuditConfigConstant.USERNAME, userName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, userName));
                        }
                        else if (loanStatus == StatusConstant.EXPORT_WAITING)
                        {
                            LoanAudit.InsertLoanAudit(db, loan, auditExportDescs[0].Replace(AuditConfigConstant.USERNAME, userName), auditExportDescs[1].Replace(AuditConfigConstant.USERNAME, userName));
                        }

                        loanPurgeWaiting = true;
                    }
                    if (loansearch != null)
                    {
                        loansearch.Status = loanStatus;
                        db.Entry(loansearch).State = EntityState.Modified;
                        db.SaveChanges();
                        if (loanStatus == StatusConstant.PURGE_WAITING)
                        {
                            LoanAudit.InsertLoanAudit(db, loan, auditDescs[0].Replace(AuditConfigConstant.USERNAME, userName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, userName));
                        }
                        else if (loanStatus == StatusConstant.EXPORT_WAITING)
                        {
                            LoanAudit.InsertLoanAudit(db, loan, auditExportDescs[0].Replace(AuditConfigConstant.USERNAME, userName), auditExportDescs[1].Replace(AuditConfigConstant.USERNAME, userName));
                        }
                        searchPurgeWaiting = true;
                    }
                    trans.Commit();
                    if (searchPurgeWaiting && loanPurgeWaiting)
                        return true;
                    else
                        return false;

                }
            }
            return false;
        }

        public Int64 PurgeStaging(PurgeStaging reqloanpurge)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    db.PurgeStaging.Add(reqloanpurge);
                    db.SaveChanges();
                    trans.Commit();
                    return reqloanpurge.BatchID;
                }
            }
        }

        public bool DeletedReverification(Int64 LoanReverificationID, Int64 LoanID, string UserReverificationName, string UserName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                LoanReverification _reverify = db.LoanReverification.AsNoTracking().Where(l => l.ReverificationName == UserReverificationName).FirstOrDefault();
                if (_reverify != null)
                {
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.REVERIFICATION_DELETED_BY);
                    LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.REVERIFICATIONNAME, UserReverificationName).Replace(AuditConfigConstant.USERNAME, UserName), auditDescs[1].Replace(AuditConfigConstant.REVERIFICATIONNAME, UserReverificationName).Replace(AuditConfigConstant.USERNAME, UserName));
                    db.Entry(_reverify).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            return true;
        }
        public bool DeleteLoan(long[] loanID, string userName)
        {
            bool isLoanStatus = false;
            bool isLoanSearchStatus = false;
            Loan _loan = null;
            LoanSearch _loanSearch = null;
            string ephesoftOutputPath = string.Empty;

            if (ConfigurationManager.AppSettings["ephesoftOutputPath"] != null && ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString() != String.Empty)
            {
                ephesoftOutputPath = ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString();
            }

            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    foreach (var item in loanID)
                    {
                        _loan = db.Loan.AsNoTracking().Where(ln => ln.LoanID == item).FirstOrDefault();
                        _loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == item).FirstOrDefault();
                        if (_loan != null)
                        {
                            if (_loan.Status == StatusConstant.FAILED_BOX_DOWNLOAD || _loan.Status == StatusConstant.PENDING_BOX_DOWNLOAD)
                            {
                                List<BoxDownloadQueue> _boxDQ = db.BoxDownloadQueue.AsNoTracking().Where(b => b.LoanID == item).ToList();
                                if (_boxDQ != null)
                                {
                                    foreach (BoxDownloadQueue _boxDwQu in _boxDQ)
                                    {
                                        _boxDwQu.Status = (int)StatusConstant.LOAN_DELETED;
                                        _boxDwQu.ModifiedOn = DateTime.Now;
                                        db.Entry(_boxDwQu).State = EntityState.Modified;
                                    }

                                }
                            }
                            _loan.Status = StatusConstant.LOAN_DELETED;
                            _loan.ModifiedOn = DateTime.Now;
                            db.Entry(_loan).State = EntityState.Modified;
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                            LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], $"Loan Deleted by" + userName);
                            isLoanStatus = true;

                            string batchID = _loan.EphesoftBatchInstanceID;

                            if (!String.IsNullOrEmpty(batchID))
                            {
                                String[] dirPath = Directory.GetDirectories(ephesoftOutputPath, batchID, SearchOption.AllDirectories);
                                foreach (var _path in dirPath)
                                {
                                    if (Directory.Exists(_path))
                                    {
                                        Directory.Delete(_path, true);
                                    }
                                }
                            }
                        }
                        if (_loanSearch != null)
                        {
                            _loanSearch.Status = StatusConstant.LOAN_DELETED;
                            _loanSearch.ModifiedOn = DateTime.Now;
                            db.Entry(_loanSearch).State = EntityState.Modified;
                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                            LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditDescs[0], $"Loan Deleted by" + userName);
                            isLoanSearchStatus = true;
                        }

                    }

                    db.SaveChanges();
                    trans.Commit();
                    if (isLoanSearchStatus || isLoanStatus)
                        return true;
                    else
                        return false;

                }
            }

            return false;
        }

        public long AssignUser(long loanId, long assignedUserId, long currentuserId, string roleName, string assignedBy, string assignedTo)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loans = db.Loan.AsNoTracking().Where(lo => lo.LoanID == loanId).FirstOrDefault();
                if (_loans != null)
                {
                    _loans.AssignedUserID = assignedUserId;
                    _loans.ModifiedOn = DateTime.Now;
                    db.Entry(_loans).State = EntityState.Modified;
                    db.SaveChanges();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.ASSIGN_LOAN_TO_USER);
                    LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0].Replace(AuditConfigConstant.ASSIGNEDBY, assignedBy).Replace(AuditConfigConstant.Role, roleName).Replace(AuditConfigConstant.ASSIGNEDTO, assignedTo), auditDescs[1].Replace(AuditConfigConstant.ASSIGNEDBY, assignedBy).Replace(AuditConfigConstant.Role, roleName).Replace(AuditConfigConstant.ASSIGNEDTO, assignedTo));
                    return assignedUserId;
                }
            }
            return 0;
        }

        public ImportStagings GetImportStaging(long loanID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.ImportStaging.AsNoTracking().Where(lo => lo.LoanId == loanID).FirstOrDefault();
            }
        }

        public void UpdateImportStaging(ImportStagings _importStaging)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                _importStaging.Status = ImportStagingConstant.Staged;
                _importStaging.ErrorMessage = string.Empty;
                _importStaging.ModifiedOn = DateTime.Now;
                _importStaging.LoanPicked = false;
                db.Entry(_importStaging).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        public bool UpdateLoanMonitor(long loanID, long loanTypeID, string userName, string ephesoftPath)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    Loan _loans = db.Loan.AsNoTracking().Where(lo => lo.LoanID == loanID).FirstOrDefault();
                    IDCFields _idcfield = db.IDCFields.Where(x => x.LoanID == loanID).FirstOrDefault();
                    LoanSearch _loansearch = db.LoanSearch.AsNoTracking().Where(lo => lo.LoanID == loanID).FirstOrDefault();
                    ImportStagings _importStaging = db.ImportStaging.AsNoTracking().Where(lo => lo.LoanId == loanID).FirstOrDefault();
                    if (_loans != null)
                    {
                        _loans.LoanTypeID = loanTypeID;
                        _loans.Status = StatusConstant.IDC_COMPLETE;
                        _loans.SubStatus = 0;
                        _loans.ModifiedOn = DateTime.Now;
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_TYPE_ASSIGNED_BY);
                        LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0].Replace(AuditConfigConstant.USERNAME, userName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, userName)); // string.Format("Loan Type Assigned by : {0}", userName));
                        db.Entry(_loans).State = EntityState.Modified;
                        if (_loansearch != null)
                        {
                            _loansearch.LoanTypeID = loanTypeID;
                            _loansearch.Status = StatusConstant.IDC_COMPLETE;
                            _loansearch.ModifiedOn = DateTime.Now;
                            db.Entry(_loansearch).State = EntityState.Modified;
                            LoanAudit.InsertLoanSearchAudit(db, _loansearch, auditDescs[0].Replace(AuditConfigConstant.USERNAME, userName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, userName));
                        }
                        db.SaveChanges();
                        string batchID = _idcfield.IDCBatchInstanceID;// _loans.EphesoftBatchInstanceID;
                        string orgPath = Path.Combine(ephesoftPath, "Output", batchID);
                        string[] dir = Directory.GetFiles(orgPath, "*.error", SearchOption.TopDirectoryOnly);
                        if (dir.Length == 1 && $"{batchID}_batch.error" == Path.GetFileName(dir[0]))
                        {
                            string extensions = Path.GetExtension(dir[0]);

                            if (extensions == ".error")
                            {
                                string renamedFile = Path.ChangeExtension(dir[0], ".lck");

                                File.Move(dir[0], renamedFile);
                            }
                        }

                        if (_importStaging != null)
                        {
                            _importStaging.Status = ImportStagingConstant.Staged;
                            _importStaging.ErrorMessage = string.Empty;
                            _importStaging.ModifiedOn = DateTime.Now;
                            _importStaging.LoanPicked = false;
                            db.Entry(_importStaging).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        trans.Commit();
                        return true;
                    }
                }
            }
            return false;
        }

        public void InsertLOSLoanExport(Int64 LoanID, string loanNumber)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LOSExportFileStaging _lStage = new LOSExportFileStaging()
                {
                    LoanID = LoanID,
                    FileType = LOSExportFileTypeConstant.LOS_LOAN_EXPORT,
                    Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                    FileName = $"{loanNumber}_IL_Export_{DateTime.Now.ToString("yyyyMMddhhmmssfff")}.json",
                    FileJson = string.Empty,
                    ErrorMsg = string.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };

                db.LOSExportFileStaging.Add(_lStage);
                db.SaveChanges();
            }
        }

        public bool DeleteOutputFolder(long[] loanID, string ephesoftPath)
        {
            bool result = false;
            using (var db = new DBConnect(TenantSchema))
            {
                foreach (var Loan_ID in loanID)
                {
                    //Loan _loans = db.Loan.AsNoTracking().Where(lo => lo.LoanID == Loan_ID).FirstOrDefault();
                    //if (_loans != null)
                    //{
                    List<string> batchIDs = db.AuditIDCFields.AsNoTracking().Where(x => x.LoanID == Loan_ID).Select(x => x.IDCBatchInstanceID).Distinct().ToList();

                    foreach (var batchID in batchIDs)
                    {
                        if (!String.IsNullOrEmpty(batchID))
                        {
                            String[] dirPath = Directory.GetDirectories(ephesoftPath, batchID, SearchOption.AllDirectories);
                            foreach (var item in dirPath)
                            {
                                if (Directory.Exists(item))
                                {
                                    Directory.Delete(item, true);
                                    result = true;
                                }
                            }
                        }
                    }

                    //}
                }
            }
            return result;
        }


        public bool UpdateLoanDetails(long loanID, string loanVlaues, string userName, Int64 Type)
        {
            bool isUpdated = false;

            using (var db = new DBConnect(TenantSchema))
            {
                if (Type == 0)
                {
                    isUpdated = UpdateLoanNumbers(db, loanID, loanVlaues, userName);
                }
                else if (Type == 1)
                {
                    isUpdated = UpdateBorrowerName(db, loanID, loanVlaues, userName);
                }
            }
            return isUpdated;
        }
        public bool UpdateLoanHeader(long loanID, LoanHeader loanVlaues, string userName)
        {
            bool isUpdated = false;

            using (var db = new DBConnect(TenantSchema))
            {

                isUpdated = UpdateloanHeader(db, loanID, loanVlaues, userName);

            }
            return isUpdated;
        }
        private bool UpdateloanHeader(DBConnect db, long loanID, LoanHeader loanVlaues, string userName)
        {
            bool isLoan = false, isloanSearch = false;
            using (var trans = db.Database.BeginTransaction())
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();

                LoanLOSFields _los = db.LoanLOSFields.AsNoTracking().Where(los => los.LoanID == loanID).FirstOrDefault();
                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == loanID).FirstOrDefault();
                if (_loan != null)
                {
                    _loan.LoanNumber = loanVlaues.LoanNumber;
                    if (loanVlaues.AuditMonthYear != null && loanVlaues.AuditMonthYear != DateTime.MinValue)
                    {
                        _loan.AuditMonthYear = loanVlaues.AuditMonthYear;
                    }

                    _loan.ModifiedOn = DateTime.Now;
                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();
                    isLoan = true;
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                    LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], $"Loan Number,Audit Month Year Updated {userName}");
                }
                if (_los != null)
                {
                    _los.PostCloser = loanVlaues.PostCloser;
                    _los.LoanOfficer = loanVlaues.LoanOfficer;
                    _los.Underwriter = loanVlaues.UnderWriter;

                    db.Entry(_los).State = EntityState.Modified;
                    db.SaveChanges();

                }
                else
                {
                    db.LoanLOSFields.Add(new LoanLOSFields
                    {
                        LoanID = loanID,
                        PostCloser = loanVlaues.PostCloser,
                        LoanOfficer = loanVlaues.LoanOfficer,
                        Underwriter = loanVlaues.UnderWriter,

                    });
                    db.SaveChanges();
                }

                if (_loanSearch != null)
                {
                    _loanSearch.LoanAmount = loanVlaues.LoanAmount;
                    _loanSearch.InvestorLoanNumber = loanVlaues.InvestorLoanNumber;
                    _loanSearch.PropertyAddress = loanVlaues.PropertyAddress;
                    _loanSearch.LoanNumber = loanVlaues.LoanNumber;
                    _loanSearch.BorrowerName = loanVlaues.BorrowerName;
                    _loanSearch.AuditDueDate = (loanVlaues.AuditDueDate == DateTime.MinValue) ? (DateTime?)null : loanVlaues.AuditDueDate;
                    _loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loanSearch).State = EntityState.Modified;
                    db.SaveChanges();
                    isloanSearch = true;
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                    LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditDescs[0], $"Borrower Name,Loan Number,Investor LoanNumber,Property Address, Updated {userName}");
                }
                trans.Commit();
                return (isLoan && isloanSearch);
            }
        }
        private bool UpdateBorrowerName(DBConnect db, long loanID, string loanVlaues, string userName)
        {
            bool isloanSearch = false;
            using (var trans = db.Database.BeginTransaction())
            {
                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == loanID).FirstOrDefault();
                if (_loanSearch != null)
                {
                    _loanSearch.BorrowerName = loanVlaues;
                    _loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loanSearch).State = EntityState.Modified;
                    db.SaveChanges();
                    isloanSearch = true;
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                    LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditDescs[0], $"Borrower Name Updated {userName}");
                }
                trans.Commit();
                return isloanSearch;
            }
        }

        private bool UpdateLoanNumbers(DBConnect db, long loanID, string loanVlaues, string userName)
        {
            bool isLoan = false, isloanSearch = false;
            using (var trans = db.Database.BeginTransaction())
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == loanID).FirstOrDefault();
                if (_loan != null)
                {
                    _loan.LoanNumber = loanVlaues;
                    _loan.ModifiedOn = DateTime.Now;
                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();
                    isLoan = true;
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                    LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], $"Loan Number Updated {userName}");
                }
                if (_loanSearch != null)
                {
                    _loanSearch.LoanNumber = loanVlaues;
                    _loanSearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loanSearch).State = EntityState.Modified;
                    db.SaveChanges();
                    isloanSearch = true;
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);
                    LoanAudit.InsertLoanSearchAudit(db, _loanSearch, auditDescs[0], $"Loan Number Updated {userName}");
                }
                trans.Commit();

                return (isLoan && isloanSearch);
            }
        }
        public List<object> GetLoanAudit(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<AuditLoan> lsLoanAudit = db.AuditLoan.AsNoTracking().Where(l => l.LoanID == LoanID).ToList();
                List<AuditLoanDetail> lsLoanDetailAudit = db.AuditLoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).ToList();

                List<object> _audit = (from l in lsLoanAudit
                                       select new
                                       {
                                           AuditDescription = l.AuditDescription,
                                           AuditDateTime = l.AuditDateTime
                                       }).Union(
                                                from al in lsLoanDetailAudit
                                                select new
                                                {
                                                    AuditDescription = al.AuditDescription,
                                                    AuditDateTime = al.AuditDateTime
                                                }
                                                ).ToList<object>();

                return _audit;
            }
        }
        public object GetRetentionLoans(DateTime AuditMonthYear)
        {
            object loanDatas;
            using (var db = new DBConnect(TenantSchema))
            {
                loanDatas = (from lo in db.Loan
                             join cus in db.CustomerMaster on lo.CustomerID equals cus.CustomerID
                             join lty in db.LoanTypeMaster on lo.LoanTypeID equals lty.LoanTypeID
                             join ls in db.LoanSearch on lo.LoanID equals ls.LoanID
                             where lo.AuditMonthYear == AuditMonthYear && lo.Status == StatusConstant.LOAN_EXPIRED
                             select new
                             {
                                 LoanID = lo.LoanID,
                                 LoanNumber = lo.LoanNumber,
                                 LoanType = lty.LoanTypeName,
                                 BorrowerName = ls.BorrowerName,
                                 CustomerName = cus.CustomerName,
                                 Status = lo.Status
                             }).ToList();
            }
            return loanDatas;
        }

        public LoanAuditReportPDF GetLoanAuditReportDeatils(Int64 LoanID)
        {
            LoanAuditReportPDF _obj = new LoanAuditReportPDF();
            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                List<ChecklistPDFTable> _checklists = (from l in db.LoanChecklistAudit.AsNoTracking()
                                                       join c in db.CheckListDetailMaster.AsNoTracking() on l.ChecklistDetailID equals c.CheckListDetailID
                                                       join cm in db.CheckListMaster.AsNoTracking() on c.CheckListID equals cm.CheckListID
                                                       where l.LoanID == LoanID
                                                       select new ChecklistPDFTable()
                                                       {
                                                           ChecklistCategory = c.Category,
                                                           ChecklistDetailName = l.ChecklistName,
                                                           ChecklistName = cm.CheckListName,
                                                           Result = l.Result,
                                                           Order = l.Result ? 0 : 1
                                                       }).ToList();


                List<ChecklistPDFTable> _criticalChecklists = _checklists.Where(x => x.ChecklistCategory.Contains("Critical")).ToList().OrderByDescending(o => o.Order).ThenBy(o => o.ChecklistDetailName).ToList();
                List<ChecklistPDFTable> _otherChecklists = _checklists.Where(x => !x.ChecklistCategory.Contains("Critical")).ToList().OrderByDescending(o => o.Order).ThenBy(o => o.ChecklistCategory).ToList();
                _obj.completeNotes = string.IsNullOrEmpty(_loan.CompleteNotes) ? string.Empty : _loan.CompleteNotes;

                _obj.ChecklistDetails = _criticalChecklists.Union(_otherChecklists).ToList();

                _obj.FailedRuleCount = _obj.ChecklistDetails.Where(x => !x.Result).ToList().Count;
                _obj.TotalRuleCount = _obj.ChecklistDetails.Count;

                _obj.TotalCriticalDefectCount = _criticalChecklists.Count;
                _obj.FailedCriticalDefectCount = _criticalChecklists.Where(x => !x.Result).ToList().Count;

                _obj.LenderName = GetLenderName();

                if (_loanDetail != null)
                    _obj.MissingDocumentCount = _loanDetail.MissingDocCount;

                if (_obj.TotalRuleCount > 0)
                {
                    decimal _qc = (Convert.ToDecimal(_obj.TotalRuleCount - _obj.FailedRuleCount) / Convert.ToDecimal(_obj.TotalRuleCount));
                    double _qcIndex = Convert.ToDouble((_qc) * 100);
                    _obj.LoanQCIndex = Convert.ToInt64(Math.Round(_qcIndex));
                    _obj.ChecklistName = _obj.ChecklistDetails[0].ChecklistName;
                }

                if (_loanSearch != null)
                {
                    CustomerMaster _cust = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == _loanSearch.CustomerID).FirstOrDefault();
                    LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(c => c.LoanTypeID == _loanSearch.LoanTypeID).FirstOrDefault();
                    List<CustLoanDocMapping> _docMapping = db.CustLoanDocMapping.AsNoTracking().Where(x => x.LoanTypeID == _loanSearch.LoanTypeID && x.CustomerID == _loanSearch.CustomerID).ToList();

                    _obj.PropertyAddress = string.IsNullOrEmpty(_loanSearch.PropertyAddress) ? string.Empty : _loanSearch.PropertyAddress;
                    _obj.LoanNumber = string.IsNullOrEmpty(_loanSearch.LoanNumber) ? string.Empty : _loanSearch.LoanNumber;
                    _obj.LoanAmount = _loanSearch.LoanAmount != null ? _loanSearch.LoanAmount.ToString("C", CultureInfo.CurrentCulture) : string.Empty;
                    _obj.TotalDocumentCount = _docMapping.Count;

                    if (_cust != null)
                        _obj.InvestorName = _cust.CustomerName;

                    if (_loanType != null)
                        _obj.LoanType = _loanType.LoanTypeName;
                }
            }

            return _obj;
        }

        public string GetLenderName()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                AppConfig _config = db.AppConfig.AsNoTracking().Where(x => x.ConfigKey == "LENDER_NAME").FirstOrDefault();

                if (_config != null)
                    return _config.ConfigValue;
                else
                    return string.Empty;
            }
        }

        public object GetRetentionLoans(DateTime fromDate, DateTime toDate)
        {
            toDate = toDate.AddDays(1);
            object loanDatas;
            using (var db = new DBConnect(TenantSchema))
            {

                loanDatas = (from lo in db.Loan
                             join cus in db.CustomerMaster on lo.CustomerID equals cus.CustomerID
                             join lty in db.LoanTypeMaster on lo.LoanTypeID equals lty.LoanTypeID
                             join ls in db.LoanSearch on lo.LoanID equals ls.LoanID
                             where lo.CreatedOn >= fromDate && lo.CreatedOn <= toDate
                             && lo.Status == StatusConstant.LOAN_EXPIRED
                             select new
                             {
                                 LoanID = lo.LoanID,
                                 LoanNumber = lo.LoanNumber,
                                 LoanType = lty.LoanTypeName,
                                 BorrowerName = ls.BorrowerName,
                                 CustomerName = cus.CustomerName,
                                 Status = lo.Status
                             }
                                ).ToList();
            }
            return loanDatas;
        }
        public List<FannieMaeFields> GetFannieMaeFields(Int64 LoanID)
        {

            using (var db = new DBConnect(TenantSchema))
            {

                LOSLoanDetails _losTableFields = db.LOSLoanDetails.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                List<FannieMaeFields> _FannieMaefields = new List<FannieMaeFields>();
                if (_losTableFields != null && !string.IsNullOrEmpty(_losTableFields.LOSDetailJSON))
                {
                    var _fields = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(_losTableFields.LOSDetailJSON);
                    var _fieldsList = _fields.ToList();


                    foreach (var _field in _fieldsList)
                    {
                        string FieldID = string.Empty;
                        string FieldValue = string.Empty;
                        string FieldName = string.Empty;

                        LOSDocumentFields _losFieldTable = db.LOSDocumentFields.AsNoTracking().Where(los => los.FieldID == _field.Key).FirstOrDefault();

                        if (_losTableFields != null)
                            FieldName = _losFieldTable.FieldName;

                        if (_field.Value.Type == FannieMaeFieldTypeConstant.SType)
                        {
                            FieldID = "#" + _field.Key + "#" + FieldName;
                            FieldValue = _field.Value.Value;

                            _FannieMaefields.Add(new FannieMaeFields() { FieldID = FieldID, FieldValue = FieldValue });

                        }
                        else if (_field.Value.Type == FannieMaeFieldTypeConstant.MType)
                        {
                            Int64 Version = 1;

                            foreach (var _fieldValue in _field.Value.Value)
                            {
                                FieldID = "#" + _field.Key + "#" + FieldName + "-V" + Version;
                                FieldValue = _fieldValue;
                                _FannieMaefields.Add(new FannieMaeFields() { FieldID = FieldID, FieldValue = FieldValue });
                                Version++;
                            }
                        }

                    }
                }


                return _FannieMaefields;

            }
        }



        public bool UpdateLoanDocument(Int64 LoanID, Int64 DocumentID, Int64 CurrentUserID, List<DocumentLevelFields> DocumentFields, Int32 VersionNumber, List<DataTable> updatedDocTables)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                LoanDetail loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                List<DocumentTables> _docTables = db.DocumentTables.AsNoTracking().ToList();

                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (loanDetail != null)
                {
                    bool _docUpdated = false;
                    using (var tran = db.Database.BeginTransaction())
                    {
                        Batch loanBatch = JsonConvert.DeserializeObject<Batch>(loanDetail.LoanObject);

                        foreach (Documents doc in loanBatch.Documents.ToArray())
                        {
                            DocumentTables docTable = _docTables.Where(d => d.DocumentTypeName == doc.Type).FirstOrDefault();

                            if (docTable != null)
                            {
                                try
                                {
                                    List<DocTableJOBJECT> _docJObject = JsonConvert.DeserializeObject<List<DocTableJOBJECT>>(docTable.Tables);

                                    foreach (DocTableJOBJECT docObject in _docJObject)
                                    {
                                        DataTable _dTable = updatedDocTables.Where(u => u.Name == docObject.TableName).FirstOrDefault();
                                        if (_dTable != null && _dTable.Rows.Count > 0)
                                        {
                                            foreach (ColumnDesignation col in docObject.Columns)
                                            {
                                                decimal amount = 0m;
                                                _dTable.Rows.ForEach(r =>
                                                {
                                                    decimal val = 0m;
                                                    RowColumn _rCol = r.RowColumns.Where(c => c.Name == col.ColumnName).FirstOrDefault();
                                                    if (_rCol != null)
                                                    {
                                                        decimal.TryParse(_rCol.Value, out val);
                                                        amount += val;
                                                    }
                                                });
                                                DocumentLevelFields _docField = DocumentFields.Where(f => f.Name == col.DestinationField).FirstOrDefault();
                                                if (_docField != null)
                                                    _docField.Value = amount.ToString("N2");
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }

                            }

                            if (doc.DocumentTypeID.Equals(DocumentID) && doc.VersionNumber.Equals(VersionNumber))
                            {
                                foreach (DocumentLevelFields x in DocumentFields)
                                {
                                    x.Value = x.Value.Replace("$", "");
                                }
                                doc.DocumentLevelFields = DocumentFields;

                                doc.DataTables = updatedDocTables;

                                loanDetail.LoanObject = JsonConvert.SerializeObject(loanBatch);

                                db.Entry(loanDetail).State = EntityState.Modified;
                                db.SaveChanges();

                                User user = db.Users.AsNoTracking().Where(u => u.UserID == CurrentUserID).FirstOrDefault();

                                string UserName = string.Empty;

                                if (user != null)
                                    UserName = string.Format("{0} {1}", user.LastName, user.FirstName);

                                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_DETAILS_UPDATED_BY);
                                LoanAudit.InsertLoanDetailsAudit(db, loanDetail, CurrentUserID, auditDescs[0].Replace(AuditConfigConstant.USERNAME, UserName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, UserName)); // string.Format("Loan Details Updated by {0}", UserName));

                                _loanSearch.ModifiedOn = DateTime.Now;
                                db.Entry(_loanSearch).State = EntityState.Modified;
                                db.SaveChanges();

                                _docUpdated = true;

                                break;
                            }
                        }

                        tran.Commit();
                    }

                    if (_docUpdated)
                    {
                        EvaluateRules ruleEngine = new EvaluateRules(TenantSchema, LoanID);
                        List<Dictionary<string, string>> checklistDetails = ruleEngine.GetAllCheckListDetails;
                        string ruleFindings = JsonConvert.SerializeObject(checklistDetails);
                        List<RuleFinding> rulefinding = JsonConvert.DeserializeObject<List<RuleFinding>>(ruleFindings);
                        SaveLoanObject(rulefinding);
                        List<LoanChecklistAudit> _loancheckList = ruleEngine.FetchLoanCheckListDetails();
                        RuleEngineDataAccess _ruleDataAccess = new RuleEngineDataAccess(TenantSchema);
                        _ruleDataAccess.InsertLoanCheckListAuditDetails(_loancheckList);

                    }
                    return _docUpdated;
                }
                return false;
            }
        }


        public void SaveLoanObject(List<RuleFinding> rulefinding)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail _loan = db.LoanDetail.AsNoTracking().FirstOrDefault();

                if (_loan != null)
                {

                    Batch batch = JsonConvert.DeserializeObject<Batch>(_loan.LoanObject);
                    batch.RuleFinding = rulefinding;
                    _loan.LoanObject = JsonConvert.SerializeObject(batch);
                    db.Entry(_loan).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        public void SetLoanPickUpUser(Int64 LoanID, Int64 PickUpUserID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoggedUserID = PickUpUserID;
                        loan.LastAccessedUserID = PickUpUserID;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();

                        var user = db.Users.AsNoTracking().Where(U => U.UserID == PickUpUserID).FirstOrDefault();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_PICKED_BY_THE_USER);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0].Replace(AuditConfigConstant.LASTNAME, user.LastName).Replace(AuditConfigConstant.FIRSTNAME, user.FirstName), auditDescs[1].Replace(AuditConfigConstant.LASTNAME, user.LastName).Replace(AuditConfigConstant.FIRSTNAME, user.FirstName));
                    }

                    tran.Commit();
                }
            }
        }

        public bool RemoveLoanLoggedUser(Int64 LoanID)
        {
            bool result = false;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoggedUserID = 0;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();

                        result = true;
                    }

                    tran.Commit();
                }
            }

            return result;
        }

        public object CheckCurrentLoanUser(Int64 LoanID, Int64 CurrentUserID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                if ((loan.LoggedUserID == CurrentUserID) || (loan.LoggedUserID == 0))
                {
                    return new { CurrentUser = true };
                }
                else
                {
                    var loggerUser = db.Users.Where(u => u.UserID == loan.LoggedUserID).AsNoTracking().FirstOrDefault();

                    return new { CurrentUser = false, LoggerUserName = string.Format("{0} {1}", loggerUser.LastName, loggerUser.FirstName) };
                }
            }
            return null;
        }

        public bool CheckLoanPDFExists(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                //LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                //if (loanPdf != null)
                //    return !string.IsNullOrEmpty(loanPdf.LoanPDFPath);
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);

                return _imageWrapper.CheckFileExists(LoanID);
            }
            return false;
        }
        //public string GetLoanPDF(Int64 LoanID)
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        LoanPDF loanPdf = db.LoanPDF.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

        //        if (loanPdf != null)
        //            return string.IsNullOrEmpty(loanPdf.LoanPDFPath) ? string.Empty : loanPdf.LoanPDFPath;

        //    }
        //    return string.Empty;
        //}
        public byte[] GetLoanPDF(Int64 LoanID)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
            return _imageWrapper.GetLoanPDF(LoanID);
        }

        public Stream GetLoanPDFStream(Int64 LoanID)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
            return _imageWrapper.GetLoanPDFStream(LoanID);
        }
        public void DeleteLoanPDF(Int64 LoanID)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
            _imageWrapper.DeleteLoanStackingOrder(LoanID);
        }

        public byte[] GetDocumentPDF(Int64 LoanID, Int64 DocumentID, string VersionNumber)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanImage> _lsImages = db.LoanImage.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocumentTypeID == DocumentID && l.Version == VersionNumber).OrderBy(o => o.PageNo).ToList();

                List<byte[]> _reverifyImg = new List<byte[]>();
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                string _footerText = GetPDFFooterName();
                foreach (LoanImage doc in _lsImages)
                {
                    byte[] _imageBytes = _imageWrapper.GetLoanImage(doc.LoanID, doc.ImageGUID.GetValueOrDefault());
                    //if (!string.IsNullOrEmpty(_footerText))
                    //{
                    //    _imageBytes = CommonUtils.CreateHeaderFooterPDF(_imageBytes, _footerText);
                    //}
                    _reverifyImg.Add(_imageBytes);
                }

                return CommonUtils.CreatePdfBytes(_reverifyImg);
            }

            return new byte[0];
        }

        public byte[] GetReverificationLoanPDF(Int64 LoanID, Int64 MappingID, string TemplateJson, Int64 UserID, string RequiredDocIDs, bool IsCoverLetterReq, string ReverificationName)
        {
            byte[] pdfBytes = null;
            byte[] zipBytes = null;
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                    List<byte[]> _reverifyPDF = new List<byte[]>();
                    List<byte[]> _reverifyImg = new List<byte[]>();
                    CustReviewLoanReverifyMapping _mapping = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(c => c.ID == MappingID).FirstOrDefault();
                    if (_mapping != null)
                    {
                        ReverificationTemplate _template = db.ReverificationTemplate.AsNoTracking().Where(t => t.TemplateID == _mapping.TemplateID).FirstOrDefault();
                        List<CustReverificationDocMapping> _reverifyDocs = db.CustReverificationDocMapping.AsNoTracking().Where(c => c.CustomerID == _mapping.CustomerID && c.ReverificationID == _mapping.ReverificationID).ToList();

                        if (_template != null)
                        {
                            if (IsCoverLetterReq)
                            {
                                string baseImage = string.Empty;
                                string guid = Convert.ToString(db.ReverificationMaster.AsNoTracking().Where(r => r.ReverificationID == _mapping.ReverificationID).Select(r => r.LogoGuid).FirstOrDefault());
                                if (!string.IsNullOrEmpty(guid))
                                {
                                    byte[] _reverificationImage = _imageWrapper.GetObject("reverification", guid);
                                    baseImage = Convert.ToBase64String(_reverificationImage);
                                }

                                StreamReader srHTML = new StreamReader(HostingEnvironment.MapPath("~/Content/ReverificationTemplates/" + _template.TemplateFileName));
                                string templateStr = srHTML.ReadToEnd();
                                //if (!string.IsNullOrEmpty(guid))
                                //{
                                //    templateStr = templateStr.Replace("@reverifyimage@", baseImage);
                                //}
                                srHTML.Dispose();

                                StreamReader srCSS = new StreamReader(HostingEnvironment.MapPath("~/Content/bootstrap.css"));
                                string templateCSS = srCSS.ReadToEnd();
                                srCSS.Dispose();

                                StreamReader srMainCSS = new StreamReader(HostingEnvironment.MapPath("~/Content/main.css"));
                                templateCSS += srMainCSS.ReadToEnd();
                                srMainCSS.Dispose();

                                Dictionary<string, object> jData = JsonConvert.DeserializeObject<Dictionary<string, object>>(TemplateJson);
                                string TemplateString = SmartFormat.Format(templateStr, jData);

                                //_reverifyPDF.Add(ImageUtilities.ConvertHTMLtoPDFByte(TemplateString, templateCSS));
                                _reverifyPDF.Add(ImageUtilities.ConvertHTMLtoPDFByteWithImage(TemplateString, templateCSS));
                            }
                            List<RequiredDocument> _docIDs = RequiredDocIDs == null || RequiredDocIDs.Equals(string.Empty) ? new List<RequiredDocument>() : JsonConvert.DeserializeObject<List<RequiredDocument>>(RequiredDocIDs);

                            //_reverifyDocs = _reverifyDocs.Where(r => _docIDs.Any(d => Convert.ToInt64(d) == r.DocumentTypeID)).ToList();

                            foreach (var item in _docIDs)
                            {
                                List<LoanImage> _loanImgs = GetLoanImages(LoanID, Convert.ToInt64(item.DocumentID), item.Version);

                                foreach (LoanImage imgdetail in _loanImgs)
                                {
                                    byte[] _imageBytes = _imageWrapper.GetLoanImage(imgdetail.LoanID, imgdetail.ImageGUID.GetValueOrDefault());
                                    _reverifyImg.Add(_imageBytes);
                                }
                            }

                            LoanReverification data = db.LoanReverification.AsNoTracking().Where(lr => lr.LoanID == LoanID && lr.CustomerID == _mapping.CustomerID && lr.ReviewTypeID == _mapping.ReviewTypeID && lr.LoanTypeID == _mapping.LoanTypeID && lr.ReverificationID == _mapping.ReverificationID && lr.ReverificationName == ReverificationName).FirstOrDefault();
                            List<LoanReverification> _loanRev = new List<LoanReverification>();
                            if (data != null)
                            {
                                _loanRev = db.LoanReverification.AsNoTracking().Where(lr => lr.LoanID == LoanID && lr.CustomerID == _mapping.CustomerID && lr.ReviewTypeID == _mapping.ReviewTypeID && lr.LoanTypeID == _mapping.LoanTypeID && lr.ReverificationID == _mapping.ReverificationID && lr.ReverificationName.StartsWith(ReverificationName)).ToList();
                            }
                            // List<LoanReverification> _loanRev = db.LoanReverification.AsNoTracking().Where(lr => lr.LoanID == LoanID && lr.CustomerID == _mapping.CustomerID && lr.ReviewTypeID == _mapping.ReviewTypeID && lr.LoanTypeID == _mapping.LoanTypeID && lr.ReverificationID == _mapping.ReverificationID && lr.ReverificationName.StartsWith(ReverificationName)).ToList();
                            Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                            User _user = db.Users.AsNoTracking().Where(u => u.UserID == UserID).FirstOrDefault();
                            ReverificationName = _loanRev.Count == 0 ? ReverificationName : $"{ReverificationName} - V{(_loanRev.Count + 1).ToString()}";
                            //if (_loanRev != null)
                            //{
                            //    _loanRev.ReverificationName = ReverificationName;
                            //    _loanRev.IsCoverLetterRequired = IsCoverLetterReq;
                            //    _loanRev.RequiredDocuments = RequiredDocIDs;
                            //    _loanRev.ReverificationFields = TemplateJson;
                            //    _loanRev.ModifiedOn = DateTime.Now;
                            //    db.Entry(_loanRev).State = EntityState.Modified;
                            //    db.SaveChanges();

                            //    if (_loan != null && _user != null)
                            //        LoanAudit.InsertLoanAudit(db, _loan, $"{ReverificationName} been initiated by {_user.LastName} {_user.FirstName}");
                            //}
                            //else
                            //{
                            LoanReverification _loanReverification = new LoanReverification();
                            _loanReverification.ReverificationName = ReverificationName;
                            _loanReverification.LoanID = LoanID;
                            _loanReverification.CustomerID = _mapping.CustomerID;
                            _loanReverification.ReviewTypeID = _mapping.ReviewTypeID;
                            _loanReverification.LoanTypeID = _mapping.LoanTypeID;
                            _loanReverification.ReverificationID = _mapping.ReverificationID;
                            _loanReverification.ReverificationFields = TemplateJson;
                            _loanReverification.CreatedOn = DateTime.Now;
                            _loanReverification.ModifiedOn = DateTime.Now;
                            _loanReverification.IsCoverLetterRequired = IsCoverLetterReq;
                            _loanReverification.RequiredDocuments = RequiredDocIDs;
                            db.LoanReverification.Add(_loanReverification);
                            db.SaveChanges();

                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.REVERIFICATION_BEEN_INITIATED_BY);

                            if (_loan != null && _user != null)
                                LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.REVERIFICATIONNAME, ReverificationName).Replace(AuditConfigConstant.LASTNAME, _user.LastName).Replace(AuditConfigConstant.FIRSTNAME, _user.FirstName), auditDescs[1].Replace(AuditConfigConstant.REVERIFICATIONNAME, ReverificationName).Replace(AuditConfigConstant.LASTNAME, _user.LastName).Replace(AuditConfigConstant.FIRSTNAME, _user.FirstName));

                            //}

                            _reverifyPDF.Add(CommonUtils.CreatePdfBytes(_reverifyImg));
                            pdfBytes = ImageUtilities.ConcatPDFByteArray(_reverifyPDF);
                            zipBytes = ImageUtilities.contactZipByteArray(pdfBytes, ReverificationName);
                            tran.Commit();
                        }
                    }
                }
            }

            // return pdfBytes;
            return zipBytes;
        }

        public byte[] GetReverificationLoanPDF(Int64 LoanReverificationID)
        {
            byte[] pdfBytes = null;
            byte[] zipBytes = null;
            string reVerificationName;
            using (var db = new DBConnect(TenantSchema))
            {
                List<byte[]> _reverifyPDF = new List<byte[]>();
                List<byte[]> _reverifyImg = new List<byte[]>();
                LoanReverification _loanInit = db.LoanReverification.AsNoTracking().Where(c => c.ID == LoanReverificationID).FirstOrDefault();
                CustReviewLoanReverifyMapping _mapping = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(c => c.CustomerID == _loanInit.CustomerID && c.LoanTypeID == _loanInit.LoanTypeID && c.ReverificationID == _loanInit.ReverificationID).FirstOrDefault();

                reVerificationName = _loanInit.ReverificationName;
                if (_mapping != null)
                {
                    ReverificationTemplate _template = db.ReverificationTemplate.AsNoTracking().Where(t => t.TemplateID == _mapping.TemplateID).FirstOrDefault();
                    List<CustReverificationDocMapping> _reverifyDocs = db.CustReverificationDocMapping.AsNoTracking().Where(c => c.CustomerID == _mapping.CustomerID && c.ReverificationID == _mapping.ReverificationID).ToList();

                    if (_template != null)
                    {
                        if (_loanInit.IsCoverLetterRequired)
                        {
                            StreamReader srHTML = new StreamReader(HostingEnvironment.MapPath("~/Content/ReverificationTemplates/" + _template.TemplateFileName));
                            string templateStr = srHTML.ReadToEnd();
                            srHTML.Dispose();

                            StreamReader srCSS = new StreamReader(HostingEnvironment.MapPath("~/Content/bootstrap.css"));
                            string templateCSS = srCSS.ReadToEnd();
                            srCSS.Dispose();

                            StreamReader srMainCSS = new StreamReader(HostingEnvironment.MapPath("~/Content/main.css"));
                            templateCSS += srMainCSS.ReadToEnd();
                            srMainCSS.Dispose();

                            Dictionary<string, object> jData = JsonConvert.DeserializeObject<Dictionary<string, object>>(_loanInit.ReverificationFields);
                            string TemplateString = SmartFormat.Format(templateStr, jData);

                            //_reverifyPDF.Add(ImageUtilities.ConvertHTMLtoPDFByte(TemplateString, templateCSS));
                            _reverifyPDF.Add(ImageUtilities.ConvertHTMLtoPDFByteWithImage(TemplateString, templateCSS));
                        }
                        List<RequiredDocument> _docIDs = _loanInit.RequiredDocuments == null || _loanInit.RequiredDocuments.Equals(string.Empty) ? new List<RequiredDocument>() : JsonConvert.DeserializeObject<List<RequiredDocument>>(_loanInit.RequiredDocuments);

                        //List<string> _docIDs = _loanInit.RequiredDocuments == null || _loanInit.RequiredDocuments.Equals(string.Empty) ? new List<string>() : _loanInit.RequiredDocuments.Split(',').ToList();

                        //_reverifyDocs = _reverifyDocs.Where(r => _docIDs.Any(d => Convert.ToInt64(d) == r.DocumentTypeID)).ToList();
                        ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                        string _footerText = GetPDFFooterName();
                        foreach (var item in _docIDs)
                        {
                            List<LoanImage> _loanImgs = GetLoanImages(_loanInit.LoanID, Convert.ToInt64(item.DocumentID), item.Version);

                            foreach (LoanImage imgdetail in _loanImgs)
                            {
                                byte[] _imageBytes = _imageWrapper.GetLoanImage(imgdetail.LoanID, imgdetail.ImageGUID.GetValueOrDefault());
                                if (!string.IsNullOrEmpty(_footerText))
                                {
                                    _imageBytes = CommonUtils.CreateHeaderFooterPDF(_imageBytes, _footerText);
                                }
                                _reverifyImg.Add(_imageBytes);
                            }
                        }
                        _reverifyPDF.Add(CommonUtils.CreatePdfBytes(_reverifyImg));
                        pdfBytes = ImageUtilities.ConcatPDFByteArray(_reverifyPDF);

                        zipBytes = ImageUtilities.contactZipByteArray(pdfBytes, reVerificationName);

                    }
                }
            }
            return zipBytes;
        }

        public string GetLoanNotes(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan loanNotes = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                if (loanNotes != null)
                    return string.IsNullOrEmpty(loanNotes.Notes) ? string.Empty : loanNotes.Notes;

            }
            return string.Empty;
        }

        public bool UpdateLoanNotes(Int64 LoanID, string LoanNotes)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan dbObject = db.Loan.Where(l => l.LoanID == LoanID).First();
                    dbObject.Notes = LoanNotes;
                    db.Entry(dbObject).State = EntityState.Modified;
                    db.SaveChanges();
                    tran.Commit();
                    return true;
                }
            }
        }

        public Loan GetLoanHeaderDeatils(Int64 LoanID)
        {
            Loan loan = null;
            using (var db = new DBConnect(TenantSchema))
            {
                loan = new Loan();
                loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
            }
            return loan;
        }

        public string GetLoanTypeName(Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanTypeMaster loan = db.LoanTypeMaster.Where(l => l.LoanTypeID == LoanTypeID).AsNoTracking().FirstOrDefault();

                if (loan != null)
                    return loan.LoanTypeName;
            }
            return string.Empty;
        }

        public string GetServiceTypeName(Int64 ServiceTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster _reviewType = db.ReviewTypeMaster.Where(l => l.ReviewTypeID == ServiceTypeID).AsNoTracking().FirstOrDefault();

                if (_reviewType != null)
                    return _reviewType.ReviewTypeName;
            }
            return string.Empty;
        }


        public object GetLoanDocInfo(Int64 LoanID, Int64 DocumentID, string VersionNumber)
        {
            object docFields = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Int64 _loanImageCount = 0;
                Int64 _docFirstImgID = 0;
                Int64 _docSecondImgID = 0;
                List<DocumentLevelFields> _docLevelFields = new List<DocumentLevelFields>();
                List<DataTable> _docTables = new List<DataTable>();

                LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == LoanID).FirstOrDefault();
                List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && ld.Version == VersionNumber).OrderBy(im => im.PageNo).ToList();
                if (_loanImages != null && _loanImages.Count > 0)
                {
                    _loanImageCount = _loanImages.Count();
                    LoanImage lImg = _loanImages.OrderBy(im => im.PageNo).FirstOrDefault();
                    _docFirstImgID = lImg.LoanImageID;
                    if (_loanImageCount > 1)
                    {
                        int nxtIndex = _loanImages.IndexOf(lImg) + 1;
                        lImg = _loanImages[nxtIndex];
                        _docSecondImgID = lImg.LoanImageID;
                    }
                }
                if (_loanDetail != null)
                {
                    Batch loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);
                    int _intVersionNumber = Convert.ToInt32(VersionNumber);
                    Documents _doc = loanBatch.Documents.Where(d => d.DocumentTypeID == DocumentID && d.VersionNumber == _intVersionNumber).FirstOrDefault();
                    _docLevelFields = _doc.DocumentLevelFields;
                    _docTables = _doc.DataTables;
                    if (_docTables == null || _docTables.Count == 0)
                    {
                        List<DocumetTypeTables> _dTT = db.DocumetTypeTables.AsNoTracking().Where(dt => dt.DocumentTypeID == _doc.DocumentTypeID).ToList();
                        foreach (DocumetTypeTables dt in _dTT)
                        {

                            _docTables.Add(JsonConvert.DeserializeObject<DataTable>(dt.TableJson));
                        }
                    }
                    if (_docLevelFields != null)
                    {
                        foreach (DocumentLevelFields _field in _docLevelFields)
                        {
                            string _fieldDisplayName = GetFieldDisplayName(_field.FieldID);
                            _field.FieldDisplayName = _fieldDisplayName.Equals(string.Empty) ? _field.Name : _fieldDisplayName;
                        }
                    }
                }

                docFields = new { TempDocTables = (_docTables != null) ? _docTables.ToArray() : null, DocLevelFields = _docLevelFields.ToArray(), ImageCount = _loanImageCount, CurrentPage = 1, ImageID = _docFirstImgID, NextImageID = _docSecondImgID, DocTables = _docTables };
            }
            return docFields;
        }

        public object GetLoanDocInfo(Int64 LoanID, Int64 DocumentID)
        {
            object docFields = null;
            using (var db = new DBConnect(TenantSchema))
            {
                string VersionNumber = "0";
                Int64 _loanImageCount = 0;
                Int64 _docFirstImgID = 0;
                Int64 _docSecondImgID = 0;

                LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == LoanID).FirstOrDefault();

                if (_loanDetail != null)
                {
                    Batch loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);

                    VersionNumber = loanBatch.Documents.Where(d => d.DocumentTypeID == DocumentID).OrderByDescending(d => d.VersionNumber).Select(d => d.VersionNumber).FirstOrDefault().ToString();

                    List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && ld.Version == VersionNumber).OrderBy(im => im.PageNo).ToList();
                    if (_loanImages != null && _loanImages.Count > 0)
                    {
                        _loanImageCount = _loanImages.Count();
                        LoanImage lImg = _loanImages.OrderBy(im => im.PageNo).FirstOrDefault();
                        _docFirstImgID = lImg.LoanImageID;
                        if (_loanImageCount > 1)
                        {
                            int nxtIndex = _loanImages.IndexOf(lImg) + 1;
                            lImg = _loanImages[nxtIndex];
                            _docSecondImgID = lImg.LoanImageID;
                        }
                    }
                }

                docFields = new { ImageCount = _loanImageCount, CurrentPage = 1, ImageID = _docFirstImgID, NextImageID = _docSecondImgID, VersionNumber = VersionNumber };
            }
            return docFields;
        }

        public object GetLoanReverifyDoc(Int64 LoanID, Int64 DocumentID, string VersionNumber)
        {
            object docFields = null;
            using (var db = new DBConnect(TenantSchema))
            {
                // string VersionNumber = "0";
                Int64 _loanImageCount = 0;
                Int64 _docFirstImgID = 0;
                Int64 _docSecondImgID = 0;

                //LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == LoanID).FirstOrDefault();

                //if (_loanDetail != null)
                // {
                //Batch loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);

                //VersionNumber = loanBatch.Documents.Where(d => d.DocumentTypeID == DocumentID).OrderByDescending(d => d.VersionNumber).Select(d => d.VersionNumber).FirstOrDefault().ToString();

                List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && ld.Version == VersionNumber).OrderBy(im => im.PageNo).ToList();
                if (_loanImages != null && _loanImages.Count > 0)
                {
                    _loanImageCount = _loanImages.Count();
                    LoanImage lImg = _loanImages.OrderBy(im => im.PageNo).FirstOrDefault();
                    _docFirstImgID = lImg.LoanImageID;
                    if (_loanImageCount > 1)
                    {
                        int nxtIndex = _loanImages.IndexOf(lImg) + 1;
                        lImg = _loanImages[nxtIndex];
                        _docSecondImgID = lImg.LoanImageID;
                    }
                }
                //}

                docFields = new { ImageCount = _loanImageCount, CurrentPage = 1, ImageID = _docFirstImgID, NextImageID = _docSecondImgID, VersionNumber = VersionNumber };
            }
            return docFields;
        }


        public object GetImageByID(Int64 ImageID)
        {
            object _img = null;
            using (var db = new DBConnect(TenantSchema))
            {
                ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);

                LoanImage _image = db.LoanImage.AsNoTracking().Where(i => i.LoanImageID == ImageID).FirstOrDefault();

                if (_image != null)
                {
                    byte[] _imageBytes = _imageWrapper.GetLoanImage(_image.LoanID, _image.ImageGUID.GetValueOrDefault());

                    string bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_imageBytes));

                    if (!string.IsNullOrEmpty(bs64Image))
                        _img = new { Image = bs64Image };
                }
            }
            return _img;
        }

        public object GetLoanBase64Images(Int64 LoanID, Int64 DocumentID, int versionNumber, long _pageNo, long lastPageNumber, Boolean ShowAllDocs)
        {
            List<object> loanImages = new List<object>();
            int lastVersionNumber = versionNumber - 1;



            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanImage> allLoanImages = new List<LoanImage>();
                //LoanImage _loanImages = null;
                if (ShowAllDocs)
                {
                    allLoanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && (ld.Version == versionNumber.ToString())).OrderBy(im => im.PageNo).ToList();

                }
                else
                {
                    for (Int64 i = lastPageNumber; i <= _pageNo; i++)
                    {
                        allLoanImages.Add(db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && ld.PageNo == i && (ld.Version == versionNumber.ToString())).OrderBy(im => im.PageNo).FirstOrDefault());
                    }
                }

                //if (_loanImages != null)
                //{
                //    //foreach (LoanImage lImg in _loanImages)
                //    //{
                //    int imgeWidth = 0;
                //    int imgHeight = 0;
                //    ImageUtilities.GetImageSize(_loanImages.Image, out imgeWidth, out imgHeight);
                //    string bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_loanImages.Image));
                //    loanImages.Add(new
                //    {
                //        Image = bs64Image,
                //        ImageVersion = _loanImages.Version,
                //        PageNo = _loanImages.PageNo,
                //        OrginalImageHeight = imgHeight,
                //        OrginalImageWidth = imgeWidth,
                //        CompressedImageHeight = imgHeight,
                //        CompressedImageWidth = imgeWidth
                //    });
                //    //}
                //}
                if (allLoanImages != null)
                {
                    //ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);

                    foreach (LoanImage lImg in allLoanImages)
                    {

                        //int imgeWidth = 0;
                        //int imgHeight = 0;
                        int imgeWidth = 1654;
                        int imgHeight = 2339;
                        //byte[] _imageBytes = _imageWrapper.GetLoanImage(lImg.LoanID, lImg.ImageGUID.GetValueOrDefault());
                        //ImageUtilities.GetImageSize(_imageBytes, out imgeWidth, out imgHeight);
                        //string bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_imageBytes));
                        loanImages.Add(new
                        {
                            ImageGuid = CommonUtils.EnDecrypt(Convert.ToString(lImg.ImageGUID), false),
                            ImageVersion = lImg.Version,
                            PageNo = lImg.PageNo,
                            OrginalImageHeight = imgHeight,
                            OrginalImageWidth = imgeWidth,
                            CompressedImageHeight = imgHeight,
                            CompressedImageWidth = imgeWidth
                        });
                    }
                }
            }
            return loanImages;
        }

        public byte[] GetLoanImage(Int64 LoanID, string LoanGuid)
        {
            Guid ImageGUID = new Guid(LoanGuid);
            byte[] _imageBytes = new ImageMinIOWrapper(TenantSchema).GetLoanImage(LoanID, ImageGUID);
            return _imageBytes;
        }



        public object GetLoanBase64ImageByPageNo(Int64 LoanID, Int64 DocumentID, string VersionNumber, Int64 pageNo)
        {

            if (VersionNumber == "0")
                VersionNumber = "1";

            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && ld.PageNo == pageNo && ld.Version == VersionNumber).ToList();
                if (_loanImages != null)
                {
                    ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);

                    foreach (LoanImage lImg in _loanImages)
                    {
                        byte[] _imageBytes = _imageWrapper.GetLoanImage(lImg.LoanID, lImg.ImageGUID.GetValueOrDefault());
                        string bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_imageBytes));
                        return new { Image = bs64Image, ImageVersion = lImg.Version };
                    }
                }
            }
            return new { Image = "", ImageVersion = "" }; ;
        }
        public object GetImageByID(Int64 LoanID, Int64 DocumentID, Int64 ImageID, string VersionNumber)
        {
            object docFields = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Int64 _loanImageCount = 0;
                Int64 _docPreImgID = 0;
                Int64 _docNextImgID = 0;
                string bs64Image = String.Empty;
                List<LoanImage> _loanImages = db.LoanImage.AsNoTracking().Where(ld => ld.LoanID == LoanID && ld.DocumentTypeID == DocumentID && ld.Version == VersionNumber).OrderBy(im => im.PageNo).ToList();
                if (_loanImages != null)
                {
                    ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
                    _loanImageCount = _loanImages.Count();
                    LoanImage lImg = _loanImages.Where(i => i.LoanImageID == ImageID).FirstOrDefault();
                    byte[] _imageBytes = _imageWrapper.GetLoanImage(lImg.LoanID, lImg.ImageGUID.GetValueOrDefault());
                    bs64Image = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(_imageBytes));

                    if (_loanImageCount > 1)
                    {
                        int nxtIndex = _loanImages.IndexOf(lImg) + 1;

                        if (_loanImageCount > nxtIndex)
                        {
                            lImg = _loanImages[nxtIndex];
                            _docNextImgID = lImg.LoanImageID;
                        }

                        int preIndex = _loanImages.IndexOf(lImg) - (_docNextImgID == 0 ? 1 : 2);

                        if (preIndex >= 0)
                        {
                            lImg = _loanImages[preIndex];
                            _docPreImgID = lImg.LoanImageID;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(bs64Image))
                    docFields = new { Image = bs64Image, PreImageID = _docPreImgID, NextImageID = _docNextImgID, CurrentImageID = ImageID };
            }
            return docFields;
        }

        public bool ChangeDocumentType(Int64 LoanID, Int64 OldDocumentID, Int64 NewDocumentID, int VersionNumber, Int64 CurrentUserID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    LoanDetail loanDetail = db.LoanDetail.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                    if (loanDetail != null)
                    {
                        Batch loanBatch = JsonConvert.DeserializeObject<Batch>(loanDetail.LoanObject);

                        Documents oldDoc = loanBatch.Documents.Where(d => d.DocumentTypeID == OldDocumentID && d.VersionNumber == VersionNumber).FirstOrDefault();

                        var oldDocList = loanBatch.Documents.Where(ld => ld.DocumentTypeID == OldDocumentID).ToList();

                        Documents checkNewDoc = loanBatch.Documents.Where(d => d.DocumentTypeID == NewDocumentID).OrderByDescending(d => d.VersionNumber).FirstOrDefault();


                        if (oldDoc != null)
                        {
                            int docIndex = loanBatch.Documents.IndexOf(oldDoc);
                            string auditMsg = string.Empty, auditSysMsg = string.Empty;
                            DocumentTypeMaster docMaster = db.DocumentTypeMaster.AsNoTracking().Where(d => d.DocumentTypeID == NewDocumentID).FirstOrDefault();
                            List<DocumentFieldMaster> docFields = db.DocumentFieldMaster.AsNoTracking().Where(d => d.DocumentTypeID == NewDocumentID && d.Active).ToList();
                            List<DataTable> _docTables = new List<DataTable>();
                            if (_docTables == null || _docTables.Count == 0)
                            {
                                List<DocumetTypeTables> _dTT = db.DocumetTypeTables.AsNoTracking().Where(dt => dt.DocumentTypeID == NewDocumentID).ToList();
                                foreach (DocumetTypeTables dt in _dTT)
                                    _docTables.Add(JsonConvert.DeserializeObject<DataTable>(dt.TableJson));
                            }
                            //List<>
                            Documents doc = oldDoc;
                            doc.DocumentTypeID = NewDocumentID;
                            doc.Type = docMaster.Name;
                            doc.Description = docMaster.DisplayName;
                            doc.VersionNumber = 1; //Default 1st Version
                            doc.DocumentLevelFields = new List<DocumentLevelFields>();
                            doc.DataTables = _docTables;

                            foreach (DocumentFieldMaster docField in docFields)
                            {
                                DocumentLevelFields field = new DocumentLevelFields()
                                {
                                    FieldID = docField.FieldID,
                                    Type = docField.Name,
                                    Name = docField.Name,
                                    FieldDisplayName = docField.DisplayName,
                                    Value = string.Empty
                                };
                                doc.DocumentLevelFields.Add(field);
                            }

                            oldDocList = loanBatch.Documents.Where(ld => ld.DocumentTypeID == OldDocumentID).ToList();

                            string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENT_TYPE_UPDATED);
                            auditMsg = auditDescs[0].Replace(AuditConfigConstant.OLDDOCUMENTTYPE, oldDoc.Type).Replace(AuditConfigConstant.NEWDOCUMENTTYPE, docMaster.Name);
                            auditSysMsg = auditDescs[1].Replace(AuditConfigConstant.OLDDOCUMENTTYPE, oldDoc.Type).Replace(AuditConfigConstant.NEWDOCUMENTTYPE, docMaster.Name);

                            if (checkNewDoc != null)
                            {
                                doc.VersionNumber = checkNewDoc.VersionNumber + 1;
                                string[] auditVDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENT_TYPE_VERSION_INCREMENTED);
                                auditMsg = auditVDescs[0].Replace(AuditConfigConstant.DOCUMENTTYPENAME, docMaster.Name);
                                auditSysMsg = auditVDescs[1].Replace(AuditConfigConstant.DOCUMENTTYPENAME, docMaster.Name);
                                //auditMsg = String.Format("Document Type : {0} version incremented", docMaster.Name);
                            }

                            oldDocList = loanBatch.Documents.Where(ld => ld.DocumentTypeID == OldDocumentID).ToList();

                            loanBatch.Documents[docIndex] = doc;
                            oldDocList = loanBatch.Documents.Where(ld => ld.DocumentTypeID == OldDocumentID).OrderByDescending(d => d.VersionNumber).ToList();
                            int count = loanBatch.Documents.Count(ld => ld.DocumentTypeID == OldDocumentID); //.Where(ld => ld.DocumentTypeID == OldDocumentID)

                            //Prakash : GIT log(Rosario: Category validation and data table warning errors, Audit complete button not displaying when there is no checklist)
                            //foreach (var lists in oldDocList)
                            //{
                            //    docIndex = loanBatch.Documents.IndexOf(lists);
                            //    lists.VersionNumber = count;
                            //    loanBatch.Documents[docIndex] = lists;
                            //    count--;

                            //    //for(int i=0;count>i  ;count --)
                            //    //{
                            //    //    lists.VersionNumber = count;
                            //    //}
                            //}




                            loanDetail.LoanObject = JsonConvert.SerializeObject(loanBatch);
                            loanDetail.ModifiedOn = DateTime.Now;
                            db.Entry(loanDetail).State = EntityState.Modified;
                            db.SaveChanges();
                            EvaluateRules evalRuleEngine = new EvaluateRules(TenantSchema, LoanID);
                            List<Dictionary<string, string>> checklistDetails = evalRuleEngine.GetAllCheckListDetails;
                            string ruleFindings = JsonConvert.SerializeObject(checklistDetails);
                            List<RuleFinding> rulefinding = JsonConvert.DeserializeObject<List<RuleFinding>>(ruleFindings);
                            Batch batch = JsonConvert.DeserializeObject<Batch>(loanDetail.LoanObject);
                            batch.RuleFinding = rulefinding;
                            loanDetail.LoanObject = JsonConvert.SerializeObject(batch);


                            LoanRuleEngine ruleEngine = new LoanRuleEngine(TenantSchema, LoanID, loanDetail);

                            loanDetail.TotalDocCount = ruleEngine.BatchDocumentCount;
                            loanDetail.MissingDocCount = ruleEngine.MissingDocumentCount;
                            loanDetail.MissingCriticalDocCount = ruleEngine.MissingCriticalDocumentCount;
                            loanDetail.MissingNonCriticalDocCount = ruleEngine.MissingNonCriticalDocumentCount;
                            db.Entry(loanDetail).State = EntityState.Modified;
                            db.SaveChanges();


                            LoanAudit.InsertLoanDetailsAudit(db, loanDetail, CurrentUserID, auditMsg, auditSysMsg);

                            oldDocList = loanBatch.Documents.Where(ld => ld.DocumentTypeID == OldDocumentID).ToList();

                            UpdateLoanImages(db, LoanID, OldDocumentID, NewDocumentID, doc.VersionNumber.ToString(), VersionNumber.ToString());

                            // Update  LoanCheckList Details
                            EvaluateRules evalRuleEngines = new EvaluateRules(TenantSchema, LoanID, loanDetail);
                            List<LoanChecklistAudit> _loancheckList = evalRuleEngines.FetchLoanCheckListDetails();
                            evalRuleEngines.InsertLoanCheckListAuditDetails(_loancheckList);
                        }
                    }
                    tran.Commit();
                    return true;
                }
            }
            return false;
        }

        public Object GetQCIQLookupDetails(Int64 loanID)
        {
            object returnItem = null;
            using (var db = new DBConnect(TenantSchema))
            {
                returnItem = (from l in db.Loan.AsNoTracking()
                              join conn in db.QCIQConnectionString.AsNoTracking().DefaultIfEmpty() on l.ReviewTypeID equals conn.ReviewTypeID
                              join connm in db.QCIQConnectionString.AsNoTracking().DefaultIfEmpty() on 0 equals connm.ReviewTypeID
                              join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                              join rev in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals rev.ReviewTypeID
                              where l.LoanID == loanID
                              select new
                              {
                                  ConnectionString = conn.ConnectIonString,
                                  MasterConnectionString = connm.ConnectIonString,
                                  MasterSQLScript = connm.SQLScript,
                                  SQLScript = conn.SQLScript,
                                  CustomerName = cus.CustomerName,
                                  ReviewTypeName = rev.ReviewTypeName

                              }).FirstOrDefault();
            }

            return returnItem;
        }

        public Boolean QCIQEnabled()
        {

            AppConfig config = null;
            using (var db = new DBConnect(SystemSchema))
            {
                config = db.AppConfig.AsNoTracking().Where(c => c.ConfigKey == ConfigConstant.QCIQSTARTDATEENABLED).FirstOrDefault();
                return Convert.ToBoolean(config.ConfigValue);
            }

        }

        public Dictionary<string, object> GetLoanNumber(Int64 loanID)
        {
            Loan loan = null;
            string loanNumber = string.Empty;
            Dictionary<string, object> result = new Dictionary<string, object> { { "loanNumber", "" }, { "startDate", DateTime.Now } };
            using (var db = new DBConnect(TenantSchema))
            {

                loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
            }
            if (loan != null)
            {
                result["loanNumber"] = loan.LoanNumber;
                result["startDate"] = loan.CreatedOn;
            }

            return result;
        }

        public void UpdateQCIQStartDate(DateTime? QCIQStartDate, Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                    //_loan.IDCCompletionDate = QCIQStartDate;
                    _loan.QCIQStartDate = QCIQStartDate;

                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPDATED_LOAN_TYPE_FROM_QCIQ);
                    LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0], "QCIQ Start Date Updated");

                    tran.Commit();
                }
            }
        }

        public void UpdateLoanCompleteUserDetails(Int64 LoanID, Int64 completeUserRoleID, Int64 completeUserID, string CompleteNotes)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    _loan.CompletedUserID = completeUserID;
                    _loan.CompletedUserRoleID = completeUserRoleID;
                    _loan.CompleteNotes = CompleteNotes;
                    _loan.ModifiedOn = DateTime.Now;
                    _loan.AuditCompletedDate = DateTime.Now;
                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();
                    tran.Commit();
                }
            }

        }
        public object GetLoanReverification(Int64 LoanID)
        {
            object result = null;
            using (var db = new DBConnect(TenantSchema))
            {
                result = (from lr in db.LoanReverification.AsNoTracking()
                          join r in db.ReverificationMaster.AsNoTracking() on lr.ReverificationID equals r.ReverificationID
                          where lr.LoanID == LoanID
                          select new
                          {
                              LoanReverificationID = lr.ID,
                              RevericationName = r.ReverificationName,
                              UserRevericationName = lr.ReverificationName
                          }).ToList();
            }

            return result;

        }

        public object GetLoanBasedReverification(Int64 LoanID)
        {
            object result = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (loan != null)
                {
                    List<ReverificationMaster> _reverifyMs = db.ReverificationMaster.AsNoTracking().ToList();
                    List<ReverificationTemplate> _tempMs = db.ReverificationTemplate.AsNoTracking().ToList();
                    //List<LoanReverification> _lsLoanReverify = db.LoanReverification.AsNoTracking().Where(lr => lr.LoanID == loan.LoanID).ToList();
                    List<CustReviewLoanReverifyMapping> _lsReverify = db.CustReviewLoanReverifyMapping.AsNoTracking().Where(lr => lr.CustomerID == loan.CustomerID && lr.LoanTypeID == loan.LoanTypeID).ToList();
                    // List<DocumentFieldMaster> _lsFields = db.DocumentFieldMaster.AsNoTracking().Where(t => t.DocumentTypeID == DocumentID).ToList();
                    var _reverify = (from lR in _lsReverify
                                     join rv in _reverifyMs on lR.ReverificationID equals rv.ReverificationID
                                     join t in _tempMs on lR.TemplateID equals t.TemplateID
                                     select new
                                     {
                                         MappingID = lR.ID,
                                         ReverificationID = lR.ReverificationID,
                                         ReverificationName = rv.ReverificationName,
                                         TemplateID = t.TemplateID,
                                         TemplateFileName = t.TemplateFileName,
                                         TemplateFieldValue = lR.TemplateFields,
                                         TemplateFields = t.TemplateFields,
                                         MappedDocuments = (from lrNew in db.CustReverificationDocMapping.AsNoTracking()
                                                            join doc in db.DocumentTypeMaster.AsNoTracking() on lrNew.DocumentTypeID equals doc.DocumentTypeID
                                                            //join docFieldMaster in db.DocumentFieldMaster.AsNoTracking() on doc.DocumentTypeID equals docFieldMaster.DocumentTypeID
                                                            where lrNew.CustomerID == lR.CustomerID && lrNew.ReverificationID == lR.ReverificationID
                                                            select new
                                                            {
                                                                DocumentTypeID = lrNew.DocumentTypeID,
                                                                DocumentName = doc.Name,
                                                                DocumentFields = (from docFields in db.DocumentFieldMaster
                                                                                  where docFields.DocumentTypeID == doc.DocumentTypeID
                                                                                  select docFields.Name
                                                                ).ToList()
                                                            }).ToList(),
                                         LogoGuid = rv.LogoGuid
                                     }).ToList();


                    //if (_lsLoanReverify != null)
                    //    result = _reverify.Where(r => !(_lsLoanReverify.Any(l => l.ReverificationID == r.ReverificationID))).ToList();
                    //else
                    result = _reverify;
                }
            }

            return result;
        }

        public List<string> GetFieldsByDocID(Int64 DocumentID)
        {
            List<string> fields = new List<string>();

            using (var db = new DBConnect(TenantSchema))
            {
                List<DocumentFieldMaster> _lsFields = db.DocumentFieldMaster.AsNoTracking().Where(t => t.DocumentTypeID == DocumentID).ToList();

                if (_lsFields != null)
                {
                    foreach (var item in _lsFields)
                    {
                        fields.Add(item.Name);
                    }
                }
            }
            return fields;
        }

        public string GetFieldValue(Int64 LoanID, string DocumentName, string DocumentField)
        {
            string result = null;
            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail _loan = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (_loan != null)
                {
                    Batch _loanDetail = JsonConvert.DeserializeObject<Batch>(_loan.LoanObject);
                    Documents doc = _loanDetail.Documents.Where(d => d.Type == DocumentName).OrderByDescending(d => d.VersionNumber).FirstOrDefault();
                    if (doc != null)
                    {
                        string fieldValue = doc.DocumentLevelFields.Where(f => f.Name == DocumentField).Select(f => f.Value).FirstOrDefault();
                        result = string.IsNullOrEmpty(fieldValue) ? string.Empty : fieldValue;
                    }
                }
            }
            return result;
        }

        public bool UpdateLoanTypeFromQCIQ(Int64 LoanID, string loanType, DateTime? QCIQStartDate)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                    LoanTypeMaster ltype = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeName == loanType).FirstOrDefault();
                    if (loan != null && ltype != null && ltype.LoanTypeID != 0)
                    {
                        loan.LoanTypeID = ltype.LoanTypeID;
                        loan.ModifiedOn = DateTime.Now;
                        //loan.QCIQStartDate = QCIQStartDate;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPDATED_LOAN_TYPE_FROM_QCIQ);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);
                        tran.Commit();
                    }
                    else
                    {
                        throw new Exception("Loan Type " + loanType + " not exists in IntellaLend");
                    }
                }
            }
            return true;
        }

        #region MAS
        public bool UpdateLOSClassificationExceptionFromMAS(Int64 LoanID, string batchid, string batchName)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                MASBaseData masBaseData = new MASBaseData();

                Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                var item = (from l in db.Loan.AsNoTracking()
                            join RTM in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals RTM.ReviewTypeID
                            join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                            join LTM in db.LoanTypeMaster.AsNoTracking() on l.LoanTypeID equals LTM.LoanTypeID
                            where l.LoanID == loan.LoanID
                            select new
                            {
                                lenderCode = cus.CustomerCode,
                                lenderName = cus.CustomerName,
                                loanId = l.LoanNumber,
                                loanType = LTM.LoanTypeName,
                                reviewType = RTM.ReviewTypeName,
                                priority = l.Priority,
                                auditPeriod = l.AuditMonthYear,
                            }).FirstOrDefault();

                masBaseData.LenderCode = item.lenderCode.ToString();
                masBaseData.LenderName = item.lenderName;
                masBaseData.LoanID = item.loanId != null ? item.loanId : "";
                masBaseData.LoanType = item.loanType.ToString();
                masBaseData.ServiceType = item.reviewType;
                masBaseData.Priority = Convert.ToInt32(item.priority);
                masBaseData.AuditPeriod = item.auditPeriod != null ? item.auditPeriod.ToString("MMM", CultureInfo.InvariantCulture) + " " + item.auditPeriod.Year.ToString() : "";

                if (loan != null)
                {
                    IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    CustomerConfig _config = new TenantConfigDataAccess(TenantSchema).GetConfigValues(0, "Ephesoft_URL");
                    LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (_loanSearch != null)
                    {
                        masBaseData.AuditDueDate = _loanSearch.AuditDueDate != null ? _loanSearch.AuditDueDate.GetValueOrDefault().ToString(DateConstance.ShortDateFormart) : "";
                    }
                    string url = string.Empty;
                    if (_config != null)
                    {
                        url = _config.ConfigValue + "/ReviewValidate.html?batch_id=";
                    }

                    if (_idcfield != null)
                    {
                        masBaseData.IDCBatchID = batchid;
                        masBaseData.IDCBatchName = batchName;
                        masBaseData.IDCBC = string.IsNullOrEmpty(_idcfield.IDCBatchClassID) ? string.Empty : _idcfield.IDCBatchClassID;
                        masBaseData.IDCBCName = string.IsNullOrEmpty(_idcfield.IDCBatchClassName) ? string.Empty : _idcfield.IDCBatchClassName;
                        masBaseData.URL = url + batchid;
                    }

                    masBaseData.Event = LOSExportEventConstant.LOS_CLASSIFICATION_EXCEPTION_EVENT;
                    masBaseData.EventDescription = LOSExportEventConstant.LOS_CLASSIFICATION_EXCEPTION_EVENT_DESC;
                }


                string fileJson = JsonConvert.SerializeObject(masBaseData);

                Dictionary<string, string> dicObjects;
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                dicObjects = CommonUtils.ExtractDataFromString(batchName, patterns);

                Int64 _trailingAuditID = 0;
                Int64 _docID = 0;
                if (dicObjects.ContainsKey("DOC_ID"))
                {
                    _docID = Convert.ToInt64(dicObjects["DOC_ID"]);
                    string fileName = $"{TenantSchema.ToUpper()}_{loan.LoanID.ToString()}_{_docID.ToString()}";

                    AuditLoanMissingDoc _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loan.LoanID && l.DocID == _docID).FirstOrDefault();

                    if (_auditDoc == null)
                        _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loan.LoanID && l.FileName.Contains(fileName)).FirstOrDefault();

                    if (_auditDoc != null)
                    {
                        _trailingAuditID = _auditDoc.AuditID;
                    }
                }

                LOSExportFileStaging _losExportFileStaging = new LOSExportFileStaging()
                {
                    LoanID = loan.LoanID,
                    FileName = (string.IsNullOrEmpty(loan.LoanNumber) ? loan.LoanID.ToString() : loan.LoanNumber) + "_OCR_Review_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json", //loannumber
                    FileJson = fileJson,
                    FileType = LOSExportFileTypeConstant.LOS_CLASSIFICATION_EXCEPTION,
                    ErrorMsg = "",
                    Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                    TrailingAuditID = _trailingAuditID,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                db.LOSExportFileStaging.Add(_losExportFileStaging);
                db.SaveChanges();

            }
            return true;
        }

        public bool UpdateLOSClassificationResultsFromMAS(Int64 LoanID, string batchid, List<MASDocument> masDocuments, string batchName, string batchClassID, string batchClassName)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                MASData masData = new MASData();

                Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                var item = (from l in db.Loan.AsNoTracking()
                            join RTM in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals RTM.ReviewTypeID
                            join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                            join LTM in db.LoanTypeMaster.AsNoTracking() on l.LoanTypeID equals LTM.LoanTypeID
                            where l.LoanID == loan.LoanID
                            select new
                            {
                                lenderCode = cus.CustomerCode,
                                lenderName = cus.CustomerName,
                                loanId = l.LoanNumber,
                                loanType = LTM.LoanTypeName,
                                reviewType = RTM.ReviewTypeName,
                                priority = l.Priority,
                                auditPeriod = l.AuditMonthYear
                            }).FirstOrDefault();

                masData.LenderCode = item.lenderCode.ToString();
                masData.LenderName = item.lenderName;
                masData.LoanID = item.loanId != null ? item.loanId : "";
                masData.LoanType = item.loanType.ToString();
                masData.ServiceType = item.reviewType;
                masData.Priority = Convert.ToInt32(item.priority);
                masData.AuditPeriod = item.auditPeriod != null ? item.auditPeriod.ToString("MMM", CultureInfo.InvariantCulture) + " " + item.auditPeriod.Year.ToString() : "";

                if (loan != null)
                {
                    IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    CustomerConfig _config = new TenantConfigDataAccess(TenantSchema).GetConfigValues(0, "Ephesoft_URL");
                    LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (_loanSearch != null)
                    {
                        masData.AuditDueDate = _loanSearch.AuditDueDate != null ? _loanSearch.AuditDueDate.GetValueOrDefault().ToString(DateConstance.ShortDateFormart) : "";
                    }
                    string url = string.Empty;
                    if (_config != null)
                    {
                        url = _config.ConfigValue + "/ReviewValidate.html?batch_id=";
                    }



                    if (_idcfield != null)
                    {
                        masData.IDCBatchID = batchid;
                        masData.IDCBatchName = batchName;
                        masData.URL = url + batchid;
                        masData.IDCBC = string.IsNullOrEmpty(_idcfield.IDCBatchClassID) ? string.Empty : _idcfield.IDCBatchClassID;
                        masData.IDCBCName = string.IsNullOrEmpty(_idcfield.IDCBatchClassName) ? string.Empty : _idcfield.IDCBatchClassName;

                        // if reviewer not available set to system
                        masData.ReviewBy = string.IsNullOrEmpty(_idcfield.IDCReviewerName) ? "System" : _idcfield.IDCReviewerName.Substring(0, _idcfield.IDCReviewerName.LastIndexOf('|')).Trim();
                        masData.ReviewOn = _idcfield.IDCLevelOneCompletionDate.ToString();
                        masData.ReviewDuration = string.IsNullOrEmpty(_idcfield.IDCLevelOneDuration) ? "" : _idcfield.IDCLevelOneDuration.Substring(0, _idcfield.IDCLevelOneDuration.LastIndexOf('|')).Trim();
                    }

                    masData.Event = LOSExportEventConstant.LOS_CLASSIFICATION_RESULTS_EVENT;
                    masData.EventDescription = LOSExportEventConstant.LOS_CLASSIFICATION_RESULTS_EVENT_DESC;
                }

                masData.Documents = masDocuments;

                string fileJson = JsonConvert.SerializeObject(masData);

                Dictionary<string, string> dicObjects;
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                dicObjects = CommonUtils.ExtractDataFromString(batchName, patterns);

                Int64 _trailingAuditID = 0;
                Int64 _docID = 0;
                if (dicObjects.ContainsKey("DOC_ID"))
                {
                    _docID = Convert.ToInt64(dicObjects["DOC_ID"]);
                    string fileName = $"{TenantSchema.ToUpper()}_{loan.LoanID.ToString()}_{_docID.ToString()}";

                    AuditLoanMissingDoc _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loan.LoanID && l.DocID == _docID).FirstOrDefault();

                    if (_auditDoc == null)
                        _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loan.LoanID && l.FileName.Contains(fileName)).FirstOrDefault();

                    if (_auditDoc != null)
                    {
                        _trailingAuditID = _auditDoc.AuditID;
                    }
                }

                LOSExportFileStaging _losExportFileStaging = new LOSExportFileStaging()
                {
                    LoanID = loan.LoanID,
                    FileName = (string.IsNullOrEmpty(loan.LoanNumber) ? loan.LoanID.ToString() : loan.LoanNumber) + "_OCR_Review_Results_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json",
                    FileJson = fileJson,
                    FileType = LOSExportFileTypeConstant.LOS_CLASSIFICATION_RESULTS,
                    ErrorMsg = "",
                    Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                    TrailingAuditID = _trailingAuditID,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                db.LOSExportFileStaging.Add(_losExportFileStaging);
                db.SaveChanges();


            }
            return true;
        }

        public bool UpdateLOSValidationExceptionFromMAS(Int64 LoanID, string batchid, string batchName)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                MASBaseData masBaseData = new MASBaseData();

                Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                var item = (from l in db.Loan.AsNoTracking()
                            join RTM in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals RTM.ReviewTypeID
                            join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                            join LTM in db.LoanTypeMaster.AsNoTracking() on l.LoanTypeID equals LTM.LoanTypeID
                            where l.LoanID == loan.LoanID
                            select new
                            {
                                lenderCode = cus.CustomerCode,
                                lenderName = cus.CustomerName,
                                loanId = l.LoanNumber,
                                loanType = LTM.LoanTypeName,
                                reviewType = RTM.ReviewTypeName,
                                priority = l.Priority,
                                auditPeriod = l.AuditMonthYear,
                            }).FirstOrDefault();

                masBaseData.LenderCode = item.lenderCode.ToString();
                masBaseData.LenderName = item.lenderName;
                masBaseData.LoanID = item.loanId != null ? item.loanId : "";
                masBaseData.LoanType = item.loanType.ToString();
                masBaseData.ServiceType = item.reviewType;
                masBaseData.Priority = Convert.ToInt32(item.priority);
                masBaseData.AuditPeriod = item.auditPeriod != null ? item.auditPeriod.ToString("MMM", CultureInfo.InvariantCulture) + " " + item.auditPeriod.Year.ToString() : "";

                if (loan != null)
                {
                    IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    CustomerConfig _config = new TenantConfigDataAccess(TenantSchema).GetConfigValues(0, "Ephesoft_URL");
                    LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (_loanSearch != null)
                    {
                        masBaseData.AuditDueDate = _loanSearch.AuditDueDate != null ? _loanSearch.AuditDueDate.GetValueOrDefault().ToString(DateConstance.ShortDateFormart) : "";
                    }
                    string url = string.Empty;
                    if (_config != null)
                    {
                        url = _config.ConfigValue + "/ReviewValidate.html?batch_id=";
                    }

                    if (_idcfield != null)
                    {
                        masBaseData.IDCBatchID = batchid;
                        masBaseData.IDCBatchName = batchName;
                        masBaseData.IDCBC = string.IsNullOrEmpty(_idcfield.IDCBatchClassID) ? string.Empty : _idcfield.IDCBatchClassID;
                        masBaseData.IDCBCName = string.IsNullOrEmpty(_idcfield.IDCBatchClassName) ? string.Empty : _idcfield.IDCBatchClassName;
                        masBaseData.URL = url + batchid;
                    }

                    masBaseData.Event = LOSExportEventConstant.LOS_VALIDATION_EXCEPTION_EVENT;
                    masBaseData.EventDescription = LOSExportEventConstant.LOS_VALIDATION_EXCEPTION_EVENT_DESC;
                }


                string fileJson = JsonConvert.SerializeObject(masBaseData);

                Dictionary<string, string> dicObjects;
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                dicObjects = CommonUtils.ExtractDataFromString(batchName, patterns);

                Int64 _trailingAuditID = 0;
                Int64 _docID = 0;
                if (dicObjects.ContainsKey("DOC_ID"))
                {
                    _docID = Convert.ToInt64(dicObjects["DOC_ID"]);
                    string fileName = $"{TenantSchema.ToUpper()}_{loan.LoanID.ToString()}_{_docID.ToString()}";

                    AuditLoanMissingDoc _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loan.LoanID && l.DocID == _docID).FirstOrDefault();

                    if (_auditDoc == null)
                        _auditDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == loan.LoanID && l.FileName.Contains(fileName)).FirstOrDefault();

                    if (_auditDoc != null)
                    {
                        _trailingAuditID = _auditDoc.AuditID;
                    }
                }

                LOSExportFileStaging _losExportFileStaging = new LOSExportFileStaging()
                {
                    LoanID = loan.LoanID,
                    FileName = (string.IsNullOrEmpty(loan.LoanNumber) ? loan.LoanID.ToString() : loan.LoanNumber) + "_OCR_Validation_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".json",
                    FileJson = fileJson,
                    FileType = LOSExportFileTypeConstant.LOS_VALIDATION_EXCEPTION,
                    ErrorMsg = "",
                    Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                    TrailingAuditID = _trailingAuditID,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                db.LOSExportFileStaging.Add(_losExportFileStaging);
                db.SaveChanges();

            }
            return true;
        }
        #endregion MAS

        public bool UpdateLoanTypeFromEncompass(Int64 LoanID, string LoanNumber, string BorrowerName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    // EncompassConnector _encompassConnector = new EncompassConnector();

                    Dictionary<string, string> _fetchFields = new Dictionary<string, string>();
                    _fetchFields.Add(EncompassFieldConstant.LOAN_TYPE, "");
                    _fetchFields.Add(EncompassFieldConstant.LOAN_CLOSER, "");
                    _fetchFields.Add(EncompassFieldConstant.LOAN_OFFICER, "");
                    _fetchFields.Add(EncompassFieldConstant.POST_CLOSER, "");
                    _fetchFields.Add(EncompassFieldConstant.UNDERWRITER, "");

                    Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();

                    string EncompassLoanGUID = loan.EnCompassLoanGUID == null ? string.Empty : loan.EnCompassLoanGUID.ToString();

                    foreach (string FieldKey in _fetchFields.Keys)
                    {
                        _fetchFields[FieldKey] = EncompassConnectorApp.QueryEncompass(EncompassLoanGUID, FieldKey, TenantSchema);
                    }
                    //_fetchFields = EncompassConnector.FieldLoanTypeLookup(EncompassFieldConstant.LOAN_NUMBER_LOOKUP, LoanNumber.Trim(), _fetchFields, FieldMatchTypes.Exact);

                    //if (string.IsNullOrEmpty(_fetchFields[EncompassFieldConstant.LOAN_TYPE]))
                    //{
                    //    if (loan != null)
                    //    {
                    //        CustomerMaster _custM = db.CustomerMaster.AsNoTracking().Where(l => l.CustomerID == loan.CustomerID).FirstOrDefault();

                    //        if (_custM != null)
                    //        {
                    //            List<Dictionary<string, string>> _fields = new List<Dictionary<string, string>>();

                    //            Dictionary<string, string> _dic = new Dictionary<string, string>();
                    //            _dic.Add("FieldID", EncompassFieldConstant.BORROWER_NAME);
                    //            _dic.Add("FieldValue", BorrowerName.Trim());
                    //            _fields.Add(_dic);
                    //            _dic = new Dictionary<string, string>();
                    //            _dic.Add("FieldID", EncompassFieldConstant.INVESTOR_NAME);
                    //            _dic.Add("FieldValue", _custM.CustomerName.Trim());
                    //            _fields.Add(_dic);

                    //            _fetchFields = EncompassConnector.FieldLoanTypeLookup(_fields, _fetchFields);
                    //        }
                    //    }
                    //}

                    LoanTypeMaster ltype = db.LoanTypeMaster.AsNoTracking().Where(l => l.LoanTypeName == _fetchFields[EncompassFieldConstant.LOAN_TYPE]).FirstOrDefault();

                    if (loan != null && ltype != null && ltype.LoanTypeID != 0)
                    {
                        LoanLOSFields _idcfield = db.LoanLOSFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                        _idcfield.LoanOfficer = _fetchFields[EncompassFieldConstant.LOAN_OFFICER];
                        _idcfield.Closer = _fetchFields[EncompassFieldConstant.LOAN_CLOSER];
                        _idcfield.PostCloser = _fetchFields[EncompassFieldConstant.POST_CLOSER];
                        _idcfield.Underwriter = _fetchFields[EncompassFieldConstant.UNDERWRITER];

                        _idcfield.EmailLoanOfficer = _fetchFields[EncompassFieldConstant.EMAIL_LOAN_OFFICER];
                        _idcfield.EmailCloser = _fetchFields[EncompassFieldConstant.EMAIL_LOAN_CLOSER];
                        _idcfield.EmailPostCloser = _fetchFields[EncompassFieldConstant.EMAIL_POST_CLOSER];
                        _idcfield.EmailUnderwriter = _fetchFields[EncompassFieldConstant.EMAIL_UNDERWRITER];

                        //IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                        //if (_idcfield != null)
                        //{
                        //    _idcfield.LoanOfficer = _fetchFields[EncompassFieldConstant.LOAN_OFFICER];
                        //    _idcfield.Closer = _fetchFields[EncompassFieldConstant.LOAN_CLOSER];
                        //    _idcfield.PostCloser = _fetchFields[EncompassFieldConstant.POST_CLOSER];
                        //    _idcfield.UnderWritter = _fetchFields[EncompassFieldConstant.UNDERWRITER];
                        //    _idcfield.ModifiedOn = DateTime.Now;
                        //    db.Entry(_idcfield).State = EntityState.Modified;
                        //    db.SaveChanges();
                        //}
                        //else
                        //{
                        //    _idcfield = new IDCFields()
                        //    {
                        //        LoanOfficer = _fetchFields[EncompassFieldConstant.LOAN_OFFICER],
                        //        Closer = _fetchFields[EncompassFieldConstant.LOAN_CLOSER],
                        //        PostCloser = _fetchFields[EncompassFieldConstant.POST_CLOSER],
                        //        UnderWritter = _fetchFields[EncompassFieldConstant.UNDERWRITER],
                        //        Createdon = DateTime.Now,
                        //        ModifiedOn = DateTime.Now
                        //    };
                        //    db.IDCFields.Add(_idcfield);
                        //    db.SaveChanges();
                        //}

                        //LoanAudit.InsertLoanIDCFieldAudit(db, _idcfield, "Updated LOS Fields", "");
                        db.SaveChanges();
                        loan.LoanTypeID = ltype.LoanTypeID;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPDATED_LOAN_TYPE_FROM_LOS);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);
                        tran.Commit();
                    }
                    else
                    {
                        throw new Exception("Loan Type " + _fetchFields[EncompassFieldConstant.LOAN_TYPE] + " not exists in IntellaLend");
                    }
                }
            }
            return true;

        }

        public Object GetLoanDetailsForEphesoft(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {

                    BoxDownloadQueue _loanBox = db.BoxDownloadQueue.AsNoTracking().Where(b => b.LoanID == LoanID).FirstOrDefault();

                    if (_loanBox != null)
                    {
                        var item = (from l in db.Loan.AsNoTracking()
                                    join RTM in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals RTM.ReviewTypeID
                                    join LTM in db.LoanTypeMaster.AsNoTracking() on l.LoanTypeID equals LTM.LoanTypeID
                                    join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                                    where l.LoanID == LoanID
                                    select new
                                    {
                                        LoanID = l.LoanID,
                                        Customer = cus.CustomerName,
                                        ServiceType = RTM.ReviewTypeName,
                                        AuditMonthYear = l.AuditMonthYear,
                                        LoanType = LTM.LoanTypeName,
                                        LoanNumber = l.LoanNumber,
                                        Priority = l.Priority, //RTM.ReviewTypePriority == null ? 0 : RTM.ReviewTypePriority
                                    }).FirstOrDefault();

                        LoanSearch _lSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                        string AuditDueDate = string.Empty;
                        if (_lSearch != null)
                        {
                            AuditDueDate = _lSearch.AuditDueDate != null ? _lSearch.AuditDueDate.Value.ToString(DateConstance.ShortDateFormart) : "";
                        }

                        var auditMonth = item.AuditMonthYear != null ? item.AuditMonthYear.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.AuditMonthYear.Year.ToString() : "";
                        return new
                        {
                            LoanID = item.LoanID,
                            Customer = item.Customer,
                            ServiceType = item.ServiceType,
                            AuditMonthYear = auditMonth,
                            LoanType = item.LoanType,
                            LoanNumber = item.LoanNumber,
                            Priority = item.Priority,
                            AuditDueDate = AuditDueDate
                        };

                    }
                    else
                    {
                        var item = (from l in db.Loan.AsNoTracking()
                                    join RTM in db.ReviewTypeMaster.AsNoTracking() on l.ReviewTypeID equals RTM.ReviewTypeID
                                    join LTM in db.LoanTypeMaster.AsNoTracking() on l.LoanTypeID equals LTM.LoanTypeID
                                    join cus in db.CustomerMaster.AsNoTracking() on l.CustomerID equals cus.CustomerID
                                    where l.LoanID == LoanID
                                    select new
                                    {
                                        LoanID = l.LoanID,
                                        Customer = cus.CustomerName,
                                        ServiceType = RTM.ReviewTypeName,
                                        AuditMonthYear = l.AuditMonthYear,
                                        LoanType = LTM.LoanTypeName,
                                        LoanNumber = l.LoanNumber,
                                        Priority = l.Priority //RTM.ReviewTypePriority
                                    }).FirstOrDefault();

                        LoanSearch _lSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                        string AuditDueDate = string.Empty;
                        if (_lSearch != null)
                        {
                            AuditDueDate = _lSearch.AuditDueDate != null ? _lSearch.AuditDueDate.Value.ToString(DateConstance.ShortDateFormart) : "";
                        }

                        var auditMonth = item.AuditMonthYear != null ? item.AuditMonthYear.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.AuditMonthYear.Year.ToString() : "";
                        return new
                        {
                            LoanID = item.LoanID,
                            Customer = item.Customer,
                            ServiceType = item.ServiceType,
                            AuditMonthYear = auditMonth,
                            Priority = item.Priority,
                            LoanType = item.LoanType,
                            LoanNumber = item.LoanNumber,
                            AuditDueDate = AuditDueDate
                        };
                    }
                }
            }
        }

        public string GetDocumentStackingOrder(Int64 loanID, Int64 configId)
        {
            string result = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                if (_loan != null)
                {
                    Int64 StackingGrpId = 0;
                    StackingGrpId = db.CustReviewLoanStackMapping.AsNoTracking().Where(map => map.CustomerID == _loan.CustomerID && map.ReviewTypeID == _loan.ReviewTypeID && map.LoanTypeID == _loan.LoanTypeID).FirstOrDefault().StackingOrderID;
                    if (StackingGrpId != 0)
                    {
                        List<StackingOrderDetailMaster> _stackingOrder = db.StackingOrderDetailMaster.AsNoTracking().Where(s => s.StackingOrderID == StackingGrpId).OrderBy(s => s.SequenceID).ToList();
                        var resObj = (from sodm in _stackingOrder
                                      join dms in db.DocumentTypeMaster.AsNoTracking() on sodm.DocumentTypeID equals dms.DocumentTypeID
                                      where dms.Active == true
                                      select new
                                      {
                                          ConfigID = configId,
                                          SequenceNumber = sodm.SequenceID,
                                          DocumentName = dms.Name
                                      }).OrderBy(a => a.SequenceNumber).ToList();
                        result = JsonConvert.SerializeObject(resObj);
                    }
                }
            }
            return result;
        }

        public object CheckLoanPageCount(Int64 loanID, Int64 pageCount)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();

                if (_loan != null)
                {
                    return new { Equals = (_loan.PageCount == pageCount) };
                }
            }

            return new { Equals = false };
        }

        public bool UpdateEphesoftBatchDetail(Int64 LoanID, string batchid, Int64 DocID, string batchclassid, string batchclassname)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    if (DocID == 0)
                    {
                        string _reviewerName = string.Empty;
                        Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                        if (loan != null)
                        {
                            IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                            string sql = "select review_operator_user_name from batch_instance where identifier='" + batchid + "'";
                            System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);

                            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
                            {
                                _reviewerName = DBNull.Value.Equals(dt.Rows[0][0]) ? string.Empty : dt.Rows[0][0].ToString();
                            }
                            if (_idcfield != null)
                            {
                                _idcfield.IDCReviewerName = _reviewerName;
                                _idcfield.IDCValidatorName = string.Empty;
                                _idcfield.IDCBatchInstanceID = batchid;
                                _idcfield.IDCReviewerName = _reviewerName;
                                _idcfield.IDCBatchClassID = batchclassid;
                                _idcfield.IDCBatchClassName = batchclassname;
                                _idcfield.ModifiedOn = DateTime.Now;
                                db.Entry(_idcfield).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                _idcfield = new IDCFields()
                                {
                                    LoanID = loan.LoanID,
                                    IDCReviewerName = _reviewerName,
                                    IDCValidatorName = string.Empty,
                                    IDCBatchInstanceID = batchid,
                                    OCRAccuracyCalculated = false,
                                    IDCBatchClassID = batchclassid,
                                    IDCBatchClassName = batchclassname,
                                    Createdon = DateTime.Now
                                };
                                db.IDCFields.Add(_idcfield);
                                db.SaveChanges();
                            }

                            LoanAudit.InsertLoanIDCFieldAudit(db, _idcfield, "Updated EphesoftBatchInstanceID & Ephesoft Reviewer Name", "");

                            //string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPDATED_LOAN_TYPE_FROM_QCIQ);
                            //LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], "Updated EphesoftBatchInstanceID & Ephesoft Reviewer Name");
                            tran.Commit();
                        }
                    }
                    else
                    {
                        string fileName = $"{TenantSchema.ToUpper()}_{LoanID.ToString()}_{DocID.ToString()}";

                        AuditLoanMissingDoc _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == LoanID && l.DocID == DocID).FirstOrDefault();

                        if (_loan == null)
                            _loan = db.AuditLoanMissingDoc.AsNoTracking().Where(l => l.LoanID == LoanID && l.FileName.Contains(fileName)).FirstOrDefault();

                        if (_loan != null)
                        {
                            // _loan.Status = _status == 7 ? StatusConstant.IDCERROR : StatusConstant.MOVED_TO_IDC;
                            _loan.IDCBatchInstanceID = batchid;
                            _loan.ModifiedOn = DateTime.Now;
                            db.Entry(_loan).State = EntityState.Modified;
                            db.SaveChanges();
                            //string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                            //LoanAudit.InsertLoanAudit(db, _loan, auditDescs[0].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc), auditDescs[1].Replace(AuditConfigConstant.LOANSTATUSDESC, loanStatusDesc));
                            tran.Commit();
                        }
                    }
                }
            }
            return true;
        }

        public bool UpdateEphesoftReviewedDate(Int64 LoanID, string batchid)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    string _lastModified = string.Empty;
                    string _duration = string.Empty;
                    string _reviewerName = string.Empty;
                    Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                    if (loan != null)
                    {
                        IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                        string sql = $"select duration, end_time, start_time, user_name from hist_manual_steps_in_workflow where batch_instance_id = '{batchid}' and batch_instance_status = 'READY_FOR_REVIEW'";
                        System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            _duration = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                if (DBNull.Value != dr["end_time"] && DBNull.Value != dr["start_time"] && dr["start_time"].ToString() != "" && dr["end_time"].ToString() != "")
                                {
                                    _lastModified = dr["end_time"].ToString();
                                    string _dbDuration = dr["duration"].ToString();
                                    DateTime start_time_ = Convert.ToDateTime(dr["start_time"].ToString());
                                    DateTime end_time_ = Convert.ToDateTime(_lastModified);
                                    TimeSpan time = TimeSpan.FromMilliseconds(Convert.ToInt64(_dbDuration));
                                    _duration += $"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")} | ";
                                    _reviewerName += DBNull.Value != dr["user_name"] ? dr["user_name"].ToString() + " | " : string.Empty;
                                }
                            }
                        }
                        if (_idcfield != null)
                        {
                            _idcfield.IDCReviewerName = _reviewerName;
                            _idcfield.IDCLevelOneCompletionDate = null;
                            if (_lastModified != null && !string.IsNullOrEmpty(_lastModified.Trim()))
                                _idcfield.IDCLevelOneCompletionDate = Convert.ToDateTime(_lastModified);

                            _idcfield.IDCLevelOneDuration = _duration;
                            _idcfield.ModifiedOn = DateTime.Now;
                            db.Entry(_idcfield).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            IDCFields fields = new IDCFields
                            {
                                LoanID = loan.LoanID,
                                IDCReviewerName = _reviewerName,
                                IDCLevelOneDuration = _duration,
                                OCRAccuracyCalculated = false,
                                Createdon = DateTime.Now
                            };

                            fields.IDCLevelOneCompletionDate = null;
                            if (_lastModified != null && !string.IsNullOrEmpty(_lastModified.Trim()))
                                fields.IDCLevelOneCompletionDate = Convert.ToDateTime(_lastModified);

                            db.IDCFields.Add(fields);
                            db.SaveChanges();
                        }
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MANUAL_QUESTIONER_UPDATED);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], "Updated Review Completion Date");
                        tran.Commit();
                    }


                }
            }
            return true;
        }

        public bool UpdateEphesoftValidatorDate(Int64 LoanID, string batchid)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    string _lastModified = string.Empty;
                    string _duration = string.Empty;
                    string _validatorName = string.Empty;
                    Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                    if (loan != null)
                    {
                        IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                        string sql = $"select duration, end_time, start_time, user_name from hist_manual_steps_in_workflow where batch_instance_id = '{batchid}' and batch_instance_status = 'READY_FOR_VALIDATION'";
                        System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            _duration = string.Empty;
                            _validatorName = string.Empty;
                            foreach (System.Data.DataRow dr in dt.Rows)
                            {
                                if (DBNull.Value != dr["end_time"] && DBNull.Value != dr["start_time"] && dr["start_time"].ToString() != "" && dr["end_time"].ToString() != "")
                                {
                                    string _dbDuration = dr["duration"].ToString();
                                    _lastModified = dr["end_time"].ToString();
                                    DateTime start_time_ = Convert.ToDateTime(dr["start_time"].ToString());
                                    DateTime end_time_ = Convert.ToDateTime(_lastModified);
                                    //TimeSpan time = TimeSpan.FromMilliseconds(end_time_.Subtract(start_time_).TotalMilliseconds);
                                    TimeSpan time = TimeSpan.FromMilliseconds(Convert.ToInt64(_dbDuration));
                                    _duration += $"{time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")} | ";
                                    _validatorName += DBNull.Value != dr["user_name"] ? dr["user_name"].ToString() + " | " : string.Empty;
                                }
                            }
                        }
                        if (_idcfield != null)
                        {
                            _idcfield.IDCLevelTwoCompletionDate = null;
                            if (_lastModified != null && !string.IsNullOrEmpty(_lastModified.Trim()))
                                _idcfield.IDCLevelTwoCompletionDate = Convert.ToDateTime(_lastModified);

                            _idcfield.IDCLevelTwoDuration = _duration;
                            _idcfield.IDCValidatorName = _validatorName;
                            _idcfield.ModifiedOn = DateTime.Now;
                            db.Entry(_idcfield).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            IDCFields fields = new IDCFields
                            {
                                LoanID = loan.LoanID,
                                IDCLevelTwoDuration = _duration,
                                IDCValidatorName = _validatorName,
                                OCRAccuracyCalculated = false,
                                Createdon = DateTime.Now
                            };
                            fields.IDCLevelTwoCompletionDate = null;
                            if (_lastModified != null && !string.IsNullOrEmpty(_lastModified.Trim()))
                                fields.IDCLevelTwoCompletionDate = Convert.ToDateTime(_lastModified);

                            db.IDCFields.Add(fields);
                            db.SaveChanges();
                        }
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MANUAL_QUESTIONER_UPDATED);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], "Updated Validation Completion Date");
                        tran.Commit();
                    }
                }
            }
            return true;
        }

        public bool UpdateEphesoftValidatorName(Int64 LoanID, string batchid)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    string _validatorName = string.Empty;
                    Loan loan = db.Loan.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                    if (loan != null)
                    {
                        IDCFields _idcfield = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                        string sql = "select validation_operator_user_name from batch_instance where identifier='" + batchid + "'";
                        System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
                        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
                        {
                            _validatorName = DBNull.Value.Equals(dt.Rows[0][0]) ? string.Empty : dt.Rows[0][0].ToString();
                        }
                        if (_idcfield != null)
                        {
                            _idcfield.IDCValidatorName = _validatorName;
                            _idcfield.ModifiedOn = DateTime.Now;
                            db.Entry(_idcfield).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.IDCFields.Add(new IDCFields
                            {
                                LoanID = loan.LoanID,
                                IDCValidatorName = _validatorName,
                                OCRAccuracyCalculated = false,
                                Createdon = DateTime.Now
                            });
                            db.SaveChanges();
                        }
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MANUAL_QUESTIONER_UPDATED);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], "Updated Ephesoft Validator Name Updated");
                        tran.Commit();
                    }
                }
            }
            return true;
        }



        public bool UpdateLoanQuestioner(Int64 LoanID, List<ManualQuestioner> questioner, Int64 CurrentUserID)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    LoanDetail loanDetail = db.LoanDetail.Where(l => l.LoanID == LoanID).AsNoTracking().FirstOrDefault();
                    LoanEvaluatedResult loanEvaluatedResult = db.LoanEvaluatedResult.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                    var EvaluatedResult = JsonConvert.DeserializeObject<CheckListResult>(loanEvaluatedResult.EvaluatedResult);
                    EvaluatedResult.loanQuestioner = questioner;

                    if (loanDetail != null)
                    {
                        loanDetail.ManualQuestioners = JsonConvert.SerializeObject(questioner);
                        loanDetail.ModifiedOn = DateTime.Now;
                        db.Entry(loanDetail).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.MANUAL_QUESTIONER_UPDATED);
                        LoanAudit.InsertLoanDetailsAudit(db, loanDetail, CurrentUserID, auditDescs[0], auditDescs[1]);
                        loanEvaluatedResult.EvaluatedResult = JsonConvert.SerializeObject(EvaluatedResult);
                        loanEvaluatedResult.ModifiedOn = DateTime.Now;
                        db.Entry(loanEvaluatedResult).State = EntityState.Modified;
                        db.SaveChanges();
                        tran.Commit();
                    }
                }
            }
            return true;
        }

        public void UpdateEvalResult(Int64 LoanID, string _evalResult)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanEvaluatedResult evalResult = db.LoanEvaluatedResult.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (evalResult != null)
                {
                    evalResult.EvaluatedResult = _evalResult;
                    evalResult.ModifiedOn = DateTime.Now;
                    db.Entry(evalResult).State = EntityState.Modified;
                }
                else
                {
                    db.LoanEvaluatedResult.Add(new LoanEvaluatedResult()
                    {
                        LoanID = LoanID,
                        EvaluatedResult = _evalResult,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }

                db.SaveChanges();
            }
        }
        public string GetEvaluatedResult(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanEvaluatedResult evalResult = db.LoanEvaluatedResult.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (evalResult != null)
                {
                    return evalResult.EvaluatedResult;
                }
            }

            return null;
        }
        public string GetEncompassFieldID(Int64 EnCompassID)
        {
            using (var db = new DBConnect(SystemSchema))
            {
                EncompassFields en = db.EncompassFields.AsNoTracking().Where(x => x.ID == EnCompassID).FirstOrDefault();

                if (en != null)
                {
                    return en.FieldID;
                }

                return string.Empty;
            }
        }


        public List<ManualQuestioner> GetLoanQuestioner(Int64 LoanID, Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            List<ManualQuestioner> questions = new List<ManualQuestioner>();

            using (var db = new DBConnect(TenantSchema))
            {
                var loanDetails = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                if (loanDetails != null && !string.IsNullOrEmpty(loanDetails.ManualQuestioners))
                {
                    try
                    {
                        questions = JsonConvert.DeserializeObject<List<ManualQuestioner>>(loanDetails.ManualQuestioners);
                        foreach (ManualQuestioner item in questions)
                        {
                            CheckListDetailMaster _checkDetail = db.CheckListDetailMaster.AsNoTracking().Where(c => c.CheckListDetailID == item.CheckListDetailID).FirstOrDefault();
                            if (_checkDetail != null)
                                item.SequenceID = _checkDetail.SequenceID;
                        }
                        return questions;
                    }
                    catch
                    { }
                }

                var obj = (from map in db.CustReviewLoanCheckMapping.AsNoTracking()
                           join cdm in db.CheckListDetailMaster.AsNoTracking() on map.CheckListID equals cdm.CheckListID
                           join rm in db.RuleMaster.AsNoTracking() on cdm.CheckListDetailID equals rm.CheckListDetailID
                           where map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID && cdm.Active == true && (cdm.Rule_Type == null ? 0 : cdm.Rule_Type) == 1
                           select new
                           {
                               RuleID = rm.RuleID,
                               Category = cdm.Category,
                               CheckListDetailID = cdm.CheckListDetailID,
                               CheckListName = cdm.Name,
                               Question = rm.RuleDescription,
                               OptionJson = rm.RuleJson,
                               SequenceID = cdm.SequenceID,
                               LOSFieldToEvalRule = cdm.LOSFieldToEvalRule,
                               LosIsMatched = cdm.LosIsMatched,
                               LOSValueToEvalRule = cdm.LOSValueToEvalRule
                           }).ToList();

                if (obj != null)
                {
                    Loan loan = db.Loan.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    foreach (var item in obj)
                    {
                        string enCompassGUID = loan.EnCompassLoanGUID == null ? string.Empty : loan.EnCompassLoanGUID.ToString();

                        string enFieldID = GetEncompassFieldID(item.LOSFieldToEvalRule);

                        if (!string.IsNullOrEmpty(enFieldID) && !string.IsNullOrEmpty(enCompassGUID))
                        {
                            string enFieldVal = EncompassConnectorApp.QueryEncompass(enCompassGUID, enFieldID, TenantSchema);

                            if (item.LosIsMatched == 1)
                            {
                                if (enFieldVal != null && enFieldVal != "null" && !string.IsNullOrEmpty(item.LOSValueToEvalRule) && item.LOSValueToEvalRule.Split('|').Any(v => v.Trim().ToLower().Equals(enFieldVal.Trim().ToLower())))
                                    questions.Add(new ManualQuestioner()
                                    {
                                        RuleID = item.RuleID,
                                        Category = item.Category,
                                        CheckListDetailID = item.CheckListDetailID,
                                        CheckListName = item.CheckListName,
                                        Question = item.Question,
                                        OptionJson = item.OptionJson,
                                        SequenceID = item.SequenceID,
                                        AnswerJson = "{\"Ansewer\":[], \"Notes\": \"\"}"
                                    });
                            }
                            else if (item.LosIsMatched == 2)
                            {
                                if (enFieldVal != null && enFieldVal != "null" && !string.IsNullOrEmpty(item.LOSValueToEvalRule) && !item.LOSValueToEvalRule.Split('|').Any(v => v.Trim().ToLower().Equals(enFieldVal.Trim().ToLower())))
                                    questions.Add(new ManualQuestioner()
                                    {
                                        RuleID = item.RuleID,
                                        Category = item.Category,
                                        CheckListDetailID = item.CheckListDetailID,
                                        CheckListName = item.CheckListName,
                                        Question = item.Question,
                                        OptionJson = item.OptionJson,
                                        SequenceID = item.SequenceID,
                                        AnswerJson = "{\"Ansewer\":[], \"Notes\": \"\"}"
                                    });
                            }
                        }
                        else
                        {
                            questions.Add(new ManualQuestioner()
                            {
                                RuleID = item.RuleID,
                                Category = item.Category,
                                CheckListDetailID = item.CheckListDetailID,
                                CheckListName = item.CheckListName,
                                Question = item.Question,
                                OptionJson = item.OptionJson,
                                SequenceID = item.SequenceID,
                                AnswerJson = "{\"Ansewer\":[], \"Notes\": \"\"}"
                            });
                        }

                    }

                    return questions;
                }

            }

            return questions;
        }

        public void UpdateDocumentCounts(Int64 LoanID, Int64 UserID, Int64 TotalDocCount, Int64 MissingDocCount, Int64 MissingCriticalDocCount, Int64 MissingNonCriticalDocCount)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (_loanDetail != null)
                    {
                        _loanDetail.TotalDocCount = TotalDocCount;
                        _loanDetail.MissingDocCount = MissingDocCount;
                        _loanDetail.MissingCriticalDocCount = MissingCriticalDocCount;
                        _loanDetail.MissingNonCriticalDocCount = MissingNonCriticalDocCount;

                        db.Entry(_loanDetail).State = EntityState.Modified;
                        db.SaveChanges();

                        //string auditDesc = "Documents Count Updated";
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);

                        LoanAudit.InsertLoanDetailsAudit(db, _loanDetail, UserID, auditDescs[0], auditDescs[1]);
                    }

                    tran.Commit();
                }
            }

        }

        public System.Data.DataSet GetQCIQData(string connectIonString, string sqlScript)
        {
            return DynamicDataAccess.ExecuteSQLDataSet(connectIonString, sqlScript); ;
        }

        #endregion

        #region Private Methods

        private string GetDateString(DateTime AuditMonthYear)
        {
            return AuditMonthYear != null ? AuditMonthYear.ToString("MMMM", CultureInfo.InvariantCulture) + " " + AuditMonthYear.Year.ToString() : "";
        }

        private List<LoanImage> GetLoanImages(Int64 loanId, Int64 documentTypeId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var imageList = db.LoanImage.AsNoTracking().Where(m => (m.LoanID == loanId) && (m.DocumentTypeID == documentTypeId)).OrderBy(i => i.PageNo).ToList();
                var maxItem = imageList.OrderByDescending(x => Convert.ToInt32(x.Version)).FirstOrDefault();
                return imageList.Where(x => x.Version == maxItem.Version).OrderBy(i => i.PageNo).ToList();
            }
        }

        private List<LoanImage> GetLoanImages(Int64 loanId, Int64 documentTypeId, string VersionNumber)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var imageList = db.LoanImage.AsNoTracking().Where(m => (m.LoanID == loanId) && (m.DocumentTypeID == documentTypeId) && (m.Version == VersionNumber)).OrderBy(i => i.PageNo).ToList();
                return imageList;
            }
        }

        private void UpdateLoanImages(DBConnect db, Int64 LoanID, Int64 OldDocumentID, Int64 NewDocumentID, string NewVersionNumber, string OldVersionNumber)
        {
            List<LoanImage> lsLoanImages = db.LoanImage.AsNoTracking().Where(i => i.LoanID == LoanID && i.DocumentTypeID == OldDocumentID && i.Version == OldVersionNumber).ToList();

            foreach (LoanImage loanImage in lsLoanImages)
            {
                loanImage.DocumentTypeID = NewDocumentID;
                loanImage.Version = NewVersionNumber;
                db.Entry(loanImage).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        private string GetFieldDisplayName(Int64 FieldID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                DocumentFieldMaster docField = db.DocumentFieldMaster.AsNoTracking().Where(f => f.FieldID == FieldID).FirstOrDefault();

                if (docField != null)
                    return docField.DisplayName;
            }

            return string.Empty;
        }

        #endregion
        #region Email Tracker
        public object GetEmailTrackerDetails(DateTime FromDate, DateTime ToDate)
        {
            object data = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from Et in db.EmailTracker.AsNoTracking()
                        join L in db.Loan.AsNoTracking() on Et.LoanID equals L.LoanID
                        where Et.CreatedOn >= FromDate && Et.CreatedOn <= ToDate
                        select new
                        {
                            ID = Et.ID,
                            To = Et.To,
                            SendBy = Et.SendBy,
                            Body = Et.Body,
                            Attachments = Et.Attachments,
                            Delivered = Et.Delivered,
                            ErrorMessage = Et.ErrorMessage,
                            LoanID = Et.LoanID,
                            Subject = Et.Subject,
                            Parameters = Et.Parameters,
                            UserID = Et.UserID,
                            CreatedOn = Et.CreatedOn,
                            ModifiedOn = Et.ModifiedOn,
                            TemplateID = Et.TemplateID,
                            LoanNumber = L.LoanNumber,
                            AttachmentsName = Et.AttachmentsName
                        }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return data;

        }
        public object GetEmailTracker()
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from Et in db.EmailTracker.AsNoTracking()
                        join L in db.Loan.AsNoTracking() on Et.LoanID equals L.LoanID
                        join U in db.Users.AsNoTracking() on Et.UserID equals U.UserID
                        select new
                        {
                            ID = Et.ID,
                            To = string.IsNullOrEmpty(U.Email) ? Et.To : U.Email,
                            SendBy = Et.SendBy,
                            Body = Et.Body,
                            Attachments = Et.Attachments,
                            Delivered = Et.Delivered,
                            ErrorMessage = Et.ErrorMessage,
                            LoanID = Et.LoanID,
                            Subject = Et.Subject,
                            Parameters = Et.Parameters,
                            UserID = Et.UserID,
                            CreatedOn = Et.CreatedOn,
                            ModifiedOn = Et.ModifiedOn,
                            TemplateID = Et.TemplateID,
                            LoanNumber = L.LoanNumber,
                            AttachmentsName = Et.AttachmentsName
                        }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return data;

        }
        public object GetCurrentData(Int64 Id)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = db.EmailTracker.AsNoTracking().Where(x => x.ID == Id).FirstOrDefault();
            }
            return data;
        }

        public string GetEncompassDocPages(Int64 LoanID, Int64 DocID, ref bool isEncompassLoan)
        {
            string EncompassDocPages = string.Empty;

            using (var db = new DBConnect(TenantSchema))
            {
                if (DocID == 0)
                {
                    LoanLOSFields data = db.LoanLOSFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                    if (data != null)
                    {
                        isEncompassLoan = true;
                        EncompassDocPages = data.EncompassDocPages;
                    }
                    else
                        isEncompassLoan = false;
                }
                else
                {
                    Loan loan = db.Loan.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                    if (loan != null && loan.UploadType == UploadConstant.ENCOMPASS)
                    {
                        AuditLoanMissingDoc data = db.AuditLoanMissingDoc.AsNoTracking().Where(x => x.LoanID == LoanID && x.DocID == DocID).FirstOrDefault();
                        if (data != null)
                        {
                            isEncompassLoan = true;

                            DocumentTypeMaster dm = db.DocumentTypeMaster.AsNoTracking().Where(x => x.DocumentTypeID == DocID).FirstOrDefault();

                            if (dm != null)
                            {
                                List<EnDocumentType> _enDocTypes = new List<EnDocumentType>();
                                _enDocTypes.Add(new EnDocumentType()
                                {
                                    DocumentTypeName = dm.Name,
                                    Pages = new List<int> { -999 }
                                });

                                EncompassDocPages = JsonConvert.SerializeObject(_enDocTypes);
                            }
                        }
                        else
                            isEncompassLoan = false;
                    }
                    else
                        isEncompassLoan = false;
                }
            }

            return EncompassDocPages;
        }


        public object GetDataByLoanId(Int64 LoanID)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from Et in db.EmailTracker.AsNoTracking()
                        join L in db.Loan.AsNoTracking() on Et.LoanID equals L.LoanID
                        where Et.LoanID == LoanID
                        select new
                        {
                            ID = Et.ID,
                            To = Et.To,
                            SendBy = Et.SendBy,
                            Body = Et.Body,
                            Attachments = Et.Attachments,
                            Delivered = Et.Delivered,
                            ErrorMessage = Et.ErrorMessage,
                            LoanID = Et.LoanID,
                            Subject = Et.Subject,
                            Parameters = Et.Parameters,
                            UserID = Et.UserID,
                            CreatedOn = Et.CreatedOn,
                            ModifiedOn = Et.ModifiedOn,
                            TemplateID = Et.TemplateID,
                            LoanNumber = L.LoanNumber,
                            AttachmentsName = Et.AttachmentsName
                        }).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return data;
        }

        public object SendEmailDetails(string To, string Subject, string Attachements, string Body, Int64 UserID, string SendBy, Int64 LoanID, string AttachmentsName)
        {
            object data = null;
            int IsDelivered = 0;
            using (var db = new DBConnect(TenantSchema))
            {
                db.EmailTracker.Add(new EmailTracker
                {
                    To = To,
                    Subject = Subject,
                    SendBy = SendBy,
                    UserID = UserID,
                    Attachments = Attachements,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    Delivered = IsDelivered,
                    Body = Body,
                    LoanID = LoanID,
                    TemplateID = 0,
                    AttachmentsName = AttachmentsName
                });
                db.SaveChanges();
            }
            return data = new { Success = true };
        }

        public bool ResendEmail(Int64 ID)
        {
            bool flag = false;
            using (var db = new DBConnect(TenantSchema))
            {
                EmailTracker tracker = db.EmailTracker.AsNoTracking().Where(t => t.ID == ID).FirstOrDefault();
                tracker.Delivered = 0;
                db.Entry(tracker).State = EntityState.Modified;
                db.SaveChanges();
                flag = true;
            }
            return flag;
        }
        #endregion

        #region Loan Export Monitor 

        public object SearchExportMonitorDetails(Int64 Status, Int64 CustomerId, DateTime FromDate, DateTime ToDate)
        {
            object data = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {
                if ((CustomerId != 0) && (Status != -1 && FromDate != null && ToDate != null))
                {

                    data = (from L in db.LoanJobExport.AsNoTracking()
                            join u in db.Users.AsNoTracking() on L.ExportedBy equals u.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            where (L.CustomerID == CustomerId && L.Status == Status && L.CreatedOn >= FromDate && L.ModifiedOn < ToDate)
                            select new
                            {
                                JobID = L.JobID,
                                JobName = L.JobName,
                                ExportedBy = string.IsNullOrEmpty(ul.FirstName) && string.IsNullOrEmpty(ul.LastName) ? string.Empty : ul.LastName + " " + ul.FirstName,
                                LoanCount = L.LoanCount,
                                ExportPath = L.ExportPath,
                                CoverLetter = L.CoverLetter,
                                TableOfContent = L.TableOfContent,
                                Password = L.Password,
                                CoverLetterContent = L.CoverLetterContent,
                                Status = L.Status,
                                ErrorMsg = L.ErrorMsg,
                                ErrorStackTrace = L.ErrorStackTrace,
                                CreatedOn = L.CreatedOn,
                                ModifiedOn = L.ModifiedOn,
                                PasswordProtected = L.PasswordProtected,
                                CustomerName = db.CustomerMaster.Where(x => x.CustomerID == L.CustomerID).FirstOrDefault().CustomerName,
                            }).OrderByDescending(t => t.JobID).ToList();


                }
                else if ((CustomerId != 0) && FromDate != null && ToDate != null)
                {
                    data = (from L in db.LoanJobExport.AsNoTracking()
                            join u in db.Users.AsNoTracking() on L.ExportedBy equals u.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            where (L.CustomerID == CustomerId && L.CreatedOn >= FromDate && L.ModifiedOn < ToDate)
                            select new
                            {
                                JobID = L.JobID,
                                JobName = L.JobName,
                                ExportedBy = string.IsNullOrEmpty(ul.FirstName) && string.IsNullOrEmpty(ul.LastName) ? string.Empty : ul.LastName + " " + ul.FirstName,
                                LoanCount = L.LoanCount,
                                ExportPath = L.ExportPath,
                                CoverLetter = L.CoverLetter,
                                TableOfContent = L.TableOfContent,
                                Password = L.Password,
                                CoverLetterContent = L.CoverLetterContent,
                                Status = L.Status,
                                ErrorMsg = L.ErrorMsg,
                                ErrorStackTrace = L.ErrorStackTrace,
                                CreatedOn = L.CreatedOn,
                                ModifiedOn = L.ModifiedOn,
                                PasswordProtected = L.PasswordProtected,
                                CustomerName = db.CustomerMaster.Where(x => x.CustomerID == L.CustomerID).FirstOrDefault().CustomerName,
                            }).OrderByDescending(t => t.JobID).ToList();



                }
                else if ((Status != -1) & FromDate != null && ToDate != null)
                {
                    data = (from L in db.LoanJobExport.AsNoTracking()
                            join u in db.Users.AsNoTracking() on L.ExportedBy equals u.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            where (L.Status == Status && L.CreatedOn >= FromDate && L.ModifiedOn < ToDate)
                            select new
                            {
                                JobID = L.JobID,
                                JobName = L.JobName,
                                ExportedBy = string.IsNullOrEmpty(ul.FirstName) && string.IsNullOrEmpty(ul.LastName) ? string.Empty : ul.LastName + " " + ul.FirstName,
                                LoanCount = L.LoanCount,
                                ExportPath = L.ExportPath,
                                CoverLetter = L.CoverLetter,
                                TableOfContent = L.TableOfContent,
                                Password = L.Password,
                                CoverLetterContent = L.CoverLetterContent,
                                Status = L.Status,
                                ErrorMsg = L.ErrorMsg,
                                ErrorStackTrace = L.ErrorStackTrace,
                                CreatedOn = L.CreatedOn,
                                ModifiedOn = L.ModifiedOn,
                                PasswordProtected = L.PasswordProtected,
                                CustomerName = db.CustomerMaster.Where(x => x.CustomerID == L.CustomerID).FirstOrDefault().CustomerName,
                            }).OrderByDescending(o => o.JobID).ToList();



                }
                else
                {

                    data = (from L in db.LoanJobExport.AsNoTracking()
                            join u in db.Users.AsNoTracking() on L.ExportedBy equals u.UserID into lu
                            from ul in lu.DefaultIfEmpty()
                            where (L.CreatedOn >= FromDate && L.ModifiedOn < ToDate)
                            select new
                            {
                                JobID = L.JobID,
                                JobName = L.JobName,
                                ExportedBy = string.IsNullOrEmpty(ul.FirstName) && string.IsNullOrEmpty(ul.LastName) ? string.Empty : ul.LastName + " " + ul.FirstName,
                                LoanCount = L.LoanCount,
                                ExportPath = L.ExportPath,
                                CoverLetter = L.CoverLetter,
                                TableOfContent = L.TableOfContent,
                                Password = L.Password,
                                CoverLetterContent = L.CoverLetterContent,
                                Status = L.Status,
                                ErrorMsg = L.ErrorMsg,
                                ErrorStackTrace = L.ErrorStackTrace,
                                CreatedOn = L.CreatedOn,
                                ModifiedOn = L.ModifiedOn,
                                PasswordProtected = L.PasswordProtected,
                                CustomerName = db.CustomerMaster.Where(x => x.CustomerID == L.CustomerID).FirstOrDefault().CustomerName,
                            }).OrderByDescending(o => o.JobID).ToList();


                }




                // if (CustomerId == 0 && Status == -1 && FromDate.Year == DateTime.MinValue.Year && ToDate.Year == DateTime.MinValue.Year)
                //{
                //    data = db.LoanJobExport.AsNoTracking().ToList();
                //}
                //else
                //{
                //    data = (from L in db.LoanJobExport.AsNoTracking()

                //            where (
                //                   (CustomerId == 0 || L.CustomerID == CustomerId)
                //                    && (Status == -1 || L.Status == Status)
                //                    && ((FromDate.Year == DateTime.MinValue.Year) || (L.CreatedOn >= FromDate && L.CreatedOn < ToDate))
                //                   ) select L).ToList();

                // data = db.LoanJobExport.AsNoTracking().Where(x => x.Status == Status && (x.CreatedOn >= FromDate && x.CreatedOn <= ToDate)).ToList();
            }

            return data;
        }
        public object GetLoanExportMonitorDetails()
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from L in db.LoanJobExport.AsNoTracking()
                        join u in db.Users.AsNoTracking() on L.ExportedBy equals u.UserID into lu
                        from ul in lu.DefaultIfEmpty()

                        select new
                        {
                            JobID = L.JobID,
                            JobName = L.JobName,
                            ExportedBy = string.IsNullOrEmpty(ul.FirstName) && string.IsNullOrEmpty(ul.LastName) ? string.Empty : ul.LastName + " " + ul.FirstName,
                            LoanCount = L.LoanCount,
                            ExportPath = L.ExportPath,
                            CoverLetter = L.CoverLetter,
                            TableOfContent = L.TableOfContent,
                            Password = L.Password,
                            CoverLetterContent = L.CoverLetterContent,
                            Status = L.Status,
                            ErrorMsg = L.ErrorMsg,
                            ErrorStackTrace = L.ErrorStackTrace,
                            CreatedOn = L.CreatedOn,
                            ModifiedOn = L.ModifiedOn,
                            PasswordProtected = L.PasswordProtected,
                            CustomerName = db.CustomerMaster.Where(x => x.CustomerID == L.CustomerID).FirstOrDefault().CustomerName,
                        }).OrderByDescending(d => d.JobID).ToList();


            }
            return data;
        }


        public object SearchLoanExport(DateTime FromDate, DateTime ToDate, Int64 CurrentUserID, string LoanNumber, Int64 LoanType, string BorrowerName, string LoanAmount,
           Int64 ReviewStatus, DateTime? AuditMonthYear, Int64 ReviewType, Int64 Customer, string PropertyAddress, string InvestorLoanNumber)
        {
            List<LoanSearchReport> loans = new List<LoanSearchReport>();
            object loan;
            using (var db = new DBConnect(TenantSchema))
            {
                ToDate = ToDate.AddDays(1);
                DateTime? auditMonth = null;
                decimal loanAmount = 0;
                List<WorkFlowStatusMaster> wfMaster = new IntellaLendDataAccess().GetWorkFlowMaster();
                if (!string.IsNullOrEmpty(LoanAmount))
                    Decimal.TryParse(LoanAmount, out loanAmount);

                if (FromDate.Year == DateTime.MinValue.Year && ToDate.Year == DateTime.MinValue.Year && LoanNumber == "" && BorrowerName == "" &&
                    LoanAmount == "" && LoanType == 0 && ReviewStatus == 0 && AuditMonthYear == null && ReviewType == 0 && Customer == 0 && PropertyAddress == "" && InvestorLoanNumber == "")
                {
                    loans = (from L in db.Loan.AsNoTracking()
                             join search in db.LoanSearch.AsNoTracking() on L.LoanID equals search.LoanID
                             join LTM in db.LoanTypeMaster.AsNoTracking() on L.LoanTypeID equals LTM.LoanTypeID
                             join CUST in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CUST.CustomerID
                             join field in db.IDCFields.AsNoTracking() on L.LoanID equals field.LoanID into idcfield
                             from IDC in idcfield.DefaultIfEmpty()
                             where ((search.Status == StatusConstant.COMPLETE || search.Status == StatusConstant.PENDING_AUDIT || search.Status == StatusConstant.LOAN_EXPIRED)
                                 )
                             select new LoanSearchReport()
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = search.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 EphesoftBatchInstanceID = string.IsNullOrEmpty(IDC.IDCBatchInstanceID) ? string.Empty : IDC.IDCBatchInstanceID, //L.EphesoftBatchInstanceID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = search.Status,
                                 LoanAmount = search.LoanAmount,
                                 LoanTypeName = LTM.LoanTypeName,
                                 BorrowerName = search.BorrowerName,
                                 StatusDescription = "",
                                 LoggedUserID = L.LoggedUserID,
                                 ServiceTypeName = db.ReviewTypeMaster.Where(r => r.ReviewTypeID == L.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                                 AuditMonthYearDate = L.AuditMonthYear,
                                 AuditMonthYear = "",
                                 Customer = CUST.CustomerName,
                                 AssignedUserID = L.AssignedUserID > 0 ? L.AssignedUserID : 0,
                             }
                          ).OrderByDescending(c => c.ReceivedDate).Take(100).ToList();
                }
                else
                {
                    loans = (from L in db.Loan.AsNoTracking()
                             join search in db.LoanSearch.AsNoTracking() on L.LoanID equals search.LoanID
                             join LTM in db.LoanTypeMaster.AsNoTracking() on L.LoanTypeID equals LTM.LoanTypeID
                             join CUST in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CUST.CustomerID
                             join field in db.IDCFields.AsNoTracking() on L.LoanID equals field.LoanID into idcfield
                             from IDC in idcfield.DefaultIfEmpty()
                             where ((FromDate == DateTime.MinValue || (L.CreatedOn >= FromDate && L.CreatedOn < ToDate))
                                    && (ReviewStatus == 0 || L.Status == ReviewStatus)
                                    && (LoanType == 0 || L.LoanTypeID == LoanType)
                                    && (ReviewType == 0 || L.ReviewTypeID == ReviewType)
                                    && (Customer == 0 || L.CustomerID == Customer)
                                    && (AuditMonthYear == null || L.AuditMonthYear == AuditMonthYear)
                                    && (string.IsNullOrEmpty(LoanNumber) || search.LoanNumber.ToUpper() == LoanNumber.ToUpper())
                                    && (string.IsNullOrEmpty(BorrowerName) || search.BorrowerName.ToUpper() == BorrowerName.ToUpper())
                                    && (loanAmount == 0 || search.LoanAmount == loanAmount)
                                    && (string.IsNullOrEmpty(PropertyAddress) || search.PropertyAddress.ToUpper() == PropertyAddress.ToUpper())
                                    && (string.IsNullOrEmpty(InvestorLoanNumber) || search.InvestorLoanNumber.ToUpper() == InvestorLoanNumber.ToUpper())
                                    && (search.Status == StatusConstant.COMPLETE || search.Status == StatusConstant.PENDING_AUDIT || search.Status == StatusConstant.LOAN_EXPIRED)
                                    )
                             select new LoanSearchReport()
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = search.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 EphesoftBatchInstanceID = string.IsNullOrEmpty(IDC.IDCBatchInstanceID) ? string.Empty : IDC.IDCBatchInstanceID, //L.EphesoftBatchInstanceID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = search.Status,
                                 LoanAmount = search.LoanAmount,
                                 LoanTypeName = LTM.LoanTypeName,
                                 BorrowerName = search.BorrowerName,
                                 StatusDescription = "",
                                 LoggedUserID = L.LoggedUserID,
                                 ServiceTypeName = db.ReviewTypeMaster.Where(r => r.ReviewTypeID == L.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                                 AuditMonthYearDate = L.AuditMonthYear,
                                 AuditMonthYear = "",
                                 Customer = CUST.CustomerName,
                                 AssignedUserID = L.AssignedUserID > 0 ? L.AssignedUserID : 0,
                             }
                         ).ToList();
                }


                foreach (var item in loans.ToList())
                    item.AuditMonthYear = GetDateString(item.AuditMonthYearDate);



                loans = (from l in loans.AsEnumerable()
                         join wm in wfMaster on l.Status equals wm.StatusID
                         where l.Status != StatusConstant.PENDING_IDC && l.Status != StatusConstant.IDC_COMPLETE
                         select new LoanSearchReport()
                         {
                             LoanID = l.LoanID,
                             LoanNumber = l.LoanNumber,
                             LoanTypeID = l.LoanTypeID,
                             EphesoftBatchInstanceID = l.EphesoftBatchInstanceID,
                             ReceivedDate = l.ReceivedDate,
                             Status = l.Status,
                             LoanAmount = l.LoanAmount,
                             LoanTypeName = l.LoanTypeName,
                             BorrowerName = l.BorrowerName,
                             StatusDescription = wm.StatusDescription,
                             LoggedUserID = l.LoggedUserID,
                             ServiceTypeName = l.ServiceTypeName,
                             AuditMonthYear = l.AuditMonthYear,
                             AuditMonthYearDate = l.AuditMonthYearDate,
                             Customer = l.Customer,
                             AssignedUserID = l.AssignedUserID > 0 ? l.AssignedUserID : 0,
                         }).ToList();

                loan = (from l in loans.AsEnumerable()
                        join u in db.Users on l.LoggedUserID equals u.UserID into lu
                        from ul in lu.DefaultIfEmpty()
                        select new
                        {
                            LoanID = l.LoanID,
                            LoanNumber = l.LoanNumber,
                            LoanTypeID = l.LoanTypeID,
                            EphesoftBatchInstanceID = l.EphesoftBatchInstanceID,
                            ReceivedDate = l.ReceivedDate,
                            Status = l.Status,
                            LoanAmount = l.LoanAmount,
                            LoanTypeName = l.LoanTypeName,
                            BorrowerName = l.BorrowerName,
                            StatusDescription = l.StatusDescription,
                            LoggedUserID = l.LoggedUserID,
                            LoggerUserFirstName = ul?.FirstName ?? String.Empty,
                            LoggerUserLastName = ul?.LastName ?? String.Empty,
                            CurrentUserID = CurrentUserID,
                            ServiceTypeName = l.ServiceTypeName,
                            AuditMonthYear = l.AuditMonthYear,
                            Customer = l.Customer,
                            AssignedUserID = l.AssignedUserID > 0 ? l.AssignedUserID : 0,
                        }).ToList().OrderBy(c => c.Customer).ToList();
            }

            return loan;
        }


        public object GetCurrentJobDetail(Int64 JobID)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from LB in db.LoanJobExport.AsNoTracking()
                        join LDetail in db.LoanJobExportDetail.AsNoTracking() on LB.JobID equals LDetail.JobID
                        join L in db.Loan.AsNoTracking() on LDetail.LoanID equals L.LoanID
                        join u in db.Users.AsNoTracking() on LB.ExportedBy equals u.UserID into lu
                        from ul in lu.DefaultIfEmpty()
                        where LB.JobID == JobID
                        select new
                        {
                            JobID = LB.JobID,
                            JobName = LB.JobName,
                            ExportedBy = string.IsNullOrEmpty(ul.FirstName) && string.IsNullOrEmpty(ul.LastName) ? string.Empty : ul.LastName + " " + ul.FirstName,
                            LoanCount = LB.LoanCount,
                            BatchStatus = LB.Status,
                            BatchMessage = LB.ErrorMsg,
                            LoanID = LDetail.LoanID,
                            LoanStatus = LDetail.Status,
                            LoanMessage = LDetail.ErrorMsg,
                            ReviewTypeName = db.ReviewTypeMaster.Where(r => r.ReviewTypeID == L.ReviewTypeID).FirstOrDefault().ReviewTypeName,
                            CustomerName = db.CustomerMaster.Where(x => x.CustomerID == L.CustomerID).FirstOrDefault().CustomerName,
                            LoanTypeName = db.LoanTypeMaster.Where(x => x.LoanTypeID == L.LoanTypeID).FirstOrDefault().LoanTypeName,
                            LoanNumber = L.LoanNumber
                        }).ToList();
            }

            return data;
        }

        public object SaveLoanJob(string JobName, Int64 CustomerID, Boolean CoverLetter,
               Boolean TableOfContent, Boolean PasswordProtected, string Password, string CoverLetterContent, List<BatchLoanDetail> BatchLoanDoc, Int64 ExportedBy)
        {
            bool Success = false;

            using (var db = new DBConnect(TenantSchema))
            {
                Int64 Count = BatchLoanDoc.Count();
                if (CustomerID == 0)
                {
                    Int64 _loanID = BatchLoanDoc[0].LoanID;
                    CustomerID = db.Loan.AsNoTracking().Where(x => x.LoanID == _loanID).FirstOrDefault().CustomerID;
                }
                LoanJobExport _LoanJobExport = null;
                try
                {
                    _LoanJobExport = db.LoanJobExport.Add(new LoanJobExport
                    {
                        JobName = JobName,
                        ExportedBy = ExportedBy,
                        CustomerID = CustomerID,
                        LoanCount = Count,
                        CoverLetter = CoverLetter,
                        TableOfContent = TableOfContent,
                        PasswordProtected = PasswordProtected,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        ErrorMsg = string.Empty,
                        ErrorStackTrace = string.Empty,
                        ExportPath = string.Empty,
                        Status = StatusConstant.JOB_WAITING,
                        CoverLetterContent = CoverLetterContent,
                        Password = Password,
                    });
                    db.SaveChanges();
                    string jobID = _LoanJobExport.JobID.ToString("D5");
                    _LoanJobExport.JobName = jobID;
                    db.Entry(_LoanJobExport).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    _LoanJobExport = db.LoanJobExport.Add(new LoanJobExport
                    {
                        JobName = JobName,
                        LoanCount = BatchLoanDoc.Count(),
                        CoverLetter = CoverLetter,
                        ExportedBy = ExportedBy,
                        TableOfContent = TableOfContent,
                        PasswordProtected = PasswordProtected,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        ErrorMsg = ex.Message,
                        ErrorStackTrace = ex.StackTrace,
                        Status = StatusConstant.JOB_ERROR,
                        ExportPath = string.Empty,
                        CoverLetterContent = string.Empty,
                        Password = string.Empty
                    });
                    db.SaveChanges();
                    string jobID = _LoanJobExport.JobID.ToString("D5");
                    _LoanJobExport.JobName = jobID;
                    db.Entry(_LoanJobExport).State = EntityState.Modified;
                    db.SaveChanges();
                }

                foreach (var item in BatchLoanDoc)
                {
                    try
                    {
                        db.LoanJobExportDetail.Add(new LoanJobExportDetail
                        {

                            JobID = _LoanJobExport.JobID,
                            LoanID = item.LoanID,
                            LoanDocumentConfig = item.Documents,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            Status = ExportLoanStatusConstant.JOB_LOAN_WAITING,
                            FileName = string.Empty,
                            ErrorMsg = string.Empty,
                            FilePath = string.Empty,

                        });
                        db.SaveChanges();
                    }

                    catch (Exception ex)
                    {
                        db.LoanJobExportDetail.Add(new LoanJobExportDetail
                        {

                            JobID = _LoanJobExport.JobID,
                            LoanID = item.LoanID,
                            LoanDocumentConfig = item.Documents,
                            CreatedOn = DateTime.Now,
                            ModifiedOn = DateTime.Now,
                            ErrorMsg = ex.Message,
                            Status = ExportLoanStatusConstant.JOB_LOAN_WAITING,
                            FileName = string.Empty,
                            FilePath = string.Empty,
                        });
                        db.SaveChanges();
                    }

                }
                Success = true;
            }

            return Success;
        }

        public object DeleteJob(Int64 JobID)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                //List<LoanJobExportDetail> _loanbatchexportDetail = db.LoanJobExportDetail.Where(x => x.BatchID == BatchID).ToList();
                //foreach (LoanJobExportDetail exportdetail in _loanbatchexportDetail)
                //{
                //    db.Entry(exportdetail).State = EntityState.Deleted;
                //    db.SaveChanges();
                //}
                LoanJobExport _batchExport = db.LoanJobExport.Where(x => x.JobID == JobID).FirstOrDefault();
                _batchExport.Status = StatusConstant.JOB_DELETED;
                db.Entry(_batchExport).State = EntityState.Modified;
                db.SaveChanges();
            }

            return data = true;

        }
        #endregion


        public void AddReportConfigDetails(Int64 ReportID, Int64 LoanID, Int64 ReviewTypeID, bool status)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanReporting loan = db.LoanReporting.AsNoTracking().Where(lr => lr.ReportID == ReportID && lr.LoanID == LoanID).FirstOrDefault();
                if (loan == null)
                {
                    db.LoanReporting.Add(new LoanReporting()
                    {
                        ReportID = ReportID,
                        LoanID = LoanID,
                        AddToReport = status,
                        ReviewTypeID = ReviewTypeID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }

                db.SaveChanges();
            }

        }

        public ReportMaster GetReportMasterDetails()
        {
            ReportMaster _reportMaster = null;
            using (var db = new DBConnect(TenantSchema))
            {
                _reportMaster = db.ReportMaster.AsNoTracking().FirstOrDefault();
            }
            return _reportMaster;
        }

        public List<ReportConfig> GetReportMasterDocumentNames(Int64 ReportMasterID)
        {
            List<ReportConfig> reportConfig = new List<ReportConfig>(); ;
            using (var db = new DBConnect(TenantSchema))
            {
                //reportConfig = (from rm in db.ReportMaster.AsNoTracking()
                //                join rc in db.ReportConfig.AsNoTracking() on rm.ReportMasterID equals rc.ReportMasterID
                //                where rm.ReportMasterID == ReportMasterID
                //                select rc).ToList();

                reportConfig = db.ReportConfig.AsNoTracking().Where(x => x.ReportMasterID == ReportMasterID).ToList();


            }
            return reportConfig;
        }

        public void RemoveLoanReportingEntries(Int64 LoanId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanReporting> loan = db.LoanReporting.AsNoTracking().Where(lr => lr.LoanID == LoanId).ToList();
                if (loan.Count > 0)
                {
                    db.LoanReporting.RemoveRange(db.LoanReporting.Where(lr => lr.LoanID == LoanId));
                    db.SaveChanges();
                }
            }
        }

        public List<LoanStipulation> SaveLoanStipulations(Int64 LoanId, LoanStipulationDetails loanStipulationDetails, string username)
        {
            LoanStipulation _loanStipulation = null;
            List<LoanStipulation> _loanInvestorStipulation = null;
            List<LoanStipulationDetails> ListObj = new List<LoanStipulationDetails>();
            //LoanInvestorStipulationDetails _loanInvestorStipulation = new LoanInvestorStipulationDetails();
            //List<LoanInvestorStipulationDetails> _loanInvestorStipulation = null;
            using (var db = new DBConnect(TenantSchema))
            {
                string categoryName = new IntellaLendDataAccess().GetStipulationCategoryName(loanStipulationDetails.StipulationCategoryID);
                Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanId).FirstOrDefault();
                IDCFields _idcfields = db.IDCFields.AsNoTracking().Where(l => l.LoanID == LoanId).FirstOrDefault();
                if (loan != null)
                {
                    _loanStipulation = db.LoanStipulation.Add(new LoanStipulation()
                    {
                        LoanID = LoanId,
                        LoanTypeID = loan.LoanTypeID,
                        StipulationCategoryID = loanStipulationDetails.StipulationCategoryID,
                        CustomerID = loan.CustomerID,
                        ReviewTypeID = loan.ReviewTypeID,
                        StipulationDescription = loanStipulationDetails.StipulationDescription,
                        StipulationStatus = loanStipulationDetails.StipulationStatus,
                        Notes = loanStipulationDetails.StipulationNotes,
                        AuditMonthYear = loan.AuditMonthYear,
                        RecievedDate = _idcfields.Createdon,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        StipulationCategoryName = categoryName
                        //StipulationCategoryName = categoryName;
                    });

                    db.SaveChanges();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STIPULATION_CATEGORY);
                    LoanAudit.InsertLoanAudit(db, loan, loanStipulationDetails.StipulationDescription + " " + auditDescs[0] + " " + username, loanStipulationDetails.StipulationDescription + " " + auditDescs[1] + " " + username);
                    _loanInvestorStipulation = GetLoanStipulationDetails(LoanId);
                }
                return _loanInvestorStipulation;
            }
        }

        public List<LoanStipulation> UpdateLoanStipulations(Int64 LoanId, LoanStipulationDetails loanStipulationDetails, string username)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanStipulation> _loan = null;
                Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanId).FirstOrDefault();
                IDCFields _idcfilds = db.IDCFields.AsNoTracking().Where(l => l.LoanID == LoanId).FirstOrDefault();
                LoanStipulation _loanStipulation = db.LoanStipulation.AsNoTracking().Where(l => l.LoanID == LoanId && l.ID == loanStipulationDetails.ID).FirstOrDefault();
                _loanStipulation.StipulationCategoryID = loanStipulationDetails.StipulationCategoryID;
                _loanStipulation.StipulationDescription = loanStipulationDetails.StipulationDescription;
                _loanStipulation.StipulationStatus = loanStipulationDetails.StipulationStatus;
                _loanStipulation.Notes = loanStipulationDetails.StipulationNotes;
                db.Entry(_loanStipulation).State = EntityState.Modified;
                db.SaveChanges();
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STIPULATION_CATEGORY);
                LoanAudit.InsertLoanAudit(db, loan, loanStipulationDetails.StipulationDescription + " " + auditDescs[0], loanStipulationDetails.StipulationDescription + " " + auditDescs[1]);
                _loan = GetLoanStipulationDetails(LoanId);
                return _loan;
            }



        }

        public List<LoanStipulation> GetLoanStipulationDetails(Int64 LoanId)
        {
            List<LoanInvestorStipulationDetails> _loanInvestorStipulation = new List<LoanInvestorStipulationDetails>();
            List<LoanStipulation> _loanStipulation = new List<LoanStipulation>();
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanStipulation> _sloan = new List<LoanStipulation>();
                _sloan = db.LoanStipulation.AsNoTracking().Where(lr => lr.LoanID == LoanId).ToList();
                foreach (LoanStipulation _loanstip in _sloan)
                {
                    string CategoryName = new IntellaLendDataAccess().GetStipulationCategoryName(_loanstip.StipulationCategoryID);
                    _loanStipulation.Add(new LoanStipulation()
                    {
                        StipulationCategoryID = _loanstip.StipulationCategoryID,
                        StipulationCategoryName = CategoryName,
                        ID = _loanstip.ID,
                        Notes = _loanstip.Notes,
                        StipulationDescription = _loanstip.StipulationDescription,
                        StipulationStatus = _loanstip.StipulationStatus,
                        CreatedOn = _loanstip.CreatedOn
                    });
                }
            }
            return _loanStipulation;
        }

        #region Investor Stipulation
        public object GetReviewTypeSearchCriteria(Int64 ReviewTypeId)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                var ReviewData = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeId).FirstOrDefault().SearchCriteria;
                if (ReviewData != null && ReviewData != "")
                {
                    return data = (ReviewData == "Audit_Month_and_Year") ? 1 : 2;
                }
                else
                {
                    return data = 1;
                }
            }
        }

        public object GetReviewTypeItems()
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = db.ReviewTypeMaster.AsNoTracking().Where(x => (x.ReviewTypeName == "Pre-Closing Audit" || x.ReviewTypeName == "Pre-Closing Audit") && x.Active == true).ToList();
            }
            return data;
        }
        #endregion
        public bool ReSentJobLoanExport(Int64 JobID, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanJobExportDetail> _LoanJobExportDetail = db.LoanJobExportDetail.Where(x => x.JobID == JobID && x.Status == -1 && x.LoanID == LoanID).ToList();
                foreach (LoanJobExportDetail jobexportdetail in _LoanJobExportDetail)
                {
                    jobexportdetail.Status = ExportLoanStatusConstant.JOB_LOAN_WAITING; ;
                    db.Entry(jobexportdetail).State = EntityState.Modified;
                    db.SaveChanges();
                }
                LoanJobExport loanjobexport = db.LoanJobExport.Where(x => x.JobID == JobID).FirstOrDefault();
                if (loanjobexport != null)
                {
                    loanjobexport.Status = StatusConstant.JOB_WAITING;
                    db.Entry(loanjobexport).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return true;
        }

        public bool SaveLoanAuditDueDate(Int64 LoanID, DateTime AuditDueDate)
        {
            bool result = false;
            //AuditDueDate = AuditDueDate.AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {
                LoanSearch _loansearch = db.LoanSearch.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loansearch != null)
                {
                    _loansearch.AuditDueDate = (AuditDueDate == DateTime.MinValue) ? (DateTime?)null : AuditDueDate;
                    _loansearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loansearch).State = EntityState.Modified;
                    db.SaveChanges();

                    result = true;
                }
            }
            return result;
        }
        public string GetRuleFindings(long loanID)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                string _ruleFindings = string.Empty;
                string _rules = db.EmailTracker.AsNoTracking().Where(l => l.LoanID == loanID).Select(a => a.Body).FirstOrDefault();
                string loanNumber = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).Select(l => l.LoanNumber).FirstOrDefault();
                Int64 _assignedUserID = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).Select(l => l.AssignedUserID).FirstOrDefault();
                string userName = string.Empty;
                if (_assignedUserID > 0)
                {
                    User _user = db.Users.AsNoTracking().Where(u => u.UserID == _assignedUserID).FirstOrDefault();
                    userName = _user.FirstName + " " + _user.LastName;
                }
                if (!(string.IsNullOrEmpty(_rules)) && !(string.IsNullOrEmpty(loanNumber)))
                {
                    _ruleFindings = _rules + "~" + loanNumber + "~" + userName;
                    return _ruleFindings;
                }
                else
                {
                    return "";
                }

            }

        }
        public bool UpdateAuditCompletedDate(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loans = db.Loan.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loans != null)
                {
                    if (_loans.Status == 1)
                        _loans.AuditCompletedDate = DateTime.Now;
                    _loans.ModifiedOn = DateTime.Now;
                    db.Entry(_loans).State = EntityState.Modified;
                    db.SaveChanges();
                }
                LoanSearch _loansearch = db.LoanSearch.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loansearch != null)
                {
                    _loansearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loansearch).State = EntityState.Modified;
                    db.SaveChanges();
                }
                // string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.COMPLETED_BY);
                // LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0] + " " + UserName, auditDescs[1] + " " + UserName);
                return true;
            }
        }
        public bool UpdateLoanStatus(Int64 LoanID, string UserName)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loans = db.Loan.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loans != null)
                {
                    _loans.Status = StatusConstant.PENDING_AUDIT;
                    _loans.AuditCompletedDate = null;
                    _loans.CompleteNotes = string.Empty;
                    _loans.CompletedUserRoleID = 0;
                    _loans.CompletedUserID = 0;
                    _loans.ModifiedOn = DateTime.Now;
                    db.Entry(_loans).State = EntityState.Modified;
                    db.SaveChanges();
                }
                LoanSearch _loansearch = db.LoanSearch.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loansearch != null)
                {
                    _loansearch.Status = StatusConstant.PENDING_AUDIT;
                    _loansearch.ModifiedOn = DateTime.Now;
                    db.Entry(_loansearch).State = EntityState.Modified;
                    db.SaveChanges();
                }
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.REVERT_TO_READY_FOR_AUDIT);
                LoanAudit.InsertLoanAudit(db, _loans, auditDescs[0] + " " + UserName, auditDescs[1] + " " + UserName);
                return true;
            }
        }

        public string GetApplicationURL()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                string applicationURL = db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == 0 && c.ConfigKey == ConfigConstant.APPLICATIONURL).Select(cu => cu.ConfigValue).FirstOrDefault();
                return applicationURL;
            }

        }
        public string GetPDFFooterName()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                string _footerName = db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == 0 && c.ConfigKey == ConfigConstant.PDFFOOTER && c.Active == true).Select(cu => cu.ConfigValue).FirstOrDefault();
                return _footerName;
            }

        }


        //public object GetEncompassException()
        //{
        //    object data = null;
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        data = (from Et in db.EncompassDownloadExceptions.AsNoTracking()
        //                where Et.Status!= EncompassExceptionStatus.SUCCESSFUL_RETRY
        //                select new
        //                {
        //                    EncompassExceptionID = Et.EncompassExceptionID,
        //                    EncompassLoanNumber = Et.EncompassLoanNumber,
        //                    EncompassGuid = Et.EncompassGuid,
        //                    ExceptionMessage = Et.ExceptionMessage,
        //                    Status = Et.Status,
        //                    RetryCount = Et.RetryCount,
        //                     CreatedOn=Et.CreatedOn,
        //                      ModifiedOn=Et.ModifiedOn
        //                }).ToList();
        //    }
        //    return data;
        //}

        public object GetEncompassExceptionDetails(DateTime FromDate, DateTime ToDate)
        {
            object data = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from lt in db.EncompassDownloadExceptions.AsNoTracking()
                        where lt.ModifiedOn >= FromDate && lt.ModifiedOn <= ToDate && lt.Status != EncompassExceptionStatus.SUCCESSFUL_RETRY
                        select new
                        {

                            EncompassExceptionID = lt.EncompassExceptionID,
                            EncompassLoanNumber = lt.EncompassLoanNumber,
                            EncompassGuid = lt.EncompassGuid,
                            ExceptionMessage = lt.ExceptionMessage,
                            Status = lt.Status,
                            RetryCount = lt.RetryCount,
                            ModifiedOn = lt.ModifiedOn
                        }).ToList();
            }
            return data;

        }
        //public object ExportToBox(Int64 LoanID)
        //{
        //    object data = null;

        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        data = (from l in db.Loan.AsNoTracking()
        //                where LoanID == l.LoanID
        //                join d in db.LoanDetail.AsNoTracking() on l.LoanID equals d.LoanID
        //                join c in db.CustomerMaster.AsNoTracking() on l.CustomerID equals c.CustomerID
        //                join lt in db.LoanTypeMaster.AsNoTracking() on l.LoanTypeID equals lt.LoanTypeID
        //                select new
        //                {
        //                    LoanID = LoanID,
        //                    CustomerName = c.CustomerName,
        //                    LoanTypeName = lt.LoanTypeName,
        //                    LoanNumber = l.LoanNumber,
        //                    LoanObject = d.LoanObject,

        //                }).FirstOrDefault();
        //    }

        //    return data;
        //}
        public Loan GetLoan(Int64 LoanID)
        {
            Loan _loan = null;

            using (var db = new DBConnect(TenantSchema))
            {
                _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                if (_loan != null)
                {
                    _loan.LoanDetails = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                }
            }
            return _loan;
        }

        public string GetCustomerName(Int64 CustomerID)
        {

            string CustomerName = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerMaster cm = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == CustomerID).FirstOrDefault();
                if (cm != null)
                {
                    CustomerName = cm.CustomerName;
                }
            }
            return CustomerName;
        }

        public string GetLoanObject(Int64 LoanID)
        {
            string LoanObject = null;
            using (var db = new DBConnect(TenantSchema))
            {
                LoanObject = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault().LoanObject;

            }
            return LoanObject;

        }
        public bool RetryException(Int64 EncompassExceptionID)
        {
            bool flag = false;
            using (var db = new DBConnect(TenantSchema))
            {
                EncompassDownloadExceptions encompass = db.EncompassDownloadExceptions.AsNoTracking().Where(t => t.EncompassExceptionID == EncompassExceptionID).FirstOrDefault();
                if (encompass != null && encompass.Status != EncompassExceptionStatus.RETRY_LOAN && encompass.Status != EncompassExceptionStatus.SUCCESSFUL_RETRY)
                {
                    encompass.Status = EncompassExceptionStatus.RETRY_LOAN;
                    encompass.ModifiedOn = DateTime.Now;
                    encompass.RetryCount += 1;
                    db.Entry(encompass).State = EntityState.Modified;
                    db.SaveChanges();
                    flag = true;
                }

            }
            return flag;
        }
        public bool RetryBoxException(Int64 ID)
        {
            bool flag;
            using (var db = new DBConnect(TenantSchema))
            {
                BoxDownloadException _box = db.BoxDownloadException.AsNoTracking().Where(l => l.ID == ID).FirstOrDefault();

                if (_box != null)
                {
                    if (_box.Status != BoxExceptionStatus.SUCCESSFUL_RETRY && _box.Status != BoxExceptionStatus.RETRY_LOAN)
                    {
                        _box.Status = BoxExceptionStatus.RETRY_LOAN;
                        _box.RetryCount += 1;
                        db.Entry(_box).State = EntityState.Modified;
                        db.SaveChanges();
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }

                return flag;
            }
        }
        public object GetBoxDownloadExceptionDetails(DateTime FromDate, DateTime ToDate)
        {

            object data = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from _box in db.BoxDownloadException.AsNoTracking()
                        where _box.ModifiedOn >= FromDate && _box.ModifiedOn < ToDate
                        select new
                        {
                            ID = _box.ID,
                            LoanID = _box.LoanID,
                            BoxEntityID = _box.BoxEntityID,
                            BoxFileName = _box.BoxFileName,
                            BoxFilePath = _box.BoxFilePath,
                            Status = _box.Status,
                            RetryCount = _box.RetryCount,
                            CreatedOn = _box.CreatedOn,
                            ModifiedOn = _box.ModifiedOn,
                            FileSize = _box.FileSize,
                            ErrorMsg = _box.ErrorMsg,
                        }).ToList();
            }
            return data;
        }

        public object GetLoanMissingDocuments(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<MissingDocReport> failedResult = (from d in db.ELoanAttachmentDownload.AsNoTracking()
                                                       where d.LoanID == LoanID && (d.Status == EncompassStatusConstant.DOWNLOAD_FAILED || d.Status == EncompassStatusConstant.DOWNLOAD_RETRY)
                                                       select new MissingDocReport
                                                       {
                                                           StagingID = d.ID,
                                                           LoanID = d.LoanID,
                                                           Uploaded = d.CreatedOn,
                                                           EphesoftBatchInstanceID = "",
                                                           Status = d.Status == EncompassStatusConstant.DOWNLOAD_FAILED ? StatusConstant.FAILED_ENCOMPASS_DOWNLOAD : StatusConstant.PENDING_ENCOMPASS_DOWNLOAD,
                                                           ErrorMsg = d.Error
                                                       }).ToList();

                List<MissingDocReport> passedResult = (from l in db.AuditLoanMissingDoc.AsNoTracking()
                                                       where l.LoanID == LoanID
                                                       select new MissingDocReport
                                                       {
                                                           StagingID = l.EDownloadStagingID,
                                                           LoanID = l.LoanID,
                                                           Uploaded = l.AuditDateTime,
                                                           EphesoftBatchInstanceID = l.IDCBatchInstanceID,
                                                           Status = l.Status,
                                                           ErrorMsg = ""
                                                       }).ToList();

                return passedResult.Union(failedResult).Distinct().ToList();
            }
        }


        public bool DocumentObsolete(Int64 LoanID, Int64 DocId, Int64 DocVersion, bool IsObsolete, Int64 CurrentUserID, string DocName)
        {
            bool result = false;

            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail _loandetail = db.LoanDetail.Where(x => x.LoanID == LoanID).FirstOrDefault();
                if (_loandetail != null)
                {
                    Batch _docbatch = JsonConvert.DeserializeObject<Batch>(_loandetail.LoanObject);
                    foreach (Documents _docs in _docbatch.Documents)
                    {
                        if (_docs.DocumentTypeID == DocId && _docs.VersionNumber == DocVersion)
                        {
                            _docs.Obsolete = IsObsolete;
                        }
                    }
                    _loandetail.LoanObject = JsonConvert.SerializeObject(_docbatch);
                    _loandetail.ModifiedOn = DateTime.Now;
                    db.Entry(_loandetail).State = EntityState.Modified;
                    db.SaveChanges();

                    User user = db.Users.AsNoTracking().Where(u => u.UserID == CurrentUserID).FirstOrDefault();

                    string UserName = string.Empty;

                    if (user != null)
                    { UserName = string.Format("{0} {1}", user.LastName, user.FirstName); }

                    string[] auditDescs;
                    if (IsObsolete == true)
                    {
                        auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.Obsoleted);
                    }
                    else
                    {
                        auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.Document_Obsoleted_Reverted);
                    }
                    LoanAudit.InsertLoanDetailsAudit(db, _loandetail, CurrentUserID, auditDescs[0].Replace(AuditConfigConstant.USERNAME, UserName).Replace(AuditConfigConstant.DOCUMENTTYPENAME, DocName), auditDescs[1].Replace(AuditConfigConstant.USERNAME, UserName).Replace(AuditConfigConstant.DOCUMENTTYPENAME, DocName));
                    AddReportConfigDetails(TenantSchema, IsObsolete, LoanID);
                    result = true;
                }
            }
            return result;
        }
        public void AddReportConfigDetails(string schema, bool isObsolete, Int64 LoanID)
        {
            LoanRuleEngine ruleEngine = new LoanRuleEngine(schema, LoanID);

            List<Dictionary<string, string>> _missingDocs = ruleEngine.GetMissingDocumentsInLoan;
            ReportMaster _reportMaster = GetReportMasterDetails();
            if (_reportMaster != null && _missingDocs.Count > 0)
            {
                List<ReportConfig> reportConfig = GetReportMasterDocumentNames(_reportMaster.ReportMasterID);
                RemoveLoanReportingEntries(ruleEngine.loan.LoanID);
                for (int i = 0; i < _missingDocs.Count; i++)
                {
                    foreach (var property in reportConfig)
                    {
                        if (_missingDocs[i]["DocName"] == property.DocumentName)
                        {
                            AddReportConfigDetails(property.ReportID, ruleEngine.loan.LoanID, ruleEngine.loan.ReviewTypeID, true);

                        }
                    }
                }
            }
        }

        //Encompass Export:

        public object SearchEncompassExportDetails(Int64 StatusID, DateTime FromDate, DateTime ToDate, Int64 customerId)
        {
            object data = null;

            List<EUploadDetails> _eLoan = new List<EUploadDetails>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {
                //status = 5 -- default filter "ALL"
                //customerID=0 --default filter "ALL"
                var _eLoanData = (from upload in db.EUploadStaging.AsNoTracking()
                                  join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                                  join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                                  where (upload.ModifiedOn >= FromDate && upload.ModifiedOn < ToDate)
                                  && (StatusID == 5 || upload.Status == StatusID)
                                  && (customerId == 0 || cus.CustomerID == customerId)
                                  select new
                                  {
                                      LoanID = upload.LoanID,
                                      LoanNumber = loan.LoanNumber,
                                      CustomerName = cus.CustomerName,
                                      ELoanGUID = loan.EnCompassLoanGUID
                                  }).Distinct().ToList();



                foreach (var item in _eLoanData)
                {
                    List<ELoanAttachmentUpload> _stg = db.ELoanAttachmentUpload.AsNoTracking().Where(x => x.LoanID == item.LoanID && x.Status != EncompassUploadConstant.UPLOAD_COMPLETE).ToList();

                    Int64 status = EncompassUploadConstant.UPLOAD_COMPLETE;
                    string ErrorMsg = string.Empty;

                    if (_stg.Any(x => x.Status == EncompassUploadConstant.UPLOAD_RETRY))
                        status = EncompassUploadConstant.UPLOAD_RETRY;
                    else if (_stg.Any(x => x.Status == EncompassUploadConstant.UPLOAD_FAILED))
                        status = EncompassUploadConstant.UPLOAD_FAILED;
                    else if (_stg.Any(x => x.Status == EncompassUploadConstant.UPLOAD_WAITING))
                        status = EncompassUploadConstant.UPLOAD_WAITING;
                    else if (_stg.Any(x => x.Status == EncompassUploadConstant.UPLOAD_PROCESSING))
                        status = EncompassUploadConstant.UPLOAD_PROCESSING;


                    ELoanAttachmentUpload _stgVal = _stg.Where(x => x.Error != null && x.Error != "" && x.Status == status && x.Status != EncompassUploadConstant.UPLOAD_COMPLETE).OrderByDescending(x => x.ModifiedOn).FirstOrDefault();
                    if (_stgVal != null)
                    {
                        ErrorMsg = _stgVal.Error;
                        if (StatusID == status || StatusID == 5)
                        {
                            _eLoan.Add(new EUploadDetails()
                            {
                                ID = 0,
                                ELoanGUID = item.ELoanGUID.GetValueOrDefault(),
                                TypeOfUpload = EncompassLoanAttachmentDownloadConstant.Loan,
                                CustomerName = item.CustomerName,
                                LoanID = item.LoanID,
                                LoanNumber = item.LoanNumber,
                                Status = status,
                                Error = ErrorMsg,
                                CreatedOn = _stg.OrderByDescending(x => x.ModifiedOn).FirstOrDefault().ModifiedOn
                            });
                        }
                    }

                }

                List<ELoanAttachmentUpload> _eLoanAttachmentSuccess = db.ELoanAttachmentUpload.AsNoTracking()
                    .Where(x => x.Status == EncompassUploadConstant.UPLOAD_COMPLETE && (StatusID == 5 || StatusID == EncompassUploadConstant.UPLOAD_COMPLETE) && x.ModifiedOn >= FromDate && x.ModifiedOn < ToDate)
                    .Where(y => !db.ELoanAttachmentUpload.Any(z => !y.ID.Equals(z.ID) && y.LoanID.Equals(z.LoanID) && y.ModifiedOn < z.ModifiedOn && z.Status.Equals(EncompassUploadConstant.UPLOAD_COMPLETE)))
                    .ToList();
                foreach (var eLoanAttachment in _eLoanAttachmentSuccess)
                {

                    Loan eloan = db.Loan.AsNoTracking().Where(l => l.LoanID == eLoanAttachment.LoanID).FirstOrDefault();
                    if (eloan != null)
                    {
                        CustomerMaster _custDetail = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == eloan.CustomerID && (c.CustomerID == customerId || customerId == 0)).FirstOrDefault();
                        if (_custDetail != null)
                        {
                            _eLoan.Add(new EUploadDetails()
                            {
                                ID = 0,
                                ELoanGUID = eLoanAttachment.ELoanGUID,
                                TypeOfUpload = EncompassLoanAttachmentDownloadConstant.Loan,
                                CustomerName = _custDetail.CustomerName,
                                LoanID = eLoanAttachment.LoanID,
                                LoanNumber = eloan.LoanNumber,
                                Status = EncompassUploadConstant.UPLOAD_COMPLETE,
                                Error = string.Empty,
                                CreatedOn = eLoanAttachment.ModifiedOn
                            });
                        }
                    }

                }


                //if ((customerId != 0) && (Status != 5 && FromDate != null && ToDate != null))
                //{
                //    _eLoan = (from upload in db.ELoanAttachmentUpload.AsNoTracking()
                //              join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                //              join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                //              where (loan.CustomerID == customerId && upload.CreatedOn >= FromDate && upload.ModifiedOn < ToDate)
                //              select new EUploadDetails()
                //              {
                //                  ID = upload.ID,
                //                  LoanID = upload.LoanID,
                //                  // CustomerID = cus.CustomerID,
                //                  LoanNumber = loan.LoanNumber,
                //                  CustomerName = cus.CustomerName,
                //                  ELoanGUID = upload.ELoanGUID,
                //                  TypeOfUpload = upload.TypeOfUpload,
                //                  Status = upload.Status,
                //                  Error = upload.Error,
                //                  CreatedOn = upload.CreatedOn,

                //              }).OrderByDescending(o => o.CreatedOn).ToList();
                //    data = _eLoan.GroupBy(item => item.LoanID).Select(
                //           group => new
                //           {
                //               ID = _eLoan.First().ID,
                //               LoanID = group.First().LoanID,
                //               CustomerID = group.First().CustomerID,
                //               LoanNumber = group.First().LoanNumber,
                //               CustomerName = group.First().CustomerName,
                //               ELoanGUID = group.First().ELoanGUID,
                //               TypeOfUpload = group.First().TypeOfUpload,
                //               Status = GetStatusForEncompassUplod(group.First().ID, group.First().LoanID),
                //               Error = group.First().Error,
                //               CreatedOn = group.First().CreatedOn,


                //           }).OrderByDescending(o => o.CreatedOn).Where(l => l.Status == Status).ToList();


                //}
                //else if ((customerId != 0) && FromDate != null && ToDate != null)
                //{
                //    _eLoan = (from upload in db.ELoanAttachmentUpload.AsNoTracking()
                //              join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                //              join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                //              where loan.CustomerID == customerId && upload.CreatedOn >= FromDate && upload.ModifiedOn < ToDate
                //              select new EUploadDetails()
                //              {
                //                  ID = upload.ID,
                //                  LoanID = upload.LoanID,
                //                  LoanNumber = loan.LoanNumber,
                //                  //  CustomerID=cus.CustomerID,
                //                  CustomerName = cus.CustomerName,
                //                  ELoanGUID = upload.ELoanGUID,
                //                  TypeOfUpload = upload.TypeOfUpload,
                //                  Status = upload.Status,
                //                  Error = upload.Error,
                //                  CreatedOn = upload.CreatedOn,

                //              }).ToList();
                //    data = _eLoan.GroupBy(item => item.LoanID).Select(
                //           group => new
                //           {
                //               ID = _eLoan.First().ID,
                //               LoanID = group.First().LoanID,
                //               // CustomerID = group.First().CustomerID,
                //               LoanNumber = group.First().LoanNumber,
                //               CustomerName = group.First().CustomerName,
                //               ELoanGUID = group.First().ELoanGUID,
                //               TypeOfUpload = group.First().TypeOfUpload,
                //               Status = GetStatusForEncompassUplod(group.First().ID, group.First().LoanID),
                //               Error = group.First().Error,
                //               CreatedOn = group.First().CreatedOn,


                //           }).OrderByDescending(o => o.CreatedOn).ToList();



                //}
                //else if ((Status != 5) & FromDate != null && ToDate != null)
                //{
                //    _eLoan = (from upload in db.ELoanAttachmentUpload.AsNoTracking()
                //              join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                //              join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                //              where upload.CreatedOn >= FromDate && upload.ModifiedOn < ToDate
                //              select new EUploadDetails()
                //              {
                //                  ID = upload.ID,
                //                  LoanID = upload.LoanID,
                //                  // CustomerID = cus.CustomerID,
                //                  LoanNumber = loan.LoanNumber,
                //                  CustomerName = cus.CustomerName,
                //                  ELoanGUID = upload.ELoanGUID,
                //                  TypeOfUpload = upload.TypeOfUpload,
                //                  Status = upload.Status,
                //                  Error = upload.Error,
                //                  CreatedOn = upload.CreatedOn,

                //              }).ToList();
                //    data = _eLoan.GroupBy(item => item.LoanID).Select(
                //           group => new
                //           {
                //               ID = _eLoan.First().ID,
                //               LoanID = group.First().LoanID,
                //               //  CustomerID = group.First().CustomerID,
                //               LoanNumber = group.First().LoanNumber,
                //               CustomerName = group.First().CustomerName,
                //               ELoanGUID = group.First().ELoanGUID,
                //               TypeOfUpload = group.First().TypeOfUpload,
                //               Status = GetStatusForEncompassUplod(group.First().ID, group.First().LoanID),
                //               Error = group.First().Error,
                //               CreatedOn = group.First().CreatedOn,


                //           }).OrderByDescending(o => o.CreatedOn).Where(l => l.Status == Status).ToList();
                //}
                //else if (Status != 5)
                //{


                //    _eLoan = (from upload in db.ELoanAttachmentUpload.AsNoTracking()
                //              join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                //              join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                //              where upload.CreatedOn >= FromDate && upload.ModifiedOn < ToDate
                //              select new EUploadDetails()
                //              {
                //                  ID = upload.ID,
                //                  LoanID = upload.LoanID,
                //                  // CustomerID = cus.CustomerID,
                //                  LoanNumber = loan.LoanNumber,
                //                  CustomerName = cus.CustomerName,
                //                  ELoanGUID = upload.ELoanGUID,
                //                  TypeOfUpload = upload.TypeOfUpload,
                //                  Status = upload.Status,
                //                  Error = upload.Error,
                //                  CreatedOn = upload.CreatedOn,

                //              }).ToList();
                //    data = _eLoan.GroupBy(item => item.LoanID).Select(
                //           group => new
                //           {
                //               ID = _eLoan.First().ID,
                //               LoanID = group.First().LoanID,
                //               CustomerID = group.First().CustomerID,
                //               LoanNumber = group.First().LoanNumber,
                //               CustomerName = group.First().CustomerName,
                //               ELoanGUID = group.First().ELoanGUID,
                //               TypeOfUpload = group.First().TypeOfUpload,
                //               Status = GetStatusForEncompassUplod(group.First().ID, group.First().LoanID),
                //               Error = group.First().Error,
                //               CreatedOn = group.First().CreatedOn,


                //           }).OrderByDescending(o => o.CreatedOn).Where(l => l.Status == Status).ToList();



                //}
                //else if (FromDate != null && ToDate != null)
                //{
                //    _eLoan = (from upload in db.ELoanAttachmentUpload.AsNoTracking()
                //              join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                //              join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                //              where upload.CreatedOn >= FromDate && upload.ModifiedOn < ToDate
                //              select new EUploadDetails()
                //              {
                //                  ID = upload.ID,
                //                  LoanID = upload.LoanID,
                //                  CustomerID = cus.CustomerID,
                //                  LoanNumber = loan.LoanNumber,
                //                  CustomerName = cus.CustomerName,
                //                  ELoanGUID = upload.ELoanGUID,
                //                  TypeOfUpload = upload.TypeOfUpload,
                //                  Status = upload.Status,
                //                  Error = upload.Error,
                //                  CreatedOn = upload.CreatedOn,

                //              }).ToList();
                //    data = _eLoan.GroupBy(item => item.LoanID).Select(
                //           group => new
                //           {
                //               ID = group.First().ID,
                //               LoanID = group.First().LoanID,
                //               // CustomerID = group.First().CustomerID,
                //               LoanNumber = group.First().LoanNumber,
                //               CustomerName = group.First().CustomerName,
                //               ELoanGUID = group.First().ELoanGUID,
                //               TypeOfUpload = group.First().TypeOfUpload,
                //               Status = GetStatusForEncompassUplod(group.First().ID, group.First().LoanID),
                //               Error = group.First().Error,
                //               CreatedOn = group.First().CreatedOn,

                //           }).OrderByDescending(o => o.CreatedOn).ToList();
                //}
                //else
                //{

                //    _eLoan = (from upload in db.ELoanAttachmentUpload.AsNoTracking()
                //              join loan in db.Loan.AsNoTracking() on upload.LoanID equals loan.LoanID
                //              join cus in db.CustomerMaster.AsNoTracking() on loan.CustomerID equals cus.CustomerID
                //              where upload.CreatedOn >= FromDate && upload.ModifiedOn < ToDate
                //              select new EUploadDetails()
                //              {
                //                  ID = upload.ID,
                //                  LoanID = upload.LoanID,
                //                  CustomerID = cus.CustomerID,
                //                  LoanNumber = loan.LoanNumber,
                //                  CustomerName = cus.CustomerName,
                //                  ELoanGUID = upload.ELoanGUID,
                //                  TypeOfUpload = upload.TypeOfUpload,
                //                  Status = upload.Status,
                //                  Error = upload.Error,
                //                  CreatedOn = upload.CreatedOn,

                //              }).ToList();
                //    data = _eLoan.GroupBy(item => item.LoanID).Select(
                //           group => new
                //           {
                //               ID = group.First().ID,
                //               LoanID = group.First().LoanID,
                //               // CustomerID = group.First().CustomerID,
                //               LoanNumber = group.First().LoanNumber,
                //               CustomerName = group.First().CustomerName,
                //               ELoanGUID = group.First().ELoanGUID,
                //               TypeOfUpload = group.First().TypeOfUpload,
                //               Status = GetStatusForEncompassUplod(group.First().ID, group.First().LoanID),
                //               Error = group.First().Error,
                //               CreatedOn = group.First().CreatedOn,

                //           }).OrderByDescending(o => o.CreatedOn).ToList();

                //}

                return _eLoan;
            }
        }
        public bool RetryEncompassExport(Int64 ID, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<ELoanAttachmentUpload> _eFailedLoans = db.ELoanAttachmentUpload.Where(x => x.LoanID == LoanID && (x.Status == EncompassUploadConstant.UPLOAD_FAILED || x.Status == EncompassUploadConstant.UPLOAD_WAITING)).ToList();
                if (_eFailedLoans != null && _eFailedLoans.Count > 0)
                {
                    foreach (ELoanAttachmentUpload _Loans in _eFailedLoans)
                    {
                        _Loans.Status = EncompassUploadConstant.UPLOAD_RETRY;
                        _Loans.ModifiedOn = DateTime.Now;
                        db.Entry(_Loans).State = EntityState.Modified;
                        List<EUploadStaging> _eUploadStagingLoans = db.EUploadStaging.Where(x => x.UploadStagingID == _Loans.ID && x.LoanID == _Loans.LoanID && x.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED).ToList();
                        foreach (EUploadStaging _eUplod in _eUploadStagingLoans)
                        {
                            _eUplod.Status = EncompassUploadConstant.UPLOAD_RETRY;
                            _eUplod.ModifiedOn = DateTime.Now;
                            db.Entry(_eUplod).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            return false;
        }
        public bool RetryEncompassLoanDownload(Int64 LoanID, Int64 EDownloadID)
        {

            using (var db = new DBConnect(TenantSchema))
            {
                ELoanAttachmentDownload _eFailedLoans = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.ID == EDownloadID).FirstOrDefault();

                if (LoanID > 0 && _eFailedLoans.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan)
                {
                    Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                    LoanSearch loansearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.Status = StatusConstant.PENDING_ENCOMPASS_DOWNLOAD;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs1 = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.LOAN_STATUS_MODIFIED);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs1[0], auditDescs1[1]);
                    }

                    if (loansearch != null)
                    {
                        loansearch.Status = StatusConstant.PENDING_ENCOMPASS_DOWNLOAD;
                        loansearch.ModifiedOn = DateTime.Now;
                        db.Entry(loansearch).State = EntityState.Modified;
                        db.SaveChanges();

                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.STATUS_UPDATED_BY_SYSTEM);
                        LoanAudit.InsertLoanSearchAudit(db, loansearch != null ? loansearch : loansearch, auditDescs[0], auditDescs[1]);
                        db.SaveChanges();
                    }
                }

                if (_eFailedLoans != null)
                {
                    EWebhookEvents eWebhookEvents = db.EWebhookEvents.AsNoTracking().Where(x => x.Response.ToUpper().Contains(_eFailedLoans.ELoanGUID.ToString().ToUpper())).FirstOrDefault();
                    
                    if(eWebhookEvents != null)
                    {
                        eWebhookEvents.Status = EWebHookStatusConstant.EWEB_HOOK_STAGED;
                        db.Entry(eWebhookEvents).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    _eFailedLoans.Status = EncompassStatusConstant.DOWNLOAD_RETRY;
                    _eFailedLoans.Error = string.Empty;
                    _eFailedLoans.ModifiedOn = DateTime.Now;
                    db.Entry(_eFailedLoans).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public Int64 GetStatusForEncompassUplod(Int64 ID, Int64 LoanID)
        {
            Int64 status = 0;
            List<ELoanAttachmentUpload> data = new List<ELoanAttachmentUpload>();
            List<EUploadStaging> _eLoans = new List<EUploadStaging>();
            using (var db = new DBConnect(TenantSchema))
            {

                _eLoans = db.EUploadStaging.AsNoTracking().Where(l => l.LoanID == LoanID && l.UploadStagingID == ID).ToList();

                if (_eLoans.Any(l => l.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED) == true)
                {
                    status = EncompassUploadConstant.UPLOAD_FAILED;

                }
                else if (_eLoans.Any(l => l.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_WAITING) == true)
                {
                    status = EncompassUploadConstant.UPLOAD_WAITING;

                }
                else if (_eLoans.Any(l => l.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_PROCESSING) == true)
                {
                    status = EncompassUploadConstant.UPLOAD_PROCESSING;

                }
                else if (_eLoans.Any(l => l.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE) == true)
                {
                    status = EncompassUploadConstant.UPLOAD_COMPLETE;

                }

                UpdateStatus(status, ID, LoanID);

            }
            return status;
        }

        public void UpdateStatus(Int64 status, Int64 ID, Int64 Loanid)
        {
            using (var db = new DBConnect(TenantSchema))
            {

                ELoanAttachmentUpload upload = db.ELoanAttachmentUpload.AsNoTracking().Where(ls => ls.LoanID == Loanid && ls.ID == ID).FirstOrDefault();
                if (upload != null)
                {
                    upload.Status = status;
                    upload.ModifiedOn = DateTime.Now;
                    db.Entry(upload).State = EntityState.Modified;
                    db.SaveChanges();


                }


            }
        }
        public object GetEncompassCurrentExportDetail(Int64 ID, Int64 LoanID)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from _eLoans in db.EUploadStaging.AsNoTracking().Where(l => l.LoanID == LoanID)

                        select new EUploadStagingDetails()
                        {
                            ID = _eLoans.ID,
                            UploadStagingID = _eLoans.UploadStagingID,
                            LoanID = _eLoans.LoanID,
                            TypeOfUpload = _eLoans.TypeOfUpload,
                            Document = _eLoans.Document,
                            EParkingSpot = _eLoans.EParkingSpot,
                            Version = _eLoans.Version,
                            FileName = _eLoans.Document + "-V" + _eLoans.Version + ".pdf",
                            Status = _eLoans.Status,
                            ErrorMsg = _eLoans.ErrorMsg,
                            CreatedOn = _eLoans.CreatedOn,
                            ModifiedOn = _eLoans.ModifiedOn,
                        }).ToList();
            }


            return data;
        }

        public bool RetryEncompassUploadStaging(Int64 ID, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<EUploadStaging> _eFailedLoans = db.EUploadStaging.Where(x => x.ID == ID && x.Status == EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED && x.LoanID == LoanID).ToList();
                foreach (EUploadStaging _Loans in _eFailedLoans)
                {
                    _Loans.Status = EncompassUploadStagingConstant.UPLOAD_STAGING_WAITING;
                    db.Entry(_Loans).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            return true;
        }
        public object GetEncompassExportDetails()
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                data = (from _eLoans in db.ELoanAttachmentUpload.AsNoTracking()


                        select new
                        {
                            ID = _eLoans.ID,
                            LoanID = _eLoans.LoanID,
                            //LoanNumber = db.Loan.AsNoTracking().Where(l => l.LoanID == _eLoans.LoanID).FirstOrDefault().LoanNumber,
                            ELoanGUID = _eLoans.ELoanGUID,
                            TypeOfUpload = _eLoans.TypeOfUpload,
                            Documents = _eLoans.Documents,
                            Status = _eLoans.Status,
                            Error = _eLoans.Error,
                            CreatedOn = _eLoans.CreatedOn,
                            ModifiedOn = _eLoans.ModifiedOn,

                        }).ToList();

            }
            return data;
        }
        public object SearchLosExportMonitorDetails(Int64 CustomerId, DateTime FromDate, DateTime ToDate, Int64 LoanTypeId, Int64 serviceTypeId)
        {
            object data = null;
            List<LOSExportDetails> _losLoan = new List<LOSExportDetails>();

            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TenantSchema))
            {

                var _losdata = (from L in db.LOSExportFileStaging.AsNoTracking()
                                join u in db.Loan.AsNoTracking() on L.LoanID equals u.LoanID
                                join idc in db.IDCFields.AsNoTracking() on L.LoanID equals idc.LoanID
                                join cus in db.CustomerMaster.AsNoTracking() on u.CustomerID equals cus.CustomerID
                                join loan in db.LoanTypeMaster.AsNoTracking() on u.LoanTypeID equals loan.LoanTypeID
                                join r in db.ReviewTypeMaster.AsNoTracking() on u.ReviewTypeID equals r.ReviewTypeID

                                where (L.CreatedOn >= FromDate && L.ModifiedOn < ToDate)
                                                                  && (CustomerId == 0 || cus.CustomerID == CustomerId)
                                                                  && (LoanTypeId == 0 || loan.LoanTypeID == LoanTypeId)
                                                                  && (serviceTypeId == 0 || r.ReviewTypeID == serviceTypeId)
                                select new
                                {
                                    LoanID = L.LoanID,
                                    LoanNumber = u.LoanNumber,
                                    EphesoftBatchInstanceID = idc.IDCBatchInstanceID,
                                    LoanTypeName = loan.LoanTypeName,
                                    CustomerName = cus.CustomerName,
                                    ReviewTypeName = r.ReviewTypeName,
                                }).Distinct().ToList();



                foreach (var item in _losdata)
                {
                    List<LOSExportFileStaging> _exportDetails = db.LOSExportFileStaging.AsNoTracking().Where(x => x.LoanID == item.LoanID).OrderByDescending(x => x.ModifiedOn).ToList();
                    LOSExportFileStaging _stg = _exportDetails.FirstOrDefault();

                    Int32 status = 0;
                    string ErrorMsg = string.Empty;
                    string FileName = string.Empty;

                    if (_stg != null)
                    {
                        LOSExportFileStaging _export = null;

                        if (_exportDetails.Any(x => x.Status == LOSExportStatusConstant.LOS_LOAN_ERROR))
                        {
                            _export = _exportDetails.Where(x => x.Status == LOSExportStatusConstant.LOS_LOAN_ERROR).OrderByDescending(x => x.ModifiedOn).FirstOrDefault();
                            status = LOSExportFileTypeConstant.GetFileTypeErrorStatus(_export.FileType);
                        }
                        else if (_exportDetails.Any(x => x.Status == LOSExportStatusConstant.LOS_LOAN_STAGED))
                        {
                            _export = _exportDetails.Where(x => x.LoanID == item.LoanID).OrderByDescending(x => x.ModifiedOn).FirstOrDefault();
                            status = LOSExportStatusConstant.LOS_LOAN_STAGED;
                        }
                        else
                        {
                            status = _stg.Status == LOSExportStatusConstant.LOS_LOAN_PROCESSED ? LOSExportFileTypeConstant.GetFileTypeProcessedStatus(_stg.FileType) : _stg.Status;
                        }

                        ErrorMsg = _export == null ? _stg.ErrorMsg : _export.ErrorMsg;
                        FileName = _export == null ? _stg.FileName : _export.FileName;
                    }

                    _losLoan.Add(new LOSExportDetails()
                    {
                        ID = 0,
                        LoanID = item.LoanID,
                        FileName = FileName,
                        Status = status,
                        ErrorMsg = ErrorMsg,
                        LoanNumber = item.LoanNumber,
                        EphesoftBatchInstanceID = item.EphesoftBatchInstanceID,
                        LoanTypeName = item.LoanTypeName,
                        CustomerName = item.CustomerName,
                        ReviewTypeName = item.ReviewTypeName
                    });

                }

            }


            return _losLoan;
        }






        public object GetLOSCurrentExportLoanDetail(Int64 ID, Int64 LoanID)
        {
            object data = null;

            using (var db = new DBConnect(TenantSchema))
            {
                var _losLoans = (from lefs in db.LOSExportFileStaging.AsNoTracking()
                                 join amd in db.AuditLoanMissingDoc.AsNoTracking() on lefs.TrailingAuditID equals amd.AuditID into grpjoin_lefs_amd
                                 from amd in grpjoin_lefs_amd.DefaultIfEmpty()

                                 where (lefs.LoanID == LoanID)

                                 select new
                                 {
                                     ID = lefs.ID,
                                     LoanID = lefs.LoanID,
                                     FileName = lefs.FileName,
                                     FileJson = lefs.FileJson,
                                     FileType = lefs.FileType,
                                     ErrorMsg = lefs.ErrorMsg,
                                     Status = lefs.Status,
                                     ModifiedOn = lefs.ModifiedOn,
                                     CreatedOn = lefs.CreatedOn,
                                     TrailingBatchId = amd.IDCBatchInstanceID
                                 }
                    ).OrderBy(l => l.ModifiedOn).ToList();
                data = (_losLoans.Select(item => new
                {
                    ID = item.ID,
                    LoanID = item.LoanID,
                    FileName = item.FileName,
                    FileJson = item.FileJson,
                    FileType = item.FileType,
                    FileTypeName = LOSExportFileTypeConstant.GetFileTypeDescription(item.FileType),
                    ErrorMsg = item.ErrorMsg,
                    Status = item.Status,
                    ModifiedOn = item.ModifiedOn,
                    TrailingBatchId = item.TrailingBatchId,
                    IsLatest = (_losLoans.Where(x => x.FileType == item.FileType).OrderByDescending(x => x.CreatedOn).FirstOrDefault() == item),
                })).OrderBy(l => l.ModifiedOn).ToList();
            }

            return data;
        }

        public bool RetryLosExportDetails(Int64 ID, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<LOSExportFileStaging> _LosFailedLoans = db.LOSExportFileStaging.AsNoTracking().Where(x => x.ID == ID && x.Status == LOSExportStatusConstant.LOS_LOAN_ERROR).ToList();
                if (_LosFailedLoans != null && _LosFailedLoans.Count > 0)
                {
                    foreach (LOSExportFileStaging _Loans in _LosFailedLoans)
                    {
                        _Loans.Status = LOSExportStatusConstant.LOS_LOAN_STAGED;
                        _Loans.ErrorMsg = string.Empty;
                        _Loans.ModifiedOn = DateTime.Now;
                        db.Entry(_Loans).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            return false;
        }

        public bool ReExportLosDetail(Int64 LoanID, Int32 FileType, Int64 ID)
        {
            bool Result = false;
            using (var db = new DBConnect(TenantSchema))
            {
                Loan loan = db.Loan.AsNoTracking().Where(x => x.LoanID.Equals(LoanID)).FirstOrDefault();
                LOSExportFileStaging losExportFileStaging = db.LOSExportFileStaging.AsNoTracking().Where(x => x.ID.Equals(ID)).FirstOrDefault();
                if (Result = (loan != null))
                {
                    LOSExportFileStaging _lStage = new LOSExportFileStaging()
                    {
                        LoanID = LoanID,
                        FileType = FileType,
                        Status = LOSExportStatusConstant.LOS_LOAN_STAGED,
                        FileName = $"{loan.LoanNumber}{LOSExportFileTypeConstant.GetFileNameWord(FileType)}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.json",
                        FileJson = losExportFileStaging.FileJson,
                        ErrorMsg = string.Empty,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.LOSExportFileStaging.Add(_lStage);
                    db.SaveChanges();
                }
            }
            return Result;
        }

        public class LOSExportDetails
        {
            public Int64 ID { get; set; }
            public Int64 LoanID { get; set; }
            public string FileName { get; set; }
            public string FileJson { get; set; }
            public Int32 FileType { get; set; }
            public string FileTypeName { get; set; }
            public string ErrorMsg { get; set; }
            public Int32 Status { get; set; }
            public DateTime? CreatedOn { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public string LoanNumber { get; set; }
            public string EphesoftBatchInstanceID { get; set; }
            public string LoanTypeName { get; set; }
            public string CustomerName { get; set; }
            public string ReviewTypeName { get; set; }
        }
        public class EUploadDetails
        {
            public Int64 ID { get; set; }
            public Int64 LoanID { get; set; }
            public Guid ELoanGUID { get; set; }
            public Int64 TypeOfUpload { get; set; }
            public string LoanNumber { get; set; }
            //public string Documents { get; set; }
            public Int64 Status { get; set; }
            public Int64 CustomerID { get; set; }
            public string Error { get; set; }
            public DateTime CreatedOn { get; set; }
            public string CustomerName { get; set; }
            //public DateTime ModifiedOn { get; set; }
        }

        public class EUploadStagingDetails
        {

            public Int64 ID { get; set; }
            public Int64 UploadStagingID { get; set; }
            public Int64 LoanID { get; set; }
            public Int64 TypeOfUpload { get; set; }
            public string Document { get; set; }
            public string EParkingSpot { get; set; }
            public Int64 Version { get; set; }
            public string FileName { get; set; }
            public Int64 Status { get; set; }
            public string ErrorMsg { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime ModifiedOn { get; set; }

        }

        class RequiredDocument
        {
            public string DocumentID { get; set; }
            public string Version { get; set; }
        }

        class MissingDocReport
        {
            public Int64 StagingID { get; set; }
            public Int64 LoanID { get; set; }
            public DateTime Uploaded { get; set; }
            public string EphesoftBatchInstanceID { get; set; }
            public Int64 Status { get; set; }
            public Int64 SubStatus { get; set; }
            public string ErrorMsg { get; set; }
        }

        class LoanSearchReport
        {
            public Int64 LoanID { get; set; }
            public string LoanNumber { get; set; }
            public Int64 LoanTypeID { get; set; }

            public Int64 UploadType { get; set; }
            public DateTime? ReceivedDate { get; set; }
            public Int64 Status { get; set; }
            //   public Int64 SubStatus { get; set; }
            public decimal LoanAmount { get; set; }
            public string LoanTypeName { get; set; }
            public string BorrowerName { get; set; }
            public string StatusDescription { get; set; }
            public Int64 LoggedUserID { get; set; }
            public string ServiceTypeName { get; set; }
            public DateTime AuditMonthYearDate { get; set; }
            public string AuditMonthYear { get; set; }
            public string Customer { get; set; }
            public string EphesoftBatchInstanceID { get; set; }
            public Int64 AssignedUserID { get; set; }
            public string AssignedUser { get; set; }
            public DateTime? AuditDueDate { get; set; }

        }

    }
}

