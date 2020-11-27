using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperInterseptor;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
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
        private HttpClient client;

        public EncompassWrapperAPI(string _apiURL, string _header)
        {
            API_URL = _apiURL;
            HEADER = _header;
            client = new HttpClient(new TokenAppendHandler(HEADER, API_URL));
            client.BaseAddress = new Uri(API_URL);

        }

        public List<string> GetLoans(List<Dictionary<string, string>> _eFields)
        {
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


            var result = client.PostAsync(EncompassURLConstant.GET_LOAN, GetByteArrayContent(_res)).Result;

            string res = result.Content.ReadAsStringAsync().Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return (JsonConvert.DeserializeObject<List<EPipelineLoans>>(res)).Select(x => x.LoanGuid).ToList();
            }

            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Unable to get loan(s) from Encompass. Message : {_error.Details}");
        }

        public List<EAttachment> GetUnassignedAttachments(string loanGUID)
        {
            LogMessage($"GetUnassignedAttachments : {loanGUID}");

            var result = client.GetAsync(string.Format(EncompassURLConstant.GET_UNATTACHMENTS, loanGUID)).Result;

            string res = result.Content.ReadAsStringAsync().Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<EAttachment>>(res);
            }

            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Unable to get loan attachment(s) for the loan('{loanGUID}') from Encompass. Message : {_error.Details}");
        }

        public void UploadProcessFlag(string loanGUID, string fieldID, string fieldValue)
        {

            UpdateCustomFieldRequest _req = new UpdateCustomFieldRequest()
            {
                LoanGuid = loanGUID,
                Fields = new Dictionary<string, string>()
                    {
                        { fieldID , fieldValue }
                    }
            };

            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, EncompassURLConstant.UPDATE_CUSTOM_FIELD)
            {
                Content = GetByteArrayContent(_req)
            };

            var result = client.SendAsync(request).Result;

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(result.Content.ReadAsStringAsync().Result);

                if (_error.Details.Contains("read-only mode"))
                    throw new EncompassWrapperLoanLockException(_error.Details);

                throw new EncompassWrapperException($"Unable to update the download complete flag in Encompass. Field ID : '{fieldID}', FieldValue: '{fieldValue}'.Message : {_error.Details}");
            }
        }


        public byte[] DownloadAttachment(string loanGUID, string attachmentGUID, string AttachmentName)
        {
            LogMessage($"DownloadAttachment : {loanGUID}, {attachmentGUID}");

            var result = client.GetAsync(string.Format(EncompassURLConstant.GET_DOWNLOAD_ATTACHMENT, loanGUID, attachmentGUID)).Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Content.ReadAsByteArrayAsync().Result;
            }
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(result.Content.ReadAsStringAsync().Result);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Error while downloading from Unassigned folder. Attachment : {AttachmentName}. Message : {_error.Details}");
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
            LogMessage($"GetAllLoanDocuments : {loanGUID}");
            var result = client.GetAsync(string.Format(EncompassURLConstant.GET_ALLLOANDOCUMENTS, loanGUID)).Result;

            string res = result.Content.ReadAsStringAsync().Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<EContainer>>(res);
            }
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Error while fetching EFolder(s) in Encompass. Message : {_error.Details}");
        }

        public EUploadResponse UploadAttachment(string loanGUID, string fileName, string fileNameWithExtension, byte[] file)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();
            LogMessage($"UploadAttachment : {loanGUID}, Filename : {fileName}, FileLength : {file.Length}");
            form.Add(new StringContent(loanGUID), "loanGUID");
            form.Add(new StringContent(fileName), "fileName");
            form.Add(new StringContent(fileNameWithExtension), "fileNameWithExtension");
            form.Add(new ByteArrayContent(file, 0, file.Length), "file", fileNameWithExtension);

            var result = client.PostAsync(EncompassURLConstant.UPLOAD_ATTACHMENT, form).Result;
            string res = result.Content.ReadAsStringAsync().Result;
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
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Unable to upload the attachment ('{fileName}'). Message : {_error.Details}");
        }


        public AddContainerResponse AddDocument(string loanGUID, string documentName)
        {
            AddContainerRequest _req = new AddContainerRequest()
            {
                LoanGUID = loanGUID,
                Documents = new List<EAddDocument>() { new EAddDocument() { DocumentName = documentName, DocumentDescription = documentName } }
            };


            var method = new HttpMethod("POST");

            var request = new HttpRequestMessage(method, EncompassURLConstant.ADD_DOCUMENT)
            {
                Content = GetByteArrayContent(_req)
            };

            var result = client.SendAsync(request).Result;

            string res = result.Content.ReadAsStringAsync().Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<AddContainerResponse>(res);
            }
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Unable to create the processed folder '{documentName}' in Encompass. Message : {_error.Details}");
        }

        public bool AssignDocumentAttachments(string loanGUID, string documentGuid, List<string> attachmentGUIDs, string FolderName)
        {
            AssignAttachmentRequest _req = new AssignAttachmentRequest()
            {
                LoanGUID = loanGUID,
                DocumentGUID = documentGuid,
                AttachmentGUIDs = attachmentGUIDs

            };

            var method = new HttpMethod("PATCH");

            var request = new HttpRequestMessage(method, EncompassURLConstant.ASSIGN_DOCUMENT_ATTACHMENT)
            {
                Content = GetByteArrayContent(_req)
            };

            var result = client.SendAsync(request).Result;
            //check result and throw error
            string res = result.Content.ReadAsStringAsync().Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                EAddRemoveAttachmentResponse _res = JsonConvert.DeserializeObject<EAddRemoveAttachmentResponse>(res);
                if (_res.Status)
                    return _res.Status;
            }
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Could not move the attachment(s) from Unassigned folder to processed folder '{FolderName}'. Message : {_error.Details}");
        }

        public List<EFieldResponse> GetPredefinedFieldValues(string loanGUID, string[] fieldIds)
        {
            FieldGetRequest _req = new FieldGetRequest()
            {
                LoanGUID = loanGUID,
                FieldIDs = fieldIds
            };

            var method = new HttpMethod("POST");

            var request = new HttpRequestMessage(method, EncompassURLConstant.GET_PREDEFINED_FIELDVALUES)
            {
                Content = GetByteArrayContent(_req)
            };

            var result = client.SendAsync(request).Result;

            string res = result.Content.ReadAsStringAsync().Result;

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<EFieldResponse>>(res);
            }
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            if (_error.Details.Contains("read-only mode"))
                throw new EncompassWrapperLoanLockException(_error.Details);

            throw new EncompassWrapperException($"Cannot able to get field values from encompass. Fields : {string.Join(",", fieldIds)}. Message : {_error.Details}");
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
