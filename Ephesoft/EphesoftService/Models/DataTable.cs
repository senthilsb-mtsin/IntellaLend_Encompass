using System.Collections.Generic;

namespace EphesoftService.Models
{
    public partial class DataTable
    {
        public string Name { get; set; }
        public HeaderRow HeaderRow { get; set; }
        public List<Row> Rows { get; set; }
    }

    [System.Xml.Serialization.XmlType(Namespace = "Header")]
    public partial class HeaderRow
    {
        public List<Column> Columns { get; set; }
    }

    public partial class Column
    {
        public string Name { get; set; }
    }

    [System.Xml.Serialization.XmlType(Namespace = "Row")]
    public partial class Row
    {
        public List<Ephesoft.Models.TableRow.Column> Columns { get; set; }
    }
    public partial class Coordinates
    {
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
    }

}

namespace Ephesoft.Models.TableRow
{
    public partial class Column
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Confidence { get; set; }
        public List<Coordinates> CoordinatesList { get; set; }
        public string Page { get; set; }
    }



    public partial class Coordinates
    {
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
    }

}