using MTSEntBlocks.LoggerBlock;
using System.Web.Mvc;

namespace IntellaLendAPI.Controllers
{
    public class LOSExportController : Controller
    {
        // GET: LOSExport
        public ActionResult LOSExportJSONStructure()
        {
            Logger.WriteTraceLog($"Start LOSExportJSONStructure()");
            return View();
        }
    }
}