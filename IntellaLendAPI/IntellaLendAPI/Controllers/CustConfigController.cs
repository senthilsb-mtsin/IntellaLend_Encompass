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
    public class CustConfigController : ApiController
    {
        [HttpPost]
        public TokenResponse AddRetention(RequestAddRetention reqAddRetention)
        {
            Logger.WriteTraceLog($"Start AddRetention()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqAddRetention)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustConfigService(reqAddRetention.TableSchema).AddRetention(reqAddRetention.CustomerID, reqAddRetention.ConfigKey, reqAddRetention.ConfigValue, reqAddRetention.Active, reqAddRetention.CreatedOn, reqAddRetention.ModifiedOn));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AddRetention()");
            return response;
        }


        [HttpPost]
        public TokenResponse AddMultipleCustConfig(RequestAddMultipleRetention reqAddRetention)
        {
            Logger.WriteTraceLog($"Start AddMultipleCustConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqAddRetention)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustConfigService(reqAddRetention.TableSchema).AddMultipleCustConfig(reqAddRetention.CustomerID, reqAddRetention.custConfigItems));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AddMultipleCustConfig()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllCustConfig(RequestGetAllRetention getCustConfig)
        {
            Logger.WriteTraceLog($"Start GetAllCustConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(getCustConfig)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustConfigService(getCustConfig.TableSchema).GetAllCustConfig(getCustConfig.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetAllCustConfig()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetAllDistinctCustData(RequestGetAllRetention getCustConfig)
        {
            Logger.WriteTraceLog($"Start GetAllDistinctCustData()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(getCustConfig)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustConfigService(getCustConfig.TableSchema).GetAllDistinctCustData(getCustConfig.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetAllDistinctCustData()");
            return response;
        }

        [HttpPost]
        public TokenResponse EditRetention(RequestEditRetention editRetentionData)
        {
            Logger.WriteTraceLog($"Start EditRetention()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(editRetentionData)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustConfigService(editRetentionData.TableSchema).EditRetention(editRetentionData.updatecustomerconfiguration));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End EditRetention()");
            return response;
        }
        [HttpPost]
        public TokenResponse DeleteCustomerConfig(RequestDeleteRetention reqdelretention)
        {
            Logger.WriteTraceLog($"Start DeleteCustomerConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqdelretention)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CustConfigService(reqdelretention.TableSchema).DeleteCustomerConfig(reqdelretention.CustomerID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DeleteCustomerConfig()");
            return response;
        }
    }
}