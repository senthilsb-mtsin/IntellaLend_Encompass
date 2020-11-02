using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace IntellaLendAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "GetImage",
               url: "api/{controller}/{action}/{TenantSchema}/{LoanID}/{Guid}",
               defaults: new { TenantSchema = RouteParameter.Optional, LoanID = RouteParameter.Optional, Guid = RouteParameter.Optional }
           );


            routes.MapRoute(
              name: "GetAuditRport",
              url: "api/{controller}/{action}/{TenantSchema}/{LoanID}",
              defaults: new { TenantSchema = RouteParameter.Optional, LoanID = RouteParameter.Optional }
          );
        }
    }
}
