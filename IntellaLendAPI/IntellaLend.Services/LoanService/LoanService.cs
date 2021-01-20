using IntellaLend.BoxWrapper;
using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using IntellaLend.RuleEngine;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace IntellaLend.CommonServices
{
    public class LoanService
    {
        private string TenantSchema;

        #region Constructor

        public LoanService()
        { }

        public LoanService(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion


        #region Public Methods

        //public List<LoanSearch> GetLoans(string LoanNumber,Int64 LoanType,DateTime FromDate,DateTime ToDate, string BorrowerName, decimal LoanAmount,Int64 ReviewStatus)
        //{
        //    return new LoanDataAccess(TenantSchema).GetLoans(LoanNumber, LoanType, FromDate, ToDate, BorrowerName, LoanAmount, ReviewStatus);
        //}

        public object GetLoans(
            DateTime FromDate,
            DateTime ToDate,
            Int64 CurrentUserID,
            string LoanNumber,
            Int64 LoanType,
            string BorrowerName,
            string LoanAmount,
            Int64 ReviewStatus,
            DateTime? AuditMonthYear,
            Int64 ReviewType,
            Int64 Customer,
            string PropertyAddress,
            string InvestorLoanNumber,
            string PostCloser,
            string LoanOfficer,
            string UnderWriter,
            bool isWorkFlow,
            DateTime AuditDueDate,
            string[] SelectedLoanStatus
            )
        {
            return new LoanDataAccess(TenantSchema).GetLoans
                (
                FromDate,
                ToDate,
                CurrentUserID,
                LoanNumber,
                LoanType,
                BorrowerName,
                LoanAmount,
                ReviewStatus,
                AuditMonthYear,
                ReviewType,
                Customer,
                PropertyAddress,
                InvestorLoanNumber,
                PostCloser,
                LoanOfficer,
                UnderWriter,
                isWorkFlow,
                AuditDueDate,
                SelectedLoanStatus
                );
        }

        public void SetLoanPickUpUser(Int64 LoanID, Int64 PickUpUserID)
        {
            new LoanDataAccess(TenantSchema).SetLoanPickUpUser(LoanID, PickUpUserID);
        }

        public bool RemoveLoanLoggedUser(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).RemoveLoanLoggedUser(LoanID);
        }

        public object CheckCurrentLoanUser(Int64 LoanID, Int64 CurrentUserID)
        {
            return new LoanDataAccess(TenantSchema).CheckCurrentLoanUser(LoanID, CurrentUserID);
        }

        public bool CheckLoanPDFExists(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).CheckLoanPDFExists(LoanID);
        }

        public object GetRetentionLoans(DateTime fromDate, DateTime toDate)
        {
            return new LoanDataAccess(TenantSchema).GetRetentionLoans(fromDate, toDate);
        }
        public List<FannieMaeFields> GetFannieMaeFields(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetFannieMaeFields(LoanID);
        }

        public object GetRetentionLoans(DateTime AuditMonthYear)
        {
            return new LoanDataAccess(TenantSchema).GetRetentionLoans(AuditMonthYear);
        }

        public List<object> GetLoanAudit(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanAudit(LoanID);

        }

        public object GetEvaluatedChecklist(Int64 LoanID, bool ReRun)
        {
            CheckListResult result = new CheckListResult();
            LoanDataAccess _loanData = new LoanDataAccess(TenantSchema);

            string evaluatedResult = _loanData.GetEvaluatedResult(LoanID);
            if (!string.IsNullOrEmpty(evaluatedResult))
                result = JsonConvert.DeserializeObject<CheckListResult>(evaluatedResult);
            else
                ReRun = true;

            if (ReRun)
            {
                EvaluateRules ruleEngine = new EvaluateRules(TenantSchema, LoanID);
                result.loanQuestioner = _loanData.GetLoanQuestioner(ruleEngine.loan.LoanID, ruleEngine.loan.CustomerID, ruleEngine.loan.ReviewTypeID, ruleEngine.loan.LoanTypeID);
                result.allChecklist = ruleEngine.GetAllCheckListDetails;
                ruleEngine.InsertLoanCheckListAuditDetails(ruleEngine.FetchLoanCheckListDetails(result.allChecklist));
                _loanData.UpdateEvalResult(LoanID, JsonConvert.SerializeObject(result));
            }

            return result;
        }

        public object GetLoanDetails(Int64 LoanID, bool IsChangeDocumentType = false)
        {
            LoanDataAccess _dataAccess = new LoanDataAccess(TenantSchema);
            LoanRuleEngine ruleEngine = new LoanRuleEngine(TenantSchema, LoanID);

            if (IsChangeDocumentType)
            {
                List<Dictionary<string, string>> _missingDocs = ruleEngine.GetMissingDocumentsInLoan;
                ReportMaster _reportMaster = _dataAccess.GetReportMasterDetails();
                if (_reportMaster != null && _missingDocs.Count > 0)
                {
                    List<ReportConfig> reportConfig = _dataAccess.GetReportMasterDocumentNames(_reportMaster.ReportMasterID);
                    _dataAccess.RemoveLoanReportingEntries(ruleEngine.loan.LoanID);
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
                }

            }

            return new
            {
                LoanID = LoanID,
                LoanTypeID = ruleEngine.loan.LoanTypeID,
                LoanHeaderInfo = ruleEngine.GetLoanHeaderInfo,
                Status = ruleEngine.loan.Status,
                loanDocuments = ruleEngine.batchDocTypes,
                //missingDocuments = ruleEngine.GetMissingDocumentsInLoan,
                missingDocuments = ruleEngine.GetMissingDocumentStatus,
                //failedChecklist = ruleEngine.GetFailedCheckList,
                //allChecklist = ruleEngine.GetAllCheckListDetails,
                //loanQuestioner = checkListQuestion,
                loanAudit = ruleEngine.GetLoanAudit,
                //totalCheckListCount = ruleEngine.TotalCheckListCount,
                //showDownload = ruleEngine.IsLoanPDFGenerated,
                loanStackingOrder = ruleEngine.LoanStackingDetails,
                reverificationCount = ruleEngine.ReverificationCount,
                success = true
            };
        }

        public bool PurgeStaging(PurgeStaging reqLoanPurge, long[] loanID, string userName)
        {
            reqLoanPurge.CreatedOn = DateTime.Now;
            reqLoanPurge.ModifiedOn = DateTime.Now;
            LoanDataAccess loanDataAccess = new LoanDataAccess(TenantSchema);
            Int64 batchID = loanDataAccess.PurgeStaging(reqLoanPurge);
            foreach (var loanid in loanID)
            {
                loanDataAccess.PurgeStagingDetails(loanid, batchID, reqLoanPurge.Status);
                loanDataAccess.UpdatePurgeWaiting(loanid, userName, reqLoanPurge.Status);
            }
            return true;
        }
        public object GetPurgeStagingDetails(long batchID)
        {
            return new LoanDataAccess(TenantSchema).GetPurgeStagingDetails(batchID);
        }
        public object GetLoanDocInfo(Int64 LoanID, Int64 DocumentID, int VersionNumber)
        {
            return new LoanDataAccess(TenantSchema).GetLoanDocInfo(LoanID, DocumentID, VersionNumber.ToString());
        }

        public object GetLoanReverifyDoc(Int64 LoanID, Int64 DocumentID, int VersionNumber)
        {
            return new LoanDataAccess(TenantSchema).GetLoanReverifyDoc(LoanID, DocumentID, VersionNumber.ToString());
        }


        public object GetLoanDocInfo(Int64 LoanID, Int64 DocumentID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanDocInfo(LoanID, DocumentID);
        }

        public string GetXlFileString(Int64 LoanID, List<ChecklistObject> _loanChecklists)
        {
            LoanDataAccess _loanDataAccess = new LoanDataAccess(TenantSchema);
            Loan _loan = _loanDataAccess.GetLoanHeaderDeatils(LoanID);
            string _loanTypeName = _loanDataAccess.GetLoanTypeName(_loan.LoanTypeID);
            string _serviceTypeName = _loanDataAccess.GetServiceTypeName(_loan.ReviewTypeID);

            List<string> exportCSV = new List<string>() {
                "Loan Type",
                "Service Type",
                "Document Type(s)",
                "Checklist Item Name",
                "Description",
                "Result"
            };


            StringBuilder str = new StringBuilder();
            str.AppendLine(string.Join(",", exportCSV));
            //str.Append("<table>");
            //str.Append("<tr>");
            //str.Append("<td><b><font face=Arial Narrow size=3>Loan Type</font></b></td>");
            //str.Append("<td><b><font face=Arial Narrow size=3>Service Type</font></b></td>");
            //str.Append("<td><b><font face=Arial Narrow size=3>Document Type(s)</font></b></td>");
            //str.Append("<td><b><font face=Arial Narrow size=3>Checklist Item Name</font></b></td>");
            //str.Append("<td><b><font face=Arial Narrow size=3>Description</font></b></td>");
            //str.Append("<td><b><font face=Arial Narrow size=3>Result</font></b></td>");
            //str.Append("</tr>");

            Regex _regex = new Regex(@"(?=\[).+?(?=\])");

            foreach (ChecklistObject val in _loanChecklists)
            {

                var _tempDocFields = _regex.Matches(val.Formula);
                List<string> docs = new List<string>();
                if (_tempDocFields != null)
                {
                    foreach (var docField in _tempDocFields)
                    {
                        string _newDocField = docField.ToString().Substring(1, docField.ToString().Length - 1);
                        string[] docFieldArry = _newDocField.Split('.');
                        if (docFieldArry.Length > 1)
                            docs.Add(docFieldArry[0]);
                    }
                }

                docs = docs.Distinct().ToList();
                string _result = val.Result == "True" ? "Pass" : "Fail";
                //str.Append("<tr>");
                //str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + _loanTypeName + "</font></td>");
                //str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + _serviceTypeName + "</font></td>");
                //str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + string.Join(",", docs) + "</font></td>");
                //str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.CheckListName + "</font></td>");
                //str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + val.Formula + "</font></td>");
                //str.Append("<td><font face=Arial Narrow size=" + "14px" + ">" + _result + "</font></td>");
                //str.Append("</tr>");
                List<string> _newRow = new List<string>() {
                    _loanTypeName.Replace(",", "|"),
                    _serviceTypeName.Replace(",",  "|"),
                     string.Join("", docs).Replace(",", "|"),
                     val.CheckListName.Replace(",",  "|"),
                     val.Formula.Replace(",",  "|"),
                     _result.Replace(",",  "|")
                };
                str.AppendLine(string.Join(",", _newRow));
            }

            //str.Append("</table>");
            return str.ToString();
        }

        public byte[] GetLoanPDF(Int64 LoanID)
        {
            byte[] loanPdf = new LoanDataAccess(TenantSchema).GetLoanPDF(LoanID);
            //string loanPdfPath = new LoanDataAccess(TenantSchema).GetLoanPDF(LoanID);

            //if (!(loanPdfPath.Equals(string.Empty)))
            //    return File.ReadAllBytes(loanPdfPath);

            //return new byte[0];

            return loanPdf;
        }

        public Stream GetLoanPDFStream(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanPDFStream(LoanID);
        }
        public byte[] GetDocumentPDF(Int64 LoanID, Int64 DocumentID, Int64 VersionNumber)
        {
            return new LoanDataAccess(TenantSchema).GetDocumentPDF(LoanID, DocumentID, Convert.ToString(VersionNumber));
        }


        public byte[] GetReverificationLoanPDF(Int64 LoanID, Int64 MappingID, string TemplateJson, Int64 UserID, string RequiredDocIDs, bool IsCoverLetterReq, string ReverificationName)
        {
            //string loanPdfPath = new LoanDataAccess(TenantSchema).GetReverificationLoanPDF(LoanID);

            //if (!(loanPdfPath.Equals(string.Empty)))
            //    return File.ReadAllBytes(loanPdfPath);

            return new LoanDataAccess(TenantSchema).GetReverificationLoanPDF(LoanID, MappingID, TemplateJson, UserID, RequiredDocIDs, IsCoverLetterReq, ReverificationName);
        }

        public byte[] GetReverificationLoanPDF(Int64 LoanReverificationID)
        {
            return new LoanDataAccess(TenantSchema).GetReverificationLoanPDF(LoanReverificationID);
        }


        //this method created by Manikandan
        public string GetLoanNotes(Int64 LoanID)
        {
            string loanNotes = new LoanDataAccess(TenantSchema).GetLoanNotes(LoanID);

            if (!(loanNotes.Equals(string.Empty)))
                return loanNotes;

            return string.Empty;
        }


        public bool UpdateLoanNotes(Int64 LoanID, string LoanNotes)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanNotes(LoanID, LoanNotes);
        }

        public Loan GetLoanHeaderDeatils(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanHeaderDeatils(LoanID);
        }

        public object UpdateLoanDocument(Int64 LoanID, Int64 DocumentID, Int64 CurrentUserID, List<DocumentLevelFields> DocumentFields, Int32 VersionNumber, List<DataTable> DocumentTables)
        {
            bool result = new LoanDataAccess(TenantSchema).UpdateLoanDocument(LoanID, DocumentID, CurrentUserID, DocumentFields, VersionNumber, DocumentTables);

            if (result)
                return GetLoanDetails(LoanID);
            else
                return new { success = false };
        }

        public object GetImageByID(Int64 ImageID)
        {
            return new LoanDataAccess(TenantSchema).GetImageByID(ImageID);
        }

        public object GetImageByID(Int64 LoanID, Int64 DocumentID, Int64 ImageID, int VersionNumber)
        {
            return new LoanDataAccess(TenantSchema).GetImageByID(LoanID, DocumentID, ImageID, VersionNumber.ToString());
        }

        public object GetLoanBase64Images(Int64 LoanID, Int64 DocumentID, int versionNumber, long _pageNo, long lastPageNumber, Boolean ShowAllDocs)
        {
            return new LoanDataAccess(TenantSchema).GetLoanBase64Images(LoanID, DocumentID, versionNumber, _pageNo, lastPageNumber, ShowAllDocs);
        }

        public byte[] GetLoanImage(Int64 LoanID, string LoanGuid)
        {
            return new LoanDataAccess(TenantSchema).GetLoanImage(LoanID, LoanGuid);
        }


        public object GetLoanBase64ImageByPageNo(Int64 LoanID, Int64 DocumentID, string VersionNumber, Int64 pageNo)
        {
            return new LoanDataAccess(TenantSchema).GetLoanBase64ImageByPageNo(LoanID, DocumentID, VersionNumber, pageNo);
        }

        public object GetPurgeMonitor(DateTime fromDate, DateTime toDate, long workFlowStatus)
        {
            return new LoanDataAccess(TenantSchema).GetPurgeMonitor(fromDate, toDate, workFlowStatus);
        }

        public object GetMissingDocStatus(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetMissingDocStatus(LoanID);
        }

        public object GetMissingDocVersion(Int64 LoanID, Int64 DocID)
        {
            return new LoanDataAccess(TenantSchema).GetMissingDocVersion(LoanID, DocID);
        }

        public object ChangeDocumentType(Int64 LoanID, Int64 OldDocumentID, Int64 NewDocumentID, int VersionNumber, Int64 CurrentUserID)
        {
            bool result = new LoanDataAccess(TenantSchema).ChangeDocumentType(LoanID, OldDocumentID, NewDocumentID, VersionNumber, CurrentUserID);

            if (result)
                return GetLoanDetails(LoanID, true);
            else
                return new { success = false };
        }
        public bool RetryPurge(long[] batchID)
        {
            return new LoanDataAccess(TenantSchema).RetryPurge(batchID);
        }
        public object GetLoanReverification(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanReverification(LoanID);
        }

        public object GetLoanBasedReverification(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanBasedReverification(LoanID);
        }

        public List<string> GetFieldsByDocID(Int64 DocumentID)
        {
            return new LoanDataAccess(TenantSchema).GetFieldsByDocID(DocumentID);
        }

        public string GetFieldValue(Int64 LoanID, string DocumentName, string DocumentField)
        {
            return new LoanDataAccess(TenantSchema).GetFieldValue(LoanID, DocumentName, DocumentField);
        }

        public Object GetQCIQLookupDetails(Int64 loanID)
        {
            return new LoanDataAccess(TenantSchema).GetQCIQLookupDetails(loanID);
        }
        public bool UpdateLoanQuestioner(Int64 LoanID, List<ManualQuestioner> questioner, Int64 CurrentUserID)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanQuestioner(LoanID, questioner, CurrentUserID);
        }

        public bool UpdateLoanTypeFromQCIQ(Int64 LoanID, string loanType, DateTime? QCIQStartDate)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanTypeFromQCIQ(LoanID, loanType, QCIQStartDate);
        }
        #region MAS
        public bool UpdateLOSClassificationExceptionFromMAS(Int64 LoanID, string batchid, string batchName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLOSClassificationExceptionFromMAS(LoanID, batchid, batchName);
        }

        public bool UpdateLOSClassificationResultsFromMAS(Int64 LoanID, string batchid, List<MASDocument> masDocuments, string batchName, string batchClassID, string batchClassName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLOSClassificationResultsFromMAS(LoanID, batchid, masDocuments, batchName, batchClassID, batchClassName);
        }

        public bool UpdateLOSValidationExceptionFromMAS(Int64 LoanID, string batchid, string batchName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLOSValidationExceptionFromMAS(LoanID, batchid, batchName);
        }
        #endregion MAS

        public bool UpdateLoanTypeFromEncompass(Int64 LoanID, string LoanNumber, string BorrowerName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanTypeFromEncompass(LoanID, LoanNumber, BorrowerName);
        }


        public bool UpdateEphesoftBatchDetail(Int64 loanID, string batchid, Int64 docID, string batchclassid, string batchclassname)
        {
            return new LoanDataAccess(TenantSchema).UpdateEphesoftBatchDetail(loanID, batchid, docID, batchclassid, batchclassname);
        }

        public string GetDocumentStackingOrder(Int64 loanID, Int64 configId)
        {
            return new LoanDataAccess(TenantSchema).GetDocumentStackingOrder(loanID, configId);
        }

        public bool UpdateEphesoftReviewedDate(Int64 loanID, string batchid)
        {
            return new LoanDataAccess(TenantSchema).UpdateEphesoftReviewedDate(loanID, batchid);
        }

        public bool UpdateEphesoftValidatorDate(Int64 loanID, string batchid)
        {
            return new LoanDataAccess(TenantSchema).UpdateEphesoftValidatorDate(loanID, batchid);
        }

        public bool UpdateEphesoftValidatorName(Int64 loanID, string batchid)
        {
            return new LoanDataAccess(TenantSchema).UpdateEphesoftValidatorName(loanID, batchid);
        }


        public Object GetLoanDetailsForEphesoft(Int64 loanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanDetailsForEphesoft(loanID);
        }

        public Object CheckLoanPageCount(Int64 loanID, Int64 pageCount)
        {
            return new LoanDataAccess(TenantSchema).CheckLoanPageCount(loanID, pageCount);
        }


        public void LoanComplete(Int64 loanID, Int64 CompleteuserRoleID, Int64 CompleteUserID, string CompleteNotes)
        {

            LoanDataAccess _loanDataAccess = new LoanDataAccess(TenantSchema);
            var obj = _loanDataAccess.GetQCIQLookupDetails(loanID);
            Boolean _qciqEnabled = _loanDataAccess.QCIQEnabled();
            Dictionary<string, object> loanDetails = _loanDataAccess.GetLoanNumber(loanID);
            DateTime? QCIQStartDate = Convert.ToDateTime(loanDetails["startDate"]);
            if (obj != null && _qciqEnabled)
            {
                QCIQLoanDeails _lDetails = JsonConvert.DeserializeObject<QCIQLoanDeails>(JsonConvert.SerializeObject(obj));

                string sqlScript = _lDetails.MasterSQLScript;
                sqlScript = sqlScript.Replace("<<CUSTOMER_NAME>>", _lDetails.CustomerName.Replace("'", "''"));
                //Get Master Data QCIQ
                System.Data.DataSet MasterQCIQData = _loanDataAccess.GetQCIQData(_lDetails.MasterConnectionString, sqlScript);

                Int64 CustomerID = 0;
                if (MasterQCIQData != null && MasterQCIQData.Tables.Count > 0)
                {
                    System.Data.DataTable dt = GetTableFromSet(MasterQCIQData, "CUSTOMER");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        CustomerID = Convert.ToInt64(dt.Rows[0]["customerid"]);

                        if (CustomerID > 0)
                        {
                            sqlScript = _lDetails.SQLScript;
                            sqlScript = sqlScript.Replace("<<CUSTOMER_ID>>", CustomerID.ToString());
                            sqlScript = sqlScript.Replace("<<LOAN_NUMBER>>", loanDetails["loanNumber"].ToString().Replace("'", "''"));

                            //Get Data for particular loan from QCIQ
                            System.Data.DataSet QCIQData = _loanDataAccess.GetQCIQData(_lDetails.ConnectionString, sqlScript);

                            if (QCIQData != null && QCIQData.Tables.Count > 0)
                            {
                                dt = GetTableFromSet(QCIQData, "ASSIGNMENTHISTORYDATA");
                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    QCIQStartDate = Convert.ToDateTime(dt.Rows[0]["WhenAssigned"]);

                                }
                            }
                        }
                    }
                }
            }
            _loanDataAccess.UpdateQCIQStartDate(QCIQStartDate, loanID);
            //Update User Role Details

            _loanDataAccess.UpdateLoanCompleteUserDetails(loanID, CompleteuserRoleID, CompleteUserID, CompleteNotes);
            // Update  LoanCheckList Details
            EvaluateRules ruleEngine = new EvaluateRules(TenantSchema, loanID);
            List<LoanChecklistAudit> loancheckList = ruleEngine.FetchLoanCheckListDetails();
            RuleEngineDataAccess _ruleDataAccess = new RuleEngineDataAccess(TenantSchema);
            _ruleDataAccess.InsertLoanCheckListAuditDetails(loancheckList);
            //Update to Workflow
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("TENANT_SCHEMA", TenantSchema);
            dictionary.Add("APPROVE", "Y");
            dictionary.Add("LOANID", loanID.ToString());
            dictionary.Add("MISSINGDOCUMENT", false.ToString());
            dictionary.Add("STATUS", StatusConstant.PENDING_AUDIT.ToString());
            //WFProxy.ExecuteWorkFlow(dictionary);
            IntellaLend.WFProxy.WFProxy.ExecuteWorkFlow(dictionary);

            string ephesoftOutputPath = string.Empty;

            if (ConfigurationManager.AppSettings["ephesoftOutputPath"] != null && ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString() != String.Empty)
            {
                ephesoftOutputPath = ConfigurationManager.AppSettings["ephesoftOutputPath"].ToString();
            }

            long[] _loanId = { loanID };


            _loanDataAccess.InsertLOSLoanExport(loanID, loanDetails["loanNumber"].ToString());

            _loanDataAccess.DeleteOutputFolder(_loanId, ephesoftOutputPath);
        }

        public bool UpdateLoanMonitor(long loanID, long loanTypeID, string UserName, string ephesoftPath)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanMonitor(loanID, loanTypeID, UserName, ephesoftPath);
        }

        public bool DeleteOutputFolder(long[] loanID, string ephesoftPath)
        {
            return new LoanDataAccess(TenantSchema).DeleteOutputFolder(loanID, ephesoftPath);
        }

        public bool UpdateLoanDetails(long loanID, string loanVlaues, string userName, Int64 Type)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanDetails(loanID, loanVlaues, userName, Type);
        }
        public bool UpdateLoanHeader(long loanID, LoanHeader loanVlaues, string userName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanHeader(loanID, loanVlaues, userName);
        }

        public bool DeleteLoan(long[] loanID, string userName)
        {
            return new LoanDataAccess(TenantSchema).DeleteLoan(loanID, userName);
        }
        public bool DeletedReverification(Int64 LoanReverificationID, Int64 LoanID, string UserReverificationName, string UserName)
        {
            return new LoanDataAccess(TenantSchema).DeletedReverification(LoanReverificationID, LoanID, UserReverificationName, UserName);
        }

        public long AssignUser(long loanId, long assignedUserId, long currentUserId, string serviceTypeName, string assignedBy, string assignedTo)
        {
            string roleName = string.Empty;
            if (ConfigurationManager.AppSettings["PreClosing"] == serviceTypeName)
            {
                roleName = RoleConstant.UNDERWRITER;
            }
            else if (ConfigurationManager.AppSettings["PostClosing"] == serviceTypeName)
            {
                roleName = RoleConstant.POST_CLOSER;
            }
            return new LoanDataAccess(TenantSchema).AssignUser(loanId, assignedUserId, currentUserId, roleName, assignedBy, assignedTo);
        }

        #endregion

        private System.Data.DataTable GetTableFromSet(System.Data.DataSet QCIQData, string TableName)
        {

            foreach (System.Data.DataTable dt in QCIQData.Tables)
            {
                if (dt.Rows.Count > 0 &&
                    Convert.ToString(dt.Rows[0]["TABLE_NAME"]).ToUpper().Trim() == TableName)
                    return dt;
            }

            return null;
        }
        public object GetEmailTrackerDetails(EmailtrackerModel emailtrack)
        {
            return new LoanDataAccess(TenantSchema).GetEmailTrackerDetails(emailtrack.FromDate, emailtrack.ToDate);
        }

        public object GetLoan(string _loanGUID)
        {
            try
            {

                _loanGUID = CommonUtils.EnDecrypt(_loanGUID, true);
                return new LoanDataAccess(TenantSchema).GetLoanInfo(_loanGUID);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public LoanAuditReportPDF GetLoanAuditReportDeatils(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanAuditReportDeatils(LoanID);
        }

        public object GetEmailTracker()
        {
            return new LoanDataAccess(TenantSchema).GetEmailTracker();
        }
        //public object GetEncompassException()
        //{
        //    return new LoanDataAccess(TenantSchema).GetEncompassException();
        //}
        public object GetEncompassExceptionDetails(EncompassExceptionModel encompassException)
        {
            return new LoanDataAccess(TenantSchema).GetEncompassExceptionDetails(encompassException.FromDate, encompassException.ToDate);
        }
        public bool RetryException(Int64 EncompassExceptionID)
        {
            return new LoanDataAccess(TenantSchema).RetryException(EncompassExceptionID);
        }
        public bool ResendEmail(Int64 ID)
        {
            return new LoanDataAccess(TenantSchema).ResendEmail(ID);
        }

        public object GetCurrentData(Int64 Id)
        {
            return new LoanDataAccess(TenantSchema).GetCurrentData(Id);
        }
        public object SendEmailDetails(string To, string Subject, string Attachements, string Body, Int64 UserID, string SendBy, Int64 LoanID, string AttachmentsName)
        {
            return new LoanDataAccess(TenantSchema).SendEmailDetails(To, Subject, Attachements, Body, UserID, SendBy, LoanID, AttachmentsName);
        }
        public object GetDataByLoanId(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetDataByLoanId(LoanID);
        }

        public string GetEncompassDocPages(Int64 LoanID, Int64 DocID, ref bool isEncompassLoan)
        {
            return new LoanDataAccess(TenantSchema).GetEncompassDocPages(LoanID, DocID, ref isEncompassLoan);
        }


        #region Export Loan Monitor

        public object SearchExportMonitorDetails(Int64 Status, Int64 CustomerId, Exportmodel exportmodel)
        {
            return new LoanDataAccess(TenantSchema).SearchExportMonitorDetails(Status, CustomerId, exportmodel.FromDate, exportmodel.ToDate);
        }
        public object GetLoanExportMonitorDetails()
        {
            return new LoanDataAccess(TenantSchema).GetLoanExportMonitorDetails();
        }

        public object SearchLoanExport(DateTime FromDate, DateTime ToDate, Int64 CurrentUserID, string LoanNumber, Int64 LoanType, string BorrowerName, string LoanAmount,
           Int64 ReviewStatus, DateTime? AuditMonthYear, Int64 ReviewType, Int64 Customer, string PropertyAddress, string InvestorLoanNumber)
        {
            return new LoanDataAccess(TenantSchema).SearchLoanExport(FromDate, ToDate, CurrentUserID, LoanNumber, LoanType, BorrowerName, LoanAmount, ReviewStatus, AuditMonthYear, ReviewType, Customer, PropertyAddress, InvestorLoanNumber);
        }

        public object GetCurrentJobDetail(Int64 JobID)
        {
            return new LoanDataAccess(TenantSchema).GetCurrentJobDetail(JobID);
        }
        public bool ExportToBox(Int64 LoanID)
        {
            string BoxExportFolderID = ConfigurationManager.AppSettings["BoxExportFolderID"].ToString();
            LoanDataAccess _loanData = new LoanDataAccess(TenantSchema);
            Loan loan = _loanData.GetLoan(LoanID);
            bool result = false;
            if (loan != null)
            {
                string CustomerName = _loanData.GetCustomerName(loan.CustomerID).Trim();
                string LoanTypeName = _loanData.GetLoanTypeName(loan.LoanTypeID).Trim();
                string LoanObject = (loan.LoanDetails != null ? loan.LoanDetails.LoanObject : string.Empty).Trim();
                string LoanNumber = (string.IsNullOrEmpty(loan.LoanNumber) ? LoanID.ToString() : loan.LoanNumber).Trim();

                if (!string.IsNullOrEmpty(CustomerName) && !string.IsNullOrEmpty(LoanTypeName) && !string.IsNullOrEmpty(LoanObject) && !string.IsNullOrEmpty(LoanNumber))
                    result = new BoxAPIWrapper(TenantSchema, 1).CreateLoanCSV(BoxExportFolderID, CustomerName, LoanTypeName, LoanNumber, ".json", LoanObject);
            }
            return result;
        }

        public object SaveLoanJob(string BatchName, Int64 CustomerID, Boolean CoverLetter,
               Boolean TableOfContent, Boolean PasswordProtected, string Password, string CoverLetterContent, List<BatchLoanDetail> BatchLoanDoc, Int64 ExportedBy)
        {
            return new LoanDataAccess(TenantSchema).SaveLoanJob(BatchName, CustomerID, CoverLetter, TableOfContent, PasswordProtected, Password, CoverLetterContent, BatchLoanDoc, ExportedBy);
        }
        public object GetLoanDocDetails(Int64 LoanID)
        {
            LoanRuleEngine ruleEngine = new LoanRuleEngine(TenantSchema, LoanID, true);
            return new
            {
                LoanID = LoanID,
                loanDocuments = ruleEngine.batchDocTypes,
                success = true
            };
        }
        public object DeleteJob(Int64 JobID)
        {
            return new LoanDataAccess(TenantSchema).DeleteJob(JobID);
        }

        #endregion

        public List<LoanStipulation> SaveLoanStipulations(Int64 LoanID, LoanStipulationDetails stipulationDetails, string userName)
        {
            return new LoanDataAccess(TenantSchema).SaveLoanStipulations(LoanID, stipulationDetails, userName);
        }

        public List<LoanStipulation> GetLoanStipulationDetails(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanStipulationDetails(LoanID);
        }
        public List<LoanStipulation> UpdateLoanStipulations(Int64 LoanID, LoanStipulationDetails stipulationDetails, string userName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanStipulations(LoanID, stipulationDetails, userName);
        }
        #region -- Investor Stipulation
        public object GetReviewTypeSearchCriteria(Int64 ReviewTypeId)
        {
            return new LoanDataAccess(TenantSchema).GetReviewTypeSearchCriteria(ReviewTypeId);
        }
        #endregion
        public object ReSentJobLoanExport(Int64 JobID, Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).ReSentJobLoanExport(JobID, LoanID);
        }
        public bool SaveLoanAuditDueDate(Int64 LoanID, DateTime AuditDueDate)
        {
            return new LoanDataAccess(TenantSchema).SaveLoanAuditDueDate(LoanID, AuditDueDate);
        }

        public string GetRuleFindings(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetRuleFindings(LoanID);
        }
        public string GetApplicationURL()
        {
            return new LoanDataAccess(TenantSchema).GetApplicationURL();
        }
        public bool UpdateLoanStatus(Int64 LoanID, string UserName)
        {
            return new LoanDataAccess(TenantSchema).UpdateLoanStatus(LoanID, UserName);
        }

        public bool DocumentObsolete(Int64 LoanID, Int64 DocId, Int64 DocVersion, bool IsObsolete, Int64 CurrentUserID, string DocName)
        {
            return new LoanDataAccess(TenantSchema).DocumentObsolete(LoanID, DocId, DocVersion, IsObsolete, CurrentUserID, DocName);
        }

        public object GetLoanMissingDocuments(Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLoanMissingDocuments(LoanID);
        }

        public string GetPDFFooterName()
        {
            return new LoanDataAccess(TenantSchema).GetPDFFooterName();
        }
        public object GetBoxDownloadExceptionDetails(BoxDownloadExceptionModel boxdownloadException)
        {
            return new LoanDataAccess(TenantSchema).GetBoxDownloadExceptionDetails(boxdownloadException.FromDate, boxdownloadException.ToDate);

        }
        public bool RetryBoxException(Int64 ID)
        {
            return new LoanDataAccess(TenantSchema).RetryBoxException(ID);
        }


        public object GetEncompassExportDetails()
        {
            return new LoanDataAccess(TenantSchema).GetEncompassExportDetails();
        }
        public object RetryEncompassExport(Int64 ID, Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).RetryEncompassExport(ID, LoanID);
        }
        public object GetEncompassCurrentExportDetail(Int64 ID, Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetEncompassCurrentExportDetail(ID, LoanID);
        }
        public object SearchEncompassExportDetails(Int64 Status, EncompassExportmodel encompassExport, Int64 CustomerId)
        {
            return new LoanDataAccess(TenantSchema).SearchEncompassExportDetails(Status, encompassExport.FromDate, encompassExport.ToDate, CustomerId);
        }
        public bool RetryEncompassLoanDownload(Int64 LoanID, Int64 EDownloadID)
        {
            return new LoanDataAccess(TenantSchema).RetryEncompassLoanDownload(LoanID, EDownloadID);
        }
        public object GetLOSCurrentExportLoanDetail(Int64 ID, Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).GetLOSCurrentExportLoanDetail(ID, LoanID);
        }
        public object RetryLosExportDetails(Int64 ID, Int64 LoanID)
        {
            return new LoanDataAccess(TenantSchema).RetryLosExportDetails(ID, LoanID);
        }
        public object SearchLosExportMonitorDetails(Int64 CustomerId, Exportmodel exportmodel, Int64 LoanTypeId, Int64 ServiceTypeId)
        {
            return new LoanDataAccess(TenantSchema).SearchLosExportMonitorDetails(CustomerId, exportmodel.FromDate, exportmodel.ToDate, LoanTypeId, ServiceTypeId);
        }
        public object ReExportLosDetail(Int64 LoanID, Int32 FileType, Int64 ID)
        {
            return new LoanDataAccess(TenantSchema).ReExportLosDetail(LoanID, FileType, ID);
        }
    }
}
