using IntellaLendAPI.App_Start;
using IntellaLendJWTToken;
using System.Web.Http;


namespace IntellaLendAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services           
            config.EnableCors();

            config.MessageHandlers.Add(new JsonWebTokenValidationHandler()
            {
                Audience = JWTToken._clientID,  // client id
                SymmetricKey = JWTToken._secretKey   // client secret
            });

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "CustomApi",
            //    routeTemplate: "api/{controller}/{action}/{TableSchema}",
            //    defaults: new { TableSchema = "" }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "CustomApi",
            //    routeTemplate: "api/{controller}/{action}"
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


            //To Trace 
            // unSecuredRoute.EnableSystemDiagnosticsTracing();
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
