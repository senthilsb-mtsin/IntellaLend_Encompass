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
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace EncompassWrapperAPI.Controllers
{
    [ApiController]
    [EnableCors("OpenPolicy")]
    [Route("api/[controller]")]
    public class EncompassLoanController : CustomControllerBase
    {
        #region Construtor

        private readonly IHttpClientFactory _clientFactory;

        private HttpClient _client;

        private readonly ILogger<EncompassLoanController> _logger;

        public EncompassLoanController(ILogger<EncompassLoanController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient(HttpClientFactoryConstant.RequestWithValidator);
        }

        #endregion

        #region Get Loan

        [HttpGet("GetLoan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(object))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetLoan(string loanGuid)
        {
            ErrorResponse _error = new ErrorResponse();
            string responseStream = string.Empty;
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN, loanGuid)).Result;

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

        [HttpPost("GetLoans")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EPipelineLoans>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetLoans(LoanRequest loanRequest)
        {
            ErrorResponse _error = new ErrorResponse();
            try
            {
                _logger.LogError(JsonConvert.SerializeObject(loanRequest));
                List<EPipelineLoans> _loans = new List<EPipelineLoans>();

                string responseStream = string.Empty;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                ELoansRequest _lReq = new ELoansRequest()
                {
                    Filter = new QueryFields()
                    {
                        Operator = EncompassFieldOperator.AND,
                        Terms = loanRequest.QueryFields
                    },
                    Fields = loanRequest.ReturnFields
                };

                _logger.LogError(JsonConvert.SerializeObject(_lReq));

                var response = _client.PostAsJsonAsync(string.Format(EncompassURLConstant.GET_LOANS, loanRequest.ReturnLoanLimit), _lReq).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _loans = JsonConvert.DeserializeObject<List<EPipelineLoans>>(responseStream);
                    return Ok(_loans);
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _error = new ErrorResponse() { Summary = "Unauthorized", ErrorCode = HttpStatusCode.Unauthorized.ToString(), Details = "" };
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

        #region Lock Loan


        [HttpPost("LockLoan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(LockLoanResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult LockLoan(string loanGuid)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                LockResourceModel lockResource = LoanResource.LockLoan(_client, loanGuid);

                if (lockResource.Status)
                {
                    return Ok(new LockResponse() { LockID = lockResource.Message });
                }
                else
                {
                    _badRequest.Details = lockResource.Message;
                    _badRequest.Summary = ResponseConstant.ERROR;
                    _badRequest.ErrorCode = HttpStatusCode.Conflict.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _badRequest.Details = ResponseConstant.LOCK_FAILED;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Summary = ResponseConstant.ERROR;
            }

            return BadRequest(_badRequest);
        }

        [HttpPost("UnLockLoan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult UnLockLoan(string LockID, string LoanGUID)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                LockResourceModel lockResource = LoanResource.UnLockLoan(_client, LockID, LoanGUID);

                if (lockResource.Status)
                {
                    return Ok(lockResource.Status);
                }
                else
                {
                    _badRequest.Details = lockResource.Message;
                    _badRequest.Summary = ResponseConstant.ERROR;
                    _badRequest.ErrorCode = HttpStatusCode.Conflict.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _badRequest.Details = ResponseConstant.UNLOCKED_FAILED;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Summary = ResponseConstant.ERROR;
            }

            return BadRequest(_badRequest);
        }

        #endregion

        #region Update Loan Custom Field

        [HttpPatch("UpdateLoanCustomField")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult UpdateLoanCustomField(UpdateCustomFieldRequest req)
        {
            ErrorResponse _badRes = new ErrorResponse();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);
            LockResourceModel lockResource = null;
            string responseStream = string.Empty;
            try
            {
                lockResource = LoanResource.LockLoan(_client, req.LoanGuid);

                if (lockResource.Status)
                {
                    List<EFieldUpdate> _fields = new List<EFieldUpdate>();
                    foreach (string item in req.Fields.Keys)
                    {
                        _fields.Add(new EFieldUpdate()
                        {

                            FieldName = item,
                            StringValue = req.Fields[item]
                        });
                    }

                    ECustomFieldUpdateRequest _updateCustomFieldRequest = new ECustomFieldUpdateRequest()
                    {
                        CustomFields = _fields
                    };

                    _logger.LogError(JsonConvert.SerializeObject(_updateCustomFieldRequest));
                    var response = _client.PatchAsync(string.Format(EncompassURLConstant.UPDATE_CUSTOM_FIELD, req.LoanGuid), new ObjectContent(typeof(ECustomFieldUpdateRequest), _updateCustomFieldRequest, new JsonMediaTypeFormatter())).Result;

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created)
                    {
                        responseStream = response.Content.ReadAsStringAsync().Result;
                        return Ok("Success");
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
                    LoanResource.UnLockLoan(_client, lockResource.Message, req.LoanGuid);
                }
            }

            return BadRequest(_badRes);
        }

        #endregion
    }
}
