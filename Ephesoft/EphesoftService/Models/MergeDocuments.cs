namespace EphesoftService.Models
{
    public class MergeDocuments
    {
        public string SourceDocType { get; set; }
        public string DestinationDocType { get; set; }
        public bool DestinationDocFieldToTable { get; set; }
    }
}