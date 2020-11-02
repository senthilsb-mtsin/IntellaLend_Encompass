using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System.Collections.Generic;
using IntellaLend.Model;
using IntellaLend.Constance;
using EncompassRequestBody.EResponseModel;
using MTSEntBlocks.UtilsBlock;
using EncompassRequestBody.WrapperReponseModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using EncompassAPIHelper;

namespace IL.EncompassFileDownloader
{
    public class EncompassFileDownloader : IMTSServiceBase
    {


        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;
        private static string EncompassFetchField = string.Empty;
        private static string ProcessedParkingSpot = string.Empty;
        private const Int32 PDF_CREATED = 0;
        private static string GhostScriptPath = string.Empty;
        private static string InvestorLoanTypes = string.Empty;
        private static string CurrentReviewTypeName = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value; //http://mts100:8099/
            //EncompassFetchField = Params.Find(f => f.Key == "EncompassFetchField").Value;
            ProcessedParkingSpot = Params.Find(f => f.Key == "ProcessedParkingSpot").Value;

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
                BaseExceptionHandler.HandleException(ref ex);
            }

            return true;
        }


        private void DownloadFromEncompass(EncompassWrapperAPI _api, EncompassFileDownloaderDataAccess dataAccess)
        {
            List<IntellaAndEncompassFetchFields> _enLookupFields = dataAccess.GetEncompassLookUpFields();

            List<KeyValuePair<string, string>> _eFields = new List<KeyValuePair<string, string>>();
            List<IntellaAndEncompassFetchFields> _enImportFields = dataAccess.GetEncompassImportFields();
            List<IntellaAndEncompassFetchFields> _singleFields = _enImportFields.Where(e => e.IsSingleValue).ToList();
            List<IntellaAndEncompassFetchFields> _multipleFields = _enImportFields.Where(e => !e.IsSingleValue).ToList();

            List<Dictionary<string, string>> _loopFields = _singleFields;

            foreach (IntellaAndEncompassFetchFields _importField in _singleFields)
            {
                _eFields.Add(new KeyValuePair<string, string>(_importField.EncompassFieldID, _importField.EncompassFieldValue));
                //EncompassFetchField = _importField.EFetchFieldID;
            }
            List<string[]> multpleFid = new List<string[]>();

            foreach (IntellaAndEncompassFetchFields _importField in _multipleFields)
            {


                string[] _enFieldValue = _importField.EncompassFieldValue.Split(',');
                for (int i = 0; i < _enFieldValue.Length; i++)
                {
                    _eFields.Add(new KeyValuePair<string, string>(_importField.EncompassFieldID, _enFieldValue[i].Trim()));
                    
                }
            }
            
            foreach (IntellaAndEncompassFetchFields _importField in _multipleFields)
            {
                string[] _enFieldValue = _importField.EncompassFieldValue.Split(',');
                string[] _iFieldValue = _importField.IntellaMappingValue.Split(',');
                LogMessage(string.Join(",", _enFieldValue));
                LogMessage(string.Join(",", _iFieldValue));
                for (int i = 0; i < _enFieldValue.Length; i++)
                {
                    //_eFields.Add(new KeyValuePair<string, string>(_importField.EncompassFieldID, _enFieldValue[i].Trim()));
                    Int64 ReviewTypeID = dataAccess.GetReviewTypeID(_iFieldValue[i].Trim());
                    CurrentReviewTypeName = _iFieldValue[i].Trim();

                    if (ReviewTypeID != 0)
                    {
                        List<string> _lsLoans = _api.GetLoans(_eFields);
                        LogMessage(string.Join(",", _lsLoans));
                        foreach (string _eLoanGUID in _lsLoans)
                        {
                            string LoanNumber = GetLoanNoFromEncompass(_api, _eLoanGUID);
                            Loan loan = new Loan();
                            LoanSearch loanSearch = new LoanSearch();
                            LoanLOSFields _loanLOSField = new LoanLOSFields();
                            List<EFieldResponse> _enFieldResponse = GetAllFieldValuesFromEncompass(_api, _eLoanGUID, _enLookupFields);
                            loan.ReviewTypeID = ReviewTypeID;
                            SetLoanInfo(loan, _eLoanGUID, loanSearch, _enLookupFields, _enFieldResponse, dataAccess, LoanNumber, _loanLOSField);
                            
                            Int64 downloadID = dataAccess.InsertLoan(loan, loanSearch, _loanLOSField);
                            PDFMerger merger = null;
                            List<byte[]> _pdfBytes = new List<byte[]>();
                            string lockFileName = string.Empty;
                            try
                            {
                                List<EAttachment> _eAttachments = _api.GetUnassignedAttachments(_eLoanGUID);

                                List<EDownloadStaging> _steps = dataAccess.SetDownloadSteps(_eAttachments, downloadID, _eLoanGUID);
                                List<EDownloadStaging> _movedFiles = new List<EDownloadStaging>();
                                foreach (EDownloadStaging item in _steps.Where(s => s.Step == EncompassDownloadStepConstant.LoanAttachment).ToArray())
                                {
                                    try
                                    {
                                        dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Processing);
                                        byte[] _fileArrary = _api.DownloadAttachment(_eLoanGUID, item.AttachmentGUID);
                                        _pdfBytes.Add(_fileArrary);
                                        dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Completed);
                                        _movedFiles.Add(item);
                                    }
                                    catch (Exception ex)
                                    {
                                        dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Error, ex.Message);
                                        throw ex;
                                    }                                  
                                }
                                
                                string exactPath = Path.Combine(IntellaLendLoanUploadPath, dataAccess.TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                                string newFileName = dataAccess.TenantSchema + "_" + loan.LoanID.ToString() + ".lck";

                                lockFileName = Path.Combine(exactPath, newFileName);

                                string OrgFileName = Path.ChangeExtension(lockFileName, ".pdf");

                                if (_movedFiles.Count > 0)
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

                                    List<string> attachmentGUIDs = _movedFiles.Select(a => a.AttachmentGUID).ToList();

                                    List<EContainer> _eLoanDocuments = _api.GetAllLoanDocuments(_eLoanGUID);

                                    EContainer eContainer = _eLoanDocuments.Where(e => e.Title == ProcessedParkingSpot).FirstOrDefault();

                                    if (eContainer != null)
                                    {
                                        _api.AssignDocumentAttachments(_eLoanGUID, eContainer.DocumentId, attachmentGUIDs);
                                    }
                                    else
                                    {
                                        AddContainerResponse res = _api.AddDocument(_eLoanGUID, ProcessedParkingSpot);
                                        _api.AssignDocumentAttachments(_eLoanGUID, res.DocumentID, attachmentGUIDs);
                                    }
                                }
                                else {
                                    throw new Exception("Attachment(s) not found");
                                }

                                EDownloadStaging _fieldUpdate = _steps.Where(s => s.Step == EncompassDownloadStepConstant.UpdateField).FirstOrDefault();

                                if (_fieldUpdate != null)
                                {
                                    try
                                    {
                                        _api.UploadProcessFlag(_eLoanGUID, EncompassFetchField, "Yes");
                                        dataAccess.UpdateDownloadStaging(_fieldUpdate.ID, EncompassDownloadStepStatusConstant.Completed);
                                    }
                                    catch (Exception ex)
                                    {
                                        dataAccess.UpdateDownloadStaging(_fieldUpdate.ID, EncompassDownloadStepStatusConstant.Error, ex.Message);
                                        throw ex;
                                    }

                                    File.Move(lockFileName, OrgFileName);
                                    dataAccess.UpdateEDownloadStatus(loan.LoanID, EncompassStatusConstant.DOWNLOAD_SUCCESS);
                                }
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
                                    dataAccess.UpdateEDownloadStatus(loan.LoanID, EncompassStatusConstant.DOWNLOAD_FAILED, ex.Message);

                                BaseExceptionHandler.HandleException(ref ex);
                            }
                        }
                    }
                    else
                    {
                        Logger.Write($"Review Type Unavailable : {_iFieldValue[i].Trim()}");
                    }
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
        private List<EFieldResponse> GetAllFieldValuesFromEncompass(EncompassWrapperAPI _api, string _eLoanGUID, List<IntellaAndEncompassFetchFields> _enLookupFields)
        {
            //assign field values
            List<string> _enLookupLists = new List<string>();
            foreach (string Field in LOSLookupMapping.GetDestinationFields)
            {
                IntellaAndEncompassFetchFields _enField = _enLookupFields.Where(f => f.IntellaMappingColumn == Field.Trim()).FirstOrDefault();
                string[] fieldType = Field.Split('.');
                if (_enField != null)
                {
                    _enLookupLists.Add(_enField.EncompassFieldID);
                }
            }
            List<EFieldResponse> _enFieldResponse = _api.GetPredefinedFieldValues(_eLoanGUID, _enLookupLists.ToArray());
            return _enFieldResponse;

        }

        private void SetLoanInfo(Loan loan, string _eLoanGUID, LoanSearch loanSearch, List<IntellaAndEncompassFetchFields> _enLookupFields, List<EFieldResponse> _enFieldResponse, EncompassFileDownloaderDataAccess dataAccess, string LoanNumber, LoanLOSFields _loanLOSField)
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
                                            throw new Exception($"'{CurrentReviewTypeName}' Service type name not assigned to '{_fieldValue.Trim()}' Investor in IntellaLend."); //CustomerID : {CustomerID} & ReviewTypeID : {loan.ReviewTypeID}, Encompass Loan No : {LoanNumber}");
                                    }
                                    else
                                        throw new Exception($"Encompass Investor name '{_fieldValue}' not found in IntellaLend");
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
                                            throw new Exception($"'{LoanTypeName}' Loantype name not assigned to Service type '{CurrentReviewTypeName}' for the Investor '{CustomerName}' in IntellaLend."); // CustomerID : {loan.CustomerID} & ReviewTypeID : {loan.ReviewTypeID} & LoanTypeID : {LoanTypeID}, Encompass Loan No : {LoanNumber}");
                                    }
                                    else
                                        throw new Exception($"'{LoanTypeName}' Loantype name not found in IntellaLend."); // Encompass LoanType Name : {_fieldValue}, Encompass Loan No : {LoanNumber}");
                                }
                                else
                                    throw new Exception($"Loantype name is empty for the Loan '{LoanNumber}' in Encompass");
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

            FormLoanSearch(loan, loanSearch);
        }


        private void FormLoanSearch(Loan loan, LoanSearch loanSearch)
        {
            loanSearch.LoanNumber = loan.LoanNumber; //need to fill
            loanSearch.LoanTypeID = loan.LoanTypeID;
            loanSearch.ReceivedDate = loan.CreatedOn;
            loanSearch.Status = loan.Status;
            loanSearch.BorrowerName = string.Empty;
            loanSearch.CreatedOn = DateTime.Now;
            loanSearch.ModifiedOn = DateTime.Now;
            loanSearch.CustomerID = loan.CustomerID;
            loanSearch.AuditDueDate = null;
            loanSearch.PropertyAddress = string.Empty;
        }


        private void LogMessage(string _msg)
        {
            bool logMsg = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["EncompassFileDownloaderDebug"], out logMsg);
            if (logMsg)
                Logger.Write(_msg);
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


