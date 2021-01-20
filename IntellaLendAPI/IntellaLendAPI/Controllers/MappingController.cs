using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MappingController : ApiController
    {
        [HttpPost]
        public TokenResponse CustReviewTypeMapping(RequestCustLoanMapping custLoanMap)
        {
            Logger.WriteTraceLog($"Start CustReviewTypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(custLoanMap)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data= new JWTToken().CreateJWTToken(new MappingService(custLoanMap.TableSchema).CustReviewTypeMapping(custLoanMap.CustomerID));
            }
            catch(Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CustReviewTypeMapping()");
            return response;

        }

        [HttpPost]
        public TokenResponse CustReviewLoanMapping(RequestCustLoanReviewMapping custloanreviewmap)
        {
            Logger.WriteTraceLog($"Start CustReviewLoanMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(custloanreviewmap)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(custloanreviewmap.TableSchema).CustReviewLoanMapping(custloanreviewmap.CustomerID, custloanreviewmap.ReviewTypeID));
            }
            catch(Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CustReviewLoanMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveCustReviewMapping(RequestCustLoanReviewMapping req)
        {
            Logger.WriteTraceLog($"Start SaveCustReviewMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).SaveCustReviewMapping(req.CustomerID, req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveCustReviewMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveCustReviewLoanMapping(RequestCustLoanReviewCheckListMapping req)
        {
            Logger.WriteTraceLog($"Start SaveCustReviewLoanMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).SaveCustReviewLoanMapping(req.CustomerID, req.ReviewTypeID, req.LoanTypeID,req.BoxUploadPath,req.LoanUploadPath));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveCustReviewLoanMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustLoantypeMapping(RequestCustLoanMapping req)
        {
            Logger.WriteTraceLog($"Start GetCustLoantypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).GetCustLoantypeMapping(req.CustomerID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustLoantypeMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveCustLoanTypeMapping(CustLoantypeMappingUpdateRequest req)
        {
            Logger.WriteTraceLog($"Start SaveCustLoanTypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).SaveCustLoantypeMapping(req.lsCustLoanTypeMappings));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveCustLoanTypeMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveCustLoanUploadPath(RequestCustLoanReviewCheckListMapping req)
        {
            Logger.WriteTraceLog($"Start SaveCustLoanUploadPath()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).SaveCustLoanUploadPath(req.CustomerID, req.ReviewTypeID, req.LoanTypeID,req.BoxUploadPath, req.LoanUploadPath,req.isRetainUpdate));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveCustLoanUploadPath()");
            return response;
        }

        [HttpPost]
        public TokenResponse CheckCustLoanUploadPath(RequestCheckLoanPath req)
        {
            Logger.WriteTraceLog($"Start CheckCustLoanUploadPath()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).CheckCustLoanUploadPath(req.CustomerID, req.ReviewTypeID, req.LoanTypeID, req.LoanUploadPath));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CheckCustLoanUploadPath()");
            return response;
        }




        [HttpPost]
        public TokenResponse GetCustReviewLoanCheckList(RequestCustLoanReviewCheckListMapping req)
        {
            Logger.WriteTraceLog($"Start GetCustReviewLoanCheckList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).GetCustReviewLoanCheckList(req.CustomerID, req.ReviewTypeID, req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustReviewLoanCheckList()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetSyncLoanDetails(SyncCustomerLoanTypeRequest request)
        {
            Logger.WriteTraceLog($"Start GetSyncLoanDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(request.TableSchema).GetSyncLoanDetails(request.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSyncLoanDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse SyncCustomerLoanType(SyncCustomerLoanTypeRequest request)
        {
            Logger.WriteTraceLog($"Start SyncCustomerLoanType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(request.TableSchema).SyncCustomerLoanType(request.LoanTypeID, request.UserID,request.SyncLevel));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SyncCustomerLoanType()");
            return response;
        }

        [HttpPost]
        public TokenResponse RetainCustReviewMapping(RequestCustLoanReviewMapping req)
        {
            Logger.WriteTraceLog($"Start RetainCustReviewMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).RetainCustReviewMapping(req.CustomerID, req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RetainCustReviewMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse RetainCustReviewLoanMapping(RequestCustLoanReviewStackMapping req)
        {
            Logger.WriteTraceLog($"Start RetainCustReviewLoanMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).RetainCustReviewLoanMapping(req.CustomerID, req.ReviewTypeID, req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RetainCustReviewLoanMapping()");
            return response;
        }
        

        [HttpPost]
        public TokenResponse CheckCustReviewMapping(RequestCustLoanReviewMapping req)
        {
            Logger.WriteTraceLog($"Start CheckCustReviewMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).CheckCustReviewMapping(req.CustomerID, req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CheckCustReviewMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse CheckCustReviewLoanMapping(RequestCustLoanReviewCheckListMapping req)
        {
            Logger.WriteTraceLog($"Start CheckCustReviewLoanMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).CheckCustReviewLoanMapping(req.CustomerID, req.ReviewTypeID, req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CheckCustReviewLoanMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse CustReviewLoanStackMapping(RequestCustLoanReviewStackMapping reqstackmapping)
        {
            Logger.WriteTraceLog($"Start CustReviewLoanStackMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqstackmapping)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(reqstackmapping.TableSchema).CustReviewLoanStackMapping(reqstackmapping.CustomerID, reqstackmapping.LoanTypeID, reqstackmapping.ReviewTypeID));

            } catch (Exception exc) {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CustReviewLoanStackMapping()");
            return response;
        }
        [HttpPost]
        public TokenResponse CustReviewLoanCheckListMapping(RequestCustLoanReviewCheckListMapping custloanreviewcheclistkmap)
        {
            Logger.WriteTraceLog($"Start CustReviewLoanCheckListMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(custloanreviewcheclistkmap)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(custloanreviewcheclistkmap.TableSchema).CustReviewLoanCheckListMapping(custloanreviewcheclistkmap.CustomerID, custloanreviewcheclistkmap.ReviewTypeID, custloanreviewcheclistkmap.LoanTypeID));
            }
            catch(Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CustReviewLoanCheckListMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse CustLoanDocTypeMapping(RequestCustLoanReviewDocTypeMapping docTypeMapping)
        {
            Logger.WriteTraceLog($"Start CustLoanDocTypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(docTypeMapping)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(docTypeMapping.TableSchema).CustLoanDocTypeMapping(docTypeMapping.CustomerID, docTypeMapping.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CustLoanDocTypeMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustLoanDocTypeMapping(RequestCustLoanDocMapping docTypeMapping)
        {
            Logger.WriteTraceLog($"Start GetCustLoanDocTypeMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(docTypeMapping)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(docTypeMapping.TableSchema).GetCustLoanDocTypeMapping(docTypeMapping.LoanTypeID, docTypeMapping.DocumentTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustLoanDocTypeMapping()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetDocumentFieldMasters(RequestGetFieldMaster documentfieldtype)
        {
            Logger.WriteTraceLog($"Start GetDocumentFieldMasters()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(documentfieldtype)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(documentfieldtype.TableSchema).GetDocumentFieldMasters(documentfieldtype.DocumentTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetDocumentFieldMasters()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetDocumentTypesBasedonLoanType(RequestGetDocFieldMaster documentfieldtype)
        {
            Logger.WriteTraceLog($"Start GetDocumentTypesBasedonLoanType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(documentfieldtype)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(documentfieldtype.TableSchema).GetDocumentTypesBasedonLoanType());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetDocumentTypesBasedonLoanType()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetLoanTypeForCustomer(DocumentFieldsRequest docfieldrequest)
        {
            Logger.WriteTraceLog($"Start GetLoanTypeForCustomer()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(docfieldrequest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(docfieldrequest.TableSchema).GetLoanTypeForCustomer(docfieldrequest.CustomerID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanTypeForCustomer()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetSystemReviewTypes(RequestCustLoanMapping custLoanTypeReq)
        {
            Logger.WriteTraceLog($"Start GetSystemReviewTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(custLoanTypeReq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(custLoanTypeReq.TableSchema).GetSystemReviewTypes(custLoanTypeReq.CustomerID));
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
        public TokenResponse GetCustReviewTypes(RequestCustLoanMapping req)
        {
            Logger.WriteTraceLog($"Start GetCustReviewTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).GetCustReviewTypes(req.CustomerID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustReviewTypes()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetCustReviewLoanTypes(CustReviewLoanTypeRequest CLR)
        {
            Logger.WriteTraceLog($"Start GetCustReviewLoanTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(CLR)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(CLR.TableSchema).GetCustReviewLoanTypes(CLR.CustomerID, CLR.ReviewTypeID,CLR.isSaveEdit));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCustReviewLoanTypes()");
            return response;
        }

        [HttpPost]
        public TokenResponse CloneFromSystem(CloneFromSystemRequest CSR)
        {
            Logger.WriteTraceLog($"Start CloneFromSystem()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(CSR)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(CSR.TableSchema).CloneFromSystem(CSR.CustomerID, CSR.ReviewTypeID, CSR.LoanTypeIDs));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CloneFromSystem()");
            return response;
        }

        [HttpPost]
        public TokenResponse RemoveCustReviewMapping(CustReviewLoanTypeRequest req)
        {
            Logger.WriteTraceLog($"Start RemoveCustReviewMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).RemoveCustReviewMapping(req.CustomerID, req.ReviewTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RemoveCustReviewMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse RemoveCustReviewLoanMapping(RequestCustLoanReviewStackMapping req)
        {
            Logger.WriteTraceLog($"Start RemoveCustReviewLoanMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).RemoveCustReviewLoanMapping(req.CustomerID, req.ReviewTypeID, req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RemoveCustReviewLoanMapping()");
            return response;
        }

        [HttpPost]
        public TokenResponse RemoveCustConfigUploadPath(RequestCustLoanReviewStackMapping req)
        {
            Logger.WriteTraceLog($"Start RemoveCustConfigUploadPath()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).RemoveCustConfigUploadPath(req.CustomerID, req.ReviewTypeID, req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End RemoveCustConfigUploadPath()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetReviewLoanLenderMapped(RequestReviewLoanLenderMapping req)
        {
            Logger.WriteTraceLog($"Start GetReviewLoanLenderMapped()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).GetReviewLoanLenderMapped(req.ReviewTypeID,req.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetReviewLoanLenderMapped()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveReviewLoanLenderMapping(RequestSaveReviewLoanLenderMapping req)
        {
            Logger.WriteTraceLog($"Start SaveReviewLoanLenderMapping()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MappingService(req.TableSchema).SaveReviewLoanLenderMapping(req.ReviewTypeID, req.LoanTypeID,req.AllLendersIDs,req.AssignedLendersIDs,req.IsAdd));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveReviewLoanLenderMapping()");
            return response;
        }


    }
}