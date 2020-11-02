using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using MTSEntBlocks.ExceptionBlock.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EncompassConsoleConnector
{
    public class EncompassConnectorApp
    {
        //static Int32 eConnectorExecutionMilliSeconds = 30 * 60000;
        //static string eConnectorPath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "EncompassConnector.exe");

        #region Private Variables

        private static string _encompassServerURL;

        #endregion

        #region Public Variables



        #endregion

        #region Constructor

        static EncompassConnectorApp()
        {
            //_server = ConfigurationManager.AppSettings["EncompassServer"];// EncompassConnectorDataAccess.ENCOMPASS_SERVER;
            //_userName = ConfigurationManager.AppSettings["EncompassUserName"];//EncompassConnectorDataAccess.ENCOMPASS_USERNAME;
            //_password = ConfigurationManager.AppSettings["EncompassPassword"];//EncompassConnectorDataAccess.ENCOMPASS_PASSWORD;
            _encompassServerURL = ConfigurationManager.AppSettings["EncompassServerURL"];
        }

        #endregion

        public static string QueryEncompass(string LoanGUID, string FieldID, string TenantSchema)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_encompassServerURL);
                    EncompassAccessToken _token = SetToken(TenantSchema);
                    string[] fields = { FieldID };

                    client.DefaultRequestHeaders.Add(EncompassConstant.TokenHeader, _token.AccessToken);
                    client.DefaultRequestHeaders.Add(EncompassConstant.TokenTypeHeader, _token.TokenType);

                    FieldGetRequest req = new FieldGetRequest() {
                        LoanGUID = LoanGUID,
                        FieldIDs = fields
                    };

                    var method = new HttpMethod("POST");

                    var request = new HttpRequestMessage(method, "/api/EncompassField/GetPreDefinedFieldValues")
                    {
                        Content = GetByteArrayContent(req)
                    };

                    HttpResponseMessage result = client.SendAsync(request).Result;

                    string res = result.Content.ReadAsStringAsync().Result;
                    
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (!string.IsNullOrEmpty(res))
                        {
                            List<EFieldResponse> _res = JsonConvert.DeserializeObject<List<EFieldResponse>>(res);
                            if (_res.Count > 0)
                            {
                                return _res[0].Value;
                            }
                        }
                        return "";
                    }

                    ErrorResponse _error = JsonConvert.DeserializeObject<ErrorResponse>(res);

                    throw new Exception($"{_error.Summary}");
                    
                }
            }
            catch (Exception ex)
            {
                Exception exe = new Exception($"Error Requesting Encompass loanGUID : {LoanGUID}", ex);
                MTSExceptionHandler.HandleException(ref exe);
            }

            return null;
            
        }

        private static EncompassAccessToken SetToken(string TenantSchema)
        {
            EncompassInterseptorDataAccess _dataAccess = new EncompassInterseptorDataAccess(TenantSchema);
            EncompassAccessToken _token = _dataAccess.GetDBToken();
            List<EncompassConfig> _config = _dataAccess.GetEncompassConfig();

            if (_config.Count == 0)
                throw new Exception("Encompass Token Configuration not available");

            string clientID = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_ID).FirstOrDefault().ConfigValue;
            string clientSecret = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_SECRET).FirstOrDefault().ConfigValue;

            bool validToken = false;

            if (_token != null)
                validToken = CheckValidToken(_token.AccessToken, clientID, clientSecret);

            if (!validToken)
            {
                EToken newToken = GetToken(_config, clientID, clientSecret);
                if (newToken != null)
                {
                    _dataAccess.UpdateNewToken(newToken.TokenType, newToken.AccessToken);
                    _token.AccessToken = newToken.AccessToken;
                    _token.TokenType = newToken.TokenType;
                }
            }

            return _token;
        }
        private static bool CheckValidToken(string _token, string _clientID, string _clientSecret)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_encompassServerURL);
                TokenValidateRequest _res = new TokenValidateRequest()
                {
                    AccessToken = _token,
                    ClientID = _clientID,
                    ClientSecret = _clientSecret
                };

                var result = client.PostAsync(EncompassURLConstant.VALIDATE_TOKEN, GetByteArrayContent(_res)).Result;

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string res = result.Content.ReadAsStringAsync().Result;
                    ETokenValidateResponse apiResult = JsonConvert.DeserializeObject<ETokenValidateResponse>(res);
                    return apiResult.ValidToken;
                }
            }

            return false;
        }

        private static EToken GetToken(List<EncompassConfig> _config, string _clientID, string _clientSecret)
        {
            string grantType = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.GRANT_TYPE).FirstOrDefault().ConfigValue;
            string scope = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.SCOPE).FirstOrDefault().ConfigValue;
            EncompassConfig instanctID = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.INSTANCE_ID).FirstOrDefault();
            EncompassConfig userName = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.USERNAME).FirstOrDefault();
            EncompassConfig password = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.PASSWORD).FirstOrDefault();

            using (var client = new HttpClient())
            {
                ByteArrayContent byteContent = null;
                string url = string.Empty;
                client.BaseAddress = new Uri(_encompassServerURL);
                if (grantType == "password")
                {
                    ETokenUserRequest _res = new ETokenUserRequest()
                    {
                        ClientID = _clientID,
                        ClientSecret = _clientSecret,
                        GrantType = grantType,
                        Scope = scope,
                        InstanceID = instanctID != null ? instanctID.ConfigValue : string.Empty,
                        UserName = userName != null ? userName.ConfigValue : string.Empty,
                        Password = password != null ? password.ConfigValue : string.Empty
                    };
                    byteContent = GetByteArrayContent(_res);
                    url = EncompassURLConstant.GET_TOKEN_WITH_USER;
                }
                else
                {
                    ETokenRequest _res = new ETokenRequest()
                    {
                        ClientID = _clientID,
                        ClientSecret = _clientSecret,
                        GrantType = grantType,
                        Scope = scope,
                        InstanceID = instanctID != null ? instanctID.ConfigValue : string.Empty
                    };
                    byteContent = GetByteArrayContent(_res);
                    url = EncompassURLConstant.GET_TOKEN;
                }

                var result = client.PostAsync(url, byteContent).Result;

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string res = result.Content.ReadAsStringAsync().Result;
                    Logger.Write(res);
                    return JsonConvert.DeserializeObject<EToken>(res);
                }
                Logger.Write(result.StatusCode);
            }

            return null;

        }


        private static ByteArrayContent GetByteArrayContent(object data)
        {
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
        
    }

    public class FieldGetRequest
    {
        [JsonProperty(PropertyName = "loanGUID")]
        public string LoanGUID { get; set; }

        [JsonProperty(PropertyName = "fieldIDs")]
        public string[] FieldIDs { get; set; }

    }
    public class EFieldResponse
    {
        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
    public class ErrorResponse
    {
        public string Summary { get; set; }
        public string Details { get; set; }
        public string ErrorCode { get; set; }

    }
}
