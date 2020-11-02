using EphesoftService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;


namespace EphesoftService
{
    public enum DocumentLocation { AFTER, BEFORE, UPWARD, DOWNWARD };
    public enum EphesoftPageType { FIRST, MIDDLE, LAST, None };

    /// <summary>
    /// This class provides the XPath utility methods to read and manipulate the Ephesoft Batch XML
    /// </summary>
    public static class UtilFunctions
    {

        private static readonly CustomLogger logger = new CustomLogger("UtilFunctions");

        #region Public Methods

        /// <summary>
        /// This method retrieves the BatchClassIdentifier element value from the input Ephesoft XML.
        /// This will be used to retrieve the rules from the DB for this batch class.
        /// </summary>
        /// <param name="xmlDoc">Input Ephesoft Batch XML</param>
        /// <returns>The value in the BatchClassIdentifier element</returns>
        public static string GetBatchClassIdForEphesoftXML(XPathNavigator inputXmlNavigator)
        {
            string batchClassId = String.Empty;
            logger.Debug("Get the Batch class Id from the input XML.");

            if (inputXmlNavigator != null)
            {
                string xpathExpr = "/Batch/BatchClassIdentifier";
                batchClassId = inputXmlNavigator.SelectSingleNode(xpathExpr).Value;
            }

            logger.Debug("Retrieved the batch class Id from the input XML.");
            return batchClassId;
        }


        /// <summary>
        /// This method retrieves the BatchInstanceIdentifier element value from the input Ephesoft XML.
        /// </summary>
        /// <param name="xmlDoc">Input Ephesoft Batch XML</param>
        /// <returns>The value in the BatchInstanceIdentifier element</returns>
        public static string GetBatchIdForEphesoftXML(XPathNavigator inputXmlNavigator)
        {
            string batchId = String.Empty;
            logger.Debug("Get the Batch Id from the input XML.");

            if (inputXmlNavigator != null)
            {
                string xpathExpr = "/Batch/BatchInstanceIdentifier";
                batchId = inputXmlNavigator.SelectSingleNode(xpathExpr).Value;
                if (String.IsNullOrEmpty(batchId))
                {
                    logger.Error("BatchInstanceIdentifier is NULL in the input XML.");
                }
            }

            logger.Debug("Batch Id:" + batchId + " - BatchId retrieved for the input XML.");
            return batchId;
        }

        public static string GetBatchNameForEphesoftXML(XPathNavigator inputXmlNavigator)
        {
            string batchName = String.Empty;

            if (inputXmlNavigator != null)
            {
                string xpathExpr = "/Batch/BatchName";
                batchName = inputXmlNavigator.SelectSingleNode(xpathExpr).Value;
                if (String.IsNullOrEmpty(batchName))
                {
                    logger.Error("BatchInstanceName is NULL in the input XML.");
                }
            }
            return batchName;
        }

        public static void AddDataTableToDocument(XPathNavigator document, DataTable fieldToTable)
        {
            string expression = "./DataTables";
            XPathExpression expr = document.Compile(expression);
            XPathNavigator tables = document.SelectSingleNode(expr);

            tables.AppendChild(CreateXML(fieldToTable));
        }

        private static string CreateXML(Object YourClassObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(YourClassObject.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, YourClassObject);
                xmlStream.Position = 0;

                xmlDoc.Load(xmlStream);
                string outXMl = RemoveAllNamespaces(xmlDoc.InnerXml);
                return outXMl;
            }
        }

        private static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        public static DataTable FieldsToDataTable(List<DocumentField> docFields, string docName, string xmlDocID)
        {
            DataTable fieldTable = new DataTable()
            {
                Name = $"{docName}_{xmlDocID}",
                HeaderRow = new HeaderRow()
                {
                    Columns = new List<Column>() {
                        new Column() { Name = "Field Name" },
                        new Column() { Name = "Field Value" }
                    }
                }
            };

            fieldTable.Rows = new List<Row>();

            foreach (DocumentField field in docFields)
            {
                List<Ephesoft.Models.TableRow.Coordinates> rowCords = new List<Ephesoft.Models.TableRow.Coordinates>();
                foreach (var item in field.CoordinatesList)
                {
                    rowCords.Add(new Ephesoft.Models.TableRow.Coordinates() { x0 = item.x0, x1 = item.x1, y0 = item.y0, y1 = item.y1 });
                }

                List<Ephesoft.Models.TableRow.Column> rowData = new List<Ephesoft.Models.TableRow.Column>();

                rowData.Add(new Ephesoft.Models.TableRow.Column()
                {
                    Name = "Field Name",
                    Value = field.FieldName,
                    Page = string.Empty,
                    Confidence = string.Empty,
                    CoordinatesList = new List<Ephesoft.Models.TableRow.Coordinates>()
                });

                rowData.Add(new Ephesoft.Models.TableRow.Column()
                {
                    Name = "Field Value",
                    Value = field.Value,
                    Page = field.Page,
                    Confidence = field.Confidence,
                    CoordinatesList = rowCords
                });

                fieldTable.Rows.Add(new Row()
                {
                    Columns = rowData
                });
            }

            return fieldTable;
        }

        public static void AppendSourceDocTableToDestination(XPathNavigator destinationDoc, XPathNavigator sourceDoc)
        {
            string expression = "./DataTables/DataTable";
            XPathExpression expr = sourceDoc.Compile(expression);
            XPathNodeIterator DataTables = sourceDoc.Select(expr);

            foreach (XPathNavigator table in DataTables.OfType<XPathNavigator>().ToList())
            {
                destinationDoc.AppendChild(table);
            }
        }

        public static List<DocumentField> GetDocumentFields(XPathNavigator document)
        {
            string expression = "./DocumentLevelFields/DocumentLevelField";
            XPathExpression expr = document.Compile(expression);
            XPathNodeIterator fields = document.Select(expr);

            List<DocumentField> fieldDic = new List<DocumentField>();
            foreach (XPathNavigator field in fields.OfType<XPathNavigator>().ToList())
            {

                List<Coordinates> _cords = new List<Coordinates>();

                XPathNodeIterator xmlCords = field.Select(field.Compile("./CoordinatesList/Coordinates"));
                foreach (XPathNavigator xmlCord in xmlCords.OfType<XPathNavigator>().ToList())
                {
                    _cords.Add(new Coordinates()
                    {
                        x0 = xmlCord.SelectSingleNode(xmlCord.Compile("./x0")).ValueAsInt,
                        x1 = xmlCord.SelectSingleNode(xmlCord.Compile("./x1")).ValueAsInt,
                        y0 = xmlCord.SelectSingleNode(xmlCord.Compile("./y0")).ValueAsInt,
                        y1 = xmlCord.SelectSingleNode(xmlCord.Compile("./y1")).ValueAsInt,
                    });
                }

                string _page = string.Empty;
                if (field.SelectSingleNode(field.Compile("./Page")) != null)
                    _page = field.SelectSingleNode(field.Compile("./Page")).Value;

                fieldDic.Add(new DocumentField()
                {
                    FieldName = field.SelectSingleNode(field.Compile("./Name")).Value,
                    Value = field.SelectSingleNode(field.Compile("./Value")).Value,
                    Type = field.SelectSingleNode(field.Compile("./Type")).Value,
                    Page = _page,
                    CoordinatesList = _cords,
                    Confidence = field.SelectSingleNode(field.Compile("./Confidence")).Value
                });
            }
            return fieldDic;
        }

        public static void DeleteAllDocuments(XPathNavigator inputXmlNavigator)
        {
            string xpathExprPagesNode = "//Document";
            XPathNodeIterator docNodes = inputXmlNavigator.Select(xpathExprPagesNode);

            foreach (XPathNavigator doc in docNodes.OfType<XPathNavigator>().ToList())
            {
                doc.DeleteSelf();
            }
        }

        public static void DeleteAndInsertPages(XPathNavigator tempXmlNavigator, List<XPathNavigator> newPages)
        {
            string expression = "./Pages/Page";
            XPathExpression expr = tempXmlNavigator.Compile(expression);
            XPathNodeIterator pages = tempXmlNavigator.Select(expr);

            foreach (XPathNavigator page in pages.OfType<XPathNavigator>().ToList())
            {
                page.DeleteSelf();
            }

            string xpathExprPagesNode = "./Pages";
            XPathNavigator pageNode = tempXmlNavigator.SelectSingleNode(xpathExprPagesNode);

            foreach (XPathNavigator page in newPages)
            {
                pageNode.AppendChild(page);
            }
        }

        public static void DeleteDocumentFields(XPathNavigator tempXmlNavigator)
        {
            string expression = "./DocumentLevelFields/DocumentLevelField";
            XPathExpression expr = tempXmlNavigator.Compile(expression);
            XPathNodeIterator fields = tempXmlNavigator.Select(expr);

            foreach (XPathNavigator field in fields.OfType<XPathNavigator>().ToList())
            {
                field.DeleteSelf();
            }
        }

        public static void DeleteDocumentTables(XPathNavigator tempXmlNavigator)
        {
            string expression = "./DataTables/DataTable";
            XPathExpression expr = tempXmlNavigator.Compile(expression);
            XPathNodeIterator DataTables = tempXmlNavigator.Select(expr);

            foreach (XPathNavigator table in DataTables.OfType<XPathNavigator>().ToList())
            {
                table.DeleteSelf();
            }
        }

        public static void DeleteDocumentPages(XPathNavigator tempXmlNavigator)
        {
            string expression = "./Pages/Page";
            XPathExpression expr = tempXmlNavigator.Compile(expression);
            XPathNodeIterator pages = tempXmlNavigator.Select(expr);

            foreach (XPathNavigator page in pages.OfType<XPathNavigator>().ToList())
            {
                page.DeleteSelf();
            }
        }

        /// <summary>
        /// This method sets the Reviewed element value for the document from the input Ephesoft XML.
        /// </summary>
        /// <param name="xmlDoc">XPathNavigator for the Document element</param>
        /// <returns>The value in the Reviewed element</returns>
        public static void SetDocumentNodeValue(XPathNavigator docNavigator, string node, string nodeValue)
        {
            logger.Debug($"Set the {node} value of the input document node.");

            if (docNavigator != null)
            {
                string xpathExpr = $"./{node}";
                XPathNavigator reviewedNode = docNavigator.SelectSingleNode(xpathExpr);
                reviewedNode.SetValue(nodeValue);
            }

            logger.Debug($"{node} value updated for the input document. {node} : {nodeValue}");
        }


        public static void SetClassificationType(XPathNavigator inputXmlNavigator)
        {
            if (inputXmlNavigator != null)
            {
                string newNode = $"<DocumentClassificationTypes><DocumentClassificationType>SearchClassification</DocumentClassificationType></DocumentClassificationTypes>";
                string xpathExprPagesNode = "//Batch";
                XPathNavigator docNodes = inputXmlNavigator.SelectSingleNode(xpathExprPagesNode);
                // docNodes.AppendChild(newDocument);
                docNodes.AppendChild(newNode);
            }
        }


        public static void CreateDocumentNode(XPathNavigator docNavigator, string node, string nodeValue)
        {
            logger.Debug($"Set the {node} value of the input document node.");

            if (docNavigator != null)
            {
                string newNode = $"<{node}>{nodeValue}</{node}>";
                docNavigator.AppendChild(newNode);
                //string xpathExpr = $"./{node}";
                //XPathNavigator reviewedNode = docNavigator.SelectSingleNode(xpathExpr);
                //reviewedNode.SetValue(nodeValue);
            }

            logger.Debug($"New node {node} created on the input document. {node} : {nodeValue}");
        }

        public static void InsertDocument(XPathNavigator inputXmlNavigator, XPathNavigator newDocument)
        {
            string xpathExprPagesNode = "//Documents";
            XPathNavigator docNodes = inputXmlNavigator.SelectSingleNode(xpathExprPagesNode);
            docNodes.AppendChild(newDocument);
        }

        public static XPathNavigator GetPageWithNumber(List<XPathNavigator> allPageXmlNavigator, Int32 PageNo)
        {
            foreach (XPathNavigator nodeNavigator in allPageXmlNavigator)
            {
                XPathNavigator pageIdCurrentNode = nodeNavigator.SelectSingleNode("./Identifier");
                int currentPageId;
                Int32.TryParse(pageIdCurrentNode.Value.Substring(2), out currentPageId);
                if (currentPageId == PageNo)
                    return nodeNavigator;
            }

            return null;
        }

        /// <summary>
        /// This methods gets a list of all the Document node in the Ephesoft XML
        /// </summary>
        /// <param name="inputXmlDocument">The input Ephesoft XML</param>
        /// <returns>List containing the XPathNavigator references to all the DOcument nodes in the XML</returns>
        public static XPathNavigator GetFirstDocument(XPathNavigator inputXmlNavigator)
        {
            logger.Debug("Get First the document node in the input XML");
            XPathNavigator firstDocument = null;
            if (inputXmlNavigator != null)
            {
                string expression = "//Document";
                XPathExpression expr = inputXmlNavigator.Compile(expression);
                XPathNodeIterator iterator = inputXmlNavigator.Select(expr);

                // Iterate through the Nodes extracted and add to list
                while (iterator.MoveNext())
                {
                    firstDocument = iterator.Current.Clone();
                    break;
                }
            }

            logger.Debug("Retrieved  the document node in the input XML.");

            return firstDocument;
        }

        public static XPathNavigator GetDuplicateFirstDocument(XPathNavigator inputXmlNavigator)
        {
            logger.Debug("Get First the document node in the input XML");
            XPathNavigator firstDocument = null;
            if (inputXmlNavigator != null)
            {
                string expression = "//Document";
                XPathExpression expr = inputXmlNavigator.Compile(expression);
                XPathNodeIterator iterator = inputXmlNavigator.Select(expr);

                // Iterate through the Nodes extracted and add to list
                while (iterator.MoveNext())
                {
                    firstDocument = iterator.Current.Clone();
                    break;
                }

                inputXmlNavigator.SelectSingleNode(inputXmlNavigator.Compile("//Documents")).AppendChild(firstDocument);
            }

            logger.Debug("Retrieved  the document node in the input XML.");

            return firstDocument;
        }

        /// <summary>
        /// This methods gets a list of all the Document node in the Ephesoft XML
        /// </summary>
        /// <param name="inputXmlDocument">The input Ephesoft XML</param>
        /// <returns>List containing the XPathNavigator references to all the DOcument nodes in the XML</returns>
        public static List<XPathNavigator> GetAllPages(XPathNavigator inputXmlNavigator)
        {
            logger.Debug("Get all page nodes in the input XML");

            List<XPathNavigator> _allPages = new List<XPathNavigator>();

            if (inputXmlNavigator != null)
            {
                string expression = "//Document/Pages/Page";
                XPathNodeIterator iterator = inputXmlNavigator.Select(expression);

                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();
                    _allPages.Add(nodeNavigator);
                }
            }

            logger.Debug("Retrieved all page nodes in the input XML.");

            return _allPages;
        }

        public static bool CheckSourceDocExist(XPathNavigator inputXmlNavigator, List<MergeDocuments> sourceDocs)
        {
            bool result = false;
            foreach (MergeDocuments item in sourceDocs)
            {
                List<XPathNavigator> xmlSourceDocs = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, item.SourceDocType);

                if (xmlSourceDocs.Count > 0)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        /// <summary>
        /// This method retrieves all the Document elements from the XML with the given document name. 
        /// </summary>
        /// <param name="xmlDocument">The document representing the Ephesoft XML</param>
        /// <param name="documentName">The document name to be retrieved</param>documents
        /// <returns>An ArrayList of the XPathNavigator for each Document node</returns>
        public static List<XPathNavigator> GetDocumentOfDocType(XPathNavigator inputXmlNavigator, string documentName)
        {
            logger.Debug("Get the list of Document node with the input name in the XML. Document name - " + documentName);

            List<XPathNavigator> documentList = new List<XPathNavigator>();

            if (inputXmlNavigator != null && !string.IsNullOrEmpty(documentName))
            {
                string expression = "//Document[Type=\"" + documentName + "\"]";
                XPathExpression expr = inputXmlNavigator.Compile(expression);
                XPathNodeIterator iterator = inputXmlNavigator.Select(expr);

                // Iterate through the Nodes extracted and add to list
                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();
                    documentList.Add(nodeNavigator);
                }
            }

            logger.Debug("Retrieved the list of document node with the input name. No of document nodes: " + documentList.Count);
            return documentList;
        }


        /// <summary>
        /// This method updates element in the input document node from the Ephesoft XML with the value provided.
        /// </summary>
        /// <param name="documentNode">The XPathNavigator represents the element in which value will be updated.</param>
        /// <param name="elementName">The element name whose value needs to be updated.</param>
        /// <param name="value">The value to be updated. </param>
        public static void UpdateSingleElementByElementName(XPathNavigator documentNode, string elementName, string value)
        {
            logger.Debug("Update the element value for the input element name. Element Name - " + elementName);

            if (documentNode != null && !string.IsNullOrEmpty(elementName) && !string.IsNullOrEmpty(value))
            {
                XPathNodeIterator it = documentNode.Select("./" + elementName);
                // If an element is selected from the expression, update the given value.
                if (it.MoveNext())
                {
                    it.Current.SetValue(value);
                }
                if (it.Count > 1)
                {
                    logger.Info("More than one element with the name {0} found. Updated value of first element. " + elementName);
                }
            }

            logger.Debug(String.Format("Updated element name - {0}  with input value - {1}", elementName, value));
        }


        /// <summary>
        /// This method updates element in the input document node from the Ephesoft XML with the value provided.
        /// </summary>
        /// <param name="documentNode">The XPathNavigator represents the element in which value will be updated.</param>
        /// <param name="elementName">The element name whose value needs to be updated.</param>
        /// <param name="value">The value to be updated. </param>
        public static string GetSingleElementByElementName(XPathNavigator documentNode, string elementName)
        {
            logger.Debug("Update the element value for the input element name. Element Name - " + elementName);

            if (documentNode != null && !string.IsNullOrEmpty(elementName))
            {
                XPathNodeIterator it = documentNode.Select("./" + elementName);
                // If an element is selected from the expression, update the given value.
                if (it.MoveNext())
                {
                    return it.Current.Value;
                }
                if (it.Count > 1)
                {
                    logger.Info("More than one element with the name {0} found. Updated value of first element. " + elementName);
                }
            }

            return string.Empty;
        }


        public static XPathNavigator GetDocumentViaFirstPage(XPathNavigator inputXML, string documentName)
        {
            List<XPathNavigator> documents = GetAllDocuments(inputXML);

            XPathNavigator returnDoc = null;

            foreach (XPathNavigator doc in documents)
            {
                string FirstPageDocumentName = GetFirstPageDocType(doc);
                if (!String.IsNullOrEmpty(FirstPageDocumentName) && FirstPageDocumentName.Contains(documentName))
                {
                    returnDoc = doc;
                    break;
                }
            }

            return returnDoc;
        }

        //this method retrieves the previous document of the input child document 
        public static XPathNavigator GetDocumentViaChildDocument(XPathNavigator inputXML, XPathNavigator childDocument)
        {
            List<XPathNavigator> documents = GetAllDocuments(inputXML);
            XPathNavigator previousDoc = null;
            XPathNavigator returnDoc = null;
            string childDocIdentifier = string.Empty;

            if (inputXML != null && childDocument != null)
            {
                childDocIdentifier = GetDocumentIdenifier(childDocument);

                string expression = "//Document";
                XPathExpression expr = inputXML.Compile(expression);
                XPathNodeIterator iterator = inputXML.Select(expr);

                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();

                    string currentDocIdentifier = GetDocumentIdenifier(nodeNavigator);
                    if (currentDocIdentifier == childDocIdentifier)
                    {
                        return previousDoc;
                    }
                    else
                    {
                        previousDoc = nodeNavigator;
                    }
                }
            }

            return returnDoc;
        }

        //this method retrieves the child document by checking the child's first and second page 
        public static XPathNavigator GetDocumentViaFirstAndSecondPage(XPathNavigator inputXML, string parentDocName, string childDocName)
        {
            List<XPathNavigator> documents = GetAllDocuments(inputXML);
            XPathNavigator returnDoc = null;

            bool isParent = false;
            string childFirstPageDocType = string.Empty;
            string currentDocName = string.Empty;
            string childSecondPageDocType = string.Empty;

            if (inputXML != null)
            {
                string expression = "//Document";
                XPathExpression expr = inputXML.Compile(expression);
                XPathNodeIterator iterator = inputXML.Select(expr);

                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();
                    if (isParent)
                    {
                        currentDocName = GetDocumentTypeName(nodeNavigator);
                        if (currentDocName == parentDocName) //checking if current doctype is equal to parent doctype
                        {
                            childFirstPageDocType = GetFirstPageDocType(nodeNavigator);
                            if (!String.IsNullOrEmpty(childFirstPageDocType) && childFirstPageDocType.Contains(childDocName)) //checking if current doc's firstpageDoctype is equal to child's doctype
                            {
                                childSecondPageDocType = GetSecondPageDocType(nodeNavigator);
                                if (!String.IsNullOrEmpty(childSecondPageDocType) && childSecondPageDocType.Contains(parentDocName)) //checking if current doc's secondpageDoctype is equal to parent's doctype
                                {
                                    returnDoc = nodeNavigator;
                                    return returnDoc;
                                }
                                else
                                {
                                    isParent = false;
                                }
                            }
                            else
                            {
                                isParent = false;
                            }
                        }
                        else
                        {
                            isParent = false;
                        }
                    }
                    currentDocName = GetDocumentTypeName(nodeNavigator);
                    if (currentDocName == parentDocName)
                    {
                        isParent = true;
                    }
                }
            }
            return returnDoc;
        }


        /// <summary>
        /// This method retrieves the document identifier for the given Document node of Ephesoft XML
        /// </summary>
        /// <param name="documentNode">A reference of XPathNavigator that represents a document node </param>
        /// <returns>The document identifier name </returns>
        public static string GetDocumentIdenifier(XPathNavigator documentNode)
        {
            logger.Debug("Get the document Identifier for the input document node");

            string docIdentifier = String.Empty;
            if (documentNode != null)
            {
                string xpathExpr = "./Identifier";
                docIdentifier = documentNode.SelectSingleNode(xpathExpr).Value;
                if (String.IsNullOrEmpty(docIdentifier))
                {
                    logger.Error("Document identifier not retrieved from input Document node.");
                }
            }

            logger.Debug("Retrieved the document identifier for the input document node. Document Id - " + docIdentifier);
            return docIdentifier;
        }

        public static int GetLastDocumentIdenifier(XPathNavigator documentNode)
        {
            logger.Debug("Get the document Identifier for the input document node");

            if (documentNode != null)
            {
                string xpathExpr = "//Document/Identifier";
                XPathNodeIterator docIdentifiers = documentNode.Select(xpathExpr);
                List<int> docIds = new List<int>();
                while (docIdentifiers.MoveNext())
                {
                    docIds.Add(Convert.ToInt32(docIdentifiers.Current.Value.Replace("DOC", "")));
                }

                return docIds.OrderByDescending(a => a).FirstOrDefault();
            }

            return 0;
        }



        /// <summary>
        /// This method retrieves the document name for the given Document node of Ephesoft XML
        /// </summary>
        /// <param name="documentNode">A reference of XPathNavigator that represents a document node</param>
        /// <returns>The document name</returns>
        public static string GetDocumentTypeName(XPathNavigator documentNode)
        {
            logger.Debug("Get the document type name for the input document node.");
            string docTypeName = String.Empty;

            if (documentNode != null)
            {
                string xpathExpr = "./Type";
                docTypeName = documentNode.SelectSingleNode(xpathExpr).Value;
                if (String.IsNullOrEmpty(docTypeName))
                {
                    logger.Info("Document type name not retrieved from input document node.");
                }
            }

            logger.Debug("Retrieved the document type name of the input document node. DocumentName - " + docTypeName);
            return docTypeName;
        }


        /// <summary>
        /// This method merged the PAGE node of the 2 document based on the input POSITION. 
        /// </summary>
        /// <param name="documentToMerge">The PAGE node from this document will be merged to the document</param>
        /// <param name="document">The document to which the PAGE node will be added in the input position</param>
        /// <param name="position">AFTER or BEFORE. The PAGE nodes will be appended or prepended based on this input</param>
        public static void MergeDocument(XPathNavigator documentToMerge, XPathNavigator document, DocumentLocation position)
        {
            logger.Debug("Merge the two document nodes based on the input position.");

            if (documentToMerge != null && document != null)
            {
                string xpathExprPagesNode = "./Pages";
                XPathNavigator pageNode = document.SelectSingleNode(xpathExprPagesNode);

                string xpathExpr = "./Pages/Page";
                // Get the list of PAGE nodes to be merged
                XPathNodeIterator pageNodesToMerge = documentToMerge.Select(xpathExpr);

                // If the position is AFTER, merge the pages to the end of the PAGE nodes
                if (position.Equals(DocumentLocation.AFTER))
                {
                    while (pageNodesToMerge.MoveNext())
                    {
                        pageNode.AppendChild(pageNodesToMerge.Current.Clone());
                    }
                    logger.Debug("Appended all pages to the end of the document.");
                }
                else if (position.Equals(DocumentLocation.BEFORE))
                {
                    // If the position is BEFORE, merge the pages to the beginning of the PAGE nodes
                    List<XPathNavigator> docToPrepend = new List<XPathNavigator>();
                    while (pageNodesToMerge.MoveNext())
                    {
                        docToPrepend.Add(pageNodesToMerge.Current.Clone());
                    }
                    for (int _x = docToPrepend.Count - 1; _x >= 0; _x--)
                    {
                        pageNode.PrependChild(docToPrepend[_x]);
                    }
                    logger.Debug("Prepend all pages to the beginning of the document.");
                }
                // Delete the merged node               
                documentToMerge.DeleteSelf();
            }
            logger.Debug("Input documents merged");
        }

        public static void MergeDocument(XPathNavigator destinationDoc, XPathNavigator pageToAppend)
        {
            logger.Debug("Merge page to the destination document");

            if (destinationDoc != null && pageToAppend != null)
            {
                string xpathExprPagesNode = "./Pages";
                XPathNavigator pageNode = destinationDoc.SelectSingleNode(xpathExprPagesNode);

                pageNode.AppendChild(pageToAppend);

                logger.Debug("Appended page to the end of the document.");
            }

        }


        /// <summary>
        /// This methods gets a list of all the Document node in the Ephesoft XML
        /// </summary>
        /// <param name="inputXmlDocument">The input Ephesoft XML</param>
        /// <returns>List containing the XPathNavigator references to all the DOcument nodes in the XML</returns>
        public static List<XPathNavigator> GetAllDocuments(XPathNavigator inputXmlNavigator)
        {
            logger.Debug("Get all the document nodes in the input XML");

            List<XPathNavigator> documentList = new List<XPathNavigator>();
            if (inputXmlNavigator != null)
            {
                string expression = "//Document";
                XPathExpression expr = inputXmlNavigator.Compile(expression);
                XPathNodeIterator iterator = inputXmlNavigator.Select(expr);

                // Iterate through the Nodes extracted and add to list
                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();
                    documentList.Add(nodeNavigator);
                }
            }

            logger.Debug("Retrieved the list of document nodes in the input XML.");
            return documentList;
        }


        /// <summary>
        /// This method retrieves the Confidence element value for the document from the input Ephesoft XML.
        /// </summary>
        /// <param name="xmlDoc">XPathNavigator for the Document element</param>
        /// <returns>The value in the Confidence element</returns>
        public static double GetConfidenceForDocument(XPathNavigator docNavigator)
        {
            logger.Debug("Get the confidence of the input document node.");
            double confidence = 0;

            if (docNavigator != null)
            {
                string xpathExpr = "./Confidence";
                confidence = Double.Parse(docNavigator.SelectSingleNode(xpathExpr).Value);
            }

            logger.Debug("Retrieved the confidence of the input document. Confidence - " + confidence);
            return confidence;
        }


        /// <summary>
        /// This method retrieves the ConfidenceThreshold element value for the document from the input Ephesoft XML.
        /// </summary>
        /// <param name="xmlDoc">XPathNavigator for the Document element</param>
        /// <returns>The value in the ConfidenceThreshold element</returns>
        public static double GetConfidenceThresholdForDocument(XPathNavigator docNavigator)
        {
            logger.Debug("Get the confidence threshold for the input document node");

            double confidenceThreshold = 0;
            if (docNavigator != null)
            {
                string xpathExpr = "./ConfidenceThreshold";
                confidenceThreshold = Double.Parse(docNavigator.SelectSingleNode(xpathExpr).Value);
            }

            logger.Debug("Retrieved the confidence threshold for the input document node. Confidence threshold - " + confidenceThreshold);
            return confidenceThreshold;
        }


        /// <summary>
        /// Checks if the Confidence of the document is more than the confidence threshold.
        /// </summary>
        /// <param name="docNavigator">XPathNavigator for the Document element</param>
        /// <returns>Boolean indicating is the document is confident or not</returns>
        public static Boolean IsDocConfident(XPathNavigator docNavigator)
        {
            logger.Debug("Check if the input document node is confident");

            double docConfidence = GetConfidenceForDocument(docNavigator);
            double docConfidenceThreshold = GetConfidenceThresholdForDocument(docNavigator);

            if (docConfidence > docConfidenceThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// This method gets the Page element in the input position
        /// </summary>
        /// <param name="xmlDoc">XPathNavigator for the Document element</param>
        /// <returns>The XPathNavigator reference for the page element</returns>
        public static XPathNavigator GetPageFromDocument(XPathNavigator docNavigator, int pagePosition)
        {
            logger.Debug("Get the Page node in the input position from the document.");

            XPathNavigator pageInInputPosition = null;
            if (docNavigator != null)
            {
                string xpathExpr = "./Pages/Page[" + pagePosition + "]";
                pageInInputPosition = docNavigator.SelectSingleNode(xpathExpr);
            }

            logger.Debug("Retrieved the page node in the input position - " + pagePosition);
            return pageInInputPosition;
        }


        /// <summary>
        /// This method gets all Page element from the Document
        /// </summary>
        /// <param name="docNavigator"></param>
        /// <returns>The XPathNavigator reference for the page element</returns>
        public static List<int> GetAllPagesFromDocument(XPathNavigator docNavigator)
        {

            List<int> PageNos = new List<int>();
            if (docNavigator != null)
            {
                string expression = "./Pages/Page";
                XPathExpression expr = docNavigator.Compile(expression);
                XPathNodeIterator iterator = docNavigator.Select(expr);

                while (iterator.MoveNext())
                {
                    string PageNo = iterator.Current.SelectSingleNode("./Identifier").ToString();
                    if (!String.IsNullOrEmpty(PageNo))
                        PageNos.Add(Convert.ToInt32(PageNo.Replace("PG", string.Empty)));
                }
            }

            return PageNos;
        }

        /// <summary>
        /// This method Gets the particular pages from the Batch and Merge them to the destination document
        /// </summary>
        /// <param name="inputXmlNavigator"></param>
        /// <param name="PageNos"></param>
        /// <param name="destinationDoc"></param>
        /// <returns></returns>
        public static void GetPageAndMerge(XPathNavigator inputXmlNavigator, List<int> PageNos, XPathNavigator destinationDoc)
        {
            if (PageNos != null)
            {
                foreach (int PageNo in PageNos)
                {
                    string expression = "//Document/Pages/Page[Identifier=\"PG" + PageNo.ToString() + "\"]";
                    XPathExpression expr = inputXmlNavigator.Compile(expression);
                    XPathNavigator page = inputXmlNavigator.SelectSingleNode(expr);

                    if (page != null)
                    {
                        MergeDocument(destinationDoc, page);
                        page.DeleteSelf();
                    }
                }
            }
        }

        /// <summary>
        /// This method returns the page type of the first page in the input document
        /// </summary>
        /// <param name="docNavigator"></param>
        /// <returns></returns>
        public static EphesoftPageType GetFirstPageType(XPathNavigator docNavigator)
        {
            logger.Debug("Get the page type of the first page of the document.");

            EphesoftPageType pageType = EphesoftPageType.None;
            if (docNavigator != null)
            {
                // Get the first page of the document 
                XPathNavigator pageNav = GetPageFromDocument(docNavigator, 1);
                if (pageNav != null)
                {
                    // Retrieve the Value of the PageLevelField which has the prefix like First_Page, Middle_Page or Last_Page.
                    string xpathExpr = "./PageLevelFields/PageLevelField[1]/Value ";
                    string pageTypeValue = pageNav.SelectSingleNode(xpathExpr).Value;
                    if (pageTypeValue.Contains("First_Page"))
                    {
                        pageType = EphesoftPageType.FIRST;
                    }
                    else
                        if (pageTypeValue.Contains("Middle_Page"))
                    {
                        pageType = EphesoftPageType.MIDDLE;
                    }
                    else
                            if (pageTypeValue.Contains("Last_Page"))
                    {
                        pageType = EphesoftPageType.LAST;
                    }
                }
            }

            logger.Debug("Retrieved the document type of the first page of the document.");
            return pageType;
        }


        public static string GetFirstPageDocType(XPathNavigator docNavigator)
        {
            logger.Debug("Fetch the document type of the first page of the document.");
            string pageTypeValue = string.Empty;
            if (docNavigator != null)
            {
                // Get the first page of the document 
                XPathNavigator pageNav = GetPageFromDocument(docNavigator, 1);
                if (pageNav != null)
                {
                    string xpathExpr = "./PageLevelFields/PageLevelField[1]/Value ";
                    pageTypeValue = pageNav.SelectSingleNode(xpathExpr).Value;
                }
            }

            logger.Debug("Retrieved the document type of the first page of the document.");
            return pageTypeValue;
        }

        public static string GetSecondPageDocType(XPathNavigator docNavigator)
        {
            logger.Debug("Fetch the document type of the first page of the document.");
            string pageTypeValue = string.Empty;
            if (docNavigator != null)
            {
                // Get the first page of the document 
                XPathNavigator pageNav = GetPageFromDocument(docNavigator, 2);
                if (pageNav != null)
                {
                    string xpathExpr = "./PageLevelFields/PageLevelField[1]/Value ";
                    pageTypeValue = pageNav.SelectSingleNode(xpathExpr).Value;
                }
            }

            logger.Debug("Retrieved the document type of the first page of the document.");
            return pageTypeValue;
        }

        /// <summary>
        /// This method sets the Reviewed element value for the document from the input Ephesoft XML.
        /// </summary>
        /// <param name="xmlDoc">XPathNavigator for the Document element</param>
        /// <returns>The value in the Reviewed element</returns>
        public static void SetReviewedForDocument(XPathNavigator docNavigator, string reviewedValue)
        {
            logger.Debug("Set the Reviewed value of the input document node.");

            if (docNavigator != null)
            {
                string xpathExpr = "./Reviewed";
                XPathNavigator reviewedNode = docNavigator.SelectSingleNode(xpathExpr);
                reviewedNode.SetValue(reviewedValue);
            }

            logger.Debug("Reviewed value updated for the input document. Reviewed - " + reviewedValue);
        }

        /// <summary>
        /// This method sorts all the Page nodes in the Document in the Ascending order of the Page Identifier numbers. 
        /// </summary>
        /// <param name="docNavigator">XPathNavigator for the Document element</param>
        public static void SortDocumentPages(XPathNavigator docNavigator)
        {
            logger.Debug("Sort the Pages nodes in the Document node - " + UtilFunctions.GetDocumentIdenifier(docNavigator));

            if (docNavigator != null)
            {
                SortedDictionary<int, XPathNavigator> sortedPageList = new SortedDictionary<int, XPathNavigator>();
                // Get all Page nodes in the current document and add it to the sorted map. 
                // The sorted map key is the digits after the 'PG' prefix of the Identifier. 
                // When all Page nodes are added to the map, the order of the Pages node will be in the ascending order of the numeric Identifier.
                XPathExpression expr = docNavigator.Compile("./Pages/Page");
                XPathNodeIterator iterator = docNavigator.Select(expr);

                // Iterate through the Nodes extracted and add to sorted map
                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();
                    XPathNavigator pageIdCurrentNode = nodeNavigator.SelectSingleNode("./Identifier");
                    int currentPageId;
                    Int32.TryParse(pageIdCurrentNode.Value.Substring(2), out currentPageId);
                    sortedPageList.Add(currentPageId, nodeNavigator);
                }

                logger.Debug("Sorted Map size: " + sortedPageList.Count);

                // Get a XPathNavigator to the Pages node to prepend the sorted elements from the map. 
                XPathNodeIterator pagesNode = docNavigator.Select("./Pages");
                pagesNode.MoveNext();

                // Get a XPathNavigator to the first Page node before prepending all the sorted Page nodes. 
                XPathNodeIterator pageNodeToDelete = docNavigator.Select("./Pages/Page");
                pageNodeToDelete.MoveNext();

                // Get a XPathNavigator to the last element of the Page node. 
                XPathNodeIterator lastPageNode = docNavigator.Select("./Pages/Page[last()]");
                lastPageNode.MoveNext();

                // Prepend all the sorted Page nodes to the beginning of the Page nodes iterator.    
                SortedDictionary<int, XPathNavigator>.ValueCollection valueColl = sortedPageList.Values;
                foreach (XPathNavigator node in valueColl.Reverse())
                {
                    pagesNode.Current.PrependChild(node);
                }

                // Delete the Page nodes after the prepended sorted Page node from the Pages element. 
                if (lastPageNode.Count != 0)
                    pageNodeToDelete.Current.DeleteRange(lastPageNode.Current);
            }

            logger.Debug("Sorted all Page nodes in the document");
        }



        /// <summary>
        /// This method updates element in the input document node from the Ephesoft XML with the value provided.
        /// </summary>
        /// <param name="documentNode">The XPathNavigator represents the element in which value will be updated.</param>
        /// <param name="elementName">The element name whose value needs to be updated.</param>
        /// <param name="value">The value to be updated. </param>
        /*public static void updateMultiElementByElementName(XPathNavigator documentNode, string elementName, string value)
        {
            // TODO Complete method
            try
            {
                XPathNodeIterator it = documentNode.Select("./" + elementName);
                // If a element is selected from the expression, update the given value.
                if (it.MoveNext())
                {
                    it.Current.SetValue(value);
                }
            }
            catch (XPathException xPathEx)
            {
                Console.WriteLine(xPathEx.Message);
            }
        }*/

        #endregion
    }
}