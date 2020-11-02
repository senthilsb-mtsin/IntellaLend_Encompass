using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLend_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        [HttpPost]
        public TokenResponse LoginSubmit(UserDetailsRequest userForm)
        {
            Logger.WriteTraceLog($"Start LoginSubmit()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(userForm)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                string Hash = string.Empty;
                //response.token = new JWTToken().CreateJWTToken();
                logOnService _logOn = new logOnService(userForm.TableSchema);
                response.data = new JWTToken().CreateJWTToken(_logOn.GetLoginUser(userForm.UserName, userForm.Password, out Hash));
                response.token = new JWTToken().CreateJWTToken(Hash, userForm.TableSchema);
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End LoginSubmit()");
            return response;
        }

        [HttpPost]
        public TokenResponse UserNameCheck(GetUserRequest user)
        {
            Logger.WriteTraceLog($"Start UserNameCheck()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                object User = new UserService(user.TableSchema).GetUserSecurityQuestion(user.UserName);
                if (User != null)
                {
                    response.data = JsonConvert.SerializeObject(User);
                }
                else
                    response.token = null;
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End UserNameCheck()");
            return response;
        }

        [HttpPost]
        public TokenResponse ForgetPassword(GetUserRequest user)
        {
            Logger.WriteTraceLog($"Start ForgetPassword()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.data = new UserService(user.TableSchema).ResetUserPassword(user.UserName).ToString();
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End ForgetPassword()");
            return response;
        }

        [HttpPost]
        public TokenResponse LockUnlockUser(LockUnlockUserRquest user)
        {
            Logger.WriteTraceLog($"Start LockUnlockUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.data = new UserService(user.TableSchema).LockUnlockUser(user.UserID, user.Lock).ToString();
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }

            Logger.WriteTraceLog($"End LockUnlockUser()");
            return response;
        }
    }
}
