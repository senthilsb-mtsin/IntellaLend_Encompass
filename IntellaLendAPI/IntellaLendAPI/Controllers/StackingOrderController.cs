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
    public class StackingOrderController : ApiController
    {
        [HttpPost]
        public TokenResponse SearchStackingOrder(RequestSearchStackingOrder reqsearchstackorder)
        {
            Logger.WriteTraceLog($"Start SearchStackingOrder()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqsearchstackorder)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(reqsearchstackorder.TableSchema).SearchStackingOrder(reqsearchstackorder.StackingOrderID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SearchStackingOrder()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveStackingOrderDetails(RequestSaveSearchStackingOrder savestackingorder)
        {
            Logger.WriteTraceLog($"Start SaveStackingOrderDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(savestackingorder)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(savestackingorder.TableSchema).SaveStackingOrderDetails(savestackingorder.StackOrderID, savestackingorder.StackOrder));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveStackingOrderDetails()");
            return response;
        }

        [HttpGet]
        public TokenResponse GetAllSystemStackingOrderDetails()
        {
            Logger.WriteTraceLog($"Start GetAllSystemStackingOrderDetails()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetAllSystemStackingOrderDetails());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);

            }
            Logger.WriteTraceLog($"End GetAllSystemStackingOrderDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetSystemDocumentTypes(RequestSystemDocTypes reqSysDocTypes)
        {
            Logger.WriteTraceLog($"Start GetSystemDocumentTypes()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqSysDocTypes)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetSystemDocumentTypesWithDocFields(reqSysDocTypes.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSystemDocumentTypes()");
            return response;
        }



        [HttpGet]
        public TokenResponse GetStackSystemDocumentTypes()
        {
            Logger.WriteTraceLog($"Start GetStackSystemDocumentTypes()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetStackSystemDocumentTypes());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetStackSystemDocumentTypes()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetLoanStackDocs(RequestSystemDocTypes req)
        {
            Logger.WriteTraceLog($"Start GetLoanStackDocs()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetLoanStackDocs(req.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanStackDocs()");
            return response;
        }
        [HttpPost]
        public TokenResponse SetOrderByField(RequestSetOrderByField req)
        {
            Logger.WriteTraceLog($"Start SetOrderByField()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().SetOrderByField(req.DocumentTypeID, req.FieldID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SetOrderByField()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetDocFieldValue(RequestSetTenantOrderByField req)
        {
            Logger.WriteTraceLog($"Start SetDocFieldValue()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(req.TableSchema).SetDocFieldValue(req.DocumentTypeID, req.FieldID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SetDocFieldValue()");
            return response;
        }
        [HttpPost]
        public TokenResponse SetTenantOrderByField(RequestSetTenantOrderByField req)
        {
            Logger.WriteTraceLog($"Start SetTenantOrderByField()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(req.TableSchema).SetTenantOrderByField(req.DocumentTypeID, req.FieldID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SetTenantOrderByField()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetTenantDocFielValue(RequestSetTenantOrderByField req)
        {
            Logger.WriteTraceLog($"Start SetTenantDocFielValue()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(req.TableSchema).SetTenantDocFielValue(req.DocumentTypeID, req.FieldID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SetTenantDocFielValue()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetSystemStackingOrderDetailMaster(RequestSystemStackingOrderDetailMaster reqSysStackingOrder)
        {
            Logger.WriteTraceLog($"Start GetSystemStackingOrderDetailMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqSysStackingOrder)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetSystemStackingOrderDetailMaster(reqSysStackingOrder.stackingOrderID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSystemStackingOrderDetailMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveSystemStackingOrderDetails(RequestSaveSearchStackingOrder savestackingorder)
        {
            Logger.WriteTraceLog($"Start SaveSystemStackingOrderDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(savestackingorder)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().SaveSystemStackingOrderDetails(savestackingorder.StackOrderID, savestackingorder.StackOrderName, savestackingorder.IsActive, savestackingorder.StackOrder));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveSystemStackingOrderDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetDocGroupFieldValue(RequestSaveStackingOrderGroupField request)
        {
            Logger.WriteTraceLog($"Start SetDocGroupFieldValue()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(request.TableSchema).SetDocGroupFieldValue(request.StackOrder, request.StackingOrderDetails));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetDocGroupFieldValue()");
            return response;
        }
        [HttpPost]
        public TokenResponse SetTenantDocGroupFieldValue(RequestSaveStackingOrderGroupField request)
        {
            Logger.WriteTraceLog($"Start SetTenantDocGroupFieldValue()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService(request.TableSchema).SetTenantDocGroupFieldValue(request.StackOrder, request.StackingOrderDetails));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SetTenantDocGroupFieldValue()");
            return response;
        }
    }
}