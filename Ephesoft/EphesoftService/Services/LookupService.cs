using EphesoftService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService
{
    public class LookupService
    {
        public XPathNavigator InputXmlNavigator { get; set; }
        private Int64 m_LoanID;
        private Int64 m_LoanTypeID;
        private DateTime? m_QCIQStartDate;
        private string m_LoanType;
        private string m_Schema;
        private XMLBatch m_XMLBatch;
        private string m_ReviewTypeName { get; set; }
        private string m_CustomerName { get; set; }
        private string m_QCIQConnectionString { get; set; }
        private string m_MasterQCIQConnectionString { get; set; }
        private string m_QCIQSQLScript { get; set; }
        private string m_LoanNumber { get; set; }
        private string m_BorrowerName { get; set; }
        private Int64 m_CustomerID { get; set; }
        private string m_MasterSQLScript { get; set; }
        private bool m_IsMissingDocument { get; set; }

        private bool IsManualLookup = false;
        private bool IsEncompass = false;
        public Int64 LoanID { get { return m_LoanID; } }
        public bool IsMissingDocument { get { return m_IsMissingDocument; } }
        public Int64 LoanTypeID { get { return m_LoanTypeID; } }
        public DateTime? QCIQStartDate { get { return m_QCIQStartDate; } }
        public string LoanType { get { return m_LoanType; } }
        public string Schema { get { return m_Schema; } }
        public string ReviewTypeName { get { return m_ReviewTypeName; } }
        public string CustomerName { get { return m_CustomerName; } }
        public string QCIQConnectionString { get { return m_QCIQConnectionString; } }
        public string MasterQCIQConnectionString { get { return m_MasterQCIQConnectionString; } }
        public string QCIQSQLScript { get { return m_QCIQSQLScript; } }
        public string LoanNumber { get { return m_LoanNumber; } }
        public Int64 CustomerID { get { return m_CustomerID; } }
        public string MasterSQLScript { get { return m_MasterSQLScript; } }
        public XMLBatch Batch
        {
            get { return m_XMLBatch; }
        }

        public LookupService(XPathNavigator _inputXmlNavigator, bool isManualLookup = false, bool isEncompass = false)
        {
            InputXmlNavigator = _inputXmlNavigator;
            IsManualLookup = isManualLookup;
            IsEncompass = isEncompass;
            try
            {
                //Fill Batch detsils
                m_XMLBatch = new XMLBatch(InputXmlNavigator);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while parsing XML", ex);
            }

            //Fill Loan Specific Details
            if (isEncompass)
                FillBatchXMLDetails();
            else
                FillLoanDetails();
        }

        private string FindLoanNumber()
        {
            double lastConfident = 0;
            string number = string.Empty;
            string[] extArry = System.Configuration.ConfigurationManager.AppSettings["LoanNumberLabel"].Split('|');
            foreach (var doc in m_XMLBatch.Documents)
            {
                foreach (var item in doc.DocumentLevelFields)
                {
                    //if (Array.Exists(extArry, e => e.ToUpper() == item.Name.ToUpper()) && lastConfident <= Convert.ToDouble(item.Confidence) && !string.IsNullOrEmpty(item.Value))
                    if (Array.Exists(extArry, e => e.ToUpper() == item.Name.ToUpper()) && !string.IsNullOrEmpty(item.Value))
                    {
                        number = item.Value;
                        lastConfident = Convert.ToDouble(item.Confidence);

                        Regex r = new Regex("^[a-zA-Z0-9]*$");

                        if (!string.IsNullOrEmpty(number.Trim()) && r.IsMatch(number))
                            return number;

                        //Int64 _lNumber = 0;
                        //Int64.TryParse(number, out _lNumber);

                        //if(!_lNumber.Equals(0))
                        //    return number;

                        //if ((lastConfident == 100 || IsManualLookup) && !_lNumber.Equals(0))
                        //    return number;
                    }
                }
            }
            return number;
        }

        private string FindBorrowerName()
        {
            double lastConfident = 0;
            string bName = string.Empty;
            string[] extArry = System.Configuration.ConfigurationManager.AppSettings["BorrowerNameLabel"].Split('|');
            foreach (var doc in m_XMLBatch.Documents)
            {
                foreach (var item in doc.DocumentLevelFields)
                {
                    if (Array.Exists(extArry, e => e.ToUpper() == item.Name.ToUpper()) && lastConfident <= Convert.ToDouble(item.Confidence) && !string.IsNullOrEmpty(item.Value))
                    {
                        bName = item.Value;
                        lastConfident = Convert.ToDouble(item.Confidence);

                        if ((lastConfident == 100 || IsManualLookup) && !string.IsNullOrEmpty(bName) && !string.IsNullOrWhiteSpace(bName))
                            return bName;
                    }
                }
            }
            return bName;
        }


        public string GetXMLString()
        {
            XmlDocument doc = new XmlDocument();
            doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            this.InputXmlNavigator.MoveToRoot();
            doc.Load(InputXmlNavigator.ReadSubtree());
            return doc.OuterXml;
        }

        public void SetCustometID(DataSet masterDataSet)
        {
            System.Data.DataTable dt = GetTableFromSet(masterDataSet, "CUSTOMER");
            if (dt != null && dt.Rows.Count > 0)
            {
                m_CustomerID = Convert.ToInt64(dt.Rows[0]["customerid"]);
            }
            else
            {
                throw new Exception("Customer Not found in QCIQ");
            }
        }

        public void SetLoanType(DataSet masterDataSet, DataSet QCIQData)
        {
            if (!m_IsMissingDocument)
            {

                System.Data.DataTable dt = GetTableFromSet(QCIQData, "LOANMASTER");
                if (dt != null && dt.Rows.Count > 0)
                {
                    m_LoanTypeID = Convert.ToInt64(dt.Rows[0]["qloantypeid"]);
                }

                dt = GetTableFromSet(masterDataSet, "QPROTAXONOMY");
                DataRow[] rowType = dt.Select("LOANTYPEID =" + m_LoanTypeID.ToString());

                if (rowType != null && rowType.Length > 0)
                {
                    m_LoanType = Convert.ToString(rowType[0]["LOANTYPE"]);
                }

                dt = GetTableFromSet(masterDataSet, "ASSIGNMENTHISTORYDATA");
                if (dt != null && dt.Rows.Count > 0)
                {
                    m_QCIQStartDate = Convert.ToDateTime(dt.Rows[0]["WhenAssigned"]);
                }

                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

                try
                {
                    using (var handler = new HttpClientHandler() { })
                    using (var client = new HttpClient(handler))
                    {
                        QCIQUpdateLoanTypeRequest request = new QCIQUpdateLoanTypeRequest();
                        request.LoanID = m_LoanID;
                        request.TableSchema = m_Schema;
                        request.LoanType = m_LoanType;
                        request.QCIQStartDate = m_QCIQStartDate;
                        string cont = JsonConvert.SerializeObject(request);
                        HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateLoanTypeFromQCIQ", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                        IntellaLendResponse objres = httpres.Content.ReadAsAsync<IntellaLendResponse>().Result;
                        if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                            throw new Exception(objres.ResponseMessage.MessageDesc);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while Updating Loan Type", ex);
                }

                SetLenderInfoLoanType();
            }

        }

        private void SetLenderInfoLoanType()
        {
            if (!string.IsNullOrEmpty(m_LoanType))
            {
                string[] loanTypes = m_LoanType.Split(' ');

                m_XMLBatch.Documents.ForEach(e =>
                {
                    if (e.Type == "Lender Loan Information")
                    {
                        e.DocumentLevelFields.ForEach(f =>
                        {
                            if (f.Type == "MortgageType")
                                f.Value = loanTypes[1];

                            if (f.Type == "PurposeOfLoan")
                                f.Value = loanTypes[loanTypes.Length - 1];
                        });
                    }


                    if (e.Type == "Loan Application 1003 Format New")
                    {
                        e.DocumentLevelFields.ForEach(f =>
                        {
                            if (f.Type == "Borrower Address Type")
                                f.Value = m_LoanType.Contains("Refinance") ? "Own" : "Rent";

                            if (f.Type == "Borrower Former Address Type")
                                f.Value = m_LoanType.Contains("Refinance") ? "Own" : "Rent";
                        });
                    }
                });
            }
        }

        public void SetLoanType()
        {
            string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

            try
            {
                using (var handler = new HttpClientHandler() { })
                using (var client = new HttpClient(handler))
                {
                    EncompassUpdateLoanTypeRequest request = new EncompassUpdateLoanTypeRequest();
                    request.LoanID = m_LoanID;
                    request.TableSchema = m_Schema;
                    request.LoanNumber = m_LoanNumber;
                    request.BorrowerName = m_BorrowerName;
                    string cont = JsonConvert.SerializeObject(request);
                    HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateLoanTypeFromEncompass", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                    IntellaLendResponse objres = httpres.Content.ReadAsAsync<IntellaLendResponse>().Result;
                    if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                        throw new Exception(objres.ResponseMessage.MessageDesc);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception while Updating Loan Type", ex);
            }

        }

        private void FillLoanDetails()
        {
            try
            {
                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(m_XMLBatch.BatchName, patterns);
                m_Schema = dicObjects["SCHEMA"];
                m_LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);

                if (dicObjects.ContainsKey("DOC_ID"))
                {
                    m_IsMissingDocument = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while parsing LoanID and Schema", ex);
            }


            m_LoanNumber = FindLoanNumber();

            if (string.IsNullOrEmpty(m_LoanNumber))
                throw new Exception("Loan Number not found in  input xml");

            string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

            try
            {
                using (var handler = new HttpClientHandler() { })
                using (var client = new HttpClient(handler))
                {
                    QCIQDBDetailsRequest request = new QCIQDBDetailsRequest();
                    request.LoanID = m_LoanID;
                    request.TableSchema = m_Schema;
                    string cont = JsonConvert.SerializeObject(request);
                    HttpResponseMessage httpres = client.PostAsync(baseURL + "/GetQCIQConnectionString", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                    QCIQDBDetailsResponse objres = httpres.Content.ReadAsAsync<QCIQDBDetailsResponse>().Result;
                    if (objres.data == null)
                        throw new Exception(objres.ResponseMessage.MessageDesc);
                    LoanDeails det = JsonConvert.DeserializeObject<LoanDeails>(objres.data);
                    m_ReviewTypeName = det.ReviewTypeName;
                    m_QCIQConnectionString = det.ConnectionString;
                    m_QCIQSQLScript = det.SQLScript;
                    m_CustomerName = det.CustomerName;
                    m_MasterQCIQConnectionString = det.MasterConnectionString;
                    m_MasterSQLScript = det.MasterSQLScript;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception while getting loan data from Intellalend.", ex);
            }

        }

        private void FillBatchXMLDetails()
        {
            try
            {
                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(m_XMLBatch.BatchName, patterns);
                m_Schema = dicObjects["SCHEMA"];
                m_LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);

                if (dicObjects.ContainsKey("DOC_ID"))
                {
                    m_IsMissingDocument = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while parsing LoanID and Schema", ex);
            }


            m_LoanNumber = FindLoanNumber();

            m_BorrowerName = FindBorrowerName();

            if (string.IsNullOrEmpty(m_LoanNumber))
                throw new Exception("Loan Number not found in  input xml");
        }

        private static Dictionary<string, string> ExtractDataFromString(string inputString, string[] regexPattern)
        {
            Dictionary<string, string> returnDic = new Dictionary<string, string>();
            foreach (string pattern in regexPattern)
            {
                Regex regPattern = new Regex(pattern);
                Match match = regPattern.Match(inputString);
                if (regPattern.Match(inputString).Success)
                {
                    foreach (string group in regPattern.GetGroupNames())
                    {
                        if (group != "0")
                            returnDic[group] = match.Groups[group].Value;
                    }
                    break;
                }

            }

            return returnDic;
        }


        public void SetLookupData(System.Data.DataTable lookupMapping, DataSet QCIQData, DataSet MasterQCIQData)
        {

            bool isTestEnvironment = false;
            //if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("Environment") && System.Configuration.ConfigurationManager.AppSettings["Environment"].ToLower() == "test")
            isTestEnvironment = true;
            try
            {
                foreach (DataRow row in lookupMapping.Rows)
                {
                    string TableName = Convert.ToString(row["TABLE_NAME"]).ToUpper().Trim();
                    string ColumnName = Convert.ToString(row["COLUMN_NAME"]).ToUpper().Trim();
                    string FieldName = Convert.ToString(row["FIELD_NAME"]).ToUpper().Trim();
                    string documentName = Convert.ToString(row["DOCUMENT_NAME"]).ToUpper().Trim();
                    string condition = Convert.ToString(row["CONDITION"]).ToUpper().Trim();
                    List<Document> docList = m_XMLBatch.Documents.Where(d => d.Type.ToUpper().Trim() == documentName).ToList();
                    if (docList != null && docList.Count > 0)
                    {
                        foreach (var doc in docList)
                        {
                            List<DocumentLevelField> fieldList = doc.DocumentLevelFields.Where(f => f.Name.ToUpper().Trim() == FieldName).ToList();
                            foreach (var fld in fieldList)
                            {
                                if (fld.ForceReview || isTestEnvironment)
                                {
                                    System.Data.DataTable dt = GetTableFromSet(QCIQData, TableName);
                                    if (dt != null && dt.Columns.Contains(ColumnName) && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0][ColumnName])))
                                    {

                                        string val = string.Empty;
                                        if (!string.IsNullOrEmpty(condition))
                                        {

                                            if (condition.ToUpper().Contains("DECRPT USING CRYPTION"))
                                            {
                                                val = LookupCryption.Decrypt((byte[])dt.Rows[0][ColumnName]);
                                                //Prakash - Mail Sub : UHS IntellaLend - QCIQ Lookup Validation Changes
                                                if (string.IsNullOrEmpty(val))
                                                    continue;
                                            }
                                            else if (condition.ToUpper().Contains("REFCODE_LOOKUP"))
                                            {
                                                val = Convert.ToString(dt.Rows[0][ColumnName]);
                                                string whereCon = "refcodeid=" + val + " and " + condition.ToUpper().Replace("REFCODE_LOOKUP >", "");
                                                System.Data.DataTable refCode = GetTableFromSet(MasterQCIQData, "REFCODES");
                                                DataRow[] filtResult = refCode.Select(whereCon);
                                                if (filtResult.Length > 0)
                                                    val = Convert.ToString(filtResult[0]["refcodetext"]);
                                            }
                                            else if (condition.ToUpper().Contains("LOANTYPE_LOOKUP"))
                                            {
                                                val = Convert.ToString(dt.Rows[0][ColumnName]);
                                                string whereCon = "LOANTYPEID=" + val;
                                                System.Data.DataTable refCode = GetTableFromSet(MasterQCIQData, "QPROTAXONOMY");
                                                DataRow[] filtResult = refCode.Select(whereCon);
                                                if (filtResult.Length > 0)
                                                    val = Convert.ToString(filtResult[0]["MORTGAGE_TYPE"]);
                                            }
                                            else
                                            {
                                                val = Convert.ToString(dt.Rows[0][ColumnName]);
                                                string whereCon = condition.ToUpper();
                                                whereCon = ParseQueryString(whereCon);
                                                DataRow[] filtResult = dt.Select(whereCon);
                                                if (filtResult.Length > 0)
                                                    val = Convert.ToString(filtResult[0][ColumnName]);
                                            }
                                        }
                                        else
                                        {
                                            val = Convert.ToString(dt.Rows[0][ColumnName]);
                                        }

                                        //Prakash - Mail Sub : UHS IntellaLend - QCIQ Lookup Validation Changes
                                        if (FieldName == "Borrower Name" && val.Trim().StartsWith("0"))
                                            continue;

                                        //Prakash - Mail Sub : UHS Summary of Recent IntellaLend Open Items || Prakash - Mail Sub : QCIQ Lookup and Updating Address Fields
                                        if (FieldName == "Property Address" && (val.Trim().StartsWith("0 0") || val.Trim().Equals(string.Empty)))
                                            continue;

                                        //Prakash - Mail Sub : QCIQ Lookup and Updating Address Fields
                                        if (FieldName == "Borrower Address" && (val.Trim().StartsWith("0 0") || val.Trim().Equals(string.Empty)))
                                            continue;

                                        fld.Value = val;
                                    }
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while setting Lookup data.", ex);
            }

        }

        private string ParseQueryString(string input)
        {
            string outText = input;
            if (input.Contains("<%"))
            {
                MatchCollection tags = Regex.Matches(input, @"(\<%(.*?)%\>)");
                foreach (Match tag in tags)
                {
                    string replacement = string.Empty;
                    string token = Regex.Match(tag.ToString(), @"%([^%]*)").Groups[1].Value;
                    string[] str = token.Split(new char[] { '.' });
                    if (str.Length > 1)
                    {
                        List<Document> docList = m_XMLBatch.Documents.Where(d => d.Type.ToUpper().Trim() == str[0]).ToList();
                        if (docList != null && docList.Count > 0)
                        {
                            var FieldList = docList[0].DocumentLevelFields.Where(f => f.Name == str[1]).ToList();
                            if (FieldList != null && FieldList.Count > 0)
                            {
                                outText = outText.Replace(tag.ToString(), Convert.ToString(FieldList[0].Value).Replace("'", "''"));
                            }
                        }
                    }
                }
            }

            return outText;
        }


        private System.Data.DataTable GetTableFromSet(DataSet QCIQData, string TableName)
        {

            foreach (System.Data.DataTable dt in QCIQData.Tables)
            {
                if (dt.Rows.Count > 0 &&
                    Convert.ToString(dt.Rows[0]["TABLE_NAME"]).ToUpper().Trim() == TableName)
                    return dt;
            }

            return null;
        }
    }

    public class LoanDeails
    {
        public string ConnectionString { get; set; }
        public string MasterConnectionString { get; set; }
        public string MasterSQLScript { get; set; }
        public string SQLScript { get; set; }
        public string CustomerName { get; set; }
        public string ReviewTypeName { get; set; }
    }




}