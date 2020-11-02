using IntellaLend.CommonServices;
using IntellaLend.Model;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.Options;
using System;
using System.IO;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace IntellaLendAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ImageController : Controller
    {
        [System.Web.Mvc.HttpGet]
        public ActionResult Get(string TenantSchema, Int64 LoanID, string Guid)
        {
            Logger.WriteTraceLog($"Start Get() in Image Controller");
            // var stream = new MemoryStream();
            byte[] bytes = new LoanService(TenantSchema).GetLoanImage(LoanID, CommonUtils.EnDecrypt(Guid, true));
            //byte[] bytes = new LoanService(TenantSchema).GetLoanImage(LoanID, Guid);
            Logger.WriteTraceLog($"End Get() in Image Controller");
            return File(bytes, "image/jpeg");
            //stream.Write(bytes, 0, bytes.Length);
            //var content = new StreamContent(stream);
            //content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            //content.Headers.ContentLength = stream.GetBuffer().Length;
            //return Ok(content);
        }

        [System.Web.Mvc.HttpGet]
        public void DownloadLoanPDF(string TenantSchema, Int64 LoanID)
        {
            Logger.WriteTraceLog($"Start DownloadLoanPDF()");
            LoanService _loanService = new LoanService(TenantSchema);

            Loan loanObj = _loanService.GetLoanHeaderDeatils(LoanID);
            string FileName = String.Format("{0}.pdf", string.IsNullOrEmpty(loanObj.LoanNumber) ? LoanID.ToString() : loanObj.LoanNumber);
            string _footerText = _loanService.GetPDFFooterName();
            Stream iStream = _loanService.GetLoanPDFStream(LoanID);
            byte[] buffer = new Byte[1048576];

            int length;
            long dataToRead;
            dataToRead = iStream.Length;
            Response.BufferOutput = true;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(FileName, System.Text.Encoding.UTF8));
            while (dataToRead > 0)
            {
                if (Response.IsClientConnected)
                {
                    length = iStream.Read(buffer, 0, 10000);
                    Response.OutputStream.Write(buffer, 0, length);
                    Response.Flush();
                    buffer = new Byte[10000];
                    dataToRead = dataToRead - length;
                }
                else
                {
                    dataToRead = -1;
                }
            }
            Logger.WriteTraceLog($"End DownloadLoanPDF()");
            //return File(iStream, "application/octet-stream", FileName);
        }

        //[System.Web.Http.HttpPost]
        //public System.Threading.Tasks.Task<HttpResponseMessage> DownloadLoanPDF(string TenantSchema, Int64 LoanID)
        //{
        //    LoanService _loanService = new LoanService(TenantSchema);
        //    Loan loanObj = _loanService.GetLoanHeaderDeatils(LoanID);
        //    string PdfFileName = String.Format("{0}.pdf", string.IsNullOrEmpty(loanObj.LoanNumber) ? string.Empty : loanObj.LoanNumber);
        //    //  byte[] bytes = _loanService.GetLoanPDF(LoanID);
        //    byte[] loanPdf = System.IO.File.ReadAllBytes(@"D:\Project\81037127179_Box.pdf");
        //    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        //    //httpResponseMessage.Content = new StreamContent(loanPdf);
        //    httpResponseMessage.Content = new ByteArrayContent(loanPdf);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

        //    return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        //}

        [System.Web.Mvc.HttpGet]
        public ActionResult GetReverificationImage(string TenantSchema, string Guid)
        {
            Logger.WriteTraceLog($"Start GetReverificationImage()");
            //byte[] bytes = new LoanService(TenantSchema).GetLoanImage(LoanID, CommonUtils.EnDecrypt(Guid, true));
            //var bucketName = "Reverification";
            //ImageMinIOWrapper _imageWrapper = new ImageMinIOWrapper(TenantSchema);
            byte[] bytes = new byte[] { };
            if (!string.IsNullOrEmpty(TenantSchema) && !string.IsNullOrEmpty(Guid) && Guid != "null")
            {
                byte[] _image = new IntellaLendServices(TenantSchema).GetReverificationLogo(Guid); // _imageWrapper.GetObject(bucketName, Guid);
                return File(_image, "image/jpeg");
            }
            Logger.WriteTraceLog($"End GetReverificationImage()");
            return File(bytes, "image/jpeg");
        }

        [System.Web.Http.HttpGet]
        public ActionResult DownloadLoanAuditReport(string TenantSchema, Int64 LoanID)
        {
            Logger.WriteTraceLog($"Start DownloadLoanAuditReport()");
            var report = new ActionAsPdf("AuditReport", new { TenantSchema = TenantSchema, LoanID = LoanID })
            {
                CustomSwitches = " --footer-html FooterView.html",
                PageMargins = new Margins(10, 5, 10, 5)
            };

            byte[] bytes = report.BuildPdf(ControllerContext);
            Logger.WriteTraceLog($"End DownloadLoanAuditReport()");
            return File(bytes, "application/octet-stream", $"AuditReport_{LoanID.ToString()}.pdf");
        }


        private ActionResult AuditReport(string TenantSchema, Int64 LoanID)
        {
            Logger.WriteTraceLog($"Start AuditReport()");
            LoanService _loanService = new LoanService(TenantSchema);
            LoanAuditReportPDF loanObj = _loanService.GetLoanAuditReportDeatils(LoanID);
            Logger.WriteTraceLog($"End AuditReport()");
            return View(loanObj);
        }


    }

    //public class loanPdfResult : IHttpActionResult
    //{
    //    byte[] loanPdf;
    //    string PdfFileName;
    //    HttpResponseMessage httpResponseMessage;
    //    public loanPdfResult(byte[] data, string filename)
    //    {
    //        loanPdf = data;
    //        PdfFileName = filename;
    //    }
    //    public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
    //    {
    //        httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
    //        //httpResponseMessage.Content = new StreamContent(loanPdf);
    //        httpResponseMessage.Content = new ByteArrayContent(loanPdf);
    //        httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
    //        httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
    //        httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

    //        return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
    //    }
    //}
}