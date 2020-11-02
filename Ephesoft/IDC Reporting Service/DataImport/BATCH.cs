using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DataImport
{
    public partial class BATCH
    {
        public string BatchInstanceID; //= ConfigurationManager.AppSettings["REPORT_DATA_FOLDER"].ToString();
        public string BatchClassIdentifier;

        public string REPORT_DATA_FOLDER = ConfigurationManager.AppSettings["REPORT_DATA_FOLDER"].ToString();
        public string PATTERNS = ConfigurationManager.AppSettings["PATTERNS"].ToString();
        public string EphesoftConnection = ConfigurationManager.ConnectionStrings["EPHESOFTDB"].ToString();
        public string IntellaConnection = ConfigurationManager.ConnectionStrings["INTELLADB"].ToString();

        public void ProcessPatterns()
        {
            this.BatchInstanceID = this.BATCH_INSTANCEID;
            GetBatchClassIdentifier();
            LoadXMLFile(GetPageProcessedPatterns(), GetFieldProcessedPatterns());
        }

        public void GetBatchClassIdentifier()
        {
            using (SqlConnection connection = new SqlConnection(EphesoftConnection))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(null, connection))
                {
                    //For EphesoftDB
                    string str = "SELECT batch_class_id FROM BATCH_CLASS WHERE batch_class_id = (SELECT BATCH_CLASS_ID FROM BATCH_INSTANCE WHERE IDENTIFIER = '" + this.BatchInstanceID + "')";

                    //For Ephesoft Reporting DB
                    // string str = "SELECT BATCH_CLASS_ID FROM BATCH_INSTANCE WHERE IDENTIFIER = '" + this.BatchInstanceID + "'";
                    sqlCommand.CommandText = str;
                    using (IDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (!dataReader.Read())
                            return;

                        //For EphesoftDB
                        this.BatchClassIdentifier = dataReader["batch_class_id"].ToString();

                        //For Ephesoft Reporting DB
                        // this.BatchClassIdentifier = dataReader["BATCH_CLASS_ID"].ToString();
                    }
                }
            }
        }

        public List<string> GetPageProcessedPatterns()
        {
            List<string> stringList = new List<string>();
            using (SqlConnection connection = new SqlConnection(IntellaConnection))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(null, connection))
                {
                    string str1 = "SELECT DISTINCT PATTERN FROM PAGES WHERE BATCH_INSTANCEID = '" + this.BatchInstanceID + "'";
                    sqlCommand.CommandText = str1;
                    using (IDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string str2 = dataReader["PATTERN"].ToString();
                            stringList.Add(str2);
                        }
                    }
                }
            }
            return stringList;
        }

        public List<string> GetFieldProcessedPatterns()
        {
            List<string> stringList = new List<string>();
            using (SqlConnection connection = new SqlConnection(IntellaConnection))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(null, connection))
                {
                    string str1 = "SELECT DISTINCT PATTERN FROM FIELDS WHERE BATCH_INSTANCEID = '" + this.BatchInstanceID + "'";
                    sqlCommand.CommandText = str1;
                    using (IDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            string str2 = dataReader["PATTERN"].ToString();
                            stringList.Add(str2);
                        }
                    }
                }
            }
            return stringList;
        }

        private void LoadXMLFile(List<string> PageValuePL, List<string> FieldValuePL)
        {
            string str1 = Path.Combine(REPORT_DATA_FOLDER, this.BATCH_INSTANCEID);
            Console.WriteLine($"LoadXML File Path : {str1}");
            Console.WriteLine($"PATTERNS : {PATTERNS}");
            foreach (string format in PATTERNS.Split(';'))
            {
                string nPattern = string.Format(format, this.BATCH_INSTANCEID, this.BatchClassIdentifier);
                string str2 = Path.Combine(str1, nPattern);
                Console.WriteLine($"XML File Path : {str2}");
                if (File.Exists(str2))
                {
                    if (PageValuePL.IndexOf(nPattern) < 0)
                        this.SetPageInformation(this.GetXmlRootElement(str2), nPattern);
                    if (FieldValuePL.IndexOf(nPattern) < 0)
                        this.SetFieldInformation(this.GetXmlRootElement(str2), nPattern);


                    Console.WriteLine($"XML File Exsits : {str2}");
                }
                else
                    Console.WriteLine($"XML File Not Found : {str2}");
            }
            Console.WriteLine($"LoadXML File Path : {str1}");
        }

        private void SetPageInformation(XElement RootElement, string nPattern)
        {
            using (StringReader stringReader = new StringReader(RootElement.ToString()))
            {
                XPathNodeIterator xpathNodeIterator1 = new XPathDocument((TextReader)stringReader).CreateNavigator().Select("/Batch/Documents/Document");
                while (xpathNodeIterator1.MoveNext())
                {
                    string valueAsString1 = this.GetValueAsString(xpathNodeIterator1.Current.SelectSingleNode("Identifier"));
                    string valueAsString2 = this.GetValueAsString(xpathNodeIterator1.Current.SelectSingleNode("Type"));
                    Decimal valueAsDecimal1 = this.GetValueAsDecimal(xpathNodeIterator1.Current.SelectSingleNode("Confidence"));
                    Decimal valueAsDecimal2 = this.GetValueAsDecimal(xpathNodeIterator1.Current.SelectSingleNode("ConfidenceThreshold"));
                    bool valueAsBoolean1 = this.GetValueAsBoolean(xpathNodeIterator1.Current.SelectSingleNode("Valid"));
                    bool valueAsBoolean2 = this.GetValueAsBoolean(xpathNodeIterator1.Current.SelectSingleNode("Reviewed"));
                    XPathNodeIterator xpathNodeIterator2 = xpathNodeIterator1.Current.Select("Pages/Page");
                    int num = 0;
                    while (xpathNodeIterator2.MoveNext())
                    {
                        PAGE entity = new PAGE();
                        entity.PAGEID = this.GetValueAsString(xpathNodeIterator2.Current.SelectSingleNode("Identifier"));
                        string pageid = entity.PAGEID;
                        entity.PATTERN = nPattern;
                        entity.BATCH_INSTANCEID = this.BATCH_INSTANCEID;
                        entity.DOCID = valueAsString1;
                        entity.DOCTYPE = valueAsString2;
                        entity.DOC_CONFIDENCE = valueAsDecimal1;
                        entity.CONFIDENCE_THRESHOLD = valueAsDecimal2;
                        entity.VALID = valueAsBoolean1;
                        entity.REVIEWED = valueAsBoolean2;
                        entity.PAGE_POSITION = num != 0 ? (num != xpathNodeIterator2.Count - 1 ? "MIDDLE" : "LAST") : "FIRST";
                        ++num;
                        XPathNodeIterator xpathNodeIterator3 = xpathNodeIterator2.Current.Select("PageLevelFields/PageLevelField");
                        if (xpathNodeIterator3.MoveNext())
                        {
                            entity.PAGE_CONFIDENCE = this.GetValueAsDecimal(xpathNodeIterator3.Current.SelectSingleNode("Confidence"));
                            entity.LEARNED_FILENAME = this.GetValueAsString(xpathNodeIterator3.Current.SelectSingleNode("LearnedFileName"));
                        }
                        Program.ReportDb.PAGES.Add(entity);
                    }
                }
            }
        }

        private void InsertPages()
        {

        }

        private void SetFieldInformation(XElement RootElement, string nPattern)
        {
            using (StringReader stringReader = new StringReader(RootElement.ToString()))
            {
                XPathNodeIterator xpathNodeIterator1 = new XPathDocument((TextReader)stringReader).CreateNavigator().Select("/Batch/Documents/Document");
                while (xpathNodeIterator1.MoveNext())
                {
                    string valueAsString1 = this.GetValueAsString(xpathNodeIterator1.Current.SelectSingleNode("Identifier"));
                    string valueAsString2 = this.GetValueAsString(xpathNodeIterator1.Current.SelectSingleNode("Type"));
                    XPathNodeIterator xpathNodeIterator2 = xpathNodeIterator1.Current.Select("DocumentLevelFields/DocumentLevelField");
                    while (xpathNodeIterator2.MoveNext())
                        Program.ReportDb.FIELDS.Add(new FIELD()
                        {
                            FIELDNAME = this.GetValueAsString(xpathNodeIterator2.Current.SelectSingleNode("Name")),
                            FIELDVALUE = this.GetValueAsString(xpathNodeIterator2.Current.SelectSingleNode("Value")),
                            PATTERN = nPattern,
                            BATCH_INSTANCEID = this.BATCH_INSTANCEID,
                            DOCID = valueAsString1,
                            DOCTYPE = valueAsString2
                        });
                }
            }
        }

        private string GetValueAsString(XPathNavigator navigator)
        {
            return navigator != null ? navigator.Value : null;
        }

        private Decimal GetValueAsDecimal(XPathNavigator navigator)
        {
            return navigator != null ? Convert.ToDecimal(navigator.Value) : 0;
        }

        private bool GetValueAsBoolean(XPathNavigator navigator)
        {
            return navigator != null && Convert.ToBoolean(navigator.Value);
        }

        private XElement GetXmlRootElement(string ZipFilePath)
        {
            return XDocument.Load(XmlReader.Create((TextReader)new StringReader(this.GetXmlContent(ZipFilePath)))).Element((XName)"Batch");
        }

        private string GetXmlContent(string FilePath)
        {
            string str = (string)null;
            using (ZipFile zipFile = ZipFile.Read(FilePath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zipFile[0].Extract(memoryStream);
                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        memoryStream.Position = 0L;
                        str = streamReader.ReadToEnd();
                    }
                }
            }
            return str;
        }

        public void SetEphesoftMetadata()
        {
            string str1 = "select creation_date, last_modified, review_operator_user_name, validation_operator_user_name, batch_status, batch_name from  batch_instance with(nolock) where identifier = '" + this.BATCH_INSTANCEID + "'";

            // string str2 = "SELECT  batch_class_id, batch_class_name FROM BATCH_INSTANCE WHERE IDENTIFIER = '" + this.BATCH_INSTANCEID + "'";
            string str2 = "select bc.batch_class_id, bc.batch_class_name from batch_class bc where bc.batch_class_id = (select bi.batch_class_id from batch_instance bi where bi.identifier = '" + this.BATCH_INSTANCEID + "')";
            using (SqlConnection connection = new SqlConnection(EphesoftConnection))
            {
                connection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(null, connection))
                {
                    sqlCommand.CommandText = str1;
                    using (IDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            this.CREATEDATE = dataReader.GetDateTime(0);
                            this.LASTMODIFIEDDATE = dataReader.GetDateTime(1);
                            this.REVIEW_OPERATOR = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                            this.VALIDATION_OPERATOR = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                            this.STATUS = dataReader.GetString(4);
                            this.BATCH_NAME = dataReader.GetString(5);
                        }
                    }
                }
                using (SqlCommand sqlCommand = new SqlCommand(null, connection))
                {
                    sqlCommand.CommandText = str2;
                    using (IDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            this.BATCHCLASS_ID = dataReader.GetString(0);
                            this.BATCHCLASS_NAME = dataReader.GetString(1);
                        }
                    }
                }
            }
        }
    }
}
