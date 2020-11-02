using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.WorkFlow
{
    public class WorkFlowBase : IWorkFlow
    {
        public virtual void SetWorkFlowState(ref Dictionary<string, string> wfValues)
        {
            if (wfValues.ContainsKey("TENANT_SCHEMA") && wfValues.ContainsKey("LOANID") && wfValues.ContainsKey("STATUS"))
            {
                using (var db = new DBConnect(wfValues["TENANT_SCHEMA"]))
                {
                    using (var tran = db.Database.BeginTransaction())
                    {

                        if (wfValues.ContainsKey("MISSINGDOCUMENT") && wfValues["MISSINGDOCUMENT"] == "False")
                        {

                            Int64 loanID = Convert.ToInt64(wfValues["LOANID"]);

                            Loan loan = db.Loan.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();

                            if (wfValues.ContainsKey("APPROVE") && wfValues["APPROVE"].Equals("Y"))
                            {
                                IDCFields _idcObj = db.IDCFields.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();

                                if (loan.QCIQStartDate != null)
                                {
                                    if (_idcObj == null)
                                    {
                                        _idcObj = new IDCFields() { LoanID = loan.LoanID, Createdon = DateTime.Now, ModifiedOn = DateTime.Now };
                                        _idcObj.IDCCompletionDate = DateTime.Now;
                                        db.IDCFields.Add(_idcObj);
                                    }
                                    else
                                    {
                                        _idcObj.IDCCompletionDate = DateTime.Now;
                                        _idcObj.ModifiedOn = DateTime.Now;
                                        db.Entry(_idcObj).State = EntityState.Modified;
                                    }

                                    db.SaveChanges();

                                    TimeSpan time = TimeSpan.FromMilliseconds(DateTime.Now.Subtract(loan.QCIQStartDate.Value).TotalMilliseconds);
                                    loan.LoanDuration = $"{time.Days.ToString("00")} Day(s), {time.Hours.ToString("00")}:{time.Minutes.ToString("00")}:{time.Seconds.ToString("00")}";
                                    loan.AuditCompletedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0,0 );
                                }
                            }

                            loan.Status = Convert.ToInt64(wfValues["STATUS"]);
                            if(loan.Status == StatusConstant.PENDING_AUDIT)
                            {
                                loan.SubStatus = StatusConstant.EXTRACTION_COMPLETED;
                            }
                            loan.ModifiedOn = DateTime.Now;
                            db.Entry(loan).State = EntityState.Modified;
                            db.SaveChanges();

                            string auditDesc = string.Empty, auditSysDesc = string.Empty;

                            if (wfValues.ContainsKey("APPROVE") && wfValues["APPROVE"].Equals("Y"))
                            {
                                string _userName = WorkFlowBaseDataAccess.GetUserName(db, loan.LastAccessedUserID);
                                string[] auditDescs = AuditDataAccess.GetAuditDescription(wfValues["TENANT_SCHEMA"], AuditConfigConstant.COMPLETED_BY);

                                auditDesc = auditDescs[0].Replace(AuditConfigConstant.USERNAME, _userName);
                                auditSysDesc = auditDescs[1].Replace(AuditConfigConstant.USERNAME, _userName);
                            }
                            else
                            {
                                string[] auditDescs = AuditDataAccess.GetAuditDescription(wfValues["TENANT_SCHEMA"], AuditConfigConstant.LOAN_MOVED_TO);
                                auditDesc = auditDescs[0].Replace(AuditConfigConstant.LOANSTATUS, StatusConstant.GetStatusDescription(Convert.ToInt64(wfValues["STATUS"])));
                                auditSysDesc = auditDescs[1].Replace(AuditConfigConstant.LOANSTATUS, StatusConstant.GetStatusDescription(Convert.ToInt64(wfValues["STATUS"])));
                            }


                            LoanAudit.InsertLoanAudit(db, loan, auditDesc, auditSysDesc);

                            LoanSearch loanSearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == loanID).FirstOrDefault();

                            if (loanSearch != null)
                            {
                                loanSearch.Status = Convert.ToInt64(wfValues["STATUS"]);
                                loanSearch.ModifiedOn = DateTime.Now;
                                db.Entry(loanSearch).State = EntityState.Modified;
                                db.SaveChanges();

                                string[] auditDescs = AuditDataAccess.GetAuditDescription(wfValues["TENANT_SCHEMA"], AuditConfigConstant.LOAN_STATUS_MODIFIED);

                                LoanAudit.InsertLoanSearchAudit(db, loanSearch, auditDescs[0], auditDescs[1]);
                            }
                        }

                        tran.Commit();
                    }
                }
            }
        }
    }
}
