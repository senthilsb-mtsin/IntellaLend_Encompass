using Ephesoft.Models.TableRow;
using EphesoftService.Models;
using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService.Controllers
{
    public class DocumentController : ApiController
    {
        #region Private Variables

        private MTSEphesoftServiceDBAccess dbAccess = new MTSEphesoftServiceDBAccess();
        private DocumentRulesProcess docRulesProcess = new DocumentRulesProcess();
        private static readonly CustomLogger logger = new CustomLogger("DocumentController");

        #endregion

        #region CONSTANTS

        public const string PARENTCHILDMERGE = "ParentChildMerge";

        #endregion

        XPathNavigator inputXmlNavigator = null;
        public string currentBatchId = null;
        public string currentBatchName = null;

        #region Public Methods

        /// <summary>
        /// The execute method processes the Ephesoft XML for the following operations:
        /// Append - Adding a document BEFORE or AFTER another document, 
        /// Concatenate - Combine the same document types into a single document, 
        /// Convert - Renames the document type to a given name
        /// </summary>
        [HttpPut]
        public HttpResponseMessage Execute()
        {
            try
            {
                string defaultOrder = ConfigurationManager.AppSettings["orderOfExecution"] != null ? ConfigurationManager.AppSettings["orderOfExecution"].ToString() : string.Empty;
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                EphesoftRequest ephesoftReq = this.ProcessRequest(requestJson);

                if (string.IsNullOrEmpty(ephesoftReq.orderOfExecution))
                    ephesoftReq.orderOfExecution = defaultOrder;
                //EventLog.WriteEntry("EphesoftTestAPI", ephesoftReq.inputXML);
                //Logger.Write(ephesoftReq.inputXML);
                //logger.Info(ephesoftReq.inputXML);

                this.currentBatchId = UtilFunctions.GetBatchIdForEphesoftXML(this.inputXmlNavigator);
                //EphesoftService.Globals.

                this.currentBatchName = UtilFunctions.GetBatchNameForEphesoftXML(this.inputXmlNavigator);

                logger.Info("Batch Id:" + this.currentBatchId + " - Processing request from client.");


                // Retrieve the ConfigId from the DB for the Batch class of the input Ephesoft batch XML
                int configId = this.GetConfigId(this.inputXmlNavigator);

                //For UHS
                //if (configId != 0)
                //{
                //    this.RearrangeMiscellaneousDocument(configId);
                //}

                //For Republic Bank
                //if (ephesoftReq.ephesoftModule == 3) //Prakash : 3 = Document Assembly Module
                //{
                //    Removed Manual Classification
                //    ManualDocumentAssembly(ephesoftReq.inputXML);
                //}

                List<string> orderOfExecution = ephesoftReq.orderOfExecution.Split('|').Select(x => x).ToList();
                logger.Debug($"ephesoftReq.orderOfExecution   :{ephesoftReq.orderOfExecution} ,ephesoftReq.ephesoftModule : {ephesoftReq.ephesoftModule} ");

                foreach (string item in orderOfExecution)
                {
                    switch (item.ToLower().Trim())
                    {
                        case "append":
                            {
                                logger.Debug($"Executing Append Rule : {ephesoftReq.ephesoftModule}");
                                this.ApplyAppendRules(configId, ephesoftReq.ephesoftModule);
                                this.ParentChildMerge(configId);
                                break;
                            }
                        case "concatenate":
                            {
                                logger.Debug($"Executing Concatenate Rule : {ephesoftReq.ephesoftModule}");
                                this.ApplyConcatenateRules(configId, ephesoftReq.ephesoftModule);
                                break;
                            }
                        case "convert":
                            {
                                logger.Debug($"Executing Convert Rule : {ephesoftReq.ephesoftModule}");
                                this.ApplyConversionRules(configId, ephesoftReq.ephesoftModule);
                                break;
                            }
                        case "pagesequence":
                            {
                                logger.Debug($"Executing Pagesequence Rule : {ephesoftReq.ephesoftModule}");
                                this.PageSequenceMerge();
                                break;
                            }
                        case "advmerge":
                            {
                                logger.Debug($"Executing Advmerge Rule : {ephesoftReq.ephesoftModule}");
                                this.ApplyAdvancedRules(configId);
                                break;
                            }
                        default:
                            break;
                    }
                }

                //Zero Page Document Check
                this.ZeroPageDocumentCheck();


                //// If the appendFlag in the request is set, apply the append rules
                //if (ephesoftReq.appendFlag)
                //{
                //    this.ApplyAppendRules(configId);
                //    this.ParentChildMerge(configId);
                //}
                //// If the concatenateFlag in the request is set, apply the concatenate rules
                //if (ephesoftReq.concatenateFlag)
                //{
                //    this.ApplyConcatenateRules(configId, ephesoftReq.ephesoftModule);
                //}
                //// If the convertFlag in the request is set, apply the conversion rules
                //if (ephesoftReq.convertFlag)
                //{
                //    //this.ApplyConversionRules(configId, ephesoftReq.ephesoftModule);
                //    this.ApplyConversionRules(configId, ephesoftReq.ephesoftModule);
                //}

                //this.ApplyAdvancedRules(configId);

                //// If the pageSequenceFlag in the request is set, check for Page Sequence in a document
                //if (ephesoftReq.pageSequenceFlag)
                //{
                //    //Page Sequence Check and Merge
                //    this.PageSequenceMerge();
                //}

                ////Zero Page Document Check
                //this.ZeroPageDocumentCheck();

                return this.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                logger.Error("Batch Id:" + this.currentBatchId + " - Error Processing request");
                return this.CreateExceptionResponse(ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage MergeConfigDocuments()
        {
            try
            {
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                EphesoftRequest ephesoftReq = this.ProcessRequest(requestJson);

                int configId = this.GetConfigId(this.inputXmlNavigator);

                MergeConfigDocuments(configId);

                return this.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                logger.Error("Batch Id:" + this.currentBatchId + " - Error Processing request");
                return this.CreateExceptionResponse(ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage FieldValidate()
        {
            EphesoftLookupRequest ephesoftReq = new EphesoftLookupRequest();
            try
            {
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {
                    ephesoftReq = JsonConvert.DeserializeObject<EphesoftLookupRequest>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }
                bool _isFieldUpdated = false;
                this.inputXmlNavigator = GetXMLNavigator(ephesoftReq.inputXML);
                XMLBatch m_XMLBatch = new XMLBatch(this.inputXmlNavigator);

                foreach (var document in m_XMLBatch.Documents.Where(x => x.m_Type == "Credit Report").ToList())
                {
                    DocumentLevelField _paymentField = document.DocumentLevelFields.Where(x => x.m_Name == "Mortgage Payments").FirstOrDefault();
                    DocumentLevelField _balanceField = document.DocumentLevelFields.Where(x => x.m_Name == "Mortgage Balance").FirstOrDefault();

                    if (_paymentField != null && _balanceField != null)
                    {
                        foreach (var field in document.DocumentLevelFields)
                        {
                            if (field.Name == "Mortgage Loan")
                            {
                                field.Value = "No";
                                if (!string.IsNullOrEmpty(_paymentField.Value.Trim()) || !string.IsNullOrEmpty(_balanceField.Value.Trim()))
                                {
                                    field.Value = "Yes";
                                    _isFieldUpdated = true;
                                }
                                break;
                            }
                        }
                    }

                }

                if (!_isFieldUpdated)
                {
                    List<XPathNavigator> documentsList = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, "Credit Report");
                    if (documentsList.Count > 0)
                    {

                        foreach (XPathNavigator document in documentsList.ToList())
                        {
                            XPathNavigator _mloanField = null;
                            string expression = "./DocumentLevelFields/DocumentLevelField";
                            XPathExpression expr = document.Compile(expression);
                            XPathNodeIterator fields = document.Select(expr);

                            foreach (XPathNavigator field in fields.OfType<XPathNavigator>().ToList())
                            {
                                XPathNavigator fieldName = field.SelectSingleNode(field.Compile("./Name"));
                                if (fieldName != null)
                                {
                                    if (fieldName.Value == "Mortgage Loan")
                                    {
                                        _mloanField = field;
                                    }

                                }
                            }

                            string rowExpression = "./DataTables/DataTable/Rows/Row";
                            XPathExpression rowExpr = document.Compile(rowExpression);
                            XPathNodeIterator datatableRows = document.Select(rowExpr);

                            foreach (XPathNavigator row in datatableRows.OfType<XPathNavigator>().ToList())
                            {
                                string _accType = string.Empty;
                                string expressionColm = "./Columns/Column";
                                XPathExpression exprCol = row.Compile(expressionColm);
                                XPathNodeIterator columns = row.Select(exprCol);
                                foreach (XPathNavigator column in columns.OfType<XPathNavigator>().ToList())
                                {
                                    XPathNavigator colName = column.SelectSingleNode(column.Compile("./Name"));
                                    if (colName != null)
                                    {
                                        if (colName.Value.Trim().ToUpper() == "ACCOUNT TYPE")
                                        {
                                            XPathNavigator colValue = column.SelectSingleNode(column.Compile("./Value"));
                                            if (colValue != null && !string.IsNullOrEmpty(colValue.Value))
                                            {
                                                if (colValue.Value.Trim().ToUpper() == "MORTGAGE")
                                                {
                                                    _accType = "MORTGAGE";
                                                }
                                            }
                                        }
                                        if (colName.Value.Trim().ToUpper() == "ACCOUNTS" && _accType == "MORTGAGE")
                                        {
                                            XPathNavigator colValue = column.SelectSingleNode(column.Compile("./Value"));
                                            if (colValue != null && !string.IsNullOrEmpty(colValue.Value))
                                            {
                                                string accVal = colValue.Value.Replace(".", "").Replace("$", "").Replace(",", "").Trim();
                                                Int64.TryParse(accVal, out long _accountFieldVal);
                                                if (_accountFieldVal > 0 && _mloanField != null)
                                                {
                                                    XPathNavigator fieldValue = _mloanField.SelectSingleNode(_mloanField.Compile("./Value"));
                                                    if (fieldValue != null)
                                                    {
                                                        fieldValue.SetValue("Yes");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                logger.Info("Mortgage Loan field updated successfully");
                return this.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }

        private void MergeConfigDocuments(int configID)
        {
            List<MergeDocuments> docs = dbAccess.GetMergeDocumentConfig(configID);

            string[] destDocs = docs.Select(x => x.DestinationDocType).Distinct().ToArray();

            foreach (string destDoc in destDocs)
            {
                List<MergeDocuments> sourceDocs = docs.Where(d => d.DestinationDocType == destDoc).ToList();

                List<XPathNavigator> xmlDestDoc = UtilFunctions.GetDocumentOfDocType(this.inputXmlNavigator, destDoc);

                if (UtilFunctions.CheckSourceDocExist(this.inputXmlNavigator, sourceDocs))
                {
                    if (xmlDestDoc.Count == 0)
                    {
                        xmlDestDoc.Add(CreateDestinationDocumentType(destDoc));
                    }
                    else
                    {
                        if (sourceDocs.Any(x => x.DestinationDocFieldToTable))
                        {
                            List<DocumentField> docFields = UtilFunctions.GetDocumentFields(xmlDestDoc[0]);

                            if (docFields.Count > 0)
                            {
                                string DocID = UtilFunctions.GetDocumentIdenifier(xmlDestDoc[0]);

                                Models.DataTable fieldTable = UtilFunctions.FieldsToDataTable(docFields, destDoc, DocID);

                                UtilFunctions.AddDataTableToDocument(xmlDestDoc[0], fieldTable);

                                UtilFunctions.DeleteDocumentFields(xmlDestDoc[0]);
                            }
                        }
                    }

                    foreach (MergeDocuments item in sourceDocs)
                    {
                        if (item.DestinationDocType != item.SourceDocType)
                        {
                            List<XPathNavigator> xmlSourceDocs = UtilFunctions.GetDocumentOfDocType(this.inputXmlNavigator, item.SourceDocType);

                            foreach (XPathNavigator _currentSourceDoc in xmlSourceDocs)
                            {
                                string DocID = UtilFunctions.GetDocumentIdenifier(_currentSourceDoc);

                                List<DocumentField> docFields = UtilFunctions.GetDocumentFields(_currentSourceDoc);

                                if (docFields.Count > 0)
                                {
                                    Models.DataTable fieldTable = UtilFunctions.FieldsToDataTable(docFields, item.SourceDocType, DocID);

                                    UtilFunctions.AddDataTableToDocument(xmlDestDoc[0], fieldTable);
                                }

                                UtilFunctions.AppendSourceDocTableToDestination(xmlDestDoc[0], _currentSourceDoc);

                                UtilFunctions.MergeDocument(_currentSourceDoc, xmlDestDoc[0], DocumentLocation.AFTER);
                            }

                            UtilFunctions.SortDocumentPages(xmlDestDoc[0]);
                        }
                    }
                }

                if (xmlDestDoc.Count > 1)
                {
                    for (int i = 1; i < xmlDestDoc.Count; i++)
                    {
                        List<DocumentField> docFields = UtilFunctions.GetDocumentFields(xmlDestDoc[i]);
                        if (docFields.Count > 0)
                        {
                            string DocID = UtilFunctions.GetDocumentIdenifier(xmlDestDoc[i]);

                            Models.DataTable fieldTable = UtilFunctions.FieldsToDataTable(docFields, destDoc, DocID);

                            UtilFunctions.AddDataTableToDocument(xmlDestDoc[0], fieldTable);
                        }

                        UtilFunctions.AppendSourceDocTableToDestination(xmlDestDoc[0], xmlDestDoc[i]);

                        UtilFunctions.MergeDocument(xmlDestDoc[i], xmlDestDoc[0], DocumentLocation.AFTER);
                    }
                    UtilFunctions.SortDocumentPages(xmlDestDoc[0]);
                }
            }
        }

        private XPathNavigator CreateDestinationDocumentType(string docTypeName)
        {
            XPathNavigator _tempDocument = UtilFunctions.GetDuplicateFirstDocument(this.inputXmlNavigator);
            int _lastDocID = UtilFunctions.GetLastDocumentIdenifier(this.inputXmlNavigator);
            UtilFunctions.DeleteDocumentPages(_tempDocument);
            UtilFunctions.DeleteDocumentFields(_tempDocument);
            UtilFunctions.DeleteDocumentTables(_tempDocument);
            UtilFunctions.SetDocumentNodeValue(_tempDocument, "Identifier", $"DOC{(_lastDocID + 1).ToString()}");
            UtilFunctions.SetDocumentNodeValue(_tempDocument, "Type", docTypeName);
            UtilFunctions.SetDocumentNodeValue(_tempDocument, "Description", docTypeName);
            return _tempDocument;
        }



        public void ManualDocumentAssembly(string inputXML)
        {
            bool isEncompassLoan = false;
            bool isMissingDocument = false;

            List<EnDocumentType> _enDocList = GetEncompassDocumentPages(inputXML, ref isEncompassLoan, ref isMissingDocument);

            if (isEncompassLoan)
            {
                this.ManualDocumentAssembly(_enDocList, isMissingDocument);
            }
        }

        public List<EnDocumentType> GetEncompassDocumentPages(string inputXML, ref bool isEncompassLoan, ref bool isMissingDocument)
        {
            EncompassDocPagesResponse objres = new EncompassDocPagesResponse();
            List<EnDocumentType> eDocs = new List<EnDocumentType>();
            try
            {

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);

                if (dicObjects.ContainsKey("SCHEMA") && dicObjects.ContainsKey("LOAN_ID"))
                {

                    string Schema = dicObjects["SCHEMA"];
                    Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);

                    Int64 DocID = dicObjects.ContainsKey("DOC_ID") ? Convert.ToInt64(dicObjects["DOC_ID"]) : 0;
                    isMissingDocument = dicObjects.ContainsKey("DOC_ID");
                    string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

                    try
                    {
                        using (var handler = new HttpClientHandler() { })
                        using (var client = new HttpClient(handler))
                        {
                            EncompassDocPagesRequest request = new EncompassDocPagesRequest();
                            request.LoanID = LoanID;
                            request.DocID = DocID;
                            request.TableSchema = Schema;
                            string cont = JsonConvert.SerializeObject(request);
                            HttpResponseMessage httpres = client.PostAsync(baseURL + "/GetEncompassDocumentPages", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                            objres = httpres.Content.ReadAsAsync<EncompassDocPagesResponse>().Result;
                            if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                                throw new Exception(objres.ResponseMessage.MessageDesc);
                        }

                        isEncompassLoan = objres.isEncompassLoan;

                        return JsonConvert.DeserializeObject<List<EnDocumentType>>(objres.EncompassDocPages);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exception while Fetching detail from IntellaLend", ex);
                    }
                }
                else
                    return eDocs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateReviewedDate()
        {
            IntellaLnedWrapper ephesoftReq = new IntellaLnedWrapper();
            EphesoftLoanDetailsResponse objres = new EphesoftLoanDetailsResponse();
            try
            {

                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<IntellaLnedWrapper>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(ephesoftReq.inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                string Schema = dicObjects["SCHEMA"];
                Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);

                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

                try
                {
                    using (var handler = new HttpClientHandler() { })
                    using (var client = new HttpClient(handler))
                    {
                        EphesoftLoanDetailsRequest request = new EphesoftLoanDetailsRequest();
                        request.LoanID = LoanID;
                        request.TableSchema = Schema;
                        request.BatchID = xmlBatch.BatchInstanceIdentifier;
                        string cont = JsonConvert.SerializeObject(request);
                        HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateReviewerDate", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                        objres = httpres.Content.ReadAsAsync<EphesoftLoanDetailsResponse>().Result;
                        if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                            throw new Exception(objres.ResponseMessage.MessageDesc);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while Updating Loan Type", ex);
                }


                return CreateSuccessResponse(objres.LoanDetailsJson);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateValidatorDate()
        {
            IntellaLnedWrapper ephesoftReq = new IntellaLnedWrapper();
            EphesoftLoanDetailsResponse objres = new EphesoftLoanDetailsResponse();
            try
            {

                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<IntellaLnedWrapper>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(ephesoftReq.inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                string Schema = dicObjects["SCHEMA"];
                Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);

                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

                try
                {
                    using (var handler = new HttpClientHandler() { })
                    using (var client = new HttpClient(handler))
                    {
                        EphesoftLoanDetailsRequest request = new EphesoftLoanDetailsRequest();
                        request.LoanID = LoanID;
                        request.TableSchema = Schema;
                        request.BatchID = xmlBatch.BatchInstanceIdentifier;
                        string cont = JsonConvert.SerializeObject(request);
                        HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateValidatorDate", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                        objres = httpres.Content.ReadAsAsync<EphesoftLoanDetailsResponse>().Result;
                        if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                            throw new Exception(objres.ResponseMessage.MessageDesc);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while Updating Loan Type", ex);
                }


                return CreateSuccessResponse(objres.LoanDetailsJson);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage OrderByStacking()
        {
            Logger.WriteTraceLog("In OrderByStacking()");
            try
            {
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                EphesoftRequest ephesoftReq = this.ProcessRequest(requestJson);

                this.currentBatchId = UtilFunctions.GetBatchIdForEphesoftXML(this.inputXmlNavigator);

                this.currentBatchName = UtilFunctions.GetBatchNameForEphesoftXML(this.inputXmlNavigator);

                logger.Info("Batch Id:" + this.currentBatchId + " - Processing request from client.");

                int configId = this.GetConfigId(this.inputXmlNavigator);

                if (configId != 0)
                {
                    XMLBatch xmlBatch = new XMLBatch(this.inputXmlNavigator);
                    string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                    Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                    string Schema = dicObjects["SCHEMA"];
                    Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);
                    this.RearrangeMiscellaneousDocument(configId, Schema, LoanID);
                }

                return this.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                logger.Error("Batch Id:" + this.currentBatchId + " - Error Processing request");
                return this.CreateExceptionResponse(ex);
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// This method retrieves the relevant append rules for the config Id of the input XML.
        /// </summary>
        /// <param name="configId">The configId in the database for the batch class of the input XML</param>
        private void ManualDocumentAssembly(List<EnDocumentType> _lsEnDocs, bool isMissingDocument)
        {
            docRulesProcess.ManualDocumentAssembly(this.inputXmlNavigator, _lsEnDocs, isMissingDocument);
        }

        private HttpResponseMessage CreateSuccessResponse(string result)
        {
            // Create the response object and add the XML to the content
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");
            return response;
        }


        private static Dictionary<string, string> ExtractDataFromString(string inputString, string[] regexPattern)
        {
            Dictionary<string, string> returnDic = new Dictionary<string, string>();
            foreach (string pattern in regexPattern)
            {
                Regex regPattern = new Regex(pattern);
                Match match = regPattern.Match(inputString);
                if (regPattern.Match(inputString).Success)
                {
                    foreach (string group in regPattern.GetGroupNames())
                    {
                        if (group != "0")
                            returnDic[group] = match.Groups[group].Value;
                    }
                    break;
                }

            }

            return returnDic;
        }


        /// <summary>
        /// This method creates the response object when the request is processed successfully. 
        /// The response contains the updated Ephesoft batch XML
        /// </summary>
        /// <returns>The service response object</returns>
        private HttpResponseMessage CreateSuccessResponse()
        {
            // Create the XMLDocument from the XPathNavigator
            XmlDocument doc = new XmlDocument();
            doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            this.inputXmlNavigator.MoveToRoot();
            doc.Load(this.inputXmlNavigator.ReadSubtree());

            // Create the response object and add the XML to the content
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(doc.OuterXml, Encoding.UTF8, "application/xml");
            return response;
        }

        /// <summary>
        /// This method creates the error response with the exception details.
        /// </summary>
        /// <param name="ex">The exception thrown during request processing</param>
        /// <returns>The service response object</returns>
        private HttpResponseMessage CreateExceptionResponse(Exception ex)
        {
            HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            return response;
        }

        /// <summary>
        /// This method processes the Json contents of the request and converts it to the request object. 
        /// </summary>
        /// <param name="requestJson">Json string containing the request</param>
        /// <returns></returns>
        private EphesoftRequest ProcessRequest(string requestJson)
        {
            EphesoftRequest ephesoftReq = new EphesoftRequest();
            try
            {
                if (!string.IsNullOrEmpty(requestJson))
                {
                    // Deserialize the json string in the content of the web service request to the EphesoftRequest model
                    ephesoftReq = JsonConvert.DeserializeObject<EphesoftRequest>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });
                    string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

                    XmlDocument inputXmlDocument = new XmlDocument();
                    inputXmlDocument.LoadXml(ephesoftReq.inputXML);
                    this.inputXmlNavigator = inputXmlDocument.CreateNavigator();
                }
                else
                {
                    // If the request has null json string, throw an exception
                    logger.Error("Batch Id:" + this.currentBatchId + " - Request Json input is NULL.");
                    MTSPassThruException customEx = new MTSPassThruException("Error: Request Json input is NULL");
                    throw customEx;
                }
            }
            catch (MTSPassThruException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                logger.Error("Batch Id:" + this.currentBatchId + " - Error parsing JSON request.", ex);
                Exception customEx = new MTSPassThruException("Error Parsing Input request");
                throw customEx;
            }
            return ephesoftReq;
        }

        /// <summary>
        /// This method gets the batch class identifier for the input batch XML and retrieves the configId 
        /// for the batch class Id from the MTS.IDC_RULE_CONFIG table. 
        /// </summary>
        /// <param name="inputXmlNavigator">The XPathNavigator instance of the input Ephesoft batch XML</param>
        /// <returns></returns>
        private int GetConfigId(XPathNavigator inputXmlNavigator)
        {
            logger.Debug("Batch Id:" + this.currentBatchId + " - Get ConfigId from the DB the batch class of the input XML.");

            int configId = 0;
            // Get the batch class identifier for the input batch XML.
            string xmlBatchClasssId = UtilFunctions.GetBatchClassIdForEphesoftXML(inputXmlNavigator);
            if (String.IsNullOrEmpty(xmlBatchClasssId))
            {
                logger.Error("BatchClassID not retrieved from input XML.");
                throw new MTSPassThruException("BatchClassId not retrieved from input XML");
            }
            else
            {
                // Get the configId from the database for the batch class Id
                configId = dbAccess.GetCongidId(xmlBatchClasssId);
            }

            logger.Info("Batch Id:" + this.currentBatchId + " - Retrieved configId from DB for the input XML - " + configId);
            return configId;
        }

        /// <summary>
        /// This method retrieves the relevant conversion rules for the config Id of the input XML and 
        /// the ephesoft module in the request. 
        /// </summary>
        /// <param name="configId">The configId in the database for the batch class of the input XML</param>
        private void ApplyConversionRules(int configId, Int64 ephesoftModuleId)
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Apply the conversion rules for the configId - " + configId);

            // Retrieve the conversion rules from DB for given config Id and module
            System.Data.DataTable dbResult = dbAccess.GetConversionRules(configId, ephesoftModuleId);
            logger.Debug($"dbResult.Rows.Count : {dbResult.Rows.Count}");
            if (dbResult.Rows.Count > 0)
            {
                docRulesProcess.ConvertDocumentTypes(inputXmlNavigator, dbResult, this.currentBatchId);
                logger.Debug("Batch Id:" + this.currentBatchId + " - Applied the conversion rules for the config Id - " + configId);
            }
        }

        private void PageSequenceMerge()
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Documents Page Sequence Check Started ");

            List<XPathNavigator> documentsList = UtilFunctions.GetAllDocuments(inputXmlNavigator);
            if (documentsList.Count > 1)
            {
                docRulesProcess.PageSequenceMerge(documentsList, inputXmlNavigator);
            }

            logger.Debug("Batch Id:" + this.currentBatchId + " - Documents Merge Completed ");
        }

        private void ZeroPageDocumentCheck()
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Zero Page Document Check Started ");

            List<XPathNavigator> documentsList = UtilFunctions.GetAllDocuments(inputXmlNavigator);

            if (documentsList.Count > 1)
            {
                foreach (XPathNavigator document in documentsList.ToList())
                {
                    List<int> PageList = UtilFunctions.GetAllPagesFromDocument(document);

                    if (PageList.Count.Equals(0))
                        document.DeleteSelf();
                }
            }

            logger.Debug("Batch Id:" + this.currentBatchId + " - Zero Page Document Check Completed ");

        }

        [HttpPut]
        public HttpResponseMessage FormatCommonDocumentFields()
        {
            try
            {
                logger.Info("Batch Id:" + this.currentBatchId + " - FormatCommonDocumentFields() Started... ");
                // Read the contents of the request
                EphesoftLookupRequest ephesoftReq = new EphesoftLookupRequest();
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {
                    ephesoftReq = JsonConvert.DeserializeObject<EphesoftLookupRequest>(requestJson);
                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                this.inputXmlNavigator = GetXMLNavigator(ephesoftReq.inputXML);

                int configId = this.GetConfigId(this.inputXmlNavigator);

                // Retrieve the DocumentFieldMergingConfigs from DB
                System.Data.DataTable dbResult = dbAccess.GetFieldFormatConfigData(configId);

                List<XPathNavigator> _xmlDocuments = UtilFunctions.GetAllDocuments(this.inputXmlNavigator);

                if (dbResult.Rows.Count > 0)
                {
                    Dictionary<string, string> formatFieldsCollection = dbResult.AsEnumerable().Select(d => new { key = d["FieldName"].ToString().ToLower(), value = d["FormatPattern"].ToString() }).ToDictionary(d => d.key, d => d.value);
                    foreach (XPathNavigator doc in _xmlDocuments)
                    {
                        List<XPathNavigator> _xmlDocFields = UtilFunctions.GetXMLDocumentFields(doc);

                        foreach (XPathNavigator field in _xmlDocFields)
                        {
                            string FieldName = field.SelectSingleNode(field.Compile("./Name")).Value;

                            if (formatFieldsCollection.ContainsKey(FieldName.ToLower()))
                            {
                                XPathNavigator _fieldXMLPath = field.SelectSingleNode(field.Compile("./Value"));

                                switch (formatFieldsCollection[FieldName.ToLower()].ToLower().Trim())
                                {
                                    case "ssn":
                                        {
                                            _fieldXMLPath.SetValue(FormatSSN(_fieldXMLPath.Value.Trim()));
                                            break;
                                        }
                                    case "amount":
                                        {
                                            _fieldXMLPath.SetValue(FormatAmount(_fieldXMLPath.Value.Trim()));
                                            break;
                                        }
                                    default:
                                        break;
                                }

                            }
                        }
                    }
                }
                //}

                //XMLBatch xmlBatch = new XMLBatch(this.inputXmlNavigator);
                //int configId = this.GetConfigId(this.inputXmlNavigator);

                //// Retrieve the DocumentFieldMergingConfigs from DB
                //System.Data.DataTable dbResult = dbAccess.GetFieldFormatConfigData(configId);
                //logger.Debug($"dbResult.Rows.Count : {dbResult.Rows.Count}");
                //Dictionary<string, string> formatFieldsCollection = new Dictionary<string, string>();
                //if (dbResult.Rows.Count > 0)
                //{
                // foreach (DataRow row in dbResult.Rows)
                // {
                // //preparing dictionary to hold sourceFiledsData

                // string fieldName = Convert.ToString(row["FieldName"]);
                // string formatPattern = Convert.ToString(row["FormatPattern"]);

                // if (!formatFieldsCollection.ContainsKey(fieldName))
                // formatFieldsCollection.Add(fieldName, formatPattern);
                // }
                //}

                ////formatting stuff
                //foreach (var document in xmlBatch.Documents)
                //{
                // foreach (var formatField in formatFieldsCollection)
                // {
                // DocumentLevelField df = document.DocumentLevelFields.Where(x => x.m_Name == formatField.Key).FirstOrDefault();
                // if (df != null)
                // {
                // if (!string.IsNullOrEmpty(df.Value.Trim()))
                // {
                // switch (formatField.Value.ToLower().Trim())
                // {
                // case "ssn":
                // {
                // df.Value = FormatSSN(df.Value);
                // break;
                // }
                // case "amount":
                // {
                // df.Value = FormatAmount(df.Value);
                // break;
                // }
                // default:
                // break;
                // }
                // }
                // }
                // }
                //}

                logger.Info("Batch Id:" + this.currentBatchId + " - FormatCommonDocumentFields() Compoleted. ");
                return this.CreateSuccessResponse();

            }
            catch (Exception ex)
            {

                MTSExceptionHandler.HandleException(ref ex);
                logger.Error("Batch Id:" + this.currentBatchId + " - Error occured in FormatCommonDocumentFields()");
                return this.CreateExceptionResponse(ex);
            }
        }

        public string FormatAmount(string amount)
        {
            int decimalPointCount = amount.Count(f => (f == '.'));

            if (decimalPointCount > 0)
            {
                if (decimalPointCount == 1)
                {
                    int decimalDigits = amount.Substring(amount.IndexOf(".") + 1).Length;
                    if (decimalDigits > 2)
                    {
                        amount = amount.Replace(".", "");
                        return amount;
                    }

                }
                else
                {
                    Regex regx = new Regex("\\.(?=.*?\\.)");
                    amount = regx.Replace(amount, "");
                    int decimalDigits = amount.Substring(amount.IndexOf(".") + 1).Length;
                    if (decimalDigits > 2)
                    {
                        amount = amount.Replace(".", "");
                        return amount;
                    }
                }
            }
            return amount;
        }

        public string FormatSSN(string SSN)
        {
            if (IsValidSsn(SSN))
            {
                if (IsValidSsnWithDashes(SSN))
                {
                    return SSN;
                }
                else if (IsValidSsnWithoutDashes(SSN))
                {
                    if (SSN.Length > 3 && SSN.Length <= 5)
                        SSN = SSN.Insert(3, "-");
                    else if (SSN.Length > 5)
                        SSN = SSN.Insert(5, "-").Insert(3, "-");

                    return SSN;
                }
            }
            else
            {
                SSN = Regex.Replace(SSN, @"\s+", string.Empty);
                SSN = SSN.Replace("-", "");
                SSN = SSN.Replace(".", "");
                if (SSN.Length == 9)
                {
                    SSN = SSN.Insert(5, "-").Insert(3, "-");
                    return SSN;
                }
                else
                {
                    if (SSN.Length > 3 && SSN.Length <= 5)
                        SSN = SSN.Insert(3, "-");
                    else if (SSN.Length > 5)
                        SSN = SSN.Insert(5, "-").Insert(3, "-");

                    return SSN;
                }
            }
            return SSN;
        }

        public bool IsValidSsnWithDashes(string value)
        {
            Regex ssnRegex = new Regex(@"^\d{3}-\d{2}-\d{4}$");
            return ssnRegex.IsMatch(value);
        }

        public bool IsValidSsnWithoutDashes(string value)
        {
            return Regex.IsMatch(value, @"^\d{9}$");
        }

        public bool IsValidSsn(string value)
        {
            Regex ssnRegex = new Regex(@"^(?:\d{9}|\d{3}-\d{2}-\d{4})$");
            return ssnRegex.IsMatch(value);
        }

        /// <summary>
        /// combine the values of configured fieldnames as comma separated string and assign the value to configured(“Loan Purpose”) field
        /// </summary>
        [HttpPut]
        public HttpResponseMessage CombineAndAssignCheckedFields()
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Combine And Assign CheckedFields Started... ");
            try
            {
                //Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                EphesoftRequest ephesoftReq = this.ProcessRequest(requestJson);

                int configId = this.GetConfigId(this.inputXmlNavigator);
                // Retrieve the DocumentFieldMergingConfigs from DB 
                System.Data.DataTable dbResult = dbAccess.GetDocumentFieldValueMergeData(configId, ephesoftReq.ephesoftModule);
                logger.Debug($"dbResult.Rows.Count : {dbResult.Rows.Count}");
                if (dbResult.Rows.Count > 0)
                {
                    foreach (DataRow row in dbResult.Rows)
                    {
                        //preparing dictionary to hold sourceFiledsData
                        Dictionary<string, string> sourceFieldsCollection = new Dictionary<string, string>();
                        string documentName = Convert.ToString(row["DocumentName"]);
                        string destinationFieldName = Convert.ToString(row["DestinationField"]);
                        string sourceFieldsData = Convert.ToString(row["SourceFieldsData"]);
                        JObject sourceFieldsObj = JObject.Parse(sourceFieldsData);
                        JArray fieldList = (JArray)sourceFieldsObj["Data"];
                        List<SourceDocField> sf = fieldList.ToObject<List<SourceDocField>>();
                        foreach (SourceDocField item in sf)
                        {

                            if (!sourceFieldsCollection.ContainsKey(item.FieldName.ToLower()))
                                sourceFieldsCollection.Add(item.FieldName.ToLower(), item.AppendValue);
                        }

                        //Get the combined string of SourceDocumentField Names 
                        SetCombinedFieldNames(sourceFieldsCollection, documentName, destinationFieldName);
                    }
                }
                logger.Info("Batch Id:" + this.currentBatchId + " - Combine And Assign CheckedFields Completed. ");
                return this.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                logger.Error("Batch Id:" + this.currentBatchId + " - Error occured in CombineAndAssignCheckedFields");
                return this.CreateExceptionResponse(ex);
            }

        }

        private void SetCombinedFieldNames(Dictionary<string, string> sourceFieldsCollection, string documentName, string destinationFieldName)
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - SetCombinedFieldNames() Started... ");
            string combinedFieldValue = "";

            List<XPathNavigator> documentsList = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, documentName);
            if (documentsList.Count > 0)
            {
                foreach (XPathNavigator document in documentsList.ToList())
                {
                    List<string> combinedFieldArray = new List<string>();
                    string expression = "./DocumentLevelFields/DocumentLevelField";
                    XPathExpression expr = document.Compile(expression);
                    XPathNodeIterator fields = document.Select(expr);

                    foreach (XPathNavigator field in fields.OfType<XPathNavigator>().ToList())
                    {
                        XPathNavigator fieldName = field.SelectSingleNode(field.Compile("./Name"));
                        if (fieldName != null)
                        {
                            if (sourceFieldsCollection.ContainsKey(fieldName.Value.ToLower()))
                            {
                                XPathNavigator fieldValue = field.SelectSingleNode(field.Compile("./Value"));
                                if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.Value) && fieldValue.Value.ToLower().Equals("yes"))
                                {
                                    combinedFieldArray.Add(sourceFieldsCollection[fieldName.Value.ToLower()]);
                                }
                            }

                        }
                    }
                    combinedFieldArray.Sort();
                    combinedFieldValue = string.Join(",", combinedFieldArray);

                    //Get Destination DocumentField and Assign combined values to destination field value

                    foreach (XPathNavigator field in fields.OfType<XPathNavigator>().ToList())
                    {
                        XPathNavigator fieldName = field.SelectSingleNode(field.Compile("./Name"));
                        if (fieldName != null)
                        {
                            if (fieldName.Value == destinationFieldName)
                            {
                                XPathNavigator fieldValue = field.SelectSingleNode(field.Compile("./Value"));
                                if (fieldValue != null)
                                {
                                    fieldValue.SetValue(combinedFieldValue);
                                }
                            }

                        }
                    }
                }
                logger.Info("Batch Id:" + this.currentBatchId + " - SetCombinedFieldNames() Completed. ");
            }
        }

        /// <summary>
        /// check if the value of the given documentfield is 'yes' or 'no'
        /// </summary>
        /// <param name="df">DocumentField</param>
        /// <returns></returns>
        private bool IsFieldChecked(DocumentField df)
        {
            bool isChecked = false;

            if (!string.IsNullOrEmpty(df.Value) && df.Value.ToLower().Equals("yes"))
            {
                isChecked = true;
            }

            return isChecked;
        }

        private void ParentChildMerge(int configId)
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Apply the Parent Child merge rules for the configId - " + configId);
            System.Data.DataTable dt = dbAccess.GetAdvancedRules(configId);
            if (dt.Rows.Count > 0)
            {
                string advancedRule = string.Empty;
                Boolean flag = false;

                foreach (DataRow dbRow in dt.Rows)
                {
                    advancedRule = dbRow["AdvancedOptions"].ToString();
                    flag = Boolean.Parse(dbRow["Flag"].ToString());

                    if (advancedRule.Equals(PARENTCHILDMERGE) && flag)
                    {
                        System.Data.DataTable dbResult = dbAccess.GetParentChildMergeRules(configId);
                        if (dbResult.Rows.Count > 0)
                        {
                            docRulesProcess.ProcessParentChildMerge(inputXmlNavigator, dbResult, this.currentBatchId);
                            logger.Debug("Batch Id:" + this.currentBatchId + " - Applied the Parent Child merge rules for the config Id - " + configId);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// This method retrieves the relevant append rules for the config Id of the input XML.
        /// </summary>
        /// <param name="configId">The configId in the database for the batch class of the input XML</param>
        private void ApplyAppendRules(int configId, int moduleId)
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Apply the Append rules for the configId - " + configId);

            // Retrieve the append rules from DB for given config Id
            System.Data.DataTable dbResult = dbAccess.GetAppendRules(configId, moduleId);
            if (dbResult.Rows.Count > 0)
            {
                docRulesProcess.ProcessAppendRules(inputXmlNavigator, dbResult, this.currentBatchId);
                logger.Debug("Batch Id:" + this.currentBatchId + " - Applied the append rules for the config Id - " + configId);
            }
        }

        /// <summary>
        /// This method retrieves the relevant concatenate rules for the config Id of the input XML.
        /// </summary>
        /// <param name="configId">The configId in the database for the batch class of the input XML</param>
        private void ApplyConcatenateRules(int configId, int ephesoftModule)
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Apply the concatenate rules for the configId - " + configId);

            // Retrieve the append rules from DB for given config Id
            System.Data.DataTable dbResult = dbAccess.GetConcatenateRules(configId, ephesoftModule);
            if (dbResult.Rows.Count > 0)
            {
                docRulesProcess.ApplyConcatenateDocuments(inputXmlNavigator, dbResult, this.currentBatchId);
                logger.Debug("Batch Id:" + this.currentBatchId + " - Applied the concatenate rules for the config Id - " + configId);
            }
        }


        /// <summary>
        /// This method applies any advanced rules like AdvancedMerge to the input Ephesoft XML
        /// </summary>
        /// <param name="configId">The ConfigId for the input batch XML</param>
        private void ApplyAdvancedRules(int configId)
        {
            logger.Info("Batch Id:" + this.currentBatchId + " - Apply the advanced rules for the configId - " + configId);

            // Retrieve the advanced rules from DB for given config Id
            System.Data.DataTable dbResult = dbAccess.GetAdvancedRules(configId);
            if (dbResult.Rows.Count > 0)
            {
                docRulesProcess.ApplyAdvancedRules(this.inputXmlNavigator, dbResult, this.currentBatchId);
                logger.Info("Batch Id:" + this.currentBatchId + " - Applied the advanced rules for the config Id - " + configId);
            }
        }

        /// <summary>
        /// This method processes the Json contents of the request and converts it to the request object. 
        /// </summary>
        /// <param name="requestJson">Json string containing the request</param>
        /// <returns></returns>
        private XPathNavigator GetXMLNavigator(string inputXML)
        {
            XPathNavigator inputXmlNavigator = null;
            try
            {
                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                XmlDocument inputXmlDocument = new XmlDocument();
                inputXmlDocument.LoadXml(inputXML);
                inputXmlNavigator = inputXmlDocument.CreateNavigator();
            }
            catch (MTSPassThruException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                Exception customEx = new MTSPassThruException("Error Parsing Input XML :" + inputXML, ex);
                throw customEx;
            }
            return inputXmlNavigator;
        }



        private System.Data.DataTable GetStackingOrderDocuments(int configId, string schema, Int64 loanId)
        {
            Logger.WriteTraceLog("In GetStackingOrderDocuments()");
            StackingOrderDocumentsResponse objres = new StackingOrderDocumentsResponse();
            string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];
            Logger.WriteTraceLog($"baseURL : {baseURL}");
            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler))
            {
                StackOrderDocumentsRequest request = new StackOrderDocumentsRequest();
                request.TableSchema = schema;
                request.LoanID = loanId;
                request.ConfigID = configId;
                string cont = JsonConvert.SerializeObject(request);
                HttpResponseMessage httpres = client.PostAsync(baseURL + "/GetDocumentStackingOrder", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                objres = httpres.Content.ReadAsAsync<StackingOrderDocumentsResponse>().Result;
                Logger.WriteTraceLog($"objres :{JsonConvert.SerializeObject(objres)}");
                if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                    throw new Exception(objres.ResponseMessage.MessageDesc);

                var table = JsonConvert.DeserializeObject<System.Data.DataTable>(objres.stackingOrderDocuments);
                return table;
            }
        }

        private void RearrangeMiscellaneousDocument(int configId, string schema, Int64 loanid)
        {
            Logger.WriteTraceLog("In RearrangeMiscellaneousDocument()");
            System.Data.DataTable dbResult = GetStackingOrderDocuments(configId, schema, loanid);
            //System.Data.DataTable dbResult = dbAccess.GetDocumentStackingOrder(configId);
            if (dbResult.Rows.Count > 0)
            {
                List<XPathNavigator> docTypeList = new List<XPathNavigator>();
                List<string> _configDocs = new List<string>();
                List<string> _allDocNames = new List<string>();
                List<string> _autoDocs = new List<string>();
                foreach (DataRow dr in dbResult.Rows)
                {
                    string _documentName = dr["DocumentName"].ToString().Trim();
                    List<XPathNavigator> dTypeList = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, _documentName);
                    foreach (XPathNavigator dt in dTypeList)
                        docTypeList.Add(dt);

                    _configDocs.Add(_documentName);
                }
                //Console.WriteLine(docTypeList);
                List<XPathNavigator> xPathDocs = UtilFunctions.GetAllDocuments(this.inputXmlNavigator);

                foreach (XPathNavigator xdocs in xPathDocs)
                    _allDocNames.Add(UtilFunctions.GetSingleElementByElementName(xdocs, "Type"));

                List<string> _unAvailableDocs = _allDocNames.Distinct().Where(a => !_configDocs.Distinct().Any(c => c == a)).ToList();

                System.Data.DataTable _skipdocs = dbAccess.GetDocumentsToSkip(configId);

                if (_skipdocs.Rows.Count > 0)
                {
                    foreach (DataRow dr in _skipdocs.Rows)
                    {
                        string _documentName = dr["DOCUMENT_NAME"].ToString().Trim();
                        _autoDocs.Add(_documentName);
                    }
                }
                List<string> _autovalidatedocs = _autoDocs.Distinct().Where(x => _unAvailableDocs.Distinct().Any(a => a == x)).ToList();
                if(_autovalidatedocs != null)
                {
                    if(_autovalidatedocs.Count > 0)
                    {
                        foreach (string _autoDocName in _autovalidatedocs)
                        {
                            List<XPathNavigator> dTypeList = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, _autoDocName);
                            foreach (XPathNavigator dt in dTypeList)
                                docTypeList.Add(dt);
                        }
                        _unAvailableDocs = _unAvailableDocs.Distinct().Where(x => !_autovalidatedocs.Distinct().Any(a => a == x)).ToList();
                    }
                }

                foreach (string _unDocumentName in _unAvailableDocs)
                {
                    List<XPathNavigator> dTypeList = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, _unDocumentName);
                    foreach (XPathNavigator dt in dTypeList)
                        docTypeList.Add(dt);
                }

                foreach (XPathNavigator xdocs in xPathDocs)
                    xdocs.DeleteSelf();

                string xPathDocuments = "/Batch/Documents";
                XPathNavigator documentsNode = inputXmlNavigator.SelectSingleNode(xPathDocuments);
                foreach (XPathNavigator xp in docTypeList)
                {
                    documentsNode.AppendChild(xp);
                }
            }
        }

        #endregion
    }




}