using System.Collections.Generic;

namespace MTSRuleEngine
{
    public partial class DataTable
    {
        public string Name { get; set; }
        public HeaderRow HeaderRow { get; set; }
        public List<Row> Rows { get; set; }
    }

    public partial class HeaderRow
    {
        public List<HeaderColumns> HeaderColumns { get; set; }
    }

    public partial class HeaderColumns
    {
        public string Name { get; set; }
    }

    public partial class Row
    {
        public RowCoordinates RowCoordinates { get; set; }
        public List<RowColumn> RowColumns { get; set; }
    }

    public partial class RowColumn
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Confidence { get; set; }
        public List<CoordinatesList> CoordinatesList { get; set; }
        public string Page { get; set; }
    }

    public partial class RowCoordinates
    {
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
    }

    public partial class CoordinatesList
    {
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
    }
}
