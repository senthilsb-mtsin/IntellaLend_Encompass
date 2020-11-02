using System.Collections.Generic;

namespace EphesoftService.Models
{
    public class DocumentField
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Confidence { get; set; }
        public List<Coordinates> CoordinatesList { get; set; }
        public string Page { get; set; }
    }
}