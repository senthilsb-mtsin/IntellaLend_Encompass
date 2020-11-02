using IntellaLend.CommonServices;
using IntellaLend.Model;
using MTSEntBlocks.ExceptionBlock.Handlers;
using Newtonsoft.Json;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Linq;

namespace IntellaLendAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private string _parameters = string.Empty;
        private DateTime _startDateTime = DateTime.Now;

        protected void Application_Start()
        {
            MiniProfilerEF6.Initialize();
            JWTRegistration.RegisterJWT();
            LicenseRegistration.RegisterLicense();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["EncompassDocFields"] = GetLosFields();
            Application["LoanTapeDocFields"] = new object();
        }
        
        protected void Application_BeginRequest()
        {
            if (Request.Url.LocalPath.EndsWith("/api/Loan/GetLoanBase64Images") && RequestLogging.Enabled)
            {
                _startDateTime = DateTime.Now;
                using (MemoryStream destination = new MemoryStream())
                {
                    Request.InputStream.CopyTo(destination);
                    byte[] byteInput = destination.ToArray();
                    destination.Seek(0, SeekOrigin.Begin);
                    _parameters = Encoding.UTF8.GetString(byteInput, 0, byteInput.Length);
                }
                MiniProfiler.Start();
            }
        }
        protected List<EncompassDocFields> GetLosFields()
        {
            List<EncompassDocFields> ls = new List<EncompassDocFields>(); ;
            IntellaLendServices _serv = new IntellaLendServices("IL");
            ls = _serv.GetLOSDocFields();
            return ls;
        }

        protected void Application_EndRequest()
        {
            if (Request.Url.LocalPath.EndsWith("/api/Loan/GetLoanBase64Images") && RequestLogging.Enabled)
            {
                if (MiniProfiler.Current != null && MiniProfiler.Current.Root != null)
                {
                    MiniProfiler.Stop();
                    if (!string.IsNullOrEmpty(_parameters))
                    {
                        string log = MiniProfiler.ToJson();
                        XNode _log = JsonConvert.DeserializeXNode(log, "Log");
                        XNode _requestParams = JsonConvert.DeserializeXNode(_parameters, "Parameters");

                        RequestLogging.InsertRequestResponseLog(_startDateTime, DateTime.Now, $"<Log>{_log.ToString().Replace("<Log>", "").Replace("</Log>", "")}{_requestParams.ToString()}</Log>");
                    }
                    _parameters = string.Empty;
                }
            }
        }

        private bool IsUserAllowedToSeeMiniProfilerUI(HttpRequest httpRequest)
        {
            var principal = httpRequest.RequestContext.HttpContext.User;
            return true;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }
    }
}
