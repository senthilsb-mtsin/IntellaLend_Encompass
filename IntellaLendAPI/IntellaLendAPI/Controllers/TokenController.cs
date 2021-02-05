using IntellaLend.CommonServices;
using IntellaLendAPI.Models;
using IntellaLendJWTToken;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenController : ApiController
    {
        [HttpGet, Route("api/v2/token/create")]
        public IHttpActionResult Create()
        {
            Logger.WriteTraceLog($"Start GetToken()");
            ExternalTokenResponse response = new ExternalTokenResponse();
            try
            {
                string requestHeader = Request.Headers.Authorization.Parameter;

                if (string.IsNullOrEmpty(requestHeader))
                    return BadRequest("Header is Empty");

                Logger.WriteTraceLog($"Before Decrypt : {requestHeader}");
                string requestObject = string.Empty;
                try
                {
                    requestObject = EnDecryptor.DecryptAES(requestHeader);
                }
                catch (Exception ex)
                {
                    MTSExceptionHandler.HandleException(ref ex);
                    return BadRequest($"Decryption Failed. Message : {ex.Message}");
                }

                if (string.IsNullOrEmpty(requestObject))
                    return BadRequest($"Request Object is Empty");

                Logger.WriteTraceLog($"After Decrypt : {requestObject}");
                CreateTokenRequest _userInfo = new CreateTokenRequest();
                try
                {
                    _userInfo = JsonConvert.DeserializeObject<CreateTokenRequest>(requestObject);
                    Logger.WriteTraceLog($"_userInfo.Username : {_userInfo.Username}");
                    Logger.WriteTraceLog($"_userInfo.Password : {_userInfo.Password}");
                    Logger.WriteTraceLog($"_userInfo.UTCTime : {_userInfo.UTCTime}");
                    Logger.WriteTraceLog($"_userInfo.TokenExpireTime : {_userInfo.TokenExpireTime}");
                }
                catch (Exception ex)
                {
                    MTSExceptionHandler.HandleException(ref ex);
                    return BadRequest($"Not able to Convert Request Object");
                }

                bool userValid = new logOnService("T1").APIUserCheck(_userInfo.Username, _userInfo.Password);

                if (!userValid)
                    return Unauthorized();

                response.Token = new JWTToken().CreateJWTToken(_userInfo.UTCTime, _userInfo.TokenExpireTime);
                response.Type = "Bearer";

                return Ok(response);
            }
            catch (Exception exc)
            {
                MTSExceptionHandler.HandleException(ref exc);
            }
            Logger.WriteTraceLog($"End GetToken()");
            return BadRequest("Unable to Generate Token");
        }

        [HttpGet, Route("api/v2/token/test")]
        public IHttpActionResult Test()
        {
            try
            {
                return Ok("Hello World");
            }
            catch (Exception exc)
            {
                MTSExceptionHandler.HandleException(ref exc);
            }
            return BadRequest("Unable to Generate Token");
        }
    }
}
