using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.WrapperReponseModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.Web.Helpers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace EncompassWrapperInterseptor
{
    public class TokenAppendHandler
    {
        private string _tenantSchema;
        private string _apiURL;
        private EncompassInterseptorDataAccess _dataAccess;

        public TokenAppendHandler(string TenantSchema, string APIUrl)
        {
            _tenantSchema = TenantSchema;
            _apiURL = APIUrl;
            _dataAccess = new EncompassInterseptorDataAccess(_tenantSchema);
        }

        public void GetTokenFromDB(object sender, WebClientArgs e)
        {
            EncompassAccessToken _token = _dataAccess.GetDBToken();
            List<EncompassConfig> _config = _dataAccess.GetEncompassConfig();

            if (_config.Count == 0)
                throw new Exception("Encompass Token Configuration not available");

            string clientID = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_ID).FirstOrDefault().ConfigValue;
            string clientSecret = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_SECRET).FirstOrDefault().ConfigValue;

            //bool validToken = false;

            //if (_token != null)
            //    validToken = CheckValidToken(_token.AccessToken, clientID, clientSecret);

            Logger.WriteTraceLog($" _token not available in Database");

            if (_token == null)
            {
                EToken newToken = GetToken(_config, clientID, clientSecret);
                if (newToken != null)
                {
                    _dataAccess.UpdateNewToken(newToken.TokenType, newToken.AccessToken);
                    _token.AccessToken = newToken.AccessToken;
                    _token.TokenType = newToken.TokenType;
                }
            }
            Logger.WriteTraceLog($" _token.AccessToken: { _token.AccessToken}");
            e.HeaderDataList.Add(EncompassConstant.TokenHeader, _token.AccessToken);
            e.HeaderDataList.Add(EncompassConstant.TokenTypeHeader, _token.TokenType);
        }

        //No need to validate token aas per JOHN comments
        //private bool CheckValidToken(string _token, string _clientID, string _clientSecret)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(_apiURL);
        //        TokenValidateRequest _res = new TokenValidateRequest()
        //        {
        //            AccessToken = _token,
        //            ClientID = _clientID,
        //            ClientSecret = _clientSecret
        //        };

        //        var result = client.PostAsync(EncompassURLConstant.VALIDATE_TOKEN, GetByteArrayContent(_res)).Result;

        //        if (result.StatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            string res = result.Content.ReadAsStringAsync().Result;
        //            ETokenValidateResponse apiResult = JsonConvert.DeserializeObject<ETokenValidateResponse>(res);
        //            return apiResult.ValidToken;
        //        }
        //    }

        //    return false;
        //}

        private EToken GetToken(List<EncompassConfig> _config, string _clientID, string _clientSecret)
        {
            string grantType = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.GRANT_TYPE).FirstOrDefault().ConfigValue;
            string scope = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.SCOPE).FirstOrDefault().ConfigValue;
            EncompassConfig instanctID = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.INSTANCE_ID).FirstOrDefault();
            EncompassConfig userName = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.USERNAME).FirstOrDefault();
            EncompassConfig password = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.PASSWORD).FirstOrDefault();

            RestWebClient client = new RestWebClient(_apiURL);

            HttpRequestObject req = null;
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

                req = new HttpRequestObject() { Content = _res, REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.GET_TOKEN_WITH_USER) };
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
                req = new HttpRequestObject() { Content = _res, REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.GET_TOKEN) };
            }

            IRestResponse result = client.Execute(req);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string res = result.Content;
                Logger.WriteTraceLog(res);
                return JsonConvert.DeserializeObject<EToken>(res);
            }
            Logger.WriteTraceLog(result.StatusCode.ToString());

            return null;

        }

        public void SetToken()
        {
            List<EncompassConfig> _config = _dataAccess.GetEncompassConfig();
            string _clientID = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_ID).FirstOrDefault().ConfigValue;
            string _clientSecret = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_SECRET).FirstOrDefault().ConfigValue;
            string grantType = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.GRANT_TYPE).FirstOrDefault().ConfigValue;
            string scope = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.SCOPE).FirstOrDefault().ConfigValue;
            EncompassConfig instanctID = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.INSTANCE_ID).FirstOrDefault();
            EncompassConfig userName = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.USERNAME).FirstOrDefault();
            EncompassConfig password = _config.Where(c => c.Type.Contains(EncompassConstant.RequestToken) && c.ConfigKey == EncompassConfigConstant.PASSWORD).FirstOrDefault();

            RestWebClient client = new RestWebClient(_apiURL);

            HttpRequestObject req = null;
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

                req = new HttpRequestObject() { Content = _res, REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.GET_TOKEN_WITH_USER) };
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
                req = new HttpRequestObject() { Content = _res, REQUESTTYPE = "POST", URL = string.Format(EncompassURLConstant.GET_TOKEN) };
            }

            IRestResponse result = client.Execute(req);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string res = result.Content;
                Logger.WriteTraceLog(res);
                EToken newToken = JsonConvert.DeserializeObject<EToken>(res);
                _dataAccess.UpdateNewToken(newToken.TokenType, newToken.AccessToken);
            }
            Logger.WriteTraceLog(result.StatusCode.ToString());
        }


        private ByteArrayContent GetByteArrayContent(object data)
        {
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}
