using EphesoftService.Models;
using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService.Controllers
{
    public class FieldMappingController : ApiController
    {
        private static readonly CustomLogger logger = new CustomLogger("DocumentController");


        public string currentBatchId = string.Empty;
        QCIQLookupDataAccess dataAccess = new QCIQLookupDataAccess();

        [HttpPut]
        public HttpResponseMessage Execute()
        {
            EphesoftLookupRequest ephesoftReq = new EphesoftLookupRequest();
            try
            {
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<EphesoftLookupRequest>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }


                //Load lookup service with the xml
                LookupService lookupService = new LookupService(this.GetXMLNavigator(ephesoftReq.inputXML), ephesoftReq.isManual);

                List<string> documentList = new List<string>();
                foreach (var item in lookupService.Batch.Documents)
                {
                    documentList.Add(item.Type);
                }

                //Field Validation
                FieldValidation validation = new FieldValidation();
                validation.RemoveQuestionMarkFieldOption(lookupService.Batch);


                if (!string.IsNullOrEmpty(lookupService.QCIQConnectionString))
                {
                    //Get Lookup mapping data
                    System.Data.DataTable lookupMapping = dataAccess.GetMappingDetails(documentList, lookupService.ReviewTypeName);

                    if (lookupMapping == null || (lookupMapping != null && lookupMapping.Rows.Count == 0))
                    {
                        Exception ex = new Exception("Lookup data not found for the Review Type :" + lookupService.ReviewTypeName);
                        MTSExceptionHandler.HandleException(ref ex);
                        goto returnstatement;
                    }

                    //Get Master Data QCIQ
                    DataSet MasterQCIQData = dataAccess.GetQCIQData(lookupService.MasterQCIQConnectionString, GetMasterSqlScript(lookupService));

                    //FInd Custmer ID from Master DB
                    lookupService.SetCustometID(MasterQCIQData);

                    //Get Data for particular loan from QCIQ
                    DataSet QCIQData = dataAccess.GetQCIQData(lookupService.QCIQConnectionString, GetSqlScript(lookupService));

                    if (QCIQData != null)
                    {
                        bool loanMasterExists = false;

                        foreach (System.Data.DataTable dt in QCIQData.Tables)
                        {
                            if (dt.Rows.Count > 0 && dt.Columns.Contains("TABLE_NAME") && Convert.ToString(dt.Rows[0]["TABLE_NAME"]).ToLower() == "loanmaster")
                            {
                                loanMasterExists = true;
                                break;
                            }
                        }

                        if (!loanMasterExists)
                        {
                            Exception ex = new Exception("Loan Data not found in QCIQ");
                            MTSExceptionHandler.HandleException(ref ex);
                            goto returnstatement;
                        }
                    }

                    //Set Lookup Data
                    lookupService.SetLookupData(lookupMapping, QCIQData, MasterQCIQData);

                    //Set Loan Type from QCIQ
                    lookupService.SetLoanType(MasterQCIQData, QCIQData);



                }

                returnstatement:
                return this.CreateSuccessResponse(lookupService.GetXMLString());
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex, ephesoftReq.inputXML);
            }
        }

        [HttpPut]
        public HttpResponseMessage SetEncompassLoanType()
        {
            EphesoftLookupRequest ephesoftReq = new EphesoftLookupRequest();
            try
            {
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<EphesoftLookupRequest>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }

                //Load lookup service with the xml
                LookupService lookupService = new LookupService(this.GetXMLNavigator(ephesoftReq.inputXML), ephesoftReq.isManual, true);

                //Field Validation
                FieldValidation validation = new FieldValidation();
                validation.RemoveQuestionMarkFieldOption(lookupService.Batch);

                lookupService.SetLoanType();

                return this.CreateSuccessResponse(lookupService.GetXMLString());
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex, ephesoftReq.inputXML);
            }
        }

        [HttpPut]
        public HttpResponseMessage SetLoanType()
        {
            EphesoftLookupRequest ephesoftReq = new EphesoftLookupRequest();
            try
            {
                // Read the contents of the request
                string requestJson = this.Request.Content.ReadAsStringAsync().Result;
                try
                {

                    ephesoftReq = JsonConvert.DeserializeObject<EphesoftLookupRequest>(requestJson, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });

                    logger.Debug("RequestJson" + requestJson);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while parrsing input JSON \nInput JSON:" + requestJson, ex);
                }


                //Load lookup service with the xml
                LookupService lookupService = new LookupService(this.GetXMLNavigator(ephesoftReq.inputXML), ephesoftReq.isManual);

                List<string> documentList = new List<string>();
                foreach (var item in lookupService.Batch.Documents)
                {
                    documentList.Add(item.Type);
                }

                //Field Validation
                FieldValidation validation = new FieldValidation();
                validation.RemoveQuestionMarkFieldOption(lookupService.Batch);


                if (!string.IsNullOrEmpty(lookupService.QCIQConnectionString))
                {

                    //Get Master Data QCIQ
                    DataSet MasterQCIQData = dataAccess.GetQCIQData(lookupService.MasterQCIQConnectionString, GetMasterSqlScript(lookupService));

                    //FInd Custmer ID from Master DB
                    lookupService.SetCustometID(MasterQCIQData);

                    //Get Data for particular loan from QCIQ
                    DataSet QCIQData = dataAccess.GetQCIQData(lookupService.QCIQConnectionString, GetSqlScript(lookupService));

                    if (QCIQData != null)
                    {
                        bool loanMasterExists = false;

                        foreach (System.Data.DataTable dt in QCIQData.Tables)
                        {
                            if (dt.Rows.Count > 0 && dt.Columns.Contains("TABLE_NAME") && Convert.ToString(dt.Rows[0]["TABLE_NAME"]).ToLower() == "loanmaster")
                            {
                                loanMasterExists = true;
                                break;
                            }
                        }

                        if (!loanMasterExists)
                        {
                            throw new Exception("Loan Data not found in QCIQ");
                        }
                    }

                    //Set Loan Type from QCIQ
                    lookupService.SetLoanType(MasterQCIQData, QCIQData);
                }

                return this.CreateSuccessResponse(lookupService.GetXMLString());
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex, ephesoftReq.inputXML);
            }
        }



        private string GetSqlScript(LookupService lookupService)
        {
            string sqlScript = lookupService.QCIQSQLScript;
            sqlScript = sqlScript.Replace("<<CUSTOMER_ID>>", lookupService.CustomerID.ToString());
            sqlScript = sqlScript.Replace("<<LOAN_NUMBER>>", lookupService.LoanNumber.Replace("'", "''"));
            return sqlScript;
        }

        private string GetMasterSqlScript(LookupService lookupService)
        {
            string sqlScript = lookupService.MasterSQLScript;
            sqlScript = sqlScript.Replace("<<CUSTOMER_NAME>>", lookupService.CustomerName.Replace("'", "''"));
            return sqlScript;
        }



        private HttpResponseMessage CreateSuccessResponse(string result)
        {
            // Create the response object and add the XML to the content
            HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(result, Encoding.UTF8, "application/xml");
            return response;
        }

        private HttpResponseMessage CreateExceptionResponse(Exception ex, string inputXML)
        {
            HttpResponseMessage response = this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            response.Content = new StringContent(inputXML, Encoding.UTF8, "application/xml");
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

    }

}