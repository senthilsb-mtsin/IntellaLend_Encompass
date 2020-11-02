using IntellaLend.BoxWrapper;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace IL.BoxFileDownloader
{
    public class BoxFileDownloader : IMTSServiceBase
    {


        private static string IntellaLendLoanUploadPath = string.Empty;
        private static int MaxRetryCount = 0;
        private const Int32 PDF_CREATED = 0;
        private static string GhostScriptPath = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            MaxRetryCount = Convert.ToInt32(Params.Find(f => f.Key == "DownloadFailRetryCOunt").Value);
            GhostScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GhostScript", "gswin64c.exe");
        }

        public bool DoTask()
        {
            try
            {
                var TenantList = BoxFileDownloaderDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    BoxFileDownloaderDataAccess dataAccess = new BoxFileDownloaderDataAccess(tenant.TenantSchema, MaxRetryCount);

                    List<BoxDownloadQueue> itemToDownload = dataAccess.GetFilesToDownload(BoxDownloadStatusConstant.DOWNLOAD_PENDING);
                    while (itemToDownload != null && itemToDownload.Count > 0 && itemToDownload[0].ID != 0)
                    {
                        DownloadFromBox(itemToDownload, dataAccess, tenant.TenantSchema);
                        itemToDownload = dataAccess.GetFilesToDownload(BoxDownloadStatusConstant.DOWNLOAD_PENDING);
                    }

                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return true;
        }


        private void DownloadFromBox(List<BoxDownloadQueue> fileItems, BoxFileDownloaderDataAccess dataAccess, string TenantSchema)
        {
            string lockFileName = string.Empty;
            string OrgFileName = string.Empty;
            string gsFileName = string.Empty;
            PDFMerger merger = null;
            try
            {
                string exactPath = Path.Combine(IntellaLendLoanUploadPath, TenantSchema, "Input", DateTime.Now.ToString("yyyyMMdd"));
                if (!Directory.Exists(exactPath))
                    Directory.CreateDirectory(exactPath);
                string newFileName = TenantSchema + "_" + fileItems[0].LoanID.ToString() + ".lck";

                lockFileName = Path.Combine(exactPath, newFileName);

                OrgFileName = Path.ChangeExtension(lockFileName, Path.GetExtension(fileItems[0].BoxFileName));

                gsFileName = Path.Combine(exactPath, $"{TenantSchema}_{fileItems[0].LoanID.ToString()}.gs");

                BoxAPIWrapper boxwrap = new BoxAPIWrapper(TenantSchema, fileItems[0].UserID);
                if (fileItems.Count == 1)
                {
                    File.WriteAllBytes(lockFileName, boxwrap.DownloadFile(fileItems[0].BoxEntityID));
                }
                else
                {
                    merger = new PDFMerger(lockFileName);
                    merger.OpenDocument();
                    foreach (var item in fileItems)
                    {
                        merger.AppendPDF(boxwrap.DownloadFile(item.BoxEntityID));
                    }
                    merger.SaveDocument();
                }

                Int32 tiffGenerated = -1; // Prakash : PDF to PDF Conversion Issue(InvalidPDFException) //GhostScriptConvertion(lockFileName, gsFileName);

                if (tiffGenerated == PDF_CREATED)
                {
                    if (File.Exists(lockFileName))
                        File.Delete(lockFileName);

                    File.Move(gsFileName, OrgFileName);
                }
                else
                {
                    if (File.Exists(gsFileName))
                        File.Delete(gsFileName);

                    File.Move(lockFileName, OrgFileName);
                }

                dataAccess.UpdateFileDownloadStatus(fileItems[0].ID, BoxDownloadStatusConstant.DOWNLOAD_SUCCESS, Path.GetFileName(OrgFileName));
            }
            catch (Exception ex)
            {
                if (merger != null)
                {
                    merger.SaveDocument();
                }

                if (File.Exists(lockFileName))
                    File.Delete(lockFileName);
                if (File.Exists(OrgFileName))
                    File.Delete(OrgFileName);
                dataAccess.UpdateFileDownloadStatus(fileItems[0].ID, BoxDownloadStatusConstant.DOWNLOAD_FAILED, string.Empty, ex.Message);
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        private int GhostScriptConvertion(string lockFileName, string outputPath)
        {
            String ars = $"-dNOPAUSE -sDEVICE=pdfwrite -sOutputFile=\"{outputPath}\" -sPAPERSIZE=a4 \"{lockFileName}\" -c quit";
            Process proc = new Process();
            proc.StartInfo.FileName = GhostScriptPath;
            proc.StartInfo.Arguments = ars;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit(20 * 60000);
            bool isRunning = !proc.HasExited;
            if (isRunning)
                proc.Kill();
            return isRunning ? -1 : proc.ExitCode;
        }
    }

}
