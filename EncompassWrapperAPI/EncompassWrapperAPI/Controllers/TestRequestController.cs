using Swagger.Net.Annotations;
using System.Net;
using System.Web.Http;

namespace EncompassConnectorAPI.Controllers
{
    public class TestRequestController : ApiController
    {
        [HttpGet, Route("api/v1/test")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Bad Request")]
        public IHttpActionResult Test()
        {
            return Ok("API Up and Running");
        }

    }
}
