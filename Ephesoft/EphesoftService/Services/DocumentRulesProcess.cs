using EphesoftService.Models;
using MTSRuleEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.XPath;

namespace EphesoftService
{

    /// <summary>
    /// This class applies the various rules for Append, Convert and concatenate on the input Ephesoft XML.
    /// </summary>
    public class DocumentRulesProcess
    {

        #region CONSTANTS

        public const string ADVANCEDMERGE = "AdvancedMerge";

        #endregion

        private static readonly CustomLogger logger = new CustomLogger("DocumentRulesProcess");

        #region Public Methods
        /// <summary>
        /// This method applies the rules to convert the document type to a configured document type for
        /// the input ephesoft XML.
        /// </summary>
        /// <param name="inputXmlNavigator">The XPathNavigator object for the input ephesoft XML</param>
        /// <param name="conversionRules">The list of rules from the database for the input XML's batch class</param>
        public void ConvertDocumentTypes(XPathNavigator inputXmlNavigator, System.Data.DataTable conversionRules, string currentBatchId)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the conversion rules to the input XML");

            string docTypeName = null;
            string convertDocTypeName = null;
            string conversionRule = null;
            // For each rule in the DB, change the document name and desc as given in the db rule.
            foreach (DataRow dbRow in conversionRules.Rows)
            {
                docTypeName = dbRow["DocType"].ToString();
                convertDocTypeName = dbRow["ToDocType"].ToString();
                conversionRule = Convert.ToString(dbRow["CONVERSION_RULE"]);


                // Get the list of the document node from the XML for which the document type names must be changed. 
                List<XPathNavigator> documents = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, docTypeName);
                foreach (XPathNavigator doc in documents)
                {
                    if (!string.IsNullOrEmpty(conversionRule) && !CheckRule(doc, conversionRule))
                    {
                        logger.Error(String.Format("Batch Id:" + currentBatchId + " - Document Convertion: Convertion Rule Failed Rule"));
                        continue;
                    }

                    // For each document, change the document type name and description. 
                    UtilFunctions.UpdateSingleElementByElementName(doc, "Type", convertDocTypeName);
                    UtilFunctions.UpdateSingleElementByElementName(doc, "Description", convertDocTypeName);
                }
            }

            logger.Debug("Batch Id:" + currentBatchId + " - Applied all the conversion rules for the input XML");
        }

        private bool CheckRule(XPathNavigator doc, string conversionRule)
        {
            var output = new Dictionary<string, MTSRuleResult>();
            MTSRules CheckListRules = new MTSRules();
            CheckListRules["Rule1"] = conversionRule;
            output = CheckListRules.Eval(GetDocFields(doc));
            if (output["Rule1"].Result.ToString().ToLower() == "true")
                return true;
            else
                return false;

        }

        private Dictionary<string, object> GetDocFields(XPathNavigator doc)
        {
            Dictionary<string, object> docFields = new Dictionary<string, object>();
            string fieldName = string.Empty;
            string fieldValue = string.Empty;
            Document xmlDoc = new Document(doc, null);
            foreach (var fld in xmlDoc.DocumentLevelFields)
            {
                fieldName = fld.Name;
                fieldValue = string.IsNullOrEmpty(fld.Value) ? string.Empty : fld.Value;
                docFields.Add(fieldName, fieldValue);
            }
            return docFields;
        }


        public void ProcessParentChildMerge(XPathNavigator inputXmlNavigator, System.Data.DataTable ParentChildRules, string currentBatchId)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the Parent Child merge rules for the input XML.");

            // For each rule in the DB, check if the Parent Child Rules rule enabled
            foreach (DataRow dbRow in ParentChildRules.Rows)
            {
                if (dbRow["Active"] != DBNull.Value && Convert.ToBoolean(dbRow["Active"]))
                {
                    logger.Info("Batch Id:" + currentBatchId + " - Apply Parent Child merge rule for the input XML.");
                    ParentChildMergeBatch(inputXmlNavigator, currentBatchId, dbRow);
                }
            }
            logger.Debug("Batch Id:" + currentBatchId + " - Applied Parent Child merge rules for the input XML.");
        }

        public void ManualDocumentAssembly(XPathNavigator inputXmlNavigator, List<EnDocumentType> _lsEnDocs, bool isMissingDocument)
        {
            XPathNavigator firstDocument = UtilFunctions.GetFirstDocument(inputXmlNavigator);

            List<XPathNavigator> _allPages = UtilFunctions.GetAllPages(inputXmlNavigator);

            UtilFunctions.DeleteAllDocuments(inputXmlNavigator);

            //UtilFunctions.SetClassificationType(inputXmlNavigator);

            //UtilFunctions.CreateDocumentNode(firstDocument, "Valid", "false");
            //UtilFunctions.CreateDocumentNode(firstDocument, "Reviewed", "false");
            //UtilFunctions.CreateDocumentNode(firstDocument, "ConfidenceThreshold", "0.0");

            Int32 documentID = 1;

            if (isMissingDocument && _lsEnDocs.Count == 1 && _lsEnDocs[0].Pages.Count > 0 && _lsEnDocs[0].Pages[0] == -999)
            {
                XPathNavigator _tempDocument = firstDocument.Clone();
                UtilFunctions.DeleteAndInsertPages(_tempDocument, _allPages);
                UtilFunctions.SetDocumentNodeValue(_tempDocument, "Identifier", $"DOC{documentID.ToString()}");
                UtilFunctions.SetDocumentNodeValue(_tempDocument, "Type", _lsEnDocs[0].DocumentTypeName);
                UtilFunctions.SetDocumentNodeValue(_tempDocument, "Description", _lsEnDocs[0].DocumentTypeName);
                UtilFunctions.InsertDocument(inputXmlNavigator, _tempDocument);
            }
            else if (_lsEnDocs.Count > 0)
            {
                foreach (EnDocumentType enDocument in _lsEnDocs)
                {
                    XPathNavigator _tempDocument = firstDocument.Clone();

                    List<XPathNavigator> _docPages = new List<XPathNavigator>();

                    foreach (Int32 pgNo in enDocument.Pages)
                    {
                        XPathNavigator _curtPg = UtilFunctions.GetPageWithNumber(_allPages, pgNo);

                        if (_curtPg != null)
                            _docPages.Add(_curtPg);
                        else
                            logger.Debug($"Page No Missing : {pgNo.ToString()}");
                    }

                    UtilFunctions.DeleteAndInsertPages(_tempDocument, _docPages);

                    UtilFunctions.SetDocumentNodeValue(_tempDocument, "Identifier", $"DOC{documentID.ToString()}");
                    UtilFunctions.SetDocumentNodeValue(_tempDocument, "Type", enDocument.DocumentTypeName);
                    UtilFunctions.SetDocumentNodeValue(_tempDocument, "Description", enDocument.DocumentTypeName);

                    UtilFunctions.InsertDocument(inputXmlNavigator, _tempDocument);

                    documentID++;
                }
            }
        }


        /// <summary>
        /// This method appends documents based on the position given in the DB rule
        /// </summary>
        /// <param name="inputXmlDocument">The input Ephesoft XML</param>
        /// <param name="xmlBatchClasssId">The batch class ID of the input XML</param>
        public void ProcessAppendRules(XPathNavigator inputXmlNavigator, System.Data.DataTable appendRules, string currentBatchId)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the append rules to the input XML");

            // Retrieve the append rules from DB for Batch class Id.
            string docType1 = null;
            DocumentLocation position = new DocumentLocation();
            string toDocType2 = null;

            List<XPathNavigator> documentsToSort = new List<XPathNavigator>();
            // For each rule in the DB, concatenate the given document type
            foreach (DataRow dbRow in appendRules.Rows)
            {
                docType1 = dbRow["DocType1"].ToString();
                position = (DocumentLocation)Enum.Parse(typeof(DocumentLocation), dbRow["DocLocation"].ToString());
                toDocType2 = dbRow["DocType2"].ToString();

                // Get all the document node from the input XML, with the name in as DocType1 column.
                List<XPathNavigator> documents = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, docType1);

                // For each document, append DocType1 to DocType2 based on the position after or before. 
                // If DocType2 is null, append DocType1 to the next or previous document. 
                foreach (XPathNavigator doc in documents)
                {
                    XPathNavigator docAppendDestination = doc.Clone();
                    string documentIdentifier = UtilFunctions.GetDocumentIdenifier(doc);

                    if (position.Equals(DocumentLocation.AFTER))
                    {
                        // Check if the previous document node is available 
                        if (docAppendDestination.MoveToPrevious())
                        {
                            AppendDocuments(doc, docAppendDestination, toDocType2, position);
                        }
                        else
                        {
                            //UtilFunctions.SetReviewedForDocument(doc, "false");
                            logger.Error(String.Format("Batch Id:" + currentBatchId + " - Document Append: No document node available before the document - {0} to merge", documentIdentifier));
                        }
                    }
                    else if (position.Equals(DocumentLocation.BEFORE))
                    {
                        // Check if the next document node is available 
                        if (docAppendDestination.MoveToNext())
                        {
                            AppendDocuments(doc, docAppendDestination, toDocType2, position);
                        }
                        else
                        {
                            //UtilFunctions.SetReviewedForDocument(doc, "false");
                            logger.Error(String.Format("Batch Id:" + currentBatchId + " - Document Append: No document node available after the document - {0} to merge", documentIdentifier));
                        }
                    }
                    else if (position.Equals(DocumentLocation.UPWARD))
                    {
                        if (!String.IsNullOrEmpty(toDocType2))
                        {
                            Boolean checkPrevDocument = true;
                            do
                            {
                                if (docAppendDestination.MoveToPrevious())
                                {
                                    if (String.Equals(UtilFunctions.GetDocumentTypeName(docAppendDestination), toDocType2, StringComparison.OrdinalIgnoreCase))
                                    {
                                        checkPrevDocument = false;
                                        AppendDocuments(doc, docAppendDestination, toDocType2, DocumentLocation.AFTER);
                                        UtilFunctions.SortDocumentPages(docAppendDestination);
                                    }
                                }
                                else
                                {
                                    checkPrevDocument = false;
                                    logger.Error(String.Format("Batch Id:" + currentBatchId + " - Document Append: No document node available before the document - {0} to merge", documentIdentifier));
                                }
                            } while (checkPrevDocument);
                        }
                        else
                        {
                            logger.Error("Batch Id:" + currentBatchId + " - Document Append: No DocType2 specified for the UPWARD rule in DB for docType - " + documentIdentifier);
                        }
                    }
                    else if (position.Equals(DocumentLocation.DOWNWARD))
                    {
                        if (!String.IsNullOrEmpty(toDocType2))
                        {
                            Boolean checkNextDocument = true;
                            do
                            {
                                if (docAppendDestination.MoveToNext())
                                {
                                    if (String.Equals(UtilFunctions.GetDocumentTypeName(docAppendDestination), toDocType2, StringComparison.OrdinalIgnoreCase))
                                    {
                                        checkNextDocument = false;
                                        AppendDocuments(doc, docAppendDestination, toDocType2, DocumentLocation.BEFORE);
                                        UtilFunctions.SortDocumentPages(docAppendDestination);
                                    }
                                }
                                else
                                {
                                    checkNextDocument = false;
                                    logger.Error(String.Format("Batch Id:" + currentBatchId + " - Document Append: No document node available after the document - {0} to merge", documentIdentifier));
                                }
                            } while (checkNextDocument);
                        }
                        else
                        {
                            logger.Error("Batch Id:" + currentBatchId + " - Document Append: No DocType2 specified for the DOWNWARD rule in DB for docType - " + documentIdentifier);
                        }
                    }
                }
            }
            logger.Debug("Batch Id:" + currentBatchId + " - Applied all the append rules for the input XML");
        }


        /// <summary>
        /// This method concatenates documents of the same type. When the consecutive flag is set, only consecutive documents
        /// of the same type are merged.
        /// </summary>
        /// <param name="inputXmlDocument">The input Ephesoft XML</param>
        /// <param name="xmlBatchClasssId">The batch class ID of the input XML</param>
        public void ApplyConcatenateDocuments(XPathNavigator inputXmlNavigator, System.Data.DataTable concatenateRules, string currentBatchId)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the concatenate rules to the input XML.");

            string docTypeName = null;
            Boolean consecutiveFlag = false;
            int count = concatenateRules.Rows.Count;

            // For each rule in the DB, concatenate the given document type
            foreach (DataRow dbRow in concatenateRules.Rows)
            {
                docTypeName = dbRow["DocType"].ToString();
                consecutiveFlag = Boolean.Parse(dbRow["ConsecutiveFlag"].ToString());

                // Get a list of all the document node with the document type name as in the DB rule
                List<XPathNavigator> documents = UtilFunctions.GetDocumentOfDocType(inputXmlNavigator, docTypeName);

                if (documents.Any())
                {
                    // When consecutive flag is not set, merge all the documents in the list to the primary/first document.
                    if (!consecutiveFlag)
                    {
                        // All documents of the same type will be merged to the first document in the list.
                        XPathNavigator primaryDoc = documents[0].Clone();
                        documents.RemoveAt(0); // Remove the first document from the list as it will be the primary document to which others are merged.

                        foreach (XPathNavigator doc in documents)
                        {
                            UtilFunctions.MergeDocument(doc, primaryDoc, DocumentLocation.AFTER);
                        }
                    }
                    else
                    {
                        List<XPathNavigator> consecutiveDocsToMerge = new List<XPathNavigator>();
                        // If the consecutive flag is set to true, merge only consecutive documents of the same type 
                        // to the first document in the consecutive list
                        foreach (XPathNavigator doc in documents)
                        {
                            string docIdentifier = UtilFunctions.GetDocumentIdenifier(doc);
                            logger.Debug("Batch Id:" + currentBatchId + " - Checking the consecutive documents for DocID: " + docIdentifier);

                            List<XPathNavigator> docsToMerge = new List<XPathNavigator>();

                            XPathNavigator nextDoc = doc.Clone();
                            while (nextDoc.MoveToNext())
                            {
                                // Check all document after current doc , if they are the same doc type add them 
                                // to a list to be merged. 
                                string nextDocType = UtilFunctions.GetDocumentTypeName(nextDoc);
                                if (nextDocType.Equals(docTypeName))
                                {
                                    docsToMerge.Add(nextDoc.Clone());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            // Merge the list of consecutive documents. 
                            foreach (XPathNavigator mergeDoc in docsToMerge)
                            {
                                UtilFunctions.MergeDocument(mergeDoc, doc, DocumentLocation.AFTER);
                            }
                            docsToMerge.Clear();
                        }
                    }
                }
            }

            logger.Debug("Batch Id:" + currentBatchId + " - Applied all the concatenate rules for the input XML");
        }


        /// <summary>
        /// This method applies the advanced rules like AdvanceMerge if its set for the config Id. 
        /// </summary>
        /// <param name="inputXmlNavigator">XPathNavigator reference for the input Batch XML</param>
        /// <param name="advancedRules">The list of advanced rules for the configId</param>
        public void ApplyAdvancedRules(XPathNavigator inputXmlNavigator, System.Data.DataTable advancedRules, string currentBatchId)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the advanced rules for the input XML.");

            string advancedRule = null;
            Boolean flag = false;

            // For each rule in the DB, check if the advanced rule enabled
            foreach (DataRow dbRow in advancedRules.Rows)
            {
                advancedRule = dbRow["AdvancedOptions"].ToString();
                flag = Boolean.Parse(dbRow["Flag"].ToString());

                if (advancedRule.Equals(ADVANCEDMERGE) && flag)
                {
                    logger.Info("Batch Id:" + currentBatchId + " - Apply Advanced Merge rule for the input XML.");
                    AdvanceMergeBatch(inputXmlNavigator, currentBatchId);
                }
            }
            logger.Debug("Batch Id:" + currentBatchId + " - Applied Advanced rules for the input XML.");
        }


        public void PageSequenceMerge(List<XPathNavigator> documentsList, XPathNavigator inputXmlNavigator)
        {
            foreach (XPathNavigator document in documentsList.ToList())
            {
                List<int> PageList = UtilFunctions.GetAllPagesFromDocument(document);
                if (PageList.Count > 0)
                {
                    List<int> MissingPgNo = Enumerable.Range(PageList.Min(), PageList.Max() - PageList.Min() + 1).Except(PageList).ToList();
                    if (MissingPgNo.Count > 0)
                    {
                        UtilFunctions.GetPageAndMerge(inputXmlNavigator, MissingPgNo, document);
                        UtilFunctions.SortDocumentPages(document);
                    }
                }
            }

        }


        #endregion

        #region Private Methods

        //Prakash : Commented for Mail Subject : Advanced Merge Rule 
        //                        Mail Subject : PUB Ephesoft - Advanced Merge Rule
        ///// <summary>
        ///// This method performs the Advanced Merge function on the input XML.
        ///// The Advanced Merge checks for the following conditions to merge documents:
        ///// For each document in the XML
        ///// - Check if the cuurrent document is confident, if yes continue
        ///// - Get the details of the first and second consecutive documents
        ///// - If the current document type is the same as the second consecutive document, continue
        ///// - Check if the first consecutive document is not confident, if yes continue.
        ///// - Get the Ephesoft classification page type (FIRST, MIDDLE or LAST) for the first page 
        /////   of the first and second consecutive page. 
        ///// - If its Middle, merge the first and second consecutive document to the current document
        ///// </summary>
        ///// <param name="inputXmlNavigator">XPathNavigator</param>
        //private void AdvanceMergeBatch(XPathNavigator inputXmlNavigator, string currentBatchId)
        //{
        //    logger.Debug("Batch Id:" + currentBatchId + " - Apply the Advanced merge rule for the input XML.");

        //    // Get all the documents in the XML
        //    List<XPathNavigator> documentsList = UtilFunctions.GetAllDocuments(inputXmlNavigator);
        //    if (documentsList.Any())
        //    {
        //        int totalDocCount = documentsList.Count;
        //        int currentIndex = 1;
        //        int firstConsecutiveDocIndex = 0, secondConsecutiveDocIndex = 0;
        //        // For each document check is AdvancedMerge should be performed.
        //        foreach (XPathNavigator doc in documentsList)
        //        {
        //            // Check if the current document is confident.
        //            Boolean docConfidence = UtilFunctions.IsDocConfident(doc);
        //            if (docConfidence)
        //            {
        //                string currentDocType = UtilFunctions.GetDocumentTypeName(doc);

        //                // Get the details of the first consecutive Document
        //                XPathNavigator firstConsecutiveDoc = doc.Clone();
        //                firstConsecutiveDoc.MoveToNext();
        //                firstConsecutiveDocIndex = currentIndex + 1;
        //                // Get the details of the second consecutive document
        //                XPathNavigator secondConsecutiveDoc = firstConsecutiveDoc.Clone();
        //                secondConsecutiveDoc.MoveToNext();
        //                secondConsecutiveDocIndex = currentIndex + 2;
        //                string secondConsecDocType = UtilFunctions.GetDocumentTypeName(secondConsecutiveDoc);

        //                if (secondConsecutiveDocIndex > totalDocCount)
        //                {
        //                    break;
        //                }

        //                // If the current document type is the same as the second consecutive document type, continue
        //                if (currentDocType.Equals(secondConsecDocType))
        //                {
        //                    // Check if the first consecutive document is not confident, if yes continue.
        //                    if (!UtilFunctions.IsDocConfident(firstConsecutiveDoc))
        //                    {
        //                        // Get the Ephesoft classification page type (FIRST, MIDDLE or LAST) for the first page 
        //                        // of the first and second consecutive page. If its Middle, continue. 
        //                        EphesoftPageType firstConsDocPageType = UtilFunctions.GetFirstPageType(firstConsecutiveDoc);
        //                        EphesoftPageType secondConsDocPageType = UtilFunctions.GetFirstPageType(secondConsecutiveDoc);

        //                        // If both the consecutive documents start with a middle page, continue
        //                        if (firstConsDocPageType == EphesoftPageType.MIDDLE && secondConsDocPageType == EphesoftPageType.MIDDLE)
        //                        {
        //                            logger.Debug("Batch Id:" + currentBatchId + " - Merge consecutive docs for Document type - " + currentDocType);
        //                            // Merge the first and second consecutive document to the current document
        //                            UtilFunctions.MergeDocument(firstConsecutiveDoc, doc, DocumentLocation.AFTER);
        //                            UtilFunctions.MergeDocument(secondConsecutiveDoc, doc, DocumentLocation.AFTER);
        //                        }
        //                    }
        //                }
        //            }
        //            currentIndex++;
        //        }
        //    }
        //    logger.Debug("Batch Id:" + currentBatchId + " - Applied the advanced merge rule for the input XML");
        //}

        /// <summary>
        /// This method performs the Advanced Merge function on the input XML.
        /// The Advanced Merge checks for the following conditions to merge documents:
        /// For each document in the XML
        /// - Check if the cuurrent document is confident, if yes continue
        /// - Get the details of the first and second consecutive documents
        /// - If the current document type is the same as the second consecutive document, continue
        /// - Check if the first consecutive document is not confident, if yes continue.
        /// - Get the Ephesoft classification page type (FIRST, MIDDLE or LAST) for the first page 
        ///   of the first and second consecutive page. 
        /// - If its Middle, merge the first and second consecutive document to the current document
        /// </summary>
        /// <param name="inputXmlNavigator">XPathNavigator</param>
        private void AdvanceMergeBatch(XPathNavigator inputXmlNavigator, string currentBatchId)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the Advanced merge rule for the input XML.");

            List<XPathNavigator> documentsList = UtilFunctions.GetAllDocuments(inputXmlNavigator);
            if (documentsList.Any())
            {
                foreach (XPathNavigator doc in documentsList)
                {
                    Boolean docConfidence = UtilFunctions.IsDocConfident(doc);
                    if (docConfidence)
                    {
                        string currentDocType = UtilFunctions.GetDocumentTypeName(doc);

                        XPathNavigator firstConsecutiveDoc = doc.Clone();
                        firstConsecutiveDoc.MoveToNext();

                        string firstConsecutiveDocType = UtilFunctions.GetDocumentTypeName(firstConsecutiveDoc);

                        if (currentDocType.Equals(firstConsecutiveDocType))
                        {
                            if (!UtilFunctions.IsDocConfident(firstConsecutiveDoc))
                            {
                                EphesoftPageType firstConsDocPageType = UtilFunctions.GetFirstPageType(firstConsecutiveDoc);

                                if (firstConsDocPageType == EphesoftPageType.MIDDLE)
                                {
                                    logger.Debug("Batch Id:" + currentBatchId + " - Merge consecutive docs for Document type - " + currentDocType);

                                    UtilFunctions.MergeDocument(firstConsecutiveDoc, doc, DocumentLocation.AFTER);
                                }
                            }
                        }
                    }
                }
            }
            logger.Debug("Batch Id:" + currentBatchId + " - Applied the advanced merge rule for the input XML");
        }


        private void ParentChildMergeBatch(XPathNavigator inputXmlNavigator, string currentBatchId, DataRow dRow)
        {
            logger.Debug("Batch Id:" + currentBatchId + " - Apply the Parent Child merge rule for the input XML.");
            XPathNavigator ChildDocument = null;
            XPathNavigator ParentDocument = null;

            // Get the child document in the XML using firstpage and secondpage doctype of configid
            ChildDocument = UtilFunctions.GetDocumentViaFirstAndSecondPage(inputXmlNavigator, dRow["ParentFirstPageDocType"].ToString(), dRow["ChildFirstPageDocType"].ToString());

            if (ChildDocument != null)
            {
                //Get the parent document in the XML using child document
                ParentDocument = UtilFunctions.GetDocumentViaChildDocument(inputXmlNavigator, ChildDocument);
            }


            if (ParentDocument != null && ChildDocument != null)
            {
                UtilFunctions.MergeDocument(ChildDocument, ParentDocument, DocumentLocation.AFTER);
                UtilFunctions.SortDocumentPages(ParentDocument);
            }

            logger.Debug("Batch Id:" + currentBatchId + " - Applied the Parent child merge rule for the input XML");
        }


        /// <summary>
        /// This method adds a document to another document depending the position input AFTER or BEFORE. If the 
        /// database provided the documentname it should be merged to, the document is appended only to this document
        /// type. 
        /// </summary>
        /// <param name="docToAppend">The document to be appended</param>
        /// <param name="docAppendDestination">The document to which the first document is appended</param>
        /// <param name="docAppendDestinationDB">The name of the document to which the input document should be appended</param>
        /// <param name="position">The position to append the document</param>
        private void AppendDocuments(XPathNavigator docToAppend, XPathNavigator docAppendDestination, string docAppendDestinationDB, DocumentLocation position)
        {
            logger.Debug("Merge the input documents based on the input position");

            // Get the name of the document type of the next Document node
            string docTypeName = UtilFunctions.GetDocumentTypeName(docAppendDestination);

            // If the DocType2 column is not null in the DB, then DocType1 should be merged only with docType2.
            if (!String.IsNullOrEmpty(docAppendDestinationDB))
            {
                // Check if the destination document type is DocType2, to continue with the merge.
                if (String.Equals(docAppendDestinationDB, docTypeName, StringComparison.OrdinalIgnoreCase))
                {
                    UtilFunctions.MergeDocument(docToAppend, docAppendDestination, position);
                    logger.Info("Merged DocType1 with DocType2 - " + docAppendDestinationDB);
                }
                else
                {
                    //UtilFunctions.SetReviewedForDocument(docToAppend, "false");
                    logger.Error(String.Format("Document Append: The DocType1 could not be appended. {0} not found after/before current document to merge. ", docTypeName));
                }
            }
            else
            {
                logger.Info(String.Format("Merge document to {0} document in the input position.", docTypeName));
                UtilFunctions.MergeDocument(docToAppend, docAppendDestination, position);
            }
            logger.Debug("Merge the input documents based on the input position.");
        }

        #endregion

    }
}
