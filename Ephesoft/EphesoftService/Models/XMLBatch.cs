using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService
{   

    public class XMLBatchManipulate
    {
        private static readonly CustomLogger logger = new CustomLogger("XMLBatchManipulate");

        public List<XPathNavigator> GetNodListNavigator(XPathNavigator navigator, string nodeName)
        {
            List<XPathNavigator> documentList = new List<XPathNavigator>();
            if (navigator != null)
            {                
                XPathExpression expr = navigator.Compile(nodeName);
                XPathNodeIterator iterator = navigator.Select(expr);

                // Iterate through the Nodes extracted and add to list
                while (iterator.MoveNext())
                {
                    XPathNavigator nodeNavigator = iterator.Current.Clone();
                    documentList.Add(nodeNavigator);
                }
            }

            return documentList;
        }

        public string GetNodeValue(XPathNavigator docNavigator, string NodeName)
        {
            try
            {
                return Convert.ToString(docNavigator.SelectSingleNode("./" + NodeName).Value);
            }
            catch (Exception ex)
            {
                //Log If need
                //logger.Debug("Error While Parsing " + NodeName + " " + ex.Message);
                return string.Empty;
            }
        }        
    }

    public class XMLBatch : XMLBatchManipulate
    {
        public XPathNavigator SourceinputXmlNavigator { get; set; }
        public XPathNavigator inputXmlNavigator { get; set; }
        private string m_BatchInstanceIdentifier;
        private string m_BatchClassIdentifier;        
        private string m_BatchClassVersion;
        private string m_BatchName;
        private string m_BatchClassName;
        private string m_BatchDescription;
        private string m_BatchPriority;
        private string m_BatchStatus;
        private string m_BatchCreationDate;
        private string m_BatchLocalPath;
        private string m_UNCFolderPath;       
        private List<Document> m_Documents;
        private Int64 m_PageCount;


        public string BatchInstanceIdentifier { get { return m_BatchInstanceIdentifier; } }
        public string BatchClassIdentifier { get { return m_BatchClassIdentifier; } }        
        public string BatchClassVersion { get { return m_BatchClassVersion; } }
        public string BatchName { get { return m_BatchName; } }
        public string BatchClassName { get { return m_BatchClassName; } }
        public string BatchDescription { get { return m_BatchDescription; } }
        public string BatchPriority { get { return m_BatchPriority; } }
        public string BatchStatus { get { return m_BatchStatus; } }
        public string BatchCreationDate { get { return m_BatchCreationDate; } }
        public string BatchLocalPath { get { return m_BatchLocalPath; } }
        public string UNCFolderPath { get { return m_UNCFolderPath; } }        
        public List<Document> Documents { get { return m_Documents; } }
        public Int64 PageCount { get { return m_PageCount; } }

        public XMLBatch(XPathNavigator _inputXmlNavigator,bool _loadDocument=true)
        {
            SourceinputXmlNavigator = _inputXmlNavigator;
            inputXmlNavigator = GetNodListNavigator(_inputXmlNavigator, "//Batch")[0];
            m_BatchInstanceIdentifier = GetNodeValue(inputXmlNavigator, "BatchInstanceIdentifier");
            m_BatchClassIdentifier = GetNodeValue(inputXmlNavigator, "BatchClassIdentifier");
            m_BatchClassVersion = GetNodeValue(inputXmlNavigator, "BatchClassVersion");
            m_BatchName = GetNodeValue(inputXmlNavigator, "BatchName");
            m_BatchClassName = GetNodeValue(inputXmlNavigator, "BatchClassName");
            m_BatchDescription = GetNodeValue(inputXmlNavigator, "BatchDescription");
            m_BatchPriority = GetNodeValue(inputXmlNavigator, "BatchPriority");
            m_BatchStatus = GetNodeValue(inputXmlNavigator, "BatchStatus");
            m_BatchCreationDate = GetNodeValue(inputXmlNavigator, "BatchCreationDate");
            m_BatchLocalPath = GetNodeValue(inputXmlNavigator, "BatchLocalPath");
            m_UNCFolderPath = GetNodeValue(inputXmlNavigator, "UNCFolderPath");
            m_PageCount = GetNodListNavigator(inputXmlNavigator, "//Document//Pages//Page").Count;


            List<XPathNavigator> documentList = GetNodListNavigator(inputXmlNavigator, "//Document");
            m_Documents = new List<Document>();

            if (_loadDocument)
            {
                foreach (var doc in documentList)
                {
                    m_Documents.Add(new Document(doc, this));
                }
            }
        }


       
    }

    public class Document : XMLBatchManipulate
    {
        public XMLBatch m_Parrent;        
        public string m_Identifier;
        public string m_Type;
        public string m_Description;
        public string m_Confidence;
        public bool m_Reviewed;
        public string m_MultiPagePdfFile;
        public string m_MultiPageTiffFile;
        public bool m_Valid;
        public List<DocumentLevelField> m_DocumentLevelFields;

        public XMLBatch Batch { get { return m_Parrent; } }
        public string Identifier { get { return m_Identifier; } }
        public string Type { get { return m_Type; } }
        public string Description { get { return m_Description; } }
        public string Confidence { get { return m_Confidence; } }
        public bool Reviewed { get { return m_Reviewed; } set { m_Reviewed = value; SetFieldValue("Reviewed", Convert.ToString(m_Reviewed).ToLower()); } }
        public string MultiPagePdfFile { get { return m_MultiPagePdfFile; } }
        public string MultiPageTiffFile { get { return m_MultiPageTiffFile; } }
        public bool Valid { get { return m_Valid; } set { m_Valid = value; SetFieldValue("Valid", Convert.ToString(m_Valid).ToLower()); } }
        public List<DocumentLevelField> DocumentLevelFields { get {return m_DocumentLevelFields; } }

        public Document(XPathNavigator _inputXmlNavigator, XMLBatch _parrent)
        {
            m_Parrent = _parrent;            
            m_Identifier = GetNodeValue(_inputXmlNavigator, "Identifier");
            m_Type = GetNodeValue(_inputXmlNavigator, "Type");
            m_Description = GetNodeValue(_inputXmlNavigator, "Description");
            m_Confidence = GetNodeValue(_inputXmlNavigator, "Confidence");
            if (Convert.ToString(GetNodeValue(_inputXmlNavigator, "Reviewed")).Trim().ToLower() == "true")
                m_Reviewed = true;
            if (Convert.ToString(GetNodeValue(_inputXmlNavigator, "Valid")).Trim().ToLower() == "true")
                m_Valid = true;
            m_MultiPagePdfFile = GetNodeValue(_inputXmlNavigator, "MultiPagePdfFile");
            m_MultiPageTiffFile = GetNodeValue(_inputXmlNavigator, "MultiPageTiffFile");            
            List<XPathNavigator> fieldList = GetNodListNavigator(_inputXmlNavigator, "./DocumentLevelFields/DocumentLevelField");
            m_DocumentLevelFields = new List<DocumentLevelField>();
            foreach (var fld in fieldList)
            {
                m_DocumentLevelFields.Add(new DocumentLevelField(fld,this));
            }
        }

        public void SetFieldValue(string node, string value)
        {
            XPathExpression expr = this.Batch.SourceinputXmlNavigator.Compile("//Document[Identifier=\"" + this.Identifier + "\"]");
            XPathNodeIterator iterator = this.Batch.SourceinputXmlNavigator.Select(expr);

            // Iterate through the Nodes extracted and add to list
            while (iterator.MoveNext())
            {
                XPathNavigator nodeNavigator = iterator.Current;
                var cnode = nodeNavigator.SelectSingleNode("./" + node);
                if (cnode != null)
                    cnode.SetValue(value);
            }
        }


    }

    public class DocumentLevelField : XMLBatchManipulate
    {
        public Document m_Parrent;        
        public string m_Name;
        public string m_Value;
        public string m_Type;
        public string m_Confidence;
        public bool m_ForceReview;
        public string m_FieldValueOptionList;


        public Document Document { get { return m_Parrent; } }
        public string Name { get { return m_Name; } }
        public string Value { get { return m_Value; } set { m_Value = value; SetFieldValue("Value",value); } }
        public string Type { get { return m_Type; } }
        public string Confidence { get { return m_Confidence; } }
        public bool ForceReview { get { return m_ForceReview; } set { m_ForceReview = value; SetFieldValue("ForceReview", Convert.ToString(m_ForceReview).ToLower()); } }
        public string FieldValueOptionList { get { return m_FieldValueOptionList; } set { m_Value = value; SetFieldValue("FieldValueOptionList", value); } }


        public DocumentLevelField(XPathNavigator _inputXmlNavigator, Document _parent)
        {
            m_Parrent = _parent;            
            m_Name = GetNodeValue(_inputXmlNavigator, "Name");
            m_Value = GetNodeValue(_inputXmlNavigator, "Value");
            m_Type = GetNodeValue(_inputXmlNavigator, "Type");
            m_Confidence = GetNodeValue(_inputXmlNavigator, "Confidence");            
            if (Convert.ToString(GetNodeValue(_inputXmlNavigator, "ForceReview")).Trim().ToLower() == "true")
                m_ForceReview = true;
            m_FieldValueOptionList = GetNodeValue(_inputXmlNavigator, "FieldValueOptionList");
        }


        public void SetFieldValue(string node,string value)
        {
            XPathExpression expr = this.Document.Batch.SourceinputXmlNavigator.Compile("//Document[Identifier=\"" + this.Document.Identifier+"\"]/DocumentLevelFields/DocumentLevelField[Name=\""+this.Name+"\"]");
            XPathNodeIterator iterator = this.Document.Batch.SourceinputXmlNavigator.Select(expr);

            // Iterate through the Nodes extracted and add to list
            while (iterator.MoveNext())
            {
                XPathNavigator nodeNavigator = iterator.Current;
                var cnode = nodeNavigator.SelectSingleNode("./" + node);
                if(cnode!=null)
                    cnode.SetValue(value);
            }
        }
    }

}