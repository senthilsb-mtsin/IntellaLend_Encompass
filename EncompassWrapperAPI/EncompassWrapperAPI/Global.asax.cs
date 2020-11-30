using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EncompassConnectorAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static List<Dictionary<string, string>> exceptions = new List<Dictionary<string, string>>();
        public static List<Dictionary<string, string>> Exceptions { get { return exceptions; } }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        public static void AddException(Exception ex)
        {
            var error = new Dictionary<string, string>
            {
                {"TimeStamp", DateTime.UtcNow.ToString()},
                {"Type", ex.GetType().ToString()},
                {"Message", ex.Message},
                {"StackTrace", ex.StackTrace}
            };
            foreach (DictionaryEntry data in ex.Data)
                error.Add(data.Key.ToString(), data.Value.ToString());

            Exceptions.Add(error);
            //Store only last 5 exceptions
            if (Exceptions.Count >= 5)
            {
                Exceptions.RemoveAt(5);
            }
        }

    }
}
