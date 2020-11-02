using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace IntellaLend.MinIOWrapper
{
    public class MinIOWrapperDataAccess
    {

        #region Private Variables

        private static string TenantSchema;
        public static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public MinIOWrapperDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public Loan GetLoanDetails(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.Loan.AsNoTracking().Where(a => a.LoanID == loanID).FirstOrDefault();
            }
        }

        public MinIOStorage GetLoanStackingOrderDetails(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return (from l in db.Loan.AsNoTracking()
                        join p in db.LoanPDF.AsNoTracking() on l.LoanID equals p.LoanID
                        where l.LoanID == loanID
                        select new MinIOStorage()
                        {
                            CreatedOn = l.CreatedOn,
                            LoanGUID = l.LoanGUID,
                            ObjectGUID = p.LoanPDFGUID
                        }).FirstOrDefault();
            }
        }

        public void InsertLoanImage(LoanImage _img)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                _img.ImageGUID = Guid.NewGuid();
                db.LoanImage.Add(_img);
                db.SaveChanges();
            }
        }

        public Guid InsertLoanPDF(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                Guid _pdfGUIDID = Guid.NewGuid();

                LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (loanPdf != null)
                {
                    loanPdf.LoanPDFGUID = _pdfGUIDID;
                    loanPdf.ModifiedOn = DateTime.Now;
                    db.Entry(loanPdf).State = EntityState.Modified;
                }
                else
                {
                    LoanPDF _pdf = new LoanPDF()
                    {
                        LoanID = LoanID,
                        LoanPDFPath = string.Empty,
                        LoanPDFGUID = _pdfGUIDID,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };
                    db.LoanPDF.Add(_pdf);
                }
                db.SaveChanges();
                return _pdfGUIDID;
            }
        }


        public void DeleteLoanImage(Int64 _imgID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanImage _img = db.LoanImage.AsNoTracking().Where(l => l.LoanImageID == _imgID).FirstOrDefault();
                if (_img != null)
                {
                    db.Entry(_img).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        public bool CheckBucket(DateTime? createdOn)
        {
            //using (var db = new DBConnect(TenantSchema))
            //{
            //    DateTime tempCreatedON = new DateTime(createdOn.Value.Year, createdOn.Value.Month, createdOn.Value.Day, 0, 0, 0);
            //    DateTime tempCreatedONTODate = tempCreatedON.AddDays(1);
            //    List<Loan> _loans = db.Loan.AsNoTracking().Where(l =>
            //    //l.Status != StatusConstant
            //    //l.CreatedOn > tempCreatedON && l.CreatedOn < tempCreatedONTODate).ToList();
            //    return _loans.Count == 0;
            //}
            return false;
        }


        public void DeleteLoanImage(Guid? _imgID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanImage _img = db.LoanImage.AsNoTracking().Where(l => l.ImageGUID == _imgID).FirstOrDefault();
                if (_img != null)
                {
                    db.Entry(_img).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        public void DeleteLoanPDF(Int64 loanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF _pdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();
                if (_pdf != null)
                {
                    db.Entry(_pdf).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }

        public Dictionary<string, string> GetTenantConfig()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            using (var db = new DBConnect(SystemSchema))
            {
                var tenant = db.TenantMaster.AsNoTracking().Where(X => X.TenantSchema == TenantSchema).FirstOrDefault();

                if (tenant != null)
                    config = tenant.TenantConfig;

            }

            return config;
        }


        #endregion
    }
}
