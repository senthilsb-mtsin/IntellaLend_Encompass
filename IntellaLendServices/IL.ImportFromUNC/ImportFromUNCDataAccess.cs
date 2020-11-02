using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IL.ImportFromUNC
{
    public class ImportFromUNCDataAccess
    {
        public static string SystemSchema = "IL";
        public static string TenantSchema;

        public ImportFromUNCDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public static List<TenantMaster> GetAllTenants()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }

        public List<CustReviewLoanUploadPath> GetAllLoanPathDetails()
        {

            using (var db = new DBConnect(TenantSchema))
            {
                return db.CustReviewLoanUploadPath.AsNoTracking().Where(m => m.Active == true).ToList();

            }
        }

        public void DeleteLoan(Int64 _loanId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == _loanId).FirstOrDefault();
                    if (_loan != null)
                    {
                        db.Entry(_loan).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        AuditLoan _audit = db.AuditLoan.AsNoTracking().Where(l => l.LoanID == _loanId).FirstOrDefault();
                        if (_audit != null)
                        {
                            db.Entry(_audit).State = System.Data.Entity.EntityState.Deleted;
                            db.SaveChanges();
                        }

                    }

                    tran.Commit();
                }
            }
        }


        public Loan AddFileUploadDetails(Loan loan)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    loan.Status = StatusConstant.READY_FOR_IDC;
                    db.Loan.Add(loan);
                    db.SaveChanges();
                    string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.UPLOADED_FROM_INTELLALEND);

                    LoanAudit.InsertLoanAudit(db, loan, auditDescs[0], auditDescs[1]);

                    tran.Commit();
                }
            }
            return loan;
        }

        public Int64 GetPriority(Int64 reviewTypeId)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                var priority = db.ReviewTypeMaster.AsNoTracking().Where(t => t.ReviewTypeID == reviewTypeId).Select(t => t.ReviewTypePriority).FirstOrDefault();

                if (priority != null)
                    return Convert.ToInt64(priority);
                else
                    return 4;
            }                
        }
    }

}
