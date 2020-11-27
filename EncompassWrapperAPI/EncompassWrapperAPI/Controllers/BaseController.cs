using Encompass.WebConnector.Models;
using MTS.Web.Helpers;
using System.Web.Http;

namespace EncompassWrapperAPI.Controllers
{
    [Helpers.BaseActionFilter]
    public class BaseController : ApiController
    {
        public RestWebClient _client { get; set; }
        public EncompassWebConnectorSession EncompassWebConnectorSession { get; set; }
    }
}
