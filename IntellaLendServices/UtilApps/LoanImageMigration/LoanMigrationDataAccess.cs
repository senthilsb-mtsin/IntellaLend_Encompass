using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanImageMigration
{
    public class LoanMigrationDataAccess
    {
       // private static string TenantSchema = "T1";
        private DataAccess2 dataAccess;


        public LoanMigrationDataAccess()
        {
            dataAccess = new DataAccess2("AppConnectionName");
        }

        #region GET

        public Int64? GetMigrationLoan(Int64 _loanID)
        {            
            System.Data.DataTable dt = dataAccess.GetDataTable("GETLOAN", _loanID);

            if (dt.Rows.Count > 0)
                return _loanID;

            return null;


            //using (var db = new DBConnect(TenantSchema))
            //{
            //    return (from l in db.Loan.AsNoTracking()
            //                 join ld in db.LoanDetail.AsNoTracking() on l.LoanID equals ld.LoanID
            //                 where l.LoanID == _loanID && l.LoanTypeID != 0 && l.Status != 2 && l.Status != 999 
            //            select l).FirstOrDefault();
            //}
        }

        public Int64 GetLoanID()
        {
            System.Data.DataTable dt = dataAccess.GetDataTable("GETLASTLOANID");

            if (dt.Rows.Count > 0)
                return Convert.ToInt64(dt.Rows[0]["LoanID"]);

            return 0;
            //using (var db = new DBConnect(TenantSchema))
            //{
            //    return db.Loan.AsNoTracking().OrderByDescending(l => l.LoanID).FirstOrDefault().LoanID;
            //}
        }

        public List<Int64> GetLoanImageIDs(Int64 _loanID)
        {
            System.Data.DataTable dt = dataAccess.GetDataTable("GETLOANIMAGEID", _loanID);

            List<Int64> _ids = new List<long>();

            if (dt.Rows.Count > 0) {
                foreach (System.Data.DataRow item in dt.Rows)
                    _ids.Add(Convert.ToInt64(item["LoanImageID"]));               
            }

            return _ids;

            //using (var db = new DBConnect(TenantSchema))
            //{
            //    return (from l in db.LoanImage.AsNoTracking()
            //            where l.LoanID == _loanID
            //            select l.LoanImageID).ToList<Int64>();
            //}
        }

        public ImageObj GetLoanImage(Int64 _loanImageID)
        {
            System.Data.DataTable dt = dataAccess.GetDataTable("GETLOANIMAGE", _loanImageID);

            ImageObj _img = null;

            if (dt.Rows.Count > 0)
            {
                _img = new ImageObj();
                _img.ImageGuid = Guid.Parse(Convert.ToString(dt.Rows[0]["ImageGUID"]));
                _img.ImageBytes = (byte[])dt.Rows[0]["Image"];
            }

            return _img;

            //using (var db = new DBConnect(TenantSchema))
            //{
            //    return db.LoanImage.AsNoTracking().Where(l => l.LoanImageID == _loanImageID).FirstOrDefault();
            //}
        }
        

        //public static List<LoanImage> GetLoanImages(Int64 _loanID)
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        return db.LoanImage.AsNoTracking().Where(l => l.LoanID == _loanID).ToList();
        //    }
        //}

        public PDFObj GetLoanPDF(Int64 _loanID)
        {
            System.Data.DataTable dt = dataAccess.GetDataTable("GETLOANPDF", _loanID);

            PDFObj _pdf= null;

            if (dt.Rows.Count > 0)
            {
                _pdf = new PDFObj();
                _pdf.LoanPDFGUID = Guid.Parse(Convert.ToString(dt.Rows[0]["LoanPDFGUID"]));
                _pdf.LoanPDFPath = Convert.ToString(dt.Rows[0]["LoanPDFPath"]);
            }

            return _pdf;

            //using (var db = new DBConnect(TenantSchema))
            //{
            //    return db.LoanPDF.AsNoTracking().Where(l => l.LoanID == _loanID).FirstOrDefault();
            //}
        }

        #endregion

        #region UPDATE

        public string UpdateLoanGUID(Int64 _loanID)
        {
            System.Data.DataTable dt = dataAccess.GetDataTable("UPDATELOANGUID", _loanID);

            if (dt.Rows.Count > 0)
                return Convert.ToString(dt.Rows[0]["LoanGUID"]);

            return "";

            //using (var db = new DBConnect(TenantSchema))
            //{
            //    _loan.LoanGUID = Guid.NewGuid();
            //    db.Entry(_loan).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
        }

        //public static void GenerateLoanGUID()
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        List<Loan> _lsLoans = db.Loan.AsNoTracking().ToList();

        //        foreach (Loan _loan in _lsLoans)
        //        {
        //            _loan.LoanGUID = Guid.NewGuid();
        //            db.Entry(_loan).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //    }
        //}        

        //public static void UpdateLoanImageGUID(LoanImage _loanImg)
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        _loanImg.ImageGUID = Guid.NewGuid();
        //        db.Entry(_loanImg).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}

        //public static void UpdateLoanPDFGUID(LoanPDF _loanPDF)
        //{
        //    using (var db = new DBConnect(TenantSchema))
        //    {
        //        _loanPDF.LoanPDFGUID = Guid.NewGuid();
        //        db.Entry(_loanPDF).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //}


        #endregion
    }
}
