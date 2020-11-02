using Microsoft.Practices.EnterpriseLibrary.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IL.EncompassExport
{
    public class EncompassWrapperAPI : IDisposable
    {
        private string API_URL = string.Empty;
        private HttpClient client;

        public EncompassWrapperAPI(string _apiURL, string _header)
        {
            API_URL = _apiURL;
            client = new HttpClient() { BaseAddress = new Uri(API_URL) };
            client.Timeout = TimeSpan.FromMinutes(30);
        }

        public List<string> GetPipeLineLoans(List<Dictionary<string, string>> _importField)
        {
            PipeLineRequest _req = new PipeLineRequest()
            {
                EFields = _importField
            };
            LogException.Log("GetPipeLineLoans Request Data: " + JsonConvert.SerializeObject(_req));
            var result = client.PostAsync(EncompassURLConstant.GET_PIPELINE_LOANS, GetByteArrayContent(_req)).Result;

            string res = result.Content.ReadAsStringAsync().Result;

            LogException.Log("GetPipeLineLoans Response Data: " + JsonConvert.SerializeObject(res));
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<string>>(res);
            }
            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            throw new EncompassSDKAPIException($"Unable to get loan(s) from Encompass. Message : {_error.Summary}");
        }

        public Dictionary<string, string> GetPredefinedFieldValues(string loanGUID, List<string> fieldIDs)
        {

            LoanDefinedFields _req = new LoanDefinedFields()
            {
                LoanGUID = loanGUID,
                FieldIDs = fieldIDs
            };
            LogException.Log("GetPredefinedFieldValues Request Data: " + JsonConvert.SerializeObject(_req));
            var result = client.PostAsync(EncompassURLConstant.GET_LOAN_FIELDS, GetByteArrayContent(_req)).Result;

            string res = result.Content.ReadAsStringAsync().Result;
            LogException.Log("GetPredefinedFieldValues Response Data: " + JsonConvert.SerializeObject(res));
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
            }

            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            throw new EncompassSDKAPIException($"Cannot able to fetch Fields for the loan. LoanGUID : {loanGUID}, Message : {_error.Summary}");
        }

        public List<IntellaLend.Model.EnDocuments> GetAllLoanAttachments(string loanGUID, Dictionary<string, string> configuredParkingSpot)
        {
            DownloadAttachmentRequest _req = new DownloadAttachmentRequest() {
                loanGUID = loanGUID,
                parkingSpot = configuredParkingSpot
            };

            LogException.Log("GetAllLoanAttachments Request Data : " + JsonConvert.SerializeObject(_req));
            var result = client.PostAsync(EncompassURLConstant.GET_LOAN_ATTACHMENT, GetByteArrayContent(_req)).Result;

            string res = result.Content.ReadAsStringAsync().Result;
            LogException.Log("GetAllLoanAttachments Response Data : " + res);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<List<IntellaLend.Model.EnDocuments>>(res);
            }

            ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

            throw new EncompassSDKAPIException($"Cannot able to get loan attachments. LoanGUID : {loanGUID}, Message : {_error.Summary}");
        }

        public void UpdateLoanCustomField(string loanGUID, Dictionary<string, object> _updateField)
        {
            UpdateLoanField _req = new UpdateLoanField()
            {
                loanGUID = loanGUID,
                fields = _updateField
            };
            LogException.Log("UpdateLoanCustomField Request Data: " + JsonConvert.SerializeObject(_req));
            var result = client.PostAsync(EncompassURLConstant.UPDATE_LOAN_CUSTOM_FIELD, GetByteArrayContent(_req)).Result;

            string res = result.Content.ReadAsStringAsync().Result;
            LogException.Log("UpdateLoanCustomField Response Data : " + res);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

                throw new EncompassSDKAPIException($"Unable to update field in Encompass. Message : {_error.Summary}");
            }
        }

        private ByteArrayContent GetByteArrayContent(object data)
        {
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }

        public void Dispose()
        {
            client.Dispose();
        }

    }

    public class EncompassSDKAPIException : Exception
    {
        public EncompassSDKAPIException(string message) : base(message)
        { }

        public EncompassSDKAPIException(string message, Exception innerException) : base(message, innerException)
        { }
    }

}
