using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class CustomerDataAccess
    {
        private static string TableSchema;

        #region Constructor

        public CustomerDataAccess() { }
        public CustomerDataAccess(string tableschema) { TableSchema = tableschema; }

        #endregion

        #region Public Methods        

        public List<CustomerMaster> GetCustomerList(bool Active)
        {
            List<CustomerMaster> cusMasters;
            using (var db = new DBConnect(TableSchema))
            {
                if (Active)
                    cusMasters = db.CustomerMaster.Where(c => c.Active == Active).ToList();
                else
                    cusMasters = db.CustomerMaster.ToList();

                db.Dispose();
            }
            return cusMasters;
        }

        public object AddCustomer(CustomerMaster customerMaster)
        {
            bool Success = false, CustomerNameExist = false, CustomerCodeExist = false;
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CustomerNameExist = db.CustomerMaster.Any(x => x.CustomerName.Equals(customerMaster.CustomerName));
                    CustomerCodeExist = (!string.IsNullOrEmpty(customerMaster.CustomerCode.Trim()) && db.CustomerMaster.Any(x => x.CustomerCode.Equals(customerMaster.CustomerCode)));
                    if (!CustomerNameExist && !CustomerCodeExist)
                    {
                        db.CustomerMaster.Add(customerMaster);
                        db.SaveChanges();
                        trans.Commit();
                        Success = true;
                    }
                }
            }
            return new { Success, CustomerNameExist, CustomerCodeExist, CustomerID = customerMaster.CustomerID };
        }

        public bool DeleteLoanImages(Int64 loanid)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    List<LoanImage> lsImages = db.LoanImage.AsNoTracking().Where(li => li.LoanID == loanid).ToList();

                    foreach (LoanImage img in lsImages)
                    {
                        db.Entry(img).State = EntityState.Deleted;
                        db.SaveChanges();
                    }

                    trans.Commit();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteLoanPDF(Int64 loanid)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    LoanPDF loanPDF = db.LoanPDF.AsNoTracking().Where(lp => lp.LoanID == loanid).FirstOrDefault();

                    db.Entry(loanPDF).State = EntityState.Deleted;
                    db.SaveChanges();
                    trans.Commit();
                    return true;
                }
            }
            return false;
        }


        public bool RetentionPurgeStatusChange(Int64 loanID, long userID, string username)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    LoanSearch loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == loanID).FirstOrDefault();
                    Loan loan = db.Loan.AsNoTracking().Where(ls => ls.LoanID == loanID).FirstOrDefault();
                    bool searchPurged = false;
                    bool loanPurged = false;
                    if (loanSearch != null)
                    {
                        loanSearch.Status = StatusConstant.LOAN_PURGED;
                        db.Entry(loanSearch).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TableSchema, AuditConfigConstant.LOAN_SEARCH_PURGED_BY_USER);
                        LoanAudit.InsertLoanSearchAudit(db, loanSearch, auditDescs[0].Replace(AuditConfigConstant.USERNAME, username), auditDescs[1].Replace(AuditConfigConstant.USERNAME, username));
                        searchPurged = true;
                    }

                    if (loan != null)
                    {
                        loan.Status = StatusConstant.LOAN_PURGED;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TableSchema, AuditConfigConstant.LOAN_PURGED_BY_USER);
                        LoanAudit.InsertLoanAudit(db, loan, auditDescs[0].Replace(AuditConfigConstant.USERNAME, username), auditDescs[1].Replace(AuditConfigConstant.USERNAME, username));
                        loanPurged = true;
                    }

                    trans.Commit();

                    if (loanPurged && searchPurged)
                        return true;
                    else if (loanPurged)
                        return true;
                }
            }
            return false;
        }

        public object EditCustomer(CustomerMaster customerMaster)
        {
            bool Success = false, CustomerNameExist = false, CustomerCodeExist = false;
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {

                    CustomerNameExist = db.CustomerMaster.Any(x => !x.CustomerID.Equals(customerMaster.CustomerID) && x.CustomerName.Equals(customerMaster.CustomerName));
                    CustomerCodeExist = (!string.IsNullOrEmpty(customerMaster.CustomerCode.Trim()) && db.CustomerMaster.Any(x => !x.CustomerID.Equals(customerMaster.CustomerID) && x.CustomerCode.Equals(customerMaster.CustomerCode)));
                    if (!CustomerNameExist && !CustomerCodeExist)
                    {
                        CustomerMaster updateCustomer = db.CustomerMaster.AsNoTracking().Single(u => u.CustomerID == customerMaster.CustomerID);
                        customerMaster.CreatedOn = updateCustomer.CreatedOn;
                        customerMaster.ModifiedOn = DateTime.Now;
                        db.Entry(customerMaster).State = EntityState.Modified;
                        db.SaveChanges();
                        //db.Update(customerMaster);
                        //db.SaveChanges();
                        trans.Commit();
                        Success = true;
                    }
                }
            }
            return new { Success, CustomerNameExist, CustomerCodeExist };
        }

        public object GetCustomerImportStaging(Int32 Status, DateTime ImportDateFrom, DateTime ImportDateTo, Int64 AssignType)
        {
            ImportDateTo = ImportDateTo.AddDays(1);
            List<CustomerImportStaging> customerImportStagingList;
            using (var db = new DBConnect(TableSchema))
            {
                customerImportStagingList = db.CustomerImportStaging.AsNoTracking().Where(
                    x => (x.Status == Status || Status == 0) && (x.AssignType == AssignType)
                    && x.CreatedOn >= ImportDateFrom
                    && x.CreatedOn <= ImportDateTo
                    ).ToList();
            }
            return customerImportStagingList.OrderByDescending(x => x.ModifiedOn).ToList();
        }

        public object GetCustomerImportStagingDetails(Int64 CustomerImportStagingID)
        {
            List<CustomerImportStagingDetail> customerImportStagingDetailsList;
            using (var db = new DBConnect(TableSchema))
            {
                customerImportStagingDetailsList = db.CustomerImportStagingDetail.AsNoTracking().Where(
                    x => x.CustomerImportStagingID == CustomerImportStagingID
                    ).ToList();
            }
            return customerImportStagingDetailsList.OrderBy(x => x.CreatedOn).ToList();
        }
        #endregion

    }
}
