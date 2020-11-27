using System;
using System.Collections.Generic;

namespace MTS.Web.Helpers
{
    public class HttpRequestObject
    {
        public string URL { get; set; }
        public string REQUESTTYPE { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, object> URLParams { get; set; }
        public dynamic Content { get; set; }
        public byte[] FileStream { get; set; }
        public string RequestContentType { get; set; }
        public string ResponseType { get; set; } = typeof(String).ToString();

        public HttpRequestObject()
        {
            Headers = new Dictionary<string, string>();
            URLParams = new Dictionary<string, object>();
            RequestContentType = "application/json";
        }
    }

}
