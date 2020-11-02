using System;
using System.Collections.Generic;

namespace IntellaLend.Model
{
    public class TOCDetails
    {
        public int StartingPage { get; set; }
        public string Type { get; set; }
    }

    public class DocumentDetails
    {
        public Int64 DocumentTypeID { get; set; }
        public Int32 VersionNumber { get; set; }
    }

    public class JobConfigDetails
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class ReArrangePDF
    {
        public List<TOCDetails> _tocDetails { get; set; }
        public byte[] TocPDF { get; set; }
    }
}
