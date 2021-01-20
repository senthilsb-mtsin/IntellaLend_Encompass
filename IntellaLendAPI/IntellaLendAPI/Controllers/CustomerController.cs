using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerController : ApiController
    {
        [HttpPost]
        public TokenResponse GetCustomerList(CustomerRequest customerDetails)
        {
            Logger.WriteTraceLog($"Start GetCustomerList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(customerDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(customerDetails.TableSchema).GetCustomerList(false));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustomerList()");
            return response;

        }
        [HttpPost]
        public TokenResponse AddCustomer(CustomerUpdateRequest customerMasterDetails)
        {
            Logger.WriteTraceLog($"Start AddCustomer()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(customerMasterDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(customerMasterDetails.TableSchema).AddCustomer(customerMasterDetails.customerMaster));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AddCustomer()");
            return response;
        }
        [HttpPost]
        public TokenResponse EditCustomer(CustomerUpdateRequest customerMasterDetials)

        {
            Logger.WriteTraceLog($"Start EditCustomer()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(customerMasterDetials)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(customerMasterDetials.TableSchema).EditCustomer(customerMasterDetials.customerMaster));
            }

            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End EditCustomer()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetActiveCustomerList(CommonListRequest table)
        {
            Logger.WriteTraceLog($"Start GetActiveCustomerList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(table)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(table.TableSchema).GetCustomerList(true));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetActiveCustomerList()");
            return response;

        }
        [HttpPost]

        public TokenResponse RetentionPurge(RequestRetentionPurge reqretpurge)
        {
            Logger.WriteTraceLog($"Start RetentionPurge()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqretpurge)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(reqretpurge.TableSchema).RetentionPurge(reqretpurge.LoanIDs, reqretpurge.UserID, reqretpurge.UserName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RetentionPurge()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustomerImportStaging(RequestCustomerImportStaging req)
        {
            Logger.WriteTraceLog($"Start GetCustomerImportStaging()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(req.TableSchema).GetCustomerImportStaging(req.Status, req.ImportDateFrom, req.ImportDateTo,req.AssignType));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustomerImportStaging()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustomerImportStagingDetails(RequestCustomerImportStagingDetails req)
        {
            Logger.WriteTraceLog($"Start GetCustomerImportStagingDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustomerService(req.TableSchema).GetCustomerImportStagingDetails(req.CustomerImportStagingID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustomerImportStagingDetails()");
            return response;
        }
    }
}
