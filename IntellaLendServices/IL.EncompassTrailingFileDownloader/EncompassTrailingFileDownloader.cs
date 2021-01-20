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

namespace IL.EncompassTrailingFileDownloader
{
    public class EncompassTrailingFileDownloader : IMTSServiceBase
    {
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;
        private static string CurrentReviewTypeName = string.Empty;
        private static string ProcessedParkingSpot = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value; //http://mts100:8099/
            ProcessedParkingSpot = Params.Find(f => f.Key == "ProcessedParkingSpot").Value;
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
            List<LoanDownload> _retryLoanList = dataAccess.GetRetryTrailingLoans();
            List<LoanDownload> _dbLoanList = dataAccess.GetDBLoans();
            List<LoanDownload> _loanList = _retryLoanList.Union(_dbLoanList).ToList();

            foreach (var _eLoan in _loanList)
            {
                IntellaAndEncompassFetchFields _importField = dataAccess.GetEncompassImportFields();

                if (_importField != null)
                {
                    string[] _enFieldValue = _importField.EncompassFieldValue.Split(',');
                    //Getting milestone and custom field values from Encompass
                    string[] _enFieldLookup = new List<string>() { _importField.EFetchFieldID }.ToArray();
                    Logger.WriteTraceLog($"{_eLoan.EnCompassLoanGUID.ToString()}, {string.Join(",", _enFieldLookup)}");
                    List<EFieldResponse> _enFieldResponse = _api.GetPredefinedFieldValues(_eLoan.EnCompassLoanGUID.ToString(), _enFieldLookup);
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
                            List<EAttachment> _eAttachments = _api.GetUnassignedAttachments(_eLoan.EnCompassLoanGUID.ToString());
                            if (_eAttachments.Count > 0)
                            {
                                _elAttachment = dataAccess.AddOrUpdateEDownloadStatus(_eLoan.LoanID, _eLoan.DownloadID, EncompassStatusConstant.DOWNLOAD_PENDING, _eLoan.RetryFlag); // user Retry and DownloadID to check

                                List<EDownloadStaging> _steps = dataAccess.SetDownloadSteps(_eAttachments, _elAttachment.ID, _elAttachment.ELoanGUID.ToString());

                                //Download the attachments...                          
                                foreach (EDownloadStaging item in _steps)
                                {
                                    byte[] _fileArrary = _api.DownloadAttachment(_eLoan.EnCompassLoanGUID.GetValueOrDefault().ToString(), item.AttachmentGUID, item.EAttachmentName);
                                    _pdfBytes.Add(_fileArrary);
                                    dataAccess.UpdateDownloadSteps(item.ID, EncompassDownloadStepStatusConstant.Completed);
                                }

                                //AuditLoanMissingDoc auditLoanMissingDoc
                                int auditLoanMissingDocCount = dataAccess.GetAuditLoanMissingDocCount(_eLoan.LoanID);

                                string exactPath = Path.Combine(IntellaLendLoanUploadPath, dataAccess.TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                                string newFileName = dataAccess.TenantSchema + "_" + _eLoan.LoanID.ToString() + "_" + (auditLoanMissingDocCount + 1) + ".lck";

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

                                List<string> attachmentGUIDs = _eAttachments.Select(a => a.AttachmentID).ToList();

                                //code to move MTSProcessed parking spot
                                List<EContainer> _eLoanDocuments = _api.GetAllLoanDocuments(_eLoan.EnCompassLoanGUID.ToString());
                                //need to check mtsprocess
                                EContainer eContainer = _eLoanDocuments.Where(e => e.Title == ProcessedParkingSpot).FirstOrDefault();
                                if (eContainer != null)
                                {
                                    _api.AssignDocumentAttachments(_eLoan.EnCompassLoanGUID.ToString(), eContainer.DocumentId, attachmentGUIDs, ProcessedParkingSpot);

                                }
                                else
                                {
                                    AddContainerResponse res = _api.AddDocument(_eLoan.EnCompassLoanGUID.ToString(), ProcessedParkingSpot);
                                    _api.AssignDocumentAttachments(_eLoan.EnCompassLoanGUID.ToString(), res.DocumentID, attachmentGUIDs, ProcessedParkingSpot);
                                }

                                //insert into auditloanmissingdoc
                                Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                                missingDocAuditInfo["LOANID"] = _eLoan.LoanID;
                                missingDocAuditInfo["DOCID"] = 0;
                                missingDocAuditInfo["USERID"] = 0;
                                missingDocAuditInfo["FILENAME"] = newFileName;
                                missingDocAuditInfo["EDOWNLOADSTAGINGID"] = _elAttachment.ID;

                                dataAccess.MissingDocFileUpload(missingDocAuditInfo);
                                string _donFile = Path.ChangeExtension(OrgFileName, ".don");
                                File.Move(lockFileName, _donFile);
                                dataAccess.UpdateEDownloadStatus(_eLoan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_SUCCESS);
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
                            dataAccess.UpdateEDownloadStatus(_eLoan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_FAILED, ex.Message);

                            //Exception exc = new Exception($"Error while Creating PDF with Encompass attachment. Encompass LoanGUID : {_eLoan.EnCompassLoanGUID.ToString()}", ex);
                            MTSExceptionHandler.HandleException(ref ex);
                        }
                    }
                    else if (lastCompletedMilestone != "")
                    {
                        //update downloadedcomplete flag from loans table to 1(Yes)
                        dataAccess.updateLoanCompleteStatus(_eLoan.LoanID, dataAccess.TenantSchema);
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
