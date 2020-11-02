using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperAPI.Helper;
using EncompassWrapperConstants;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace EncompassWrapperAPI.Controllers
{
    [ApiController]
    [EnableCors("OpenPolicy")]
    [Route("api/[controller]")]
    public class EncompassTokenController : Controller
    {
        #region Construtor

        private readonly IHttpClientFactory _clientFactory;

        private HttpClient _client;

        private readonly ILogger<EncompassTokenController> _logger;

        public EncompassTokenController(ILogger<EncompassTokenController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient(HttpClientFactoryConstant.RequestWithoutValidator);
        }

        #endregion

        #region Token Validate & Get New Token

        [HttpPost("ValidateToken")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ETokenValidateResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult ValidateToken(TokenValidateRequest validateRequest)
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

                var response = _client.PostAsync(EncompassURLConstant.TOKEN_INTROSPECTION, new FormUrlEncodedContent(form)).Result;

                if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string responseStream = response.Content.ReadAsStringAsync().Result;

                    TokenInvalidModel _invalidToken = JsonConvert.DeserializeObject<TokenInvalidModel>(responseStream);

                    _eToken.ValidToken = false;
                }

                return Ok(_eToken);
            }
            catch (Exception ex)
            {
                _badRes.Summary = ResponseConstant.ERROR;
                _badRes.Details = ex.Message;
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRes);
        }

        [HttpPost("GetToken")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ETokenResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetToken(ETokenRequest tokenRequest)
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

                var response = _client.PostAsync(EncompassURLConstant.GET_TOKEN, new FormUrlEncodedContent(form)).Result;

                string responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
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
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRes);
        }


        [HttpPost("GetTokenWithUser")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ETokenResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetTokenWithUser(ETokenUserRequest tokenRequest)
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

                var response = _client.PostAsync(EncompassURLConstant.GET_TOKEN, new FormUrlEncodedContent(form)).Result;

                string responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
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
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRes);
        }

        #endregion

        #region Private Methods


        #endregion

    }
}
