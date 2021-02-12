
using BitMiracle.LibTiff.Classic;
using IL.Ephesoft.Export;
using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace IL.ExportToEphesoft
{
    public class ExportToExphesoft : IMTSServiceBase
    {
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string TiffProcessingPath = string.Empty;
        private static string[] extArry = { ".zip", ".rar" };
        private int WaitingBatchCount = 1;
        private bool UseGhostScript = false;
        private static string GhostScriptPath = string.Empty;
        private static string iMagickPath = string.Empty;
        private static Int32 GhostScriptExecutionMintues = 20;
        private static Int32 GhostScriptExecutionMilliSeconds = 0;
        private const Int32 TIFF_CREATED = 0;
        private const Int32 PDF_CREATED = 0;
        private const Int32 MISSING_DOC_PRIORITY = 1;
        private bool logTracing = false;
        private Int32 TiffWidth = 2550;
        private Int32 TiffHeight = 3300;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            TiffProcessingPath = Params.Find(f => f.Key == "TiffProcessingPath").Value;
            Int32.TryParse(Params.Find(f => f.Key == "WaitingBatchCount").Value, out WaitingBatchCount);
            Int32.TryParse(Params.Find(f => f.Key == "TiffImageWidth").Value, out TiffWidth);
            Int32.TryParse(Params.Find(f => f.Key == "TiffImageHeight").Value, out TiffHeight);
            Boolean.TryParse(Params.Find(f => f.Key == "UseGhostScript").Value, out UseGhostScript);
            GhostScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GhostScript", "gswin64c.exe");
            iMagickPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "ImageMagick", "magick.exe");
            Int32.TryParse(Params.Find(f => f.Key == "ScriptExecutionTime").Value, out GhostScriptExecutionMintues);
            GhostScriptExecutionMilliSeconds = GhostScriptExecutionMintues * 60000;
            Boolean.TryParse(ConfigurationManager.AppSettings["MoveToEphesoftLog"].ToLower(), out logTracing);
        }

        public bool DoTask()
        {
            try
            {
                ExportToProcessingQueue();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return true;
        }

        #region Private Methods

        private bool ExportToProcessingQueue()
        {
            try
            {
                var TenantList = EphesoftExportDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EphesoftExportDataAccess dataAccess = new EphesoftExportDataAccess(tenant.TenantSchema);
                    Int32 _batchCountToExport = GetDocumentExportCount();
                    if (_batchCountToExport > 0)
                        MoveToProcessingQueue(dataAccess, tenant, _batchCountToExport);
                }
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
            return false;
        }

        private void MoveToProcessingQueue(EphesoftExportDataAccess dataAccess, TenantMaster tenant, Int32 _batchCountToExport)
        {
            try
            {
                Int32 totalExportedDocument = 0;

                string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, tenant.TenantSchema);

                List<string> missingDocFiles = Directory.GetFiles(Path.Combine(tenantFolder, "Input"), "*_*_*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".lck") && !name.EndsWith(".gs")).ToList();
                //List<string> missingDocFiles = Directory.GetFiles(Path.Combine(tenantFolder, "Input"), "*_*_*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".lck")).ToList();

                //missingDocFiles = (from s in missingDocFiles select s.Split('.')[0]).Distinct().ToList();

                LogMessage($"Missing Documents to be Exported : {missingDocFiles.Count.ToString()}");

                if (missingDocFiles.Count > 0)
                    totalExportedDocument = MoveDocumentToProcessingQueue(dataAccess, tenant.TenantSchema, _batchCountToExport, tenantFolder, missingDocFiles);

                _batchCountToExport = _batchCountToExport - totalExportedDocument;

                if (totalExportedDocument < _batchCountToExport)
                {
                    LogMessage($"totalExportedDocument after Missing Docs : {totalExportedDocument.ToString()}");
                    List<string> loanDocFiles = GetLoansToBeExported(dataAccess, tenantFolder, _batchCountToExport);
                    totalExportedDocument = MoveDocumentToProcessingQueue(dataAccess, tenant.TenantSchema, _batchCountToExport, tenantFolder, loanDocFiles);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<string> GetLoansToBeExported(EphesoftExportDataAccess dataAccess, string tenantFolder, Int32 loanFetchCount)
        {
            List<ExportLoan> _loans = dataAccess.GetLoansToBeExported(loanFetchCount);
            List<string> loanDocFiles = new List<string>();
            LogMessage($"Loan to be Exported : {_loans.Count.ToString()}");
            foreach (ExportLoan item in _loans)
            {
                LogMessage($"Checking Path : {Path.Combine(tenantFolder, "Input")} {item.TenantSchema.ToUpper()}_{item.LoanID.ToString()}.*");
                string filePath = Directory.GetFiles(Path.Combine(tenantFolder, "Input"), $"{item.TenantSchema.ToUpper()}_{item.LoanID.ToString()}.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".lck") && !name.EndsWith(".gs")).FirstOrDefault();
                LogMessage($"File Path : {filePath}");

                if (!string.IsNullOrEmpty(filePath))
                    loanDocFiles.Add(filePath);
            }

            return loanDocFiles;
        }

        private int MoveDocumentToProcessingQueue(EphesoftExportDataAccess dataAccess, string tenantName, int documentCount, string tenantFolder, List<string> inputFiles)
        {
            int totalExportedDocument = 0;

            Int64 _loanID = 0, DocID = 0;
            try
            {
                foreach (string file in inputFiles)
                {
                    if (documentCount <= 0)
                        return totalExportedDocument;

                    LogMessage($"Processing File : {file}");

                    string fileName = file;
                    string _file = Path.GetFileNameWithoutExtension(file);
                    FileInfo fileInfo = new FileInfo(fileName);

                    LogMessage($"File Name : {_file}");
                    string test = string.Join(",", _file.Split('_'));
                    LogMessage($"Split Values {test}");

                    _loanID = Convert.ToInt64(_file.Split('_')[1]);
                    if (_file.Split('_').Length > 2)
                    {
                        LogMessage($"DocID : {_file.Split('_')[2]}");
                        DocID = Convert.ToInt64(_file.Split('_')[2]);
                    }

                    if (!Directory.Exists(TiffProcessingPath))
                        Directory.CreateDirectory(TiffProcessingPath);

                    string outPath = Path.Combine(Path.Combine(tenantFolder, "Output"), DateTime.Now.ToString("yyyyMMdd"));

                    if (!Directory.Exists(outPath))
                        Directory.CreateDirectory(outPath);

                    Int64 _processQueueID = EphesoftExportDataAccess.AddExportProcessingQueue(tenantName, fileName, MISSING_DOC_PRIORITY);
                    if (_processQueueID > 0)
                    {
                        try
                        {
                            //Lock file before copy
                            string lockFileName = Path.ChangeExtension(fileName, ".lck");
                            File.Move(fileName, lockFileName);

                            string gsFileName = Path.ChangeExtension(fileName, ".gs");

                            string _processingFolderPath = Path.Combine(Path.Combine(TiffProcessingPath, _file));

                            if (!Directory.Exists(_processingFolderPath))
                                Directory.CreateDirectory(_processingFolderPath);

                            Int32 pdfGenerated = 1;

                            byte[] pdfBytes = File.ReadAllBytes(fileName);
                            if (CommonUtils.CheckEmbeddedPDF(pdfBytes))
                            {
                                pdfBytes = CommonUtils.GetEmbeddedPDFs(pdfBytes);
                                if (pdfBytes == null)
                                    throw new Exception("Cannot able convert Embedded PDF's to single PDF");

                                File.WriteAllBytes(fileName, pdfBytes);
                            }

                            if (UseGhostScript)
                            {
                                pdfGenerated = GhostScriptConvertion(lockFileName, gsFileName);

                                if (pdfGenerated == PDF_CREATED)
                                {
                                    //if (File.Exists(lockFileName))
                                    //    File.Delete(lockFileName);                                    

                                    Int32 tiffGenerated = CreateTiffBatchFolder(gsFileName, _processingFolderPath);

                                    if (tiffGenerated != TIFF_CREATED)
                                    {
                                        Array.ForEach(Directory.GetFiles(_processingFolderPath), delegate (string path) { File.Delete(path); });
                                        //copy file to Ephesoft folder
                                        if (!Directory.Exists(_processingFolderPath))
                                            Directory.CreateDirectory(_processingFolderPath);

                                        string pdfFile = Path.Combine(_processingFolderPath, fileInfo.Name);

                                        if (File.Exists(pdfFile))
                                            File.Delete(pdfFile);

                                        File.Copy(gsFileName, pdfFile);
                                    }
                                }
                                else
                                {
                                    if (File.Exists(gsFileName))
                                        File.Delete(gsFileName);

                                    //copy file to Ephesoft folder
                                    File.Copy(lockFileName, Path.Combine(_processingFolderPath, fileInfo.Name));
                                }
                            }
                            else
                            {
                                File.Copy(lockFileName, Path.Combine(_processingFolderPath, fileInfo.Name));
                            }

                            //Move to Output Folder
                            if (File.Exists(gsFileName))
                                File.Delete(gsFileName);

                            Int64 _pageCount = CommonUtils.GetPDFPageCount(File.ReadAllBytes(lockFileName));

                            File.Move(lockFileName, Path.Combine(outPath, Path.ChangeExtension(fileInfo.Name, ".don")));

                            dataAccess.UpdateLoanStatus(_loanID, StatusConstant.MOVE_TO_PROCESSING_QUEUE, DocID);
                            dataAccess.SetLoanPDFPageCount(_loanID, _pageCount);

                            EphesoftExportDataAccess.UpdateExportProcessingQueue(_processQueueID, _processingFolderPath);
                        }
                        catch (EntityCommandExecutionException ex)
                        {
                            Exception exe = new Exception("EntityCommandExecutionException", ex);
                            MTSExceptionHandler.HandleException(ref exe);
                            EphesoftExportDataAccess.UpdateExportProcessingQueueStatus(_processQueueID, -1, ex.Message);
                            throw exe;
                        }
                        catch (Exception ex)
                        {
                            MTSExceptionHandler.HandleException(ref ex);
                            EphesoftExportDataAccess.UpdateExportProcessingQueueStatus(_processQueueID, -1, ex.Message);
                            throw ex;
                        }
                        documentCount--;
                        totalExportedDocument++;
                    }
                    else
                    {
                        throw new Exception($"Process Queue ID Not Generated : {fileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                dataAccess.UpdateLoanStatus(_loanID, StatusConstant.IDC_ERROR, DocID);
                throw new Exception($"Error Processing LoanID : {_loanID.ToString()}", ex);
            }

            return totalExportedDocument;
        }

        private int GhostScriptConvertion(string inputPath, string gsFileName)
        {
            String ars = $"-dNOPAUSE -sDEVICE=pdfwrite -sOutputFile=\"{gsFileName}\" -sPAPERSIZE=a4 \"{inputPath}\" -c quit";
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

        private int CreateTiffBatchFolder(string originalFile, string ephsoftTempPath)
        {
            String ars = $"-dNOPAUSE -sDEVICE=tiffscaled8 -sCompression=lzw -r300 -sOutputFile=\"{ephsoftTempPath}/%d.tif\" -sPAPERSIZE=a4 \"{originalFile}\" -c quit";
            Process proc = new Process();
            proc.StartInfo.FileName = GhostScriptPath;
            proc.StartInfo.Arguments = ars;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit(GhostScriptExecutionMilliSeconds);
            bool isRunning = !proc.HasExited;

            if (!isRunning && proc.ExitCode == 0)
                ResizeTiff(ephsoftTempPath);

            if (isRunning)
                proc.Kill();

            return isRunning ? -1 : proc.ExitCode;


            //return proc.ExitCode;
        }

        private void ResizeTiff(string folderPath)
        {
            string[] tifFiles = Directory.GetFiles(folderPath, "*.tif", SearchOption.AllDirectories).OrderBy(a => a).ToArray();
            Process proc = new Process();
            proc.StartInfo.FileName = iMagickPath;
            proc.StartInfo.LoadUserProfile = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            foreach (string file in tifFiles)
            {
                Size size = new Size();
                using (Tiff input = Tiff.Open(file, "r"))
                {
                    if (input == null)
                        continue;
                    size.Width = input.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                    size.Height = input.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                }

                if (size.Height > TiffHeight || size.Width > TiffWidth)
                {
                    string ars = $@"convert {file} -resize {TiffWidth}x{TiffHeight} {file}";
                    proc.StartInfo.Arguments = ars;
                    proc.Start();
                    proc.WaitForExit();
                    int tes = proc.ExitCode;
                }

            }
        }


        private Int32 GetDocumentExportCount()
        {
            //int totalPendingBatch = Directory.GetDirectories(TiffProcessingPath).Count();

            int totalPendingBatch = EphesoftExportDataAccess.GetWaitingProcessedBatchCount();

            return (WaitingBatchCount - totalPendingBatch);
        }

        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

        #endregion

    }
}
