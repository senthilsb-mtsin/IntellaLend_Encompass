using System.IO;

namespace IL.EmailTrackers
{
    public struct EmailTemplate
    {
        public string To;
        public string From;
        public string Subject;
        public string Body;
        public MemoryStream Attachment;
        public string AttachmnetName;
        public string ContentType;
    }
}
