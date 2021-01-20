using System;
using System.Collections.Generic;

namespace MTS.Web.Helpers
{
    public class WebClientArgs : EventArgs
    {
        private KeyValuePair<string, string> _headers;
        private Dictionary<string, string> _headerList;
        public KeyValuePair<string, string> HeaderData { get { return _headers; } set { _headers = value; } }
        public Dictionary<string, string> HeaderDataList { get { return _headerList; } set { _headerList = value; } }

        public WebClientArgs()
        {
            HeaderDataList = new Dictionary<string, string>();
        }
    }
}
