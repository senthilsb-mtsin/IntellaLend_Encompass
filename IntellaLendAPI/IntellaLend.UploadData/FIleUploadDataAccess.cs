using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.BoxWrapper;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class FileUploadDataAccess
    {
        protected static string TableSchema;

        #region Constructor

        public FileUploadDataAccess() { }

        public FileUploadDataAccess(string tableSchema)
        {
            TableSchema = tableSchema;
        }

        #endregion

        #region Public Methods        

        public Loan AddFileUploadDetails(Loan loan, DateTime AuditDueDate)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    WorkFlowStatusMaster isExiists = new IntellaLendDataAccess().GetWorkFlowMaster().Where(u => u.StatusID == StatusConstant.READY_FOR_IDC).FirstOrDefault();
                    if (isExiists != null)
                    {
                        loan.Status = isExiists.StatusID;
                        db.Loan.Add(loan);
                        db.SaveChanges();

                        IDCFields _idcObj = new IDCFields() { LoanID = loan.LoanID, Createdon = DateTime.Now, ModifiedOn = DateTime.Now, OCRAccuracyCalculated = false };
                        db.IDCFields.Add(_idcObj);
                        db.SaveChanges();

                        LoanAudit.InsertLoanIDCFieldAudit(db, _idcObj, "Loan Uploaded", "");

                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TableSchema, AuditConfigConstant.UPLOADED_FROM_INTELLALEND);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);

                        LoanSearch _loansearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == loan.LoanID).FirstOrDefault();
                        if (_loansearch == null)
                        {
                            db.LoanSearch.Add(new LoanSearch
                            {
                                AuditDueDate = AuditDueDate,
                                ReceivedDate = loan.CreatedOn,
                                CreatedOn = DateTime.Now,
                                LoanID = loan.LoanID,
                                Status = loan.Status,
                                LoanTypeID = loan.LoanTypeID,
                                CustomerID = loan.CustomerID
                            });
                            db.SaveChanges();
                        }
                        else
                        {
                            _loansearch.LoanTypeID = loan.LoanTypeID;
                            _loansearch.CustomerID = loan.CustomerID;
                            _loansearch.ReceivedDate = loan.CreatedOn;
                            _loansearch.AuditDueDate = AuditDueDate;
                            _loansearch.ModifiedOn = DateTime.Now;
                            _loansearch.LoanID = loan.LoanID;
                            _loansearch.Status = loan.Status;
                            db.Entry(_loansearch).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        tran.Commit();
                    }
                }
            }
            return loan;
        }

        public int GetAuditLoanMissingDocCount(Int64 loanid)
        {
            using (var db = new DBConnect(TableSchema))
            {
                List<AuditLoanMissingDoc> auditLoanMissingDocCount = db.AuditLoanMissingDoc.AsNoTracking().Where(x => x.LoanID == loanid).ToList();
                return auditLoanMissingDocCount.Count;
            }

        }
        public bool BoxFileUploadRetry(Int64 loanID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    var itemList = db.BoxDownloadQueue.Where(m => m.LoanID == loanID).ToList();
                    if (itemList == null)
                        throw new Exception("No Such item found or you don't have access to that item");

                    var updated = false;
                    foreach (var item in itemList)
                    {
                        if (item.Status == BoxDownloadStatusConstant.DOWNLOAD_FAILED)
                        {
                            item.Status = BoxDownloadStatusConstant.DOWNLOAD_PENDING;
                            item.ModifiedOn = DateTime.Now;
                            db.Entry(item).State = EntityState.Modified;
                            db.SaveChanges();
                            updated = true;
                        }
                        else
                        {
                            throw new Exception("Item is not in failed state");
                        }
                    }
                    if (updated)
                    {
                        var loan = db.Loan.Where(m => m.LoanID == loanID).FirstOrDefault();
                        loan.Status = StatusConstant.PENDING_BOX_DOWNLOAD;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    trans.Commit();

                    return true;
                }
            }
        }


        public bool AddBoxFileUploadDetails(Loan loan, List<BoxDownloadQueue> boxqList, DateTime AuditDueDate)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    WorkFlowStatusMaster isExiists = new IntellaLendDataAccess().GetWorkFlowMaster().Where(u => u.StatusID == StatusConstant.PENDING_BOX_DOWNLOAD).FirstOrDefault();
                    if (isExiists != null)
                    {

                        loan.SubStatus = 0;
                        loan.LoggedUserID = 0;
                        loan.FileName = string.Empty;
                        loan.CreatedOn = DateTime.Now;
                        loan.ModifiedOn = DateTime.Now;
                        loan.Status = isExiists.StatusID;
                        db.Loan.Add(loan);
                        db.SaveChanges();

                        IDCFields _idcObj = new IDCFields() { LoanID = loan.LoanID, Createdon = DateTime.Now, ModifiedOn = DateTime.Now, OCRAccuracyCalculated = false };
                        //  db.IDCFields.Add(_idcObj);
                        // db.SaveChanges();

                        //   LoanAudit.InsertLoanIDCFieldAudit(db, _idcObj, "Loan Uploaded", "");

                        foreach (var boxq in boxqList)
                        {
                            boxq.LoanID = loan.LoanID;
                            boxq.UserID = loan.UploadedUserID;
                            boxq.Status = 0;
                            boxq.ErrorMsg = string.Empty;
                            boxq.CreatedOn = DateTime.Now;
                            boxq.ModifiedOn = DateTime.Now;
                            db.BoxDownloadQueue.Add(boxq);
                            db.SaveChanges();
                        }
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TableSchema, AuditConfigConstant.UPLOADED_FROM_BOX);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);

                        LoanSearch _loansearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == loan.LoanID).FirstOrDefault();
                        if (_loansearch == null)
                        {
                            _loansearch = new LoanSearch();
                            _loansearch.LoanTypeID = loan.LoanTypeID;
                            _loansearch.CustomerID = loan.CustomerID;
                            _loansearch.Status = loan.Status;
                            _loansearch.ReceivedDate = loan.CreatedOn;
                            _loansearch.ModifiedOn = DateTime.Now;
                            _loansearch.AuditDueDate = AuditDueDate;
                            _loansearch.CreatedOn = DateTime.Now;
                            _loansearch.LoanID = loan.LoanID;
                            db.LoanSearch.Add(_loansearch);
                            db.SaveChanges();
                        }
                        else
                        {
                            _loansearch.AuditDueDate = AuditDueDate;
                            _loansearch.ModifiedOn = DateTime.Now;
                            _loansearch.LoanID = loan.LoanID;
                            db.Entry(_loansearch).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        tran.Commit();
                    }
                }
            }
            return true;
        }

        public object GetBoxUploadedItems(DateTime FromDate, DateTime ToDate, Int64 CurrentUserID, Int64 status, Int64 CustomerID)
        {

            using (var db = new DBConnect(TableSchema))
            {
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);
                ToDate = ToDate.AddDays(1);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day);

                List<BoxDownloadQueue> boxList = new List<BoxDownloadQueue>();

                Int64 subStatus = status;

                if (status > StatusConstant.MOVED_TO_IDC && status < StatusConstant.IDCERROR)
                    status = StatusConstant.PENDING_IDC;
                //status=-3---All
                boxList = db.BoxDownloadQueue.AsNoTracking().Where(search => search.CreatedOn >= FromDate && search.CreatedOn < ToDate &&
                (
                (status == -3 && (search.Status == BoxDownloadStatusConstant.DOWNLOAD_FAILED || search.Status == BoxDownloadStatusConstant.DOWNLOAD_PENDING || search.Status == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS || search.Status == StatusConstant.LOAN_DELETED || search.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE || search.Status == StatusConstant.LOS_LOAN_STAGED || search.Status == StatusConstant.COMPLETE)) ||
                (status == -2 && search.Status == BoxDownloadStatusConstant.DOWNLOAD_FAILED) ||
                (status == StatusConstant.READY_FOR_IDC && search.Status == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS) ||
                (status == StatusConstant.PENDING_BOX_DOWNLOAD && search.Status == BoxDownloadStatusConstant.DOWNLOAD_PENDING) ||
                (status == StatusConstant.PENDING_IDC && search.Status == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS) ||
                (status == StatusConstant.PENDING_AUDIT && search.Status == BoxDownloadStatusConstant.DOWNLOAD_SUCCESS) ||
                (status == StatusConstant.FAILED_BOX_DOWNLOAD && search.Status == BoxDownloadStatusConstant.DOWNLOAD_FAILED) ||
                (status == StatusConstant.LOAN_DELETED && search.Status == StatusConstant.LOAN_DELETED) ||
                (status == StatusConstant.MOVE_TO_PROCESSING_QUEUE && search.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE) ||
                (status == StatusConstant.LOS_LOAN_STAGED && search.Status == StatusConstant.LOS_LOAN_STAGED) ||
                (status == StatusConstant.COMPLETE && search.Status == StatusConstant.COMPLETE))
                ).ToList();


                var groupList = boxList.GroupBy(item => item.LoanID)
                     .Select(group => new { LoanID = group.Key, Items = group.ToList() })
                     .ToList();

                var boxErrorLoans = (from L in db.Loan.AsNoTracking()
                                     join search in db.BoxDownloadQueue.AsNoTracking() on L.LoanID equals search.LoanID
                                     where (L.CreatedOn >= FromDate && L.CreatedOn < ToDate) && (L.UploadType == UploadConstant.BOX) &&
                                     ((status == -3 || status == -2) && L.Status == StatusConstant.IDC_ERROR)
                                     select search).ToList().GroupBy(item => item.LoanID)
                                             .Select(group => new { LoanID = group.Key, Items = group.ToList() })
                                             .ToList();
                List<BoxMonitorReport> loanList = new List<BoxMonitorReport>();

                var newGroupList = groupList.Where(a => !boxErrorLoans.Any(m => m.LoanID == a.LoanID)).ToList();
                if (status != 12 && status != LOSImportStatusConstant.LOS_IMPORT_STAGED && status != LOSImportStatusConstant.LOS_IMPORT_FAILED)
                {
                    loanList = (from search in newGroupList.AsEnumerable()
                                join L in db.Loan.AsNoTracking() on search.LoanID equals L.LoanID
                                join CM in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CM.CustomerID
                                join RTM in db.ReviewTypeMaster.AsNoTracking() on L.ReviewTypeID equals RTM.ReviewTypeID
                                join U in db.Users.AsNoTracking() on L.AssignedUserID equals U.UserID

                                where (
                                        (status == -3 && (L.Status == StatusConstant.READY_FOR_IDC ||
                                          L.Status == StatusConstant.PENDING_IDC ||
                                           L.Status == StatusConstant.COMPLETE ||

                                          L.Status == StatusConstant.PENDING_AUDIT ||
                                          L.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE ||
                                          L.Status == StatusConstant.IDC_ERROR ||
                                          L.Status == StatusConstant.FAILED_BOX_DOWNLOAD ||
                                          L.Status == StatusConstant.PENDING_BOX_DOWNLOAD ||
                                          L.Status == StatusConstant.LOAN_DELETED)) ||
                                        (status == StatusConstant.READY_FOR_IDC && L.Status == StatusConstant.READY_FOR_IDC) ||
                                        (status == StatusConstant.PENDING_IDC && L.SubStatus == subStatus) ||
                                        (status == StatusConstant.COMPLETE && L.SubStatus == subStatus) ||
                                        (status == StatusConstant.COMPLETE && L.Status == StatusConstant.COMPLETE) ||

                                        (status == StatusConstant.PENDING_AUDIT && L.Status == StatusConstant.PENDING_AUDIT) ||
                                        (status == StatusConstant.FAILED_BOX_DOWNLOAD && L.Status == StatusConstant.FAILED_BOX_DOWNLOAD) ||
                                        (status == StatusConstant.PENDING_BOX_DOWNLOAD && L.Status == StatusConstant.PENDING_BOX_DOWNLOAD) ||
                                         (status == StatusConstant.PENDING_ENCOMPASS_DOWNLOAD && L.Status == StatusConstant.PENDING_ENCOMPASS_DOWNLOAD) ||
                                         (status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD && L.Status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD) ||
                                                                           (status == StatusConstant.LOAN_DELETED && L.Status == StatusConstant.LOAN_DELETED) ||
                                            (status == StatusConstant.MOVE_TO_PROCESSING_QUEUE && L.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE) ||
                                        (status == -2 && L.Status == StatusConstant.IDC_ERROR)
                                    ) && (CustomerID == 0 || L.CustomerID == CustomerID)
                                select new BoxMonitorReport()
                                {
                                    LoanID = search.LoanID,
                                    Customer = CM.CustomerName,
                                    CustomerID = CM.CustomerID,
                                    LoanType = L.LoanTypeID,
                                    UserName = U.UserName,
                                    ReviewType = RTM.ReviewTypeName,
                                    ReviewTypeID = RTM.ReviewTypeID,
                                    BoxFileName = string.Join("|", search.Items.Select(x => x.BoxFilePath + "/" + x.BoxFileName)),
                                    Status = (Int64)(search.Items[0].Status == 2 ? -2 : L.Status),
                                    SubStatus = L.SubStatus,
                                    ErrorMsg = search.Items[0].ErrorMsg,
                                    Uploaded = L.CreatedOn,
                                    Priority = search.Items[0].Priority,
                                    UploadType = L.UploadType,
                                    LastName = U.LastName,
                                    FirstName = U.FirstName,
                                    LoanNumber = L.LoanNumber,
                                    LoanTypeName = "",
                                    StatusDescription = "",
                                    LoanAmount = "",
                                    AuditMonthYearDate = L.AuditMonthYear,
                                    BorrowerName = "",
                                    AuditMonthYear = "",
                                    AssignedUserID = L.AssignedUserID,
                                    EDownloadID = 0
                                }).Distinct().Union(from L in db.Loan.AsNoTracking()
                                                    join CM in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CM.CustomerID
                                                    join RTM in db.ReviewTypeMaster.AsNoTracking() on L.ReviewTypeID equals RTM.ReviewTypeID
                                                    join U in db.Users.AsNoTracking() on L.UploadedUserID equals U.UserID

                                                    where (L.CreatedOn >= FromDate && L.CreatedOn < ToDate) && (L.UploadType == UploadConstant.ADHOC || L.UploadType == UploadConstant.ENCOMPASS || L.UploadType == UploadConstant.UNC || L.UploadType == UploadConstant.LOS) &&
                                                    (
                                                     (status == -3 && (L.Status == StatusConstant.IDC_ERROR || L.Status == StatusConstant.READY_FOR_IDC || L.Status == StatusConstant.COMPLETE || L.Status == StatusConstant.PENDING_IDC || L.Status == StatusConstant.PENDING_AUDIT || L.Status == StatusConstant.LOAN_DELETED || L.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE ||

                                                     L.Status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD || L.Status == StatusConstant.PENDING_ENCOMPASS_DOWNLOAD || L.Status == StatusConstant.LOS_LOAN_STAGED)) ||

                                                     (status == StatusConstant.READY_FOR_IDC && L.Status == StatusConstant.READY_FOR_IDC) ||
                                                          (status == StatusConstant.COMPLETE && L.Status == StatusConstant.COMPLETE) ||

                                                    (status == StatusConstant.PENDING_IDC && L.SubStatus == subStatus) ||
                                                     (status == StatusConstant.PENDING_AUDIT && L.Status == StatusConstant.PENDING_AUDIT) ||
                                                      (status == StatusConstant.MOVE_TO_PROCESSING_QUEUE && L.Status == StatusConstant.MOVE_TO_PROCESSING_QUEUE) ||
                                                     (status == StatusConstant.LOAN_DELETED && L.Status == StatusConstant.LOAN_DELETED) ||
                                                             (status == StatusConstant.PENDING_ENCOMPASS_DOWNLOAD && L.Status == StatusConstant.PENDING_ENCOMPASS_DOWNLOAD) ||
                                                            (status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD && L.Status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD) ||

                                                            (status == -2 && L.Status == StatusConstant.IDC_ERROR) ||
                                                            (status == StatusConstant.LOS_LOAN_STAGED && L.Status == StatusConstant.LOS_LOAN_STAGED)
                                                    ) && (CustomerID == 0 || L.CustomerID == CustomerID)
                                                    select new BoxMonitorReport()
                                                    {
                                                        LoanID = L.LoanID,
                                                        Customer = CM.CustomerName,
                                                        CustomerID = CM.CustomerID,
                                                        LoanType = L.LoanTypeID,
                                                        UserName = U.UserName,
                                                        ReviewType = RTM.ReviewTypeName,
                                                        ReviewTypeID = RTM.ReviewTypeID,
                                                        BoxFileName = L.FileName,
                                                        Status = L.Status,
                                                        SubStatus = L.SubStatus,
                                                        ErrorMsg = "",
                                                        Uploaded = L.CreatedOn,
                                                        Priority = L.Priority,
                                                        UploadType = L.UploadType,
                                                        LastName = U.LastName,
                                                        FirstName = U.FirstName,
                                                        LoanNumber = L.LoanNumber,
                                                        LoanTypeName = "",
                                                        StatusDescription = "",
                                                        LoanAmount = "",
                                                        AuditMonthYear = "",
                                                        BorrowerName = "",
                                                        AuditMonthYearDate = L.AuditMonthYear,
                                                        AssignedUserID = L.AssignedUserID,
                                                        EDownloadID = 0
                                                    }).Distinct().Union(from search in boxErrorLoans.AsEnumerable()
                                                                        join L in db.Loan.AsNoTracking() on search.LoanID equals L.LoanID
                                                                        join CM in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CM.CustomerID
                                                                        join RTM in db.ReviewTypeMaster.AsNoTracking() on L.ReviewTypeID equals RTM.ReviewTypeID
                                                                        join U in db.Users.AsNoTracking() on L.UploadedUserID equals U.UserID
                                                                        where (CustomerID == 0 || L.CustomerID == CustomerID)
                                                                        select new BoxMonitorReport()
                                                                        {
                                                                            LoanID = search.LoanID,
                                                                            Customer = CM.CustomerName,
                                                                            CustomerID = CM.CustomerID,
                                                                            LoanType = L.LoanTypeID,
                                                                            UserName = U.UserName,
                                                                            ReviewType = RTM.ReviewTypeName,
                                                                            ReviewTypeID = RTM.ReviewTypeID,
                                                                            BoxFileName = string.Join("|", search.Items.Select(x => x.BoxFilePath + "/" + x.BoxFileName)),
                                                                            Status = (Int64)(search.Items[0].Status == 2 ? -2 : L.Status),
                                                                            SubStatus = L.SubStatus,
                                                                            ErrorMsg = search.Items[0].ErrorMsg,
                                                                            Uploaded = L.CreatedOn,
                                                                            Priority = search.Items[0].Priority,
                                                                            UploadType = L.UploadType,
                                                                            LastName = U.FirstName,
                                                                            FirstName = U.LastName,
                                                                            LoanNumber = L.LoanNumber,
                                                                            LoanTypeName = "",
                                                                            StatusDescription = "",
                                                                            LoanAmount = "",
                                                                            AuditMonthYearDate = L.AuditMonthYear,
                                                                            BorrowerName = "",
                                                                            AuditMonthYear = "",
                                                                            AssignedUserID = L.AssignedUserID,
                                                                            EDownloadID = 0
                                                                        }).ToList().OrderByDescending(o => o.UploadType).ThenBy(o => o.LoanID).ToList().Distinct().ToList();


                    foreach (BoxMonitorReport item in loanList)
                    {
                        ELoanAttachmentDownload Eloan = db.ELoanAttachmentDownload.AsNoTracking().Where(l => l.LoanID == item.LoanID && l.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan).FirstOrDefault();

                        if (Eloan != null)
                        {
                            item.ErrorMsg = Eloan.Error;
                            item.EDownloadID = Eloan.ID;
                        }

                        LoanSearch _search = db.LoanSearch.AsNoTracking().Where(lt => lt.LoanID == item.LoanID).FirstOrDefault();
                        item.LoanTypeName = db.LoanTypeMaster.AsNoTracking().Where(lt => lt.LoanTypeID == item.LoanType).FirstOrDefault() != null ? db.LoanTypeMaster.AsNoTracking().Where(lt => lt.LoanTypeID == item.LoanType).FirstOrDefault().LoanTypeName : "";
                        item.StatusDescription = StatusConstant.GetStatusDescription(item.Status);
                        if (_search != null)
                        {
                            item.LoanNumber = String.IsNullOrEmpty(_search.LoanNumber) ? string.Empty : _search.LoanNumber;
                            item.BorrowerName = String.IsNullOrEmpty(_search.BorrowerName) ? string.Empty : _search.BorrowerName;
                            item.LoanAmount = _search.LoanAmount.ToString();
                        }
                        item.AuditMonthYear = item.AuditMonthYearDate != null ? item.AuditMonthYearDate.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.AuditMonthYearDate.Year.ToString() : "";
                        IDCFields _IDCFields = db.IDCFields.AsNoTracking().Where(x => x.LoanID == item.LoanID).FirstOrDefault();
                        item.EphesoftBatchInstanceID = (_IDCFields != null && !string.IsNullOrEmpty(_IDCFields.IDCBatchInstanceID)) ? _IDCFields.IDCBatchInstanceID : "";
                        if (item.Status == StatusConstant.PENDING_AUDIT)
                        {
                            List<AuditLoanMissingDoc> loanDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(a => a.LoanID == item.LoanID &&
                            (a.Status == StatusConstant.MOVED_TO_IDC ||
                            a.Status == StatusConstant.CLASSIFICATION_WAITING ||
                            a.Status == StatusConstant.FIELD_EXTRACTION_WAITING ||
                            a.Status == StatusConstant.IDCERROR ||
                            a.Status == StatusConstant.RUNNING ||
                            a.Status == StatusConstant.IDC_DELETED

                         )).ToList();

                            List<ELoanAttachmentDownload> _docs = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.LoanID == item.LoanID && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.TrailingDocuments && x.Status == EncompassStatusConstant.DOWNLOAD_FAILED).ToList();

                            item.IsMissingDocAvailable = loanDoc.Count > 0 || _docs.Count > 0;
                        }
                    }
                }
                //Get Encompass Failed Loans
                if (status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD || status == -3)
                {
                    List<ELoanAttachmentDownload> _failedEDownload = db.ELoanAttachmentDownload.AsNoTracking().Where(x => x.LoanID == 0 && x.TypeOfDownload == EncompassLoanAttachmentDownloadConstant.Loan && x.Status == EncompassStatusConstant.DOWNLOAD_FAILED).ToList();

                    foreach (ELoanAttachmentDownload item in _failedEDownload)
                    {
                        User _user = db.Users.AsNoTracking().Where(x => x.UserID == 1).FirstOrDefault();
                        BoxMonitorReport data = new BoxMonitorReport()
                        {
                            LoanID = 0,
                            AssignedUserID = 0,
                            AuditMonthYear = item.CreatedOn.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.CreatedOn.Year.ToString(),
                            AuditMonthYearDate = item.CreatedOn,
                            BorrowerName = string.Empty,
                            BoxFileName = string.Empty,
                            Customer = string.Empty,
                            CustomerID = 0,
                            EphesoftBatchInstanceID = string.Empty,
                            ErrorMsg = item.Error,
                            FirstName = _user != null ? _user.FirstName : string.Empty,
                            IsMissingDocAvailable = false,
                            LastName = _user != null ? _user.LastName : string.Empty,
                            LoanAmount = "",
                            LoanNumber = item.ELoanNumber,
                            LoanType = 0,
                            LoanTypeName = "",
                            Priority = 0,
                            ReviewType = "",
                            ReviewTypeID = 0,
                            Status = StatusConstant.FAILED_ENCOMPASS_DOWNLOAD,
                            StatusDescription = StatusConstant.GetStatusDescription(StatusConstant.FAILED_ENCOMPASS_DOWNLOAD),
                            SubStatus = 0,
                            Uploaded = item.CreatedOn,
                            UploadType = UploadConstant.ENCOMPASS,
                            UserName = _user != null ? _user.UserName : string.Empty,
                            EDownloadID = item.ID
                        };
                        loanList.Add(data);
                    }
                }

                //Get LOS Failed Loans
                if (status == LOSImportStatusConstant.LOS_IMPORT_FAILED || status == -3)
                {
                    List<LOSImportStaging> _failedEDownload = db.LOSImportStaging.AsNoTracking().Where(x => x.LoanID == 0 && (x.Status == LOSImportStatusConstant.LOS_IMPORT_FAILED || x.ValidJson == false) && x.Createdon >= FromDate && x.Createdon < ToDate).ToList();

                    foreach (LOSImportStaging item in _failedEDownload)
                    {
                        User _user = db.Users.AsNoTracking().Where(x => x.UserID == 1).FirstOrDefault();
                        LoanTypeMaster LoanType = db.LoanTypeMaster.AsNoTracking().Where(x => x.LoanTypeID == item.LoanTypeID).FirstOrDefault();
                        ReviewTypeMaster ReviwType = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == item.ReviewTypeID).FirstOrDefault();
                        CustomerMaster customer = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == item.CustomerID).FirstOrDefault();
                        BoxMonitorReport data = new BoxMonitorReport()
                        {
                            LOSImportID = item.ID,
                            LoanID = 0,
                            AssignedUserID = 0,
                            AuditMonthYear = item.Createdon.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.Createdon.Year.ToString(),
                            AuditMonthYearDate = item.Createdon,
                            BorrowerName = string.Empty,
                            BoxFileName = Path.GetFileName(item.FileName),
                            Customer = customer != null ? customer.CustomerName : string.Empty,
                            CustomerID = customer != null ? customer.CustomerID : 0,
                            EphesoftBatchInstanceID = string.Empty,
                            ErrorMsg = item.ErrorMsg,
                            FirstName = _user != null ? _user.FirstName : string.Empty,
                            IsMissingDocAvailable = false,
                            LastName = _user != null ? _user.LastName : string.Empty,
                            LoanAmount = "",
                            LoanNumber = "",
                            LoanType = LoanType != null ? LoanType.LoanTypeID : 0,
                            LoanTypeName = LoanType != null ? LoanType.LoanTypeName : string.Empty,
                            Priority = ReviwType != null ? ReviwType.ReviewTypePriority : 0,
                            ReviewType = ReviwType != null ? ReviwType.ReviewTypeName : string.Empty,
                            ReviewTypeID = ReviwType != null ? ReviwType.ReviewTypeID : 0,
                            Status = LOSImportStatusConstant.LOS_IMPORT_FAILED,
                            StatusDescription = LOSImportStatusConstant.GetStatusDescription(LOSImportStatusConstant.LOS_IMPORT_FAILED),
                            SubStatus = 0,
                            Uploaded = item.Createdon,
                            UploadType = UploadConstant.LOS,
                            UserName = _user != null ? _user.UserName : string.Empty,
                            EDownloadID = 0
                        };
                        loanList.Add(data);
                    }
                }
                //Get Los Processing Loans
                if (status == 12 || status == -3)
                {
                    List<LOSImportStaging> _losProcessing = db.LOSImportStaging.AsNoTracking().Where(x => x.LoanID == 0 && x.Status == LOSImportStatusConstant.LOS_IMPORT_PROCESSING && x.Createdon >= FromDate && x.Createdon < ToDate).ToList();

                    foreach (LOSImportStaging item in _losProcessing)
                    {
                        User _user = db.Users.AsNoTracking().Where(x => x.UserID == 1).FirstOrDefault();
                        LoanTypeMaster LoanType = db.LoanTypeMaster.AsNoTracking().Where(x => x.LoanTypeID == item.LoanTypeID).FirstOrDefault();
                        ReviewTypeMaster ReviwType = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == item.ReviewTypeID).FirstOrDefault();
                        CustomerMaster customer = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == item.CustomerID).FirstOrDefault();
                        BoxMonitorReport data = new BoxMonitorReport()
                        {
                            LOSImportID = item.ID,
                            LoanID = 0,
                            AssignedUserID = 0,
                            AuditMonthYear = item.Createdon.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.Createdon.Year.ToString(),
                            AuditMonthYearDate = item.Createdon,
                            BorrowerName = string.Empty,
                            BoxFileName = Path.GetFileName(item.FileName),
                            Customer = customer != null ? customer.CustomerName : string.Empty,
                            CustomerID = customer != null ? customer.CustomerID : 0,
                            EphesoftBatchInstanceID = string.Empty,
                            ErrorMsg = item.ErrorMsg,
                            FirstName = _user != null ? _user.FirstName : string.Empty,
                            IsMissingDocAvailable = false,
                            LastName = _user != null ? _user.LastName : string.Empty,
                            LoanAmount = "",
                            LoanNumber = "",
                            LoanType = LoanType != null ? LoanType.LoanTypeID : 0,
                            LoanTypeName = LoanType != null ? LoanType.LoanTypeName : string.Empty,
                            Priority = ReviwType != null ? ReviwType.ReviewTypePriority : 0,
                            ReviewType = ReviwType != null ? ReviwType.ReviewTypeName : string.Empty,
                            ReviewTypeID = ReviwType != null ? ReviwType.ReviewTypeID : 0,
                            Status = LOSImportStatusConstant.LOS_IMPORT_PROCESSING,
                            StatusDescription = LOSImportStatusConstant.GetStatusDescription(LOSImportStatusConstant.LOS_IMPORT_PROCESSING),
                            SubStatus = 0,
                            Uploaded = item.Createdon,
                            UploadType = UploadConstant.LOS,
                            UserName = _user != null ? _user.UserName : string.Empty,
                            EDownloadID = 0
                        };
                        loanList.Add(data);
                    }
                }
                //Get Los Staged Loans
                if (status == LOSImportStatusConstant.LOS_IMPORT_STAGED || status == -3)
                {
                    List<LOSImportStaging> _losProcessing = db.LOSImportStaging.AsNoTracking().Where(x => x.LoanID == 0 && x.Status == LOSImportStatusConstant.LOS_IMPORT_STAGED && x.Createdon >= FromDate && x.Createdon < ToDate).ToList();

                    foreach (LOSImportStaging item in _losProcessing)
                    {
                        User _user = db.Users.AsNoTracking().Where(x => x.UserID == 1).FirstOrDefault();
                        LoanTypeMaster LoanType = db.LoanTypeMaster.AsNoTracking().Where(x => x.LoanTypeID == item.LoanTypeID).FirstOrDefault();
                        ReviewTypeMaster ReviwType = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == item.ReviewTypeID).FirstOrDefault();
                        CustomerMaster customer = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == item.LoanTypeID).FirstOrDefault();
                        BoxMonitorReport data = new BoxMonitorReport()
                        {
                            LOSImportID = item.ID,
                            LoanID = 0,
                            AssignedUserID = 0,
                            AuditMonthYear = item.Createdon.ToString("MMMM", CultureInfo.InvariantCulture) + " " + item.Createdon.Year.ToString(),
                            AuditMonthYearDate = item.Createdon,
                            BorrowerName = string.Empty,
                            BoxFileName = Path.GetFileName(item.FileName),
                            Customer = customer != null ? customer.CustomerName : string.Empty,
                            CustomerID = customer != null ? customer.CustomerID : 0,
                            EphesoftBatchInstanceID = string.Empty,
                            ErrorMsg = item.ErrorMsg,
                            FirstName = _user != null ? _user.FirstName : string.Empty,
                            IsMissingDocAvailable = false,
                            LastName = _user != null ? _user.LastName : string.Empty,
                            LoanAmount = "",
                            LoanNumber = "",
                            LoanType = LoanType != null ? LoanType.LoanTypeID : 0,
                            LoanTypeName = LoanType != null ? LoanType.LoanTypeName : string.Empty,
                            Priority = ReviwType != null ? ReviwType.ReviewTypePriority : 0,
                            ReviewType = ReviwType != null ? ReviwType.ReviewTypeName : string.Empty,
                            ReviewTypeID = ReviwType != null ? ReviwType.ReviewTypeID : 0,
                            Status = LOSImportStatusConstant.LOS_IMPORT_STAGED,
                            StatusDescription = LOSImportStatusConstant.GetStatusDescription(LOSImportStatusConstant.LOS_IMPORT_STAGED),
                            SubStatus = 0,
                            Uploaded = item.Createdon,
                            UploadType = UploadConstant.LOS,
                            UserName = _user != null ? _user.UserName : string.Empty,
                            EDownloadID = 0
                        };
                        loanList.Add(data);
                    }
                }
                foreach (BoxMonitorReport _missingDoc in loanList)
                {
                    List<AuditLoanMissingDoc> loanDoc = db.AuditLoanMissingDoc.AsNoTracking().Where(a => a.LoanID == _missingDoc.LoanID &&
                    (a.Status == StatusConstant.MOVED_TO_IDC ||
                    a.Status == StatusConstant.CLASSIFICATION_WAITING ||
                    a.Status == StatusConstant.FIELD_EXTRACTION_WAITING ||
                    a.Status == StatusConstant.IDCERROR ||
                    a.Status == StatusConstant.RUNNING ||
                    a.Status == StatusConstant.IDC_DELETED
                )).ToList();

                    _missingDoc.IsMissingDocAvailable = loanDoc.Count > 0;
                }

                foreach (BoxMonitorReport item in loanList)
                {

                    if (item.UploadType == UploadConstant.LOS)
                    {
                        string FileName = string.Empty;
                        if (item.LoanID > 0)
                        {
                            FileName = db.LOSImportStaging.AsNoTracking().Where(l => l.LoanID == item.LoanID).OrderBy(x => x.Createdon).FirstOrDefault().FileName;
                        }
                        else
                        {
                            FileName = db.LOSImportStaging.AsNoTracking().Where(l => l.ID == item.LOSImportID).OrderBy(x => x.Createdon).FirstOrDefault().FileName;
                        }
                        FileName = Path.GetFileNameWithoutExtension(FileName);
                        item.BoxFileName = FileName + ".json";

                    }
                }
                var loans = loanList.OrderByDescending(o => o.Uploaded).ToList();
                return loans;
            }

        }
        public string GetErrorMessage(Int64 status, Int64 LoanID)
        {
            string errorMsg = string.Empty;
            using (var db = new DBConnect(TableSchema))
            {
                if (status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD)
                {
                    errorMsg = db.ELoanAttachmentDownload.AsNoTracking().Where(l => l.LoanID == LoanID && l.Status == StatusConstant.FAILED_ENCOMPASS_DOWNLOAD).FirstOrDefault().Error.ToString();

                }


            }
            return errorMsg;
        }
        public List<BoxDuplicatedFilesFolder> GetBoxDuplicateUploadFiles(long customerID, long reviewType, List<BoxItem> boxItems, Int64 UserID, string FileFilter)
        {
            using (var db = new DBConnect(TableSchema))
            {
                List<BoxDuplicatedFilesFolder> _fileandFolder = new List<BoxDuplicatedFilesFolder>();
                List<BoxFolderFileDuplicates> fileDuplicates = new List<BoxFolderFileDuplicates>();
                List<BoxFolderFileDuplicates> _folderFileDuplicates = new List<BoxFolderFileDuplicates>();
                foreach (BoxItem boxItem in boxItems)
                {
                    List<BoxFolderFileDuplicates> tempFileDupFiles = new List<BoxFolderFileDuplicates>();
                    List<BoxFolderFileDuplicates> tempFolderDupFiles = new List<BoxFolderFileDuplicates>();
                    if (boxItem.ItemType == "folder")
                    {
                        BoxCollection collection = new BoxCollection();
                        BoxAPIWrapper wrap = new BoxAPIWrapper(TableSchema, UserID);
                        BoxCollection colDetails = new BoxAPIWrapper(TableSchema, UserID).GetFolderDetails(boxItem.BoxID, 1, 0, FileFilter);
                        for (int offset = 0; offset < colDetails.TotalCount; offset = offset + 500)
                        {
                            collection = new BoxAPIWrapper(TableSchema, UserID).GetFolderDetails(boxItem.BoxID, 500, offset, FileFilter);
                        }
                        string FolderPath = collection.BoxEntities[0].ParentPath;
                        tempFolderDupFiles = (from L in db.Loan.AsNoTracking()
                                              join bd in db.BoxDownloadQueue on L.LoanID equals bd.LoanID
                                              join us in db.Users on L.UploadedUserID equals us.UserID
                                              where (L.CustomerID == customerID && L.ReviewTypeID == reviewType && L.Status != (StatusConstant.LOAN_DELETED) && bd.BoxFilePath.Equals(FolderPath))
                                              select new BoxFolderFileDuplicates()
                                              {

                                                  FolderID = boxItem.BoxID,
                                                  Id = bd.BoxEntityID,
                                                  FileType = "folder",
                                                  FileName = bd.BoxFileName,
                                                  FilePath = bd.BoxFilePath,
                                                  UserName = us.LastName + " " + us.FirstName,
                                                  CreatedDate = EntityFunctions.TruncateTime(L.CreatedOn),
                                                  Priority = boxItem.Priority
                                              }).Distinct().ToList();
                    }
                    else
                    {
                        tempFileDupFiles = (from L in db.Loan.AsNoTracking()
                                            join bd in db.BoxDownloadQueue.AsNoTracking() on L.LoanID equals bd.LoanID
                                            join us in db.Users on L.UploadedUserID equals us.UserID
                                            where (L.CustomerID == customerID && L.ReviewTypeID == reviewType && L.Status != (StatusConstant.LOAN_DELETED) && bd.BoxEntityID == boxItem.BoxID)
                                            select new BoxFolderFileDuplicates()
                                            {
                                                Id = bd.BoxEntityID,
                                                FileName = bd.BoxFileName,
                                                FileType = "file",
                                                FilePath = bd.BoxFilePath,
                                                UserName = us.LastName + " " + us.FirstName,
                                                CreatedDate = EntityFunctions.TruncateTime(L.CreatedOn),
                                                Priority = boxItem.Priority

                                            }).Distinct().ToList();
                    }
                    fileDuplicates.AddRange(tempFileDupFiles);
                    _folderFileDuplicates.AddRange(tempFolderDupFiles);
                }

                if (_folderFileDuplicates != null || fileDuplicates != null)
                {
                    _fileandFolder.Add(new BoxDuplicatedFilesFolder()
                    {
                        FolderFilesCount = _folderFileDuplicates,
                        FilesExistsCount = fileDuplicates
                    });
                }
                return _fileandFolder;
            }
        }

        //private BoxDownloadQueue GetBoxDupLicateFilesFromBoxDownloadQueue(DBConnect db, Loan items, List<BoxItem> boxItems)
        //{
        //    BoxDownloadQueue _bxDFiles = new BoxDownloadQueue();
        //    foreach (BoxItem item in boxItems)
        //    {
        //        _bxDFiles = db.BoxDownloadQueue.AsNoTracking().Where(b => b.LoanID == items.LoanID && b.BoxEntityID == item.BoxID).FirstOrDefault();
        //    }
        //    return _bxDFiles;
        //}

        public void DeleteFileDetails(Loan loan)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    db.Loan.Remove(loan);
                    db.SaveChanges();
                    tran.Commit();
                }
            }
        }

        public void MissingDocFileUpload(Dictionary<string, object> AuditLoan)
        {
            using (var db = new DBConnect(TableSchema))
            {
                string[] auditDescs = AuditDataAccess.GetAuditDescription(TableSchema, AuditConfigConstant.MISSING_DOCUMENT_UPLOADED);

                LoanAudit.InsertLoanMissingDocAudit(db, AuditLoan, StatusConstant.MOVED_TO_IDC, auditDescs[0], auditDescs[1]);
            }
        }

        public Dictionary<string, string> GetEphesoftURL(string BatchID, string EphesoftURL, Int64 customerID)
        {

            //string Ephesoft_URL = string.Empty;
            Dictionary<string, string> ephesoftvalue = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(BatchID))
            {
                string sql = "select batch_status,curr_user from batch_instance where identifier='" + BatchID + "'";
                System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    string _curr_user = DBNull.Value.Equals(dt.Rows[0][1]) ? string.Empty : dt.Rows[0][1].ToString();
                    string _status = DBNull.Value.Equals(dt.Rows[0][0]) ? string.Empty : dt.Rows[0][0].ToString();
                    CustomerConfig _config = new TenantConfigDataAccess(TableSchema).GetConfigValues(customerID, EphesoftURL);
                    if (_config != null)
                    {
                        if ((_status.Equals("READY_FOR_REVIEW") || _status.Equals("READY_FOR_VALIDATION")) && (_curr_user.Equals("0") || _curr_user.Equals("")))
                        {
                            ephesoftvalue.Add("Ephesoft_URL", $"{_config.ConfigValue}/ReviewValidate.html?batch_id={BatchID}");
                            ephesoftvalue.Add("Ephesoft_Status", _status);
                            ephesoftvalue.Add("Ephesoft_lock_owner", "false");
                        }
                        else
                        {
                            ephesoftvalue.Add("Ephesoft_Status", _status);
                            ephesoftvalue.Add("Ephesoft_lock_owner", "true");
                        }
                    }
                }
            }

            return ephesoftvalue;

        }

        #endregion
    }

    public class BoxMonitorReport
    {
        public string UserName { get; set; }
        public long ReviewTypeID { get; set; }
        public long CustomerID { get; set; }
        public Int64 LoanID { get; set; }
        public string Customer { get; set; }
        public Int64 LoanType { get; set; }
        public string ReviewType { get; set; }
        public string BoxFileName { get; set; }
        public Int64 Status { get; set; }
        public int SubStatus { get; set; }
        public string ErrorMsg { get; set; }
        public DateTime? Uploaded { get; set; }
        public Int64? Priority { get; set; }
        public string EphesoftBatchInstanceID { get; set; }
        // public bool FromBox { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string LoanNumber { get; set; }
        public string LoanTypeName { get; set; }
        public string StatusDescription { get; set; }
        public string LoanAmount { get; set; }
        public string BorrowerName { get; set; }
        public string AuditMonthYear { get; set; }
        public DateTime AuditMonthYearDate { get; set; }
        public int UploadType { get; set; }
        public Int64 AssignedUserID { get; set; }
        public bool IsMissingDocAvailable { get; set; }
        public Int64 EDownloadID { get; set; }
        public Int64 LOSImportID { get; set; }
    }
}
