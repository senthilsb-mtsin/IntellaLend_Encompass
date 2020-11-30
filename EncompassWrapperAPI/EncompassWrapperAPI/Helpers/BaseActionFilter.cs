using Encompass.WebConnector.Models;
using EncompassConnectorAPI.Controllers;
using EncompassWrapperConstants;
using MTS.Web.Helpers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EncompassConnectorAPI.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseActionFilter : ActionFilterAttribute, IExceptionFilter
    {

        //WebConnectorSession to connect to ECM
        private readonly WebConnectorSession webConnectorSession = new WebConnectorSession();
        private BaseController baseController;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string[] Decrypt(string data)
        {
            string DecryptedStr = EnDecryptor.DecryptAES(data);
            byte[] Base64Bytes = Convert.FromBase64String(DecryptedStr);
            string DecodeStr = Encoding.ASCII.GetString(Base64Bytes, 0, Base64Bytes.Length);
            return DecodeStr.Split(',');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        private void ActionExecuting(HttpActionContext actionContext)
        {
            baseController = (BaseController)actionContext.ControllerContext.Controller;

            IEnumerable<string> _token = new List<string>();
            IEnumerable<string> _tokenType = new List<string>();
            actionContext.ControllerContext.Request.Headers.TryGetValues(HeaderConstant.TokenHeader, out _token);
            actionContext.ControllerContext.Request.Headers.TryGetValues(HeaderConstant.TokenTypeHeader, out _tokenType);
            webConnectorSession.Token = _token.FirstOrDefault();
            webConnectorSession.TokenType = _tokenType.FirstOrDefault();

            try
            {
                //Check connection
                if (baseController.EncompassWebConnectorSession == null)
                {
                    //Create Connection and Add to BaseController
                    baseController.EncompassWebConnectorSession = new EncompassWebConnectorSession(webConnectorSession);
                    baseController._client = new RestWebClient(ConfigurationManager.AppSettings["EncompassURL"]);
                    baseController._client.RefreshValidationHeaders += AddToken;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"Exception while creating session {ex.Message} StackTrace : {ex.StackTrace}");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

        }

        private void AddToken(object sender, WebClientArgs e)
        {
            e.HeaderData = new KeyValuePair<string, string>("Authorization", $"{baseController.EncompassWebConnectorSession.TokenType} {baseController.EncompassWebConnectorSession.Token}");
        }


        /// <summary>
        /// 
        /// </summary>
        private void ActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            baseController.EncompassWebConnectorSession = null;
        }



        /// <summary>
        /// Executes an asynchronous exception filter.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                WebApiApplication.AddException(actionExecutedContext.Exception);
                Logger.WriteErrorLog($"Exception : {actionExecutedContext.Exception.Message} StackTrace : {actionExecutedContext.Exception.StackTrace}");
            }, cancellationToken);
        }
        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                ActionExecuted(actionExecutedContext);
                //base.OnActionExecuted(actionExecutedContext);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"Exception while creating session {ex.Message} StackTrace : {ex.StackTrace}");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
        }
        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    ActionExecuted(actionExecutedContext);
                    //base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog($"Exception while creating session {ex.Message} StackTrace : {ex.StackTrace}");
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }

            }, cancellationToken);
        }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                ActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                Logger.WriteErrorLog($"Exception OnActionExecuting {ex.Message} StackTrace : {ex.StackTrace}");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            // base.OnActionExecuting(actionContext);
        }
        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    ActionExecuting(actionContext);
                    //base.OnActionExecutingAsync(actionContext, cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog($"Exception OnActionExecuting {ex.Message} StackTrace : {ex.StackTrace}");
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }

            }, cancellationToken);

        }
    }
}