using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MTS.Web.Helpers
{


    #region ApiException

    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }



        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }



        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }

    public partial class ApiException<TResult> : ApiException
    {
        public TResult Result { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }
    }

    #endregion

    public class ECMWebClient
    {
        private readonly System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient();
        private readonly System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        private KeyValuePair<string, string> Token { get; set; }
        private KeyValuePair<string, string> ValidationData { get; set; }

        public event EventHandler<WebClientArgs> RefreshToken;
        public event EventHandler<WebClientArgs> RefreshValidationData;

        private void OnRefreshToken(WebClientArgs e)
        {
            EventHandler<WebClientArgs> handler = RefreshToken;
            handler?.Invoke(this, e);
        }

        private void OnRefreshValidationData(WebClientArgs e)
        {
            EventHandler<WebClientArgs> handler = RefreshValidationData;
            handler?.Invoke(this, e);
        }

        private Newtonsoft.Json.JsonSerializerSettings CreateSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            return settings;
        }

        private string BaseUrl { get; }

        private Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }

        public ECMWebClient(string baseurl)
        {
            BaseUrl = baseurl;
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(CreateSerializerSettings);

        }

        private struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }

        private bool ReadResponseAsString { get; set; }

        private async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }

            if (ReadResponseAsString)
            {
                var responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    var typedBody = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, responseText);
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (var streamReader = new System.IO.StreamReader(responseStream))
                    using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(streamReader))
                    {
                        var serializer = Newtonsoft.Json.JsonSerializer.Create(JsonSerializerSettings);
                        var typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (Newtonsoft.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        private string BuildURL(Dictionary<string, object> urlandparams)
        {
            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/{urlandparams["URL"]}");
            for (int i = 2; i < urlandparams.Count; i++)
            {
                KeyValuePair<string, object> kv = urlandparams.ElementAt(i);
                urlBuilder.Append(System.Uri.EscapeDataString(kv.Key) + "=").Append(System.Uri.EscapeDataString(kv.Value.ToString())).Append(i < urlandparams.Count - 1 ? "&" : "");
            }

            return urlBuilder.ToString();

        }

        private string BuildURL(HttpRequestObject requestObject)
        {
            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append($"/{requestObject.URL}");
            if (requestObject.URLParams != null)
            {
                //for (int i = 0; i < requestObject.URLParams.Count; i++)
                //{
                //    KeyValuePair<string, object> kv = requestObject.URLParams[i];
                //    urlBuilder.Append(System.Uri.EscapeDataString(kv.Key) + "=").Append(System.Uri.EscapeDataString(kv.Value.ToString())).Append(i < requestObject.URLParams.Count - 1 ? "&" : "");
                //}

                foreach (var item in requestObject.URLParams)
                {
                    
                    urlBuilder.Append(System.Uri.EscapeDataString(item.Key) + "=").Append(System.Uri.EscapeDataString(item.Value.ToString())).Append("&");
                }
            }
            urlBuilder.Remove(urlBuilder.Length - 1, 1);
            return urlBuilder.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns>T</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<T> RequestAsync<T>(Dictionary<string, object> requestObject)
        {
            Logger.WriteTraceLog($"Calling Method RequestAsync URL:{requestObject["URL"]}");


            try
            {
                using (var request = new System.Net.Http.HttpRequestMessage())
                {
                    request.Method = new System.Net.Http.HttpMethod((string)requestObject["REQUESTTYPE"]);
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));


                    object content;
                    if (requestObject.TryGetValue("CONTENT", out content))
                    {
                        var contentjson = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content, _settings.Value), Encoding.UTF8, "application/json");
                        request.Content = contentjson;
                        requestObject.Remove("CONTENT");
                    }

                    var url_ = BuildURL(requestObject);
                    request.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    //Set Token and ValidationData
                    var token = new WebClientArgs();
                    OnRefreshToken(token);
                    request.Headers.Add(token.HeaderData.Key, token.HeaderData.Value);
                    token = new WebClientArgs();
                    OnRefreshValidationData(token);
                    request.Headers.Add(token.HeaderData.Key, token.HeaderData.Value);

                    var response = await _httpClient.SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }

                        var status = ((int)response.StatusCode).ToString();
                        if (status == "200")
                        {
                            var objectResponse = await ReadObjectResponseAsync<T>(response, headers).ConfigureAwait(false);
                            return objectResponse.Object;
                        }
                        else
                        if (status != "200" && status != "204")
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData, headers, null);
                        }

                        return default(T);
                    }
                    finally
                    {
                        if (response != null)
                            response.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new MTSException($"Api Request Exception URL:{requestObject["URL"]}", ex);
            }
            finally
            {
                Logger.WriteTraceLog($"Called Method RequestAsync URL:{requestObject["URL"]}");
            }
        }

        public async Task<T> RequestAsync<T>(HttpRequestObject requestObject)
        {
            Logger.WriteTraceLog($"Calling Method RequestAsync URL:{requestObject.URL}");


            try
            {
                using (var request = new System.Net.Http.HttpRequestMessage())
                {
                    request.Method = new System.Net.Http.HttpMethod((string)requestObject.REQUESTTYPE);
                    request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));


                    object content = requestObject.Content;
                    if (content != null)
                    {
                        var contentjson = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(content, _settings.Value));
                        request.Content = contentjson;
                    }

                    var url_ = BuildURL(requestObject);
                    request.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    //Set Token and ValidationData
                    var token = new WebClientArgs();
                    OnRefreshToken(token);
                    request.Headers.Add(token.HeaderData.Key, token.HeaderData.Value);
                    token = new WebClientArgs();
                    OnRefreshValidationData(token);
                    request.Headers.Add(token.HeaderData.Key, token.HeaderData.Value);

                    var response = await _httpClient.SendAsync(request, System.Net.Http.HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    try
                    {
                        var headers = System.Linq.Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                        if (response.Content != null && response.Content.Headers != null)
                        {
                            foreach (var item in response.Content.Headers)
                                headers[item.Key] = item.Value;
                        }

                        var status = ((int)response.StatusCode).ToString();
                        if (status == "200")
                        {
                            var objectResponse = await ReadObjectResponseAsync<T>(response, headers).ConfigureAwait(false);
                            return objectResponse.Object;
                        }
                        else
                        if (status != "200" && status != "204")
                        {
                            var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + (int)response.StatusCode + ").", (int)response.StatusCode, responseData, headers, null);
                        }

                        return default(T);
                    }
                    finally
                    {
                        if (response != null)
                            response.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new MTSException($"Api Request Exception URL:{requestObject.URL}", ex);
            }
            finally
            {
                Logger.WriteTraceLog($"Called Method RequestAsync URL:{requestObject.URL}");
            }
        }
    }
}