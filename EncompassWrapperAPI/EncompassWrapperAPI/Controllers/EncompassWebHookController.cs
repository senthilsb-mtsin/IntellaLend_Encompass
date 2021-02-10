using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperConstants;
using MTS.Web.Helpers;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using RestSharp;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace EncompassConnectorAPI.Controllers
{
    ///<Summary>
    /// Encompass WebHook Callback Releated Activities
    ///</Summary>
    public class EncompassWebHookController : BaseController
    {
        #region Constructure 


        #endregion



        ///<Summary>
        /// Create new Webhook Subscription
        ///</Summary>
        [HttpGet, Route("api/v1/webhook")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<WebHookSubscriptions>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public IHttpActionResult GetAllSubscription()
        {
            Logger.WriteTraceLog($"GetAllSubscription");
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_ALL_WEBHOOK_SUBSCIPTIONS), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content; Logger.WriteTraceLog($"response.Content : {response.Content}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(JsonConvert.DeserializeObject<List<WebHookSubscriptions>>(responseStream));
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
                else
                {
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteTraceLog($"ex  : {ex.Message}");
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }



        ///<Summary>
        /// Create new Webhook Subscription
        ///</Summary>
        [HttpPost, Route("api/v1/webhook/create")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public IHttpActionResult CreateWebHook(WebHookRequestModel res)
        {
            Logger.WriteTraceLog($"CreateWebHook req : {JsonConvert.SerializeObject(res)}");
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.CREATE_WEBHOOK_SUBSCIPTION), Content = res, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content; Logger.WriteTraceLog($"response.Content : {response.Content}");

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    return Ok(responseStream);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
                else
                {
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteTraceLog($"ex  : {ex.Message}");
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }


        ///<Summary>
        /// Delete Subscribed Webhook
        ///</Summary>
        [HttpDelete, Route("api/v1/webhook/delete/{subscriptionId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public IHttpActionResult DeleteWebHook(string subscriptionId)
        {
            Logger.WriteTraceLog($"DeleteWebHook req : {subscriptionId}");
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.DELETE_WEBHOOK_SUBSCIPTION, subscriptionId), REQUESTTYPE = HeaderConstant.DELETE };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content; Logger.WriteTraceLog($"response.Content : {response.Content}");

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
                {
                    return Ok("Deleted Successfully");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
                else
                {
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteTraceLog($"ex  : {ex.Message}");
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

    }
}
