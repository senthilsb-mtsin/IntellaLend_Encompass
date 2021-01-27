using EncompassConstants;
using EncompassRequestBody.ECallbackModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassWrapperConstants;
using MTS.Web.Helpers;
using MTSEntBlocks.ExceptionBlock.Handlers;
using RestSharp;
using Swagger.Net.Annotations;
using System;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace EncompassConnectorAPI.Controllers
{
    ///<Summary>
    /// Encompass WebHook Callback Releated Activities
    ///</Summary>
    public class EncompassCallbackController : ApiController
    {
        #region Constructure 

        private readonly RestWebClient _client;

        ///<Summary>
        /// To Create Rest Client Instance
        ///</Summary>
        public EncompassCallbackController()
        {
            _client = new RestWebClient(ConfigurationManager.AppSettings["IntellaLendAPIURL"]);
        }

        #endregion


        ///<Summary>
        /// Get Document Resource WebHook Callback Event
        ///</Summary>
        [HttpPost, Route("api/v1/callback/document")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public IHttpActionResult DocumentCallBack(ECallBack res)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(IntellaLendURLConstant.SET_DOCUMENT_EVENT), Content = res, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Exception ex = new Exception(responseStream);
                    MTSExceptionHandler.HandleException(ref ex);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest();
        }

        ///<Summary>
        /// Get Milestone Resource WebHook Callback Event
        ///</Summary>
        [HttpPost, Route("api/v1/callback/milestone")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public IHttpActionResult MileStoneCallBack(ECallBack res)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(IntellaLendURLConstant.SET_MILESTONE_EVENT), Content = res, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Exception ex = new Exception(responseStream);
                    MTSExceptionHandler.HandleException(ref ex);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest();
        }

    }
}
