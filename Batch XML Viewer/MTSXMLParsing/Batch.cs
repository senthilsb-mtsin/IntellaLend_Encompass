using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTSXMLParsing
{
    public partial class Batch
    {
        public string BatchInstanceIdentifier { get; set; }
        public string BatchClassIdentifier { get; set; }
        public string BatchClassName { get; set; }
        public string BatchClassDescription { get; set; }
        public string BatchClassVersion { get; set; }
        public string BatchName { get; set; }
        public string BatchDescription { get; set; }
        public string BatchPriority { get; set; }
        public string BatchStatus { get; set; }
        public string BatchCreationDate { get; set; }
        public string BatchLocalPath { get; set; }
        public string UNCFolderPath { get; set; }
        public List<Documents> Documents { get; set; }
        public string LastPageNumber { get; set; }
        public Int64 LoanID { get; set; }
        public string Schema { get; set; }
        public decimal Confidence { get; set; }
        public decimal BatchExtractionAccuracy { get; set; }
    }

    public partial class Documents
    {
        public string Identifier { get; set; }
        public string Type { get; set; }
        public int VersionNumber { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public string Description { get; set; }
        public string Confidence { get; set; }
        public bool Reviewed { get; set; }
        public string MultiPagePdfFile { get; set; }
        public string MultiPageTiffFile { get; set; }
        public List<DocumentLevelFields> DocumentLevelFields { get; set; }
        public List<string> Pages { get; set; }
        public List<DataTable> DataTables { get; set; }
        public string DocumentExtractionAccuracy { get; set; }
    }

    public partial class DocumentLevelFields
    {
        public Int64 FieldID { get; set; }
        public string FieldDisplayName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string Confidence { get; set; }
        public int FieldOrderNumber { get; set; }
        public string FieldValueOptionList { get; set; }
        public CoordinatesList CoordinatesList { get; set; }
        public string PageNo { get; set; }
    }

    public partial class CoordinatesList
    {
        public int x0 { get; set; }
        public int y0 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
    }





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
}
