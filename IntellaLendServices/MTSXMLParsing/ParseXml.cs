using IntellaLend.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MTSXMLParsing
{
    public class ParseXml
    {
        static List<String> SimpleBatchProperties = new List<string> {
        "BatchName","BatchStatus","UNCFolderPath", "BatchInstanceIdentifier"
        };

        public static Batch GetParsedDataByFile(string xmlPath)
        {
            Batch BatchData = new Batch();
            List<Documents> lstBatchDoc = new List<Documents>();
            XmlDocument ParsedNodeXml;
            bool IsNodeIterated = false;
            
            using (XmlTextReader BatchXmlReader = new XmlTextReader(xmlPath))
            {
                BatchXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                while (IsNodeIterated || BatchXmlReader.Read())
                {
                    IsNodeIterated = false;
                    if (BatchXmlReader.NodeType == XmlNodeType.Element && BatchXmlReader.IsStartElement() == true)
                    {
                        if (SimpleBatchProperties.Contains(BatchXmlReader.LocalName))
                        {
                            String nodeName = BatchXmlReader.LocalName;
                            ParsedNodeXml = new XmlDocument();
                            ParsedNodeXml.LoadXml(BatchXmlReader.ReadOuterXml());
                            BatchData.GetType().GetProperty(nodeName).SetValue(BatchData, ParsedNodeXml.SelectSingleNode(nodeName).InnerText, null);
                            IsNodeIterated = true;
                        }
                        else if (BatchXmlReader.LocalName == "Document")
                        {
                            ParsedNodeXml = new XmlDocument();
                            ParsedNodeXml.LoadXml(BatchXmlReader.ReadOuterXml());//ReadOuterXml reads current node and move the reader to nex node automatically
                            lstBatchDoc.Add(ProcessDocumentNode(ParsedNodeXml));
                            IsNodeIterated = true;
                        }
                    }
                }
                BatchXmlReader.Close();

            }

            BatchData.Documents = lstBatchDoc;
            return BatchData;
        }

        public static Batch GetParsedDataByString(string batchXml)
        {
            Batch BatchData = new Batch();
            List<Documents> lstBatchDoc = new List<Documents>();
            XmlDocument ParsedNodeXml;
            bool IsNodeIterated = false;           

            byte[] byteArray = Encoding.ASCII.GetBytes(batchXml);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                using (XmlTextReader BatchXmlReader = new XmlTextReader(stream))
                {
                    BatchXmlReader.WhitespaceHandling = WhitespaceHandling.None;
                    while (IsNodeIterated || BatchXmlReader.Read())
                    {
                        IsNodeIterated = false;
                        if (BatchXmlReader.NodeType == XmlNodeType.Element && BatchXmlReader.IsStartElement() == true)
                        {
                            if (SimpleBatchProperties.Contains(BatchXmlReader.LocalName))
                            {
                                String nodeName = BatchXmlReader.LocalName;
                                ParsedNodeXml = new XmlDocument();
                                ParsedNodeXml.LoadXml(BatchXmlReader.ReadOuterXml());
                                BatchData.GetType().GetProperty(nodeName).SetValue(BatchData, ParsedNodeXml.SelectSingleNode(nodeName).InnerText, null);
                                IsNodeIterated = true;
                            }
                            else if (BatchXmlReader.LocalName == "Document")
                            {
                                ParsedNodeXml = new XmlDocument();
                                ParsedNodeXml.LoadXml(BatchXmlReader.ReadOuterXml());//ReadOuterXml reads current node and move the reader to nex node automatically
                                lstBatchDoc.Add(ProcessDocumentNode(ParsedNodeXml));
                                IsNodeIterated = true;
                            }
                        }
                    }
                    BatchXmlReader.Close();

                }
            }

            BatchData.Documents = lstBatchDoc;
            return BatchData;
        }

        private static Documents ProcessDocumentNode(XmlDocument docXmlNode)
        {
            XElement eldoc = XElement.Load(new XmlNodeReader(docXmlNode));
            Documents batchDoc = SetDocumentValues(eldoc);

            IEnumerable<XElement> doclevelfields = from doclevelflds in eldoc.Elements("DocumentLevelFields")
                                                   select doclevelflds;

            List<DocumentLevelFields> lstDocFlds = new List<DocumentLevelFields>();
            foreach (XElement eldoclvlflds in doclevelfields)
            {
                IEnumerable<XElement> doclevelfld = from doclvlfld in eldoclvlflds.Elements("DocumentLevelField")
                                                    select doclvlfld;

                foreach (XElement fld in doclevelfld)
                {
                    DocumentLevelFields docflds = SetDocumentLevelFldsValues(fld);
                    lstDocFlds.Add(docflds);
                }
            }

            IEnumerable<XElement> pglevelfields = from pglevelflds in eldoc.Elements("Pages")
                                                   select pglevelflds;

            List<PageLevelFields> lstPgFlds = new List<PageLevelFields>();
            foreach (XElement elpglvlflds in pglevelfields)
            {
                IEnumerable<XElement> pglevelfld = from pglvlfld in elpglvlflds.Elements("Page")
                                                    select pglvlfld;

                foreach (XElement fld in pglevelfld)
                {
                    PageLevelFields pgflds = SetPageLevelFldsValues(fld);
                    lstPgFlds.Add(pgflds);
                }
            }

            batchDoc.PageLevelFields = lstPgFlds;

            batchDoc.DocumentLevelFields = lstDocFlds;

            return batchDoc;
        }

        private static Documents SetDocumentValues(XElement eldoc)
        {
            Documents doc = new Documents();
            doc.Identifier = SetValue("Identifier", eldoc);
            doc.Type = SetValue("Type", eldoc);
            doc.Confidence = SetValue("Confidence", eldoc);
            doc.Reviewed = Convert.ToBoolean(SetValue("Reviewed", eldoc));
            doc.MultiPagePdfFile = SetValue("MultiPagePdfFile", eldoc);
            doc.MultiPageTiffFile = SetValue("MultiPageTiffFile", eldoc);
            doc.Description = SetValue("Description", eldoc);
            doc.Pages = GetPageList(eldoc);
            doc.DataTables = GetTableList(eldoc);
            return doc;
        }

        private static PageLevelFields SetPageLevelFldsValues(XElement eldoc)
        {
            PageLevelFields pg = new PageLevelFields();
            pg.PageNo = SetValue("Identifier", eldoc).Substring(2);
            pg.Direction = SetValue("Direction", eldoc);            
            pg.IsRotated = Convert.ToBoolean(SetValue("IsRotated", eldoc));
            return pg;
        }
        

        private static DocumentLevelFields SetDocumentLevelFldsValues(XElement elfld)
        {
            DocumentLevelFields docflds = new DocumentLevelFields();
            docflds.Confidence = SetValue("Confidence", elfld);
            docflds.Name = SetValue("Name", elfld);
            docflds.Type = SetValue("Type", elfld);
            docflds.FieldOrderNumber = Convert.ToInt32(SetValue("FieldOrderNumber", elfld));
            docflds.Value = SetValue("Value", elfld);
            docflds.FieldValueOptionList = SetValue("FieldValueOptionList", elfld);
            docflds.PageNo = SetValue("Page", elfld);
            docflds.CoordinatesList = GetCoordinates(elfld);
            return docflds;
        }

        private static string SetValue(string Name, XElement ele)
        {
            if (ele.Element(Name) != null)
                return ele.Element(Name).Value;
            return string.Empty;
        }

        private static List<string> GetPageList(XElement ele)
        {
            List<string> PageNos = new List<string>();
            IEnumerable<XElement> pgsEle = from pgEles in ele.Elements("Pages")
                                                select pgEles;
            foreach (XElement pages in pgsEle)
            {
                IEnumerable<XElement> pgLstEle = from pgsEles in pages.Elements("Page")
                                                 select pgsEles;
                foreach (var pg in pgLstEle)
                    PageNos.Add(SetValue("Identifier", pg));
            }
            return PageNos;
        }

        private static CoordinatesList GetCoordinates(XElement ele)
        {
            CoordinatesList Cords = new CoordinatesList();

            IEnumerable<XElement> coOrdLstEle = from cdEle in ele.Elements("CoordinatesList")
                                                select cdEle;
            foreach (XElement CoOrds in coOrdLstEle)
            {
                IEnumerable<XElement> CoOrdEles = from CoOrd in CoOrds.Elements("Coordinates")
                                                 select CoOrd;
                foreach (XElement CoOrd in CoOrdEles)
                {
                    Cords.x0 = Convert.ToInt32(SetValue("x0", CoOrd));
                    Cords.y0 = Convert.ToInt32(SetValue("y0", CoOrd));
                    Cords.x1 = Convert.ToInt32(SetValue("x1", CoOrd));
                    Cords.y1 = Convert.ToInt32(SetValue("y1", CoOrd));                    
                }                
            }
            return Cords;
        }




        private static List<DataTable> GetTableList(XElement ele)
        {
            List<DataTable> dataTable = new List<DataTable>();
            IEnumerable<XElement> tableEle = from tableEles in ele.Elements("DataTables")
                                           select tableEles;
            foreach (XElement table in tableEle)
            {
                IEnumerable<XElement> tableLstEle = from tableEles in table.Elements("DataTable")
                                                 select tableEles;
                foreach (var tb in tableLstEle)
                {
                    DataTable tab = new DataTable();
                    tab.Name = SetValue("Name", tb);
                    tab.HeaderRow = GetHeaderRowList(tb);
                    tab.Rows = GetTableRowList(tb);
                   dataTable.Add(tab);
                }
            }
            return dataTable;
        }


        private static HeaderRow GetHeaderRowList(XElement element)
        {
            HeaderRow HeaderRow = new HeaderRow();
            XElement hrows = element.Elements("HeaderRow").FirstOrDefault();
            if (hrows == null) return HeaderRow;
            HeaderRow.HeaderColumns = new List<HeaderColumns>();
            XElement columns = hrows.Elements("Columns").FirstOrDefault();
            if (columns == null) return HeaderRow;
            IEnumerable<XElement> column = from sele in columns.Elements("Column")
                                           select sele;
            foreach (var colele in column)
            {
                HeaderColumns hcol = new HeaderColumns();
                hcol.Name = SetValue("Name", colele);
                HeaderRow.HeaderColumns.Add(hcol);
            }

            return HeaderRow;
        }


        private static List<Row> GetTableRowList(XElement elemnt)
        {
            List<Row> Rows = new List<Row>();
            XElement rows = elemnt.Elements("Rows").FirstOrDefault();
            if (rows == null) return Rows;
            IEnumerable<XElement> rowList = from sele in rows.Elements("Row")
                                           select sele;
            foreach (var row in rowList)
            {
                Row objrow = new Row();
                objrow.RowColumns = new List<RowColumn>();

                XElement rowCords = elemnt.Elements("RowCoordinates").FirstOrDefault();
                objrow.RowCoordinates = new RowCoordinates();
                if (rowCords != null)
                {                 
                    objrow.RowCoordinates.x0 = Convert.ToInt32(SetValue("x0", row));
                    objrow.RowCoordinates.y0 = Convert.ToInt32(SetValue("y0", row));
                    objrow.RowCoordinates.x1 = Convert.ToInt32(SetValue("x1", row));
                    objrow.RowCoordinates.y1 = Convert.ToInt32(SetValue("y1", row));
                }
                XElement cols = row.Elements("Columns").FirstOrDefault();
                if (cols == null) return Rows;
                IEnumerable<XElement> column = from sele in cols.Elements("Column")
                                               select sele;
                foreach (var colele in column)
                {
                    RowColumn rcol = new RowColumn();
                    rcol.Name = SetValue("Name", colele);
                    rcol.Value = SetValue("Value", colele);
                    rcol.Page = SetValue("Page", colele);

                    IEnumerable<XElement> cordlist = from sele in colele.Elements("CoordinatesList")
                                                   select sele;
                    rcol.CoordinatesList = new List<CoordinatesList>();
                    foreach (var cord in cordlist)
                    {
                        rcol.CoordinatesList.Add(GetCoordinates(colele));
                    }
                    objrow.RowColumns.Add(rcol);
                }
                Rows.Add(objrow);
            }
            return Rows;
        }

    }
}