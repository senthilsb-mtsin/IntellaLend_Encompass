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
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QueryAnalyzerController : ApiController
    {
        [HttpPost]
        public TokenResponse GetQueryResultData(QueryAnalyzerRequest querystring)
        {
            Logger.WriteTraceLog($"Start GetQueryResultData()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(querystring)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken(Request.Headers.GetValues("HashValue").FirstOrDefault().ToString(), Request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString());
                response.data = new JWTToken().CreateJWTToken(new QCIQService().GetQueryResultData(querystring.Querystring));

            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetQueryResultData()");
            return response;
        }

        [HttpPost]
        public TokenResponse QCIQAddLoanType(QCIQLoanTypeRequest reqloantypes)
        {
            Logger.WriteTraceLog($"Start QCIQAddLoanType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqloantypes)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken(Request.Headers.GetValues("HashValue").FirstOrDefault().ToString(), Request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString());
                response.data = new JWTToken().CreateJWTToken(new QCIQService(reqloantypes.TableSchema).QCIQAddLoanType(reqloantypes.QCIQReq));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End QCIQAddLoanType()");
            return response;
        }


        [HttpPost]
        public TokenResponse QCIQAddReviewType(QCIQReviewTypeRequest reviewtypereq)
        {
            Logger.WriteTraceLog($"Start QCIQAddReviewType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reviewtypereq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken(Request.Headers.GetValues("HashValue").FirstOrDefault().ToString(), Request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString());
                response.data = new JWTToken().CreateJWTToken(new QCIQService(reviewtypereq.TableSchema).QCIQAddReviewType(reviewtypereq.QCIQReq));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End QCIQAddReviewType()");
            return response;
        }

    }
}