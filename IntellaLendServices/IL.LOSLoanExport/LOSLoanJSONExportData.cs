using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IL.LOSLoanExport
{
    public class LOSLoanJSONExportData
    {
        private string TenantSchema;
        public LOSLoanJSONExportData(string _tenantSchema)
        {
            TenantSchema = _tenantSchema;
        }

        public Dictionary<string, object> GetLoanData(Int64 LoanID, string ExportPath)
        {
            Dictionary<string, object> loanDic = new Dictionary<string, object>();

            using (var db = new DBConnect(TenantSchema))
            {
                Loan _loan = db.Loan.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                CustomerMaster _customer = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == _loan.CustomerID).FirstOrDefault();
                ReviewTypeMaster _reviewType = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == _loan.ReviewTypeID).FirstOrDefault();
                LoanTypeMaster _loanType = db.LoanTypeMaster.AsNoTracking().Where(x => x.LoanTypeID == _loan.LoanTypeID).FirstOrDefault();
                IDCFields _idcFields = db.IDCFields.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                if (_idcFields != null)
                {
                    _idcFields.IDCReviewerName = string.IsNullOrEmpty(_idcFields.IDCReviewerName) ? string.Empty : _idcFields.IDCReviewerName.Substring(0, _idcFields.IDCReviewerName.LastIndexOf('|')).Trim();
                    _idcFields.IDCLevelOneDuration = string.IsNullOrEmpty(_idcFields.IDCLevelOneDuration) ? string.Empty : _idcFields.IDCLevelOneDuration.Substring(0, _idcFields.IDCLevelOneDuration.LastIndexOf('|')).Trim();

                    _idcFields.IDCValidatorName = string.IsNullOrEmpty(_idcFields.IDCValidatorName) ? string.Empty : _idcFields.IDCValidatorName.Substring(0, _idcFields.IDCValidatorName.LastIndexOf('|')).Trim();
                    _idcFields.IDCLevelTwoDuration = string.IsNullOrEmpty(_idcFields.IDCLevelTwoDuration) ? string.Empty : _idcFields.IDCLevelTwoDuration.Substring(0, _idcFields.IDCLevelTwoDuration.LastIndexOf('|')).Trim();
                }

                string appURL = db.CustomerConfig.AsNoTracking().Where(x => x.ConfigKey == ConfigConstant.APPLICATIONURL && x.CustomerID == 0).FirstOrDefault().ConfigValue;
                LoanDetail _loanDetails = db.LoanDetail.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();

                LoanEvaluatedResult _loanEvaluatedResult = db.LoanEvaluatedResult.AsNoTracking().Where(x => x.LoanID == LoanID).FirstOrDefault();
                CustReviewLoanCheckMapping _rMapping = db.CustReviewLoanCheckMapping.AsNoTracking().Where(x => x.CustomerID == _loan.CustomerID && x.ReviewTypeID == _loan.ReviewTypeID && x.LoanTypeID == _loan.LoanTypeID).FirstOrDefault();

                List<RuleResult> _rMaster = (from r in db.RuleMaster.AsNoTracking()
                                             join c in db.CheckListDetailMaster.AsNoTracking() on r.CheckListDetailID equals c.CheckListDetailID
                                             where c.CheckListID == _rMapping.CheckListID && c.Rule_Type == 0
                                             select new RuleResult
                                             {
                                                 RuleID = r.RuleID,
                                                 RuleName = c.Name,
                                                 RuleDescription = c.Description,
                                                 Expression = r.RuleDescription,
                                                 Result = false,
                                                 RuleType = "Automatic",
                                                 ErrorMessage = "",
                                                 EvaluatedExpression = "",
                                                 RDocTypes = r.DocumentType,
                                                 Notes = "",
                                             }).ToList();


                CheckListResult ruleResults = JsonConvert.DeserializeObject<CheckListResult>(_loanEvaluatedResult.EvaluatedResult);
                List<RuleResult> _mRuleResult = GetManualQuestions(_loanDetails.ManualQuestioners, ruleResults.loanQuestioner);
                List<RuleResult> _aRuleResult = GetAutoRuleResults(ruleResults.allChecklist, _rMaster, db);

                Batch _loanBatch = JsonConvert.DeserializeObject<Batch>(_loanDetails.LoanObject);

                List<ExportDocuments> _exportDocs = GetDocuments(_loanBatch.Documents, ExportPath);

                List<RuleResult> _rules = _aRuleResult.Concat(_mRuleResult).ToList();
                Logger.WriteTraceLog($"_aRuleResult Result : {JsonConvert.SerializeObject(_rules)}");
                Logger.WriteTraceLog($"Rules : {_rules.Count}");

                loanDic["TenantSchema"] = TenantSchema;
                loanDic["Loan"] = _loan;
                loanDic["LoanLoanSearch"] = _loanSearch;
                loanDic["CustomerMaster"] = _customer;
                loanDic["ReviewTypeMaster"] = _reviewType;
                loanDic["LoanTypeMaster"] = _loanType;
                loanDic["IDCFields"] = _idcFields;
                loanDic["Event"] = LOSExportEventConstant.LOS_EXPORT_EVENT;
                loanDic["EventDescription"] = LOSExportEventConstant.LOS_EXPORT_EVENT_DESC;
                loanDic["Version"] = ConfigurationManager.AppSettings["AppVersion"];
                loanDic["LoanURL"] = $"{appURL}/view/loandetails/{CommonUtils.EnDecrypt(_loan.LoanGUID.ToString())}";
                loanDic["Documents"] = _exportDocs;
                loanDic["Rules"] = _rules;
                //loanDic["AutoRules"] = _aRuleResult;
            }

            return loanDic;
        }

        private List<RuleResult> GetAutoRuleResults(List<Dictionary<string, string>> loanAutoRules, List<RuleResult> _rMaster, DBConnect db)
        {
            List<RuleResult> _aRuleResult = new List<RuleResult>();

            foreach (RuleResult item in _rMaster)
            {
                Dictionary<string, string> ruleEvalItem = new Dictionary<string, string>();
                for (int i = 0; i < loanAutoRules.Count; i++)
                {
                    if (loanAutoRules[i].ContainsKey("CheckListName") && loanAutoRules[i]["CheckListName"] == item.RuleName)
                    {
                        loanAutoRules[i]["CheckListName"] = loanAutoRules[i]["CheckListName"].Replace("\"", "'");
                        ruleEvalItem = loanAutoRules[i];
                    }
                }

                List<RuleDocs> _rDocs = new List<RuleDocs>();
                if (!string.IsNullOrEmpty(item.RDocTypes))
                {
                    foreach (string sDocID in item.RDocTypes.Trim().Split(','))
                    {
                        Int64 docID = 0;
                        Int64.TryParse(sDocID, out docID);
                        if (docID > 0)
                        {
                            DocumentTypeMaster dm = db.DocumentTypeMaster.AsNoTracking().Where(x => x.DocumentTypeID == docID).FirstOrDefault();
                            if (dm != null)
                            {
                                _rDocs.Add(new RuleDocs()
                                {
                                    DocumentType = dm.Name,
                                    DocumentDesc = dm.DisplayName
                                });
                            }
                        }
                    }
                }
                item.DocTypes = _rDocs;

                item.Options = new List<string>();
                item.SelectedAnswer = new List<string>();
                if (ruleEvalItem.Count > 0 && ruleEvalItem.ContainsKey("Result") && ruleEvalItem.ContainsKey("Message") && ruleEvalItem.ContainsKey("Expression") && ruleEvalItem.ContainsKey("ErrorMessage"))
                {
                    item.Result = Convert.ToBoolean(ruleEvalItem["Result"]);
                    item.ErrorMessage = !string.IsNullOrEmpty(ruleEvalItem["Message"]) ? ruleEvalItem["Message"] : ruleEvalItem["ErrorMessage"];
                    item.EvaluatedExpression = ruleEvalItem["Expression"];
                }
                _aRuleResult.Add(item);
            }
            Logger.WriteTraceLog($"_aRuleResult : {_aRuleResult.Count}");

            return _aRuleResult;
        }

        private List<RuleResult> GetManualQuestions(string ManualQuestioners, List<ManualQuestioner> loanQuestioner)
        {
            List<RuleResult> _mRuleResult = new List<RuleResult>();
            List<ManualkAnswerJson> _mAnserJson = new List<ManualkAnswerJson>();
            if (!string.IsNullOrEmpty(ManualQuestioners))
            {
                List<ManualQuestioner> _mAnswers = JsonConvert.DeserializeObject<List<ManualQuestioner>>(ManualQuestioners);

                foreach (var item in _mAnswers)
                {
                    ManualkAnswerJson _ans = JsonConvert.DeserializeObject<ManualkAnswerJson>(item.AnswerJson);
                    if (_ans != null)
                    {
                        _ans.Answer = JsonConvert.DeserializeObject<AnswersJson>(item.AnswerJson).Answer;
                        _ans.RuleID = item.RuleID;
                        _mAnserJson.Add(_ans);
                    }
                }
            }
            Logger.WriteTraceLog($"loanQuestioner : {loanQuestioner.Count}");

            foreach (var item in loanQuestioner)
            {
                ManualkAnswerJson _ans = _mAnserJson.Where(x => x.RuleID == item.RuleID).FirstOrDefault();

                Logger.WriteTraceLog($"item.OptionJson : {item.OptionJson}");
                RuleOptions options = JsonConvert.DeserializeObject<RuleOptions>(item.OptionJson);
                List<string> OptionVals = new List<string>();
                foreach (var itemRule in options.manualGroup)
                {
                    if (itemRule.CheckBoxChoices != null && itemRule.CheckBoxChoices.Count > 0)
                        OptionVals = itemRule.CheckBoxChoices.Select(x => x.checkboxoptions).ToList();


                    if (itemRule.raidoboxoptions != null && itemRule.raidoboxoptions.Count > 0)
                        OptionVals = itemRule.raidoboxoptions.Select(x => x.radiooptions).ToList();
                }

                Logger.WriteTraceLog($"options.manualGroup : {options.manualGroup.Count}");
                Logger.WriteTraceLog($"Question : {item.Question}");

                Regex _docRegex = new Regex(@"\[(.*?)\]");
                List<RuleDocs> _ruleDocs = new List<RuleDocs>();
                foreach (var doc in _docRegex.Matches(item.Question))
                {
                    string _docName = doc.ToString().Replace("[", "").Replace("]", "");
                    _ruleDocs.Add(new RuleDocs() { DocumentType = _docName, DocumentDesc = _docName });
                }

                _mRuleResult.Add(new RuleResult()
                {
                    RuleID = item.RuleID,
                    Notes = _ans != null ? _ans.Notes.Replace("\"", "'") : string.Empty,
                    RuleType = "Manual",
                    RuleName = item.CheckListName.Replace("\"", "'"),
                    RuleDescription = item.Question,
                    SelectedAnswer = _ans != null ? _ans.Answer.Select(x => x.ToString()).ToList() : new List<string>(),
                    Options = OptionVals,
                    DocTypes = _ruleDocs,
                    ErrorMessage = string.Empty,
                    EvaluatedExpression = string.Empty,
                    Expression = string.Empty,
                    RDocTypes = string.Empty,
                    Result = true
                });

            }
            Logger.WriteTraceLog($"_mRuleResult : {_mRuleResult.Count}");
            return _mRuleResult;
        }


        private List<ExportDocuments> GetDocuments(List<Documents> loanDocs, string ExportPath)
        {
            List<ExportDocuments> _exportDocs = new List<ExportDocuments>();

            foreach (Documents doc in loanDocs)
            {
                ExportDocuments _eDoc = new ExportDocuments();

                _eDoc.DocumentID = $"{doc.DocumentTypeID}_{doc.VersionNumber}";
                _eDoc.ILDocumentID = doc.DocumentTypeID;
                _eDoc.DocumentType = doc.Type;
                _eDoc.DocumentDesc = doc.Description;
                _eDoc.Version = $"V{doc.VersionNumber}";
                _eDoc.Obsolete = doc.Obsolete;
                _eDoc.Reviewed = doc.Reviewed;
                _eDoc.DocumentExtractionAccuracy = doc.DocumentExtractionAccuracy;
                _eDoc.Confidence = doc.Confidence;
                _eDoc.DocumentIdentifier = doc.Identifier;
                _eDoc.BatchInstanceIdentifier = doc.BatchInstanceIdentifier;

                if (doc.DocumentTypeID == 0)
                    _eDoc.DocumentFile = Path.Combine(ExportPath, $"{doc.Description}_{doc.DocumentTypeID}_{_eDoc.Version}.pdf");

                if (doc.DocumentTypeID > 0)
                    _eDoc.DocumentFile = Path.Combine(ExportPath, $"{doc.Type}_{doc.DocumentTypeID}_{_eDoc.Version}.pdf");

                _eDoc.Pages = new List<string>();
                foreach (var item in doc.Pages)
                    _eDoc.Pages.Add($"PG{item}");

                _eDoc.DataFields = new List<JSONField>();

                //Logger.WriteTraceLog($"doc.DocumentLevelFields : {doc.DocumentLevelFields.Count}");

                foreach (var item in doc.DocumentLevelFields)
                    _eDoc.DataFields.Add(new JSONField() { PropertyName = item.Name, PropertyValue = item.Value.Replace("\"", "'") });


                //Logger.WriteTraceLog($"_eDoc.DataFields : {_eDoc.DataFields.Count}");
                _eDoc.DataTables = new List<LoanTableData>();

                //Logger.WriteTraceLog($"doc.DataTables : {doc.DataTables.Count}");

                foreach (var item in doc.DataTables)
                {
                    LoanTableData _table = new LoanTableData();

                    _table.TableName = item.Name;
                    _table.Columns = item.HeaderRow.HeaderColumns.Select(x => x.Name).ToList();
                    _table.Rows = new List<RowData>();

                    foreach (var itemRow in item.Rows)
                        _table.Rows.Add(new RowData()
                        {
                            CellValues = itemRow.RowColumns.Select(x => new JSONField() { PropertyName = x.Name, PropertyValue = x.Value.Replace("\"", "'") }).ToList()
                        });

                    _eDoc.DataTables.Add(_table);
                }

                //Logger.WriteTraceLog($"_eDoc.DataTables : {_eDoc.DataTables.Count}");
                _exportDocs.Add(_eDoc);
            }
            return _exportDocs;
        }
    }
}
