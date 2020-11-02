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
    public class MenuController : ApiController
    {
        [HttpPost]
        public TokenResponse GetMenuList(RoleListRequest roleName)
        {
            Logger.WriteTraceLog($"Start GetMenuList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(roleName)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                logOnService _logOn = new logOnService(roleName.TableSchema);
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(_logOn.getRoleDetails(roleName.RoleID, roleName.UserID));                
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetMenuList()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetAllMenuList(MenuListRequest request)
        {
            Logger.WriteTraceLog($"Start GetAllMenuList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new logOnService(request.TableSchema).GetMenuList());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetAllMenuList()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetRoleMenuAccessList(RoleMenuAccessList request)
        {
            Logger.WriteTraceLog($"Start GetRoleMenuAccessList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new logOnService(request.TableSchema).GetMenuAccessList(request.RoleID,request.IsMappedMenuView));
            }
            catch (Exception ex)
            {
                response.token = null; 
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetRoleMenuAccessList()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetRoleMenuDetails(RoleMenuActiveList request)
        {
            Logger.WriteTraceLog($"Start GetRoleMenuDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new logOnService(request.TableSchema).GetRoleMenuActive(request.RoleID,request.menus));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetRoleMenuDetails()");
            return response;
        }
    }
}
