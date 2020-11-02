using IntellaLend.ExceptionHandler;
using IntellaLendAPI.Models;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ErrorHandlerController : ApiController
    {
        [HttpPost]
        public WebExceptionResponse WebConsoleErrorHandler(WebConsoleErrorHandlerRequest req)
        {
            Logger.WriteTraceLog($"Start WebConsoleErrorHandler()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            WebExceptionResponse response = new WebExceptionResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                string _error = JsonConvert.SerializeObject(req.Error);
                response.Status = true;
                Exception _webEx = new AngularWebException(_error);
                MTSUIExceptionHandler.HandleException(ref _webEx);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End WebConsoleErrorHandler()");
            return response;
        }


        [HttpPost]
        public WebExceptionResponse WebRequestErrorHandler(WebRequestErrorHandlerRequest req)
        {
            Logger.WriteTraceLog($"Start WebRequestErrorHandler()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            WebExceptionResponse response = new WebExceptionResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.Status = true;
                // string _date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                Exception _webEx = new AngularWebException($"Web TimeStamp : {req.timeStamp}, Status : {req.status.ToString()}, URL : {req.url}");
                MTSUIExceptionHandler.HandleException(ref _webEx);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End WebRequestErrorHandler()");
            return response;

        }
    }
}
