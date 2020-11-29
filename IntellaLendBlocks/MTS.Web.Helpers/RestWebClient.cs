using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTS.Web.Helpers
{


    public class RestWebClient : RestClient, IDisposable
    {
        // RestClient restClient;
        private bool disposedValue;

        public event EventHandler<WebClientArgs> RefreshValidationHeaders;

        private void OnRefreshValidationHeaders(RestRequest restRequest)
        {
            if (RefreshValidationHeaders != null)
            {
                foreach (EventHandler<WebClientArgs> item in RefreshValidationHeaders.GetInvocationList())
                {
                    WebClientArgs args = new WebClientArgs();
                    item.Invoke(this, args);
                    if (!(args.HeaderData.Equals(new KeyValuePair<string, string>())))
                        restRequest.AddHeader(args.HeaderData.Key, args.HeaderData.Value);

                    if (args.HeaderDataList != null && args.HeaderDataList.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> itemDic in args.HeaderDataList)
                        {
                            restRequest.AddHeader(itemDic.Key, itemDic.Value);
                        }
                    }
                }
            }
        }


        public RestWebClient(string baseURL) : base(baseURL)
        {
            FailOnDeserializationError = false;
            ThrowOnDeserializationError = false;
            ThrowOnAnyError = true;


            JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

        }

        private RestRequest InitRestRequest(HttpRequestObject httpRequestObject)
        {
            var restRequest = new RestRequest(httpRequestObject.URL);

            if (httpRequestObject.URLParams != null)
            {
                foreach (var kv in httpRequestObject.URLParams)
                {
                    restRequest.AddOrUpdateParameter(kv.Key, kv.Value, ParameterType.QueryString);
                }
            }


            if (httpRequestObject.Headers != null)
            {
                foreach (var kv in httpRequestObject.Headers)
                {
                    restRequest.AddHeader(kv.Key, kv.Value);
                }
            }

            restRequest.AddHeader("content-type", httpRequestObject.RequestContentType);

            switch (httpRequestObject.REQUESTTYPE)
            {
                case "POST":
                    restRequest.Method = Method.POST;
                    SetRequestBody(httpRequestObject, restRequest);
                    break;
                case "PUT":
                    restRequest.Method = Method.PUT;
                    SetRequestBody(httpRequestObject, restRequest);
                    break;
                case "PATCH":
                    restRequest.Method = Method.PATCH;
                    SetRequestBody(httpRequestObject, restRequest);
                    break;
                case "DELETE":
                    restRequest.Method = Method.DELETE;
                    break;
                default:
                    restRequest.Method = Method.GET;
                    break;
            }

            return restRequest;

        }

        private void SetRequestBody(HttpRequestObject httpRequestObject, RestRequest restRequest)
        {
            if (httpRequestObject.Content != null)
            {
                if (httpRequestObject.RequestContentType == "application/json")
                    restRequest.AddJsonBody(httpRequestObject.Content);
                else if (httpRequestObject.RequestContentType == "application/x-www-form-urlencoded")
                {
                    List<string> param = ((Dictionary<string, string>)httpRequestObject.Content).Select(item => $"{item.Key}={item.Value}").ToList();
                    restRequest.AddOrUpdateParameter(httpRequestObject.RequestContentType, string.Join("&", param), ParameterType.RequestBody);
                }
                else if (httpRequestObject.RequestContentType == "multipart/form-data")
                    restRequest.AddFileBytes(httpRequestObject.Content.FileName, httpRequestObject.FileStream, httpRequestObject.Content.FileName);
            }
        }

        public RestSharp.IRestResponse<T> Execute<T>(HttpRequestObject requestObject)
        {
            var restRequest = InitRestRequest(requestObject);

            OnRefreshValidationHeaders(restRequest);

            return Execute<T>(restRequest);
        }

        public RestSharp.IRestResponse Execute(HttpRequestObject requestObject)
        {
            var restRequest = InitRestRequest(requestObject);

            OnRefreshValidationHeaders(restRequest);

            return Execute(restRequest);
        }


        public Task<IRestResponse<T>> ExecuteAsync<T>(HttpRequestObject requestObject)
        {
            var restRequest = InitRestRequest(requestObject);

            OnRefreshValidationHeaders(restRequest);

            return ExecuteAsync<T>(restRequest);
        }


        public Task<T> RequestAsync<T>(HttpRequestObject requestObject)
        {
            RestRequest restRequest;
            if (requestObject.RequestContentType == "FILE")
            {
                restRequest = new RestRequest(requestObject.URL);
                restRequest.AddFileBytes("", requestObject.FileStream, "");
            }
            else
                restRequest = new RestRequest(requestObject.URL, DataFormat.Json);

            if (requestObject.URLParams != null)
            {
                foreach (var kv in requestObject.URLParams)
                {
                    restRequest.AddParameter(kv.Key, kv.Value);
                }
            }

            if (requestObject.Headers != null)
            {
                foreach (var kv in requestObject.Headers)
                {
                    restRequest.AddHeader(kv.Key, kv.Value);
                }
            }

            OnRefreshValidationHeaders(restRequest);

            if (requestObject.RequestContentType != "FILE" && requestObject.RequestContentType != "DELETE" && requestObject.Content != null)
            {
                restRequest.AddJsonBody(requestObject.Content);
            }

            switch (requestObject.REQUESTTYPE)
            {
                case "POST":
                    return this.PostAsync<T>(restRequest);
                case "PUT":
                    return this.PutAsync<T>(restRequest);
                case "PATCH":
                    return this.PatchAsync<T>(restRequest);
                case "DELETE":
                    return this.DeleteAsync<T>(restRequest);
                default:
                    return this.GetAsync<T>(restRequest);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RefreshValidationHeaders = null;
                    // restClient = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
