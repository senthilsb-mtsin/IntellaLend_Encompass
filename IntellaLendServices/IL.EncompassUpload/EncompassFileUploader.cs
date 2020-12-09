using EncompassAPIHelper;
using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IL.EncompassUpload
{
    public class EncompassFileUploader : IMTSServiceBase
    {
        private static string EvaluatedResultParkingSpotName = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;



        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            EvaluatedResultParkingSpotName = Params.Find(f => f.Key == "EvaluatedResultParkingSpotName").Value;
            EncompassWrapperAPIURL = Params.Find(f => f.Key == "EncompassWrapperAPIURL").Value;
        }



        public bool DoTask()
        {
            try
            {
                var TenantList = EncompassFileUploaderDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EncompassFileUploaderDataAccess _encompassFileUploaderDataAccess = new EncompassFileUploaderDataAccess(tenant.TenantSchema);
                    EncompassWrapperAPI _api = new EncompassWrapperAPI(EncompassWrapperAPIURL, tenant.TenantSchema);
                    UploadToEncompass(_api, _encompassFileUploaderDataAccess, EvaluatedResultParkingSpotName);
                    _api.Dispose();

                }

                return true;

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;

        }




        public void UploadToEncompass(EncompassWrapperAPI _api, EncompassFileUploaderDataAccess dataAccess, string EvaluatedResultParkingSpotName)
        {
            //UploadWaitingLoans
            try
            {

                List<ELoanAttachmentUpload> WaitingLoans = dataAccess.GetUploadWaitingandRetryLoans();
                foreach (var _eLoan in WaitingLoans)
                {
                    try
                    {
                        Batch Batchobj = dataAccess.GetBatchDetails(_eLoan.LoanID);
                        List<EUploadStaging> _eUploadStaging = new List<EUploadStaging>();

                        if (_eLoan.Status == EncompassUploadConstant.UPLOAD_RETRY)
                            _eUploadStaging = dataAccess.GetRetryLoans(_eLoan.ID, _eLoan.TypeOfUpload);
                        else
                            _eUploadStaging = dataAccess.SaveDocumentDetails(_eLoan.LoanID, _eLoan.TypeOfUpload, _eLoan.ID, Batchobj);

                        LogMessage($"_eUploadStaging.count : {_eUploadStaging.Count}");

                        dataAccess.UpdateEncompassUploadStatus(_eLoan.LoanID, EncompassUploadConstant.UPLOAD_PROCESSING);

                        List<EContainer> allDocs = _api.GetAllLoanDocuments(_eLoan.ELoanGUID.ToString());

                        LogMessage($"EallDocs.count : {allDocs.Count}");

                        ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(dataAccess.TenantSchema);
                        byte[] StackorderPDF = null;
                        try
                        {
                            StackorderPDF = _imageWrapper.GetLoanPDF(Batchobj.LoanID);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error while fetching Stacking order PDF in IntellaLend", ex);
                        }

                        LogMessage($"StackorderPDF : {StackorderPDF != null}");
                        List<bool> uploadFailed = new List<bool>();
                        if (StackorderPDF != null)
                        {
                            foreach (var _doctype in _eUploadStaging)
                            {
                                LogMessage($"_doctype.Document : {_doctype.Document}");
                                string dbParkingSpot = dataAccess.GetParkingSpotName(_doctype.Document);
                                LogMessage($"dbParkingSpot : {dbParkingSpot}");
                                if (!dbParkingSpot.Equals(string.Empty))
                                {
                                    dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_PROCESSING, string.Empty);
                                    List<int> pages = GetPagesFromBatch(_doctype.Document, _doctype.Version, Batchobj);
                                    LogMessage($"pages : {pages.Count}");
                                    if (pages != null)
                                    {
                                        byte[] pdfBytes = CommonUtils.GetPdfBytes(pages, StackorderPDF);
                                        EContainer EParkingSpot = allDocs.Where(x => x.Title.Trim().ToLower() == dbParkingSpot.Trim().ToLower()).FirstOrDefault();
                                        if (EParkingSpot != null && pdfBytes.Length > 0)
                                        {
                                            string Filename = $"{_doctype.Document}-V{_doctype.Version}.pdf";
                                            EUploadResponse uploadDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), Filename, Filename, pdfBytes);
                                            if (uploadDetails.Status)
                                            {
                                                List<string> _attachments = new List<string>() { uploadDetails.AttachmentGUID };
                                                bool result = _api.AssignDocumentAttachments(_eLoan.ELoanGUID.ToString(), EParkingSpot.DocumentId, _attachments, EParkingSpot.Title);

                                                if (result)
                                                {
                                                    uploadFailed.Add(true);
                                                    dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE, EParkingSpot.Title);
                                                    dataAccess.RemoveUploadedStagingDetail(_doctype.ID);
                                                }
                                                else
                                                    dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EParkingSpot.Title, $"Could not move the attachment '{Filename}' to the Efolder folder '{dbParkingSpot}'");
                                            }
                                            else
                                                dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EParkingSpot.Title, $"{Filename} Upload to Encompass Failed");
                                        }
                                        else
                                            dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, string.Empty, $"EFolder '{dbParkingSpot}' is not found in Encompass");
                                    }
                                    else
                                        dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, string.Empty, $"Stacking order PDF, missing page(s) for the Document :  {_doctype.Document}, Version : {_doctype.Version} in IntellaLend");
                                }
                                else
                                    dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, string.Empty, $"Encompass EFolder not mapped for the document type : {_doctype.Document} in IntellaLend");
                            }
                        }

                        if ((_eUploadStaging.Count > 0) && (uploadFailed.Count != _eUploadStaging.Count))
                            throw new Exception($"Upload Document(s) to Encompass Failed");

                        LogMessage("Test");

                        Int32 VersionNumber = dataAccess.GetRulePDFVersion(_eLoan.ID, _eLoan.LoanID);
                        LogMessage($"VersionNumber : {VersionNumber}");

                        string ruleFileName = VersionNumber > 0 ? $"Audit Report - V{(VersionNumber + 1)}.pdf" : $"Audit Report.pdf";

                        VersionNumber = VersionNumber + 1;

                        Int64 ruleStagingID = dataAccess.InsertRuleResultStaging(_eLoan.ID, _eLoan.LoanID, ruleFileName, VersionNumber);

                        EContainer EParkingSpotName = allDocs.Where(x => x.Title == EvaluatedResultParkingSpotName).FirstOrDefault();
                        bool ruleResultUploadFailed = true;
                        try
                        {
                            LogMessage($"VersionNumber  5: {VersionNumber}");
                            byte[] checkListPDF = new RuleEvaluatedResult().GeneratePDF(_eLoan.LoanID, dataAccess.TenantSchema);
                            LogMessage($"VersionNumber  2: {VersionNumber}");
                            if (EParkingSpotName != null)
                            {
                                LogMessage($"checkListPDF  2: {EParkingSpotName.Title}");
                                if (checkListPDF != null)
                                {
                                    EUploadResponse uploadChecklistDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), ruleFileName, ruleFileName, checkListPDF);
                                    if (uploadChecklistDetails.Status)
                                    {
                                        LogMessage($"uploadChecklistDetails  2: {EParkingSpotName.Title}");
                                        List<string> _attachments = new List<string>() { uploadChecklistDetails.AttachmentGUID };
                                        bool res = _api.AssignDocumentAttachments(_eLoan.ELoanGUID.ToString(), EParkingSpotName.DocumentId, _attachments, EvaluatedResultParkingSpotName);
                                        if (res)
                                        {
                                            ruleResultUploadFailed = false;
                                            dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE, EvaluatedResultParkingSpotName);
                                            dataAccess.RemoveUploadedStagingDetail(ruleStagingID);


                                        }
                                        else
                                            dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EvaluatedResultParkingSpotName, $"Could not move the attachment '{ruleFileName}' to the Efolder folder '{EvaluatedResultParkingSpotName}'");
                                    }
                                    else
                                    {
                                        LogMessage($"else uploadChecklistDetails  2: {EParkingSpotName.Title}");
                                        dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EvaluatedResultParkingSpotName, $"{ruleFileName} Upload to Encompass Failed");
                                    }
                                }
                                else
                                {
                                    LogMessage($"else uploadChecklistDetails Report PDF not creacted");
                                    dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EvaluatedResultParkingSpotName, $"Unable to create Audit report");
                                }
                            }
                            else
                            {
                                if (checkListPDF != null)
                                {
                                    AddContainerResponse rs = _api.AddDocument(_eLoan.ELoanGUID.ToString(), EvaluatedResultParkingSpotName);
                                    EUploadResponse uploadChecklistDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), ruleFileName, ruleFileName, checkListPDF);
                                    if (uploadChecklistDetails.Status)
                                    {
                                        List<string> _attachments = new List<string>() { uploadChecklistDetails.AttachmentGUID };
                                        bool res = _api.AssignDocumentAttachments(_eLoan.ELoanGUID.ToString(), rs.DocumentID, _attachments, EvaluatedResultParkingSpotName);
                                        if (res)
                                        {
                                            LogMessage($"else res  2: {EvaluatedResultParkingSpotName}");
                                            ruleResultUploadFailed = false;
                                            dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE, EvaluatedResultParkingSpotName);
                                            dataAccess.RemoveUploadedStagingDetail(ruleStagingID);
                                        }
                                        else
                                        {
                                            LogMessage($"else ruleStagingID  2: {EvaluatedResultParkingSpotName}");
                                            dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EvaluatedResultParkingSpotName, $"Could not move the attachment '{ruleFileName}' to the Efolder folder '{EvaluatedResultParkingSpotName}'");
                                        }
                                    }
                                    else
                                    {
                                        LogMessage($"else UpdateUploadStaging  2: {EvaluatedResultParkingSpotName}");
                                        dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EvaluatedResultParkingSpotName, $"{ruleFileName} Upload to Encompass Failed");
                                    }
                                }
                                else
                                {
                                    LogMessage($"else uploadChecklistDetails Report PDF not creacted");
                                    dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, EvaluatedResultParkingSpotName, $"Unable to create Audit report");
                                }
                            }

                            if (ruleResultUploadFailed)
                                dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_FAILED, $"Rule Evaluated result upload failed");
                            else
                                dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_COMPLETE);

                            //remove ELoanAttachmentUpload (header table) data if all details records are successfull
                            if(dataAccess.IsSuccessfullUpload(_eLoan.ID))
                            {
                                dataAccess.RemoveUploadStaging(_eLoan.ID);
                            }
                        }
                        catch (EncompassWrapperLoanLockException ex)
                        {
                            if (ruleStagingID > 0)
                                dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_RETRY, ex.Message);

                            Exception exe = new Exception("Loan Locked", ex);
                            MTSExceptionHandler.HandleException(ref exe);
                        }
                        catch (Exception ex)
                        {
                            if (ruleStagingID > 0)
                                dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, string.Empty, $"Rule result upload failed : {ex.Message}");

                            throw ex;
                        }
                    }
                    catch (EncompassWrapperLoanLockException ex)
                    {
                        dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_RETRY, ex.Message);

                        Exception exe = new Exception("Loan Locked", ex);
                        MTSExceptionHandler.HandleException(ref exe);
                    }
                    catch (Exception ex)
                    {
                        dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_FAILED, ex.Message);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                throw ex;
            }
        }

        public List<int> GetPagesFromBatch(string Document, int Version, Batch Batchobj)
        {
            List<string> btcPages = Batchobj.Documents.Where(x => (Document.Trim().ToLower().Equals(x.Type.Trim().ToLower())) && Version == x.VersionNumber).Select(x => x.Pages).FirstOrDefault();

            if (btcPages != null)
                return btcPages.ConvertAll<int>(Convert.ToInt32);

            return null;
        }

        public void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }
    }

    public class DocumentUpload
    {
        public string DocumentName { get; set; }
        public int Version { get; set; }
    }
}