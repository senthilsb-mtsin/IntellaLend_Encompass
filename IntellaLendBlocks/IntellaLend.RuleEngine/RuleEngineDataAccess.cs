using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using MTSRuleEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;

namespace IntellaLend.RuleEngine
{
    public class RuleEngineDataAccess
    {
        private string TenantSchema;
        private string SystemSchema = "IL";

        public RuleEngineDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        public Loan GetLoanDetails(Int64 LoanID)
        {
            Loan loan = null;
            using (var db = new DBConnect(TenantSchema))
            {
                string sqlQuery = (db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).ToString()).Replace("@p__linq__0", LoanID.ToString());
                loan = db.Loan.SqlQuery(sqlQuery).FirstOrDefault();

                if (loan != null)
                {
                    sqlQuery = (db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).ToString()).Replace("@p__linq__0", LoanID.ToString());
                    loan.LoanDetails = db.LoanDetail.SqlQuery(sqlQuery).FirstOrDefault();
                    //loan.AuditLoan = db.AuditLoan.AsNoTracking().Where(ld => ld.LoanID == loan.LoanID).ToList();
                }
            }

            return loan;
        }

        public List<FannieMaeFields> GetFannieMaeFields(Int64 LoanID)
        {
            List<FannieMaeFields> _FannieMaefields = new List<FannieMaeFields>();
            using (var db = new DBConnect(TenantSchema))
            {
                LOSLoanDetails _losTableFields = db.LOSLoanDetails.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (_losTableFields != null && !string.IsNullOrEmpty(_losTableFields.LOSDetailJSON))
                {
                    string docName = db.LOSDocument.AsNoTracking().Where(l => l.LOSDocumentID == _losTableFields.LOSDocumentID).FirstOrDefault().DocumentName;
                    docName = docName.Replace(".", "-");
                    var _losFields = JsonConvert.DeserializeObject<Dictionary<String, dynamic>>(_losTableFields.LOSDetailJSON);

                    string FieldName = string.Empty;

                    foreach (string _field in _losFields.Keys)
                    {
                        LOSDocumentFields _tableField = db.LOSDocumentFields.AsNoTracking().Where(los => los.FieldID == _field).FirstOrDefault();
                        if (_tableField != null)
                            FieldName = _tableField.FieldName;

                        if (_losFields[_field].Type == FannieMaeFieldTypeConstant.SType)
                        {
                            _FannieMaefields.Add(new FannieMaeFields() { FieldID = $"{docName}.#{_field}#{FieldName}", FieldValue = _losFields[_field].Value });
                        }
                        else if (_losFields[_field].Type == FannieMaeFieldTypeConstant.MType)
                        {
                            List<string> values = new List<string>();

                            foreach (var item in _losFields[_field].Value)
                                values.Add(Convert.ToString(item));

                            _FannieMaefields.Add(new FannieMaeFields() { FieldID = $"{docName}.#{_field}#{FieldName}", FieldValue = values.Count > 0 ? values.Last() : "" });
                        }

                    }
                }
                return _FannieMaefields;
            }
        }




        public Loan GetLoanMetaData(Int64 LoanID, LoanDetail _loanDetail)
        {
            Loan loan = null;
            using (var db = new DBConnect(TenantSchema))
            {
                TransactionOptions _transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted };
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions))
                {
                    loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoanDetails = _loanDetail; // db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == loan.LoanID).FirstOrDefault();
                        //loan.AuditLoan = db.AuditLoan.AsNoTracking().Where(ld => ld.LoanID == loan.LoanID).ToList();
                    }
                }
            }

            return loan;
        }

        public Loan GetLoanMetaData(Int64 LoanID)
        {
            Loan loan = null;
            using (var db = new DBConnect(TenantSchema))
            {
                TransactionOptions _transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted };
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions))
                {
                    loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoanDetails = db.LoanDetail.AsNoTracking().Where(ld => ld.LoanID == loan.LoanID).FirstOrDefault();
                        //loan.AuditLoan = db.AuditLoan.AsNoTracking().Where(ld => ld.LoanID == loan.LoanID).ToList();
                    }
                }
            }

            return loan;
        }

        public Loan GetLoanDetails(Int64 LoanID, LoanDetail _loanDetail)
        {
            Loan loan = null;
            using (var db = new DBConnect(TenantSchema))
            {
                TransactionOptions _transactionOptions = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted };
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions))
                {
                    loan = db.Loan.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                    if (loan != null)
                    {
                        loan.LoanDetails = _loanDetail;
                        loan.AuditLoan = db.AuditLoan.AsNoTracking().Where(ld => ld.LoanID == loan.LoanID).ToList();
                    }
                }
            }

            return loan;
        }

        public Dictionary<long, string> GetLoanDocTypes(Int64 CustomerID, Int64 LoanTypeID)
        {
            Dictionary<long, string> docTypeID = new Dictionary<long, string>();

            using (var db = new DBConnect(TenantSchema))
            {
                string _getCustLoanDocMapping = db.CustLoanDocMapping.AsNoTracking()
                    .Where(d => d.CustomerID == CustomerID && d.LoanTypeID == LoanTypeID).ToString();

                _getCustLoanDocMapping = _getCustLoanDocMapping.Replace("@p__linq__0", CustomerID.ToString()).Replace("@p__linq__1", LoanTypeID.ToString());

                List<CustLoanDocMapping> CustReviewLoanDocMapping = db.CustLoanDocMapping.SqlQuery(_getCustLoanDocMapping, CustomerID, LoanTypeID).ToList();

                if (CustReviewLoanDocMapping != null)
                {
                    foreach (var item in CustReviewLoanDocMapping)
                    {
                        DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(dm => dm.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();

                        if (doc != null)
                        {
                            docTypeID[doc.DocumentTypeID] = doc.DisplayName;
                        }
                    }

                    docTypeID = docTypeID.OrderBy(d => d.Key).ToDictionary(d => d.Key, d => d.Value);

                }
            }

            return docTypeID;
        }

        public List<DocumentTypeMaster> GetCustLoanDocTypes(Int64 CustomerID, Int64 LoanTypeID)
        {
            List<DocumentTypeMaster> docTypes = new List<DocumentTypeMaster>();

            using (var db = new DBConnect(TenantSchema))
            {
                string _getCustLoanDocMapping = db.CustLoanDocMapping.AsNoTracking()
                   .Where(d => d.CustomerID == CustomerID && d.LoanTypeID == LoanTypeID).ToString();

                _getCustLoanDocMapping = _getCustLoanDocMapping.Replace("@p__linq__0", CustomerID.ToString()).Replace("@p__linq__1", LoanTypeID.ToString());

                List<CustLoanDocMapping> CustReviewLoanDocMapping = db.CustLoanDocMapping.SqlQuery(_getCustLoanDocMapping, CustomerID, LoanTypeID).ToList();

                if (CustReviewLoanDocMapping != null)
                {
                    foreach (var item in CustReviewLoanDocMapping)
                    {
                        string docQuery = db.DocumentTypeMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == item.DocumentTypeID && ld.Active == true).ToString();
                        DocumentTypeMaster doc = db.DocumentTypeMaster.SqlQuery(docQuery.Replace("@p__linq__0", item.DocumentTypeID.ToString())).FirstOrDefault();

                        //DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(dm => dm.DocumentTypeID == item.DocumentTypeID).FirstOrDefault();
                        if (doc != null)
                        {
                            docQuery = db.DocumentFieldMaster.AsNoTracking().Where(ld => ld.DocumentTypeID == item.DocumentTypeID).ToString();
                            List<DocumentFieldMaster> docFields = db.DocumentFieldMaster.SqlQuery(docQuery.Replace("@p__linq__0", item.DocumentTypeID.ToString())).ToList();
                            doc.DocumentFieldMasters = docFields;
                            doc.Condition = item.Condition;
                            doc.CustDocumentLevel = item.DocumentLevel;
                            docTypes.Add(doc);
                        }
                    }
                }
            }

            return docTypes;
        }

        public List<CheckListDetailMaster> GetCheckListDetail(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            List<CheckListDetailMaster> CheckListRules = new List<CheckListDetailMaster>();

            using (var db = new DBConnect(TenantSchema))
            {
                CheckListRules = (from map in db.CustReviewLoanCheckMapping
                                  join cdm in db.CheckListDetailMaster on map.CheckListID equals cdm.CheckListID
                                  join rm in db.RuleMaster on cdm.CheckListDetailID equals rm.CheckListDetailID
                                  where map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID && cdm.Active == true //&& (cdm.Rule_Type == null ? 0 : cdm.Rule_Type) == 0
                                  select cdm).ToList<CheckListDetailMaster>();
                //if (obj != null)
                //{
                //    foreach (var item in obj)
                //    {
                //        CheckListRules[item.CheckListName.ToString()] = item.Rule;
                //    }
                //}
            }

            return CheckListRules;
        }

        public List<ChecklistRuleMaster> GetCheckListInfo(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                CustReviewLoanCheckMapping mapCheck = db.CustReviewLoanCheckMapping.AsNoTracking().Where(map => map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID).FirstOrDefault();
                List<ChecklistRuleMaster> obj = new List<ChecklistRuleMaster>();
                if (mapCheck != null)
                {

                    obj = (from cdm in db.CheckListDetailMaster.AsNoTracking()
                           join rm in db.RuleMaster.AsNoTracking() on cdm.CheckListDetailID equals rm.CheckListDetailID
                           where cdm.CheckListID == mapCheck.CheckListID && cdm.Active == true //&& (cdm.Rule_Type == null ? 0 : cdm.Rule_Type) == 0
                           select new ChecklistRuleMaster
                           {
                               ChecklistDetail = cdm,
                               RuleDescription = rm
                               //CheckListName = cdm.Name,
                               //RuleDetails = rm
                           }).ToList();
                }

                return obj;
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


        public Dictionary<string, RuleMaster> GetCheckListDetails(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            Dictionary<string, RuleMaster> result = new Dictionary<string, RuleMaster>();

            using (var db = new DBConnect(TenantSchema))
            {
                //CustLoanReviewCheckMapping CustReviewLoanCheckMapping = db.CustLoanReviewCheckMapping
                //    .Include(d => d.CheckListMaster)
                //        .ThenInclude(c => c.CheckListDetailMasters)                
                //    .Single(d => d.CustomerID == CustomerID && d.ReviewTypeID == ReviewID && d.LoanTypeID == LoanTypeID);

                var obj = (from map in db.CustReviewLoanCheckMapping
                           join cdm in db.CheckListDetailMaster on map.CheckListID equals cdm.CheckListID
                           join rm in db.RuleMaster on cdm.CheckListDetailID equals rm.CheckListDetailID
                           where map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID && cdm.Active == true //&& (cdm.Rule_Type == null ? 0 : cdm.Rule_Type) == 0
                           select new
                           {
                               CheckListName = cdm.Name,
                               RuleDetails = rm
                           }).ToList();

                if (obj != null)
                {
                    foreach (var item in obj)
                    {
                        result[item.CheckListName] = item.RuleDetails;
                    }
                }
            }

            return result;
        }

        public MTSRules GetCheckList(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            MTSRules CheckListRules = new MTSRules();

            using (var db = new DBConnect(TenantSchema))
            {
                //CustLoanReviewCheckMapping CustReviewLoanCheckMapping = db.CustLoanReviewCheckMapping
                //    .Include(d => d.CheckListMaster)
                //        .ThenInclude(c => c.CheckListDetailMasters)                
                //    .Single(d => d.CustomerID == CustomerID && d.ReviewTypeID == ReviewID && d.LoanTypeID == LoanTypeID);

                var obj = (from map in db.CustReviewLoanCheckMapping
                           join cdm in db.CheckListDetailMaster on map.CheckListID equals cdm.CheckListID
                           join rm in db.RuleMaster on cdm.CheckListDetailID equals rm.CheckListDetailID
                           where map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID && cdm.Active == true //&& (cdm.Rule_Type == null ? 0 : cdm.Rule_Type) == 0
                           select new
                           {
                               CheckListName = cdm.Name,
                               Rule = rm.RuleDescription
                           }).ToList();

                if (obj != null)
                {
                    foreach (var item in obj)
                    {
                        CheckListRules[item.CheckListName.ToString()] = item.Rule;
                    }
                }
            }

            return CheckListRules;
        }

        public string GetOrderByFieldName(Int64 DocumentTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                DocumentFieldMaster _field = db.DocumentFieldMaster.Where(f => f.DocumentTypeID == DocumentTypeID && f.DocOrderByField != null).FirstOrDefault();

                if (_field != null)
                    return _field.Name;
            }

            return string.Empty;
        }
        public List<StackingOrderGroupmasters> GetStackingGroupMasterDetails(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            //Int64 StackingGrpId=0;
            List<StackingOrderGroupmasters> sogr = null;
            using (var db = new DBConnect(TenantSchema))
            {
                Int64? StackingGrpId = db.CustReviewLoanStackMapping.AsNoTracking().Where(map => map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID).FirstOrDefault().StackingOrderID;
                sogr = db.StackingOrderGroupmasters.Where(so => so.StackingOrderID == StackingGrpId).ToList();
            }
            return sogr;
        }

        public List<StackandGroupDetails> GetStackingOrder(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            Dictionary<long, long> StackingOrder = new Dictionary<long, long>();
            List<StackandGroupDetails> stack = new List<StackandGroupDetails>();
            object data;
            using (var db = new DBConnect(TenantSchema))
            {

                Int64 StackingGrpId = 0;
                var _custreviewloan = db.CustReviewLoanStackMapping.AsNoTracking().Where(map => map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID).FirstOrDefault();
                if (_custreviewloan != null)
                {
                    StackingGrpId = _custreviewloan.StackingOrderID;
                }

                List<StackingOrderDetailMaster> sod = db.StackingOrderDetailMaster.AsNoTracking().Where(s => s.StackingOrderID == StackingGrpId).OrderBy(s => s.SequenceID).ToList();
                List<StackingOrderGroupmasters> sogr = db.StackingOrderGroupmasters.AsNoTracking().Where(so => so.StackingOrderID == StackingGrpId).ToList();

                if (StackingGrpId != 0)
                {

                    List<StackingOrderDetailMaster> _stackingOrder = db.StackingOrderDetailMaster.AsNoTracking().Where(s => s.StackingOrderID == StackingGrpId).OrderBy(s => s.SequenceID).ToList();//.ToDictionary(t => t.DocumentTypeID, t => t.SequenceID);

                    foreach (StackingOrderDetailMaster item in _stackingOrder)
                    {
                        StackingOrder[item.DocumentTypeID] = item.SequenceID;
                    }
                }


                stack = (from s in sod
                         join d in sogr on s.StackingOrderGroupID equals d.StackingOrderGroupID into sGroup
                         from d in sGroup.DefaultIfEmpty()
                         select new StackandGroupDetails()
                         {
                             DocumentID = s.DocumentTypeID,
                             SequenceNumber = s.SequenceID,
                             StackingGroupId = d?.StackingOrderGroupID ?? 0,
                             StackingOrderGroupName = d?.StackingOrderGroupName ?? String.Empty,
                             StackingOrderFieldName = d?.GroupSortField ?? String.Empty,
                             StackingOrderFieldValue = string.Empty,
                             StackingOrderGroupDetails = new string[] { "" }
                         }).ToList();
                //  rmmg?.MenuGroupTitle ?? String.Empty


            }

            return stack;
        }

        public Dictionary<string, string> GetStackingOrderDetails(Int64 CustomerID, Int64 ReviewID, Int64 LoanTypeID)
        {
            Dictionary<string, string> StackingOrder = new Dictionary<string, string>();
            StackingOrder["StackingOrderID"] = string.Empty;
            StackingOrder["StackingOrderName"] = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {

                Int64 StackingGrpId = 0;
                var _custreviewloan = db.CustReviewLoanStackMapping.AsNoTracking().Where(map => map.CustomerID == CustomerID && map.ReviewTypeID == ReviewID && map.LoanTypeID == LoanTypeID).FirstOrDefault();
                if (_custreviewloan != null)
                {
                    StackingGrpId = _custreviewloan.StackingOrderID;
                }
                if (StackingGrpId != null)
                {
                    StackingOrderMaster sm = db.StackingOrderMaster.AsNoTracking().Where(s => s.StackingOrderID == StackingGrpId).FirstOrDefault();
                    if (sm != null)
                    {
                        StackingOrder["StackingOrderID"] = sm.StackingOrderID.ToString();
                        StackingOrder["StackingOrderName"] = sm.Description;
                    }
                }
            }

            return StackingOrder;
        }

        public bool GetLoanPDF(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                LoanPDF loanPdf = db.LoanPDF.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (loanPdf != null)
                    return string.IsNullOrEmpty(loanPdf.LoanPDFPath);
            }
            return false;
        }

        public Int64 GetReverificationCount(Int64 LoanID)
        {
            Int64 count = 0;
            using (var db = new DBConnect(TenantSchema))
            {
                List<LoanReverification> _loanRCount = db.LoanReverification.AsNoTracking().Where(l => l.LoanID == LoanID).ToList();

                if (_loanRCount != null)
                    count = _loanRCount.Count;
            }
            return count;
        }

        public LoanSearch GetLoanSearchDetails(Int64 LoanID)
        {
            LoanSearch _loanSearch = new LoanSearch();
            using (var db = new DBConnect(TenantSchema))
            {
                _loanSearch = db.LoanSearch.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
            }
            return _loanSearch;
        }

        public LoanLOSFields GetLoanLOSDetails(Int64 LoanID)
        {
            LoanLOSFields _loanLOS = new LoanLOSFields();
            using (var db = new DBConnect(TenantSchema))
            {

                _loanLOS = db.LoanLOSFields.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();
                if (_loanLOS != null)
                {
                    return _loanLOS;
                }
                else
                { _loanLOS = new LoanLOSFields(); }
            }
            return _loanLOS;
        }



        public string GetReviewTypeName(Int64 reviewTypeID)
        {
            string _reviewTypeName = string.Empty;
            using (var db = new DBConnect(TenantSchema))
            {
                ReviewTypeMaster _reviewMaster = db.ReviewTypeMaster.AsNoTracking().Where(l => l.ReviewTypeID == reviewTypeID).FirstOrDefault();

                if (_reviewMaster != null)
                    _reviewTypeName = _reviewMaster.ReviewTypeName;
            }
            return _reviewTypeName;
        }

        public List<AuditLoanMissingDoc> GetMissingDocumentUploaded(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.AuditLoanMissingDoc.AsNoTracking().Where(a => a.LoanID == LoanID).ToList();
            }

        }

        public void GetMissingDocumentUploaded(ref List<Dictionary<string, string>> MissingDocumentsInLoan, Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                foreach (var doc in MissingDocumentsInLoan)
                {
                    doc["DocMissingStatusID"] = "0";

                    Int64 DocID = Convert.ToInt64(doc["DocID"]);

                    AuditLoanMissingDoc auditMissingLoan = db.AuditLoanMissingDoc.AsNoTracking().Where(md => md.LoanID == LoanID && md.DocID == DocID).FirstOrDefault();

                    if (auditMissingLoan != null)
                    {
                        doc["DocMissingStatusID"] = auditMissingLoan.Status.ToString();
                        doc["DocMissingStatusDescription"] = auditMissingLoan.Status != 0 ? StatusConstant.GetStatusDescription(auditMissingLoan.Status) : string.Empty;
                    }
                }
            }
        }

        public string GetDocumentTypeName(Int64 DocumentTypeID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                DocumentTypeMaster doc = db.DocumentTypeMaster.AsNoTracking().Where(l => l.DocumentTypeID == DocumentTypeID).FirstOrDefault();

                if (doc != null)
                    return doc.DisplayName;
            }

            return string.Empty;
        }

        public List<AuditLoanDetail> GetLoanDetailAudit(Int64 LoanID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                return db.AuditLoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).ToList();
            }
        }
        public DocumentFieldMaster GetFieldValueByDocId(Int64 DocumentTypeID, out bool _isdocorderbyfield)
        {
            DocumentFieldMaster _field = new DocumentFieldMaster();
            using (var db = new DBConnect(TenantSchema))
            {
                _field = db.DocumentFieldMaster.Where(f => f.DocumentTypeID == DocumentTypeID && f.IsDocName == true).FirstOrDefault();

                if (_field != null)
                {
                    _isdocorderbyfield = _field.IsDocName.GetValueOrDefault();
                    return _field;
                }
            }
            _isdocorderbyfield = false;
            return _field;
        }

        public void InsertLoanCheckListAuditDetails(List<LoanChecklistAudit> loancheckList)
        {

            if (loancheckList != null)
            {
                using (var db = new DBConnect(TenantSchema))
                {
                    foreach (LoanChecklistAudit _loanchecklist in loancheckList)
                    {
                        // Int64 checkLoanCount = db.LoanChecklistAudit.Where(x => x.LoanID == _loanchecklist.LoanID).Count();
                        List<LoanChecklistAudit> lsDBAuditChecklist = db.LoanChecklistAudit.AsNoTracking().Where(x => x.LoanID == _loanchecklist.LoanID).ToList();

                        if (lsDBAuditChecklist.Count <= 0)
                        {
                            db.LoanChecklistAudit.Add(_loanchecklist);
                            db.SaveChanges();
                        }
                        else
                        {
                            LoanChecklistAudit _ExistloancheckList = db.LoanChecklistAudit.Where(x => x.ChecklistGroupID == _loanchecklist.ChecklistGroupID && x.ChecklistDetailID == _loanchecklist.ChecklistDetailID && x.LoanID == _loanchecklist.LoanID).FirstOrDefault();
                            if (_ExistloancheckList != null)
                            {
                                _ExistloancheckList.LoanID = _loanchecklist.LoanID;
                                _ExistloancheckList.CustomerID = _loanchecklist.CustomerID;
                                _ExistloancheckList.ReviewTypeID = _loanchecklist.ReviewTypeID;
                                _ExistloancheckList.LoanTypeID = _loanchecklist.LoanTypeID;
                                _ExistloancheckList.ChecklistGroupID = _loanchecklist.ChecklistGroupID;
                                _ExistloancheckList.ChecklistDetailID = _loanchecklist.ChecklistDetailID;
                                _ExistloancheckList.RuleID = _loanchecklist.RuleID;
                                _ExistloancheckList.ChecklistDescription = _loanchecklist.ChecklistDescription;
                                _ExistloancheckList.CreatedOn = _loanchecklist.CreatedOn;
                                _ExistloancheckList.ModifiedOn = DateTime.Now;
                                _ExistloancheckList.ChecklistName = _loanchecklist.ChecklistName;
                                _ExistloancheckList.Result = _loanchecklist.Result;
                                _ExistloancheckList.RuleFormula = _loanchecklist.RuleFormula;
                                _ExistloancheckList.Evaluation = _loanchecklist.Evaluation;
                                _ExistloancheckList.ErrorMessage = _loanchecklist.ErrorMessage;
                                db.Entry(_ExistloancheckList).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                db.LoanChecklistAudit.Add(_loanchecklist);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

        }
    }
}
