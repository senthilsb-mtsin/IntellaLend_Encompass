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

namespace IL.EncompassUpload
{
    public class EncompassFileUploader : IMTSServiceBase
    {
        private static string EvaluatedResultParkingSpotName = string.Empty;
        private static string EncompassWrapperAPIURL = string.Empty;



        public void OnStart(string ServiceParam)
        {

            EncompassFileUploaderDataAccess _EncompassFileUploaderDataAccess = new EncompassFileUploaderDataAccess();

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
                    EncompassFileUploaderDataAccess _encompassFileUploaderDataAccess = new EncompassFileUploaderDataAccess();
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

                    dataAccess.UpdateEncompassUploadStatus(_eLoan.ID, EncompassUploadConstant.UPLOAD_PROCESSING);
                   // RuleEvaluatedResult Evaluate = new RuleEvaluatedResult();
                    List<EUploadStaging> _eUploadStaging = new List<EUploadStaging>();
                    List<EContainer> allDocs = _api.GetAllLoanDocuments(_eLoan.ELoanGUID.ToString());
                    Batch Batchobj = dataAccess.GetBatchDetails(_eLoan.LoanID);

                    if (_eLoan.Status != EncompassUploadConstant.UPLOAD_RETRY)
                        _eUploadStaging = dataAccess.SaveDocumentDetails(_eLoan.LoanID, _eLoan.TypeOfUpload, _eLoan.ID, Batchobj);
                    else
                        _eUploadStaging = dataAccess.GetRetryLoans(_eLoan.LoanID, _eLoan.TypeOfUpload);

                    ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(Batchobj.Schema);

                    byte[] StackorderPDF = _imageWrapper.GetLoanPDF(Batchobj.LoanID);



                    foreach (var _doctype in _eUploadStaging)
                    {

                        dataAccess.UpdateUploadStaging(_eLoan.LoanID, EncompassUploadStagingConstant.UPLOAD_STAGING_PROCESSING, _doctype.UploadStagingID,_doctype.ErrorMsg);
                        List<int> pages = dataAccess.GetPagesFromBatch(_doctype.Document, _doctype.Version, Batchobj);


                        byte[] pdfBytes = CommonUtils.GetPdfBytes(pages, StackorderPDF);
                        EContainer EParkingSpot = allDocs.Find(x => x.Title == _doctype.EParkingSpot);
                        if (EParkingSpot != null)
                        {
                            string Filename = _doctype.EParkingSpot;
                            EUploadResponse uploadDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), Filename, ".pdf", StackorderPDF);

                            if (uploadDetails.Status == true)
                            {
                                _api.AssignDocumentAttachment(_eLoan.ELoanGUID.ToString(), EParkingSpot.DocumentId, uploadDetails.AttachmentGUID);
                                dataAccess.UpdateUploadStaging(_eLoan.LoanID, EncompassUploadStagingConstant.UPLOAD_STAGING_COMPLETE, _doctype.UploadStagingID,uploadDetails.Message);
                            }
                            else
                            {
                                dataAccess.UpdateEncompassUploadStatus(_eLoan.LoanID, EncompassUploadConstant.UPLOAD_FAILED);
                                dataAccess.UpdateUploadStaging(_eLoan.LoanID, EncompassUploadStagingConstant.UPLOAD_STAGING_FAILED, _doctype.UploadStagingID,uploadDetails.Message);
                                Exception ex = new Exception("failed while uploading PDF to Encompass");
                            }
                        }
                    }

                    EContainer EParkingSpotName = allDocs.Find(x => x.Title == EvaluatedResultParkingSpotName);
                    if (EParkingSpotName != null)
                    {
                        
                        byte[] checkListPDF = Evaluate.GeneratePDF(_eLoan.LoanID, dataAccess, dataAccess.TenantSchema);
                        EUploadResponse uploadChecklistDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), "checklistPDF", ".pdf", checkListPDF);
                        _api.AssignDocumentAttachment(_eLoan.ELoanGUID.ToString(), EParkingSpotName.DocumentId, uploadChecklistDetails.AttachmentGUID);

                        
                    }
                    else
                    {

                        byte[] checkListPDF = Evaluate.GeneratePDF(_eLoan.LoanID, dataAccess, dataAccess.TenantSchema);
                        string rs = _api.AddDocument(_eLoan.ELoanGUID.ToString(), EParkingSpotName.Title);
                        EUploadResponse uploadChecklistDetails = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), "checklistPDF", ".pdf", checkListPDF);
                        EUploadResponse uploadParkingSpot = _api.UploadAttachment(_eLoan.ELoanGUID.ToString(), EParkingSpotName.Title, ".pdf", checkListPDF);
                        _api.AssignDocumentAttachment(_eLoan.ELoanGUID.ToString(), EParkingSpotName.DocumentId, uploadChecklistDetails.AttachmentGUID);
                    }
                    dataAccess.UpdateEncompassUploadStatus(_eLoan.LoanID, EncompassUploadConstant.UPLOAD_COMPLETE);
                }
            }

            catch (Exception ex)
            {
             
                Exception Ex = new Exception($" failed while uploading PDF to Encompass {ex.Message} ", ex);
                throw Ex;
            }

        }
    }
}