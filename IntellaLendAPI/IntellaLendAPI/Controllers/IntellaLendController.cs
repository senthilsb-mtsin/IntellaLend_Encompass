using IntellaLend.CommonServices;
using IntellaLend.Model;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IntellaLendController : ApiController
    {
        private static string[] HeaderParams = { "TableSchema", "UploadFileName", "ReverificationName", "LoanTypeID", "TemplateID", "ReverificationID", "MappingID", "Active" };
        private static string filePath = ConfigurationManager.AppSettings["filePath"].ToString();
        [HttpGet]
        public TokenResponse GetSystemReviewTypes()
        {
            Logger.WriteTraceLog($"Start GetSystemReviewTypes()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetSystemReviewTypes());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetSystemReviewTypes()");
            return response;

        }

        [HttpPost]
        public TokenResponse GetSystemLoanTypes(RequestSystemLoanTypes req)
        {
            Logger.WriteTraceLog($"Start GetSystemLoanTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetSystemLoanTypes(req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetSystemLoanTypes()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAssignedSystemLoanTypes(RequestAssignedSystemLoanTypes req)
        {
            Logger.WriteTraceLog($"Start GetAssignedSystemLoanTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetAssignedSystemLoanTypes(req.DocumentTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetAssignedSystemLoanTypes()");
            return response;

        }

        [HttpPost]
        public TokenResponse SetLoanDocTypeMapping(RequestSetSystemDocTypes req)
        {
            Logger.WriteTraceLog($"Start SetLoanDocTypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetLoanDocTypeMapping(req.LoanTypeID, req.DocMappingDetails));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetLoanDocTypeMapping()");
            return response;
        }

        [HttpGet]
        public TokenResponse GetSystemLoanTypes()
        {
            Logger.WriteTraceLog($"Start GetSystemLoanTypes()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetSystemLoanTypes());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetSystemLoanTypes()");
            return response;

        }

        [HttpPost]
        public TokenResponse GetSystemDocuments(RequestSystemDocTypes req)
        {
            Logger.WriteTraceLog($"Start GetSystemDocuments()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetSystemDocuments(req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetSystemDocuments()");
            return response;

        }
      
        [HttpPost]
        public TokenResponse SaveConditionGeneralRule(RequestConditionRule req)
        {
            Logger.WriteTraceLog($"Start SaveConditionGeneralRule()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).SaveConditionGeneralRule(req.DocumentTypeID,req.GeneralRuleValues,req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveConditionGeneralRule()");
            return response;

        }

        [HttpPost]
        public TokenResponse GetLoanTypes(RequestMapping req)
        {
            Logger.WriteTraceLog($"Start GetLoanTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetLoanTypes(req.ReviewTypeID, req.IsAdd));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLoanTypes()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCheckList(Requestchecklist req)
        {
            Logger.WriteTraceLog($"Start GetCheckList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();//GetCheckList
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetCheckList(req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCheckList()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLosSystemDocumentTypes(LosType _losType)
        {
            Logger.WriteTraceLog($"Start GetLosSystemDocumentTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(_losType)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetLosSystemDocumentTypesWithDocFields(_losType.LosName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetLosSystemDocumentTypes()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetAssignedDocumentFields(LosType _losType)
        {
            Logger.WriteTraceLog($"Start GetAssignedDocumentFields()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(_losType)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetLosSystemDocFields(_losType.LosName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End GetAssignedDocumentFields()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetStackingOrder(Requestchecklist req)
        {
            Logger.WriteTraceLog($"Start GetStackingOrder()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();//GetCheckList
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetStackingOrder(req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetStackingOrder()");
            return response;
        }



        [HttpPost]
        public TokenResponse SetLoanCheckMapping(RequestLoanMapping req)
        {
            Logger.WriteTraceLog($"Start SetLoanCheckMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetLoanCheckMapping(req.LoanTypeID, req.CheckListID, req.ChecklistItemSeq));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetLoanCheckMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetLoanStackMapping(RequestLoanMapping req)
        {
            Logger.WriteTraceLog($"Start SetLoanStackMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetLoanStackMapping(req.LoanTypeID, req.StackingOrderID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetLoanStackMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetSystemStackCheck(RequestMapping req)
        {
            Logger.WriteTraceLog($"Start GetSystemStackCheck()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetSystemStackCheck(req.ReviewTypeID, req.LoanTypeID, req.IsAdd));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetSystemStackCheck()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveMapping(RequestMapping req)
        {
            Logger.WriteTraceLog($"Start SaveMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SaveMapping(req.ReviewTypeID, req.LoanTypeID, req.CheckListID, req.StackingOrderID, req.IsAdd));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveMapping()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetReviewLoanMapped(RequestSystemLoanTypes req)
        {
            Logger.WriteTraceLog($"Start GetReviewLoanMapped()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetReviewLoanMapped(req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetReviewLoanMapped()");
            return response;
        }


        [HttpPost]
        public TokenResponse SetReviewLoanMapping(RequestSaveSystemReviewLoan req)
        {
            Logger.WriteTraceLog($"Start SetReviewLoanMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetReviewLoanMapping(req.ReviewTypeID, req.LoanTypeIDs));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetReviewLoanMapping()");
            return response;
        }


        #region Re-Verification

        [HttpGet]
        public TokenResponse GetReverificationTemplate()
        {
            Logger.WriteTraceLog($"Start GetReverificationTemplate()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetReverificationTemplate());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetReverificationTemplate()");
            return response;
        }


        [HttpGet]
        public TokenResponse GetReverificationMaster()
        {
            Logger.WriteTraceLog($"Start GetReverificationMaster()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetReverificationMaster());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetReverificationMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustReverificationMaster(RequestReverification req)
        {
            Logger.WriteTraceLog($"Start GetCustReverificationMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).GetCustReverificationMaster());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustReverificationMaster()");
            return response;
        }


        //[HttpPost]
        //public async Task<TokenResponse> SaveReverification()
        //{
        //    TokenResponse response = new TokenResponse();
        //    response.ResponseMessage = new ResponseMessage();
        //    try
        //    {
        //        var provider = new MultipartMemoryStreamProvider();
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        Dictionary<string, string> paramsValues = GetHeaderValue(Request.Headers);
        //        if (paramsValues.Count.Equals(8))
        //        {
        //            response.token = new JWTToken().CreateJWTToken();
        //            response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SaveReverification(paramsValues, filePath,provider));
        //        }
        //        else
        //            throw new Exception("Header Parameter Mismatch");
        //    }
        //    catch (Exception exc)
        //    {
        //        response.token = null;
        //        response.ResponseMessage.MessageDesc = exc.Message;
        //        MTSExceptionHandler.HandleException(ref exc);
        //    }
        //    return response;
        //}
        [HttpPost]
        public async Task<TokenResponse> ReverificationFileUploader()
        {

            Logger.WriteTraceLog($"Start ReverificationFileUploader()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Dictionary<string, string> paramsValues = GetHeaderValue(Request.Headers);
                if (paramsValues.Count.Equals(3))
                {
                    response.token = new JWTToken().CreateJWTToken();
                    response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(paramsValues["TableSchema"]).ReverificationFileUploader(paramsValues, filePath, provider));
                }
                else
                    throw new Exception("Header Parameter Mismatch");
            }
            catch (Exception Exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = Exc.Message;
                MTSExceptionHandler.HandleException(ref Exc);
            }
            Logger.WriteTraceLog($"End ReverificationFileUploader()");
            return response;
        }

        [HttpPost]
        public async Task<TokenResponse> CustReverificationFileUploader()
        {

            Logger.WriteTraceLog($"Start CustReverificationFileUploader()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                Dictionary<string, string> paramsValues = GetHeaderValue(Request.Headers);
                if (paramsValues.Count.Equals(3))
                {
                    response.token = new JWTToken().CreateJWTToken();
                    response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(paramsValues["TableSchema"]).CustReverificationFileUploader(paramsValues, filePath, provider));
                }
                else
                    throw new Exception("Header Parameter Mismatch");
            }
            catch (Exception Exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = Exc.Message;
                MTSExceptionHandler.HandleException(ref Exc);
            }
            Logger.WriteTraceLog($"End CustReverificationFileUploader()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveReverification(RequestSaveReverification req)
        {
            Logger.WriteTraceLog($"Start SaveReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SaveReverification(req.LoanTypeID, req.ReverificationName, req.TemplateID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveReverification()");
            return response;
        }
        [HttpGet]
        public TokenResponse GetAllSMPTDetails()
        {
            Logger.WriteTraceLog($"Start GetAllSMPTDetails()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetAllSMPTDetails());
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;

                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllSMPTDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveAllSMTPDetails(SMTPDetailsRequest SMTPMaster)
        {
            Logger.WriteTraceLog($"Start SaveAllSMTPDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(SMTPMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SaveAllSMTPDetails(SMTPMaster.SMTPMaster));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveAllSMTPDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateReverification(RequestUpdateReverification req)
        {

            Logger.WriteTraceLog($"Start UpdateReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().UpdateReverification(req.LoanTypeID, req.ReverificationName, req.TemplateID, req.MappingID, req.ReverificationID, req.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateReverification()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateCustReverification(RequestUpdateReverification req)
        {
            Logger.WriteTraceLog($"Start UpdateCustReverification()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).UpdateCustReverification(req.CustomerID, req.LoanTypeID, req.ReverificationName, req.TemplateID, req.MappingID, req.ReverificationID, req.Active));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateCustReverification()");
            return response;
        }



        [HttpPost]
        public TokenResponse GetReverificationMappedDoc(RequestReverificationMappedDoc req)
        {
            Logger.WriteTraceLog($"Start GetReverificationMappedDoc()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetReverificationMappedDoc(req.LoanTypeID, req.ReverificationID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetReverificationMappedDoc()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustReverificationMappedDoc(RequestReverificationMappedDoc req)
        {
            Logger.WriteTraceLog($"Start GetCustReverificationMappedDoc()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).GetCustReverificationMappedDoc(req.CustomerID, req.LoanTypeID, req.ReverificationID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustReverificationMappedDoc()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetReverifyDocMapping(RequestSetReverifyDocTypes req)
        {
            Logger.WriteTraceLog($"Start SetReverifyDocMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetReverifyDocMapping(req.ReverificationID, req.DocTypeIDs));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetReverifyDocMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetCustReverifyDocMapping(RequestSetReverifyDocTypes req)
        {
            Logger.WriteTraceLog($"Start SetCustReverifyDocMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).SetCustReverifyDocMapping(req.CustomerID, req.ReverificationID, req.DocTypeIDs));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetCustReverifyDocMapping()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetMappedTemplate(RequesGetReverifyTemplate req)
        {
            Logger.WriteTraceLog($"Start GetMappedTemplate()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();

                if (string.IsNullOrEmpty(req.TableSchema))
                    response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetMappedTemplate(req.TemplateID, req.MappingID, req.ReverificationID));
                else
                    response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).GetCustMappedTemplate(req.TemplateID, req.MappingID, req.ReverificationID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetMappedTemplate()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetFieldsByDocName(RequesGetFields req)
        {
            Logger.WriteTraceLog($"Start GetFieldsByDocName()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetDocFieldsByName(req.DocumentName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetFieldsByDocName()");
            return response;
        }

        //[HttpPost]
        //public TokenResponse CheckReverificationExist(ReverificationDupRequest req)
        //{
        //    TokenResponse response = new TokenResponse();
        //    response.ResponseMessage = new ResponseMessage();
        //    try
        //    {
        //        response.token = new JWTToken().CreateJWTToken();
        //        response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().CheckReverificationExist(req.ReverificationName));
        //    }
        //    catch (Exception exc)
        //    {
        //        response.token = null;
        //        response.ResponseMessage.MessageDesc = exc.Message;
        //        MTSExceptionHandler.HandleException(ref exc);
        //    }
        //    return response;
        //}
        [HttpPost]
        public TokenResponse CheckReverificationExistForEdit(ReverificationDupRequest req)
        {
            Logger.WriteTraceLog($"Start CheckReverificationExistForEdit()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().CheckReverificationExistForEdit(req.ReverificationName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CheckReverificationExistForEdit()");
            return response;
        }
        [HttpPost]
        public TokenResponse UpdateMappingTemplateFields(RequesTemplateFields req)
        {
            Logger.WriteTraceLog($"Start UpdateMappingTemplateFields()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                if (string.IsNullOrEmpty(req.TableSchema))
                    response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().UpdateMappingTemplateFields(req.MappingID, req.TemplateFieldJson));
                else
                    response.data = new JWTToken().CreateJWTToken(new IntellaLendServices(req.TableSchema).UpdateCustMappingTemplateFields(req.MappingID, req.TemplateFieldJson));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End UpdateMappingTemplateFields()");
            return response;
        }
        [HttpPost]
        public TokenResponse SetDocLoanTypeMapping(RequestSaveSystemDocumentLoan req)
        {
            Logger.WriteTraceLog($"Start SetDocLoanTypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetDocLoanTypeMapping(req.DocumentTypeID, req.LoanTypeIDs));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetDocLoanTypeMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeleteDocumentField(RequestDeleteDocumentField req)
        {
            Logger.WriteTraceLog($"Start DeleteDocumentField()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().DeleteDocumentField(req.FieldID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DeleteDocumentField()");
            return response;
        }


        [HttpPost]
        public TokenResponse AddDocumentField(RequestAddDocumentField req)
        {
            Logger.WriteTraceLog($"Start AddDocumentField()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().AddDocumentField(req.DocumentTypeID, req.FieldName, req.FieldDisplayName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AddDocumentField()");
            return response;
        }
        #endregion

        #region EncompassFields
        [HttpPost]
        public TokenResponse GetLOSDocFields(GetLOSFields req)
        {
            Logger.WriteTraceLog($"Start GetLOSDocFields()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();

                List<EncompassDocFields> docFields = System.Web.HttpContext.Current.Application["EncompassDocFields"] as List<EncompassDocFields>;
                string searchVal = req.SearchCriteria.ToLower();
                List<EncompassDocFields> losFields = docFields.Where(d => d.FieldIDDescription.ToLower().Contains(searchVal)).ToList();
                response.data = new JWTToken().CreateJWTToken(losFields);
                //List<EncompassDocFields> docFields = System.Web.HttpContext.Current.Application["EncompassDocFields"] as List<EncompassDocFields>;
                //string searchVal = req.SearchCriteria.ToLower();

                //List<EncompassDocFields> losFields = docFields.Where(d => d.FieldId.ToLower().Contains(searchVal)).ToList();

                //if (losFields.Count == 0)
                //{
                //    string SearchCriteria = new IntellaLendServices().GetLosValues(req.SearchCriteria.ToLower());

                //    List<EncompassDocFields> LosFields = docFields.Where(d => d.FieldIDDescription.Contains(SearchCriteria)).ToList();

                //    response.data = new JWTToken().CreateJWTToken(LosFields);
                //}
                //else
                //{
                //    response.data = new JWTToken().CreateJWTToken(losFields);

                //}


            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLOSDocFields()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetLOSDocFieldValues(LOSType req)
        {
            Logger.WriteTraceLog($"Start SetLOSDocFieldValues()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SetLOSDocFieldValues(req.LosType));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetLOSDocFieldValues()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLOSLoanTapeDocFields(GetLOSFields req)
        {
            Logger.WriteTraceLog($"Start GetLOSLoanTapeDocFields()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();


                List<LoanTapeDocFields> docFields = System.Web.HttpContext.Current.Application["LoanTapeDocFields"] as List<LoanTapeDocFields>;
                string searchVal = req.SearchCriteria.ToLower();
                List<LoanTapeDocFields> losFields = docFields.Where(d => d.FieldIDDescription.ToLower().Contains(searchVal)).ToList();
                response.data = new JWTToken().CreateJWTToken(losFields);

            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetLOSLoanTapeDocFields()");
            return response;
        }


        #endregion
        private Dictionary<string, string> GetHeaderValue(HttpRequestHeaders Headers)
        {
            Dictionary<string, string> paramsValue = new Dictionary<string, string>();

            foreach (string header in HeaderParams)
            {
                if (Headers.Contains(header))
                    paramsValue.Add(header, Headers.GetValues(header).FirstOrDefault().ToString());
            }

            return paramsValue;
        }


        [HttpGet]
        public TokenResponse GetPasswordPolicy()
        {
            Logger.WriteTraceLog($"Start GetPasswordPolicy()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().GetPasswordPolicy());
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;

                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetPasswordPolicy()");
            return response;
        }
        [HttpPost]
        public TokenResponse SavePasswordPolicy(RequestPasswordPolicy _policyRequest)
        {
            Logger.WriteTraceLog($"Start SavePasswordPolicy()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(_policyRequest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new IntellaLendServices().SavePasswordPolicy(_policyRequest.passwordPolicy));
            }

            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;

                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SavePasswordPolicy()");
            return response;
        }
    }
}