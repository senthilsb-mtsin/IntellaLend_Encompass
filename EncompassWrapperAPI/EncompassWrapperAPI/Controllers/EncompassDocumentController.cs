using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.EResponseModel;
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
using System.Linq;
using System.Net;
using System.Web.Http;

namespace EncompassConnectorAPI.Controllers
{
    ///<Summary>
    /// Encompass Document Releated Activities
    ///</Summary>
    public class EncompassDocumentController : BaseController
    {

        #region Get Document

        ///<Summary>
        /// Get All Documents of a Loan with or without Attachments
        ///</Summary>
        [HttpGet, Route("api/v1/documents")]
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
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
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
        [HttpGet, Route("api/v1/document")]
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
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
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
        /// Get All Documents of a Loan with Attachments
        ///</Summary>
        [HttpGet, Route("api/v1/document/attachments")]
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
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
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
        /// Get Encompass Loan Attachments of a Document
        ///</Summary>
        [HttpGet, Route("api/v1/document/attachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EDocumentAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetDocumentAttachment(string loanGuid, string documentID)
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

                    EContainer doc = _docs.Where(x => x.DocumentId == documentID).FirstOrDefault();

                    if (doc != null)
                    {
                        return Ok(doc.Attachments);
                    }
                    else
                        throw new Exception($"Document '{documentID}' not found");
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
        [HttpPatch, Route("api/v1/document/create")]
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
                        EIDResponse eIDs = JsonConvert.DeserializeObject<EIDResponse>(responseStream);
                        List<AddContainerResponse> _res = new List<AddContainerResponse>();
                        //foreach (var item in eIDs)
                        _res.Add(new AddContainerResponse() { DocumentID = eIDs.ID, DocumentName = eIDs.Title });

                        return Ok(_res);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Unauthorized();
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
        [HttpPatch, Route("api/v1/document/delete")]
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
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Unauthorized();
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

        #region Assign Attachment to Document

        ///<Summary>
        /// Remove Attachment from a Document
        ///</Summary>
        [HttpPatch, Route("api/v1/document/attachment/remove")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EAddRemoveAttachmentResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult RemoveDocumentAttachment(AssignAttachmentRequest _req)
        {


            LockResourceModel lockResource = null;
            ErrorResponse _eRequest = new ErrorResponse();
            try
            {

                lockResource = LoanResource.LockLoan(_client, _req.LoanGUID);

                if (lockResource.Status)
                {
                    string responseStream = string.Empty;

                    List<EAddRemoveAttachment> _eAddRemove = new List<EAddRemoveAttachment>();

                    foreach (string attachmentGUID in _req.AttachmentGUIDs)
                    {
                        _eAddRemove.Add(new EAddRemoveAttachment() { EntityId = attachmentGUID, EntityType = EncompassEntityType.ATTACHMENT });
                    }
                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.REMOVE_DOCUMENT_ATTACHMENT, _req.LoanGUID, _req.DocumentGUID), Content = _eAddRemove, REQUESTTYPE = HeaderConstant.PATCH };

                    IRestResponse response = _client.Execute(reqObj);

                    //var response = base._client.PatchAsync(string.Format(EncompassURLConstant.REMOVE_DOCUMENT_ATTACHMENT, _req.LoanGUID, _req.DocumentGUID), new ObjectContent(typeof(List<EAddRemoveAttachment>), _eAddRemove, new JsonMediaTypeFormatter())).Result;

                    responseStream = response.Content;

                    if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    {
                        return Ok(new EAddRemoveAttachmentResponse() { Status = true, Message = ResponseConstant.REMOVED_SUCCESSFULLY });
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Unauthorized();
                    else
                    {
                        _eRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                    }
                }
                else
                {
                    _eRequest.Details = lockResource.Message;
                    _eRequest.Summary = ResponseConstant.ERROR;
                    _eRequest.ErrorCode = HttpStatusCode.Conflict.ToString();
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                _eRequest.Summary = ResponseConstant.ERROR;
                _eRequest.Details = ex.Message;
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, _req.LoanGUID);
                }
            }

            return BadRequest(JsonConvert.SerializeObject(_eRequest));
        }

        ///<Summary>
        /// Assign Attachment to a Document
        ///</Summary>
        [HttpPatch, Route("api/v1/document/attachment/assign")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EAddRemoveAttachmentResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult AssignDocumentAttachment(AssignAttachmentRequest _req)
        {


            LockResourceModel lockResource = null;
            ErrorResponse _eRequest = new ErrorResponse();
            try
            {
                Logger.WriteTraceLog(JsonConvert.SerializeObject(_req));
                lockResource = LoanResource.LockLoan(_client, _req.LoanGUID);

                if (lockResource.Status)
                {
                    string responseStream = string.Empty;

                    List<EAddRemoveAttachment> _eAddRemove = new List<EAddRemoveAttachment>();

                    foreach (string attachmentGUID in _req.AttachmentGUIDs)
                    {
                        _eAddRemove.Add(new EAddRemoveAttachment() { EntityId = attachmentGUID, EntityType = EncompassEntityType.ATTACHMENT });
                    }
                    Logger.WriteTraceLog(JsonConvert.SerializeObject(_eAddRemove));

                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.ADD_DOCUMENT_ATTACHMENT, _req.LoanGUID, _req.DocumentGUID), Content = _eAddRemove, REQUESTTYPE = HeaderConstant.PATCH };

                    IRestResponse response = _client.Execute(reqObj);

                    //var response = base._client.PatchAsync(string.Format(EncompassURLConstant.ADD_DOCUMENT_ATTACHMENT, _req.LoanGUID, _req.DocumentGUID), new ObjectContent(typeof(List<EAddRemoveAttachment>), _eAddRemove, new JsonMediaTypeFormatter())).Result;

                    responseStream = response.Content;

                    if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    {
                        return Ok(new EAddRemoveAttachmentResponse() { Status = true, Message = ResponseConstant.ASSIGNED_SUCCESSFULLY });
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Unauthorized();
                    else
                    {
                        _eRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                    }
                }
                else
                {
                    _eRequest.Details = lockResource.Message;
                    _eRequest.Summary = ResponseConstant.ERROR;
                    _eRequest.ErrorCode = HttpStatusCode.Conflict.ToString();
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                _eRequest.Summary = ResponseConstant.ERROR;
                _eRequest.Details = ex.Message;
            }
            finally
            {
                if (lockResource != null && lockResource.Status)
                {
                    LoanResource.UnLockLoan(_client, lockResource.Message, _req.LoanGUID);
                }
            }

            return BadRequest(JsonConvert.SerializeObject(_eRequest));
        }

        #endregion

        #region Private Methods


        #endregion

    }
}
