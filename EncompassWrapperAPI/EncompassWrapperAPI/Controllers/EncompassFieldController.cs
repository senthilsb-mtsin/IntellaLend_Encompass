using EncompassRequestBody.CustomModel;
using EncompassRequestBody.EResponseModel;
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
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace EncompassWrapperAPI.Controllers
{
    [ApiController]
    [EnableCors("OpenPolicy")]
    [Route("api/[controller]")]
    public class EncompassFieldController : CustomControllerBase
    {
        #region Construtor

        private readonly IHttpClientFactory _clientFactory;

        private HttpClient _client;

        private readonly ILogger<EncompassFieldController> _logger;

        public EncompassFieldController(ILogger<EncompassFieldController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient(HttpClientFactoryConstant.RequestWithValidator);
        }

        #endregion

        #region Get Field 

        [HttpPost("GetFieldSchema")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(object))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetFieldSchema(string[] FieldIDs)
        {
            ErrorResponse _error = new ErrorResponse();
            string responseStream = string.Empty;
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                Dictionary<string, string> fields = new Dictionary<string, string>();

                foreach (string item in FieldIDs)
                {
                    fields[item] = "";
                }

                var response = _client.PostAsJsonAsync(string.Format(EncompassURLConstant.GET_FIELD_SCHEMA), fields).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseStream = response.Content.ReadAsStringAsync().Result;
                    return Ok(responseStream);
                }
                else
                {
                    _error = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _error.Summary = ResponseConstant.ERROR;
                _error.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _error.Details = ex.Message;
            }

            return BadRequest(_error);
        }

        [HttpPost("GetPreDefinedFieldValues")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EFieldResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetPreDefinedFieldValues(FieldGetRequest _res)
        {
            ErrorResponse _error = new ErrorResponse();
            string responseStream = string.Empty;
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.PostAsJsonAsync(string.Format(EncompassURLConstant.GET_PREDEFINED_FIELD_VALUE, _res.LoanGUID), _res.FieldIDs).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseStream = response.Content.ReadAsStringAsync().Result;
                    return Ok(JsonConvert.DeserializeObject<List<EFieldResponse>>(responseStream));
                }
                else
                {
                    _error = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _error.Summary = ResponseConstant.ERROR;
                _error.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _error.Details = ex.Message;
            }

            return BadRequest(_error);
        }

        #endregion

        #region Update Loan Field

        [HttpPatch("UpdatePredefinedFields")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EIDResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult UpdatePredefinedFields(FieldUpdateRequest req)
        {
            ErrorResponse _badRes = new ErrorResponse();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

            LockResourceModel lockResource = null;

            string responseStream = string.Empty;
            try
            {
                lockResource = LoanResource.LockLoan(_client, req.LoanGUID);

                if (lockResource.Status)
                {
                    var response = _client.PatchAsync(string.Format(EncompassURLConstant.UPDATE_LOAN_FIELD, req.LoanGUID), new ObjectContent(typeof(object), req.FieldSchemas, new JsonMediaTypeFormatter())).Result;

                    responseStream = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        EIDResponse _loans = JsonConvert.DeserializeObject<EIDResponse>(responseStream);
                        return Ok(_loans);
                    }
                    else
                    {
                        _badRes = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                    }
                }
                else
                {
                    _badRes.Details = lockResource.Message;
                    _badRes.Summary = ResponseConstant.ERROR;
                    _badRes.ErrorCode = HttpStatusCode.Conflict.ToString();
                }
            }
            catch (Exception ex)
            {
                _badRes.Summary = ResponseConstant.ERROR;
                _badRes.Details = ex.Message;
                _badRes.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, req.LoanGUID);
                }
            }

            return BadRequest(_badRes);
        }

        #endregion
    }
}
