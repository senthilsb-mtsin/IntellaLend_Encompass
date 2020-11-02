using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IntellaLend.Constance;
using Microsoft.Practices.EnterpriseLibrary.Logging;

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
                BaseExceptionHandler.HandleException(ref ex);
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

                        dataAccess.UpdateEncompassUploadStatus(_eLoan.LoanID, EncompassUploadConstant.UPLOAD_PROCESSING);

                        List<EContainer> allDocs = _api.GetAllLoanDocuments(_eLoan.ELoanGUID.ToString());

                        ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(dataAccess.TenantSchema);

                        byte[] StackorderPDF = _imageWrapper.GetLoanPDF(Batchobj.LoanID);
                        bool uploadFailed = true;
                        foreach (var _doctype in _eUploadStaging)
                        {
                            string dbParkingSpot = dataAccess.GetParkingSpotName(_doctype.Document);
                            if (dbParkingSpot.Equals(string.Empty))
                            {
                                dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_PROCESSING);
                                List<int> pages = GetPagesFromBatch(_doctype.Document, _doctype.Version, Batchobj);
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
                                            EAddRemoveAttachmentResponse result = _api.AssignDocumentAttachment(_eLoan.ELoanGUID.ToString(), EParkingSpot.DocumentId, uploadDetails.AttachmentGUID);

                                            if (result.Status)
                                            {
                                                uploadFailed = false;
                                                dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE);
                                            }
                                            else
                                                dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Assigning to Parkingspot failed");
                                        }
                                        else
                                            dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"{Filename} Upload to Encompass Failed");
                                    }
                                    else
                                        dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Parkingspot({dbParkingSpot}) not found on the Encompass");
                                }
                                else
                                    dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Pages not found for {_doctype.Document}, Version : { _doctype.Version}");
                            }
                            else
                                dataAccess.UpdateUploadStaging(_doctype.ID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Parking spot not from the document type : {_doctype.Document}");
                        }

                        if (uploadFailed && _eUploadStaging.Count > 0)
                            throw new Exception($"Upload Document(s) to Encompass Failed");

                        Logger.Write("Test");

                        Int32 VersionNumber = dataAccess.GetRulePDFVersion(_eLoan.ID, _eLoan.LoanID);
                        Logger.Write($"VersionNumber : {VersionNumber}");
                        string ruleFileName = VersionNumber > 1 ? $"Evaluated Result - V{VersionNumber}.pdf" : $"Evaluated Result.pdf";

                        Int64 ruleStagingID = dataAccess.InsertRuleResultStaging(_eLoan.ID, _eLoan.LoanID, ruleFileName, VersionNumber);

                        EContainer EParkingSpotName = allDocs.Where(x => x.Title == EvaluatedResultParkingSpotName).FirstOrDefault();
                        bool ruleResultUploadFailed = true;
                        try
                        {
                            Logger.Write($"VersionNumber  5: {VersionNumber}");
                            byte[] checkListPDF = new RuleEvaluatedResult().GeneratePDF(_eLoan.LoanID, dataAccess, ruleFileName);
                            Logger.Write($"VersionNumber  2: {VersionNumber}");
                            if (EParkingSpotName != null)
                            {
                                Logger.Write($"checkListPDF  2: {EParkingSpotName.Title}");
                                EUploadResponse uploadChecklistDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), ruleFileName, ruleFileName, checkListPDF);
                                if (uploadChecklistDetails.Status)
                                {
                                    Logger.Write($"uploadChecklistDetails  2: {EParkingSpotName.Title}");
                                    EAddRemoveAttachmentResponse res = _api.AssignDocumentAttachment(_eLoan.ELoanGUID.ToString(), EParkingSpotName.DocumentId, uploadChecklistDetails.AttachmentGUID);
                                    if (res.Status)
                                    {
                                        ruleResultUploadFailed = false;
                                        dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE);
                                    }
                                    else
                                        dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Assigning to Parkingspot failed");
                                }
                                else
                                {
                                    Logger.Write($"else uploadChecklistDetails  2: {EParkingSpotName.Title}");
                                    dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"{ruleFileName} Upload to Encompass Failed");
                                }
                            }
                            else
                            {
                           
                                AddContainerResponse rs = _api.AddDocument(_eLoan.ELoanGUID.ToString(), EParkingSpotName.Title);
                                EUploadResponse uploadChecklistDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), ruleFileName, ruleFileName, checkListPDF);
                                if (uploadChecklistDetails.Status)
                                {
                                    EAddRemoveAttachmentResponse res = _api.AssignDocumentAttachment(_eLoan.ELoanGUID.ToString(), rs.DocumentID, uploadChecklistDetails.AttachmentGUID);
                                    if (res.Status)
                                    {
                                        Logger.Write($"else res  2: {EParkingSpotName.Title}");
                                        ruleResultUploadFailed = false;
                                        dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE);
                                    }
                                    else
                                    {
                                        Logger.Write($"else ruleStagingID  2: {EParkingSpotName.Title}");
                                        dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Assigning to Parkingspot failed");
                                    }
                                }
                                else
                                {
                                    Logger.Write($"else UpdateUploadStaging  2: {EParkingSpotName.Title}");
                                    dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"{ruleFileName} Upload to Encompass Failed");
                                }
                            }

                            if (ruleResultUploadFailed)
                                dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_FAILED, $"Rule result upload failed");
                            else
                                dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_COMPLETE);
                        }
                        catch (Exception ex)
                        {
                            if (ruleStagingID > 0)
                                dataAccess.UpdateUploadStaging(ruleStagingID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, $"Rule result upload failed");

                            throw ex;
                        }
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
                BaseExceptionHandler.HandleException(ref ex);
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
    }

    public class DocumentUpload
    {
        public string DocumentName { get; set; }
        public int Version { get; set; }
    }
}