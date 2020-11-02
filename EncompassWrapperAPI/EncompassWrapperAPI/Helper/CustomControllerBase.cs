using EncompassWrapperConstants;
using EncompassWrapperHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace EncompassWrapperAPI
{
    public class CustomControllerBase : Controller
    {
        protected SessionHelper sessionHelper;

        public CustomControllerBase()
        {
            sessionHelper = new SessionHelper();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            StringValues _token = string.Empty;
            StringValues _tokenType = string.Empty;
            context.HttpContext.Request.Headers.TryGetValue(HeaderConstant.TokenHeader, out _token);
            context.HttpContext.Request.Headers.TryGetValue(HeaderConstant.TokenTypeHeader, out _tokenType);
            sessionHelper.Token = _token.ToString();
            sessionHelper.TokenType = _tokenType.ToString();
            base.OnActionExecuting(context);
        }
    }
}
