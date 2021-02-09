using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperInterseptor;
using MTS.Web.Helpers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EncompassAPIHelper
{
    public class EncompassWrapperAPI : IDisposable
    {
        private string API_URL = string.Empty;
        private string HEADER = string.Empty;
        private TokenAppendHandler _token = null;
        private RestWebClient client;
        private Int16 maxRetry = 3;

        public EncompassWrapperAPI(string _apiURL, string _header)
        {
            API_URL = _apiURL;
            HEADER = _header;
            client = new RestWebClient(API_URL);
            _token = new TokenAppendHandler(HEADER, API_URL);
            client.RefreshValidationHeaders += _token.GetTokenFromDB;
        }

        public List<string> GetLoans(List<Dictionary<string, string>> _eFields)
        {
            Int16 retryCount = 0;
            RequestAgain:

            List<Fields> fieldList = new List<Fields>();
            Fields field = null;

            foreach (Dictionary<string, string> item in _eFields)
            {
                field = new Fields();
                field.FieldID = item.Keys.FirstOrDefault();
                field.FieldValue = item.Values.FirstOrDefault();
                field.MatchType = "exact";
                fieldList.Add(field);
            }

            LoanRequest _res = new LoanRequest()
            {
                QueryFields = fieldList,
                ReturnFields = new List<string>() {
                       "Loan.GUID"
                    },
                ReturnLoanLimit = "100"
            };

            var req = new HttpRequestObject() { REQUESTTYPE = "POST", Content = _res, URL = EncompassURLConstant.GET_LOAN };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<List<EPipelineLoans>>(res)).Select(x => x.LoanGuid).ToList();
            }

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Unable to get loan(s) from Encompass. Message : {_error.Message.Details}");
        }

        public bool CreateWebhookSubscription(object reqBody)
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"CreateWebhookSubscription ");

            var req = new HttpRequestObject() { REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.WEBHOOK_CREATE), Content = reqBody };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }

            return false;
        }

        public bool DeleteWebhookSubscription(object reqBody)
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"DeleteWebhookSubscription ");

            var req = new HttpRequestObject() { REQUESTTYPE = "DELETE", URL = string.Format(EncompassURLConstant.WEBHOOK_DELETE, reqBody) };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }

            return false;
        }

        public List<WebHookSubscriptions> GetWebhookSubscriptions()
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"GetWebhookSubscriptions ");

            var req = new HttpRequestObject() { REQUESTTYPE = "GET", URL = string.Format(EncompassURLConstant.WEBHOOK_GETALL_SUBSCRIPTION) };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<List<WebHookSubscriptions>>(res));
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Unable to get loan(s) from Encompass. Message : {_error.Message.Details}");
        }

        public List<EAttachment> GetUnassignedAttachments(string loanGUID)
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"GetUnassignedAttachments : {loanGUID}");

            var req = new HttpRequestObject() { REQUESTTYPE = "GET", URL = string.Format(EncompassURLConstant.GET_UNATTACHMENTS, loanGUID) };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<EAttachment>>(res);
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Unable to get loan attachment(s) for the loan('{loanGUID}') from Encompass. Message : {_error.Message.Details}");
        }

        public void UploadProcessFlag(string loanGUID, string fieldID, string fieldValue)
        {
            Int16 retryCount = 0;
            RequestAgain:
            UpdateCustomFieldRequest _req = new UpdateCustomFieldRequest()
            {
                LoanGuid = loanGUID,
                Fields = new Dictionary<string, string>()
                    {
                        { fieldID , fieldValue }
                    }
            };

            var req = new HttpRequestObject() { REQUESTTYPE = "PATCH", URL = string.Format(EncompassURLConstant.UPDATE_CUSTOM_FIELD), Content = _req };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
                {
                    _token.SetToken();
                    goto RequestAgain;
                }
                BadErrorResponse _error = new BadErrorResponse();
                if (!res.Contains("Message"))
                {
                    ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                    _error.Message = _errorRes;
                }
                else
                {
                    BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                    ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                    _error.Message = _errorRes;
                }

                if (_error.Message.Details.Contains("read-only mode"))
                    throw new EncompassWrapperLoanLockException(_error.Message.Details);

                throw new EncompassWrapperException($"Unable to update the download complete flag in Encompass. Field ID : '{fieldID}', FieldValue: '{fieldValue}'.Message : {_error.Message.Details}");
            }
        }


        public byte[] DownloadAttachment(string loanGUID, string attachmentGUID, string AttachmentName)
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"DownloadAttachment : {loanGUID}, {attachmentGUID}");

            DownloadAttachment _req = new DownloadAttachment
            {
                loanGuid = loanGUID,
                attachmentID = attachmentGUID
            };

            var req = new HttpRequestObject() { REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.GET_DOWNLOAD_ATTACHMENT), Content = _req };

            IRestResponse result = client.Execute(req);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.RawBytes;
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!result.Content.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(result.Content);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Error while downloading from Unassigned folder. Attachment : {AttachmentName}. Message : {_error.Message.Details}");
        }


        private ByteArrayContent GetByteArrayContent(object data)
        {
            var myContent = JsonConvert.SerializeObject(data);
            LogMessage(myContent);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        //added by mani
        public List<EContainer> GetAllLoanDocuments(string loanGUID)
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"GetAllLoanDocuments : {loanGUID}");

            var req = new HttpRequestObject() { REQUESTTYPE = "GET", URL = string.Format(EncompassURLConstant.GET_ALLLOANDOCUMENTS, loanGUID) };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<EContainer>>(res);
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Error while fetching EFolder(s) in Encompass. Message : {_error.Message.Details}");
        }

        public EAttachment GetAttachment(string loanGUID, string attachmentID)
        {
            Int16 retryCount = 0;
            RequestAgain:
            LogMessage($"GetAttachments : {loanGUID},{attachmentID}");

            var req = new HttpRequestObject() { REQUESTTYPE = "GET", URL = string.Format(EncompassURLConstant.GET_ATTACHMENT, loanGUID, attachmentID) };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<EAttachment>(res);
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Error while fetching Attachment(s) in Encompass. Message : {_error.Message.Details}");
        }

        public EUploadResponse UploadAttachment(string loanGUID, string fileName, string fileNameWithExtension, byte[] file)
        {
            Int16 retryCount = 0;
            RequestAgain:
            //MultipartFormDataContent form = new MultipartFormDataContent();
            LogMessage($"UploadAttachment : {loanGUID}, Filename : {fileName}, FileLength : {file.Length}");
            //form.Add(new StringContent(loanGUID), "loanGUID");
            //form.Add(new StringContent(fileName), "fileName");
            //form.Add(new StringContent(fileNameWithExtension), "fileNameWithExtension");
            //form.Add(new ByteArrayContent(file, 0, file.Length), "file", fileNameWithExtension);

            var req = new HttpRequestObject() { Content = new { FileName = fileName }, FileStream = file, REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.UPLOAD_ATTACHMENT, loanGUID, fileName.Replace(".pdf", "")), RequestContentType = "multipart/form-data" };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            LogMessage($"res : {res} , result.StatusCode : {result.StatusCode}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EUploadResponse _upload_res = JsonConvert.DeserializeObject<EUploadResponse>(res);

                if (_upload_res.Status)
                {
                    return _upload_res;
                }
                else
                {
                    throw new EncompassWrapperException($"Unable to upload the attachment ('{fileName}'). Message : {_upload_res.Message}");
                }
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Unable to upload the attachment ('{fileName}'). Message : {_error.Message.Details}");
        }


        public AddContainerResponse AddDocument(string loanGUID, string documentName)
        {
            Int16 retryCount = 0;
            RequestAgain:
            AddContainerRequest _req = new AddContainerRequest()
            {
                LoanGUID = loanGUID,
                Documents = new List<EAddDocument>() { new EAddDocument() { DocumentName = documentName, DocumentDescription = documentName } }
            };

            var req = new HttpRequestObject() { REQUESTTYPE = "PATCH", URL = string.Format(EncompassURLConstant.ADD_DOCUMENT), Content = _req };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<AddContainerResponse> _response = JsonConvert.DeserializeObject<List<AddContainerResponse>>(res);
                if (_response.Count > 0)
                    return _response[0];
                else
                    return null;
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Unable to create the processed folder '{documentName}' in Encompass. Message : {_error.Message.Details}");
        }

        public bool AssignDocumentAttachments(string loanGUID, string documentGuid, List<string> attachmentGUIDs, string FolderName)
        {
            Int16 retryCount = 0;
            RequestAgain:
            AssignAttachmentRequest _req = new AssignAttachmentRequest()
            {
                LoanGUID = loanGUID,
                DocumentGUID = documentGuid,
                AttachmentGUIDs = attachmentGUIDs

            };

            var req = new HttpRequestObject() { REQUESTTYPE = "PATCH", URL = string.Format(EncompassURLConstant.ASSIGN_DOCUMENT_ATTACHMENT), Content = _req };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EAddRemoveAttachmentResponse _res = JsonConvert.DeserializeObject<EAddRemoveAttachmentResponse>(res);
                if (_res.Status)
                    return _res.Status;
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Could not move the attachment(s) from Unassigned folder to processed folder '{FolderName}'. Message : {_error.Message.Details}");
        }

        public bool RemoveDocumentAttachments(string loanGUID, string documentGuid, List<string> attachmentGUIDs, string FolderName)
        {
            Int16 retryCount = 0;
            RequestAgain:
            AssignAttachmentRequest _req = new AssignAttachmentRequest()
            {
                LoanGUID = loanGUID,
                DocumentGUID = documentGuid,
                AttachmentGUIDs = attachmentGUIDs

            };

            var req = new HttpRequestObject() { REQUESTTYPE = "PATCH", URL = string.Format(EncompassURLConstant.REMOVE_DOCUMENT_ATTACHMENT), Content = _req };

            IRestResponse result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EAddRemoveAttachmentResponse _res = JsonConvert.DeserializeObject<EAddRemoveAttachmentResponse>(res);
                if (_res.Status)
                    return _res.Status;
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Could not remove the attachment(s) from Upload folder '{FolderName}'. Message : {_error.Message.Details}");
        }

        public List<EFieldResponse> GetPredefinedFieldValues(string loanGUID, string[] fieldIds)
        {
            Int16 retryCount = 0;
            RequestAgain:
            FieldGetRequest _req = new FieldGetRequest()
            {
                LoanGUID = loanGUID,
                FieldIDs = fieldIds
            };

            var req = new HttpRequestObject() { REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.GET_PREDEFINED_FIELDVALUES), Content = _req };

            var result = client.Execute(req);

            string res = result.Content; Logger.WriteTraceLog($"result.Content : {result.Content}");

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<EFieldResponse>>(res);
            }
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized && retryCount++ < this.maxRetry)
            {
                _token.SetToken();
                goto RequestAgain;
            }
            BadErrorResponse _error = new BadErrorResponse();
            if (!res.Contains("Message"))
            {
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(res);
                _error.Message = _errorRes;
            }
            else
            {
                BadStringErrorResponse _errorStr = JsonConvert.DeserializeObject<BadStringErrorResponse>(result.Content);
                ErrorResponse _errorRes = JsonConvert.DeserializeObject<ErrorResponse>(_errorStr.Message);
                _error.Message = _errorRes;
            }

            if (_error.Message.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Message.Details);

            throw new EncompassWrapperException($"Cannot able to get field values from encompass. Fields : {string.Join(",", fieldIds)}. Message : {_error.Message.Details}");
        }

        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

    }

    public class EncompassWrapperException : Exception
    {
        public EncompassWrapperException(string message) : base(message)
        { }

        public EncompassWrapperException(string message, Exception innerException) : base(message, innerException)
        { }
    }

    public class EncompassWrapperLoanLockException : Exception
    {
        public EncompassWrapperLoanLockException(string message) : base(message)
        { }

        public EncompassWrapperLoanLockException(string message, Exception innerException) : base(message, innerException)
        { }
    }

}
