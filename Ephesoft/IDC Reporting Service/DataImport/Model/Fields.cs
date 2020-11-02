using System.ComponentModel.DataAnnotations;

namespace DataImport
{
    public partial class FIELD
    {
        [Key]
        public int PK { get; set; }
        public string DOCID { get; set; }
        public string DOCTYPE { get; set; }
        public string FIELDNAME { get; set; }
        public string FIELDVALUE { get; set; }
        public string BATCH_INSTANCEID { get; set; }
        public string PATTERN { get; set; }
    }
}
