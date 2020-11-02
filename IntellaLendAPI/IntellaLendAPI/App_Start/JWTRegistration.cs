using IntellaLendJWTToken;
using System;
using System.Web.Configuration;

namespace IntellaLendAPI
{
    public class JWTRegistration
    {
        public static void RegisterJWT()
        {
            JWTToken._clientID = WebConfigurationManager.AppSettings["auth0:ClientId"];
            JWTToken._secretKey = WebConfigurationManager.AppSettings["auth0:ClientSecret"];
            JWTToken._domain = WebConfigurationManager.AppSettings["auth0:Domain"];
            int timeOut = 0;
            Int32.TryParse(WebConfigurationManager.AppSettings["auth0:TimeOut"], out timeOut);
            JWTToken._tokenTimeOut = timeOut;
        }
    }
}
