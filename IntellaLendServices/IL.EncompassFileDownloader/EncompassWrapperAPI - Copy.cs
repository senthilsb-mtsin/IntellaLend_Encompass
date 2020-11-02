using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.EResponseModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using EncompassWrapperInterseptor;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IL.EncompassFileDownloader
{
    //public class EncompassWrapperAPI : IDisposable
    //{
    //    private string API_URL = string.Empty;
    //    private string HEADER = string.Empty;
    //    private HttpClient client;

    //    public EncompassWrapperAPI(string _apiURL, string _header)
    //    {
    //        API_URL = _apiURL;
    //        HEADER = _header;
    //        client = new HttpClient(new TokenAppendHandler(HEADER, API_URL));
    //        client.BaseAddress = new Uri(API_URL);

    //    }

    //    public List<string> GetLoans(List<KeyValuePair<string, string>> _eFields)
    //    {
    //        List<Fields> fieldList = new List<Fields>();
    //        Fields field = null;

    //        foreach (KeyValuePair<string, string> item in _eFields)
    //        {
    //            field = new Fields();
    //            field.fieldID = item.Key;
    //            field.fieldValue = item.Value;
    //            field.matchType = "exact";
    //            fieldList.Add(field);
    //        }

    //        LoanRequest _res = new LoanRequest()
    //        {
    //            queryFields = fieldList,
    //            returnFields = new List<string>() {
    //                   "Loan.GUID"
    //                },
    //            returnLoanLimit = "100"
    //        };


    //        var result = client.PostAsync(EncompassURLConstant.GET_LOAN, GetByteArrayContent(_res)).Result;

    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            string res = result.Content.ReadAsStringAsync().Result;
    //            return (JsonConvert.DeserializeObject<List<EPipelineLoans>>(res)).Select(x => x.LoanGuid).ToList();
    //        }

    //        throw new EncompassWrapperException(result.Content.ReadAsStringAsync().Result);
    //    }

    //    public List<EAttachment> GetUnassignedAttachments(string loanGUID)
    //    {
    //        var result = client.GetAsync(string.Format(EncompassURLConstant.GET_UNATTACHMENTS, loanGUID)).Result;

    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            string res = result.Content.ReadAsStringAsync().Result;
    //            return JsonConvert.DeserializeObject<List<EAttachment>>(res);
    //        }

    //        throw new EncompassWrapperException("Error while fetching UnassignedAttachments", new Exception(result.Content.ReadAsStringAsync().Result));
    //    }

    //    public void UploadProcessFlag(string loanGUID, string fieldID, string fieldValue)
    //    {

    //        UpdateCustomFieldRequest _req = new UpdateCustomFieldRequest()
    //        {
    //            LoanGuid = loanGUID,
    //            Fields = new Dictionary<string, string>()
    //                {
    //                    { fieldID , fieldValue }
    //                }
    //        };

    //        var method = new HttpMethod("PATCH");

    //        var request = new HttpRequestMessage(method, EncompassURLConstant.UPDATE_CUSTOM_FIELD)
    //        {
    //            Content = GetByteArrayContent(_req)
    //        };

    //        var result = client.SendAsync(request).Result;

    //        if (result.StatusCode != System.Net.HttpStatusCode.OK)
    //        {
    //            throw new EncompassWrapperException($"Cannot able to update the field. Field ID : {fieldID}", new Exception(result.Content.ReadAsStringAsync().Result));
    //        }
    //    }


    //    public byte[] DownloadAttachment(string loanGUID, string attachmentGUID)
    //    {
           
    //        var result = client.GetAsync(string.Format(EncompassURLConstant.GET_DOWNLOAD_ATTACHMENT, loanGUID, attachmentGUID)).Result;

    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            return result.Content.ReadAsByteArrayAsync().Result;
    //        }

    //        throw new EncompassWrapperException("Error while downloading from UnassignedAttachments", new Exception(result.Content.ReadAsStringAsync().Result + ", AttachmentGUID : " + attachmentGUID));
    //    }


    //    private ByteArrayContent GetByteArrayContent(object data)
    //    {
    //        var myContent = JsonConvert.SerializeObject(data);
    //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
    //        ByteArrayContent byteContent = new ByteArrayContent(buffer);
    //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    //        return byteContent;
    //    }

    //    public void Dispose()
    //    {
    //        client.Dispose();
    //    }

    //    //added by mani
    //    public List<EContainer> GetAllLoanDocuments(string loanGUID)
    //    {
    //        var result = client.GetAsync(string.Format(EncompassURLConstant.GET_ALLLOANDOCUMENTS, loanGUID)).Result;

    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            string res = result.Content.ReadAsStringAsync().Result;
    //            return JsonConvert.DeserializeObject<List<EContainer>>(res);
    //        }

    //        throw new EncompassWrapperException("Error while fetching Parkingspots", new Exception(result.Content.ReadAsStringAsync().Result));
    //    }

    //    public AddContainerResponse AddDocument(string loanGUID, string documentName)
    //    {
    //        AddContainerRequest _req = new AddContainerRequest()
    //        {
    //            LoanGUID = loanGUID,
    //            DocumentName = documentName
    //        };

    //        var method = new HttpMethod("POST");

    //        var request = new HttpRequestMessage(method, EncompassURLConstant.ADD_DOCUMENT)
    //        {
    //            Content = GetByteArrayContent(_req)
    //        };

    //        var result = client.SendAsync(request).Result;

    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            string res = result.Content.ReadAsStringAsync().Result;
    //            return JsonConvert.DeserializeObject<AddContainerResponse>(res);
    //        }

    //        throw new EncompassWrapperException("Cannot able to add new parkingspot", new Exception(result.Content.ReadAsStringAsync().Result));
    //    }

    //    public bool AssignDocumentAttachments(string loanGUID, string documentGuid, List<string> attachmentGUIDs)
    //    {
    //        AssignAttachmentRequest _req = new AssignAttachmentRequest()
    //        {
    //            LoanGUID = loanGUID,
    //            DocumentGUID = documentGuid,
    //            AttachmentGUIDs = attachmentGUIDs

    //        };

    //        var method = new HttpMethod("PATCH");

    //        var request = new HttpRequestMessage(method, EncompassURLConstant.ASSIGN_DOCUMENT_ATTACHMENT)
    //        {
    //            Content = GetByteArrayContent(_req)
    //        };

    //        var result = client.SendAsync(request).Result;
    //        //check result and throw error
    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            string res = result.Content.ReadAsStringAsync().Result;
    //            EAddRemoveAttachmentResponse _res = JsonConvert.DeserializeObject<EAddRemoveAttachmentResponse>(res);
    //            if (_res.Status)
    //                return _res.Status;
    //        }

    //        throw new EncompassWrapperException("Cannot able to assign attachment to parkingspot", new Exception(result.Content.ReadAsStringAsync().Result + ",documentGuid : " + documentGuid));
    //    }

    //    public List<EFieldResponse> GetPredefinedFieldValues(string loanGUID, string[] fieldIds)
    //    {
    //        FieldGetRequest _req = new FieldGetRequest()
    //        {
    //            LoanGUID = loanGUID,
    //            FieldIDs = fieldIds
    //        };

    //        var method = new HttpMethod("POST");

    //        var request = new HttpRequestMessage(method, EncompassURLConstant.GET_PREDEFINED_FIELDVALUES)
    //        {
    //            Content = GetByteArrayContent(_req)
    //        };

    //        var result = client.SendAsync(request).Result;

    //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
    //        {
    //            string res = result.Content.ReadAsStringAsync().Result;
    //            return JsonConvert.DeserializeObject<List<EFieldResponse>>(res);
    //        }

    //        throw new EncompassWrapperException(result.Content.ReadAsStringAsync().Result);
    //    }

    //}

    //public class EncompassWrapperException : Exception
    //{
    //    public EncompassWrapperException(string message) : base(message)
    //    { }

    //    public EncompassWrapperException(string message, Exception innerException) : base(message, innerException)
    //    { }
    //}


}
