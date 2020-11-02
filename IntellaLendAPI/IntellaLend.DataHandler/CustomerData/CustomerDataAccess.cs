using IntellaLend.Audit;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool AddCustomer(CustomerMaster customerMaster)
        {
            bool isCustomerNotExist;
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    if (!db.CustomerMaster.Any(x => x.CustomerName.Equals(customerMaster.CustomerName)))
                    {
                        db.CustomerMaster.Add(customerMaster);
                        db.SaveChanges();
                        trans.Commit();
                        isCustomerNotExist = true;
                    }
                    else
                    {
                        isCustomerNotExist = false;
                    }
                }
            }
            return isCustomerNotExist;
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
                        LoanAudit.InsertLoanSearchAudit(db, loanSearch, string.Format("Loan search Purged by user : {0}", username));
                        searchPurged = true;
                    }

                    if (loan != null)
                    {
                        loan.Status = StatusConstant.LOAN_PURGED;
                        db.Entry(loan).State = EntityState.Modified;
                        db.SaveChanges();
                        LoanAudit.InsertLoanAudit(db, loan, string.Format("Loan Purged by user : {0}", username));
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
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    CustomerMaster updateCustomer = db.CustomerMaster.AsNoTracking().Single(u => u.CustomerID == customerMaster.CustomerID);
                    customerMaster.CreatedOn = updateCustomer.CreatedOn;
                    customerMaster.ModifiedOn = DateTime.Now;
                    db.Entry(customerMaster).State = EntityState.Modified;
                    db.SaveChanges();
                    //db.Update(customerMaster);
                    //db.SaveChanges();
                    trans.Commit();
                }
            }
            return true;
        }

        #endregion

    }
}
