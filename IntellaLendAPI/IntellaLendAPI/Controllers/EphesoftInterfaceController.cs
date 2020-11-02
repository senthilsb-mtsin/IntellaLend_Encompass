using IntellaLend.CommonServices;
using IntellaLend.Constance;
using IntellaLendAPI.Models;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLend_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EphesoftInterfaceController : ApiController
    {
        [HttpPost]
        public QCIQDBDetailsResponse GetQCIQConnectionString(QCIQDBDetailsRequest request)
        {
            Logger.WriteTraceLog($"Start GetQCIQConnectionString()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            QCIQDBDetailsResponse response = new QCIQDBDetailsResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var obj = new LoanService(request.TableSchema).GetQCIQLookupDetails(request.LoanID);
                if (obj == null)
                    throw new Exception("Loan Details not found in IntellaLend");

                response.data = JsonConvert.SerializeObject(new LoanService(request.TableSchema).GetQCIQLookupDetails(request.LoanID));
            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetQCIQConnectionString()");
            return response;
        }
        #region MAS

        [HttpPost]
        public IntellaLendResponse UpdateLOSExportFileStaging(MASUpdateLOSExportFileStagingRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateLOSExportFileStaging()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            IntellaLendResponse response = new IntellaLendResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                if (request.FileType == LOSExportFileTypeConstant.LOS_CLASSIFICATION_EXCEPTION)
                {
                    new LoanService(request.TableSchema).UpdateLOSClassificationExceptionFromMAS(request.LoanID, request.BatchID, request.BatchName);
                }
                else if (request.FileType == LOSExportFileTypeConstant.LOS_CLASSIFICATION_RESULTS)
                {
                    new LoanService(request.TableSchema).UpdateLOSClassificationResultsFromMAS(request.LoanID, request.BatchID, request.MASDocumentList, request.BatchName, request.BatchClassID, request.BCName);
                }
                else if (request.FileType == LOSExportFileTypeConstant.LOS_VALIDATION_EXCEPTION)
                {
                    new LoanService(request.TableSchema).UpdateLOSValidationExceptionFromMAS(request.LoanID, request.BatchID, request.BatchName);
                }
            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLOSExportFileStaging()");
            return response;
        }

        #endregion MAS

        [HttpPost]
        public IntellaLendResponse UpdateLoanTypeFromQCIQ(QCIQUpdateLoanTypeRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateLoanTypeFromQCIQ()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            IntellaLendResponse response = new IntellaLendResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                new LoanService(request.TableSchema).UpdateLoanTypeFromQCIQ(request.LoanID, request.LoanType, request.QCIQStartDate);
            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanTypeFromQCIQ()");
            return response;
        }



        [HttpPost]
        public IntellaLendResponse UpdateLoanTypeFromEncompass(EncompassUpdateLoanTypeRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateLoanTypeFromEncompass()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            IntellaLendResponse response = new IntellaLendResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                new LoanService(request.TableSchema).UpdateLoanTypeFromEncompass(request.LoanID, request.LoanNumber, request.BorrowerName);
            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanTypeFromEncompass()");
            return response;
        }

        [HttpPost]
        public IntellaLendResponse GetLoanDetails(EphesoftLoanDetailsRequest request)
        {
            Logger.WriteTraceLog($"Start GetLoanDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            EphesoftLoanDetailsResponse response = new EphesoftLoanDetailsResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var loanService = new LoanService(request.TableSchema);
                //Update Batch id to loan
                loanService.UpdateEphesoftBatchDetail(request.LoanID, request.BatchID, request.DocID, request.BatchClassID, request.BatchClassName);

                var det = loanService.GetLoanDetailsForEphesoft(request.LoanID);

                response.LoanDetailsJson = JsonConvert.SerializeObject(det);

            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanDetails()");
            return response;
        }

        [HttpPost]
        public IntellaLendResponse CheckLoanPageCount(EphesoftLoanPageCountRequest request)
        {
            Logger.WriteTraceLog($"Start CheckLoanPageCount()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            EphesoftLoanDetailsResponse response = new EphesoftLoanDetailsResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var loanService = new LoanService(request.TableSchema);

                var det = loanService.CheckLoanPageCount(request.LoanID, request.PageCount);

                response.LoanDetailsJson = JsonConvert.SerializeObject(det);

            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckLoanPageCount()");
            return response;
        }

        [HttpPost]
        public IntellaLendResponse GetEncompassDocumentPages(EncompassDocPagesRequest request)
        {
            Logger.WriteTraceLog($"Start GetEncompassDocumentPages()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            EncompassDocPagesResponse response = new EncompassDocPagesResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var loanService = new LoanService(request.TableSchema);

                bool isEncompassLoan = false;

                string _enDocPages = loanService.GetEncompassDocPages(request.LoanID, request.DocID, ref isEncompassLoan);

                response.isEncompassLoan = isEncompassLoan;
                response.EncompassDocPages = _enDocPages;

            }
            catch (Exception ex)
            {
                response.EncompassDocPages = string.Empty;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEncompassDocumentPages()");
            return response;
        }


        [HttpPost]
        public IntellaLendResponse UpdateReviewerDate(EphesoftLoanDetailsRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateReviewerDate()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            EphesoftLoanDetailsResponse response = new EphesoftLoanDetailsResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var loanService = new LoanService(request.TableSchema);
                //Update Batch id to loan
                loanService.UpdateEphesoftReviewedDate(request.LoanID, request.BatchID);

                response.LoanDetailsJson = JsonConvert.SerializeObject(new { Success = true });

            }
            catch (Exception ex)
            {
                response.LoanDetailsJson = JsonConvert.SerializeObject(new { Success = false });
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateReviewerDate()");
            return response;
        }

        [HttpPost]
        public IntellaLendResponse UpdateValidatorDate(EphesoftLoanDetailsRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateValidatorDate()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            EphesoftLoanDetailsResponse response = new EphesoftLoanDetailsResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var loanService = new LoanService(request.TableSchema);
                //Update Batch id to loan
                loanService.UpdateEphesoftValidatorDate(request.LoanID, request.BatchID);

                response.LoanDetailsJson = JsonConvert.SerializeObject(new { Success = true });

            }
            catch (Exception ex)
            {
                response.LoanDetailsJson = JsonConvert.SerializeObject(new { Success = false });
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateValidatorDate()");
            return response;
        }

        [HttpPost]
        public IntellaLendResponse UpdateEphesoftValidatorName(EphesoftLoanDetailsRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateEphesoftValidatorName()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            EphesoftLoanDetailsResponse response = new EphesoftLoanDetailsResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                var loanService = new LoanService(request.TableSchema);
                //Update Batch id to loan
                loanService.UpdateEphesoftValidatorName(request.LoanID, request.BatchID);

                var det = loanService.GetLoanDetailsForEphesoft(request.LoanID);

                response.LoanDetailsJson = JsonConvert.SerializeObject(det);

            }
            catch (Exception ex)
            {
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateEphesoftValidatorName()");
            return response;
        }


    }
}
