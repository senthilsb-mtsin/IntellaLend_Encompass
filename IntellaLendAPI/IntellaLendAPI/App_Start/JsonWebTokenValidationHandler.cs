namespace IntellaLendAPI.App_Start
{
    using IntellaLend.CommonServices;
    using Models;
    using MTSEntBlocks.ExceptionBlock;
    using MTSEntBlocks.ExceptionBlock.Handlers;
    using MTSEntBlocks.LoggerBlock;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    public class JsonWebTokenValidationHandler : DelegatingHandler
    {
        public string SymmetricKey { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public bool IsSecretBase64Encoded { get; set; }
        private string ipAddress { get; set; }
        private string browser { get; set; }
        private string userHostName { get; set; }
        private string device { get; set; }
        public JsonWebTokenValidationHandler()
        {
            IsSecretBase64Encoded = true;
        }

        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;

            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                // Fail if no Authorization header or more than one Authorization headers  
                // are found in the HTTP request  
                return false;
            }

            // Remove the bearer token scheme prefix and return the rest as ACS token  
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return true;
        }

        //Parsing can modify here if need more info
        private RequestUserInfo GetRequestUserInfo(string requestJson)
        {
            RequestUserInfo userInfo = new RequestUserInfo();
            try
            {
                Newtonsoft.Json.Linq.JObject requestObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(requestJson);

                if (requestObj != null && requestObj["RequestUserInfo"] != null)
                {
                    if (requestObj["RequestUserInfo"]["RequestUserID"] != null)
                    {
                        Int64 requestUserID = 0;
                        Int64.TryParse(Convert.ToString(requestObj["RequestUserInfo"]["RequestUserID"]), out requestUserID);
                        userInfo.RequestUserID = requestUserID;
                    }
                    if (requestObj["RequestUserInfo"]["RequestUserTableSchema"] != null)
                    {
                        userInfo.RequestUserTableSchema = Convert.ToString(requestObj["RequestUserInfo"]["RequestUserTableSchema"]);
                    }

                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                throw new MTSException(ex.Message, ex);
            }


            return userInfo;
        }

        private void CheckUserLogin(HttpRequestMessage request, string requestJson, string requestPath, string ipAddress, string device, string browser, string userHostName)
        {
            try
            {
                if (request.Headers.Contains("HashValue") && request.Headers.Contains("TenantDBSchema") && !string.IsNullOrEmpty(requestJson))
            {
                Newtonsoft.Json.Linq.JObject requestObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(requestJson);

                if (requestObj != null)
                {

                    Int64 rUserID = 0;
                    if (requestPath.Contains("/api/User/SetSecurityQuestion") && requestObj["SecurityQuestion"] != null && requestObj["SecurityQuestion"]["UserID"] != null)
                    {
                        Int64.TryParse(requestObj["SecurityQuestion"]["UserID"].ToString(), out rUserID);
                    }
                    else if (requestObj["RequestUserInfo"] != null && requestObj["RequestUserInfo"]["RequestUserID"] != null)
                    {
                        Int64.TryParse(requestObj["RequestUserInfo"]["RequestUserID"].ToString(), out rUserID);
                    }
                    else if (requestObj["UserID"] != null)
                    {
                        Int64.TryParse(requestObj["UserID"].ToString(), out rUserID);
                    }

                        string schema = request.Headers.GetValues("TenantDBSchema").FirstOrDefault().ToString();
                        string _hash = request.Headers.GetValues("HashValue").FirstOrDefault().ToString();

                        Int64 sessionUserID = new logOnService(schema).GetUserHash(_hash);
                        if (rUserID > 0 && !rUserID.Equals(sessionUserID))
                        {
                            RequestUserInfo userInfo = GetRequestUserInfo(requestJson);
                            if (userInfo.RequestUserID != 0 && !string.IsNullOrEmpty(userInfo.RequestUserTableSchema))
                            {
                                //Int64 auditSessionUserID = new logOnService(schema).GetAuditUserHash(_hash);
                                UserService userService = new UserService(userInfo.RequestUserTableSchema);
                                userService.CreateAuditUserSession(rUserID, false, _hash, requestPath, ipAddress, device, browser, userHostName);
                                throw new Exception($"User Authentication Failed. RequestURL : {requestPath}. Request Body : {requestJson}");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LogUserAcivity(HttpRequestMessage request, bool isActive)
        {

            if (request.Content != null)
            {
                byte[] requestJsonBytes = request.Content.ReadAsByteArrayAsync().Result;
                string requestJson = Encoding.UTF8.GetString(requestJsonBytes);
                LogMessage($"Request BODY : {requestJson}");

                if (!request.RequestUri.LocalPath.Contains("/api/FileUpload"))
                {
                    if (!request.RequestUri.LocalPath.Contains("/api/IntellaLend/ReverificationFileUploader"))
                    {
                        LogMessage($"Step 1 : {request.RequestUri.LocalPath}");
                        CheckUserLogin(request, requestJson, request.RequestUri.LocalPath, ipAddress, device, browser, userHostName);
                    }
                    CreateUserSession(request, requestJson, isActive);
                }
            }
        }
        private void CreateUserSession(HttpRequestMessage request, string requestJson, bool isActive)
        {
            if (request.RequestUri.LocalPath.Contains("/api/Login/LockUnlockUser"))
            {
                Newtonsoft.Json.Linq.JObject requestObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(requestJson);
                if (requestObj != null)
                {
                    if (requestObj["Lock"] != null)
                    {
                        bool.TryParse(requestObj["Lock"].ToString(), out isActive);
                    }
                }
            }

                    RequestUserInfo userInfo = GetRequestUserInfo(requestJson);
                    if (userInfo.RequestUserID != 0 && !string.IsNullOrEmpty(userInfo.RequestUserTableSchema))
                    {
                        UserService userService = new UserService(userInfo.RequestUserTableSchema);
                        userService.AddUserSession(userInfo.RequestUserID, isActive);
                string _hash = request.Headers.GetValues("HashValue").FirstOrDefault().ToString();
                userService.CreateAuditUserSession(userInfo.RequestUserID, isActive, _hash, request.RequestUri.LocalPath, ipAddress, device, browser, userHostName);
            }
        }

        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token;
            HttpResponseMessage errorResponse = null;

            LogMessage($"Request URL : {request.RequestUri.LocalPath}");

            if (request.Content == null)
                LogMessage($"Request Has NO Content");

            ipAddress = HttpContext.Current.Request.UserHostAddress;
            LogMessage($"IpAddress User Request Server : { ipAddress}");

            browser = HttpContext.Current.Request.Browser.Browser;
            LogMessage($"User Browser : { browser}");

            userHostName = HttpContext.Current.Request.UserHostName;
            LogMessage($"User Host Name : { userHostName}");

            if (HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                device = HttpContext.Current.Request.UserAgent;
            }
            else
            {
                device = HttpContext.Current.Request.Browser.Platform;
            }
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);
            var requestInfo = string.Format("{0} {1}", request.Method, request.RequestUri);
            var requestMessage = request.Content.ReadAsByteArrayAsync().Result;

            IncommingMessageAsync(corrId, requestInfo, requestMessage);
            if (request.RequestUri.LocalPath.Contains("/api/Login/LoginSubmit") || request.RequestUri.LocalPath.Contains("/api/Login/LockUnlockUser") || request.RequestUri.LocalPath.Contains("/api/Image/") || request.RequestUri.LocalPath.Contains("ErrorHandler"))
            {
                if (request.RequestUri.LocalPath.Contains("/api/Login/LockUnlockUser"))
                {
                    if (TryRetrieveToken(request, out token))
                    {
                        byte[] secret = Base64UrlDecode(this.SymmetricKey);

                        var payloadJson = JWT.JsonWebToken.Decode(token, secret, verify: true);
                        var payloadData = JObject.Parse(payloadJson).ToObject<Dictionary<string, object>>();
                        JWTTokenHashTemp hashValue = JObject.FromObject(payloadData["data"]).ToObject<JWTTokenHashTemp>();
                        request.Headers.Add("HashValue", hashValue.HashSet);
                        request.Headers.Add("TenantDBSchema", hashValue.Schema);
                        CreateUserSession(request, Encoding.UTF8.GetString(requestMessage), false);
                    }

                }
                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                if (TryRetrieveToken(request, out token))
                {
                    try
                    {
                        var secret = this.SymmetricKey;
                        //if (IsSecretBase64Encoded)
                        //secret = secret.Replace('-', '+').Replace('_', '/');

                        Thread.CurrentPrincipal = JsonWebToken.ValidateToken(
                            token,
                            secret,
                            request,
                            audience: this.Audience,
                            checkExpiration: true,
                            issuer: this.Issuer,
                            isSecretBase64Encoded: IsSecretBase64Encoded);

                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.User = Thread.CurrentPrincipal;
                        }

                        // CheckUserLogin();
                    }
                    catch (JWT.SignatureVerificationException ex)
                    {
                        Exception exe = new Exception(ex.Message, ex);
                        MTSExceptionHandler.HandleException(ref exe);
                        errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex);
                    }
                    catch (JsonWebToken.TokenValidationException ex)
                    {
                        Exception exe = new Exception(ex.Message, ex);
                        MTSExceptionHandler.HandleException(ref exe);
                        errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex);
                    }
                    catch (Exception ex)
                    {
                        Exception exe = new Exception(ex.Message, ex);
                        MTSExceptionHandler.HandleException(ref exe);
                        errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                    }
                    //finally
                    //{

                    //}

                    try
                    {
                        bool isAcvtice = errorResponse == null;
                        LogUserAcivity(request, isAcvtice);
                    }
                    catch (MTSException ex)
                    {
                        Exception exe = new Exception($"ErrorMessage : {ex.Message}. RequestURL: {request.RequestUri.LocalPath}. Request Body : {Encoding.UTF8.GetString(requestMessage)}", ex);
                        MTSExceptionHandler.HandleException(ref exe);
                        errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                    }
                    catch (Exception ex)
                    {

                        Exception exe = new Exception($"ErrorMessage : {ex.Message}. RequestURL: {request.RequestUri.LocalPath}. Request Body : {Encoding.UTF8.GetString(requestMessage)}", ex);
                        MTSExceptionHandler.HandleException(ref exe);
                        errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex);
                    }
                }

                if (errorResponse != null)
                {
                    LogMessage($"Got error for request URL : {request.RequestUri.LocalPath}");
                    return Task.FromResult(errorResponse);
                }
                else
                {
                    var response = base.SendAsync(request, cancellationToken);
                    LogMessage($"Response Ready for the request URL : {request.RequestUri.LocalPath}");
                    return response;
                }
            }
        }
        private static byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default:
                    throw new System.Exception(
             "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }
        protected async Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {
            await Task.Run(() =>
                Logger.WriteTraceLog(string.Format("{0} - Request: {1}\r\n{2}", correlationId, requestInfo, Encoding.UTF8.GetString(message))));
        }
        public class JWTTokenHashTemp
        {
            public string HashSet { get; set; }
            public string Schema { get; set; }
        }
    }
}
