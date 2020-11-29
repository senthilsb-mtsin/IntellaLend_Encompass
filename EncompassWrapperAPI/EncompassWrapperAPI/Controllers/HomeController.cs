using System.Web.Mvc;

namespace EncompassConnectorAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Encompass Wrapper API";

            return View();
        }
    }
}
