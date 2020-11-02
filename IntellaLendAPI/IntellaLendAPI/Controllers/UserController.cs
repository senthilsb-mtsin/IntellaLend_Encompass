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
    public class UserController : ApiController
    {
        #region Get User

        [HttpPost]
        public TokenResponse GetUserList(UserRequest userdetails)
        {
            Logger.WriteTraceLog($"Start GetUserList()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(userdetails)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(userdetails.TableSchema).GetUsersList());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetUserList()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetUser(GetUserRequest user)
        {
            Logger.WriteTraceLog($"Start GetUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(user.TableSchema).GetUser(user.UserName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetUser()");
            return response;
        }
        [HttpPost]
        public TokenResponse GetUserDetails(GetServiceBasedUserRequest user)
        {
            Logger.WriteTraceLog($"Start GetUserDetails()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(user.TableSchema).GetServiceBasedUserDetails(user.LoanID, user.ServiceTypeName));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetUserDetails()");
            return response;
        }

        [HttpPost]
        public TokenResponse GetKPIConfig(UserListRequest user)
        {
            Logger.WriteTraceLog($"Start GetKPIConfig()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(user.TableSchema).getKPIConfig());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End GetKPIConfig()");
            return response;
        }

        [HttpPost]

        public TokenResponse UpdateKPIConfigData(UpdateKPIConfig user)
        {
            Logger.WriteTraceLog($"Start UpdateKPIConfigData()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(user.TableSchema).updateKPIConfig(user.ID, user.Goal, user.PeriodFrom, user.PeriodTo, user.UserGroupGoal));
            }
            catch (Exception exe)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = exe.Message;
                MTSExceptionHandler.HandleException(ref exe);
            }
            Logger.WriteTraceLog($"End UpdateKPIConfigData()");
            return response;
        }


        #endregion

        #region Update region
        [HttpPost]
        public TokenResponse ResetExpiredPassword(GetUserRequest user)
        {
            Logger.WriteTraceLog($"Start ResetExpiredPassword()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(user.TableSchema).ResetUserPassword(user.UserName).ToString());
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End ResetExpiredPassword()");
            return response;
        }
        [HttpPost]
        public TokenResponse AddUser(UserUpdateRequest User)
        {
            Logger.WriteTraceLog($"Start AddUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(User)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();
            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(User.TableSchema).AddUser(User.user));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End AddUser()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateUser(UserUpdateRequest user)
        {
            Logger.WriteTraceLog($"Start UpdateUser()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(user)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(user.TableSchema).UpdateUser(user.user));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateUser()");
            return response;
        }

        [HttpPost]
        public TokenResponse SetSecurityQuestion(UserSecurityQuestionRequest Question)
        {
            Logger.WriteTraceLog($"Start SetSecurityQuestion()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(Question)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(Question.TableSchema).SetSecurityQuestion(Question.SecurityQuestion, Question.NewPassword));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End SetSecurityQuestion()");
            return response;
        }

        [HttpPost]
        public TokenResponse CheckCurrentPassword(UserPasswordResetRequest reset)
        {
            Logger.WriteTraceLog($"Start CheckCurrentPassword()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reset)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(reset.TableSchema).CheckCurrentPassword(reset.UserID, reset.CurrentPassword));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End CheckCurrentPassword()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateNewPassword(UserPasswordResetRequest reset)
        {
            Logger.WriteTraceLog($"Start UpdateNewPassword()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reset)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(reset.TableSchema).UpdateNewPassword(reset.UserID, reset.NewPassword));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateNewPassword()");
            return response;
        }

        [HttpPost]
        public TokenResponse UpdateNewPasswordForExpiry(UserPasswordUpdateRequest Pwd)
        {
            Logger.WriteTraceLog($"Start UpdateNewPasswordForExpiry()");
            Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(Pwd)}");
            TokenResponse response = new TokenResponse();
            response.ResponseMessage = new ResponseMessage();

            try
            {
                response.token = new JWTToken().CreateJWTToken();
                response.data = new JWTToken().CreateJWTToken(new UserService(Pwd.TableSchema).UpdateNewPasswordForExpiry(Pwd.UserID, Pwd.NewPassword));
            }
            catch (Exception ex)
            {
                response.token = null;
                response.ResponseMessage.MessageDesc = ex.Message;
                MTSExceptionHandler.HandleException(ref ex);
            }
            Logger.WriteTraceLog($"End UpdateNewPasswordForExpiry()");
            return response;
        }

        #endregion

    }
}