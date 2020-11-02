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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace EncompassWrapperAPI.Controllers
{
    [ApiController]
    [EnableCors("OpenPolicy")]
    [Route("api/[controller]")]
    public class EncompassAttachmentController : CustomControllerBase
    {
        #region Construtor

        private readonly IHttpClientFactory _clientFactory;

        private HttpClient _client;

        private readonly ILogger<EncompassAttachmentController> _logger;

        public EncompassAttachmentController(ILogger<EncompassAttachmentController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient(HttpClientFactoryConstant.RequestWithValidator);
        }

        #endregion

        #region Get Attachments

        [HttpGet("GetAssignedLoanAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(IEnumerable<EAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetAssignedLoanAttachment(string loanGuid)
        {
            ErrorResponse _badRes = new ErrorResponse();
            List<EAttachment> _eAttachments = new List<EAttachment>();
            try
            {
                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENTS, loanGuid)).Result;

                string responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eAttachments = JsonConvert.DeserializeObject<List<EAttachment>>(responseStream);

                    _eAttachments = _eAttachments.Where(a => a.Document != null && a.Pages != null && a.Pages.Count > 0).ToList();

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
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRes);
        }

        [HttpGet("GetUnassginedLoanAttachments")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(IEnumerable<EAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetUnassginedLoanAttachments(string loanGuid)
        {
            ErrorResponse _badRes = new ErrorResponse();
            List<EAttachment> _eAttachments = new List<EAttachment>();
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENTS, loanGuid)).Result;

                string responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eAttachments = JsonConvert.DeserializeObject<List<EAttachment>>(responseStream);

                    _eAttachments = _eAttachments.Where(a => a.Document == null && a.Pages != null && a.Pages.Count > 0).ToList();

                    return Ok(_eAttachments);
                }
                else
                {
                    _badRes = JsonConvert.DeserializeObject<ErrorResponse>(responseStream);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRes);
        }

        [HttpGet("GetAllLoanAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(IEnumerable<EAttachment>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetAllLoanAttachment(string loanGuid)
        {
            ErrorResponse _badRes = new ErrorResponse();
            List<EAttachment> _eAttachments = new List<EAttachment>();
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN_ATTACHMENTS, loanGuid)).Result;

                string responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _eAttachments = JsonConvert.DeserializeObject<List<EAttachment>>(responseStream);

                    _eAttachments = _eAttachments.Where(a => a.Pages != null && a.Pages.Count > 0).ToList();

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
                _logger.LogError(ex, ex.Message);
            }

            return BadRequest(_badRes);
        }

        [HttpGet("GetDocumentAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(List<EDocument>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult GetDocumentAttachment(string loanGuid, string documentID)
        {
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {
                string responseStream = string.Empty;

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.GetAsync(string.Format(EncompassURLConstant.GET_LOAN_DOCUMENT_ATTACHMENTS, loanGuid, documentID)).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    List<EDocument> _docs = JsonConvert.DeserializeObject<List<EDocument>>(responseStream);
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

        #region Download Attachment

        [HttpGet("DownloadAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(FileContentResult))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult DownloadAttachment([FromQuery] DownloadAttachment _request)
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

                _logger.LogError(JsonConvert.SerializeObject(_exportJobReq));

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);

                var response = _client.PostAsJsonAsync(EncompassURLConstant.EXPORT_JOB_REQUEST, _exportJobReq).Result;

                responseStream = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.NoContent)
                {
                    EJobResponse _res = JsonConvert.DeserializeObject<EJobResponse>(responseStream);

                    if (_res != null && _res.JobId != string.Empty)
                    {
                        byte[] file = ExportQueuedJob(_client, _res);

                        if (file != null)
                        {
                            return File(file, "application/octet-stream", $"{_request.attachmentID}.pdf");
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
                _logger.LogError(ex, ex.Message);
                _badRequest.Details = ex.Message;
                _badRequest.Summary = ResponseConstant.ERROR;
                _badRequest.ErrorCode = HttpStatusCode.InternalServerError.ToString();
            }

            return BadRequest(_badRequest);
        }

        #endregion

        #region Upload Attachment

        [HttpPost("UploadAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EUploadResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult UploadAttachment([FromForm] UploadRequest request)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);
            LockResourceModel lockResource = null;
            ErrorResponse _badRequest = new ErrorResponse();
            try
            {

                lockResource = LoanResource.LockLoan(_client, request.LoanGUID);

                if (lockResource.Status)
                {
                    EUploadRequest _uploadReq = new EUploadRequest() { Title = request.FileName, FileWithExtension = request.FileNameWithExtension, CreateReason = 1 };

                    var response = _client.PostAsJsonAsync(string.Format(EncompassURLConstant.UPLOAD_ATTACHMENT_REQUEST, request.LoanGUID), _uploadReq).Result;

                    string responseStream = response.Content.ReadAsStringAsync().Result;

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
                                eUpload.AttachmentGUID = _res.MediaUrl.Split("?")[0].Split("attachment-")[1].Split(".")[0];
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

        #region Assign Attachment to Document

        [HttpPatch("RemoveDocumentAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EAddRemoveAttachmentResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult RemoveDocumentAttachment(AssignAttachmentRequest _req)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);
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

                    var response = _client.PatchAsync(string.Format(EncompassURLConstant.REMOVE_DOCUMENT_ATTACHMENT, _req.LoanGUID, _req.DocumentGUID), new ObjectContent(typeof(List<EAddRemoveAttachment>), _eAddRemove, new JsonMediaTypeFormatter())).Result;

                    responseStream = response.Content.ReadAsStringAsync().Result;

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
                _logger.LogError(ex, ex.Message);
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

            return BadRequest(_eRequest);
        }

        [HttpPatch("AssignDocumentAttachment")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(EAddRemoveAttachmentResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request", typeof(ErrorResponse))]
        public IActionResult AssignDocumentAttachment(AssignAttachmentRequest _req)
        {

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(base.sessionHelper.TokenType, base.sessionHelper.Token);
            LockResourceModel lockResource = null;
            ErrorResponse _eRequest = new ErrorResponse();
            try
            {
                _logger.LogError(JsonConvert.SerializeObject(_req));
                lockResource = LoanResource.LockLoan(_client, _req.LoanGUID);

                if (lockResource.Status)
                {
                    string responseStream = string.Empty;

                    List<EAddRemoveAttachment> _eAddRemove = new List<EAddRemoveAttachment>();

                    foreach (string attachmentGUID in _req.AttachmentGUIDs)
                    {
                        _eAddRemove.Add(new EAddRemoveAttachment() { EntityId = attachmentGUID, EntityType = EncompassEntityType.ATTACHMENT });
                    }
                    _logger.LogError(JsonConvert.SerializeObject(_eAddRemove));
                    var response = _client.PatchAsync(string.Format(EncompassURLConstant.ADD_DOCUMENT_ATTACHMENT, _req.LoanGUID, _req.DocumentGUID), new ObjectContent(typeof(List<EAddRemoveAttachment>), _eAddRemove, new JsonMediaTypeFormatter())).Result;

                    responseStream = response.Content.ReadAsStringAsync().Result;

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
                _logger.LogError(ex, ex.Message);
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

            return BadRequest(_eRequest);
        }

        #endregion

        #region Private Methods

        private byte[] ExportQueuedJob(HttpClient _client, EJobResponse _job)
        {

        GetFile:

            var response = _client.GetAsync(string.Format(EncompassURLConstant.EXPORT_JOB_REQUEST_STATUS, _job.JobId)).Result;

            string responseStream = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                EJobResponse _res = JsonConvert.DeserializeObject<EJobResponse>(responseStream);

                if (_res.Status.ToLower().Equals(EncompassExportStatus.SUCCESS))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_res.File.AuthorizationHeader.Split(" ")[0], _res.File.AuthorizationHeader.Split(" ")[1]);

                    var result = _client.GetAsync(_res.File.EntityUri).Result;

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

        private bool UploadAttachment(HttpClient _client, EncompassAttachmentUploadURL _res, UploadRequest request)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                request.File.CopyTo(ms);
                byte[] _file = ms.ToArray();
                Stream sm = new MemoryStream(_file);
                var rs = _client.PutAsync(_res.MediaUrl, new StreamContent(sm)).Result;
                string responseStream = rs.Content.ReadAsStringAsync().Result;
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
