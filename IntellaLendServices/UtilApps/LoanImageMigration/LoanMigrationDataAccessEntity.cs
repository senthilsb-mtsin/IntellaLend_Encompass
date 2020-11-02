using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanImageMigration
{
    public class LoanMigrationDataAccessEntity
    {
        private static string TenantSchema = "T1";

        #region GET

        public static Loan GetMigrationLoan(Int64 _loanID)
        {            
            using (var db = new DBConnect(TenantSchema))
            {
                return (from l in db.Loan.AsNoTracking()
                             join ld in db.LoanDetail.AsNoTracking() on l.LoanID equals ld.LoanID
                             where l.LoanID == _loanID && l.LoanTypeID != 0 && l.Status != 2 && l.Status != 999 && l.LoanGUID == null
                        select l).FirstOrDefault();
            }
        }

        public static Int64 GetLoanID()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Loan.AsNoTracking().OrderByDescending(l => l.LoanID).FirstOrDefault().LoanID;
            }
        }

        public static List<Int64> GetLoanImageIDs(Int64 _loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return (from l in db.LoanImage.AsNoTracking()
                        where l.LoanID == _loanID
                        select l.LoanImageID).ToList<Int64>();
            }
        }

        public static LoanImage GetLoanImage(Int64 _loanImageID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.LoanImage.AsNoTracking().Where(l => l.LoanImageID == _loanImageID).FirstOrDefault();
            }
        }
        

        //public static List<LoanImage> GetLoanImages(Int64 _loanID)
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        return db.LoanImage.AsNoTracking().Where(l => l.LoanID == _loanID).ToList();
        //    }
        //}

        public static LoanPDF GetLoanPDF(Int64 _loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.LoanPDF.AsNoTracking().Where(l => l.LoanID == _loanID).FirstOrDefault();
            }
        }

        #endregion

        #region UPDATE

        public static void UpdateLoanGUID(Loan _loan)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                _loan.LoanGUID = Guid.NewGuid();
                db.Entry(_loan).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void GenerateLoanGUID()
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<Loan> _lsLoans = db.Loan.AsNoTracking().ToList();

                foreach (Loan _loan in _lsLoans)
                {
                    _loan.LoanGUID = Guid.NewGuid();
                    db.Entry(_loan).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }        

        public static void UpdateLoanImageGUID(LoanImage _loanImg)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                _loanImg.ImageGUID = Guid.NewGuid();
                db.Entry(_loanImg).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static void UpdateLoanPDFGUID(LoanPDF _loanPDF)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                _loanPDF.LoanPDFGUID = Guid.NewGuid();
                db.Entry(_loanPDF).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        

        #endregion
    }
}
