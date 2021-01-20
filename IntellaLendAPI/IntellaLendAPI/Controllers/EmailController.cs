using IntellaLend.CommonServices;
using IntellaLend.CommonServices.EmailServices;
using IntellaLend.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace IntellaLendAPI.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult NewUser()
        {
            string[] emailParams = Request.QueryString["ID"].Split(',');
            ViewBag.DISPLAYNAME = emailParams[0];
            ViewBag.USERNAME = emailParams[1];
            ViewBag.PASSWORD = emailParams[2];
            ViewBag.userEmail = emailParams[3];
            ViewBag.URL = emailParams[4];
            return View();
        }

        public ActionResult ChangePasswordEmail()
        {
            string[] emailParams = Request.QueryString["ID"].Split(',');
            ViewBag.DISPLAYNAME = emailParams[0];
            ViewBag.USERNAME = emailParams[1];
            ViewBag.PASSWORD = emailParams[2];
            ViewBag.URL = emailParams[3];
            return View();
        }

        public ActionResult RuleFindingsEmail()
        {
           
            string[] emailParams = Request.QueryString["ID"].Split('|');
            LoanService _loan = new LoanService(emailParams[0]);
            long _loanID = Convert.ToInt64(emailParams[3]);
            string _rules = _loan.GetRuleFindings(_loanID);
            string[] _ruleFindings = _rules.Split('~');
            ViewBag.To = emailParams[1];
            ViewBag.Subject = emailParams[2];
            ViewBag.LoanNumber = _ruleFindings[1];
            ViewBag.UserName = _ruleFindings[2];
            ViewBag.ApplicationURL  = _loan.GetApplicationURL();
            if (!(string.IsNullOrEmpty(_rules)))
            {
              ViewBag.Rules = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(_ruleFindings[0]);
            }
            else
            {
                ViewBag.Rules = new List<Dictionary<string, string>>();
            }
           
            //ViewBag.URL = emailParams[3];
            return View();
        }

        public ActionResult MASJsonEmail()
        {
            string[] emailparams = Request.QueryString["ID"].Split(',');
            EmailService emailService = new EmailService(emailparams[0]);
            LOSImportStaging stagingDetail = emailService.GetImportStagingDetails(Convert.ToInt64(emailparams[1]));
            string fileName = Path.GetFileName(Path.ChangeExtension(stagingDetail.FileName,"json"));
            ViewBag.FileName = fileName;
            ViewBag.ErrorMsg = stagingDetail.ErrorMsg;
            return View();
        }
    }
}