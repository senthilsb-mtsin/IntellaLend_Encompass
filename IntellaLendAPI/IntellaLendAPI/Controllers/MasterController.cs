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
    public class MasterController : ApiController
    {

        #region RoleMaster

        [HttpPost]
        public TokenResponse GetRoleMaster(CommonListRequest roleMaster)
        {
            Logger.WriteTraceLog($"Start GetRoleMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(roleMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(roleMaster.TableSchema).GetRoleMaster());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetRoleMaster()");
            return response; 
        }
        [HttpPost]
        public TokenResponse GetAllRoleMasterList(CommonListRequest roleMaster)
        {
            Logger.WriteTraceLog($"Start GetAllRoleMasterList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(roleMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(roleMaster.TableSchema).GetAllRoleMasterList());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllRoleMasterList()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllADGroupMasterList(CommonListRequest roleMaster)
        {
            Logger.WriteTraceLog($"Start GetAllADGroupMasterList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(roleMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(roleMaster.TableSchema).GetAllADGroupMasterList());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllRoleMasterList()");
            return response;
        }
        [HttpPost]
        public TokenResponse AddRoleDetails(RoleRequest request)
        {
            Logger.WriteTraceLog($"Start AddRoleDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddRoleDetails(request.roletype,request.menus));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddRoleDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse UpdateRoleDetails(RoleRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateRoleDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateRoleDetails(request.roletype,request.menus));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateRoleDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse ChecUserRoleDetails(RoleMenuAccessList request)
        {
            Logger.WriteTraceLog($"Start ChecUserRoleDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).ChecUserRoleDetails(request.RoleID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End ChecUserRoleDetails()");
            return response;
        }
        #endregion

        #region CustomerList
        [HttpPost]
        public TokenResponse GetCustomerList(CommonListRequest customerMaster)
        {
            Logger.WriteTraceLog($"Start GetCustomerList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(customerMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(customerMaster.TableSchema).GetCustomerList());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetCustomerList()");
            return response;
        }

        #endregion

        #region SecurityQuestionList
        [HttpPost]
        public TokenResponse GetSecurityQuestionList(CommonListRequest QuestionTable)
        {
            Logger.WriteTraceLog($"Start GetSecurityQuestionList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(QuestionTable)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(QuestionTable.TableSchema).GetSecurityQuestionList());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSecurityQuestionList()");
            return response;
        }

        #endregion

        #region LoanTypeMaster
        [HttpPost]
        public TokenResponse GetLoanTypeMaster(CommonListRequest loanTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetLoanTypeMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loanTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(loanTypeMaster.TableSchema).GetLoanTypeMaster(true));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanTypeMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllLoanTypeMaster(CommonListRequest loanTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetAllLoanTypeMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(loanTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(loanTypeMaster.TableSchema).GetLoanTypeMaster(false));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllLoanTypeMaster()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetLoanType(GetLoanTypeRequest reuest)
        {
            Logger.WriteTraceLog($"Start GetLoanType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reuest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(reuest.TableSchema).GetLoanType(reuest.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetLoanType()");
            return response;
        }

        [HttpPost]
        public TokenResponse AddLoanType(SystemLoanTypeRequest request)
        {
            Logger.WriteTraceLog($"Start AddLoanType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddLoanType(request.LoanType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddLoanType()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateLoanType(UpdateLoanTypeRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateLoanType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateLoanType(request.LoanType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateLoanType()");
            return response;
        }


        #endregion

        #region CheckList

        [HttpPost]
        public TokenResponse GetCheckListMaster(CommonListRequest CheckListMaster)
        {
            Logger.WriteTraceLog($"Start GetCheckListMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(CheckListMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(CheckListMaster.TableSchema).GetCheckListMaster(true));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetCheckListMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllCheckListMaster(CommonListRequest CheckListMaster)
        {
            Logger.WriteTraceLog($"Start GetAllCheckListMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(CheckListMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(CheckListMaster.TableSchema).GetCheckListMaster(false));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllCheckListMaster()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetCheckList(GetCheckListRequest reuest)
        {
            Logger.WriteTraceLog($"Start GetCheckList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reuest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(reuest.TableSchema).GetCheckList(reuest.CheckListID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetCheckList()");
            return response;
        }

        [HttpPost]
        public TokenResponse AddCheckList(UpdateCheckListRequest request)
        {
            Logger.WriteTraceLog($"Start AddCheckList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddCheckList(request.CheckList));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddCheckList()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateCheckList(UpdateCheckListRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateCheckList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateCheckList(request.CheckList));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateCheckList()");
            return response;
        }

        #endregion

        #region ReviewTypeMaste
        [HttpPost]
        public TokenResponse GetReviewTypeMaster(CommonListRequest reviewTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetReviewTypeMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reviewTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(reviewTypeMaster.TableSchema).GetReviewTypeMaster(true));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetReviewTypeMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetReviewTypeList(string TableSchema)
        {
            Logger.WriteTraceLog($"Start GetReviewTypeList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(TableSchema)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(TableSchema).GetReviewTypeMaster(true));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetReviewTypeList()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllReviewTypeMaster(CommonListRequest ReviewTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetAllReviewTypeMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(ReviewTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(ReviewTypeMaster.TableSchema).GetReviewTypeMaster(false));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllReviewTypeMaster()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetReviewType(GetReviewTypeRequest reuest)
        {
            Logger.WriteTraceLog($"Start GetReviewType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reuest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(reuest.TableSchema).GetReviewType(reuest.ReviewTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetReviewType()");
            return response;
        }

        [HttpPost]
        public TokenResponse AddReviewType(SystenReviewTypeRequest request)
        {
            Logger.WriteTraceLog($"Start AddReviewType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddReviewType(request.ReviewType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddReviewType()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateReviewType(UpdateReviewTypeRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateReviewType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateReviewType(request.ReviewType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateReviewType()");
            return response;
        }

        #endregion

        #region StackingOrder

        [HttpPost]
        public TokenResponse GetStackingOrderMaster(CommonListRequest StackingOrderMaster)
        {
            Logger.WriteTraceLog($"Start GetStackingOrderMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(StackingOrderMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(StackingOrderMaster.TableSchema).GetStackingOrderMaster(true));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetStackingOrderMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetAllStackingOrderMaster(CommonListRequest StackingOrderMaster)
        {
            Logger.WriteTraceLog($"Start GetAllStackingOrderMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(StackingOrderMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(StackingOrderMaster.TableSchema).GetStackingOrderMaster(false));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllStackingOrderMaster()");
            return response;
        }


        [HttpPost]
        public TokenResponse GetStackingOrder(GetStackingOrderRequest reuest)
        {
            Logger.WriteTraceLog($"Start GetStackingOrder()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reuest)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(reuest.TableSchema).GetStackingOrder(reuest.StackingOrderID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetStackingOrder()");
            return response;
        }

        [HttpPost]
        public TokenResponse AddStackingOrder(UpdateStackingOrderRequest request)
        {
            Logger.WriteTraceLog($"Start AddStackingOrder()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddStackingOrder(request.StackingOrder));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddStackingOrder()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateStackingOrder(UpdateStackingOrderRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateStackingOrder()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateStackingOrder(request.StackingOrder));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateStackingOrder()");
            return response;
        }

        #endregion

        #region WorkFlowMaster
        [HttpGet]
        public TokenResponse GetWorkFlowMaster()
        {
            Logger.WriteTraceLog($"Start GetWorkFlowMaster()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().GetWorkFlowMaster());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetWorkFlowMaster()");
            return response;
        }

        [HttpGet]
        public TokenResponse GetSearchWorkFlowSatus()
        {
            Logger.WriteTraceLog($"Start GetSearchWorkFlowSatus()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().GetSearchWorkFlowSatus());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSearchWorkFlowSatus()");
            return response;
        }

        [HttpGet]
        public TokenResponse GetRetentionWorkFlowStatus()
        {
            Logger.WriteTraceLog($"Start GetRetentionWorkFlowStatus()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().GetRetentionWorkFlowStatus());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetRetentionWorkFlowStatus()");
            return response;
        }
        #endregion

        #region DocumentTypeMaster

        [HttpPost]
        public TokenResponse GetDocumentTypeMaster(CommonListRequest docTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetDocumentTypeMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(docTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(docTypeMaster.TableSchema).GetDocumentTypeMaster(false));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetDocumentTypeMaster()");
            return response;
        }
        [HttpPost]
        public TokenResponse CheckDocumentDupForEdit(DocumentTypeDupRequest request)
        {
            Logger.WriteTraceLog($"Start CheckDocumentDupForEdit()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().CheckDocumentDupForEdit(request.DocumentTypeName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckDocumentDupForEdit()");
            return response;
        }
        [HttpPost]
        public TokenResponse CheckDocumentDup(DocumentTypeDupRequest request)
        {
            Logger.WriteTraceLog($"Start CheckDocumentDup()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().CheckDocumentDup(request.DocumentTypeName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckDocumentDup()");
            return response;
        }
     
        [HttpGet]
        public TokenResponse GetSystemDocumentTypeMaster()
        {
            Logger.WriteTraceLog($"Start GetSystemDocumentTypeMaster()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetSystemDocumentTypes());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSystemDocumentTypeMaster()");
            return response;
        }
        [HttpPost]
        public TokenResponse CheckManagerDocumentDupForEdit(DocumentTypeDupRequest request)
        {
            Logger.WriteTraceLog($"Start CheckManagerDocumentDupForEdit()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().CheckManagerDocumentDupForEdit(request.DocumentTypeName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckManagerDocumentDupForEdit()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetActiveDocumentTypeMaster(CommonListRequest docTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetActiveDocumentTypeMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(docTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(docTypeMaster.TableSchema).GetActiveDocumentTypeMaster());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetActiveDocumentTypeMaster()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetActiveDocumentTypeMasterWithCustandLoan(RequestCustLoanReviewDocTypeMapping docTypeMaster)
        {
            Logger.WriteTraceLog($"Start GetActiveDocumentTypeMasterWithCustandLoan()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(docTypeMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(docTypeMaster.TableSchema).GetActiveDocumentTypeMasterWithCustandLoan(docTypeMaster.CustomerID,docTypeMaster.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetActiveDocumentTypeMasterWithCustandLoan()");
            return response;
        }


        [HttpPost]
        public TokenResponse AddDocumentType(UpdateDocumentTypeRequest request)
        {
            Logger.WriteTraceLog($"Start AddDocumentType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddDocumentType(request.documentType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddDocumentType()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetEncompassParkingSpot(RequestGetDocFieldMaster req)
        {
            Logger.WriteTraceLog($"Start GetEncompassParkingSpot()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().GetEncompassParkingSpot());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetEncompassParkingSpot()");
            return response;
        }

        
        
        [HttpPost]
        public TokenResponse AddNewDocumentType(AddDocumentTypeRequest request)
        {
            Logger.WriteTraceLog($"Start AddNewDocumentType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService().AddDocumentType(request.DocumentTypeName, request.DocumentDisplayName,Convert.ToInt32(request.DocumentLevel), Convert.ToInt32(request.ParkingSpotID)));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddNewDocumentType()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateDocumentType(UpdateDocumentTypeRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateDocumentType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateDocumentType(request.documentType,request.ParkingSpotID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateDocumentType()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateManagerDocumentType(UpdateDocumentManagerTypeRequest request)
        {
            Logger.WriteTraceLog($"Start UpdateManagerDocumentType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateManagerDocumentType(request.documentType,request.CustomerID,request.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateManagerDocumentType()");
            return response;
        }

        [HttpPost]
        public TokenResponse AddManagerDocumentType(UpdateDocumentManagerTypeRequest request)
        {
            Logger.WriteTraceLog($"Start AddManagerDocumentType()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).AddManagerDocumentType(request.documentType, request.CustomerID, request.LoanTypeID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddManagerDocumentType()");
            return response;
        }
        

        [HttpPost]
        public TokenResponse GetReviewPriorityMaster(CommonListRequest customerMaster)
        {
            Logger.WriteTraceLog($"Start GetReviewPriorityMaster()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(customerMaster)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(customerMaster.TableSchema).GetReviewPriorityMaster());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetReviewPriorityMaster()");
            return response;
        }


        #endregion

        #region Document Field Master

        [HttpPost]
        public TokenResponse UpdateDocumentField(DocumentFieldUpdateRequest req)
        {
            Logger.WriteTraceLog($"Start UpdateDocumentField()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(req)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(req.TableSchema).UpdateDocumentField(req.Field, req.AssignedFieldID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateDocumentField()");
            return response;
        }        

        [HttpPost]
        public TokenResponse GetUserMasters(CommonListRequest clr)
        {
            Logger.WriteTraceLog($"Start GetUserMasters()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(clr)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(clr.TableSchema).GetUserMasters());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetUserMasters()");
            return response;
        }

        [HttpGet]
        public TokenResponse GetSystemDocumentFieldMaster()
        {
            Logger.WriteTraceLog($"Start GetSystemDocumentFieldMaster()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new StackingOrderService().GetSystemDocumentFields());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetSystemDocumentFieldMaster()");
            return response;
        }

        #endregion


        #region KPI User Role Configuration
        [HttpPost]
        public TokenResponse GetUserRoleList (RoleListRequest request)
        {
            Logger.WriteTraceLog($"Start GetUserRoleList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).GetUserRoleList(request.RoleID));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetUserRoleList()");
            return response;
        }
        [HttpPost]
      public TokenResponse  SaveKpiConfigurationDetails (AddKpiGoalConfig request)
        {
            Logger.WriteTraceLog($"Start SaveKpiConfigurationDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).SaveKpiConfigurationDetails(request.KpiUserGroupConfig, request.KpiGoalConfig,request.IsExistNewUserGrp));
            }
            catch(Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveKpiConfigurationDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse SaveKPIConfigStagingDetails(KpiGoalConfigStaging request)
        {
            Logger.WriteTraceLog($"Start SaveKPIConfigStagingDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).SaveKpiConfigurationDetails(request.GroupID, request.ConfigType, request.Goal));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveKPIConfigStagingDetails()");
            return response;
        }
        [HttpPost]
        public TokenResponse UpdateKPIConfigStagingData(KpiGoalConfigStaging request)
        {
            Logger.WriteTraceLog($"Start UpdateKPIConfigStagingData()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).UpdateKPIConfigStagingData(request.ID,request.GroupID, request.ConfigType, request.Goal, request.Status));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateKPIConfigStagingData()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetKPIGoalConfigStagingDetails(KpiGoalConfigStaging request)
        {
            Logger.WriteTraceLog($"Start GetKPIGoalConfigStagingDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(request.TableSchema).GetKPIGoalConfigStagingDetails(request.GroupID, request.ConfigType));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetKPIGoalConfigStagingDetails()");
            return response;
        }
        
        #endregion


        #region Encompass Parking Spot 
        [HttpPost]
        public TokenResponse AddParkingSpot(AddParkingSpot requset)
        {
            Logger.WriteTraceLog($"Start AddParkingSpot()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(requset)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(requset.TableSchema).AddParkingSpot(requset.ParkingSpotName, requset.Active));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddParkingSpot()");
            return response;
        }
        [HttpPost]
        public TokenResponse UpdateParkingSpot(AddParkingSpot requset)
        {
            Logger.WriteTraceLog($"Start UpdateParkingSpot()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(requset)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new MasterService(requset.TableSchema).UpdateParkingSpot(requset.ParkingSpotName, requset.Active,requset.ParkingSpotID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateParkingSpot()");
            return response;
        }
        #endregion
    }
}
