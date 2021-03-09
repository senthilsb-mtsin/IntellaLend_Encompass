using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EncompassInterfaceController : ApiController
    {
        [HttpPost]
        public TokenResponse LoanComplete(LoanRequest req)
        {
            Logger.WriteTraceLog($"Start LoanComplete()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken(Request.Headers.GetValues("HashValue").FirstOrDefault().ToString(), Request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString());
                new LoanService(req.TableSchema).LoanComplete(req.LoanID, req.CompletedUserRoleID, req.CompletedUserID, req.CompleteNotes);
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End LoanComplete()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetToken(GetTokenRequest req)
        {
            Logger.WriteTraceLog($"Start GetToken()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.data = JsonConvert.SerializeObject(new IntellaLendServices().GetEncompassToken(req.InstanceID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetToken()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetDocumentEvent(ECallBack req)
        {
            Logger.WriteTraceLog($"Start SetDocumentEvent()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                //response.token = new JWTToken().CreateJWTToken();
                response.data = JsonConvert.SerializeObject(new IntellaLendServices().SetDocumentEvent(req.meta.resourceId, req.meta.instanceId));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End LoanComplete()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetMilestoneEvent(ECallBack req)
        {
            Logger.WriteTraceLog($"Start SetMilestoneEvent()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                // response.token = new JWTToken().CreateJWTToken();
                response.data = JsonConvert.SerializeObject(new IntellaLendServices().SetMileStoneEvent(req.meta.resourceId, req.meta.instanceId));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End LoanComplete()");
            return response;
        }
    }
}