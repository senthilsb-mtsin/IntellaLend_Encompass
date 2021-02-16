using EncompassConsoleConnector;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSRuleEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
//using IntellaLend.CheckRule;

namespace IntellaLend.RuleEngine
{
    public class EvaluateRules
    {
        #region Private Variables

        internal string TenantSchema;
        internal Batch batch;
        internal RuleEngineDataAccess ruleEngineDataAccess;
        internal Dictionary<string, object> batchDocFields = new Dictionary<string, object>();
        internal MTSRules CheckListRules = new MTSRules();
        internal Dictionary<string, RuleMaster> CheckListDetails = new Dictionary<string, RuleMaster>();
        internal Dictionary<string, MTSRuleResult> output;
        internal List<Dictionary<string, string>> ls;
        internal Dictionary<string, string> dic;
        internal Int64 loanID;
        internal List<CheckListDetailMaster> CheckListItems = new List<CheckListDetailMaster>();
        internal MTSRules Category = new MTSRules();
        internal Dictionary<string, string> _grpByErrorMsg = new Dictionary<string, string>();
        #endregion

        #region Public Variables

        /// <summary>
        /// Get Loan Details
        /// </summary>
        public Loan loan;
        public List<CheckListDetailMaster> checkList;
        #endregion

        #region Constructor 

        public EvaluateRules(string tenantSchema, Int64 LoanId)
        {
            Log("Constructor");
            TenantSchema = tenantSchema;
            loanID = LoanId;
            ruleEngineDataAccess = new RuleEngineDataAccess(this.TenantSchema);
            loan = ruleEngineDataAccess.GetLoanMetaData(LoanId);
            setClassProperties();
            Log("End Constructor");
        }

        public EvaluateRules(string tenantSchema, Int64 LoanId, LoanDetail loanDetail)
        {
            Log("Constructor 2");
            TenantSchema = tenantSchema;
            loanID = LoanId;
            ruleEngineDataAccess = new RuleEngineDataAccess(this.TenantSchema);
            loan = ruleEngineDataAccess.GetLoanMetaData(LoanId, loanDetail);
            setClassProperties();
            Log("End Constructor 2");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Get Total Checklist Count
        /// </summary>
        public Int64 TotalCheckListCount
        {
            get
            {
                return CheckListRules.Count;
            }
        }

        /// <summary>
        /// Get Failed Checklist of the Loan
        /// </summary>
        public List<Dictionary<string, string>> GetFailedCheckList
        {
            get
            {
                ls = new List<Dictionary<string, string>>();
                dic = new Dictionary<string, string>();
                output = new Dictionary<string, MTSRuleResult>();
                output = CheckListRules.Eval(batchDocFields);
                foreach (string rule in output.Keys)
                {
                    bool ruleResult = true;
                    bool.TryParse(output[rule].Result.ToString(), out ruleResult);
                    if (!(ruleResult))
                    {
                        dic = new Dictionary<string, string>();
                        dic["CheckListName"] = rule;
                        dic["Formula"] = CheckListRules[rule];
                        dic["Expression"] = output[rule].Expressions;
                        dic["Result"] = output[rule].Result.ToString();
                        dic["Message"] = output[rule].Message.ToString();
                        dic["SequenceID"] = CheckListItems.Where(c => c.Name == rule).FirstOrDefault().SequenceID.ToString();
                        ls.Add(dic);
                    }
                }
                return ls;
            }
        }

        /// <summary>
        /// Get All Checklist Result of the Loan
        /// </summary>
        public List<Dictionary<string, string>> GetAllCheckListDetails
        {
            get
            {
                ls = new List<Dictionary<string, string>>();
                dic = new Dictionary<string, string>();
                output = new Dictionary<string, MTSRuleResult>();
                output = CheckListRules.Eval(batchDocFields);
                foreach (string rule in output.Keys)
                {
                    dic = new Dictionary<string, string>();
                    dic["CheckListName"] = rule;
                    dic["Formula"] = CheckListRules[rule];
                    dic["Expression"] = output[rule].Expressions;
                    dic["Result"] = output[rule].Result.ToString();
                    dic["ErrorMessage"] = output[rule].ErrorMessage.ToString();
                    dic["Message"] = output[rule].Message.ToString();
                    dic["RoleType"] = CheckListItems.Where(c => c.Name == rule).FirstOrDefault().Rule_Type.ToString();
                    dic["SequenceID"] = CheckListItems.Where(c => c.Name == rule).FirstOrDefault().SequenceID.ToString();
                    dic["Category"] = CheckListItems.Where(c => c.Name == rule).FirstOrDefault().Category.ToString();
                    dynamic rJ = JsonConvert.DeserializeObject(CheckListItems.Where(c => c.Name == rule).FirstOrDefault().RuleMasters.RuleJson);
                    dic["RuleType"] = rJ["mainOperator"];

                    if (_grpByErrorMsg.ContainsKey(rule))
                    {
                        dic["ErrorMessage"] = _grpByErrorMsg[rule];
                        dic["Message"] = _grpByErrorMsg[rule];
                    }
                    ls.Add(dic);
                }
                return ls;
            }
        }

        #endregion

        #region Loan CheckList Audit

        public void InsertLoanCheckListAuditDetails(List<LoanChecklistAudit> loancheckList)
        {
            ruleEngineDataAccess.InsertLoanCheckListAuditDetails(loancheckList);
        }

        public List<LoanChecklistAudit> FetchLoanCheckListDetails(List<Dictionary<string, string>> _allCheckListDetails)
        {
            //List<CheckListDetailMaster> checkListDetails = ruleEngineDataAccess.GetCheckListDetail(CustomerID, ReviewTypeID, LoanTypeID);
            List<LoanChecklistAudit> _loancheckList = new List<LoanChecklistAudit>();
            foreach (CheckListDetailMaster _checkList in CheckListItems)
            {
                LoanChecklistAudit loancheckListAudit = new LoanChecklistAudit();
                loancheckListAudit.LoanID = loan.LoanID;
                loancheckListAudit.CustomerID = loan.CustomerID;
                loancheckListAudit.ReviewTypeID = loan.ReviewTypeID;
                loancheckListAudit.LoanTypeID = loan.LoanTypeID;
                loancheckListAudit.ChecklistGroupID = _checkList.CheckListID;
                loancheckListAudit.ChecklistDetailID = _checkList.CheckListDetailID;
                loancheckListAudit.RuleID = _checkList.RuleMasters != null ? _checkList.RuleMasters.RuleID : 0;
                // loancheckListAudit.ChecklistName = _checkList.CheckListMaster.CheckListName;
                loancheckListAudit.ChecklistDescription = _checkList.Description;
                loancheckListAudit.CreatedOn = DateTime.Now;
                loancheckListAudit.ModifiedOn = DateTime.Now;

                //Dictionary<string, string> item = _allCheckListDetails.Where(a => a["CheckListName"] == _checkList.Name).FirstOrDefault();

                //if (item != null)
                //{

                //}

                foreach (Dictionary<string, string> item in _allCheckListDetails)
                {
                    if (_checkList.Name == item["CheckListName"])
                    {
                        bool ruleResult = true;
                        bool.TryParse(item["Result"], out ruleResult);
                        loancheckListAudit.ChecklistName = item["CheckListName"];
                        loancheckListAudit.Result = ruleResult;
                        loancheckListAudit.RuleFormula = item["Formula"];
                        loancheckListAudit.Evaluation = item["Expression"];
                        loancheckListAudit.ErrorMessage = item["ErrorMessage"];
                        _loancheckList.Add(loancheckListAudit);
                    }
                }
            }
            return _loancheckList;
        }


        public List<LoanChecklistAudit> FetchLoanCheckListDetails()
        {
            //List<CheckListDetailMaster> checkListDetails = ruleEngineDataAccess.GetCheckListDetail(CustomerID, ReviewTypeID, LoanTypeID);
            List<LoanChecklistAudit> _loancheckList = new List<LoanChecklistAudit>();
            foreach (CheckListDetailMaster _checkList in CheckListItems)
            {
                LoanChecklistAudit loancheckListAudit = new LoanChecklistAudit();
                loancheckListAudit.LoanID = loan.LoanID;
                loancheckListAudit.CustomerID = loan.CustomerID;
                loancheckListAudit.ReviewTypeID = loan.ReviewTypeID;
                loancheckListAudit.LoanTypeID = loan.LoanTypeID;
                loancheckListAudit.ChecklistGroupID = _checkList.CheckListID;
                loancheckListAudit.ChecklistDetailID = _checkList.CheckListDetailID;
                loancheckListAudit.RuleID = _checkList.Rule_Type;
                // loancheckListAudit.ChecklistName = _checkList.CheckListMaster.CheckListName;
                loancheckListAudit.ChecklistDescription = _checkList.Description;
                loancheckListAudit.CreatedOn = DateTime.Now;
                loancheckListAudit.ModifiedOn = DateTime.Now;
                List<Dictionary<string, string>> _allCheckListDetails = GetAllCheckListDetails;
                foreach (Dictionary<string, string> item in _allCheckListDetails)
                {
                    if (_checkList.Name == item["CheckListName"])
                    {
                        bool ruleResult = true;
                        bool.TryParse(item["Result"], out ruleResult);
                        loancheckListAudit.ChecklistName = item["CheckListName"];
                        loancheckListAudit.Result = ruleResult;
                        loancheckListAudit.RuleFormula = item["Formula"];
                        loancheckListAudit.Evaluation = item["Expression"];
                        loancheckListAudit.ErrorMessage = item["ErrorMessage"];
                        _loancheckList.Add(loancheckListAudit);
                    }
                }
            }
            return _loancheckList;
        }

        #endregion

        #region Private Methods                

        private MTSRules GetCheckList(List<ChecklistRuleMaster> _checklistRuleMaster)
        {
            MTSRules _checkListRules = new MTSRules();

            if (_checklistRuleMaster != null)
            {
                foreach (var item in _checklistRuleMaster)
                {
                    if (item.RuleDescription != null && item.ChecklistDetail != null)
                        _checkListRules[item.ChecklistDetail.Name.ToString()] = item.RuleDescription.RuleDescription;
                }
            }

            return _checkListRules;

        }

        private Dictionary<string, RuleMaster> GetCheckListDetails(List<ChecklistRuleMaster> _checklistRuleMaster)
        {
            Dictionary<string, RuleMaster> _checkListRules = new Dictionary<string, RuleMaster>();

            if (_checklistRuleMaster != null)
            {
                foreach (var item in _checklistRuleMaster)
                {
                    if (item.RuleDescription != null && item.ChecklistDetail != null)
                        _checkListRules[item.ChecklistDetail.Name.ToString()] = item.RuleDescription;
                }
            }

            return _checkListRules;

        }

        private List<CheckListDetailMaster> GetCheckListDetail(List<ChecklistRuleMaster> _checklistRuleMaster)
        {
            List<CheckListDetailMaster> _checkListRules = new List<CheckListDetailMaster>();

            if (_checklistRuleMaster != null)
            {
                foreach (var item in _checklistRuleMaster)
                {
                    if (item.ChecklistDetail != null)
                    {
                        item.ChecklistDetail.RuleMasters = new RuleMaster();
                        item.ChecklistDetail.RuleMasters = item.RuleDescription;
                        _checkListRules.Add(item.ChecklistDetail);
                    }
                }
            }

            return _checkListRules;
        }

        private void setClassProperties()
        {
            if (loan != null)
            {
                Log("setClassProperties");
                batch = JsonConvert.DeserializeObject<Batch>(loan.LoanDetails.LoanObject);

                List<ChecklistRuleMaster> _checklistRuleMaster = ruleEngineDataAccess.GetCheckListInfo(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                Log("_checklistRuleMaster after");
                _checklistRuleMaster = LOSConditionCheck(_checklistRuleMaster);
                Log("LOSConditionCheck after");
                CheckListRules = GetCheckList(_checklistRuleMaster);
                Log("GetCheckList after");
                CheckListDetails = GetCheckListDetails(_checklistRuleMaster);
                Log("GetCheckListDetails after");
                CheckListItems = GetCheckListDetail(_checklistRuleMaster);
                Log("GetCheckListDetails after 2");
                FormBatchDocFields();
                Log("end setClassProperties");
            }
        }
        private List<ChecklistRuleMaster> LOSConditionCheck(List<ChecklistRuleMaster> _checklistRuleMaster)
        {
            Log("LOSConditionCheck");
            List<ChecklistRuleMaster> newChecklistRules = new List<ChecklistRuleMaster>();

            if (_checklistRuleMaster.Count > 0)
            {
                Log("_checklistRuleMaster.Count > 0");
                List<ChecklistRuleMaster> conRules = _checklistRuleMaster.Where(x => x.ChecklistDetail.LOSFieldToEvalRule > 0).ToList();
                Log($"conRules.Count : {conRules.Count}");
                newChecklistRules = _checklistRuleMaster.Where(x => x.ChecklistDetail.LOSFieldToEvalRule == 0).ToList();
                Log($"newChecklistRules.Count : {newChecklistRules.Count}");
                foreach (ChecklistRuleMaster cRule in conRules)
                {
                    string enCompassGUID = loan.EnCompassLoanGUID == null ? string.Empty : loan.EnCompassLoanGUID.ToString();
                    Log($"enCompassGUID : {enCompassGUID}");
                    string enFieldID = ruleEngineDataAccess.GetEncompassFieldID(cRule.ChecklistDetail.LOSFieldToEvalRule);
                    Log($"enFieldID : {enFieldID}");
                    if (!string.IsNullOrEmpty(enCompassGUID) && !string.IsNullOrEmpty(enFieldID))
                    {
                        Log($"Before enFieldVal");
                        string enFieldVal = EncompassConnectorApp.QueryEncompass(enCompassGUID, enFieldID, TenantSchema);
                        Log($"enFieldVal : {enFieldVal}");
                        if (cRule.ChecklistDetail.LosIsMatched == 1)
                        {
                            if (enFieldVal != null && enFieldVal != "null" && !string.IsNullOrEmpty(cRule.ChecklistDetail.LOSValueToEvalRule) && cRule.ChecklistDetail.LOSValueToEvalRule.Split('|').Any(v => v.Trim().ToLower().Equals(enFieldVal.Trim().ToLower())))
                                newChecklistRules.Add(cRule);
                        }
                        else if (cRule.ChecklistDetail.LosIsMatched == 2)
                        {
                            if (enFieldVal != null && enFieldVal != "null" && !string.IsNullOrEmpty(cRule.ChecklistDetail.LOSValueToEvalRule) && !cRule.ChecklistDetail.LOSValueToEvalRule.Split('|').Any(v => v.Trim().ToLower().Equals(enFieldVal.Trim().ToLower())))
                                newChecklistRules.Add(cRule);
                        }
                    }
                }
                Log($"last newChecklistRules.Count : {newChecklistRules.Count}");
            }

            return newChecklistRules;
        }


        private void FormBatchDocFields()
        {
            string fieldName = string.Empty;
            string fieldValue = string.Empty;

            batchDocFields["TENANTSCHEMA"] = TenantSchema;

            List<Documents> batchDocs = (from docs in batch.Documents
                                         where docs.Obsolete == false
                                         group docs by docs.DocumentTypeID into docGroup
                                         select docGroup.OrderByDescending(p => p.VersionNumber).First()).ToList<Documents>();

            var dupDocIDs = (from d in batch.Documents
                             select new
                             {
                                 DocumentName = d.Type,
                                 DocumentTypeID = d.DocumentTypeID,
                                 Count = batch.Documents.Where(doc => doc.DocumentTypeID == d.DocumentTypeID).Count()
                             }).Distinct().Where(dup => dup.Count > 1).ToList();

            foreach (var docIDObject in dupDocIDs)
            {
                string _fieldName = ruleEngineDataAccess.GetOrderByFieldName(docIDObject.DocumentTypeID);

                if (!string.IsNullOrEmpty(_fieldName))
                {
                    var docBatchs = (from d in batch.Documents
                                     where d.DocumentTypeID == docIDObject.DocumentTypeID
                                      && d.Obsolete == false
                                     select new
                                     {
                                         Document = d,
                                         DocVersionNumber = d.VersionNumber,
                                         OrderByFieldValue = d.DocumentLevelFields.Where(f => f.Name == _fieldName).Select(f => f.Value).FirstOrDefault(),
                                         orderbyvalue = d.DocumentLevelFields.Where(f => f.Name == _fieldName).Select(f => f.Value.Replace("$", "")).FirstOrDefault()
                                         // FieldID = d.DocumentLevelFields.Where(x=>x.Name == _fieldName).Select(x=>x.FieldID).FirstOrDefault()
                                     }).OrderByDescending(c => c.DocVersionNumber).OrderBy(o => o, new SortByFiled("orderbyvalue")).ToList();


                    batchDocs.RemoveAll(d => d.DocumentTypeID == docIDObject.DocumentTypeID);

                    int docversion = docBatchs.Count;

                    for (int i = 0; i < docBatchs.Count; i++)
                    {
                        docBatchs[i].Document.Type = docBatchs[i].Document.Type + "-V" + docversion.ToString();
                        docversion--;
                        batchDocs.Add(docBatchs[i].Document);
                    }
                }
                else
                {
                    var docBatchs = (from d in batch.Documents
                                     where d.DocumentTypeID == docIDObject.DocumentTypeID
                                     where d.Obsolete == false
                                     select new
                                     {
                                         Document = d,
                                         OrderByFieldValue = d.VersionNumber
                                     }).OrderBy(o => o.OrderByFieldValue).ToList();


                    batchDocs.RemoveAll(d => d.DocumentTypeID == docIDObject.DocumentTypeID);

                    int docversion = 1;
                    foreach (var doc in docBatchs)
                    {
                        doc.Document.Type = doc.Document.Type + "-V" + docversion.ToString();
                        docversion++;
                        batchDocs.Add(doc.Document);
                    }
                }
            }

            List<string> _keys = new List<string>();

            foreach (string item in CheckListRules.Keys.ToList())
            {
                if (CheckListRules[item].Contains("checkall"))
                {
                    string matchPattern = @"\[.+\]";
                    MatchCollection parameters = Regex.Matches(CheckListRules[item], matchPattern);

                    if (parameters.Count > 0)
                    {
                        string _field = parameters[0].Groups[0].ToString();
                        if (_field.Length > 3)
                            _keys.Add(_field.Substring(1, _field.Length - 2));
                    }
                }

                foreach (var docIDObject in dupDocIDs)
                {
                    //Max
                    if (CheckListDetails[item].DocVersion == "3" && !(CheckListRules[item].Contains("groupby")))
                    {
                        string orgRuleDesc = CheckListRules[item];
                        string newRuleDesc = orgRuleDesc;
                        newRuleDesc = orgRuleDesc.Replace(docIDObject.DocumentName + ".", docIDObject.DocumentName + "-V" + docIDObject.Count.ToString() + ".");
                        CheckListRules[item] = newRuleDesc;
                    }

                    //Min
                    if (CheckListDetails[item].DocVersion == "4" && !(CheckListRules[item].Contains("groupby")))
                    {
                        string orgRuleDesc = CheckListRules[item];
                        string newRuleDesc = orgRuleDesc;
                        newRuleDesc = orgRuleDesc.Replace(docIDObject.DocumentName + ".", docIDObject.DocumentName + "-V1.");
                        CheckListRules[item] = newRuleDesc;
                    }

                    //All
                    if (CheckListDetails[item].DocVersion == "1" && !(CheckListRules[item].Contains("groupby")))
                    {
                        string orgRuleDesc = CheckListRules[item];
                        string newRuleDesc = string.Empty;
                        if (CheckListRules[item].Contains(docIDObject.DocumentName + "."))
                        {
                            for (int i = 1; i <= docIDObject.Count; i++)
                            {
                                if (newRuleDesc == string.Empty)
                                    newRuleDesc += orgRuleDesc.Replace(docIDObject.DocumentName + ".", docIDObject.DocumentName + "-V" + i.ToString() + ".");
                                else
                                    newRuleDesc += " &&" + orgRuleDesc.Replace(docIDObject.DocumentName + ".", docIDObject.DocumentName + "-V" + i.ToString() + ".");

                            }
                        }
                        if (newRuleDesc != string.Empty)
                            CheckListRules[item] = "(" + newRuleDesc + ")";
                    }

                    //Any
                    if (CheckListDetails[item].DocVersion == "2" && !(CheckListRules[item].Contains("groupby")))
                    {
                        string orgRuleDesc = CheckListRules[item];
                        string newRuleDesc = string.Empty;
                        if (CheckListRules[item].Contains(docIDObject.DocumentName + "."))
                        {
                            for (int i = 1; i <= docIDObject.Count; i++)
                            {
                                if (newRuleDesc == string.Empty)
                                    newRuleDesc += orgRuleDesc.Replace(docIDObject.DocumentName + ".", docIDObject.DocumentName + "-V" + i.ToString() + ".");
                                else
                                    newRuleDesc += " ||" + orgRuleDesc.Replace(docIDObject.DocumentName + ".", docIDObject.DocumentName + "-V" + i.ToString() + ".");

                            }
                        }
                        if (newRuleDesc != string.Empty)
                            CheckListRules[item] = "(" + newRuleDesc + ")";
                    }
                }

            }

            foreach (Documents doc in batchDocs)
            {
                if (doc.DocumentTypeID != 0)
                {
                    foreach (var docFields in doc.DocumentLevelFields)
                    {
                        fieldName = String.Format("{0}.{1}", doc.Type, docFields.Name);
                        fieldValue = string.IsNullOrEmpty(docFields.Value) ? string.Empty : docFields.Value;
                        //batchDocFields.Add(fieldName, fieldValue);
                        //fieldValue = fieldValue.Replace("%", "").Replace("$", "").Replace(",", "");
                        try
                        {
                            decimal roundVal = Convert.ToDecimal(fieldValue.Replace("%", "").Replace("$", "").Replace(",", "").Trim());
                            batchDocFields[fieldName] = roundVal;
                        }
                        catch (Exception ex)
                        {
                            batchDocFields[fieldName] = fieldValue;
                            continue;
                        }

                    }
                }
            }


            if (loan.UploadType == UploadConstant.LOS)
            {
                List<FannieMaeFields> fannieMaeResults = ruleEngineDataAccess.GetFannieMaeFields(loan.LoanID);

                foreach (var item in fannieMaeResults)
                {
                    batchDocFields[item.FieldID] = item.FieldValue;
                }
            }

            if (CheckListRules.Any(x => x.Value.Contains("groupby")))
            {
                //Log("Inside GroupBy");
                Dictionary<string, string> _grpByRules = CheckListRules.Where(x => x.Value.Contains("groupby")).ToDictionary(x => x.Key, x => x.Value);

                foreach (string _grpRule in _grpByRules.Keys.ToList())
                {
                    //Log("Inside _grpByRules.Keys.ToList()");
                    string matchPattern = @"\{(.*?)\}";
                    string reslt = _grpByRules[_grpRule];
                    MatchCollection parameters = Regex.Matches(reslt, matchPattern);

                    foreach (Match m in parameters)
                    {
                        //Log("Inside parameters");
                        string fieldType = m.Groups[1].ToString();

                        bool isSum = fieldType.StartsWith("sum");
                        //Log($"isSum : {isSum}");
                        bool isAvg = fieldType.StartsWith("avg");
                        //Log($"isAvg : {isAvg}");
                        string rule = fieldType.Substring(4, fieldType.Length - 5);
                        //Log($"rule : {rule}");
                        string[] fields = rule.Split('|');

                        string documentType = fields[0].Substring(1, fields[0].Length - 2);
                        string groupByFields = fields[1].Substring(8, fields[1].Length - 9);
                        string orderByFields = fields[2].Substring(8, fields[2].Length - 9);
                        string valueFields = fields[3].Substring(6, fields[3].Length - 7);
                        //Log($"documentType : {documentType} , groupByFields : {groupByFields} , orderByFields : {orderByFields} , valueFields : {valueFields}");

                        Batch tempBatch = JsonConvert.DeserializeObject<Batch>(loan.LoanDetails.LoanObject);

                        List<Documents> ruleGrpDocs = tempBatch.Documents.Where(b => b.Type == documentType).ToList();
                        //Log($"DocCount : {ruleGrpDocs.Count}");
                        System.Data.DataTable _tempDT = new System.Data.DataTable();

                        if (ruleGrpDocs.Count > 0)
                        {

                            int docIndex = 0;
                            try
                            {
                                foreach (Documents tempDocRow in ruleGrpDocs)
                                {
                                    //Log($"Current Doc Field Count : {tempDocRow.DocumentLevelFields.Count}, Version : {tempDocRow.VersionNumber} ");

                                    object[] rowObjs = new object[tempDocRow.DocumentLevelFields.Count];

                                    int fieldIndex = 0;

                                    foreach (DocumentLevelFields field in tempDocRow.DocumentLevelFields)
                                    {
                                        if (docIndex == 0)
                                            _tempDT.Columns.Add(field.Name, typeof(String));

                                        rowObjs[fieldIndex] = field.Value;

                                        fieldIndex++;
                                    }

                                    _tempDT.Rows.Add(rowObjs);

                                    docIndex++;
                                }
                                //Log($"After _tempDT Table Creation");
                                System.Data.DataTable _tempGrpDT = _tempDT.Clone();

                                List<string> newRules = new List<string>();

                                foreach (string colName in groupByFields.Split('.'))
                                {
                                    //Log($"Inside groupByFields.Split('.')");
                                    string columnName = colName.Substring(1, colName.Length - 2);
                                    List<string> grpValues = _tempDT.AsEnumerable().Select(d => DBNull.Value == d[columnName] ? "" : d[columnName].ToString()).Distinct().ToList<string>();
                                    string tempOrderBy = orderByFields.Substring(1, orderByFields.Length - 2);
                                    foreach (string grpVal in grpValues)
                                    {
                                        //Log($"Inside grpValues");
                                        List<DataRow> drs = _tempDT.AsEnumerable().Where(x => x[columnName].ToString() == grpVal).ToList();
                                        DataRow dr = _tempGrpDT.NewRow();
                                        dr = drs.OrderBy(o => o, new SortByFiled(tempOrderBy)).FirstOrDefault();

                                        if (dr != null)
                                        {
                                            //Log($"Inside dr != null");
                                            _tempGrpDT.ImportRow(dr);

                                            decimal dVal = 0m;
                                            string tempValueFields = valueFields.Substring(1, valueFields.Length - 2);
                                            decimal.TryParse(dr[tempValueFields].ToString(), out dVal);

                                            newRules.Add($"[{documentType} : {grpVal} : {tempOrderBy} : {tempValueFields}]");

                                            batchDocFields[$"{documentType} : {grpVal} : {tempOrderBy} : {tempValueFields}"] = dVal;

                                            //Log($"batchDocFields : {documentType} : {grpVal} : {tempOrderBy} , dVal = {dVal}");
                                        }
                                    }
                                }

                                // decimal totalVal = 0m;
                                string newRule = string.Join("+", newRules);
                                //Log($"newRule : {newRule}");
                                if (isSum)
                                {
                                    //Log($"Inside isSum");
                                    //Log($"Before CheckListRules[_grpRule] =  {CheckListRules[_grpRule]}");
                                    //Log($"Before _grpByRules[_grpRule] =  {_grpByRules[_grpRule]}");
                                    CheckListRules[_grpRule] = _grpByRules[_grpRule].Replace("{" + fieldType + "}", newRule);
                                    //Log($"After CheckListRules[_grpRule] =  {CheckListRules[_grpRule]}");

                                    //totalVal = _tempGrpDT.AsEnumerable().Select(x =>
                                    //{
                                    //    decimal d = 0m;
                                    //    if (decimal.TryParse(x[valueFields].ToString(), out d))
                                    //        return d;
                                    //    else
                                    //        return 0;
                                    //}).Sum(d => d);
                                }
                                else if (isAvg)
                                {
                                    //Log($"Inside isAvg");

                                    string avgRule = $"(({newRule})/{newRules.Count.ToString()})";
                                    //Log($"Before CheckListRules[_grpRule] =  {CheckListRules[_grpRule]}");
                                    //Log($"Before _grpByRules[_grpRule] =  {_grpByRules[_grpRule]}");
                                    CheckListRules[_grpRule] = _grpByRules[_grpRule].Replace("{" + fieldType + "}", avgRule);
                                    //Log($"After CheckListRules[_grpRule] =  {CheckListRules[_grpRule]}");
                                    //totalVal = _tempGrpDT.AsEnumerable().Select(x =>
                                    //{
                                    //    decimal d = 0m;
                                    //    if (decimal.TryParse(x[valueFields].ToString(), out d))
                                    //        return d;
                                    //    else
                                    //        return 0;
                                    //}).Average(d => d);
                                }

                            }
                            catch (Exception ex)
                            {
                                _grpByErrorMsg.Add(_grpRule, $"Error while processing with document fields");
                                Exception exe = new Exception($"loan ID : {loan.LoanID}, rule : {_grpRule}");
                                MTSExceptionHandler.HandleException(ref ex);
                            }
                        }
                        else
                        {
                            _grpByErrorMsg.Add(_grpRule, $"'{documentType}' not found");
                        }
                    }
                }
            }

            if (CheckListRules.Any(x => x.Value.Contains("datatable")))
            {
                Dictionary<string, string> _dataTableRules = CheckListRules.Where(x => x.Value.Contains("datatable")).ToDictionary(x => x.Key, x => x.Value);

                foreach (string _tableRule in _dataTableRules.Keys)
                {
                    string matchPattern = @"\[(.*?)\]";
                    string reslt = _dataTableRules[_tableRule];
                    MatchCollection parameters = Regex.Matches(reslt, matchPattern);
                    string docType = string.Empty, tableName = string.Empty, columnName = string.Empty, key = string.Empty;
                    IntellaLend.Model.DataTable _docTable = null;
                    foreach (Match m in parameters)
                    {
                        string[] fieldTypes = m.Groups[1].ToString().Split('.');
                        if (fieldTypes.Count() == 4)
                        {
                            docType = fieldTypes[0];
                            tableName = fieldTypes[1];
                            columnName = fieldTypes[2];

                            Documents _document = batchDocs.Where(d => d.Type == docType).FirstOrDefault();

                            if (_document != null)
                            {
                                _docTable = _document.DataTables.Where(dt => dt.Name == tableName).FirstOrDefault();
                                if (_docTable != null)
                                    batchDocFields[$"{docType}.{tableName}.{columnName}"] = JsonConvert.SerializeObject(_docTable);
                                else
                                    batchDocFields[$"{docType}.{tableName}.{columnName}"] = string.Empty;
                            }
                            else
                                batchDocFields[$"{docType}.{tableName}.{columnName}"] = string.Empty;
                        }
                    }
                }
            }

            if (CheckListRules.Any(x => x.Value.Contains("isexist")))
            {
                List<string> _loanDocs = batchDocs.Select(d => d.Type.Replace("-V", "^").Split('^')[0].Trim()).Distinct().ToList();

                //Dictionary<string, bool> _loanDocs = new Dictionary<string, bool>();
                //foreach (var item in CheckLsit)
                //{
                //    _loanDocs.Add(item, false);
                //}
                batchDocFields["isexist"] = _loanDocs;
            }
            if (CheckListRules.Any(x => x.Value.Contains("isexistAny")))
            {
                List<string> _loanDocs = batchDocs.Select(d => d.Type.Replace("-V", "^").Split('^')[0].Trim()).Distinct().ToList();

                //Dictionary<string, bool> _loanDocs = new Dictionary<string, bool>();
                //foreach (var item in CheckLsit)
                //{
                //    _loanDocs.Add(item, false);
                //}
                batchDocFields["isexistAny"] = _loanDocs;
            }
            if (CheckListRules.Any(x => x.Value.Contains("Encompass")))
            {
                batchDocFields["EncompassLoanGUID"] = loan.EnCompassLoanGUID == null ? string.Empty : loan.EnCompassLoanGUID.ToString();
            }

            if (_keys.Count > 0)
            {
                foreach (string _field in _keys)
                {
                    Dictionary<string, string> _obj = new Dictionary<string, string>();
                    foreach (Documents doc in batchDocs)
                    {
                        if (doc.DocumentLevelFields.Where(d => d.Name.Equals(_field)).FirstOrDefault() != null)
                            _obj[$"{doc.Type}.{_field}"] = doc.DocumentLevelFields.Where(d => d.Name.Equals(_field)).Select(d => d.Value).FirstOrDefault().ToString();
                    }

                    batchDocFields[_field] = _obj;
                }

            }
        }


        public void Log(string _msg)
        {
            //Exception ex = new Exception(_msg);
            //MTSExceptionHandler.HandleException(ref ex);
        }

        #endregion
    }
}
