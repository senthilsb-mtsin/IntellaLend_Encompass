using IntellaLend.Constance;
using IntellaLend.Model;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace IL.EncompassExport
{
    public class EncompassExport : IMTSServiceBase
    {
        #region Service Start DoTask

        private static string IntellaLendLoanUploadPath = string.Empty;
        private const Int32 PDF_CREATED = 0;
        private static string GhostScriptPath = string.Empty;
        private static string PostCloseServiceTypeName = string.Empty;
        private static string PreCloseServiceTypeName = string.Empty;
        private static string UnMappedParkingSpotNameInIntellaLend = string.Empty;
        private static string InvestorLoanTypes = string.Empty;
        private static string EncompassSDKAPIURL = string.Empty;
        private static string CurrentReviewTypeName = string.Empty;
        private List<List<Dictionary<string, string>>> _queryCombinations = new List<List<Dictionary<string, string>>>();

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            GhostScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GhostScript", "gswin64c.exe");
            PostCloseServiceTypeName = ConfigurationManager.AppSettings["PostClosing"].ToString();
            PreCloseServiceTypeName = ConfigurationManager.AppSettings["PreClosing"].ToString();
            UnMappedParkingSpotNameInIntellaLend = Params.Find(f => f.Key == "UnMappedParkingSpotNameInIntellaLend").Value;
            InvestorLoanTypes = Params.Find(f => f.Key == "InvestorLoanTypes").Value;
            EncompassSDKAPIURL = Params.Find(f => f.Key == "EncompassSDKAPIURL").Value;
        }

        public bool DoTask()
        {
            try
            {
                var TenantList = EncompassExportDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EncompassExportDataAccess dataAccess = new EncompassExportDataAccess(tenant.TenantSchema);
                    EncompassWrapperAPI _api = new EncompassWrapperAPI(EncompassSDKAPIURL, tenant.TenantSchema);
                    DownloadFromEncompass(dataAccess, tenant.TenantSchema, _api);
                    _api.Dispose();
                }

                return true;
            }
            catch (Exception ex)
            {
                BaseExceptionHandler.HandleException(ref ex);
            }

            return false;
        }

        #endregion

        #region Helper Methods

        public void GetCombinations(List<Dictionary<string, List<string>>> fields, List<Dictionary<string, string>> output)
        {
            LogException.Log($"{fields.Count } == {output.Count}");
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

        public void DownloadFromEncompass(EncompassExportDataAccess dataAccess, string TenantSchema, EncompassWrapperAPI _api)
        {

            LogException.Log("In DownloadFromEncompass ");
            List<IntellaAndEncompassFetchFields> _enLookupFields = EncompassExportDataAccess.GetEncompassLookUpFields();
            LogException.Log($"_enLookupFields {_enLookupFields.Count}");
            List<IntellaAndEncompassFetchFields> _enImportFields = EncompassExportDataAccess.GetEncompassImportFields();
            LogException.Log($"_enImportFields {_enImportFields.Count}");
            List<IntellaAndEncompassFetchFields> _enLoanSearchFields = EncompassExportDataAccess.GetEncompassSearchFields();
            LogException.Log($"_enImportFields {_enLoanSearchFields.Count}");
            List<string> _enExceptionLoans = dataAccess.GetEncompassExceptionLoans();
            LogException.Log($"_enExceptionLoans {_enExceptionLoans.Count}");
            List<Dictionary<string, List<string>>> _eQueryFields = new List<Dictionary<string, List<string>>>();
            LogException.Log(_enImportFields.Count.ToString());
            foreach (IntellaAndEncompassFetchFields item in _enImportFields)
            {
                LogException.Log(item.EncompassFieldValue);
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                dic.Add(item.EncompassFieldID, item.EncompassFieldValue.Split(',').ToList());
                _eQueryFields.Add(dic);
            }
            _queryCombinations = new List<List<Dictionary<string, string>>>();
            GetCombinations(_eQueryFields, new List<Dictionary<string, string>>());
            LogException.Log(JsonConvert.SerializeObject(_queryCombinations));
            List<string> _lsLoans = new List<string>();
            //List<EllieMae.Encompass.BusinessObjects.Loans.Loan> _iLoans = new List<EllieMae.Encompass.BusinessObjects.Loans.Loan>();
            try
            {
                foreach (List<Dictionary<string, string>> _importField in _queryCombinations)
                {
                    _lsLoans = _api.GetPipeLineLoans(_importField);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot able to get loans from Encompass", ex);
            }

            IntellaAndEncompassFetchFields _serviceType = _enImportFields.Where(x => x.FieldType.Contains(LOSFieldType.SERVICETYPE)).FirstOrDefault();

            List<string> _lsDistLoans = new List<string>();

            foreach (var item in _lsLoans)
            {
                string upperLoanGUID = item.ToUpper();

                if (!(_enExceptionLoans.Any(e => e == upperLoanGUID)))
                {
                    _lsDistLoans.Add(upperLoanGUID);
                }
            }

            foreach (string _enLoanGUID in _lsDistLoans)
            {
                Loan loan = new Loan();
                LoanSearch loanSearch = new LoanSearch();
                LoanLOSFields _loanLOSField = new LoanLOSFields();
                string LoanNumber = string.Empty;
                string OrgFileName = string.Empty;
                string lockFileName = string.Empty;
                try
                {
                    //_enLookupFields.Add(_serviceType);
                    List<string> _eLookUpFields = _enLookupFields.Select(x => x.EncompassFieldID).ToList();
                    _eLookUpFields.Add(EncompassFieldConstant.LOAN_NUMBER);
                    _eLookUpFields.Add(_serviceType.EFetchFieldID);

                    _eLookUpFields = _eLookUpFields.Union(_enLoanSearchFields.Select(x => x.EncompassFieldID).ToList()).Distinct().ToList();

                    Dictionary<string, string> _eLoanFields = _api.GetPredefinedFieldValues(_enLoanGUID, _eLookUpFields);
                    LogException.Log("_eLoanFields : " + JsonConvert.SerializeObject(_eLoanFields));
                    string eReviewTypeName = _eLoanFields[_serviceType.EFetchFieldID].Trim();
                   
                    List<string> _eServiceTypes = _serviceType.EncompassFieldValue.Split(',').ToList();
                    LogException.Log((_eServiceTypes.IndexOf(eReviewTypeName)).ToString());

                    List<string> _iServiceTypes = _serviceType.IntellaMappingValue.Split(',').ToList();
                    LogException.Log(JsonConvert.SerializeObject(_iServiceTypes));

                    if (_eServiceTypes.IndexOf(eReviewTypeName) >= 0)
                    {
                        string CurrentReviewTypeName = _iServiceTypes[_eServiceTypes.IndexOf(eReviewTypeName)].Trim();

                        Int64 ReviewTypeID = dataAccess.GetReviewTypeID(CurrentReviewTypeName);

                        if (ReviewTypeID != 0)
                        {
                            LoanNumber = _eLoanFields[EncompassFieldConstant.LOAN_NUMBER].Trim();
                            loan.ReviewTypeID = ReviewTypeID;
                            try
                            {
                                SetLoanInfo(loan, _enLoanGUID, dataAccess, _loanLOSField, loanSearch, _enLookupFields, _enLoanSearchFields, _eLoanFields, LoanNumber);

                                dataAccess.InsertLoan(loan, _loanLOSField, loanSearch);

                                bool importSuccessfully = ImportLoanFromEncompass(_enLoanGUID, TenantSchema, loan.LoanID, LoanNumber, dataAccess, _api, ref OrgFileName, ref lockFileName);

                                Dictionary<string, object> _updateField = new Dictionary<string, object>();
                                _updateField.Add(LOSLookupMapping.PROCESSEDFIELDID, "X");

                                if (!importSuccessfully)
                                {
                                    dataAccess.RemoveInsertLoan(loan.LoanID);
                                    Exception ex = new Exception("Error while downloading PDF from Encompass");
                                    dataAccess.InsertEncompassExceptionLoan(LoanNumber, _enLoanGUID, ex);
                                }
                                else
                                {
                                    LogException.Log("_updateField : " + JsonConvert.SerializeObject(_updateField));
                                    _api.UpdateLoanCustomField(_enLoanGUID, _updateField);

                                    if (File.Exists(lockFileName))
                                        File.Move(lockFileName, OrgFileName);

                                    dataAccess.UpdateEncompassExceptionLoan(_enLoanGUID);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (loan.LoanID != 0)
                                {
                                    dataAccess.RemoveInsertLoan(loan.LoanID);
                                }

                                dataAccess.InsertEncompassExceptionLoan(LoanNumber, _enLoanGUID, ex);

                                if (File.Exists(OrgFileName))
                                    File.Delete(OrgFileName);

                                if (File.Exists(lockFileName))
                                    File.Delete(lockFileName);

                                BaseExceptionHandler.HandleException(ref ex);
                            }

                        }
                        else
                        {
                            Exception ex = new Exception($"Service Type not found in the Encompass for {_serviceType.EncompassFieldDescription} : {CurrentReviewTypeName}");
                            CurrentReviewTypeName = string.Empty;
                            BaseExceptionHandler.HandleException(ref ex);
                        }
                    }

                    CurrentReviewTypeName = string.Empty;
                }
                catch (Exception ex)
                {
                    if (loan.LoanID != 0)
                    {
                        dataAccess.RemoveInsertLoan(loan.LoanID);
                    }

                    dataAccess.InsertEncompassExceptionLoan(LoanNumber, _enLoanGUID, ex);

                    if (File.Exists(OrgFileName))
                        File.Delete(OrgFileName);

                    if (File.Exists(lockFileName))
                        File.Delete(lockFileName);

                    BaseExceptionHandler.HandleException(ref ex);
                }
                
            }
        }
        
        private bool ImportLoanFromEncompass(string _enLoanGUID, string TenantSchema, Int64 LoanID, string enLoanNumber, EncompassExportDataAccess dataAccess,EncompassWrapperAPI _api, ref string OrgFileName, ref string lockFileName)
        {
            //Exception exTemp = new Exception("Inside ImportLoanFromEncompass");
            //BaseExceptionHandler.HandleException(ref exTemp);

            Dictionary<string, string> _configDocs = EncompassExportDataAccess.GetEncompassParkingSpot(EncompassExportDataAccess.IsConfiguredSpots, UnMappedParkingSpotNameInIntellaLend);

            List<EnDocuments> _enDocAttachments = _api.GetAllLoanAttachments(_enLoanGUID, _configDocs);

            List<EnDocumentType> _enDocTypes = new List<EnDocumentType>();

            List<byte[]> _pdfBytes = new List<byte[]>();

            foreach (EnDocuments item in _enDocAttachments)
            {
                try
                {
                    EnDocumentType _enDocType = new EnDocumentType();

                    _enDocType.DocumentTypeName = item.DocumentTypeName;

                    byte[] itemArray = Convert.FromBase64String(item.Base64Attachment);

                    _pdfBytes.Add(itemArray);

                    _enDocTypes.Add(_enDocType);
                }
                catch (Exception ex)
                {
                    Exception exc = new Exception($"Error while downloading attachment from Encompass. Encompass Loan Number : {enLoanNumber}, Attachment Title : {item.DocumentTypeName}", ex);
                    BaseExceptionHandler.HandleException(ref exc);
                    return false;
                }
                
            }

            //string lockFileName = string.Empty;
            //  string OrgFileName = string.Empty;
            string gsFileName = string.Empty;
            PDFMerger merger = null;
            try
            {
                if (_pdfBytes.Count == 0)
                    throw new Exception($"Attachments Not Found. Encompass LoanNumber : {enLoanNumber}");

                string exactPath = Path.Combine(IntellaLendLoanUploadPath, TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                if (!Directory.Exists(exactPath))
                    Directory.CreateDirectory(exactPath);

                string newFileName = TenantSchema + "_" + LoanID.ToString() + ".lck";

                lockFileName = Path.Combine(exactPath, newFileName);

                merger = new PDFMerger(lockFileName);
                merger.OpenDocument();
                int pgNo = 0;
                for (int i = 0; i < _pdfBytes.Count; i++)
                {
                    List<Int32> _pgNos = new List<int>();
                    merger.AppendPDF(_pdfBytes[i], ref pgNo, ref _pgNos);
                    _enDocTypes[i].Pages = _pgNos;
                }
                merger.SaveDocument();

                //Update Encompass Doc Page Number
                dataAccess.SetEncompassDocPages(LoanID, JsonConvert.SerializeObject(_enDocTypes));

                OrgFileName = Path.ChangeExtension(lockFileName, ".pdf");

                Int32 tiffGenerated = -1; // Prakash : PDF to PDF Conversion Issue(InvalidPDFException) //GhostScriptConvertion(lockFileName, gsFileName);

                if (tiffGenerated == PDF_CREATED)
                {
                    if (File.Exists(lockFileName))
                        File.Delete(lockFileName);

                    File.Move(gsFileName, OrgFileName);
                }
                else
                {
                    if (File.Exists(gsFileName))
                        File.Delete(gsFileName);

                    //Prakash : Since Processing Queue service picks after this process. When exception occurs also this package will go into Ephesoft.
                    //File.Move(lockFileName, OrgFileName);
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
                if (File.Exists(OrgFileName))
                    File.Delete(OrgFileName);

                Exception exc = new Exception($"Error while Creating PDF with Encompass attachment. Encompass LoanNumber : {enLoanNumber}", ex);
                BaseExceptionHandler.HandleException(ref exc);
                return false;
            }

            return true;
        }

        private void SetLoanInfo(Loan loan, string _enLoanGUID, EncompassExportDataAccess dataAccess, LoanLOSFields _loanLOSField, LoanSearch loanSearch,List<IntellaAndEncompassFetchFields> _enLookupFields, List<IntellaAndEncompassFetchFields> _enLoanSearchFields, Dictionary<string, string> _eLookupFieldValues, string LoanNumber)
        {
            foreach (string Field in LOSLookupMapping.GetDestinationFields)
            {
                IntellaAndEncompassFetchFields _enField = _enLookupFields.Where(f => f.IntellaMappingColumn == Field.Trim()).FirstOrDefault();

                string[] fieldType = Field.Split('.');

                if (_enField != null)
                {
                    string _fieldValue = _eLookupFieldValues[_enField.EncompassFieldID];

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

                                    //Prakash : As per regular stand up call with RB on 24/05/2019
                                    //Mail Sub : Fwd: Republic Bank Business Test Case Scenarios v1.0 Loan 011849707 - Smith PHFA VA.xlsx
                                    string[] lsInvestorLoanTypes = InvestorLoanTypes.Split(',');

                                    string LoanTypeName = string.Empty;

                                    if (lsInvestorLoanTypes.Any(l => l == _fieldValue))
                                    {
                                        if (loan.CustomerID != 0)
                                        {
                                            string CustomerName = dataAccess.GetCustomerByID(loan.CustomerID);
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
                                            throw new Exception($"'{LoanTypeName}' Loantype name not assigned to Service type '{CurrentReviewTypeName}' for the Investor '{CutomerName}' in IntellaLend."); // CustomerID : {loan.CustomerID} & ReviewTypeID : {loan.ReviewTypeID} & LoanTypeID : {LoanTypeID}, Encompass Loan No : {LoanNumber}");
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
            loan.Status = StatusConstant.READY_FOR_IDC;
            loan.LoanNumber = LoanNumber;
            loan.SubStatus = 0;
            loan.UploadedUserID = 1;
            loan.LoggedUserID = 0;
            loan.FileName = string.Empty;
            //loan.FromBox = false;
            loan.UploadType = UploadConstant.ENCOMPASS;
            loan.CreatedOn = DateTime.Now;
            loan.ModifiedOn = DateTime.Now;
            loan.LastAccessedUserID = 0;
            loan.LoanGUID = Guid.NewGuid();
            loan.AuditMonthYear = Convert.ToDateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(IntellaLend.Constance.DateConstance.AuditDateFormat));
            loan.Priority = dataAccess.GetReviewTypePriority(loan.ReviewTypeID);
            loan.EnCompassLoanGUID = new Guid(_enLoanGUID);

            FormLoanSearch(loan, _enLoanGUID, loanSearch, _enLoanSearchFields, _eLookupFieldValues);

            string reviewTypeName = dataAccess.GetReviewTypeName(loan.ReviewTypeID).Trim();

            if (!string.IsNullOrEmpty(reviewTypeName))
            {
                string userName = string.Empty;
                if (reviewTypeName.Equals(PostCloseServiceTypeName))
                    userName = _loanLOSField.PostCloser;
                else if (reviewTypeName.Equals(PreCloseServiceTypeName))
                    userName = _loanLOSField.Underwriter;

                loan.AssignedUserID = dataAccess.GetUserID(userName);
            }
            else
                loan.AssignedUserID = 0;
        }

        private void FormLoanSearch(Loan loan, string _enLoan, LoanSearch loanSearch, List<IntellaAndEncompassFetchFields> _enLoanSearchFields, Dictionary<string, string> _eLookupFieldValues)
        {
            IntellaAndEncompassFetchFields _eBorrowFirstName =  _enLoanSearchFields.Where(x => x.EncompassFieldDescription.Contains("First") && x.FieldType.Contains(LOSFieldType.BORROWER)).FirstOrDefault();
            IntellaAndEncompassFetchFields _eBorrowLastName = _enLoanSearchFields.Where(x => x.EncompassFieldDescription.Contains("Last") && x.FieldType.Contains(LOSFieldType.BORROWER)).FirstOrDefault();
            loanSearch.LoanNumber = loan.LoanNumber;
            loanSearch.LoanTypeID = loan.LoanTypeID;
            loanSearch.ReceivedDate = loan.CreatedOn;
            loanSearch.Status = loan.Status;

            if (_eBorrowFirstName != null && _eBorrowLastName != null)
                loanSearch.BorrowerName = $"{_eLookupFieldValues[_eBorrowFirstName.EncompassFieldID].Trim()} {_eLookupFieldValues[_eBorrowLastName.EncompassFieldID].Trim()}";  //$"{_enLoan.BorrowerPairs.Current.Borrower.LastName}, {_enLoan.BorrowerPairs.Current.Borrower.FirstName}";

            loanSearch.CreatedOn = DateTime.Now;
            loanSearch.ModifiedOn = DateTime.Now;
            loanSearch.CustomerID = loan.CustomerID;

            foreach (string mappingCol in LOSLookupMapping.GetDestinationSearchFields)
            {
                List<IntellaAndEncompassFetchFields> _enSFields = _enLoanSearchFields.Where(f => f.IntellaMappingColumn == mappingCol.Trim()).OrderBy(o => o.EncompassFieldID).ToList();

                string[] fieldType = mappingCol.Split('.');

                if (_enSFields.Count == 1)
                {
                    string _fieldValue = _eLookupFieldValues[_enSFields[0].EncompassFieldID];

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
                        _fieldValues.Add(_eLookupFieldValues[_enSField.EncompassFieldID]);
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
               
        
        #endregion
    }

    public class LogException
    {
        public static void Log(string _msg)
        {
            bool logMsg = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["EncompassFileDownloaderDebug"], out logMsg);
            if (logMsg)
                BaseExceptionHandler.Write(_msg, "ServiceLoader_Logger");
        }
    }
}
