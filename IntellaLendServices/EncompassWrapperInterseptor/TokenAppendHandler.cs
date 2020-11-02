using EncompassRequestBody.ERequestModel;
using EncompassRequestBody.WrapperReponseModel;
using EncompassRequestBody.WrapperRequestModel;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace EncompassWrapperInterseptor
{
    public class TokenAppendHandler : DelegatingHandler
    {
        private string _tenantSchema;
        private string _apiURL;
        private EncompassInterseptorDataAccess _dataAccess;

        public TokenAppendHandler(string TenantSchema, string APIUrl) : base(new HttpClientHandler())
        { _tenantSchema = TenantSchema; _apiURL = APIUrl; _dataAccess = new EncompassInterseptorDataAccess(_tenantSchema); }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            EncompassAccessToken _token = _dataAccess.GetDBToken();
            List<EncompassConfig> _config = _dataAccess.GetEncompassConfig();

            if (_config.Count == 0)
                throw new Exception("Encompass Token Configuration not available");

            string clientID = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_ID).FirstOrDefault().ConfigValue;
            string clientSecret = _config.Where(c => c.Type.Contains(EncompassConstant.ValidateToken) && c.ConfigKey == EncompassConfigConstant.CLIENT_SECRET).FirstOrDefault().ConfigValue;

            bool validToken = false;

            if (_token != null)
                validToken = CheckValidToken(_token.AccessToken, clientID, clientSecret);

            Logger.WriteTraceLog($" _token validToken: { validToken}");

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
            Logger.WriteTraceLog($" _token.AccessToken: { _token.AccessToken}");
            request.Headers.Add(EncompassConstant.TokenHeader, _token.AccessToken);
            request.Headers.Add(EncompassConstant.TokenTypeHeader, _token.TokenType);

            return await base.SendAsync(request, cancellationToken);
        }

        private bool CheckValidToken(string _token, string _clientID, string _clientSecret)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiURL);
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

        private EToken GetToken(List<EncompassConfig> _config, string _clientID, string _clientSecret)
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
                client.BaseAddress = new Uri(_apiURL);
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
                    Logger.WriteTraceLog(res);
                    return JsonConvert.DeserializeObject<EToken>(res);
                }
                Logger.WriteTraceLog(result.StatusCode.ToString());
            }

            return null;

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
