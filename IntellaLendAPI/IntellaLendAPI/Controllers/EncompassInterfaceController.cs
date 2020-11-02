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
                response.token = new JWTToken().CreateJWTToken();
                new LoanService(req.TableSchema).LoanComplete(req.LoanID, req.CompletedUserRoleID, req.CompletedUserID,req.CompleteNotes);
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