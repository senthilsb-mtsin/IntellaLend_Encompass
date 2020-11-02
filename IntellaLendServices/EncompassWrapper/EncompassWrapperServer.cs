using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Client;
using SimpleHttpServer;
using SimpleHttpServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EncompassWrapperServer
{

    public sealed class EncompassServer
    {
        private static EncompassServer instance = null;
        private static readonly object padlock = new object();

        private EncompassServer() {
            Start();
        }

        public static EncompassServer Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EncompassServer();
                    }
                    return instance;
                }
            }
        }

        private static string Message = "Encompass Connector is Up";
        private static Session Session = null;
        private static Thread thread = null;


        public void Start()
        {
            try
            {
                if (thread != null)
                    thread.Suspend();

                if (Session == null)
                {
                    Connect();

                    var route_config = new List<SimpleHttpServer.Models.Route>() {
                new Route {
                    Name = "Hello Handler",
                    UrlRegex = @"^/GetFieldValue/(.*)",
                    Method = "GET",
                    Callable = (HttpRequest request) => {
                        return GetFieldValueByLoanGUID(request.Path);
                     }
                },
                new Route {
                    Name = "Hello Handler",
                    UrlRegex = @"^/$",
                    Method = "GET",
                    Callable = (HttpRequest request) => {
                        return StartUp();
                     }
                }
            };

                    HttpServer httpServer = new HttpServer(8999, route_config);

                    thread = new Thread(new ThreadStart(httpServer.Listen));
                    thread.Start();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private HttpResponse StartUp()
        {
            return new HttpResponse()
            {
                ContentAsUTF8 = Message,
                ReasonPhrase = "OK",
                StatusCode = "200"
            };
        }

        private HttpResponse GetFieldValueByLoanGUID(string RequestPath)
        {
            string[] parameter = RequestPath.Split('/');

            if (parameter.Length > 1)
            {

                if (Session != null && !Session.IsConnected)
                {
                    Session = null;
                    Connect();
                }

                string loanGUID = parameter[0];
                string fieldID = parameter[1];

                Loan _loan = Session.Loans.Open("{" + loanGUID + "}");

                if (_loan != null)
                {
                    if (_loan.Fields[fieldID].Value != null)
                    {
                        Message = _loan.Fields[fieldID].Value.ToString();
                    }
                    else
                        Message = "3"; // 3
                }
                else
                    Message = "2";  // 2

            }
            else
                Message = "1";

            return new HttpResponse()
            {
                ContentAsUTF8 = Message,
                ReasonPhrase = "OK",
                StatusCode = "200"
            };
        }

        private void Connect()
        {
            string _server = ConfigurationManager.AppSettings["EncompassServer"];
            string _userName = ConfigurationManager.AppSettings["EncompassUserName"];
            string _password = ConfigurationManager.AppSettings["EncompassPassword"];
            try
            {
                if (Session == null)
                {
                    if (!string.IsNullOrEmpty(_server) && !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
                    {
                        Session s = new Session();
                        s.Start(_server, _userName, _password);
                        Session = s;
                    }
                    else
                    {
                        Message = "4";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    //public class EncompassServer
    //{

    //}
}
