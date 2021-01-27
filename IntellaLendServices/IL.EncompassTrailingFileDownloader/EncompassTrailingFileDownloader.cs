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

namespace IL.EncompassTrailingFileDownloader
{
    public class EncompassTrailingFileDownloader : IMTSServiceBase
    {
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;
        private static string CurrentReviewTypeName = string.Empty;
        private static string ProcessedEFolder = string.Empty;
        private static string UploadEFolder = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value;
            ProcessedEFolder = Params.Find(f => f.Key == "ProcessedEFolder").Value;
            UploadEFolder = Params.Find(f => f.Key == "UploadEFolder").Value;
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
            List<LoanDownload> _loanList = dataAccess.GetEventLoans();
            Logger.WriteTraceLog($"_loanList : {_loanList.Count}");
            foreach (var _eLoan in _loanList)
            {
                dataAccess.UpdateStatusEwebHookEvents(_eLoan.WebEventID, EWebHookStatusConstant.EWEB_HOOK_PROCESSING);

                string _eLoanGUID = _eLoan.EnCompassLoanGUID.ToString();
                List<byte[]> _pdfBytes = new List<byte[]>();
                PDFMerger merger = null;
                string lockFileName = string.Empty;
                bool _docAssigned = false;

                ELoanAttachmentDownload _elAttachment = dataAccess.AddOrUpdateEDownloadStatus(_eLoan.LoanID, _eLoan.DownloadID, EncompassStatusConstant.DOWNLOAD_PENDING, _eLoan.RetryFlag); // user Retry and DownloadID to check

                try
                {
                    CheckAttachment:

                    List<EContainer> eContainers = _api.GetAllLoanDocuments(_eLoanGUID);

                    EContainer uploadContainer = eContainers.Where(x => x.Title == UploadEFolder).FirstOrDefault();

                    if (uploadContainer != null)
                    {
                        List<EDocumentAttachment> _eDocAttachments = uploadContainer.Attachments;

                        if (_eDocAttachments == null || _eDocAttachments.Count == 0)
                            goto BreakAttachmentLoop;

                        List<EAttachment> _lsEAttachments = new List<EAttachment>();

                        foreach (var item in _eDocAttachments)
                        {
                            string[] _attGUID = item.EntityId.Split('.');
                            string _attachmentGUID = _attGUID[0].Replace("attachment-", "");
                            EAttachment eAttachment = _api.GetAttachment(_eLoanGUID, _attachmentGUID);
                            _lsEAttachments.Add(eAttachment);
                        }

                        List<EDownloadStaging> _steps = dataAccess.SetDownloadSteps(_lsEAttachments, _elAttachment.ID, _eLoanGUID);
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

                        EContainer eContainer = eContainers.Where(e => e.Title == ProcessedEFolder).FirstOrDefault();

                        AddContainerResponse res = null;

                        if (eContainer == null)
                            res = _api.AddDocument(_eLoanGUID, ProcessedEFolder);

                        //Remove Attachments from MTS Upload Container
                        if (attachmentGUIDs.Count > 0)
                            _api.RemoveDocumentAttachments(_eLoanGUID, uploadContainer.DocumentId, attachmentGUIDs, UploadEFolder);

                        if (eContainer != null)
                        {
                            _docAssigned = _api.AssignDocumentAttachments(_eLoanGUID, eContainer.DocumentId, attachmentGUIDs, ProcessedEFolder);
                        }
                        else
                        {
                            _docAssigned = _api.AssignDocumentAttachments(_eLoanGUID, res.DocumentID, attachmentGUIDs, ProcessedEFolder);
                        }

                        goto CheckAttachment;
                    }

                    BreakAttachmentLoop:

                    string OrgFileName = string.Empty;
                    string newFileName = string.Empty;
                    if (_pdfBytes.Count > 0)
                    {

                        //AuditLoanMissingDoc auditLoanMissingDoc
                        int auditLoanMissingDocCount = dataAccess.GetAuditLoanMissingDocCount(_eLoan.LoanID);

                        string exactPath = Path.Combine(IntellaLendLoanUploadPath, dataAccess.TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));

                        newFileName = dataAccess.TenantSchema + "_" + _eLoan.LoanID.ToString() + "_" + (auditLoanMissingDocCount + 1) + ".lck";

                        Logger.WriteTraceLog($"newFileName : {newFileName}");
                        Logger.WriteTraceLog($"exactPath : {exactPath}");

                        try
                        {
                            if (!Directory.Exists(exactPath))
                                Directory.CreateDirectory(exactPath);

                            lockFileName = Path.Combine(exactPath, newFileName);
                            OrgFileName = Path.ChangeExtension(lockFileName, ".pdf");

                            Logger.WriteTraceLog($"lockFileName : {lockFileName}");
                            Logger.WriteTraceLog($"OrgFileName : {OrgFileName}");

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
                            dataAccess.DeleteStaginDetails(_eLoan.EnCompassLoanGUID);
                            dataAccess.DeleteWebHookEvents(_eLoan.WebEventID);
                        }
                        else
                        {
                            dataAccess.UpdateStatusEwebHookEvents(_eLoan.WebEventID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                            dataAccess.UpdateEDownloadStatus(_eLoan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_FAILED);
                        }
                    }
                    else
                    {
                        if (_elAttachment.ID > 0)
                        {
                            dataAccess.UpdateStatusEwebHookEvents(_eLoan.WebEventID, EWebHookStatusConstant.EWEB_HOOK_ERROR);
                            dataAccess.UpdateEDownloadStatus(_eLoan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_FAILED, "Attachment(s) not found in Encompass Unassigned folder");
                        }

                        throw new Exception($"Attachment(s) not found in Encompass {UploadEFolder} folder");
                    }

                    Dictionary<string, object> missingDocAuditInfo = new Dictionary<string, object>();
                    missingDocAuditInfo["LOANID"] = _eLoan.LoanID;
                    missingDocAuditInfo["DOCID"] = 0;
                    missingDocAuditInfo["USERID"] = 0;
                    missingDocAuditInfo["FILENAME"] = newFileName;
                    missingDocAuditInfo["EDOWNLOADSTAGINGID"] = _elAttachment.ID;

                    dataAccess.MissingDocFileUpload(missingDocAuditInfo);

                    File.Move(lockFileName, OrgFileName);
                    dataAccess.UpdateEDownloadStatus(_eLoan.LoanID, _elAttachment.ID, EncompassStatusConstant.DOWNLOAD_SUCCESS);
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

            List<EWebhookEvents> _eWebHookEvents = dataAccess.GetWebHooksEvent();
            foreach (var item in _eWebHookEvents)
            {
                EventEncompassLoanGUID _guid = JsonConvert.DeserializeObject<EventEncompassLoanGUID>(item.Response);
                dataAccess.UpdateStatusEwebHookEvents(item.ID, EWebHookStatusConstant.EWEB_HOOK_PROCESSING);
                dataAccess.updateLoanCompleteStatus(_guid.loanGUID.ToString(), dataAccess.TenantSchema, item.ID);
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
        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }


    }

    class LoanDownload
    {
        public Int64 LoanID { get; set; }
        public Guid? EnCompassLoanGUID { get; set; }
        public Int64 DownloadID { get; set; }
        public Int64 WebEventID { get; set; }
        public bool RetryFlag { get; set; }
    }
}
