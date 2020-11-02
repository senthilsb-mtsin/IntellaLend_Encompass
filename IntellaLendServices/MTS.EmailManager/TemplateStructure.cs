using System.IO;

namespace IL.EmailManager
{
    public struct Template
    {
        public string To;
        public string From;
        public string Cc;
        public string BCc;
        public string Subject;
        public string Body;
        public MemoryStream Attachment;
        public string AttachmnetName;
        public string ContentType;
    }  
}
