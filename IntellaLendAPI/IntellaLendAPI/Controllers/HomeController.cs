using MTSEntBlocks.LoggerBlock;
using System.Web.Mvc;

namespace IntellaLendAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Logger.WriteTraceLog($"Start Index() in Home controller");
            ViewBag.Title = "Home Page";
            Logger.WriteTraceLog($"End Index() in Home controller");
            return View();
        }
    }
}
