using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLend.RuleEngine;
using IntellaLend.WorkFlow;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntellaLend.CheckRule
{
    public class CheckRules : WorkFlowBase
    {
        public void SetWorkFlow(ref Dictionary<string, string> wfValues)
        {
            Log("Inside SetWorkFlow");
            try
            {
                if (wfValues.ContainsKey("TENANT_SCHEMA") && wfValues.ContainsKey("LOANID") && wfValues.ContainsKey("STATUS"))
                {
                    Int64 UserID = 0;

                    if (wfValues.ContainsKey("USER_ID"))
                        UserID = string.IsNullOrEmpty(wfValues["USER_ID"]) ? 0 : Convert.ToInt64(wfValues["USER_ID"]);

                    Log("Before LoanRuleEngine");

                    LoanRuleEngine ruleEngine = new LoanRuleEngine(wfValues["TENANT_SCHEMA"], Convert.ToInt64(wfValues["LOANID"]));
                    Log("After LoanRuleEngine");
                    EvaluateRules evalRuleEngine = new EvaluateRules(wfValues["TENANT_SCHEMA"], Convert.ToInt64(wfValues["LOANID"]));
                    Log("After EvaluateRules");


                    CheckRulesDataAccess _dataAccess = new CheckRulesDataAccess(wfValues["TENANT_SCHEMA"]);

                    _dataAccess.UpdateDocumentCounts(ruleEngine.loan.LoanID, UserID, ruleEngine.BatchDocumentCount, ruleEngine.MissingDocumentCount, ruleEngine.MissingCriticalDocumentCount, ruleEngine.MissingNonCriticalDocumentCount);

                    Log("After UpdateDocumentCounts");
                    ReportMaster _reportMaster = _dataAccess.GetReportMasterDetails();
                    Log("After GetReportMasterDetails");
                    List<ReportConfig> reportConfig = new List<ReportConfig>();
                    if (_reportMaster != null)
                        reportConfig = _dataAccess.GetReportMasterDocumentNames(_reportMaster.ReportMasterID);

                    Log("After GetReportMasterDocumentNames");

                    List<Dictionary<string, string>> _missingDocs = ruleEngine.GetMissingDocumentsInLoan;

                    Log("After _missingDocs");
                    _dataAccess.RemoveLoanReportingEntries(ruleEngine.loan.LoanID);
                    Log("After RemoveLoanReportingEntries");
                    //bool isMissingDocument = false;
                    for (int i = 0; i < _missingDocs.Count; i++)
                    {
                        foreach (var property in reportConfig)
                        {
                            if (_missingDocs[i]["DocName"] == property.DocumentName)
                            {
                                _dataAccess.AddReportConfigDetails(property.ReportID, ruleEngine.loan.LoanID, ruleEngine.loan.ReviewTypeID, true);

                            }
                        }
                    }
                    Log("After AddReportConfigDetails");

                    CheckListResult evalResult = new CheckListResult();
                    List<Dictionary<string, string>> checklistDetails = evalRuleEngine.GetAllCheckListDetails;
                    Log("After checklistDetails");
                    evalResult.loanQuestioner = _dataAccess.GetLoanQuestioner(ruleEngine.loan.LoanID, ruleEngine.loan.CustomerID, ruleEngine.loan.ReviewTypeID, ruleEngine.loan.LoanTypeID);

                    Log("After GetLoanQuestioner");
                    evalResult.allChecklist = checklistDetails;
                    _dataAccess.UpdateEvalResult(ruleEngine.loan.LoanID, JsonConvert.SerializeObject(evalResult));
                    Log("After UpdateEvalResult");

                    LoanLOSFields _loanLOS = _dataAccess.GetSendingEmailDetails(ruleEngine.loan.LoanID);
                    Log("After GetSendingEmailDetails");

                    string email = string.Empty;
                    if (_loanLOS != null && ruleEngine.loan.UploadType == UploadConstant.ENCOMPASS)
                    {
                        if (!(string.IsNullOrEmpty(_loanLOS.EmailPostCloser)) && !(string.IsNullOrEmpty(_loanLOS.EmailUnderwriter)))
                        {
                            email = _loanLOS.EmailPostCloser; //  + "," + _loanLOS.EmailUnderwriter;
                        }
                        else if (!(string.IsNullOrEmpty(_loanLOS.EmailPostCloser)))
                        {
                            email = _loanLOS.EmailPostCloser;
                        }
                        //else if(!(string.IsNullOrEmpty(_loanLOS.EmailUnderwriter)))
                        //{
                        //    email = _loanLOS.EmailUnderwriter;
                        //}

                        if (!(string.IsNullOrEmpty(email)))
                        {
                            string _ruleFindings = JsonConvert.SerializeObject(checklistDetails);
                            _dataAccess.SendRuleFindingsEmail(email, "Rule Findings", "", _ruleFindings, 0, "IntellaLend", ruleEngine.loan.LoanID, CustomEmailTemplateConstants.RuleFindings);
                        }
                    }

                    Log("After SendRuleFindingsEmail");
                    Log($"checklistDetails {checklistDetails.Count}");


                    if (_missingDocs.Count == 0 && checklistDetails.Where(c => c["Result"] == "False").Count() == 0)
                        wfValues["STATUS"] = StatusConstant.COMPLETE.ToString();
                    else
                        wfValues["STATUS"] = StatusConstant.PENDING_AUDIT.ToString();

                    Log("Before InsertLoanCheckListAuditDetails");

                    List<LoanChecklistAudit> lsAudit = evalRuleEngine.FetchLoanCheckListDetails(checklistDetails);
                    Log($"After FetchLoanCheckListDetails");

                    _dataAccess.InsertLoanCheckListAuditDetails(lsAudit, Convert.ToInt64(wfValues["LOANID"]));

                    Log("After InsertLoanCheckListAuditDetails");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Log(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }
    }
}
