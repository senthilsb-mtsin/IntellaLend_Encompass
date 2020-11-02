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
    public class CheckListItemController : ApiController
    {
        [HttpPost]
        public TokenResponse CheckListDetails(AddCheckListDetails addchecklistdetails)
        {
            Logger.WriteTraceLog($"Start CheckListDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(addchecklistdetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService(addchecklistdetails.TableSchema).CheckListDetails(addchecklistdetails.CheckListDetailMaster, addchecklistdetails.rulemasters));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckListDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse SearchCheckListItem(RequestCheckListItems reqchecklistsitem)
        {
            Logger.WriteTraceLog($"Start SearchCheckListItem()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqchecklistsitem)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService(reqchecklistsitem.TableSchema).SearchCheckListItem(reqchecklistsitem.CheckListDetailID));

            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SearchCheckListItem()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateCheckListDetails(AddCheckListDetails updateCheckListDetails)
        {
            Logger.WriteTraceLog($"Start UpdateCheckListDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(updateCheckListDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService(updateCheckListDetails.TableSchema).UpdateCheckListDetails(updateCheckListDetails.CheckListDetailMaster, updateCheckListDetails.rulemasters));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateCheckListDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse DeleteCheckListItem(RequestDeleteCheckListItem deletechecklistitem)
        {
            Logger.WriteTraceLog($"Start DeleteCheckListItem()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(deletechecklistitem)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService(deletechecklistitem.TableSchema).DeleteCheckListItem(deletechecklistitem.CheckListDetailsID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DeleteCheckListItem()");
            return response;
        }

        [HttpPost]
        public TokenResponse CloneCheckListItem(RequestCloneCheckListItem clonechecklistitem)
        {
            Logger.WriteTraceLog($"Start CloneCheckListItem()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(clonechecklistitem)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService(clonechecklistitem.TableSchema).CloneCheckListItem(clonechecklistitem.CheckListDetailsID, clonechecklistitem.ModifiedCheckListName));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CloneCheckListItem()");
            return response;
        }

        [HttpPost]
        public TokenResponse CloneSysCheckListItem(RequestCloneCheckListItem clonechecklistitem)
        {
            Logger.WriteTraceLog($"Start CloneSysCheckListItem()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(clonechecklistitem)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().CloneSysCheckListItem(clonechecklistitem.CheckListDetailsID, clonechecklistitem.ModifiedCheckListName,clonechecklistitem.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End CloneSysCheckListItem()");
            return response;
        }

        [HttpPost]
        public TokenResponse TestCheckListItemValues(RequestCheckListItemsValues testvalues)
        {
            Logger.WriteTraceLog($"Start TestCheckListItemValues()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(testvalues)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().TestCheckListItemValues(testvalues.CheckListItemValues, testvalues.RuleFormula));
            }
            catch(Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End TestCheckListItemValues()");
            return response;
        }

        #region IL Methods

        [HttpGet]
        public TokenResponse GetAllSysCheckListDetails()
        {
            Logger.WriteTraceLog($"Start GetAllSysCheckListDetails()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().GetAllSysCheckListDetails());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllSysCheckListDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse AddSysCheckList(RequestSysCheckListMasters reqSysChkListMaster)
        {
            Logger.WriteTraceLog($"Start AddSysCheckList()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(reqSysChkListMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().AddSysCheckList(reqSysChkListMaster.checkListMaster));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddSysCheckList()");
            return response;
        }

        [HttpGet]
        public TokenResponse GetAllSysDocTypeMasters()
        {
            Logger.WriteTraceLog($"Start GetAllSysDocTypeMasters()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().GetAllSysDocTypeMasters());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllSysDocTypeMasters()");
            return response;

        }

        [HttpPost]
        public TokenResponse GetSysDocumentFieldMasters(RequestGetDocumentFieldMaster reqSysFieldMasters)
        {
            Logger.WriteTraceLog($"Start GetSysDocumentFieldMasters()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(reqSysFieldMasters)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().GetSysDocumentFieldMasters(reqSysFieldMasters.DocumentTypeID));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSysDocumentFieldMasters()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveSysCheckListDetails(AddCheckListDetails addSysCheckListDetails)
        {
            Logger.WriteTraceLog($"Start SaveSysCheckListDetails()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(addSysCheckListDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().SaveSysCheckListDetails(addSysCheckListDetails.CheckListDetailMaster, addSysCheckListDetails.rulemasters, addSysCheckListDetails.LoanTypeID));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveSysCheckListDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveSysEditCheckListDetails(AddCSysheckListDetails addSysCheckListDetails)
        {
            Logger.WriteTraceLog($"Start SaveSysEditCheckListDetails()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(addSysCheckListDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().SaveSysEditCheckListDetails(addSysCheckListDetails.CheckListDetailMaster, addSysCheckListDetails.rulemasters,addSysCheckListDetails.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveSysEditCheckListDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetEditSysDocTypeMasters(RequestCheckListItems reqCheckListItemDoc)
        {
            Logger.WriteTraceLog($"Start GetEditSysDocTypeMasters()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(reqCheckListItemDoc)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().GetEditSysDocTypeMasters(reqCheckListItemDoc.CheckListDetailID, reqCheckListItemDoc.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEditSysDocTypeMasters()");
            return response;

        }

        [HttpPost]
        public TokenResponse UpdateSysCheckListDetails(AddCheckListDetails updateCheckListDetails)
        {
            Logger.WriteTraceLog($"Start UpdateSysCheckListDetails()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(updateCheckListDetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().UpdateSysCheckListDetails(updateCheckListDetails.CheckListDetailMaster, updateCheckListDetails.rulemasters,updateCheckListDetails.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateSysCheckListDetails()");
            return response;

        }

        [HttpPost]
        public TokenResponse GetAllCheckListItems(GetCheckListRequest checklistReq)
        {
            Logger.WriteTraceLog($"Start GetAllCheckListItems()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(checklistReq)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().GetAllCheckListItems(checklistReq.CheckListID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllCheckListItems()");
            return response;
        }
        [HttpPost]
        public TokenResponse DeleteSysCheckListItem(RequestDeleteCheckListItem deletechecklistitem)
        {
            Logger.WriteTraceLog($"Start DeleteSysCheckListItem()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(deletechecklistitem)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().DeleteSysCheckListItem(deletechecklistitem.CheckListDetailsID,deletechecklistitem.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End DeleteSysCheckListItem()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveSysCheckListName(AddSysCheckList addSysCheckList)
        {
            Logger.WriteTraceLog($"Start SaveSysCheckListName()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(addSysCheckList)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().SaveSysCheckListName(addSysCheckList.CheckListName, addSysCheckList.CheckListID, addSysCheckList.Active,addSysCheckList.Sync));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveSysCheckListName()");
            return response;
        }


        [HttpPost]
        public TokenResponse AssignChecklist(AssignSysCheckList req)
        {
            Logger.WriteTraceLog($"Start AssignChecklist()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().AssignChecklist(req.LoanTypeID, req.CheckListName, req.CheckListID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AssignChecklist()");
            return response;
        }

        [HttpPost]
        public TokenResponse AssignStackingOrder(AssignSysStackingOrder req)
        {
            Logger.WriteTraceLog($"Start AssignStackingOrder()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().AssignStackingOrder(req.LoanTypeID, req.StackingOrderName, req.StackingOrderID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End AssignStackingOrder()");
            return response;
        }
        
        [HttpPost]
        public TokenResponse GetCategoryGroups(GetCheckListGroups req)
        {
            Logger.WriteTraceLog($"Start GetCategoryGroups()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().GetChecklistCategories());
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetCategoryGroups()");
            return response;
        }

        [HttpPost]
        public TokenResponse SaveTenantCheckListDetails (AssignSysCheckList request)
        {
            Logger.WriteTraceLog($"Start SaveTenantCheckListDetails()");
            Logger.WriteTraceLog($"Request Body:{JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new CheckListItemService().SaveTenantCheckListDetails(request.CheckListID, request.LoanTypeID));
            }
            catch (Exception exc)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exc.Message;
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End SaveTenantCheckListDetails()");
            return response;
        }

        #endregion
    }



}