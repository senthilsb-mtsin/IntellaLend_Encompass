using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using IntellaLend.WFProxy;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using MTSXMLParsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.ImportToIntellaLend
{
    class IntellaLendImport : IMTSServiceBase
    {
        #region Private Variables

        //private static string EphesoftOutputPath = string.Empty;
        private static string LoanPDFPath = string.Empty;
        private static string ImageMaxHeight = string.Empty;
        private static string ImageMaxWidth = string.Empty;

        private static string LockExt = ".lck";
        private static string DoneExt = ".don";
        private static string ErrorExt = ".error";
        private static Dictionary<string, string> dicObjects;
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string SystemSchema = "IL";
        private static Int16 Retrycount = 3;
        private int CurrentRetryCnt = 0;
        #endregion

        #region Service Start DoTask

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            //EphesoftOutputPath = Params.Find(f => f.Key == "EphesoftOutputPath").Value;
            LoanPDFPath = Params.Find(f => f.Key == "LoanPDFPath").Value;

            ImageMaxHeight = Params.Find(f => f.Key == "ImageMaxHeight").Value;
            ImageMaxWidth = Params.Find(f => f.Key == "ImageMaxWidth").Value;
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
        }

        public bool DoTask()
        {
            IntellaLendImportDataAccess _dataAccess = null;
            _dataAccess = new IntellaLendImportDataAccess(SystemSchema);
            do
            {
                ImportStagings _importStageStatus = null;
                try
                {
                    _importStageStatus = _dataAccess.GetImportStageStatus();

                    if (_importStageStatus != null)
                    {
                        _importStageStatus.LoanPicked = true;
                        _dataAccess.UpdateImportStageStatus(_importStageStatus);
                        MoveToIntellaLend(_importStageStatus);
                    }
                }
                catch (DbUpdateConcurrencyException cEx)
                {
                    Exception ex = new Exception($"LoanID : {_importStageStatus.LoanId.ToString()} already in process", cEx);
                    MTSExceptionHandler.HandleException(ref ex);
                }
                catch (OptimisticConcurrencyException oEx)
                {
                    Exception ex = new Exception($"LoanID : {_importStageStatus.LoanId.ToString()} already in process.", oEx);
                    MTSExceptionHandler.HandleException(ref ex);
                }
                catch (Exception ex)
                {
                    MTSExceptionHandler.HandleException(ref ex);
                    return false;
                }
            }
            while (_dataAccess.GetImportStageStatusCount() > 0);

            return true;
        }

        #endregion

        #region Private Methods

        private bool MoveToIntellaLend(ImportStagings _loanItem)
        {
            LogMessage($"Starting Loan Process : {_loanItem.LoanId.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
            IntellaLendImportDataAccess dataAccess = null;
            dataAccess = new IntellaLendImportDataAccess(SystemSchema);
            // var xmlfile = dataAccess.GetImportStage();
            bool HasReachedMaxRetryCnt = false;
            CurrentRetryCnt = 0;
            if (!string.IsNullOrEmpty(_loanItem.Path))
            {
                string lockedXmlPath = _loanItem.Path; //Path.ChangeExtension(xmlfile.Path, LockExt);
                try
                {
                    //try
                    //{
                    //    File.Move(xmlfile.Path, lockedXmlPath);
                    //}
                    //catch (IOException) { }

                    Batch batchObj = ParseXml.GetParsedDataByFile(lockedXmlPath);

                    ProcessBatchObject(batchObj, Path.GetDirectoryName(lockedXmlPath));

                    MoveFileToDone(lockedXmlPath);
                    _loanItem.Status = ImportStagingConstant.MovedToDone;
                    dataAccess.UpdateImportStageStatus(_loanItem);
                }
                catch (LoanTypeNotFoundException exc)
                {
                    Exception ex = exc;
                    MoveFileToError(lockedXmlPath);
                    dataAccess.UpdateLoanTable(_loanItem.LoanId);
                    _loanItem.Status = ImportStagingConstant.Error;
                    _loanItem.ErrorMessage = System.Environment.MachineName + " : " + ex.Message;
                    dataAccess.UpdateImportStageStatus(_loanItem);
                    MTSExceptionHandler.HandleException(ref ex);
                    return false;
                }
                catch (Exception ex)
                {
                    Exception:
                    if (CurrentRetryCnt == Retrycount)
                    {
                        HasReachedMaxRetryCnt = true;
                        MoveFileToError(lockedXmlPath);
                        _loanItem.Status = ImportStagingConstant.Error;
                        _loanItem.ErrorMessage = System.Environment.MachineName + " : " + ex.Message;
                        dataAccess.UpdateImportStageStatus(_loanItem);
                        MTSExceptionHandler.HandleException(ref ex);
                        return false;
                    }

                    if (!HasReachedMaxRetryCnt)
                    {
                        ++CurrentRetryCnt;
                        try
                        {
                            Batch batchObj = ParseXml.GetParsedDataByFile(lockedXmlPath);

                            ProcessBatchObject(batchObj, Path.GetDirectoryName(lockedXmlPath));

                            MoveFileToDone(lockedXmlPath);
                            _loanItem.Status = ImportStagingConstant.Error;
                            _loanItem.ErrorMessage = System.Environment.MachineName + " : " + ex.Message;
                            dataAccess.UpdateImportStageStatus(_loanItem);
                        }
                        catch (LoanTypeNotFoundException exc)
                        {
                            Exception exi = exc;
                            MoveFileToError(lockedXmlPath);
                            _loanItem.Status = ImportStagingConstant.Error;
                            _loanItem.ErrorMessage = System.Environment.MachineName + " : " + ex.Message;
                            dataAccess.UpdateImportStageStatus(_loanItem);
                            MTSExceptionHandler.HandleException(ref exi);
                            return false;
                        }
                        catch (Exception excc)
                        {
                            MTSExceptionHandler.HandleException(ref excc);
                            goto Exception;
                        }
                    }
                }
            }
            LogMessage($"End Loan Process : {_loanItem.LoanId.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

            return true;
        }

        private void ProcessBatchObject(Batch batchObj, string EphesoftOutputPath)
        {
            IntellaLendImportDataAccess dataAccess = null;
            bool isMissingDocument = false;
            Int64 docID = 0;
            Int64 EUploadID = 0;
            try
            {
                UpdateLoanIDAndSchema(batchObj);

                dataAccess = new IntellaLendImportDataAccess(batchObj.Schema);

                LogMessage($"Batch Process Start : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                if (dataAccess.CheckDeletedLoans(batchObj.LoanID))
                {
                    // Prakash - Done as per Mail Sub : UHS Summary of Recent IntellaLend Open Items
                    bool loanTypeSet = true;
                    if (!dataAccess.CheckLoanType(batchObj.LoanID))
                    {
                        loanTypeSet = false;
                        try
                        {
                            string LoanTypeName = string.Empty;
                            Documents _doc = batchObj.Documents.Where(d => d.Type == "Loan Application 1003" || d.Type == "Loan Application 1003 New").FirstOrDefault();
                            if (_doc != null)
                            {
                                DocumentLevelFields _loanTypefield = _doc.DocumentLevelFields.Where(f => f.Type == "MortgageType").FirstOrDefault();
                                DocumentLevelFields _purposeTypefield = _doc.DocumentLevelFields.Where(f => f.Type == "PurposeOfLoan").FirstOrDefault();
                                string _reviewTypeName = dataAccess.GetReviewTypeName(batchObj.LoanID);
                                if (_loanTypefield != null && _purposeTypefield != null && !string.IsNullOrEmpty(_reviewTypeName))
                                {
                                    LoanTypeName = $"{_loanTypefield.Value.Trim()} {_reviewTypeName.Trim()} {_purposeTypefield.Value.Trim()}";
                                    loanTypeSet = dataAccess.SetLoanType(batchObj.LoanID, LoanTypeName);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            loanTypeSet = false;
                            MTSExceptionHandler.HandleException(ref ex);
                        }
                    }

                    if (!loanTypeSet)
                        throw new LoanTypeNotFoundException("LoanType ID Not Found");

                    UpdateDocumentTypeAndFieldType(batchObj, dataAccess);

                    UpdateLastPageNumber(batchObj);


                    //UpdateOCRConfidence(batchObj, dataAccess);

                    Int64 loanStatus = StatusConstant.PENDING_IDC;

                    isMissingDocument = dicObjects.ContainsKey("DOC_ID");

                    docID = dicObjects.ContainsKey("DOC_ID") ? Convert.ToInt64(dicObjects["DOC_ID"]) : 0;

                    Batch existingLoan = null;
                    if (isMissingDocument)
                    {
                        existingLoan = JsonConvert.DeserializeObject<Batch>(dataAccess.GetLoanObjects(batchObj.LoanID));

                        if (batchObj.Documents.Count > 0)
                        {
                            UpdateDocumentPageNo(batchObj);
                            foreach (Documents doc in batchObj.Documents)
                            {
                                UpdateDocumentVersion(existingLoan, doc);
                                existingLoan.Documents.Add(doc);
                            }
                        }

                        dataAccess.UpdateLoanDetails(batchObj, existingLoan, EphesoftOutputPath, ImageMaxHeight, ImageMaxWidth, dicObjects["DOC_ID"]);
                        loanStatus = StatusConstant.PENDING_AUDIT;
                    }
                    else
                    {
                        LogMessage($"Start Document Page No Update : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        UpdateDocumentPageNo(batchObj);
                        LogMessage($"Updating Version and Document Versions : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        UpdateDocumentVersion(batchObj);
                        LogMessage($"Start Loan Detail Insert : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        dataAccess.InsertLoanDetails(batchObj, EphesoftOutputPath, ImageMaxHeight, ImageMaxWidth);
                        LogMessage($"End Loan Detail Insert : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                    }

                    //Prakash : Will convert all tiff to .don extension 
                    //batchObj.Documents.ForEach(doc =>
                    //{
                    //    MoveFileToDone(String.IsNullOrEmpty(doc.MultiPagePdfFile) ? doc.MultiPageTiffFile : doc.MultiPagePdfFile);
                    //});

                    //get pdfbytes of documentImages and inserting it to loanpdf table
                    string existingPDF = string.Empty;
                    string missingDocumentFileName = string.Empty;

                    try
                    {
                        LogMessage($"Start Loan PDF Generation : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");
                        if (!isMissingDocument)
                        {
                            GenerateLoanPdfByStackingOrder(batchObj, dataAccess);
                            existingPDF = GetOldPDFFileName(batchObj, isMissingDocument, dataAccess);
                            EUploadID = InitializeEncompassUpload(dataAccess, batchObj);
                        }
                        else
                        {
                            GenerateLoanPdfByStackingOrder(batchObj, existingLoan, dataAccess);
                            missingDocumentFileName = GetMissingDocumentFileName(batchObj);
                            EUploadID = InitializeEncompassUpload(dataAccess, batchObj, true);
                        }
                        LogMessage($"End Loan PDF Generation : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                    }
                    catch (Exception ex)
                    {

                        MTSExceptionHandler.HandleException(ref ex);
                    }

                    Int32 LoanUploadType = dataAccess.GetLoanUploadType(batchObj.LoanID);

                    //Update to Workflow
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("TENANT_SCHEMA", batchObj.Schema);
                    dictionary.Add("LOANID", batchObj.LoanID.ToString());
                    dictionary.Add("STATUS", loanStatus.ToString());
                    dictionary.Add("MISSINGDOCUMENT", isMissingDocument.ToString());

                    if (LoanUploadType == UploadConstant.LOS)
                        dictionary.Add("LOSIMPORT", true.ToString());

                    WFProxy.ExecuteWorkFlow(dictionary);
                    LogMessage($"Batch Process End : {batchObj.LoanID.ToString()} : {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}");

                    if (!isMissingDocument && !string.IsNullOrEmpty(existingPDF) && dataAccess.CheckCustomerConfigKey() && File.Exists(existingPDF))
                        File.Delete(existingPDF);

                    if (isMissingDocument && !string.IsNullOrEmpty(missingDocumentFileName) && dataAccess.CheckCustomerConfigKey() && File.Exists(missingDocumentFileName))
                        File.Delete(missingDocumentFileName);

                    dataAccess.UpdateEUploadStatus(EUploadID);
                }
            }
            catch (LoanTypeNotFoundException exc)
            {
                if (dataAccess != null && batchObj.LoanID != 0)
                {
                    dataAccess.UpdateLoanStatus(batchObj.LoanID, StatusConstant.IDC_ERROR, ErrorCodeConstant.LOANTYPE_UNAVAILABLE, isMissingDocument, docID);
                    dataAccess.RemoveLoanEntries(batchObj.LoanID, isMissingDocument, docID, EUploadID);
                }
                LoanTypeNotFoundException newEx = new LoanTypeNotFoundException($"Exception while processing the LoanID - {batchObj.LoanID}. {exc.Message} ", exc);
                throw newEx;
            }
            catch (Exception ex)
            {
                if (dataAccess != null && batchObj.LoanID != 0)
                {
                    dataAccess.UpdateLoanStatus(batchObj.LoanID, StatusConstant.IDC_ERROR, ErrorCodeConstant.FAILED_TO_IMPORT, isMissingDocument, docID);
                    dataAccess.RemoveLoanEntries(batchObj.LoanID, isMissingDocument, docID, EUploadID);
                }
                Exception newEx = new Exception($"Exception while processing the LoanID - {batchObj.LoanID}. {ex.Message} ", ex);
                MTSExceptionHandler.HandleException(ref newEx);
                throw newEx;
            }
        }

        private Int64 InitializeEncompassUpload(IntellaLendImportDataAccess dataAccess, Batch batchObj, bool isMissingDocument = false)
        {
            string docsToUpload = string.Empty;
            if (isMissingDocument)
            {
                object docs = batchObj.Documents.Select(s => new { DocumentName = s.Type, Version = s.VersionNumber }).ToList();
                docsToUpload = JsonConvert.SerializeObject(docs);
            }

            return dataAccess.InitializeEncompassUpload(batchObj.LoanID, isMissingDocument, docsToUpload);
        }

        private void UpdateOCRConfidence(Batch batchObj, IntellaLendImportDataAccess dataAccess)
        {
            if (batchObj != null && batchObj.Documents != null && batchObj.Documents.Count > 0)
            {

                foreach (var doc in batchObj.Documents)
                {
                    decimal totalDocExtAccuracy = 0;
                    int fieldCount = 0;
                    var fieldList = dataAccess.GetFieldsForAccuracyCalc(doc.DocumentTypeID);
                    foreach (var field in fieldList)
                    {
                        var batchField = doc.DocumentLevelFields.Where(x => x.Name == field.Name).FirstOrDefault();
                        if (batchField != null)
                        {
                            decimal con = 0;
                            Decimal.TryParse(batchField.Confidence, out con);
                            totalDocExtAccuracy += con;
                            fieldCount++;
                        }
                    }

                    if (fieldCount != 0)
                        doc.DocumentExtractionAccuracy = Math.Round((totalDocExtAccuracy / fieldCount), 2).ToString();
                    else
                        doc.DocumentExtractionAccuracy = "NA";
                }


                decimal totalConfidence = 0;
                decimal totalAxtAccuracy = 0;
                foreach (var doc in batchObj.Documents)
                {
                    decimal con = 0;
                    Decimal.TryParse(doc.Confidence, out con);
                    totalConfidence += con;
                    con = 0;
                    Decimal.TryParse(doc.DocumentExtractionAccuracy, out con);
                    totalAxtAccuracy += con;
                }
                if (batchObj.Documents.Count != 0)
                {
                    batchObj.Confidence = (totalConfidence / batchObj.Documents.Count);
                    batchObj.BatchExtractionAccuracy = (totalAxtAccuracy / batchObj.Documents.Count);
                }
            }
        }

        private void UpdateDocumentVersion(Batch batchObj)
        {
            var distDocs = (from d in batchObj.Documents
                            select new { DocumentTypeID = d.DocumentTypeID }).Distinct().ToList();

            foreach (var item in distDocs)
            {
                int version = 1;
                foreach (Documents doc in batchObj.Documents.Where(b => b.DocumentTypeID == item.DocumentTypeID).ToList())
                {
                    doc.VersionNumber = version;
                    version++;

                    if (doc.DocumentTypeID == 0)
                        doc.Type = "Unknown";
                }
            }
        }

        private void UpdateDocumentVersion(Batch oldBatch, Documents doc)
        {
            var olderDoc = oldBatch.Documents.Where(x => x.DocumentTypeID == doc.DocumentTypeID).OrderByDescending(x => x.VersionNumber).FirstOrDefault();
            if (olderDoc != null && olderDoc.VersionNumber > 0)
            {
                doc.VersionNumber = olderDoc.VersionNumber + 1;
            }
            else
            {
                doc.VersionNumber = 1;
            }

            if (doc.DocumentTypeID == 0)
                doc.Type = "Unknown";
        }

        private void UpdateDocumentPageNo(Batch batchObj)
        {
            foreach (Documents doc in batchObj.Documents)
            {
                Dictionary<string, string> pageNoMapping = new Dictionary<string, string>();
                int newpageNo = 0;
                foreach (string docpageNO in doc.Pages)
                {
                    pageNoMapping.Add(docpageNO, newpageNo.ToString());
                    newpageNo++;
                }
                doc.DocumentLevelFields.ForEach(field =>
                {
                    field.PageNo = pageNoMapping.ContainsKey(field.PageNo) ? pageNoMapping[field.PageNo] : "1";
                });
            }
        }

        private void UpdateDocumentTypeAndFieldType(Batch batchObj, IntellaLendImportDataAccess dataAccess)
        {
            dataAccess.UpdateDocumentTypeAndFieldType(batchObj);
        }

        private void UpdateLoanIDAndSchema(Batch batchObj)
        {
            string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
            dicObjects = CommonUtils.ExtractDataFromString(batchObj.BatchName, patterns);
            batchObj.Schema = dicObjects["SCHEMA"];
            batchObj.LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);
        }

        private void UpdateLastPageNumber(Batch batchObj)
        {
            List<Int64> pageNoList = new List<Int64>();
            foreach (var document in batchObj.Documents)
                foreach (var page in document.Pages)
                {
                    Int64 pageNo = ExtractPageNo(page);
                    if (!pageNoList.Contains(pageNo))
                    {
                        pageNoList.Add(pageNo);
                    }
                }

            batchObj.LastPageNumber = pageNoList.Count == 0 ? "0" : pageNoList.Max().ToString();
        }

        private int ExtractPageNo(string spageNo)
        {
            string extPattern = @"PG(?<PageNO>\d+)";
            int pageNo = 0;
            Int32.TryParse(CommonUtils.ExtractDataFromString(spageNo, extPattern)["PageNO"], out pageNo);
            return pageNo;
        }

        #region File Manipulations 

        private void MoveFileToDone(string filePath)
        {
            if (File.Exists(filePath))
                File.Move(filePath, Path.ChangeExtension(filePath, DoneExt));
        }

        private void MoveFileToError(string filePath)
        {
            if (File.Exists(filePath))
                File.Move(filePath, Path.ChangeExtension(filePath, ErrorExt));
        }


        private byte[] GetPdfBytes(Int64 loanId, string tenantSchema, ImageMinIOWrapper _imageWrapper)
        {
            List<byte[]> documentsImageByteList = new List<byte[]>();
            IntellaLendImportDataAccess dataAccess = new IntellaLendImportDataAccess(tenantSchema);
            Loan loan = dataAccess.GetLoanInfo(loanId);
            if (loan != null)
            {
                Int64 stackingOrderId = dataAccess.GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                List<StackingOrderDetailMaster> stackingOrderDetailList = dataAccess.GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();

                foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                {
                    List<LoanImage> loanImageDetails = dataAccess.GetLoanImages(loanId, item.DocumentTypeID);

                    foreach (LoanImage imgdetail in loanImageDetails)
                    {
                        byte[] _imgBytes = _imageWrapper.GetLoanImage(loanId, imgdetail.ImageGUID);
                        documentsImageByteList.Add(_imgBytes);
                    }
                }

                if (documentsImageByteList.Count > 0)
                {
                    byte[] pdfBytes = CommonUtils.CreatePdfBytes(documentsImageByteList);
                    return pdfBytes;
                }
            }
            return null;
        }


        #region LoanPDF Genreation

        private void GenerateLoanPdfByStackingOrder(Batch batchObj, Batch existingLoan, IntellaLendImportDataAccess dataAccess)
        {

            string missingDocumentFileName = GetMissingDocumentFileName(batchObj);
            //string existingPDF = GetOldPDFFileName(batchObj, true, dataAccess);
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(batchObj.Schema);
            byte[] existingPDF = _imageWrapper.GetLoanPDF(batchObj.LoanID);
            if (!IsPageNumberOrdered(existingLoan))
            {
                GenerateLoanPdfFromDBImages(existingLoan, dataAccess);
                return;
            }

            // if (!string.IsNullOrEmpty(missingDocumentFileName) && !string.IsNullOrEmpty(existingPDF))
            if (!string.IsNullOrEmpty(missingDocumentFileName))
            {
                int lastPageNo = CommonUtils.GetPDFPageCount(existingPDF);

                List<int> pageNoList = new List<int>();

                if (batchObj.Documents.Count == 1)
                {
                    var doc = existingLoan.Documents[existingLoan.Documents.Count - 1];
                    for (int i = 0; i < doc.Pages.Count; i++)
                    {
                        int pno = ExtractPageNo(doc.Pages[i]);
                        if (!pageNoList.Contains(pno + 1))
                        {
                            pageNoList.Add(pno + 1);
                        }
                        doc.Pages[i] = lastPageNo.ToString();
                        lastPageNo++;
                    }
                }
                else
                {
                    var _docs = existingLoan.Documents.GetRange((existingLoan.Documents.Count - batchObj.Documents.Count), batchObj.Documents.Count);

                    foreach (var doc in _docs)
                    {
                        for (int i = 0; i < doc.Pages.Count; i++)
                        {
                            int pno = ExtractPageNo(doc.Pages[i]);
                            if (!pageNoList.Contains(pno + 1))
                            {
                                pageNoList.Add(pno + 1);
                            }
                            doc.Pages[i] = lastPageNo.ToString();
                            lastPageNo++;
                        }
                    }
                }

                byte[] _loanPdf = CommonUtils.AppendToPDF(existingPDF, missingDocumentFileName, pageNoList.ToArray());

                // dataAccess.UpdateLoanPdf(batchObj.LoanID);

                GenerateLoanPdfByStackingOrder(existingLoan, dataAccess, _loanPdf);

                if (dataAccess.CheckCustomerConfigKey())
                    File.Delete(missingDocumentFileName);
            }
            else
            {
                GenerateLoanPdfFromDBImages(existingLoan, dataAccess);
                return;
            }
        }

        private void GenerateLoanPdfByStackingOrder(Batch batchObj, IntellaLendImportDataAccess dataAccess, bool isMissingDocument = false)
        {

            List<int> pageNoList = new List<int>();

            Loan loan = dataAccess.GetLoanInfo(batchObj.LoanID);

            if (loan != null)
            {
                string existingPDF = GetOldPDFFileName(batchObj, isMissingDocument, dataAccess);

                if (!string.IsNullOrEmpty(existingPDF))
                {
                    //Get Stacking Order
                    int pageSequence = 0;
                    Int64 stackingOrderId = dataAccess.GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                    List<StackingOrderDetailMaster> stackingOrderDetailList = dataAccess.GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();
                    foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                    {
                        //List<LoanImage> loanImageDetails = dataAccess.GetLoanImages(batchObj.LoanID, item.DocumentTypeID);
                        var docList = batchObj.Documents.Where(X => X.DocumentTypeID == item.DocumentTypeID).OrderBy(sd => sd.VersionNumber).ToList();
                        GetPageNumberOrder(docList, isMissingDocument, ref pageSequence, ref pageNoList);

                    }

                    //
                    bool incAllDocs = dataAccess.GetIncLoantypeDocs();
                    if (incAllDocs)
                    {
                        List<Documents> listDoc = batchObj.Documents.Where(l => !stackingOrderDetailList.Any(ls => ls.DocumentTypeID == l.DocumentTypeID)).OrderBy(d => d.DocumentTypeID).ThenByDescending(d => d.VersionNumber).ToList();
                        GetPageNumberOrder(listDoc, isMissingDocument, ref pageSequence, ref pageNoList);
                    }
                    var isDeletOrgFile = dataAccess.CheckCustomerConfigKey();

                    Dictionary<Int32, string> pgLevelLS = new Dictionary<Int32, string>();
                    foreach (Documents doc in batchObj.Documents)
                    {
                        if (doc.PageLevelFields != null)
                        {
                            List<PageLevelFields> pgLs = doc.PageLevelFields.Where(p => p.IsRotated == true).ToList();

                            foreach (PageLevelFields pg in pgLs)
                                pgLevelLS[Convert.ToInt32(pg.PageNo)] = pg.Direction.ToUpper();

                        }
                    }


                    string newPDFPath = string.Empty;

                    string directorypath = Path.Combine(LoanPDFPath, batchObj.Schema, DateTime.Now.ToString("yyyyMMdd"));
                    newPDFPath = Path.Combine(directorypath, batchObj.LoanID + ".pdf");

                    //if (!Directory.Exists(directorypath))
                    //Directory.CreateDirectory(directorypath);

                    byte[] _pdfBytes = new byte[0];

                    if (pageNoList.Count > 0)
                    {
                        byte[] _oldPDFBytes = File.ReadAllBytes(existingPDF);
                        if (!isMissingDocument)
                        {
                            _pdfBytes = CommonUtils.ReOrderPDFPages(_oldPDFBytes, newPDFPath, pageNoList.ToArray(), pgLevelLS);

                            if (dataAccess.CheckCustomerConfigKey())
                                File.Delete(existingPDF);
                        }
                        else
                        {
                            _pdfBytes = CommonUtils.ReOrderPDFPages(_oldPDFBytes, existingPDF, pageNoList.ToArray(), pgLevelLS);
                        }

                        new ImageMinIOWrapper(batchObj.Schema).InsertLoanPDF(batchObj.LoanID, _pdfBytes);

                        dataAccess.InsertLoanPdf(batchObj, newPDFPath);
                    }
                }
                else
                {
                    GenerateLoanPdfFromDBImages(batchObj, dataAccess);
                }
            }
        }

        private void GenerateLoanPdfByStackingOrder(Batch batchObj, IntellaLendImportDataAccess dataAccess, byte[] _oldPDFBytes)
        {

            List<int> pageNoList = new List<int>();

            Loan loan = dataAccess.GetLoanInfo(batchObj.LoanID);

            if (loan != null)
            {
                //Get Stacking Order
                int pageSequence = 0;
                Int64 stackingOrderId = dataAccess.GetStackingOrderId(loan.CustomerID, loan.ReviewTypeID, loan.LoanTypeID);
                List<StackingOrderDetailMaster> stackingOrderDetailList = dataAccess.GetStackingOrderInfo(stackingOrderId).OrderBy(sd => sd.SequenceID).ToList();
                foreach (StackingOrderDetailMaster item in stackingOrderDetailList)
                {
                    //List<LoanImage> loanImageDetails = dataAccess.GetLoanImages(batchObj.LoanID, item.DocumentTypeID);
                    var docList = batchObj.Documents.Where(X => X.DocumentTypeID == item.DocumentTypeID).OrderBy(sd => sd.VersionNumber).ToList();
                    GetPageNumberOrder(docList, true, ref pageSequence, ref pageNoList);

                }

                //
                bool incAllDocs = dataAccess.GetIncLoantypeDocs();
                if (incAllDocs)
                {
                    List<Documents> listDoc = batchObj.Documents.Where(l => !stackingOrderDetailList.Any(ls => ls.DocumentTypeID == l.DocumentTypeID)).OrderBy(d => d.DocumentTypeID).ThenByDescending(d => d.VersionNumber).ToList();
                    GetPageNumberOrder(listDoc, true, ref pageSequence, ref pageNoList);
                }
                var isDeletOrgFile = dataAccess.CheckCustomerConfigKey();

                Dictionary<Int32, string> pgLevelLS = new Dictionary<Int32, string>();
                foreach (Documents doc in batchObj.Documents)
                {
                    if (doc.PageLevelFields != null)
                    {
                        List<PageLevelFields> pgLs = doc.PageLevelFields.Where(p => p.IsRotated == true).ToList();

                        foreach (PageLevelFields pg in pgLs)
                            pgLevelLS[Convert.ToInt32(pg.PageNo)] = pg.Direction.ToUpper();

                    }
                }


                //string newPDFPath = string.Empty;

                //string directorypath = Path.Combine(LoanPDFPath, batchObj.Schema, DateTime.Now.ToString("yyyyMMdd"));
                //newPDFPath = Path.Combine(directorypath, batchObj.LoanID + ".pdf");

                //if (!Directory.Exists(directorypath))
                //Directory.CreateDirectory(directorypath);

                byte[] _pdfBytes = new byte[0];

                if (pageNoList.Count > 0)
                {
                    _pdfBytes = CommonUtils.ReOrderPDFPages(_oldPDFBytes, "", pageNoList.ToArray(), pgLevelLS);

                    new ImageMinIOWrapper(batchObj.Schema).InsertLoanPDF(batchObj.LoanID, _pdfBytes);

                    dataAccess.InsertLoanPdf(batchObj, "");
                }

            }
        }


        private void GetPageNumberOrder(List<Documents> _listDocs, bool isMissingDocument, ref int pageSequence, ref List<Int32> pageNoList)
        {

            foreach (Documents doc in _listDocs)
            {
                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    int pno = 0;

                    if (isMissingDocument && doc.Pages[i].ToUpper().StartsWith("PG"))
                        continue;

                    if (doc.Pages[i].ToUpper().StartsWith("PG"))
                    {
                        pno = ExtractPageNo(doc.Pages[i]);
                    }
                    else
                    {
                        pno = Convert.ToInt32(doc.Pages[i]);
                    }

                    if (!pageNoList.Contains(pno + 1))
                    {
                        pageNoList.Add(pno + 1);
                    }
                    //Update new Sequence Page no
                    doc.Pages[i] = pageSequence.ToString();
                    pageSequence++;
                }
            }
        }

        private bool IsPageNumberOrdered(Batch batchObj)
        {
            foreach (var doc in batchObj.Documents)
            {
                foreach (var page in doc.Pages)
                {
                    if (!page.ToUpper().StartsWith("PG"))
                        return true;
                }
            }

            return false;
        }

        private string GetOldPDFFileName(Batch batchObj, bool isMissingDocument, IntellaLendImportDataAccess dataAccess)
        {
            if (isMissingDocument)
            {
                return dataAccess.GetLoanPdfPath(batchObj.LoanID);
            }
            else
            {

                string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, batchObj.Schema);
                string loanFile = batchObj.Schema + "_" + batchObj.LoanID.ToString() + ".don";
                var fileSerach = Directory.GetFiles(Path.Combine(tenantFolder, "Output"), loanFile, SearchOption.AllDirectories).ToArray();
                if (fileSerach.Length > 0)
                {
                    return fileSerach[0];
                }
            }

            return string.Empty;
        }


        private string GetMissingDocumentFileName(Batch batchObj)
        {
            Int64 DocID = 0;
            if (dicObjects.ContainsKey("DOC_ID"))
                DocID = Convert.ToInt64(dicObjects["DOC_ID"]);

            string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, batchObj.Schema);
            string loanFile = batchObj.Schema + "_" + batchObj.LoanID.ToString() + "_" + DocID.ToString() + ".don";
            var fileSerach = Directory.GetFiles(Path.Combine(tenantFolder, "Output"), loanFile, SearchOption.AllDirectories).ToArray();
            if (fileSerach.Length > 0)
            {
                return fileSerach[0];
            }
            return string.Empty;
        }

        private void GenerateLoanPdfFromDBImages(Batch batchObj, IntellaLendImportDataAccess dataAccess)
        {
            ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(batchObj.Schema);
            byte[] pdfBytes = GetPdfBytes(batchObj.LoanID, batchObj.Schema, _imageWrapper);

            if (pdfBytes != null)
            {
                //string directorypath = Path.Combine(LoanPDFPath, batchObj.Schema, DateTime.Now.ToString("yyyyMMdd"));
                //string pdfpath = Path.Combine(directorypath, batchObj.LoanID + ".pdf");

                _imageWrapper.InsertLoanPDF(batchObj.LoanID, pdfBytes);

                //if (!Directory.Exists(directorypath))
                //    Directory.CreateDirectory(directorypath);

                dataAccess.InsertLoanPdf(batchObj, "");

                //File.WriteAllBytes(pdfpath, pdfBytes);
            }
        }

        #endregion

        #endregion

        #endregion



        private void LogMessage(string msg)
        {
            Logger.WriteTraceLog(msg);
        }
    }
}
