using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
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
    /// Encompass Loan Releated Activities
    ///</Summary>
    public class EncompassLoanController : BaseController
    {

        #region Get Loan

        ///<Summary>
        /// Get Full Encompass Loan Objects
        ///</Summary>
        [HttpGet, Route("api/GetLoan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(object))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetLoan(string loanGuid)
        {
            ErrorResponse _error = new ErrorResponse();
            string responseStream = string.Empty;
            try
            {
                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN, loanGuid), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseStream = response.Content;
                    return Ok(responseStream);
                }
                else
                {
                    _error = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                _error.Summary = ResponseConstant.ERROR;
                _error.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _error.Details = ex.Message;
            }

            return BadRequest(JsonConvert.SerializeObject(_error));
        }

        ///<Summary>
        /// Get List of LoanGUID's from Encompass PipeLine with a Filter
        ///</Summary>
        [HttpPost, Route("api/QueryPipeLine")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EPipelineLoans>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult QueryPipeLine(LoanRequest loanRequest)
        {
            ErrorResponse _error = new ErrorResponse();
            try
            {
                Logger.WriteTraceLog(JsonConvert.SerializeObject(loanRequest));
                List<EPipelineLoans> _loans = new List<EPipelineLoans>();

                string responseStream = string.Empty;

                EPipelineLoanRequest _lReq = new EPipelineLoanRequest()
                {
                    Filter = new QueryFields()
                    {
                        Operator = EncompassFieldOperator.AND,
                        Terms = loanRequest.QueryFields
                    },
                    Fields = loanRequest.ReturnFields
                };

                Logger.WriteTraceLog(JsonConvert.SerializeObject(_lReq));

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN, loanRequest.ReturnLoanLimit), Content = _lReq, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                //var response = base._client.PostAsJsonAsync(string.Format(EncompassURLConstant.GET_LOANS, loanRequest.ReturnLoanLimit), _lReq).Result;

                responseStream = response.Content;

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
                MTSExceptionHandler.HandleException(ref ex);
                _error.Summary = ResponseConstant.ERROR;
                _error.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _error.Details = ex.Message;
            }

            return BadRequest(JsonConvert.SerializeObject(_error));
        }

        ///<Summary>
        /// Get Loans with Requested Fields
        ///</Summary>
        [HttpPost, Route("api/GetLoans")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EPipelineLoans>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetLoans(GetLoanRequest loanRequest)
        {
            ErrorResponse _error = new ErrorResponse();
            try
            {
                Logger.WriteTraceLog(JsonConvert.SerializeObject(loanRequest));
                List<EPipelineLoans> _loans = new List<EPipelineLoans>();

                string responseStream = string.Empty;

                ELoansRequest _lReq = new ELoansRequest()
                {
                    ReturnFields = loanRequest.ReturnFields,
                    LoanGUIDs = loanRequest.LoanGUIDs
                };

                Logger.WriteTraceLog(JsonConvert.SerializeObject(_lReq));

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN, loanRequest.ReturnLoanLimit), Content = _lReq, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

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
                MTSExceptionHandler.HandleException(ref ex);
                _error.Summary = ResponseConstant.ERROR;
                _error.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _error.Details = ex.Message;
            }

            return BadRequest(JsonConvert.SerializeObject(_error));
        }

        #endregion

        #region Lock Loan

        ///<Summary>
        /// Lock Encompass Loan
        ///</Summary>
        [HttpPost, Route("api/LockLoan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(LockLoanResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult LockLoan(string loanGuid)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
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
                MTSExceptionHandler.HandleException(ref ex);
                _badRequest.Details = ResponseConstant.LOCK_FAILED;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Summary = ResponseConstant.ERROR;
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        ///<Summary>
        /// UnLock Encompass Loan
        ///</Summary>
        [HttpPost, Route("api/UnLockLoan")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult UnLockLoan(string LockID, string LoanGUID)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

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
                MTSExceptionHandler.HandleException(ref ex);
                _badRequest.Details = ResponseConstant.UNLOCKED_FAILED;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
                _badRequest.Summary = ResponseConstant.ERROR;
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        #endregion

        #region Update Loan Custom Field

        ///<Summary>
        /// Update Loan Custom Field
        ///</Summary>
        [HttpPatch, Route("api/UpdateLoanCustomField")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult UpdateLoanCustomField(UpdateCustomFieldRequest req)
        {
            ErrorResponse _badRes = new ErrorResponse();


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

                    Logger.WriteTraceLog(JsonConvert.SerializeObject(_updateCustomFieldRequest));

                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.UPDATE_CUSTOM_FIELD, req.LoanGuid), Content = _updateCustomFieldRequest, REQUESTTYPE = HeaderConstant.PATCH };

                    IRestResponse response = _client.Execute(reqObj);

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.Created)
                    {
                        responseStream = response.Content;
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
                MTSExceptionHandler.HandleException(ref ex);
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, req.LoanGuid);
                }
            }

            return BadRequest(JsonConvert.SerializeObject(_badRes));
        }

        #endregion
    }
}
