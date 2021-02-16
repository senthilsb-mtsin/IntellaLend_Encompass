using EphesoftService.Models;
using MTSEntBlocks.ExceptionBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.XPath;

namespace EphesoftService.Controllers
{
    public class AutoValidationController : ApiController
    {
        private static readonly CustomLogger logger = new CustomLogger("DocumentController");


        public string currentBatchId = string.Empty;
        AutoValidationDataAccess dataAccess = new AutoValidationDataAccess();

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
                XPathNavigator _inputXmlNavigator = this.GetXMLNavigator(ephesoftReq.inputXML);

                XMLBatch m_XMLBatch = new XMLBatch(_inputXmlNavigator);

                int configId = this.GetConfigId(_inputXmlNavigator);

                System.Data.DataTable skipDocuments = dataAccess.GetDocumentsToSkip(configId);

                int documentOrder = 0;
                foreach (var document in m_XMLBatch.Documents)
                {
                    if (!IsSkipDocument(skipDocuments, document.Type, documentOrder, m_XMLBatch))
                    {
                        document.Reviewed = true;
                        document.Valid = true;
                        foreach (var field in document.DocumentLevelFields)
                        {
                            if (field.ForceReview)
                                field.ForceReview = false;
                        }
                    }

                    documentOrder++;
                }

                bool isAllValid = true;
                foreach (var document in m_XMLBatch.Documents)
                {
                    if (document.Valid == false)
                    {
                        isAllValid = false;
                        break;
                    }
                }

                if (isAllValid && m_XMLBatch.Documents.Count > 0)
                {
                    m_XMLBatch.Documents[0].Valid = false;
                    m_XMLBatch.Documents[0].Reviewed = false;
                }

                return this.CreateSuccessResponse(GetXMLString(_inputXmlNavigator));
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex, ephesoftReq.inputXML);
            }
        }

        
       

        [HttpPut]
        public HttpResponseMessage FieldValidate()
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
                XPathNavigator _inputXmlNavigator = this.GetXMLNavigator(ephesoftReq.inputXML);

                XMLBatch m_XMLBatch = new XMLBatch(_inputXmlNavigator);

                foreach (var document in m_XMLBatch.Documents.Where(x => x.m_Type == "Credit Report").ToList())
                {
                    DocumentLevelField _borrowerField = document.DocumentLevelFields.Where(x => x.m_Name == "Co-Borrower SSN").FirstOrDefault();
                    if(_borrowerField != null)
                    {
                        if (string.IsNullOrEmpty(_borrowerField.Value.Trim()))
                        {
                            foreach (var field in document.DocumentLevelFields)
                            {
                                if (field.Name == "EFX Score Co" || field.Name == "TU Score Co" || field.Name == "XPN Score Co")
                                {
                                    field.Value = string.Empty;
                                }
                            }
                        }
                    }

                }
                return this.CreateSuccessResponse(GetXMLString(_inputXmlNavigator));
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return this.CreateExceptionResponse(ex, ephesoftReq.inputXML);
            }
        }

        private bool IsSkipDocument(System.Data.DataTable skipDocuments, string documentName, int documentOrder, XMLBatch xMLBatch)
        {
            bool isSkip = false;
            string skipOrder = string.Empty;
            foreach (DataRow row in skipDocuments.Rows)
            {
                if (Convert.ToString(row["DOCUMENT_NAME"]).ToLower() == documentName.ToLower())
                {
                    isSkip = true;
                    skipOrder = Convert.ToString(row["INSTANCE"]).ToLower();
                }

            }

            if (isSkip)
            {
                switch (skipOrder)
                {
                    case "first":
                        {
                            List<int> orders = GetDocumentOrders(documentName, xMLBatch);
                            if (documentOrder != orders.First())
                                isSkip = false;
                            break;
                        }
                    case "last":
                        {
                            List<int> orders = GetDocumentOrders(documentName, xMLBatch);
                            if (documentOrder != orders.Last())
                                isSkip = false;
                            break;
                        }
                    case "first and last":
                        {
                            List<int> orders = GetDocumentOrders(documentName, xMLBatch);
                            if (documentOrder != orders.Last() && documentOrder != orders.First())
                                isSkip = false;
                            break;
                        }
                    case "middle":
                        {
                            List<int> orders = GetDocumentOrders(documentName, xMLBatch);
                            if (documentOrder == orders.Last() || documentOrder == orders.First())
                                isSkip = false;
                            break;
                        }
                }
            }

            return isSkip;
        }


        private List<int> GetDocumentOrders(string documentName, XMLBatch xMLBatch)
        {
            List<int> orders = new List<int>();
            int documentOrder = 0;
            foreach (var document in xMLBatch.Documents)
            {
                if (document.Type.ToLower() == documentName.ToLower())
                {
                    orders.Add(documentOrder);
                }
                documentOrder++;
            }

            return orders;
        }

        private string GetXMLString(XPathNavigator inputXmlNavigator)
        {
            XmlDocument doc = new XmlDocument();
            doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            inputXmlNavigator.MoveToRoot();
            doc.Load(inputXmlNavigator.ReadSubtree());
            return doc.OuterXml;
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


        /// <summary>
        /// This method gets the batch class identifier for the input batch XML and retrieves the configId 
        /// for the batch class Id from the MTS.IDC_RULE_CONFIG table. 
        /// </summary>
        /// <param name="inputXmlNavigator">The XPathNavigator instance of the input Ephesoft batch XML</param>
        /// <returns></returns>
        private int GetConfigId(XPathNavigator inputXmlNavigator)
        {
            logger.Debug("Batch Id:" + this.currentBatchId + " - Get ConfigId from the DB the batch class of the input XML.");

            int configId = 0;
            // Get the batch class identifier for the input batch XML.
            string xmlBatchClasssId = UtilFunctions.GetBatchClassIdForEphesoftXML(inputXmlNavigator);
            if (String.IsNullOrEmpty(xmlBatchClasssId))
            {
                logger.Error("BatchClassID not retrieved from input XML.");
                throw new MTSPassThruException("BatchClassId not retrieved from input XML");
            }
            else
            {
                // Get the configId from the database for the batch class Id
                configId = dataAccess.GetCongidId(xmlBatchClasssId);
            }

            logger.Info("Batch Id:" + this.currentBatchId + " - Retrieved configId from DB for the input XML - " + configId);
            return configId;
        }

    }

}