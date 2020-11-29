using EncompassRequestBody.CustomModel;
using EncompassRequestBody.EResponseModel;
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
using System.Net;
using System.Web.Http;

namespace EncompassConnectorAPI.Controllers
{
    ///<Summary>
    /// Encompass Field Releated Activities
    ///</Summary>
    public class EncompassFieldController : BaseController
    {

        #region Get Field 

        ///<Summary>
        /// Get Encompass Field Schema
        ///</Summary>
        [HttpPost, Route("api/GetFieldSchema")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(object))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetFieldSchema(string[] FieldIDs)
        {
            ErrorResponse _error = new ErrorResponse();
            string responseStream = string.Empty;
            try
            {

                Dictionary<string, string> fields = new Dictionary<string, string>();

                foreach (string item in FieldIDs)
                {
                    fields[item] = "";
                }

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_FIELD_SCHEMA), Content = fields, REQUESTTYPE = HeaderConstant.POST };

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
        /// Get Encompass PreDefined Field Values
        ///</Summary>
        [HttpPost, Route("api/GetPreDefinedFieldValues")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EFieldResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetPreDefinedFieldValues(FieldGetRequest _res)
        {
            ErrorResponse _error = new ErrorResponse();
            string responseStream = string.Empty;
            try
            {

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_PREDEFINED_FIELD_VALUE, _res.LoanGUID), Content = _res.FieldIDs, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    responseStream = response.Content;
                    return Ok(JsonConvert.DeserializeObject<List<EFieldResponse>>(responseStream));
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

        #region Update Loan Field

        ///<Summary>
        /// Update Encompass PreDefined Field Values
        ///</Summary>
        [HttpPatch, Route("api/UpdatePredefinedFields")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EIDResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult UpdatePredefinedFields(FieldUpdateRequest req)
        {
            ErrorResponse _badRes = new ErrorResponse();



            LockResourceModel lockResource = null;

            string responseStream = string.Empty;
            try
            {
                lockResource = LoanResource.LockLoan(_client, req.LoanGUID);

                if (lockResource.Status)
                {
                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.UPDATE_LOAN_FIELD, req.LoanGUID), Content = req.FieldSchemas, REQUESTTYPE = HeaderConstant.PATCH };

                    IRestResponse response = _client.Execute(reqObj);

                    responseStream = response.Content;

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
                MTSExceptionHandler.HandleException(ref ex);
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, req.LoanGUID);
                }
            }

            return BadRequest(JsonConvert.SerializeObject(_badRes));
        }

        #endregion
    }
}
