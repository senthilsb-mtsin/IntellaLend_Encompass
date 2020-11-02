using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
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
using System.Net.Http.Headers;

namespace EncompassWrapperAPI.Controllers
{
    [ApiController]
    [EnableCors("OpenPolicy")]
    [Route("api/[controller]")]
    public class EncompassDocumentController : CustomControllerBase
    {
        #region Construtor

        private readonly IHttpClientFactory _clientFactory;

        private HttpClient _client;

        private readonly ILogger<EncompassDocumentController> _logger;

        public EncompassDocumentController(ILogger<EncompassDocumentController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient(HttpClientFactoryConstant.RequestWithValidator);
        }

        #endregion

        #region Get Document

        [HttpGet("GetAllLoanDocuments")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EContainer>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetAllLoanDocuments(string loanGuid)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN_ALL_DOCUMENTS, loanGuid)).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    List<EContainer> _docs = JsonConvert.DeserializeObject<List<EContainer>>(responseStream);
                    return Ok(_docs);
                }
                else
                {
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRequest);
        }

        [HttpGet("GetLoanDocument")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EContainer))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetLoanDocument(string loanGuid, string documentID)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN_DOCUMENT, loanGuid, documentID)).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    EContainer _docs = JsonConvert.DeserializeObject<EContainer>(responseStream);
                    return Ok(_docs);
                }
                else
                {
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRequest);
        }


        #endregion

        #region Add Document

        [HttpPost("AddDocument")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(AddContainerResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult AddDocument(AddContainerRequest request)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

            LockResourceModel lockResource = null;

            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                lockResource = LoanResource.LockLoan(_client, request.LoanGUID);

                if (lockResource.Status)
                {
                    string responseStream = string.Empty;

                    EAddContainerRequest _addReq = new EAddContainerRequest() { Title = request.DocumentName, ApplicationId = EncompassConstant.ApplicationID };

                    var response = _client.PostAsJsonAsync(string.Format(EncompassURLConstant.ADD_DOCUMENT, request.LoanGUID), _addReq).Result;

                    responseStream = response.Content.ReadAsStringAsync().Result;

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        EIDResponse eID = JsonConvert.DeserializeObject<EIDResponse>(responseStream);
                        return Ok(new AddContainerResponse() { DocumentID = eID.ID });
                    }
                    else
                    {
                        _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                    }
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
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Details = ex.Message;
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, request.LoanGUID);
                }
            }
            return BadRequest(_badRequest);
        }

        #endregion

        #region Private Methods


        #endregion

    }
}
