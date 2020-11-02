using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.XPath;

namespace IL.ImportStaging
{

    public class XMLBatchManipulate
    {
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

    public class XMLBatchImage : XMLBatchManipulate
    {
        public XPathNavigator SourceinputXmlNavigator { get; set; }
        public XPathNavigator inputXmlNavigator { get; set; }

        private List<string> m_ImagePath;

        public List<string> ImagePaths { get { return m_ImagePath; } }

        public XMLBatchImage(XPathNavigator _inputXmlNavigator, string _ephsoftOutputPath)
        {
            SourceinputXmlNavigator = _inputXmlNavigator;
            inputXmlNavigator = GetNodListNavigator(_inputXmlNavigator, "//Batch")[0];

            List<XPathNavigator> documentList = GetNodListNavigator(inputXmlNavigator, "//Document");
            m_ImagePath = new List<string>();

            foreach (var doc in documentList)
            {
                string imagePath = GetNodeValue(doc, "MultiPageTiffFile");

                if (imagePath.ToLower().IndexOf(@"\output\") < 0)
                    throw new ImagePathException($"Wrong Image Path : '{imagePath}'");

                m_ImagePath.Add(Path.Combine(_ephsoftOutputPath, Path.GetFileName(imagePath)));
            }

        }

    }

    public class ImagePathException : Exception
    {
        public ImagePathException(string message) : base(message)
        { }

        public ImagePathException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}