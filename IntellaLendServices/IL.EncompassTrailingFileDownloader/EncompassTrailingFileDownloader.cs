using EncompassAPIHelper;
using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Newtonsoft;
using Newtonsoft.Json;

namespace IL.EncompassTrailingFileDownloader
{
    public class EncompassTrailingFileDownloader : IMTSServiceBase
    {
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;
        private static string CurrentReviewTypeName = string.Empty;
        private static string ProcessedParkingSpot = string.Empty;
        private static string UploadContainer = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value; //http://mts100:8099/
            ProcessedParkingSpot = Params.Find(f => f.Key == "ProcessedParkingSpot").Value;
            UploadContainer = Params.Find(f => f.Key == "UploadContainer").Value;
        }

        public bool DoTask()
        {
            try
            {
                var TenantList = EncompassTrailingFileDownloaderDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EncompassTrailingFileDownloaderDataAccess dataAccess = new EncompassTrailingFileDownloaderDataAccess(tenant.TenantSchema);
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

        private void DownloadFromEncompass(EncompassWrapperAPI _api, EncompassTrailingFileDownloaderDataAccess dataAccess)
        {
            //List<LoanDownload> _retryLoanList = dataAccess.GetRetryTrailingLoans();
            List<LoanDownload> _dbLoanList;
            //   List<LoanDownload> _loanList = _retryLoanList.Union(_dbLoanList).ToList();
            List<EWebhookEvents> _loanList = dataAccess.GetEncompassWebHookLoans();
            dynamic _response = string.Empty;

            foreach (var _eLoan in _loanList)
            {
                dataAccess.UpdateStatusEwebHookEvents(_eLoan.ID, EWebHookStatusConstant.EWEB_HOOK_PROCESSING);
                _response = JsonConvert.DeserializeObject(_eLoan.Response);


                _dbLoanList = dataAccess.GetDBLoans(_response.LoanQuid);
                foreach (var _loan in _dbLoanList)
                {


                    IntellaAndEncompassFetchFields _importField = dataAccess.GetEncompassImportFields();
                    if (_importField != null)
                    {
                        string[] _enFieldValue = _importField.EncompassFieldValue.Split(',');
                        //Getting milestone and custom field values from Encompass
                        string[] _enFieldLookup = new List<string>() { _importField.EFetchFieldID }.ToArray();
                        Logger.WriteTraceLog($"{_response.loanGuid.ToString()}, {string.Join(",", _enFieldLookup)}");
                        List<EFieldResponse> _enFieldResponse = _api.GetPredefinedFieldValues(_response.loanGuid.ToString(), _enFieldLookup);
                        EFieldResponse field = _enFieldResponse.Where(e => e.FieldId == _importField.EFetchFieldID).FirstOrDefault();
                        string lastCompletedMilestone = field != null ? field.Value : "";
                        Logger.WriteTraceLog($"{lastCompletedMilestone}");
                        if (_enFieldValue.Any(x => x.Trim().ToLower().Equals(lastCompletedMilestone.Trim().ToLower()))) //string.equals use
                        {
                            //if already exist EncompassStatusConstant.DOWNLOAD_PENDING
                            ELoanAttachmentDownload _elAttachment = null;
                            List<byte[]> _pdfBytes = new List<byte[]>();
                            PDFMerger merger = null;
                            string lockFileName = string.Empty;
                            try
                            {
                                List<EContainer> _eLoanDocuments = _api.GetAllLoanDocuments(_response.loanGuid.ToString());
                                EContainer loanDocument = _eLoanDocuments.Where(x => x.Title.Trim().ToLower() == UploadContainer.Trim().ToLower()).FirstOrDefault();
                                List<EDocumentAttachment> _eDocumentAttachment = loanDocument.Attachments;
                                List<EAttachment> _eAttachment;
                                foreach (EDocumentAttachment _eDocAttachment in _eDocumentAttachment)
                                {
                                    _eAttachment = _api.GetAttachments(_response.loanGuid, _eDocAttachment.EntityId);
                                    _elAttachment = dataAccess.AddOrUpdateEDownloadStatus(_loan.LoanID, _loan.DownloadID, EncompassStatusConstant.DOWNLOAD_PENDING, _loan.RetryFlag); // user Retry and DownloadID to check

                                    List<EDownloadStaging> _steps = dataAccess.SetDownloadSteps(_eAttachment, _elAttachment.ID, _elAttachment.ELoanGUID.ToString());

                                    //Download the attachments...                          
                                    foreach (EDownloadStaging item in _steps)
                                    {
                                        byte[] _fileArrary = _api.DownloadAttachment(_loan.EnCompassLoanGUID.GetValueOrDefault().ToString(), item.AttachmentGUID, item.EAttachmentName);
                                        _pdfBytes.Add(_fileArrary);
                                        dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Completed);
                                    }

                                    //AuditLoanMissingDoc auditLoanMissingDoc
                                    int auditLoanMissingDocCount = dataAccess.GetAuditLoanMissingDocCount(_loan.LoanID);

                                    string exactPath = Path.Combine(IntellaLendLoanUploadPath, dataAccess.TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                                    string newFileName = dataAccess.TenantSchema + "_" + _loan.LoanID.ToString() + "_" + (auditLoanMissingDocCount + 1) + ".lck";

                                    try
                                    {
                                        if (!Directory.Exists(exactPath))
                                            Directory.CreateDirectory(exactPath);

                                        lockFileName = Path.Combine(exactPath, newFileName);

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
                                        throw new Exception("Error while creating Trailing Document Loan PDF", ex);
                                    }

                                    string OrgFileName = Path.ChangeExtension(lockFileName, ".pdf");

                                    List<string> attachmentGUIDs = _eAttachment.Select(a => a.AttachmentID).ToList();

                                    //code to move MTSProcessed parking spot
                                    List<EContainer> _eProcessedLoans = _api.GetAllLoanDocuments(_loan.EnCompassLoanGUID.ToString());
                                    //need to check mtsprocess
                                    EContainer eContainer = _eProcessedLoans.Where(e => e.Title == ProcessedParkingSpot).FirstOrDefault();
                                    if (eContainer != null)
                                    {
                                        _api.AssignDocumentAttachments(_loan.EnCompassLoanGUID.ToString(), eContainer.DocumentId, attachmentGUIDs, ProcessedParkingSpot);

                                    }
                                    else
                                    {
                                        AddContainerResponse res = _api.AddDocument(_loan.EnCompassLoanGUID.ToString(), ProcessedParkingSpot);
                                        _api.AssignDocumentAttachments(_loan.EnCompassLoanGUID.ToString(), res.DocumentID, attachmentGUIDs, ProcessedParkingSpot);
                                    }

                                    //insert into auditloanmissingdoc
                                    Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                                    missingDocAuditInfo["LOANID"] = _loan.LoanID;
                                    missingDocAuditInfo["DOCID"] = 0;
                                    missingDocAuditInfo["USERID"] = 0;
                                    missingDocAuditInfo["FILENAME"] = newFileName;
                                    missingDocAuditInfo["EDOWNLOADSTAGINGID"] = _elAttachment.ID;

                                    dataAccess.MissingDocFileUpload(missingDocAuditInfo);

                                    File.Move(lockFileName, OrgFileName);
                                    dataAccess.UpdateEDownloadStatus(_loan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_SUCCESS);
                                    dataAccess.DeleteStaginDetails(_loan.EnCompassLoanGUID);
                                    dataAccess.DeleteWebHookEvents(_eLoan.ID);


                                }
                            }

                            catch (EncompassWrapperLoanLockException ex)
                            {
                                if (_elAttachment.ID > 0)
                                    dataAccess.UpdateEDownloadStatus(_elAttachment.ID, "Loan Locked");

                                Exception exe = new Exception("Loan Locked", ex);
                                MTSExceptionHandler.HandleException(ref exe);
                            }
                            catch (Exception ex)
                            {
                                if (merger != null)
                                {
                                    merger.SaveDocument();
                                }

                                if (File.Exists(lockFileName))
                                    File.Delete(lockFileName);

                                //need to update some fields
                                dataAccess.UpdateEDownloadStatus(_loan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_FAILED, ex.Message);
                                dataAccess.UpdateStatusEwebHookEvents(_eLoan.ID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                                //Exception exc = new Exception($"Error while Creating PDF with Encompass attachment. Encompass LoanGUID : {_eLoan.EnCompassLoanGUID.ToString()}", ex);
                                MTSExceptionHandler.HandleException(ref ex);
                            }
                        }
                        else if (lastCompletedMilestone != "")
                        {
                            //update downloadedcomplete flag from loans table to 1(Yes)
                            dataAccess.updateLoanCompleteStatus(_loan.LoanID, dataAccess.TenantSchema);
                        }
                    }
                }
            }
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
                    //string _fieldValue = EncompassConnector.GetFieldValueByLoan(_enLoan, _enField.EncompassFieldID);
                }
            }
            List<EFieldResponse> _enFieldResponse = _api.GetPredefinedFieldValues(_eLoanGUID, _enLookupLists.ToArray());
            return _enFieldResponse;

        }



    }

    class LoanDownload
    {
        public Int64 LoanID { get; set; }
        public Guid? EnCompassLoanGUID { get; set; }
        public Int64 DownloadID { get; set; }
        public bool RetryFlag { get; set; }
    }
}
