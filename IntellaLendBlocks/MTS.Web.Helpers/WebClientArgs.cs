using System;
using System.Collections.Generic;

namespace MTS.Web.Helpers
{
    public class WebClientArgs : EventArgs
    {
        private KeyValuePair<string, string> _headers;
        public KeyValuePair<string, string> HeaderData { get { return _headers; } set { _headers = value; } }
    }
}
