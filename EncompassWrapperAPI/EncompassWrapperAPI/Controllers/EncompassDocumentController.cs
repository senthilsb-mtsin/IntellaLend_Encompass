using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
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

namespace EncompassWrapperAPI.Controllers
{
    ///<Summary>
    /// Encompass Document Releated Activities
    ///</Summary>
    public class EncompassDocumentController : BaseController
    {

        #region Get Document

        ///<Summary>
        /// Get All Documents of a Loan with Attachments
        ///</Summary>
        [HttpGet, Route("api/GetAllLoanDocumentWithAttachments")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EContainer>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetAllLoanDocumentWithAttachments(string loanGuid)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_ALL_DOCUMENT_WITH_ATTACHMENTS, loanGuid), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

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
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        ///<Summary>
        /// Get All Documents of a Loan with or without Attachments
        ///</Summary>
        [HttpGet, Route("api/GetAllLoanDocuments")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EContainer>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetAllLoanDocuments(string loanGuid)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_ALL_DOCUMENTS, loanGuid), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

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
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        ///<Summary>
        /// Get Particular Document of a Loan with or without Attachments
        ///</Summary>
        [HttpGet, Route("api/GetLoanDocument")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EContainer))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetLoanDocument(string loanGuid, string documentID)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_DOCUMENT, loanGuid, documentID), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

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
                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }


        #endregion

        #region Add Document

        ///<Summary>
        /// Add Empty Document to Encompass Loan E-Folder
        ///</Summary>
        [HttpPatch, Route("api/AddDocument")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<AddContainerResponse>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult AddDocument(AddContainerRequest request)
        {
            LockResourceModel lockResource = null;

            ErrorResponse _badRequest = new ErrorResponse();
            try
            {

                if (request.Documents.Count == 0)
                    throw new Exception("Request does not have Documents");

                lockResource = LoanResource.LockLoan(_client, request.LoanGUID);

                if (lockResource.Status)
                {
                    string responseStream = string.Empty;

                    List<EAddContainerRequest> _addReq = new List<EAddContainerRequest>();

                    foreach (EAddDocument item in request.Documents)
                        _addReq.Add(new EAddContainerRequest() { title = item.DocumentName, description = item.DocumentDescription });

                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.ADD_DOCUMENT, request.LoanGUID, lockResource.Message), Content = _addReq, REQUESTTYPE = HeaderConstant.PATCH };

                    IRestResponse response = _client.Execute(reqObj);

                    responseStream = response.Content;

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        List<EIDResponse> eIDs = JsonConvert.DeserializeObject<List<EIDResponse>>(responseStream);
                        List<AddContainerResponse> _res = new List<AddContainerResponse>();
                        foreach (var item in eIDs)
                            _res.Add(new AddContainerResponse() { DocumentID = item.ID, DocumentName = item.Title });

                        return Ok(_res);
                    }
                    else
                        _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
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
                MTSExceptionHandler.HandleException(ref ex);
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, request.LoanGUID);
                }
            }
            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        ///<Summary>
        /// Remove Document from Encompass Loan E-Folder
        ///</Summary>
        [HttpPatch, Route("api/RemoveDocument")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult RemoveDocument(RemoveContainerRequest request)
        {
            LockResourceModel lockResource = null;

            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                if (request.Documents.Count == 0)
                    throw new Exception("Request does not have Documents");

                lockResource = LoanResource.LockLoan(_client, request.LoanGUID);

                if (lockResource.Status)
                {
                    string responseStream = string.Empty;

                    List<ERemoveContainerRequest> _removeReq = new List<ERemoveContainerRequest>();

                    foreach (ERemoveDocument item in request.Documents)
                        _removeReq.Add(new ERemoveContainerRequest() { id = item.DocumentID });

                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.REMOVE_DOCUMENT, request.LoanGUID, lockResource.Message), Content = _removeReq, REQUESTTYPE = HeaderConstant.PATCH };

                    IRestResponse response = _client.Execute(reqObj);

                    responseStream = response.Content;

                    if (response.StatusCode == HttpStatusCode.NoContent)
                        return Ok("Documents Removed");
                    else
                        _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
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
                MTSExceptionHandler.HandleException(ref ex);
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, request.LoanGUID);
                }
            }
            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        #endregion

        #region Private Methods


        #endregion

    }
}
