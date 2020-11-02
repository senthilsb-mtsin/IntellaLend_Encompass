namespace IntellaLendAPI.App_Start
{
    using IntellaLend.CommonServices;
    using IntellaLendJWTToken;
    using Models;
    using MTSEntBlocks.ExceptionBlock;
    using MTSEntBlocks.ExceptionBlock.Handlers;
    using MTSEntBlocks.LoggerBlock;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    public class JsonWebTokenValidationHandler : DelegatingHandler
    {
        public string SymmetricKey { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public bool IsSecretBase64Encoded { get; set; }

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

        private void CheckUserLogin(string requestJson, string requestPath)
        {
            if (JWTToken.HashToken != null)
            {
                Newtonsoft.Json.Linq.JObject requestObj = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(requestJson);

                if (requestObj != null)
                {

                    Int64 rUserID = 0;
                    if (requestPath.Contains("/api/User/SetSecurityQuestion"))
                    {
                        Int64.TryParse(requestObj["SecurityQuestion"]["UserID"].ToString(), out rUserID);
                    }
                    else if (requestObj["UserID"] != null)
                    {
                        Int64.TryParse(requestObj["UserID"].ToString(), out rUserID);
                    }

                    Int64 sessionUserID = new logOnService(JWTToken.HashToken.Schema).GetUserHash(JWTToken.HashToken.HashSet);

                    if (rUserID > 0 && !rUserID.Equals(new logOnService(JWTToken.HashToken.Schema).GetUserHash(JWTToken.HashToken.HashSet)))
                        throw new Exception("User Authentication Failed");

                }
            }
        }

        private void LogUserAcivity(HttpRequestMessage request, bool isActive)
        {

            if (request.Content != null)
            {
                string requestJson = request.Content.ReadAsStringAsync().Result;
                LogMessage($"Request BODY : {requestJson}");

                if (!request.RequestUri.LocalPath.Contains("/api/FileUpload") && !request.RequestUri.LocalPath.Contains("/api/IntellaLend/ReverificationFileUploader"))
                {
                    LogMessage($"Step 1 : {request.RequestUri.LocalPath}");
                    CheckUserLogin(requestJson, request.RequestUri.LocalPath);
                }

                RequestUserInfo userInfo = GetRequestUserInfo(requestJson);
                if (userInfo.RequestUserID != 0 && !string.IsNullOrEmpty(userInfo.RequestUserTableSchema))
                {
                    UserService userService = new UserService(userInfo.RequestUserTableSchema);
                    userService.AddUserSession(userInfo.RequestUserID, isActive);
                }
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

            if (request.RequestUri.LocalPath.Contains("/api/Login/LoginSubmit") || request.RequestUri.LocalPath.Contains("/api/Image/") || request.RequestUri.LocalPath.Contains("ErrorHandler"))
            {
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
                        Exception exe = new Exception(ex.Message, ex);
                        MTSExceptionHandler.HandleException(ref exe);
                        errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                    }
                    catch (Exception ex)
                    {
                        Exception exe = new Exception(ex.Message, ex);
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
    }
}
