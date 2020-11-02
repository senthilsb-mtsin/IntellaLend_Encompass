using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;
using static IntellaLendAPI.Models.ELoanRequest;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ExportController : ApiController
    {
        [HttpPost]
        public TokenResponse GetExportMonitorDetails(ExportMonitorRequest request)
        {
            Logger.WriteTraceLog($"Start GetExportMonitorDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetLoanExportMonitorDetails());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetExportMonitorDetails()");
            return response;
        }
        
        [HttpPost]
        public TokenResponse SearchExportMonitorDetails(ExportMonitorRequest request)
        {
            Logger.WriteTraceLog($"Start SearchExportMonitorDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SearchExportMonitorDetails(request.Status, request.Customer, request.ExportedDate));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SearchExportMonitorDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse SearchLoanExport(LoanSearchRequest request)
        {
            Logger.WriteTraceLog($"Start SearchLoanExport()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SearchLoanExport(request.FromDate, request.ToDate, request.CurrentUserID, request.LoanNumber, request.LoanType, request.BorrowerName,
                     request.LoanAmount, request.ReviewStatus, request.AuditMonthYear, request.ReviewType, request.Customer, request.PropertyAddress, request.InvestorLoanNumber));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SearchLoanExport()");
            return response;
        }

        [HttpPost]
        public TokenResponse ExportToBox(ExportToBoxRequest request)
        {
            Logger.WriteTraceLog($"Start ExportToBox()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).ExportToBox(request.LoanID));
             
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End ExportToBox()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCurrentJobDetail(SearchJobLoanRequest request)
        {

            Logger.WriteTraceLog($"Start GetCurrentJobDetail()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetCurrentJobDetail(request.JobID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetCurrentJobDetail()");
            return response;
        }
        [HttpPost]
        public TokenResponse SaveLoanJob(SaveJobLoanRequest request)
        {
            Logger.WriteTraceLog($"Start SaveLoanJob()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SaveLoanJob(request.JobName, request.CustomerID, request.CoverLetter,
                request.TableOfContent, request.PasswordProtected, request.Password, request.CoverLetterContent, request.BatchLoanDoc,request.ExportedBy));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveLoanJob()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetLoanDetails(LoanRequest loan)
        {
            Logger.WriteTraceLog($"Start GetLoanDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).GetLoanDocDetails(loan.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeleteBatch(SearchJobLoanRequest loan)
        {
            Logger.WriteTraceLog($"Start DeleteBatch()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loan)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(loan.TableSchema).DeleteJob(loan.JobID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End DeleteBatch()");
            return response;
        }
        [HttpPost]
        public TokenResponse ReSentJobLoanExport(SearchJobLoanRequest request)
        {
            Logger.WriteTraceLog($"Start ReSentJobLoanExport()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new Models.ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).ReSentJobLoanExport(request.JobID,request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End ReSentJobLoanExport()");
            return response;
        }
//Encompass Export---saranya change

        [HttpPost]
        public TokenResponse GetEncompassExportDetails(ExportMonitorRequest request)
        {
            Logger.WriteTraceLog($"Start GetEncompassExportDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetEncompassExportDetails());

            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEncompassExportDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse RetryEncompassExport(ELoanRetryRequest request)
        {
            Logger.WriteTraceLog($"Start RetryEncompassExport()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new Models.ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).RetryEncompassExport(request.ID, request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End RetryEncompassExport()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetEncompassCurrentExportDetail(ELoanRetryRequest request)
        {

            Logger.WriteTraceLog($"Start GetEncompassCurrentExportDetail()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetEncompassCurrentExportDetail(request.ID,request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEncompassCurrentExportDetail()");
            return response;
        }
        [HttpPost]
        public TokenResponse SearchEncompassExportDetails(ELoanRequest request)
        {
            Logger.WriteTraceLog($"Start SearchEncompassExportDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SearchEncompassExportDetails(request.Status,request.EncompassExportedDate,request.Customer));

            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SearchEncompassExportDetails()");
            return response;
        }
        //Los Export
        [HttpPost]
        public TokenResponse SearchLosExportMonitorDetails(LosExportMonitorRequest request)
        {
            Logger.WriteTraceLog($"Start SearchLosExportMonitorDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).SearchLosExportMonitorDetails(request.Customer, request.ExportedDate,request.LoanType,request.ServiceType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SearchLosExportMonitorDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse RetryLosExportDetails(CurrentExportLoanRequest request)
        {
            Logger.WriteTraceLog($"Start RetryLosExportDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).RetryLosExportDetails(request.ID, request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End RetryLosExportDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLOSCurrentExportLoanDetail(CurrentExportLoanRequest request)
        {

            Logger.WriteTraceLog($"Start GetLOSCurrentExportLoanDetail()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new LoanService(request.TableSchema).GetLOSCurrentExportLoanDetail(request.ID, request.LoanID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLOSCurrentExportLoanDetail()");
            return response;
        }
    }
}