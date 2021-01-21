using EncompassAPIHelper;
using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using IntellaLend.Model.Encompass;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.EncompassFileDownloader
{
    public class EncompassFileDownloader : IMTSServiceBase
    {


        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;
        private static string ProcessedEFolder = string.Empty;
        private const Int32 PDF_CREATED = 0;
        private static string GhostScriptPath = string.Empty;
        private static string InvestorLoanTypes = string.Empty;
        private static string CurrentReviewTypeName = string.Empty;
        private List<List<Dictionary<string, string>>> _queryCombinations = new List<List<Dictionary<string, string>>>();
        private static string UploadEFolder = string.Empty;
        private static string LastFinishedMileStone = string.Empty;
        private static string ServiceTypes = string.Empty;
        private static string LastFinishedMileStoneFieldID = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value; //http://mts100:8099/
            ProcessedEFolder = Params.Find(f => f.Key == "ProcessedEFolder").Value;
            UploadEFolder = Params.Find(f => f.Key == "UploadEFolder").Value;
            LastFinishedMileStoneFieldID = Params.Find(f => f.Key == "LastFinishedMileStoneFieldID").Value;
            LastFinishedMileStone = Params.Find(f => f.Key == "LastFinishedMileStone").Value;
            ServiceTypes = Params.Find(f => f.Key == "ServiceTypes").Value;
        }

        public bool DoTask()
        {
            try
            {
                var TenantList = EncompassFileDownloaderDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EncompassFileDownloaderDataAccess dataAccess = new EncompassFileDownloaderDataAccess(tenant.TenantSchema);
                    EncompassWrapperAPI _api = new EncompassWrapperAPI(EncompassWrapperAPIURL, tenant.TenantSchema);
                    DownloadFromEncompass(_api, dataAccess);
                    _api.Dispose();
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return true;
        }

        public void GetCombinations(List<Dictionary<string, List<string>>> fields, List<Dictionary<string, string>> output)
        {
            LogMessage($"{fields.Count } == {output.Count}");
            if (fields.Count == output.Count)
            {
                _queryCombinations.Add(output);
                return;
            }
            foreach (var value in fields[output.Count].Values.FirstOrDefault())
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                var newVector = output.ToList();
                dic.Add(fields[output.Count].Keys.FirstOrDefault(), value);
                newVector.Add(dic);
                GetCombinations(fields, newVector);
            }
        }

        private void DownloadFromEncompass(EncompassWrapperAPI _api, EncompassFileDownloaderDataAccess dataAccess)
        {
            LogMessage("In DownloadFromEncompass ");
            List<IntellaAndEncompassFetchFields> _enLookupFields = dataAccess.GetEncompassLookUpFields(dataAccess.TenantSchema);
            LogMessage($"_enLookupFields {_enLookupFields.Count}");
            //List<IntellaAndEncompassFetchFields> _enImportFields = dataAccess.GetEncompassImportFields();
            //LogMessage($"_enImportFields {_enImportFields.Count}");
            List<IntellaAndEncompassFetchFields> _enLoanSearchFields = dataAccess.GetEncompassSearchFields();
            LogMessage($"_enImportFields {_enLoanSearchFields.Count}");
            List<Dictionary<string, List<string>>> _eQueryFields = new List<Dictionary<string, List<string>>>();
            // LogMessage(_enImportFields.Count.ToString());
            List<string> _enExceptionLoans = dataAccess.GetEncompassExceptionLoans();
            LogMessage(_enExceptionLoans.Count.ToString());
            //foreach (IntellaAndEncompassFetchFields item in _enImportFields)
            //{
            //    LogMessage(item.EncompassFieldValue);
            //    Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            //    dic.Add(item.EncompassFieldID, item.EncompassFieldValue.Split(',').ToList());
            //    _eQueryFields.Add(dic);
            //}

            //Dictionary<string, List<string>> dicMile = new Dictionary<string, List<string>>();
            //dicMile.Add(LastFinishedMileStoneFieldID, LastFinishedMileStone.Split(',').ToList());
            //_eQueryFields.Add(dicMile);
            //_queryCombinations = new List<List<Dictionary<string, string>>>();
            //GetCombinations(_eQueryFields, new List<Dictionary<string, string>>());
            //LogMessage(JsonConvert.SerializeObject(_queryCombinations));

            List<string> _lsLoans = new List<string>();

            List<EWebhookEvents> _eWebHookEvents = dataAccess.GetWebHooksEvent();

            //IntellaAndEncompassFetchFields _serviceType = _enImportFields.Where(x => x.FieldType.Contains(LOSFieldType.SERVICETYPE)).FirstOrDefault();
            //IntellaAndEncompassFetchFields _updateField = _enImportFields.Where(x => x.FieldType.Contains(LOSFieldType.UPDATE)).FirstOrDefault();

            foreach (var _eWebHookEvent in _eWebHookEvents)
            {
                Int64 downloadID = 0;
                string LoanNumber = string.Empty;

                try
                {
                    dynamic obj = JsonConvert.DeserializeObject(_eWebHookEvent.Response);
                    string _eLoanGUID = obj.loanGUID;
                    dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_PROCESSING);

                    downloadID = dataAccess.InsertEDownload(new Guid(_eLoanGUID), EncompassStatusConstant.DOWNLOAD_PENDING);
                    LogMessage($"downloadID : {downloadID}");
                    //_enLookupFields.Add(_serviceType);
                    List<string> _eLookUpFields = _enLookupFields.Select(x => x.EncompassFieldID).ToList();
                    _eLookUpFields.Add(EncompassFieldConstant.LOAN_NUMBER);
                    _eLookUpFields.Add(LastFinishedMileStoneFieldID);
                    _eLookUpFields = _eLookUpFields.Union(_enLoanSearchFields.Select(x => x.EncompassFieldID).ToList()).Distinct().ToList();
                    LogMessage($"_eLoanGUID : {_eLoanGUID}");
                    LogMessage(JsonConvert.SerializeObject(_eLookUpFields));
                    List<EFieldResponse> _enFieldResponse = GetAllFieldValuesFromEncompass(_api, _eLoanGUID, _eLookUpFields.ToArray());
                    LogMessage(JsonConvert.SerializeObject(_enFieldResponse));
                    EFieldResponse _eResponse = _enFieldResponse.Where(x => x.FieldId == LastFinishedMileStoneFieldID).FirstOrDefault();
                    LogMessage(JsonConvert.SerializeObject(_eResponse));

                    List<string> _eServiceTypes = LastFinishedMileStone.Split(',').ToList();
                    LogMessage((_eServiceTypes.IndexOf(_eResponse.Value)).ToString());

                    List<string> _iServiceTypes = ServiceTypes.Split(',').ToList();
                    LogMessage(JsonConvert.SerializeObject(_iServiceTypes));

                    LoanNumber = _enFieldResponse.Where(e => e.FieldId == EncompassFieldConstant.LOAN_NUMBER).FirstOrDefault().Value;

                    if (_eServiceTypes.IndexOf(_eResponse.Value) >= 0)
                    {
                        string CurrentReviewTypeName = _iServiceTypes[_eServiceTypes.IndexOf(_eResponse.Value)].Trim();
                        LogMessage(JsonConvert.SerializeObject(_queryCombinations));

                        Int64 ReviewTypeID = dataAccess.GetReviewTypeID(CurrentReviewTypeName);

                        if (ReviewTypeID != 0)
                        {
                            Loan loan = new Loan() { ReviewTypeID = ReviewTypeID };
                            LoanSearch loanSearch = new LoanSearch();
                            LoanLOSFields _loanLOSField = new LoanLOSFields();

                            SetLoanInfo(loan, _eLoanGUID, loanSearch, _enLookupFields, _enFieldResponse, dataAccess, LoanNumber, _loanLOSField, _enLoanSearchFields);

                            dataAccess.InsertLoan(loan, loanSearch, _loanLOSField, downloadID);
                            PDFMerger merger = null;
                            List<byte[]> _pdfBytes = new List<byte[]>();
                            string lockFileName = string.Empty;
                            bool _docAssigned = false;
                            try
                            {
                                CheckAttachment:

                                List<EContainer> eContainers = _api.GetAllLoanDocuments(_eLoanGUID);

                                EContainer uploadContainer = eContainers.Where(x => x.Title == UploadEFolder).FirstOrDefault();

                                if (uploadContainer != null)
                                {
                                    List<EDocumentAttachment> _eDocAttachments = uploadContainer.Attachments;

                                    if (_eDocAttachments.Count == 0)
                                        goto BreakAttachmentLoop;

                                    List<EAttachment> _lsEAttachments = new List<EAttachment>();

                                    foreach (var item in _eDocAttachments)
                                    {
                                        string[] _attGUID = item.EntityId.Split('.');
                                        string _attachmentGUID = _attGUID[0].Replace("attachment-", "");
                                        EAttachment eAttachment = _api.GetAttachment(_eLoanGUID, _attachmentGUID);
                                        _lsEAttachments.Add(eAttachment);
                                    }

                                    List<EDownloadStaging> _steps = dataAccess.SetDownloadSteps(_lsEAttachments, downloadID, _eLoanGUID);
                                    List<EDownloadStaging> _movedFiles = new List<EDownloadStaging>();
                                    foreach (EDownloadStaging item in _steps.Where(s => s.Step == EncompassDownloadStepConstant.LoanAttachment).ToArray())
                                    {
                                        try
                                        {
                                            LogMessage($"Loan GUID : {_eLoanGUID}, AttachmentGUID : {item.AttachmentGUID}, EAttachmentName : {item.EAttachmentName}");
                                            dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Processing);
                                            byte[] _fileArrary = _api.DownloadAttachment(_eLoanGUID, item.AttachmentGUID, item.EAttachmentName);
                                            _pdfBytes.Add(_fileArrary);
                                            dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Completed);
                                            _movedFiles.Add(item);
                                        }
                                        catch (EncompassWrapperLoanLockException ex)
                                        {
                                            throw ex;
                                        }
                                        catch (Exception ex)
                                        {
                                            dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Error, ex.Message);
                                            throw ex;
                                        }
                                    }

                                    List<string> attachmentGUIDs = _movedFiles.Select(a => a.AttachmentGUID).ToList();

                                    //Remove Attachments from MTS Upload Container
                                    if (attachmentGUIDs.Count > 0)
                                        _api.RemoveDocumentAttachments(_eLoanGUID, uploadContainer.DocumentId, attachmentGUIDs, UploadEFolder);

                                    //Assign Attachments to Processed folder
                                    EContainer eContainer = eContainers.Where(e => e.Title == ProcessedEFolder).FirstOrDefault();

                                    if (eContainer != null)
                                    {
                                        _docAssigned = _api.AssignDocumentAttachments(_eLoanGUID, eContainer.DocumentId, attachmentGUIDs, ProcessedEFolder);
                                    }
                                    else
                                    {
                                        AddContainerResponse res = _api.AddDocument(_eLoanGUID, ProcessedEFolder);
                                        _docAssigned = _api.AssignDocumentAttachments(_eLoanGUID, res.DocumentID, attachmentGUIDs, ProcessedEFolder);
                                    }

                                    goto CheckAttachment;
                                }

                                BreakAttachmentLoop:

                                string OrgFileName = string.Empty;

                                if (_pdfBytes.Count > 0)
                                {

                                    string exactPath = Path.Combine(IntellaLendLoanUploadPath, dataAccess.TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                                    string newFileName = dataAccess.TenantSchema + "_" + loan.LoanID.ToString() + ".lck";

                                    lockFileName = Path.Combine(exactPath, newFileName);

                                    OrgFileName = Path.ChangeExtension(lockFileName, ".pdf");


                                    try
                                    {
                                        if (!Directory.Exists(exactPath))
                                            Directory.CreateDirectory(exactPath);

                                        merger = new PDFMerger(lockFileName);
                                        merger.OpenDocument();
                                        for (int j = 0; j < _pdfBytes.Count; j++)
                                        {
                                            merger.AppendPDF(_pdfBytes[j]);
                                        }
                                        merger.SaveDocument();
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("Error while creating Loan PDF", ex);
                                    }

                                    if (_docAssigned)
                                    {
                                        dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_SUCCESS, loan.LoanNumber);
                                        dataAccess.DeleteStaginDetails(loan.EnCompassLoanGUID);
                                        dataAccess.DeleteWebHookEvents(_eWebHookEvent.ID);
                                    }
                                    else
                                    {
                                        dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                                        dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_FAILED, loan.LoanNumber);
                                    }
                                }
                                else
                                {
                                    if (downloadID > 0)
                                    {
                                        dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                                        dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_FAILED, LoanNumber, "Attachment(s) not found in Encompass Unassigned folder");
                                    }

                                    throw new Exception($"Attachment(s) not found in Encompass {UploadEFolder} folder");
                                }

                                Logger.WriteTraceLog($"lockFileName : {lockFileName}");
                                Logger.WriteTraceLog($"OrgFileName : {OrgFileName}");

                                if (File.Exists(lockFileName))
                                    File.Move(lockFileName, OrgFileName);

                                dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_SUCCESS, LoanNumber);
                            }
                            catch (EncompassWrapperLoanLockException ex)
                            {
                                throw ex;
                            }
                            catch (Exception ex)
                            {
                                if (merger != null)
                                {
                                    merger.SaveDocument();
                                }

                                if (File.Exists(lockFileName))
                                    File.Delete(lockFileName);

                                if (downloadID > 0)
                                {
                                    dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_FAILED, LoanNumber, ex.Message);
                                    dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                                }
                                MTSExceptionHandler.HandleException(ref ex);
                            }
                        }
                        else
                        {
                            if (downloadID > 0)
                            {
                                dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_FAILED, LoanNumber, $"{CurrentReviewTypeName} service type not assigned to any customer in IntellaLend");
                                dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                            }
                        }
                    }
                    else
                    {
                        if (downloadID > 0)
                        {
                            dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_FAILED, LoanNumber, $"{_eResponse.Value} milestone not mapped to any Service Type in IntellaLend");
                            dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                        }
                    }
                }
                catch (EncompassWrapperLoanLockException ex)
                {
                    if (downloadID > 0)
                        dataAccess.DeleteEDownloadStatus(downloadID);

                    Exception exe = new Exception("Loan Locked", ex);
                    MTSExceptionHandler.HandleException(ref exe);
                }
                catch (Exception ex)
                {
                    LogMessage("Download ID : " + downloadID);
                    if (downloadID > 0)
                    {
                        dataAccess.UpdateEDownloadStatus(downloadID, EncompassStatusConstant.DOWNLOAD_FAILED, LoanNumber, ex.Message);
                        dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                    }
                    else
                    {
                        dataAccess.UpdateStatusEwebHookEvents(_eWebHookEvent.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                    }
                    MTSExceptionHandler.HandleException(ref ex);
                }

            }
        }



        private string GetLoanNoFromEncompass(EncompassWrapperAPI _api, string _eLoanGUID)
        {
            string[] _enLoanNoLookup = { EncompassFieldConstant.LOAN_NUMBER };
            List<EFieldResponse> enLoanNoData = _api.GetPredefinedFieldValues(_eLoanGUID, _enLoanNoLookup);
            string LoanNumber = enLoanNoData.Where(e => e.FieldId == EncompassFieldConstant.LOAN_NUMBER).FirstOrDefault().Value;
            return LoanNumber;
        }
        private List<EFieldResponse> GetAllFieldValuesFromEncompass(EncompassWrapperAPI _api, string _eLoanGUID, string[] fields)
        {
            List<EFieldResponse> _enFieldResponse = _api.GetPredefinedFieldValues(_eLoanGUID, fields);
            return _enFieldResponse;
        }

        private void SetLoanInfo(Loan loan, string _eLoanGUID, LoanSearch loanSearch, List<IntellaAndEncompassFetchFields> _enLookupFields, List<EFieldResponse> _enFieldResponse, EncompassFileDownloaderDataAccess dataAccess, string LoanNumber, LoanLOSFields _loanLOSField, List<IntellaAndEncompassFetchFields> _enLoanSearchFields)
        {
            //Assign Encompass values
            foreach (string Field in LOSLookupMapping.GetDestinationFields)
            {
                IntellaAndEncompassFetchFields _enField = _enLookupFields.Where(f => f.IntellaMappingColumn == Field.Trim()).FirstOrDefault();
                string[] fieldType = Field.Split('.');
                if (_enField != null)
                {
                    //string _fieldValue = EncompassConnector.GetFieldValueByLoan(_enLoan, _enField.EncompassFieldID);
                    string _fieldValue = _enFieldResponse.Where(e => e.FieldId == _enField.EncompassFieldID).FirstOrDefault().Value;
                    string CutomerName = string.Empty;
                    switch (fieldType[1].ToUpper())
                    {
                        case "CUSTOMERID":
                            {

                                if (!string.IsNullOrEmpty(_fieldValue))
                                {
                                    Int64 CustomerID = dataAccess.GetCustomerID(_fieldValue.Trim());

                                    CutomerName = _fieldValue.Trim();

                                    if (CustomerID != 0)
                                    {
                                        if (dataAccess.CheckCustomerReviewTypeMapping(CustomerID, loan.ReviewTypeID))
                                            loan.CustomerID = CustomerID;
                                        else
                                            throw new Exception($"'{CurrentReviewTypeName}' service type not assigned to '{_fieldValue.Trim()}' customer in IntellaLend"); //CustomerID : {CustomerID} & ReviewTypeID : {loan.ReviewTypeID}, Encompass Loan No : {LoanNumber}");
                                    }
                                    else
                                        throw new Exception($"Encompass Investor name '{_fieldValue}' does not exist in IntellaLend");
                                }
                                else
                                    throw new Exception($"Investor name is empty for the Loan '{LoanNumber}' in Encompass");
                            }
                            break;
                        case "LOANTYPEID":
                            {
                                if (!string.IsNullOrEmpty(_fieldValue))
                                {
                                    Int64 LoanTypeID = 0;
                                    string CustomerName = string.Empty;
                                    //Prakash : As per regular stand up call with RB on 24/05/2019
                                    //Mail Sub : Fwd: Republic Bank Business Test Case Scenarios v1.0 Loan 011849707 - Smith PHFA VA.xlsx
                                    string[] lsInvestorLoanTypes = InvestorLoanTypes.Split(',');

                                    string LoanTypeName = string.Empty;

                                    if (lsInvestorLoanTypes.Any(l => l == _fieldValue))
                                    {
                                        if (loan.CustomerID != 0)
                                        {
                                            CustomerName = dataAccess.GetCustomerByID(loan.CustomerID);
                                            if (string.IsNullOrEmpty(CustomerName.Trim()))
                                            {
                                                LoanTypeID = dataAccess.GetLoanTypeID(CustomerName.Trim());
                                                LoanTypeName = CustomerName.Trim();
                                            }
                                        }
                                    }

                                    if (LoanTypeID == 0)
                                    {
                                        LoanTypeID = dataAccess.GetLoanTypeID(_fieldValue.Trim());
                                        LoanTypeName = _fieldValue.Trim();
                                    }

                                    if (LoanTypeID != 0)
                                    {
                                        if (dataAccess.CheckCustomerReviewLoanTypeMapping(loan.CustomerID, loan.ReviewTypeID, LoanTypeID))
                                            loan.LoanTypeID = LoanTypeID;
                                        else
                                            throw new Exception($"'{LoanTypeName}' Loan type not assigned to service type '{CurrentReviewTypeName}' for the customer '{CustomerName}' in IntellaLend"); // CustomerID : {loan.CustomerID} & ReviewTypeID : {loan.ReviewTypeID} & LoanTypeID : {LoanTypeID}, Encompass Loan No : {LoanNumber}");
                                    }
                                    else
                                        throw new Exception($"'{LoanTypeName}' Loan type does not exist in IntellaLend"); // Encompass LoanType Name : {_fieldValue}, Encompass Loan No : {LoanNumber}");
                                }
                                else
                                    throw new Exception($"LoanType is empty for the Loan '{LoanNumber}' in Encompass");
                            }
                            break;
                        case "CLOSER":
                            {
                                _loanLOSField.Closer = _fieldValue.Trim();
                            }
                            break;
                        case "LOANOFFICER":
                            {
                                _loanLOSField.LoanOfficer = _fieldValue.Trim();
                            }
                            break;
                        case "POSTCLOSER":
                            {
                                _loanLOSField.PostCloser = _fieldValue.Trim();
                            }
                            break;
                        case "UNDERWRITER":
                            {
                                _loanLOSField.Underwriter = _fieldValue.Trim();
                            }
                            break;
                        case "EMAILCLOSER":
                            {
                                _loanLOSField.EmailCloser = _fieldValue.Trim();
                            }
                            break;
                        case "EMAILLOANOFFICER":
                            {
                                _loanLOSField.EmailLoanOfficer = _fieldValue.Trim();
                            }
                            break;
                        case "EMAILPOSTCLOSER":
                            {
                                _loanLOSField.EmailPostCloser = _fieldValue.Trim();
                            }
                            break;
                        case "EMAILUNDERWRITER":
                            {
                                _loanLOSField.EmailUnderwriter = _fieldValue.Trim();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            //Check with AMAR/Senthil/Sam //need to fill

            loan.Priority = dataAccess.GetReviewTypePriority(loan.ReviewTypeID);

            loan.Status = StatusConstant.PENDING_ENCOMPASS_DOWNLOAD;
            loan.LoanNumber = LoanNumber; //need to fill
            loan.SubStatus = 0;
            loan.UploadedUserID = 1;
            loan.LoggedUserID = 0;
            loan.FileName = string.Empty;
            loan.UploadType = UploadConstant.ENCOMPASS;
            loan.CreatedOn = DateTime.Now;
            loan.ModifiedOn = DateTime.Now;
            loan.LastAccessedUserID = 0;
            loan.LoanGUID = Guid.NewGuid();
            loan.AuditMonthYear = Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
            loan.AssignedUserID = 0;
            loan.EnCompassLoanGUID = new Guid(_eLoanGUID);

            FormLoanSearch(loan, loanSearch, _enLoanSearchFields, _enFieldResponse);
        }


        private void FormLoanSearch(Loan loan, LoanSearch loanSearch, List<IntellaAndEncompassFetchFields> _enLoanSearchFields, List<EFieldResponse> _enFieldResponse)
        {
            IntellaAndEncompassFetchFields _eBorrowFirstName = _enLoanSearchFields.Where(x => x.EncompassFieldDescription.Contains("First") && x.FieldType.Contains(LOSFieldType.BORROWER)).FirstOrDefault();
            IntellaAndEncompassFetchFields _eBorrowLastName = _enLoanSearchFields.Where(x => x.EncompassFieldDescription.Contains("Last") && x.FieldType.Contains(LOSFieldType.BORROWER)).FirstOrDefault();

            loanSearch.LoanNumber = loan.LoanNumber; //need to fill
            loanSearch.LoanTypeID = loan.LoanTypeID;
            loanSearch.ReceivedDate = loan.CreatedOn;
            loanSearch.Status = loan.Status;
            //loanSearch.BorrowerName = string.Empty;
            loanSearch.CreatedOn = DateTime.Now;
            loanSearch.ModifiedOn = DateTime.Now;
            loanSearch.CustomerID = loan.CustomerID;
            //loanSearch.AuditDueDate = null;
            //loanSearch.PropertyAddress = string.Empty;

            if (_eBorrowFirstName != null && _eBorrowLastName != null)
                loanSearch.BorrowerName = $"{_enFieldResponse.Where(e => e.FieldId == _eBorrowFirstName.EncompassFieldID).FirstOrDefault().Value.Trim()} {_enFieldResponse.Where(e => e.FieldId == _eBorrowLastName.EncompassFieldID).FirstOrDefault().Value.Trim()}";  //$"{_enLoan.BorrowerPairs.Current.Borrower.LastName}, {_enLoan.BorrowerPairs.Current.Borrower.FirstName}";


            foreach (string mappingCol in LOSLookupMapping.GetDestinationSearchFields)
            {
                List<IntellaAndEncompassFetchFields> _enSFields = _enLoanSearchFields.Where(f => f.IntellaMappingColumn == mappingCol.Trim()).OrderBy(o => o.EncompassFieldID).ToList();

                string[] fieldType = mappingCol.Split('.');

                if (_enSFields.Count == 1)
                {
                    string _fieldValue = _enFieldResponse.Where(e => e.FieldId == _enSFields[0].EncompassFieldID).FirstOrDefault().Value;

                    switch (fieldType[1].ToUpper())
                    {
                        case "INVESTORLOANNUMBER":
                            loanSearch.InvestorLoanNumber = _fieldValue;
                            break;
                        case "AUDITDUEDATE":
                            DateTime _date = DateTime.Now;

                            if (!string.IsNullOrEmpty(_fieldValue))
                            {
                                DateTime.TryParse(_fieldValue, out _date);
                                loanSearch.AuditDueDate = _date;
                            }
                            else
                                loanSearch.AuditDueDate = null;


                            break;
                        default:
                            break;
                    }
                }
                else if (_enSFields.Count > 1)
                {
                    List<string> _fieldValues = new List<string>();
                    foreach (IntellaAndEncompassFetchFields _enSField in _enSFields)
                    {
                        _fieldValues.Add(_enFieldResponse.Where(e => e.FieldId == _enSField.EncompassFieldID).FirstOrDefault().Value);
                    }

                    switch (fieldType[1].ToUpper())
                    {
                        case "PROPERTYADDRESS":
                            loanSearch.PropertyAddress = string.Join(", ", _fieldValues);
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }
    }

    class LoanDownload
    {
        // public Guid? 
        public Int64 LoanID { get; set; }
        public Guid? EnCompassLoanGUID { get; set; }
        public Int64 DownloadID { get; set; }
        public bool RetryFlag { get; set; }
    }
}


