using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntellaLend.Constance;
using IntellaLend.Audit;
using System.Data.Entity;

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

        public Loan AddFileUploadDetails(Loan loan)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    WorkFlowStatusMaster isExiists = new IntellaLendDataAccess().GetWorkFlowMaster().Where(u => u.StatusID == StatusConstant.PENDING_IDC).FirstOrDefault();
                    if (isExiists != null)
                    {
                        loan.Status = isExiists.StatusID;
                        db.Loan.Add(loan);
                        db.SaveChanges();

                        LoanAudit.InsertLoanAudit(db, loan, "Uploaded from IntellaLend");

                        tran.Commit();
                    }
                }
            }
            return loan;
        }


        public bool BoxFileUploadRetry(Int64 UploadID, Int64 CurrentUserID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    var item = db.BoxDownloadQueue.Where(m => m.ID == UploadID && m.UserID== CurrentUserID).FirstOrDefault();
                    if (item == null)
                        throw new Exception("No Such item found or you don't have access to that item");

                    if (item.Status == BoxDownloadStatusConstant.DOWNLOAD_FAILED)
                    {
                        item.Status = BoxDownloadStatusConstant.DOWNLOAD_PENDING;
                        item.ModifiedOn = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        var loan = db.Loan.Where(m => m.LoanID == item.LoanID).FirstOrDefault();
                        loan.Status = StatusConstant.PENDING_BOX_DOWNLOAD;
                        loan.ModifiedOn = DateTime.Now;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        trans.Commit();

                    }
                    else
                    {
                        throw new Exception("Item is not in failed state");
                    }
                  
                    return true;
                }
            }
        }


        public bool AddBoxFileUploadDetails(Loan loan, List<BoxDownloadQueue> boxqList)
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

                        LoanAudit.InsertLoanAudit(db, loan, "Uploaded from Box");

                        tran.Commit();
                    }
                }
            }
            return true;
        }

        public object GetBoxUploadedItems(DateTime FromDate, DateTime ToDate, Int64 CurrentUserID,int status)
        {
            object loan;
            using (var db = new DBConnect(TableSchema))
            {                
                ToDate = ToDate.AddDays(1);
                loan = (from search in db.BoxDownloadQueue.AsNoTracking()
                        join L in db.Loan.AsNoTracking() on search.LoanID equals L.LoanID
                        join CM in db.CustomerMaster.AsNoTracking() on L.CustomerID equals CM.CustomerID
                        join LTM in db.LoanTypeMaster.AsNoTracking() on L.LoanTypeID equals LTM.LoanTypeID
                        join RTM in db.ReviewTypeMaster.AsNoTracking() on L.ReviewTypeID equals RTM.ReviewTypeID
                        where search.CreatedOn >= FromDate && search.CreatedOn < ToDate &&(status==-1 || search.Status==status) &&  (search.UserID==CurrentUserID)
                             select new
                             {
                                 UploadID= search.ID,
                                 LoanID = search.LoanID,
                                 Customer=CM.CustomerName,
                                 LoanType=LTM.LoanTypeName,
                                 ReviewType=RTM.ReviewTypeName,
                                 BoxFilePath=search.BoxFilePath,
                                 BoxFileName=search.BoxFileName,
                                 Status= search.Status,                                 
                                 ErrorMsg = search.ErrorMsg,
                                 BoxEntityID=search.BoxEntityID,
                                 Uploaded = search.CreatedOn,
                                 Priority= search.Priority

                             }).ToList();

            }
            return loan;
        }

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
                LoanAudit.InsertLoanMissingDocAudit(db, AuditLoan,StatusConstant.PENDING_OCR, "Missing Document Uploaded");
            }
        }

        #endregion
    }
}
