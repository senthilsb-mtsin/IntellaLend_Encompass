using EncompassConsoleConnector;
using IntellaLend.Audit;
using IntellaLend.AuditData;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.CheckRule
{
    public class CheckRulesDataAccess
    {
        private static string TenantSchema { get; set; }
        private static string SystemSchema = "IL";

        public CheckRulesDataAccess(string TableSchema)
        {
            TenantSchema = TableSchema;
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
                        _loanDetail.ModifiedOn = DateTime.Now;

                        db.Entry(_loanDetail).State = EntityState.Modified;
                        db.SaveChanges();

                        // string auditDesc = "Documents Count Updated";
                        string[] auditDescs = AuditDataAccess.GetAuditDescription(TenantSchema, AuditConfigConstant.DOCUMENTS_COUNT_UPDATED);

                        LoanAudit.InsertLoanDetailsAudit(db, _loanDetail, UserID, auditDescs[0], auditDescs[1]);
                    }

                    tran.Commit();
                }
            }

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

        public void InsertLoanCheckListAuditDetails(List<LoanChecklistAudit> loancheckList, Int64 LoanID)
        {
            if (loancheckList != null)
            {
                using (var db = new DBConnect(TenantSchema))
                {
                    Log($"Count : {loancheckList.Count.ToString()}");

                    db.LoanChecklistAudit.RemoveRange(db.LoanChecklistAudit.Where(x => x.LoanID == LoanID));
                    db.SaveChanges();

                    foreach (LoanChecklistAudit _loanchecklist in loancheckList)
                    {
                        db.LoanChecklistAudit.Add(_loanchecklist);
                        db.SaveChanges();
                    }
                }
            }
        }

        public object SendRuleFindingsEmail(string To, string Subject, string Attachements, string Body, Int64 UserID, string SendBy, Int64 LoanID, int TemplateID)
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
                    TemplateID = TemplateID
                });
                db.SaveChanges();
            }
            return data = new { Success = true };
        }

        public LoanLOSFields GetSendingEmailDetails(Int64 LoanID)
        {
            LoanLOSFields _loanLos = null;
            using (var db = new DBConnect(TenantSchema))
            {
                _loanLos = db.LoanLOSFields.AsNoTracking().Where(ls => ls.LoanID == LoanID).FirstOrDefault();
            }
            return _loanLos;
        }

        public void Log(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }
        public void SaveLoanObject(List<RuleFinding> _rulefinding)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanDetail loan = db.LoanDetail.AsNoTracking().FirstOrDefault();


                if (loan != null)
                {

                    Batch batch = JsonConvert.DeserializeObject<Batch>(loan.LoanObject);
                    batch.RuleFinding = _rulefinding;

                }

                db.SaveChanges();
            }

        }
    }
}
