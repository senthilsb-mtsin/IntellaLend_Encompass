using System.Web.Http;
using System.Web.Http.Cors;

namespace IntellaLendAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenController : ApiController
    {
        //[HttpPost]
        //[Route("api/")]
        //public TokenResponse GetToken()
        //{
        //    Logger.WriteTraceLog($"Start AddRetention()");
        //    Logger.WriteTraceLog($"Request Body : {JsonConvert.SerializeObject(reqAddRetention)}");
        //    TokenResponse response = new TokenResponse();
        //    response.ResponseMessage = new ResponseMessage();
        //    try
        //    {
        //        response.token = new JWTToken().CreateJWTToken();
        //        response.data = new JWTToken().CreateJWTToken(new CustConfigService(reqAddRetention.TableSchema).AddRetention(reqAddRetention.CustomerID, reqAddRetention.ConfigKey, reqAddRetention.ConfigValue, reqAddRetention.Active, reqAddRetention.CreatedOn, reqAddRetention.ModifiedOn));
        //    }
        //    catch (Exception exc)
        //    {
        //        response.token = null;
        //        response.ResponseMessage.MessageDesc = exc.Message;
        //        MTSExceptionHandler.HandleException(ref exc);
        //    }
        //    Logger.WriteTraceLog($"End AddRetention()");
        //    return response;
        //}

    }
}
