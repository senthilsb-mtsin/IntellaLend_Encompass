using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IL.EncompassExport
{
    public class UpdateLoanField
    {
        public string loanGUID { get; set; }
        public Dictionary<string, object> fields { get; set; }
    }

    public class DownloadAttachmentRequest
    {
        public string loanGUID { get; set; }
        public Dictionary<string, string> parkingSpot { get; set; }
    }
}