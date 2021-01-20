using IntellaLend.ADServices;
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
    public class ADController : ApiController
    {
        [HttpPost]
        public TokenResponse GetADGroups(GetADGroupsRequest request)
        {
            Logger.WriteTraceLog($"Start GetADGroups()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(request)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new ADService(request.TableSchema).GetADGroups(request.LDAPUrl));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetADGroups()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetADConfig(GetAppConfig req)
        {
            Logger.WriteTraceLog($"Start GetADConfig()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new ADService(req.TableSchema).GetADConfig());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetADConfig()");
            return response;
        }


        [HttpPost]
        public TokenResponse SaveADConfig(RequestSaveADConfig req)
        {
            Logger.WriteTraceLog($"Start SaveADConfig()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new ADService(req.TableSchema).SaveADConfig(req.ADDOMAIN,req.LDAPURL));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SaveADConfig()");
            return response;
        }

        [HttpPost]
        public TokenResponse CheckADGroupAssignedForRole(ADGroupAssignedForRoleRequest req)
        {
            Logger.WriteTraceLog($"Start CheckADGroupAssignedForRole()");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new ADService(req.TableSchema).CheckADGroupAssignedForRole(req.ADGroupID, req.RoleID));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckADGroupAssignedForRole()");
            return response;
        }
    }
}