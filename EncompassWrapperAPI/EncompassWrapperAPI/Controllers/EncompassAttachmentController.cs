﻿using EncompassRequestBody.CustomModel;
using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperConstants;
using Microsoft.AspNetCore.Mvc;
using MTS.Web.Helpers;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using RestSharp;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPatchAttribute = System.Web.Http.HttpPatchAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace EncompassWrapperAPI.Controllers
{
    ///<Summary>
    /// Encompass Loan Attachment Releated Activities
    ///</Summary>
    public class EncompassAttachmentController : BaseController
    {

        #region Get Attachments

        ///<Summary>
        /// Get Encompass Loan Attachment which has Document assigned to it
        ///</Summary>
        [HttpGet, Route("api/GetAssignedLoanAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetAssignedLoanAttachment(string loanGuid)
        {
            ErrorResponse _badRes = new ErrorResponse();
            List<EAttachment> _eAttachments = new List<EAttachment>();
            try
            {
                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENTS, loanGuid), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                string responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eAttachments = JsonConvert.DeserializeObject<List<EAttachment>>(responseStream);

                    _eAttachments = _eAttachments.Where(a => a.Document != null).ToList();

                    return Ok(_eAttachments);
                }
                else
                {
                    _badRes = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
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
        /// Get Encompass Loan Attachment which do not have Document assigned
        ///</Summary>
        [HttpGet, Route("api/GetUnassginedLoanAttachments")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetUnassginedLoanAttachments(string loanGuid)
        {
            ErrorResponse _badRes = new ErrorResponse();
            List<EAttachment> _eAttachments = new List<EAttachment>();
            try
            {

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENTS, loanGuid), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                string responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eAttachments = JsonConvert.DeserializeObject<List<EAttachment>>(responseStream);

                    _eAttachments = _eAttachments.Where(a => a.Document == null).ToList();

                    return Ok(_eAttachments);
                }
                else
                {
                    _badRes = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {

                MTSExceptionHandler.HandleException(ref ex);
            }

            return BadRequest(JsonConvert.SerializeObject(_badRes));
        }

        ///<Summary>
        /// Get Encompass Loan Attachments
        ///</Summary>
        [HttpGet, Route("api/GetAllLoanAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetAllLoanAttachment(string loanGuid)
        {
            ErrorResponse _badRes = new ErrorResponse();
            List<EAttachment> _eAttachments = new List<EAttachment>();
            try
            {

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENTS, loanGuid), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                string responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eAttachments = JsonConvert.DeserializeObject<List<EAttachment>>(responseStream);

                    return Ok(_eAttachments);
                }
                else
                {
                    _badRes = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
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
        /// Get Encompass Loan Attachments of a Document
        ///</Summary>
        [HttpGet, Route("api/GetDocumentAttachment")]
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
        /// Get Single Encompass Loan Attachment Properties
        ///</Summary>
        [HttpGet, Route("api/GetAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EAttachment))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult GetAttachment(string loanGuid, string attachmentId)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENT, loanGuid, attachmentId), REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    EAttachment _attachment = JsonConvert.DeserializeObject<EAttachment>(responseStream);
                    return Ok(_attachment);
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
        /// Delete Encompass Loan Attachment
        ///</Summary>
        [HttpPatch, Route("api/RemoveAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult RemoveAttachment(RemoveContainerRequest request)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                if (request.Documents.Count == 0)
                    throw new Exception("Request does not have AttachmentIDs");

                string responseStream = string.Empty;

                List<ERemoveAttachmentRequest> _removeReq = new List<ERemoveAttachmentRequest>();

                foreach (ERemoveDocument item in request.Documents)
                    _removeReq.Add(new ERemoveAttachmentRequest() { id = item.DocumentID });

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.REMOVE_LOAN_ATTACHMENT, request.LoanGUID), Content = _removeReq, REQUESTTYPE = HeaderConstant.PATCH };

                IRestResponse response = _client.Execute(reqObj);

                responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.NoContent)
                    return Ok("Attachments Removed");
                else
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
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

        #region Download Attachment

        ///<Summary>
        /// Download Encompass Loan Attachment
        ///</Summary>
        [HttpPost, Route("api/DownloadAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(System.Web.Mvc.FileContentResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult DownloadAttachment(DownloadAttachment _request)
        {
            string responseStream = string.Empty;
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {

                EExportAttachmentJob _exportJobReq = new EExportAttachmentJob()
                {
                    AnnotationSettings = new VisibilitySettings()
                    {
                        Visibility = new List<string>() { EncompassAccessLevel.PUBLIC, EncompassAccessLevel.PRIVATE, EncompassAccessLevel.INTERNAL }
                    },
                    Entities = new List<Entity>() {
                         new Entity()
                         {
                             EntityId = _request.attachmentID,
                             EntityType = EncompassEntityType.ATTACHMENT
                         }
                     },
                    Source = new Entity()
                    {
                        EntityId = _request.loanGuid,
                        EntityType = EncompassEntityType.LOAN
                    }
                };

                Logger.WriteTraceLog(JsonConvert.SerializeObject(_exportJobReq));

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.EXPORT_JOB_REQUEST), REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                //var response = base._client.PostAsJsonAsync(EncompassURLConstant.EXPORT_JOB_REQUEST, _exportJobReq).Result;

                responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
                {
                    EJobResponse _res = JsonConvert.DeserializeObject<EJobResponse>(responseStream);

                    if (_res != null && _res.JobId != string.Empty)
                    {
                        byte[] file = ExportQueuedJob(_client, _res);

                        if (file != null)
                        {
                            var result = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ByteArrayContent(file)
                            };
                            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = $"{_request.attachmentID}.pdf"
                            };
                            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                            return Ok(result);
                        }
                        else
                        {
                            _badRequest.Summary = "Empty File";
                            _badRequest.Details = "Error while querying Encompass for Attachment Job";
                            _badRequest.ErrorCode = ResponseConstant.ERROR;
                        }
                    }
                }
                else
                {
                    _badRequest = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                _badRequest.Details = ex.Message;
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        #endregion

        #region Upload Attachment

        ///<Summary>
        /// Upload Encompass Loan Attachment
        ///</Summary>
        [HttpPost, Route("api/UploadAttachment")]
        [Consumes("application/x-www-form-urlencoded")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EUploadResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult UploadAttachment([FromForm] UploadRequest request)
        {


            LockResourceModel lockResource = null;
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {

                lockResource = LoanResource.LockLoan(_client, request.LoanGUID);

                if (lockResource.Status)
                {
                    EUploadRequest _uploadReq = new EUploadRequest() { Title = request.FileName, FileWithExtension = request.FileNameWithExtension, CreateReason = 1 };

                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.UPLOAD_ATTACHMENT_REQUEST, request.LoanGUID), Content = _uploadReq, REQUESTTYPE = HeaderConstant.POST };

                    IRestResponse response = _client.Execute(reqObj);

                    //var response = base._client.PostAsJsonAsync(string.Format(EncompassURLConstant.UPLOAD_ATTACHMENT_REQUEST, request.LoanGUID), _uploadReq).Result;

                    string responseStream = response.Content;

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
                    {
                        EncompassAttachmentUploadURL _res = JsonConvert.DeserializeObject<EncompassAttachmentUploadURL>(responseStream);

                        if (_res != null)
                        {
                            if (request.File.Length > 0)
                            {
                                EUploadResponse eUpload = new EUploadResponse();
                                eUpload.Status = UploadAttachment(_client, _res, request);
                                eUpload.Message = eUpload.Status ? ResponseConstant.UPLOAD_SUCCESSFULLY : ResponseConstant.UPLOAD_FAILED;
                                eUpload.AttachmentGUID = _res.MediaUrl.Split('?')[0].Split(new string[] { "attachment-" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('.')[0];
                                return Ok(eUpload);
                            }
                            else
                            {
                                _badRequest.Summary = ResponseConstant.ATTACHMENT_NOT_FOUND;
                                _badRequest.Details = ResponseConstant.ATTACHMENT_NOT_FOUND_ERROR_MESSAGE;
                                _badRequest.ErrorCode = HttpStatusCode.NotFound.ToString();
                            }
                        }
                        else
                        {
                            _badRequest.Summary = ResponseConstant.ATTACHMENT_URL_ERROR;
                            _badRequest.Details = ResponseConstant.ATTACHMENT_URL_ERROR_MESSAGE;
                            _badRequest.ErrorCode = HttpStatusCode.NotFound.ToString();
                        }
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
        [HttpPatch, Route("api/RemoveDocumentAttachment")]
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
        [HttpPatch, Route("api/AssignDocumentAttachment")]
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

        private byte[] ExportQueuedJob(RestWebClient _client, EJobResponse _job)
        {

            GetFile:

            var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.EXPORT_JOB_REQUEST_STATUS, _job.JobId), REQUESTTYPE = HeaderConstant.GET };

            IRestResponse response = _client.Execute(reqObj);

            //var response = base._client.GetAsync(string.Format(EncompassURLConstant.EXPORT_JOB_REQUEST_STATUS, _job.JobId)).Result;

            string responseStream = response.Content;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                EJobResponse _res = JsonConvert.DeserializeObject<EJobResponse>(responseStream);

                if (_res.Status.ToLower().Equals(EncompassExportStatus.SUCCESS))
                {
                    RestWebClient _newClient = new RestWebClient(_res.File.EntityUri);
                    var reqObjNew = new HttpRequestObject() { Headers = new Dictionary<string, string>() { { "Authorization", $"{_res.File.AuthorizationHeader.Split(' ')[0]} {_res.File.AuthorizationHeader.Split(' ')[1]}" } }, REQUESTTYPE = HeaderConstant.GET };

                    dynamic result = _newClient.RequestAsync<dynamic>(reqObjNew).Result;
                    //base._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_res.File.AuthorizationHeader.Split(" ")[0], _res.File.AuthorizationHeader.Split(" ")[1]);

                    //var result = base._client.GetAsync(_res.File.EntityUri).Result;

                    Stream resStream = result.Content.ReadAsStreamAsync().Result;

                    MemoryStream ms = new MemoryStream();

                    resStream.CopyTo(ms);

                    return ms.ToArray();
                }
                else if (_res.Status.ToLower().Equals(EncompassExportStatus.RUNNING))
                {
                    goto GetFile;
                }
            }

            return null;
        }

        private bool UploadAttachment(RestWebClient _client, EncompassAttachmentUploadURL _res, UploadRequest request)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                byte[] _file = ms.ToArray();

                RestWebClient _newClient = new RestWebClient(_res.MediaUrl);

                var reqObjNew = new HttpRequestObject() { FileStream = _file, REQUESTTYPE = HeaderConstant.PUT, RequestContentType = RequestTypeConstant.FILE };

                dynamic rs = _newClient.RequestAsync<dynamic>(reqObjNew).Result;

                string responseStream = rs.Content;

                if (rs.StatusCode == HttpStatusCode.OK || rs.StatusCode == HttpStatusCode.Created || rs.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

    }
}
