using AddressParserLib;
using EphesoftService.Models;
using IntellaLend.Constance;
using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService.Controllers
{
    public class IntellaLendWrapperController : ApiController
    {
        #region Private Variables

        private MTSEphesoftServiceDBAccess dbAccess = new MTSEphesoftServiceDBAccess();
        private DocumentRulesProcess docRulesProcess = new DocumentRulesProcess();
        private static readonly CustomLogger logger = new CustomLogger("IntellaLnedWrapperController");

        #endregion

        #region Public Methods

        [HttpPut]
        public HttpResponseMessage UpdateLOSExportFileStaging()
        {
            MASEphesoftRequest ephesoftReq = new MASEphesoftRequest();
            IntellaLendResponse objres = new IntellaLendResponse();
            try
            {
                Logger.WriteTraceLog("In UpdateLOSExportFileStaging()");

                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {
                    ephesoftReq = JsonConvert.DeserializeObject<MASEphesoftRequest>(requestJson);
                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(ephesoftReq.inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                string Schema = dicObjects["SCHEMA"];
                Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);
                Logger.WriteTraceLog($"LoanID : {LoanID}, Schema : {Schema}, xmlBatch.BatchInstanceIdentifier : {xmlBatch.BatchInstanceIdentifier}");

                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];
                Logger.WriteTraceLog($"baseURL : {baseURL}");

                Dictionary<int, int> fileType = new Dictionary<int, int>();
                fileType[3] = LOSExportFileTypeConstant.LOS_CLASSIFICATION_EXCEPTION;
                fileType[5] = LOSExportFileTypeConstant.LOS_CLASSIFICATION_RESULTS;
                fileType[6] = LOSExportFileTypeConstant.LOS_VALIDATION_EXCEPTION;

                try
                {
                    List<MASDocument> masDocList = null;
                    if (ephesoftReq.ephesoftModule == 5)
                    {
                        masDocList = new List<MASDocument>();
                        foreach (var document in xmlBatch.Documents)
                        {
                            MASDocument masDoc = new MASDocument();
                            masDoc.DocumentID = document.Identifier;
                            masDoc.DocumentType = document.Type;
                            masDoc.DocumentDesc = document.Description;
                            masDocList.Add(masDoc);
                        }
                    }
                    bool triggerMASFile = true;
                    if (ephesoftReq.ephesoftModule == 3)
                    {
                        //List<Document> doc = xmlBatch.Documents.FindAll(d => d.Reviewed == false);
                        //triggerMASFile = doc.Count > 0;
                        triggerMASFile = false;

                    }
                    if (ephesoftReq.ephesoftModule == 6)
                    {
                        //List<Document> doc = xmlBatch.Documents.FindAll(d => d.Valid == false);
                        //triggerMASFile = doc.Count > 0;
                        triggerMASFile = false;
                    }

                    if (triggerMASFile)
                    {
                        using (var handler = new HttpClientHandler() { })
                        using (var client = new HttpClient(handler))
                        {
                            EphsoftLOSExportFileStagingRequest request = new EphsoftLOSExportFileStagingRequest();
                            request.LoanID = LoanID;
                            request.FileType = fileType[ephesoftReq.ephesoftModule];
                            request.TableSchema = Schema;
                            request.MASDocumentList = masDocList;
                            request.BatchID = xmlBatch.BatchInstanceIdentifier;
                            request.BatchName = xmlBatch.BatchName;
                            request.BCName = xmlBatch.BatchClassName;
                            string cont = JsonConvert.SerializeObject(request);
                            HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateLOSExportFileStaging", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                            objres = httpres.Content.ReadAsAsync<IntellaLendResponse>().Result;
                            Logger.WriteTraceLog($"objres :{JsonConvert.SerializeObject(objres)}");
                            if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                                throw new Exception(objres.ResponseMessage.MessageDesc);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteTraceLog($"Error :{ex.Message}");
                    throw new Exception("Exception while Inserting LOSFileExportStaging", ex);
                }

                Logger.WriteTraceLog($"End UpdateLOSExportFileStaging()");
                Logger.WriteTraceLog($"{objres.ResponseMessage.ToString()}");

                return CreateSuccessResponse(ephesoftReq.inputXML);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage GetLoanDetails()
        {
            IntellaLnedWrapper ephesoftReq = new IntellaLnedWrapper();
            EphesoftLoanDetailsResponse objres = new EphesoftLoanDetailsResponse();
            try
            {
                Logger.WriteTraceLog("In GetLoanDetails()");
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<IntellaLnedWrapper>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(ephesoftReq.inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                string Schema = dicObjects["SCHEMA"];
                Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);
                Logger.WriteTraceLog($"LoanID : {LoanID}, Schema : {Schema}, xmlBatch.BatchInstanceIdentifier : {xmlBatch.BatchInstanceIdentifier}");
                Int64 DocID = 0;
                if (dicObjects.ContainsKey("DOC_ID"))
                    Int64.TryParse(dicObjects["DOC_ID"], out DocID);


                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];
                Logger.WriteTraceLog($"baseURL : {baseURL}");
                try
                {
                    using (var handler = new HttpClientHandler() { })
                    using (var client = new HttpClient(handler))
                    {
                        EphesoftLoanDetailsRequest request = new EphesoftLoanDetailsRequest();
                        request.LoanID = LoanID;
                        request.DocID = DocID;
                        request.TableSchema = Schema;
                        request.BatchID = xmlBatch.BatchInstanceIdentifier;
                        request.BatchClassID = xmlBatch.BatchClassIdentifier;
                        request.BatchClassName = xmlBatch.BatchClassName;
                        string cont = JsonConvert.SerializeObject(request);
                        HttpResponseMessage httpres = client.PostAsync(baseURL + "/GetLoanDetails", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                        objres = httpres.Content.ReadAsAsync<EphesoftLoanDetailsResponse>().Result;
                        Logger.WriteTraceLog($"objres :{JsonConvert.SerializeObject(objres)}");
                        if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                            throw new Exception(objres.ResponseMessage.MessageDesc);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteTraceLog($"Error :{ex.Message}");
                    throw new Exception("Exception while Updating Loan Type", ex);
                }
                Logger.WriteTraceLog($"End GetLoanDetails()");
                Logger.WriteTraceLog($"{objres.LoanDetailsJson}");
                return CreateSuccessResponse(objres.LoanDetailsJson);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage CheckLoanPageCount()
        {
            IntellaLnedWrapper ephesoftReq = new IntellaLnedWrapper();
            EphesoftLoanDetailsResponse objres = new EphesoftLoanDetailsResponse();
            try
            {

                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<IntellaLnedWrapper>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(ephesoftReq.inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                string Schema = dicObjects["SCHEMA"];
                Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);

                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

                try
                {
                    using (var handler = new HttpClientHandler() { })
                    using (var client = new HttpClient(handler))
                    {
                        EphesoftLoanPageCountRequest request = new EphesoftLoanPageCountRequest();
                        request.LoanID = LoanID;
                        request.TableSchema = Schema;
                        request.PageCount = xmlBatch.PageCount;
                        string cont = JsonConvert.SerializeObject(request);
                        HttpResponseMessage httpres = client.PostAsync(baseURL + "/CheckLoanPageCount", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                        objres = httpres.Content.ReadAsAsync<EphesoftLoanDetailsResponse>().Result;
                        if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                            throw new Exception(objres.ResponseMessage.MessageDesc);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while Updating Loan Type", ex);
                }


                return CreateSuccessResponse(objres.LoanDetailsJson);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }


        [HttpPut]
        public HttpResponseMessage GetPropertyAddressDetails()
        {
            IntellaLnedWrapper ephesoftReq = new IntellaLnedWrapper();
            EphesoftLoanDetailsResponse objres = new EphesoftLoanDetailsResponse();
            try
            {

                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<IntellaLnedWrapper>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }
                string result = string.Empty;
                if (!string.IsNullOrEmpty(ephesoftReq.inputXML.Trim()))
                {
                    AddressParseResult add = new AddressParser().ParseAddress(ephesoftReq.inputXML);

                    if (add != null)
                    {
                        result = JsonConvert.SerializeObject(new { PropertyStreet = add.StreetLine, PropertyCity = add.City, PropertyState = add.State, PropertyZip = add.Zip });
                    }
                }
                return CreateSuccessResponse(result);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
        }



        [HttpPut]
        public HttpResponseMessage UpdateEphesoftValidatorName()
        {
            IntellaLnedWrapper ephesoftReq = new IntellaLnedWrapper();
            EphesoftLoanDetailsResponse objres = new EphesoftLoanDetailsResponse();
            try
            {

                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<IntellaLnedWrapper>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                XMLBatch xmlBatch = new XMLBatch(GetXMLNavigator(ephesoftReq.inputXML));

                //string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                string[] patterns = { @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)_(?<DOC_ID>\d+)-(?<EXT>[^>]*?)-", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)", @"(?<SCHEMA>[a-zA-Z0-9]+?)_(?<LOAN_ID>\d+)-(?<EXT>[^>]*?)-" };
                Dictionary<string, string> dicObjects = ExtractDataFromString(xmlBatch.BatchName, patterns);
                string Schema = dicObjects["SCHEMA"];
                Int64 LoanID = Convert.ToInt64(dicObjects["LOAN_ID"]);


                string baseURL = System.Configuration.ConfigurationManager.AppSettings["IntellaLendInterface"];

                try
                {
                    using (var handler = new HttpClientHandler() { })
                    using (var client = new HttpClient(handler))
                    {
                        EphesoftLoanDetailsRequest request = new EphesoftLoanDetailsRequest();
                        request.LoanID = LoanID;
                        request.TableSchema = Schema;
                        request.BatchID = xmlBatch.BatchInstanceIdentifier;
                        string cont = JsonConvert.SerializeObject(request);
                        HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateEphesoftValidatorName", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                        objres = httpres.Content.ReadAsAsync<EphesoftLoanDetailsResponse>().Result;
                        if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                            throw new Exception(objres.ResponseMessage.MessageDesc);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Exception while Updating Loan Type", ex);
                }


                return CreateSuccessResponse(objres.LoanDetailsJson);

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex);
            }
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


        private HttpResponseMessage CreateSuccessResponse(string result)
        {
            // Create the response object and add the XML to the content
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(result, Encoding.UTF8, "application/json");
            return response;
        }

        private HttpResponseMessage CreateExceptionResponse(Exception ex)
        {
            HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            response.Content = new StringContent(string.Empty);
            return response;
        }

        /// <summary>
        /// This method processes the Json contents of the request and converts it to the request object. 
        /// </summary>
        /// <param name="requestJson">Json string containing the request</param>
        /// <returns></returns>
        private XPathNavigator GetXMLNavigator(string inputXML)
        {
            XPathNavigator inputXmlNavigator = null;
            try
            {
                string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                XmlDocument inputXmlDocument = new XmlDocument();
                inputXmlDocument.LoadXml(inputXML);
                inputXmlNavigator = inputXmlDocument.CreateNavigator();
            }
            catch (MTSPassThruException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                Exception customEx = new MTSPassThruException("Error Parsing Input XML :" + inputXML, ex);
                throw customEx;
            }
            return inputXmlNavigator;
        }




        #endregion
    }
}