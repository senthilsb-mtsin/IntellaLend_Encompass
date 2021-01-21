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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace EncompassConnectorAPI.Controllers
{
    ///<Summary>
    /// Encompass Loan Attachment Releated Activities
    ///</Summary>
    public class EncompassAttachmentController : BaseController
    {
        #region Get Attachments

        ///<Summary>
        /// Get Single Encompass Loan Attachment Properties
        ///</Summary>
        [HttpGet, Route("api/v1/attachment")]
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
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
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
        /// Get Encompass Loan Attachments
        ///</Summary>
        [HttpGet, Route("api/v1/attachments")]
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
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
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
        /// Get Encompass Loan Attachment which has Document assigned to it
        ///</Summary>
        [HttpGet, Route("api/v1/attachment/assigned")]
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
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
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
        [HttpGet, Route("api/v1/attachment/unassgined")]
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
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
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
        /// Delete Encompass Loan Attachment
        ///</Summary>
        [HttpPatch, Route("api/v1/attachment/remove")]
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

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Unauthorized();
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
        [HttpPost, Route("api/v1/attachment/download")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(ByteArrayContent))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public HttpResponseMessage DownloadAttachment(DownloadAttachment _request)
        {
            var result = new HttpResponseMessage(HttpStatusCode.BadRequest);
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string _fileNameGUID = _request.attachmentID;

                EAttachmentDownloadRequest _exportJobReq = new EAttachmentDownloadRequest()
                {
                    Attachments = new List<string>() { _fileNameGUID }
                };

                var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_DOWNLOAD_URL, _request.loanGuid), Content = _exportJobReq, REQUESTTYPE = HeaderConstant.POST };

                IRestResponse response = _client.Execute(reqObj);

                string responseStream = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    EDownloadURLResponse _res = JsonConvert.DeserializeObject<EDownloadURLResponse>(responseStream);

                    if (_res != null)
                    {
                        byte[] file = ExportQueuedJob(_res);

                        if (file != null)
                        {
                            result = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ByteArrayContent(file)
                            };
                            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                            {
                                FileName = $"Attachment-{_request.attachmentID}.pdf"
                            };
                            result.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentTypeConstant.BYTE);

                            return result;
                        }
                        else
                        {
                            _badRequest.Summary = "Empty File";
                            _badRequest.Details = "Error while querying Encompass for Attachment";
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
            result.Content = new StringContent(JsonConvert.SerializeObject(_badRequest));

            return result;
        }

        #endregion

        #region Upload Attachment

        ///<Summary>
        /// Upload Encompass Loan Attachment
        ///</Summary>
        [HttpPost, Route("api/v1/attachment/upload/{loanGUID}/{fileName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EUploadResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IHttpActionResult UploadAttachment(string loanGUID, string fileName)
        {
            fileName = fileName + ".pdf";
            Logger.WriteErrorLog($"UploadAttachment In : {loanGUID}, {fileName}");
            LockResourceModel lockResource = null;
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                var provider = Request.Content.ReadAsMultipartAsync().Result;

                byte[] fileStream = null;
                foreach (var file in provider.Contents)
                {
                    fileStream = file.ReadAsByteArrayAsync().Result;
                }

                lockResource = LoanResource.LockLoan(_client, loanGUID);
                Logger.WriteErrorLog($"After Lock");
                if (lockResource.Status)
                {
                    EUploadRequest _uploadReq = new EUploadRequest() { File = new EFileEntities() { Name = fileName, ContentType = ContentTypeConstant.PDF, Size = fileStream.Length }, Title = Path.GetFileNameWithoutExtension(fileName) };

                    var reqObj = new HttpRequestObject() { URL = string.Format(EncompassURLConstant.GET_UPLOAD_URL, loanGUID), Content = _uploadReq, REQUESTTYPE = HeaderConstant.POST };

                    IRestResponse response = _client.Execute(reqObj);

                    //var response = base._client.PostAsJsonAsync(string.Format(EncompassURLConstant.UPLOAD_ATTACHMENT_REQUEST, request.LoanGUID), _uploadReq).Result;
                    Logger.WriteErrorLog($"After Requwest");
                    string responseStream = response.Content;

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
                    {
                        EAttachmentUploadResponse _res = JsonConvert.DeserializeObject<EAttachmentUploadResponse>(responseStream);

                        if (_res != null)
                        {
                            if (fileStream.Length > 0)
                            {

                                EUploadResponse eUpload = new EUploadResponse();
                                eUpload.Status = UploadAttachment(_res, fileStream, fileName);
                                eUpload.Message = eUpload.Status ? ResponseConstant.UPLOAD_SUCCESSFULLY : ResponseConstant.UPLOAD_FAILED;
                                eUpload.AttachmentGUID = _res.AttachmentID; // _res.MediaUrl.Split('?')[0].Split(new string[] { "attachment-" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('.')[0];
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
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Unauthorized();
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
                    LoanResource.UnLockLoan(_client, lockResource.Message, loanGUID);
                }
            }

            return BadRequest(JsonConvert.SerializeObject(_badRequest));
        }

        #endregion

        #region Private Methods

        private byte[] ExportQueuedJob(EDownloadURLResponse _job)
        {
            List<byte[]> _pages = new List<byte[]>();
            foreach (var item in _job.Attachments[0].OriginalUrls)
            {
                RestWebClient _newClient = new RestWebClient(item);

                var reqObj = new HttpRequestObject() { REQUESTTYPE = HeaderConstant.GET };

                IRestResponse response = _newClient.Execute(reqObj);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (response.RawBytes != null)
                        return response.RawBytes;
                }
            }

            return null;
        }

        private byte[] GetSplitArray(byte[] srcArray, Int32 index, Int32 length)
        {
            byte[] destArray = new byte[length];
            Buffer.BlockCopy(srcArray, 16, destArray, 0, length);
            return destArray;
        }

        private bool UploadAttachment(EAttachmentUploadResponse _res, byte[] request, string fileName)
        {
            byte[] _file = request;

            if (_res.MultiChunkRequired)
            {
                Int32 lastIndex = 0;

                foreach (EUploadChunkEntites item in _res.MultiChunk.ChunkList)
                {
                    bool result = UploadFileRequest(item.UploadUrl, GetSplitArray(_file, lastIndex, item.Size), fileName, ContentTypeConstant.FILE);
                    if (result)
                        lastIndex = item.Size;
                }

                RestWebClient _newClient = new RestWebClient(_res.MultiChunk.CommitUrl);

                var reqObjNew = new HttpRequestObject() { REQUESTTYPE = HeaderConstant.POST };

                var res = _newClient.Execute(reqObjNew);

                string responseStream = res.Content;

                if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.Created || res.StatusCode == HttpStatusCode.NoContent)
                {
                    return true;
                }
            }
            else
            {
                return UploadFileRequest(_res.UploadUrl, _file, fileName, ContentTypeConstant.FILE);
            }

            return false;
        }

        private bool UploadFileRequest(string URL, byte[] _file, string fileName, string requestType)
        {
            RestWebClient _newClient = new RestWebClient(URL);

            var reqObjNew = new HttpRequestObject() { FileStream = _file, REQUESTTYPE = HeaderConstant.PUT, Content = new { FileName = fileName }, RequestContentType = requestType };

            var res = _newClient.Execute(reqObjNew);

            string responseStream = res.Content;

            if (res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.Created || res.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
