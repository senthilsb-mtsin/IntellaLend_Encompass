using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperConstants;
using MTS.Web.Helpers;
using MTSEntBlocks.ExceptionBlock.Handlers;
using Newtonsoft.Json;
using RestSharp;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace EncompassWrapperAPI.Controllers
{
    ///<Summary>
    /// Controller to Generate, Validate Encompass Token
    ///</Summary>
    public class EncompassTokenController : ApiController
    {
        #region Constructure 

        private readonly RestWebClient _client;

        ///<Summary>
        /// To Create Rest Client Instance
        ///</Summary>
        public EncompassTokenController()
        {
            _client = new RestWebClient(ConfigurationManager.AppSettings["EncompassURL"]);
        }

        #endregion

        #region Token Validate & Get New Token


        ///<Summary>
        /// To Validate Encompass Token       
        ///</Summary>
        [HttpPost, Route("api/Token/ValidateToken")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ETokenValidateResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult ValidateToken(TokenValidateRequest validateRequest)
        {
            ErrorResponse _badRes = new ErrorResponse();
            ETokenValidateResponse _eToken = new ETokenValidateResponse();
            try
            {
                _eToken.ValidToken = true;

                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("client_id", validateRequest.ClientID);
                form.Add("client_secret", validateRequest.ClientSecret);
                form.Add("token", validateRequest.AccessToken);

                var reqObj = new HttpRequestObject() { URL = EncompassURLConstant.TOKEN_INTROSPECTION, RequestContentType = RequestTypeConstant.FORMURLENCODE, Content = form, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseStream = response.Content;

                    TokenInvalidModel _invalidToken = JsonConvert.DeserializeObject<TokenInvalidModel>(responseStream);

                    _eToken.ValidToken = false;
                }

                return Ok(_eToken);
            }
            catch (Exception ex)
            {
                _badRes.Summary = ResponseConstant.ERROR;
                _badRes.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRes));
        }

        ///<Summary>
        /// Get Encompass Token without User credentials    
        ///</Summary>
        [HttpPost, Route("api/Token/GetToken")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ETokenResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetToken(ETokenRequest tokenRequest)
        {
            ErrorResponse _badRes = new ErrorResponse();
            ETokenResponse _eToken = new ETokenResponse();
            try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("client_id", tokenRequest.ClientID);
                form.Add("client_secret", tokenRequest.ClientSecret);
                form.Add("grant_type", tokenRequest.GrantType);
                form.Add("scope", tokenRequest.Scope);
                form.Add("instance_id", tokenRequest.InstanceID);

                var reqObj = new HttpRequestObject() { URL = EncompassURLConstant.GET_TOKEN, RequestContentType = RequestTypeConstant.FORMURLENCODE, Content = form, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                string responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eToken = JsonConvert.DeserializeObject<ETokenResponse>(responseStream);
                    return Ok(_eToken);
                }
                else
                {
                    TokenInvalidModel res = JsonConvert.DeserializeObject<TokenInvalidModel>(responseStream);
                    throw new Exception(res.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                _badRes.Summary = ResponseConstant.ERROR;
                _badRes.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRes));
        }

        ///<Summary>
        /// Get Encompass Token with User credentials    
        ///</Summary>
        [HttpPost, Route("api/Token/GetTokenWithUser")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ETokenResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetTokenWithUser(ETokenUserRequest tokenRequest)
        {
            ErrorResponse _badRes = new ErrorResponse();
            ETokenResponse _eToken = new ETokenResponse();
            try
            {
                Dictionary<string, string> form = new Dictionary<string, string>();
                form.Add("client_id", tokenRequest.ClientID);
                form.Add("client_secret", tokenRequest.ClientSecret);
                form.Add("grant_type", tokenRequest.GrantType);
                form.Add("scope", tokenRequest.Scope);
                form.Add("username", tokenRequest.UserName);
                form.Add("password", tokenRequest.Password);

                var obj = new { client_id = tokenRequest.ClientID, client_secret = tokenRequest.ClientSecret, grant_type = tokenRequest.GrantType, scope = tokenRequest.Scope, username = tokenRequest.UserName, password = tokenRequest.Password };

                var reqObj = new HttpRequestObject() { URL = EncompassURLConstant.GET_TOKEN, RequestContentType = RequestTypeConstant.FORMURLENCODE, Content = form, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                string responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eToken = JsonConvert.DeserializeObject<ETokenResponse>(responseStream);
                    return Ok(_eToken);
                }
                else
                {
                    TokenInvalidModel res = JsonConvert.DeserializeObject<TokenInvalidModel>(responseStream);
                    throw new Exception(res.ErrorDescription);
                }
            }
            catch (Exception ex)
            {
                _badRes.Summary = ResponseConstant.ERROR;
                _badRes.Details = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRes));
        }

        #endregion

        #region Private Methods


        #endregion

    }
}
