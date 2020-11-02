using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSRuleEngine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
//using IntellaLend.CheckRule;

namespace IntellaLend.RuleEngine
{
    public class LoanRuleEngine
    {
        #region Private Variables

        internal string TenantSchema;
        internal Batch batch;
        internal RuleEngineDataAccess ruleEngineDataAccess;
        internal Dictionary<long, string> loanDocTypes;
        internal List<StackandGroupDetails> stackingOrder;
        internal Dictionary<string, object> batchDocFields = new Dictionary<string, object>();
        internal MTSRules CheckListRules = new MTSRules();
        internal Dictionary<string, RuleMaster> CheckListDetails = new Dictionary<string, RuleMaster>();
        internal Dictionary<string, MTSRuleResult> output;
        internal List<Dictionary<string, string>> ls;
        internal List<BatchDocumentObject> _batchDocumentObject;
        internal List<StackingOrderGroupmasters> _stackingOrderGroupmasters;
        internal Dictionary<string, string> dic;
        internal Int64 loanID;
        internal Dictionary<string, string> loanStackingDetails;
        internal List<MissingDocumentObject> _missingDocs;
        internal List<CheckListDetailMaster> CheckListItems = new List<CheckListDetailMaster>();
        internal DocumentFieldMaster _documentfieldmaster;
        internal MTSRules Category = new MTSRules();
        internal List<DocumentTypeMaster> docTypes = new List<DocumentTypeMaster>();
        #endregion

        #region Public Variables

        /// <summary>
        /// Get Loan Details
        /// </summary>
        public Loan loan;
        public List<CheckListDetailMaster> checkList;
        #endregion

        #region Constructor 

        public LoanRuleEngine(string tenantSchema, Int64 LoanId)
        {
            TenantSchema = tenantSchema;
            loanID = LoanId;
            ruleEngineDataAccess = new RuleEngineDataAccess(this.TenantSchema);
            loan = ruleEngineDataAccess.GetLoanDetails(LoanId);
            docTypes = ruleEngineDataAccess.GetCustLoanDocTypes(loan.CustomerID, loan.LoanTypeID);
            setClassProperties();
        }

        public LoanRuleEngine(string tenantSchema, Int64 LoanId, LoanDetail _loanDetail)
        {
            TenantSchema = tenantSchema;
            loanID = LoanId;
            ruleEngineDataAccess = new RuleEngineDataAccess(this.TenantSchema);
            loan = ruleEngineDataAccess.GetLoanDetails(LoanId, _loanDetail);
            docTypes = ruleEngineDataAccess.GetCustLoanDocTypes(loan.CustomerID, loan.LoanTypeID);
            setClassProperties();
        }
        public LoanRuleEngine(string tenantSchema, Int64 LoanId, bool isExport)
        {
            TenantSchema = tenantSchema;
            loanID = LoanId;
            ruleEngineDataAccess = new RuleEngineDataAccess(this.TenantSchema);
            loan = ruleEngineDataAccess.GetLoanDetails(LoanId);
            batch = JsonConvert.DeserializeObject<Batch>(loan.LoanDetails.LoanObject);
            docTypes = ruleEngineDataAccess.GetCustLoanDocTypes(loan.CustomerID, loan.LoanTypeID);
            stackingOrder = ruleEngineDataAccess.GetStackingOrder(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);

            loanStackingDetails = ruleEngineDataAccess.GetStackingOrderDetails(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);


            GetMissingDocumentWithLevel();
            setBatchDocTypes();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// To Check Loan PDF is Avaiable
        /// </summary>
        public bool IsLoanPDFGenerated
        {
            get
            {
                return ruleEngineDataAccess.GetLoanPDF(loanID);
            }
        }

        /// <summary>
        /// To Get Reverification Count
        /// </summary>
        public Int64 ReverificationCount
        {
            get
            {
                return ruleEngineDataAccess.GetReverificationCount(loanID);
            }
        }

        /// <summary>
        /// To Get Loan Header Information
        /// </summary>
        public object GetLoanHeaderInfo
        {
            get
            {
                LoanSearch _loanSearch = ruleEngineDataAccess.GetLoanSearchDetails(loanID);
                LoanLOSFields _loanLOSField = ruleEngineDataAccess.GetLoanLOSDetails(loanID);

                return new
                {
                    LoanAmount = _loanSearch.LoanAmount,
                    BorrowerName = _loanSearch.BorrowerName,
                    ServiceType = ruleEngineDataAccess.GetReviewTypeName(loan.ReviewTypeID),
                    PropertyAddress = _loanSearch.PropertyAddress,
                    InvestorLoanNumber = _loanSearch.InvestorLoanNumber,
                    AuditMonthYear = loan.AuditMonthYear != null ? loan.AuditMonthYear.ToString("MMMM", CultureInfo.InvariantCulture) + " " + loan.AuditMonthYear.Year.ToString() : "",
                    //ReceivedDate = _loanSearch.ReceivedDate.GetValueOrDefault().ToString(DateConstance.LongDateFormart),
                    ReceivedDate = _loanSearch.ReceivedDate.GetValueOrDefault(),
                    PostCloser = string.IsNullOrEmpty(_loanLOSField.PostCloser) ? "" : _loanLOSField.PostCloser,
                    LoanOfficer = string.IsNullOrEmpty(_loanLOSField.LoanOfficer) ? "" : _loanLOSField.LoanOfficer,
                    UnderWriter = string.IsNullOrEmpty(_loanLOSField.Underwriter) ? "" : _loanLOSField.Underwriter,
                    AuditDueDate = (_loanSearch.AuditDueDate == null) ? DateTime.Now : _loanSearch.AuditDueDate.GetValueOrDefault(),
                };
            }
        }
        /// <summary>
        /// To Get Loan Tables
        /// </summary>
        public Int64 LoanTables
        {
            get
            {
                return ruleEngineDataAccess.GetReverificationCount(loanID);
            }
        }

        /// <summary>
        /// To Get Batch Document Count
        /// </summary>
        public Int64 BatchDocumentCount
        {
            get
            {
                return batch.Documents.Count;
            }
        }

        /// <summary>
        /// To Get Batch Document Critical Count
        /// </summary>
        public Int64 MissingCriticalDocumentCount
        {
            get
            {
                return _missingDocs.Where(b => b.DocumentLevel == DocumentLevelConstant.CRITICAL).Count();
            }
        }

        /// <summary>
        /// To Get Batch Document Non-Critical Count
        /// </summary>
        public Int64 MissingNonCriticalDocumentCount
        {
            get
            {
                return _missingDocs.Where(b => b.DocumentLevel == DocumentLevelConstant.NON_CRITICAL).Count();
            }
        }

        /// <summary>
        /// To Get Missing Document Count In the Loan
        /// </summary>
        public Int64 MissingDocumentCount
        {
            get
            {
                return _missingDocs.Count();
            }
        }

        /// <summary>
        /// Returns Loan Stacking Order Details
        /// </summary>
        public Dictionary<string, string> LoanStackingDetails
        {
            get
            {
                return loanStackingDetails;
            }
        }

        /// <summary>
        /// Get Documents from Loan
        /// </summary>
        public List<BatchDocumentObject> batchDocTypes
        {
            get
            {
                return _batchDocumentObject;
            }
        }

        /// <summary>
        /// Get Missing Documents from Loan
        /// </summary>
        public List<Dictionary<string, string>> GetMissingDocumentsInLoan
        {
            get
            {
                ls = new List<Dictionary<string, string>>();
                dic = new Dictionary<string, string>();
                List<DocumentObject> _documentsInLoan = DocumentsInLoan;
                if (loanDocTypes != null && _documentsInLoan != null)
                    dic = loanDocTypes.Where(x => !(_documentsInLoan.Any(y => x.Key == y.DocTypeID && !y.Obsolete))).ToDictionary(t => t.Key.ToString(), t => t.Value);

                Dictionary<string, string> docName = null;
                foreach (string key in dic.Keys)
                {
                    docName = new Dictionary<string, string>();
                    docName["DocID"] = key;
                    docName["DocName"] = dic[key];
                    docName["DocStatusDescription"] = string.Empty;
                    docName["Obsolete"] = Convert.ToString(false);
                    ls.Add(docName);
                }
                return ls;
            }
        }

        /// <summary>
        /// Get Missing Documents which is Uploaded
        /// </summary>
        public List<Dictionary<string, string>> GetMissingDocumentStatus
        {
            get
            {
                List<Dictionary<string, string>> newMissingDocuments = GetMissingDocumentsInLoan;
                ruleEngineDataAccess.GetMissingDocumentUploaded(ref newMissingDocuments, loanID);
                return newMissingDocuments;
            }
        }

        /// <summary>
        /// Get Additional Documents avaiable in Loan
        /// </summary>
        public List<Dictionary<string, string>> GetAdditionalDocumentsInLoan
        {
            get
            {
                ls = new List<Dictionary<string, string>>();
                dic = new Dictionary<string, string>();
                if (loanDocTypes != null && batchDocTypes != null)
                    dic = DocumentsInLoan.Where(x => !(loanDocTypes.Any(y => x.DocTypeID == y.Key))).ToDictionary(t => t.DocTypeID.ToString(), t => t.DocTypeName);

                Dictionary<string, string> docName = null;
                foreach (string key in dic.Keys)
                {
                    docName = new Dictionary<string, string>();
                    docName["DocID"] = key;
                    docName["DocName"] = dic[key];
                    ls.Add(docName);
                }
                return ls;
            }
        }

        /// <summary>
        /// Get All Checklist Result of the Loan
        /// </summary>
        public List<object> GetLoanAudit
        {
            get
            {
                List<object> _audit = new List<object>();

                //List<AuditLoanDetail> _auditDetail = ruleEngineDataAccess.GetLoanDetailAudit(loan.LoanID);

                //_audit = (from l in loan.AuditLoan
                //          select new
                //          {
                //              AuditDescription = l.AuditDescription,
                //              AuditDateTime = l.AuditDateTime
                //          })
                //          .Union(
                //                from al in _auditDetail
                //                select new
                //                {
                //                    AuditDescription = al.AuditDescription,
                //                    AuditDateTime = al.AuditDateTime
                //                }
                //                ).ToList<object>();

                return _audit;
            }
        }

        #endregion


        #region Private Properties        

        private List<DocumentObject> DocumentsInLoan
        {
            get
            {
                List<DocumentObject> docObj = new List<DocumentObject>();

                foreach (Documents doc in batch.Documents)
                {
                    DocumentTypeMaster docM = docTypes.Where(d => d.DocumentTypeID == doc.DocumentTypeID).FirstOrDefault();
                    string displayName = docM != null ? docM.DisplayName : "";

                    if (doc.DocumentTypeID != 0)
                    {
                        docObj.Add(new DocumentObject()
                        {
                            DocTypeID = doc.DocumentTypeID,
                            DocTypeName = displayName.Equals(string.Empty) ? doc.Type : displayName,
                            VersionNumber = doc.VersionNumber,
                            Obsolete = doc.Obsolete
                        });
                    }
                }

                return docObj;


                //return batch.Documents.Select(b => new
                //{
                //    key = b.DocumentTypeID,
                //    value = b.Type                   
                //}).OrderBy(d => d.key).ToDictionary(d => d.key, d => d.value);
            }
        }

        private void setClassProperties()
        {
            if (loan != null)
            {
                Logger.WriteTraceLog($"Before setClassProperties");
                batch = JsonConvert.DeserializeObject<Batch>(loan.LoanDetails.LoanObject);

                loanDocTypes = docTypes.ToDictionary(d => d.DocumentTypeID, d => d.DisplayName);
                Logger.WriteTraceLog($"Before setClassProperties"); //ruleEngineDataAccess.GetLoanDocTypes(loan.CustomerID, loan.LoanTypeID);

                //   CheckListRules = ruleEngineDataAccess.GetCheckList(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);

                //  CheckListDetails = ruleEngineDataAccess.GetCheckListDetails(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                Logger.WriteTraceLog($"Before GetStackingOrder");
                stackingOrder = ruleEngineDataAccess.GetStackingOrder(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                Logger.WriteTraceLog($"Before GetStackingOrderDetails");

                loanStackingDetails = ruleEngineDataAccess.GetStackingOrderDetails(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);

                //        FormBatchDocFields();

                Logger.WriteTraceLog($"Before GetMissingDocumentWithLevel");
                GetMissingDocumentWithLevel();

                Logger.WriteTraceLog($"Before setBatchDocTypes");
                setBatchDocTypes();

                Logger.WriteTraceLog($"After setBatchDocTypes");
                //  CheckListItems = ruleEngineDataAccess.GetCheckListDetail(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                // checkList = CheckListItems;
            }
        }

        private void setBatchDocTypes()
        {
            //ls = new List<Dictionary<string, string>>();
            _batchDocumentObject = new List<BatchDocumentObject>();
            //Dictionary<string, string> docName = null;


            Dictionary<int, Dictionary<long, string>> dic = new Dictionary<int, Dictionary<long, string>>();
            Dictionary<long, string> docDic = new Dictionary<long, string>();
            List<DocumentObject> docObj = new List<DocumentObject>();

            string fieldName = string.Empty;
            string fieldValue = string.Empty;
            Dictionary<string, object> batchDocFieldsDic = new Dictionary<string, object>();
            MTSRules missingDocRules = new MTSRules();

            foreach (Documents doc in batch.Documents)
            {
                DocumentTypeMaster currtDoc = docTypes.Where(d => d.DocumentTypeID == doc.DocumentTypeID).FirstOrDefault();
                string displayName = currtDoc != null ? currtDoc.DisplayName : string.Empty; // ruleEngineDataAccess.GetDocumentTypeName(doc.DocumentTypeID);
                //bool _isdocorderbyfield = false;
                string _fieldGroupValue = string.Empty;
                StackandGroupDetails _stackandGroupDetailField = stackingOrder.Where(s => s.DocumentID == doc.DocumentTypeID && s.StackingGroupId > 0).FirstOrDefault();

                if (_stackandGroupDetailField != null)
                {
                    _fieldGroupValue = string.IsNullOrEmpty(_stackandGroupDetailField.StackingOrderFieldName) ? string.Empty : doc.DocumentLevelFields.Where(f => f.Name == _stackandGroupDetailField.StackingOrderFieldName).Select(f => f.Value).FirstOrDefault();

                }

                //if DocID === 0 then wont displays on LoanInfo->Document List(Stacking Order) 
                //if (doc.DocumentTypeID != 0)
                //{
                //string _fieldName = doc.DocumentTypeID != 0 ? ruleEngineDataAccess.GetOrderByFieldName(doc.DocumentTypeID) : string.Empty;
                DocumentFieldMaster docField = currtDoc != null ? currtDoc.DocumentFieldMasters.Where(f => f.DocOrderByField != null).FirstOrDefault() : null;
                string _fieldName = doc.DocumentTypeID != 0 ? docField == null ? string.Empty : docField.Name : string.Empty;

                string _fieldValue = string.IsNullOrEmpty(_fieldName) ? string.Empty : doc.DocumentLevelFields.Where(f => f.Name == _fieldName).Select(f => f.Value).FirstOrDefault();
                _documentfieldmaster = currtDoc != null ? currtDoc.DocumentFieldMasters.Where(f => f.IsDocName == true).FirstOrDefault() : null;
                //_documentfieldmaster =ruleEngineDataAccess.GetFieldValueByDocId(doc.DocumentTypeID, out _isdocorderbyfield);
                docObj.Add(new DocumentObject()
                {
                    DocTypeID = doc.DocumentTypeID,
                    DocTypeName = displayName.Equals(string.Empty) ? doc.Description : displayName,
                    //DocTypeName = displayName.Equals(string.Empty) ? doc.Type : displayName,
                    VersionNumber = doc.VersionNumber == 0 ? 1 : doc.VersionNumber,
                    OrderByField = _fieldName,
                    OrderByFieldValue = _fieldValue,
                    StackGroupValue = _fieldGroupValue,
                    DocFieldName = (_documentfieldmaster != null) ? _documentfieldmaster.DisplayName : "",
                    IsDocName = (_documentfieldmaster != null) ? _documentfieldmaster.IsDocName.GetValueOrDefault() : false,
                    DocValue = (_documentfieldmaster != null && _documentfieldmaster.FieldID != 0) ? doc.DocumentLevelFields.Where(x => x.FieldID == _documentfieldmaster.FieldID).Select(x => x.Value).FirstOrDefault() : "",
                    Obsolete = doc.Obsolete
                });
                //}
            }


            var batchDocs = (from bDoc in docObj
                             select new
                             {
                                 DocID = bDoc.DocTypeID,
                                 DocName = bDoc.DocTypeName,
                                 DocumentLevel = 100,
                                 VersionNumber = bDoc.VersionNumber,
                                 OrderByFieldValue = bDoc.OrderByFieldValue,
                                 OrderByField = bDoc.OrderByField,
                                 DocFieldName = bDoc.DocFieldName,
                                 IsDocName = bDoc.IsDocName,
                                 DocValue = bDoc.DocValue,
                                 StackGroupValue = bDoc.StackGroupValue,
                                 Obsolete = bDoc.Obsolete
                             }).ToList().Select(a => new MissingDocumentObject()
                             {
                                 DocID = a.DocID,
                                 DocName = a.DocName,
                                 DocumentLevel = a.DocumentLevel,
                                 VersionNumber = a.VersionNumber,
                                 OrderByFieldValue = a.OrderByFieldValue,
                                 OrderByField = a.OrderByField,
                                 DocFieldName = a.DocFieldName,
                                 IsDocName = a.IsDocName,
                                 DocValue = a.DocValue,
                                 StackGroupValue = a.StackGroupValue,
                                 orderbyvalue = a.OrderByFieldValue,
                                 Obsolete = a.Obsolete,
                                 IDCUrl = "",
                                 IDCStatus = ""
                             }).ToList();


            List<Documents> batchDocFields = (from docs in batch.Documents
                                              where docs.Obsolete == false
                                              group docs by docs.DocumentTypeID into docGroup
                                              select docGroup.OrderByDescending(p => p.VersionNumber).First()).ToList<Documents>();


            var _mdocMaster = docTypes.Where(x => _missingDocs.Any(y => x.DocumentTypeID == y.DocID)).ToList();

            foreach (var mdoc in _mdocMaster)
            {
                if (mdoc.CustDocumentLevel == DocumentLevelConstant.CRITICAL && !string.IsNullOrEmpty(mdoc.Condition))
                {
                    dynamic docCondition = JsonConvert.DeserializeObject(mdoc.Condition);
                    if (!string.IsNullOrEmpty(docCondition["formula"].ToString()))
                        missingDocRules.Add(mdoc.DocumentTypeID.ToString(), docCondition["formula"].ToString());
                }
            }

            foreach (Documents doc in batchDocFields)
            {
                if (doc.DocumentTypeID != 0)
                {
                    foreach (var docFields in doc.DocumentLevelFields)
                    {
                        fieldName = String.Format("{0}.{1}", doc.Type, docFields.Name);
                        fieldValue = string.IsNullOrEmpty(docFields.Value) ? string.Empty : docFields.Value;

                        try
                        {
                            decimal roundVal = Math.Round(Convert.ToDecimal(fieldValue.Replace("%", "").Replace("$", "").Replace(",", "").Trim()));
                            batchDocFieldsDic[fieldName] = roundVal;
                        }
                        catch (Exception ex)
                        {
                            batchDocFieldsDic[fieldName] = fieldValue;
                            continue;
                        }

                    }
                }
            }

            Logger.WriteTraceLog($"Before Eval");
            Logger.WriteTraceLog($"batchDocFieldsDic : {JsonConvert.SerializeObject(batchDocFieldsDic)}");
            Logger.WriteTraceLog($"missingDocRules : {JsonConvert.SerializeObject(missingDocRules)}");
            Dictionary<string, MTSRuleResult> output = missingDocRules.Eval(batchDocFieldsDic);
            Logger.WriteTraceLog($"After Eval");

            foreach (var item in _missingDocs)
            {
                bool ruleResult = false;

                if (output.ContainsKey(item.DocID.ToString()))
                    bool.TryParse(output[item.DocID.ToString()].Result.ToString(), out ruleResult);

                if (ruleResult == false)
                {
                    ruleResult = _mdocMaster.Any(x => x.DocumentTypeID == item.DocID && x.CustDocumentLevel == DocumentLevelConstant.CRITICAL && string.IsNullOrEmpty(x.Condition));
                }
                item.DocumentLevel = ruleResult ? DocumentLevelConstant.CRITICAL : DocumentLevelConstant.NON_CRITICAL;
            }

            var allDocTypes = batchDocs.Union(_missingDocs).ToList();
            //var _newList = allDocTypes.Where(ald => stackingOrder.Any(s => ald.DocID == s.Id)).ToList();
            _batchDocumentObject = (from bDoc in allDocTypes
                                    join so in stackingOrder on bDoc.DocID equals so.DocumentID into stackOrd
                                    from sOrd in stackOrd.DefaultIfEmpty()
                                    select new BatchDocumentObject
                                    {
                                        DocID = bDoc.DocID.ToString(),
                                        DocName = bDoc.DocName,
                                        DocumentLevelID = bDoc.DocumentLevel,
                                        DocumentLevel = bDoc.DocumentLevel.Equals(100) ? "In Loan" : DocumentLevelConstant.GetDocumentLevelDescription(bDoc.DocumentLevel),
                                        DocumentLevelIcon = bDoc.DocumentLevel.Equals(100) ? "Success" : DocumentLevelConstant.GetDocumentLevelIcons(bDoc.DocumentLevel),
                                        DocumentLevelIconColor = bDoc.DocumentLevel.Equals(100) ? "Success" : DocumentLevelConstant.GetDocumentLevelIconColor(bDoc.DocumentLevel),
                                        SequenceID = sOrd?.SequenceNumber ?? 0,
                                        VersionNumber = bDoc.VersionNumber,
                                        OrderByFieldValue = bDoc?.OrderByFieldValue ?? string.Empty,
                                        //OrderByField = bDoc?.OrderByField ?? string.Empty,
                                        FieldOrderBy = bDoc?.OrderByField ?? string.Empty,
                                        DocFieldName = bDoc?.DocFieldName ?? string.Empty,
                                        IsDocName = bDoc.IsDocName,
                                        DocValue = bDoc?.DocValue ?? string.Empty,
                                        StackingGroupId = sOrd?.StackingGroupId ?? 0,
                                        StackingOrderGroupName = sOrd?.StackingOrderGroupName ?? string.Empty,
                                        StackingOrderFieldName = sOrd?.StackingOrderFieldName ?? string.Empty,
                                        StackingOrderFieldValue = bDoc.StackGroupValue,
                                        IsGroup = (sOrd?.StackingGroupId ?? 0) > 0,
                                        StackingOrderGroupDetails = sOrd?.StackingOrderGroupDetails ?? new string[] { "" },
                                        orderbyvalue = bDoc?.orderbyvalue ?? string.Empty,
                                        Obsolete = bDoc.Obsolete,
                                        IDCUrl = bDoc.IDCUrl,
                                        IDCStatus = bDoc.IDCStatus
                                    }).ToList();

            _batchDocumentObject = _batchDocumentObject.OrderByDescending(o => o.SequenceID != 0)
                .ThenBy(o => o.SequenceID)
            .ThenByDescending(o => o.VersionNumber).ToList()
            .Select(a => new BatchDocumentObject
            {
                DocID = a.DocID,
                DocName = a.DocName,
                DocumentLevel = a.DocumentLevel,
                DocumentLevelIcon = a.DocumentLevelIcon,
                DocumentLevelIconColor = a.DocumentLevelIconColor,
                DocumentLevelID = a.DocumentLevelID,
                SequenceID = a.SequenceID,
                VersionNumber = a.VersionNumber,
                FieldOrderBy = a.FieldOrderBy,
                OrderByFieldValue = a.OrderByFieldValue,
                FieldOrderVersion = 0,
                DocFieldName = a.DocFieldName,
                IsDocName = a.IsDocName,
                DocValue = a.DocValue,
                StackingGroupId = a.StackingGroupId,
                StackingOrderGroupName = a.StackingOrderGroupName,
                StackingOrderFieldName = a.StackingOrderFieldName,
                StackingOrderFieldValue = a.StackingOrderFieldValue,
                IsGroup = a.IsGroup,
                StackingOrderGroupDetails = a.StackingOrderGroupDetails,
                orderbyvalue = a.orderbyvalue.Replace("$", ""),
                Obsolete = a.Obsolete,
                IDCUrl = a.IDCUrl,
                IDCStatus = a.IDCStatus
            }).ToList();




            var _distDocuments = _batchDocumentObject.Select(d => d.DocID).Distinct().ToList();

            List<BatchDocumentObject> _newObj = new List<BatchDocumentObject>();

            foreach (string item in _distDocuments)
            {
                //foreach(var x  in _batchDocumentObject)
                //{
                //    x.orderbyvalue = x.orderbyvalue.Replace("$", "");
                //}
                var _orderedObj = _batchDocumentObject.Where(d => d.DocID == item).OrderBy(o => o.FieldOrderBy != string.Empty).ThenBy(o => o, new SortByFiled("orderbyvalue")).ToList();
                int versionNo = 1;
                foreach (var _ordDocs in _orderedObj)
                {
                    _ordDocs.FieldVersionNumber = versionNo;
                    versionNo++;
                    _newObj.Add(_ordDocs);
                }
            }

            _batchDocumentObject = _newObj.GroupBy(x => x.DocID)
                                        .SelectMany(g =>
                                           g.Select((y, z) => new BatchDocumentObject
                                           {
                                               DocID = y.DocID,
                                               DocName = y.DocName,
                                               DocumentLevel = y.DocumentLevel,
                                               DocumentLevelIcon = y.DocumentLevelIcon,
                                               DocumentLevelIconColor = y.DocumentLevelIconColor,
                                               DocumentLevelID = y.DocumentLevelID,
                                               SequenceID = y.SequenceID,
                                               VersionNumber = y.VersionNumber,
                                               FieldOrderBy = y.FieldOrderBy,
                                               OrderByFieldValue = y.OrderByFieldValue,
                                               FieldOrderVersion = y.FieldVersionNumber,
                                               DocFieldName = y.DocFieldName,
                                               IsDocName = y.IsDocName,
                                               DocValue = y.DocValue,
                                               StackingGroupId = y.StackingGroupId,
                                               StackingOrderGroupName = y.StackingOrderGroupName,
                                               StackingOrderFieldName = y.StackingOrderFieldName,
                                               StackingOrderFieldValue = y.StackingOrderFieldValue,
                                               IsGroup = y.IsGroup,
                                               StackingOrderGroupDetails = y.StackingOrderGroupDetails,
                                               Obsolete = y.Obsolete,
                                               IDCUrl = y.IDCUrl,
                                               IDCStatus = y.IDCStatus
                                           })
                                       ).ToList();
        }

        private void GetMissingDocumentWithLevel()
        {
            _missingDocs = new List<MissingDocumentObject>();

            List<Dictionary<string, string>> _dic = GetMissingDocumentsInLoan;

            _missingDocs = (from mDoc in _dic
                            join docTs in docTypes on Convert.ToInt64(mDoc["DocID"]) equals docTs.DocumentTypeID
                            select new
                            {
                                DocID = Convert.ToInt64(mDoc["DocID"]),
                                DocName = mDoc["DocName"],
                                DocumentLevel = docTs.DocumentLevel,
                                VersionNumber = 0
                            }).ToList().Select(a => new MissingDocumentObject()
                            {
                                DocID = a.DocID,
                                DocName = a.DocName,
                                DocumentLevel = a.DocumentLevel,
                                VersionNumber = a.VersionNumber
                            }).ToList();

            List<AuditLoanMissingDoc> _auditMissDoc = ruleEngineDataAccess.GetMissingDocumentUploaded(loan.LoanID);

            foreach (var item in _missingDocs)
            {
                if (_auditMissDoc.Any(a => a.DocID == item.DocID))
                {
                    AuditLoanMissingDoc _auditLoan = _auditMissDoc.Where(a => a.DocID == item.DocID).FirstOrDefault();
                    item.IDCStatus = _auditLoan.Status.ToString();
                    item.IDCUrl = string.IsNullOrEmpty(_auditLoan.IDCBatchInstanceID) ? string.Empty : _auditLoan.IDCBatchInstanceID;
                }
                else
                {
                    item.IDCStatus = string.Empty;
                    item.IDCUrl = string.Empty;
                }
            }


        }

        #endregion      
    }

    class SortByFiled : IComparer<object>
    {
        private string SortField { get; set; }

        private bool SortEmptyFirst { get; set; }
        public int Compare(object x, object y)
        {
            object _tempX = null;
            object _tempY = null;

            if (x.GetType().ToString() == "System.Data.DataRow" && y.GetType().ToString() == "System.Data.DataRow")
            {
                _tempX = ((DataRow)x)[SortField];
                _tempY = ((DataRow)y)[SortField];
            }
            else
            {
                _tempX = x.GetType().GetProperty(SortField).GetValue(x, null);
                _tempY = y.GetType().GetProperty(SortField).GetValue(y, null);
            }

            string xValue = _tempX == null ? string.Empty : _tempX.ToString();
            string yValue = _tempY == null ? string.Empty : _tempY.ToString();

            DateTime xDate, yDate;
            bool xIsVal = DateTime.TryParse(xValue, out xDate);
            bool yIsVal = DateTime.TryParse(yValue, out yDate);

            if (xIsVal && yIsVal)
            {
                return yDate.CompareTo(xDate);//Date is desending order
            }

            if (xIsVal)
                return SortEmptyFirst ? 1 : -1;


            Decimal xDecimal, yDecimal;
            xIsVal = Decimal.TryParse(xValue, out xDecimal);
            yIsVal = Decimal.TryParse(yValue, out yDecimal);


            if (xIsVal && yIsVal)
            {
                return yDecimal.CompareTo(xDecimal);//Decimal is asending oder
            }

            if (xIsVal)
                return SortEmptyFirst ? 1 : -1;

            if (!xIsVal && !yIsVal)
                return yValue.CompareTo(xValue); //String is desending oder

            return SortEmptyFirst ? -1 : 1;

        }

        public SortByFiled(string sortByProperty, bool sortEmptyFirst = false)
        {
            SortField = sortByProperty;
            SortEmptyFirst = sortEmptyFirst;
        }

    }

    class DocumentObject
    {
        public Int64 DocTypeID { get; set; }
        public string DocTypeName { get; set; }
        public int VersionNumber { get; set; }
        public string OrderByFieldValue { get; set; }
        public string OrderByField { get; set; }
        public string DocFieldName { get; set; }
        public bool IsDocName { get; set; }
        public string DocValue { get; set; }
        public string StackGroupValue { get; set; }
        public bool Obsolete { get; set; }

    }

    public class MissingDocumentObject
    {
        public Int64 DocID { get; set; }
        public string DocName { get; set; }
        public int DocumentLevel { get; set; }
        public int VersionNumber { get; set; }
        public string OrderByFieldValue { get; set; }
        public string OrderByField { get; set; }
        public string DocFieldName { get; set; }
        public bool IsDocName { get; set; }
        public string DocValue { get; set; }
        public string StackGroupValue { get; set; }
        public string orderbyvalue { get; set; }
        public bool Obsolete { get; set; }
        public string IDCUrl { get; set; }
        public string IDCStatus { get; set; }
    }

    public class BatchDocumentObject
    {
        public string DocID { get; set; }
        public string DocName { get; set; }
        public int DocumentLevelID { get; set; }
        public string DocumentLevel { get; set; }
        public string DocumentLevelIcon { get; set; }
        public string DocumentLevelIconColor { get; set; }
        public long SequenceID { get; set; }
        public int VersionNumber { get; set; }
        public string FieldOrderBy { get; set; }
        public string OrderByFieldValue { get; set; }
        public int FieldOrderVersion { get; set; }
        public string DocFieldName { get; set; }
        public bool IsDocName { get; set; }
        public string DocValue { get; set; }
        public Int64 StackingGroupId { get; set; }
        public string StackingOrderGroupName { get; set; }
        public string StackingOrderFieldName { get; set; }
        public string StackingOrderFieldValue { get; set; }
        public string[] StackingOrderGroupDetails { get; set; }
        public bool IsGroup { get; set; }
        public Int32 FieldVersionNumber { get; set; }
        public string orderbyvalue { get; set; }
        public bool Obsolete { get; set; }
        public string IDCUrl { get; set; }
        public string IDCStatus { get; set; }
    }

    public class ChecklistRuleMaster
    {
        public CheckListDetailMaster ChecklistDetail { get; set; }
        public RuleMaster RuleDescription { get; set; }
    }

    public class StackandGroupDetails
    {
        public Int64 DocumentID { get; set; }
        public Int64 SequenceNumber { get; set; }
        public Int64 StackingGroupId { get; set; }
        public string StackingOrderGroupName { get; set; }
        public string StackingOrderFieldName { get; set; }
        public string StackingOrderFieldValue { get; set; }
        public string[] StackingOrderGroupDetails { get; set; }
    }
}
