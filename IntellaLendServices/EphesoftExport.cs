using IL.Ephesoft.Export;
using IntellaLend.Constance;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IL.ExportToEphesoft
{
    public class EphesoftExport : IMTSServiceBase
    {
        private static string IntellaLendLoanUploadPath = string.Empty;
        private static string EphesoftInputPath = string.Empty;
        private static string[] extArry = { ".zip", ".rar" };
        private int EphesoftTotalCore;
        private string BatchClassID = string.Empty;
        private string BatchStatus = string.Empty;
        private bool UseGhostScript = false;
        private static string GhostScriptPath = string.Empty;
        private static Int32 GhostScriptExecutionMintues = 20;
        private static Int32 GhostScriptExecutionMilliSeconds = 0;
        private const Int32 TIFF_CREATED = 0;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            EphesoftInputPath = Params.Find(f => f.Key == "EphesoftInputPath").Value;
            EphesoftTotalCore = Convert.ToInt32(Params.Find(f => f.Key == "EphesoftTotalCore").Value);
            BatchClassID = Params.Find(f => f.Key == "BatchClassID").Value;
            BatchStatus = Params.Find(f => f.Key == "BatchStatus").Value;
            Boolean.TryParse(Params.Find(f => f.Key == "UseGhostScript").Value, out UseGhostScript);
            GhostScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GhostScript", "gswin64c.exe");
            Int32.TryParse(Params.Find(f => f.Key == "ScriptExecutionTime").Value, out GhostScriptExecutionMintues);
            GhostScriptExecutionMilliSeconds = GhostScriptExecutionMintues * 60000;
        }

        public bool DoTask()
        {
            try
            {
                return ExportToEphesoft();
            }
            catch (Exception ex)
            {
                BaseExceptionHandler.HandleException(ref ex);
            }

            return false;
        }

        private bool ExportToEphesoft()
        {
            try
            {
                var TenantList = EphesoftExportDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    EphesoftExportDataAccess dataAccess = new EphesoftExportDataAccess(tenant.TenantSchema);

                    //var isDeletOrgFile = dataAccess.CheckCustomerConfigKey();
                    var isDeletOrgFile = false;

                    int documentCount = GetDocumentExportCount(dataAccess);

                    int totalExportedDocument = 0;

                    if (documentCount > 0)
                    {
                        totalExportedDocument += MoveMissingDocumentToEphesoft(tenant.TenantSchema, isDeletOrgFile, documentCount);

                        documentCount = documentCount - totalExportedDocument;

                        var loan = dataAccess.GetLoanDocumentToExport();
                        while (loan != null && loan.LoanID != 0 && documentCount > 0)
                        {

                            if (loan.Status != StatusConstant.LOAN_DELETED)
                            {
                                MoveLoanDocumentToEphesoft(loan, dataAccess, isDeletOrgFile);
                                loan = dataAccess.GetLoanDocumentToExport();
                                documentCount--;
                                totalExportedDocument++;
                            }
                            else
                            {
                                MoveLoanDocumentToDelete(loan);
                            }

                        }

                        if (totalExportedDocument == 0)
                        {
                            //send Notification
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        private int GetDocumentExportCount(EphesoftExportDataAccess dataAccess)
        {
            int totalPendingBatch = dataAccess.GetEphesoftPendingBatchCount(BatchClassID, BatchStatus);

            return (EphesoftTotalCore - totalPendingBatch);
        }


        private void CheckAndCreateFolders(string tenantFolder)
        {
            if (!Directory.Exists(tenantFolder))
                Directory.CreateDirectory(tenantFolder);
            if (!Directory.Exists(EphesoftInputPath))
                Directory.CreateDirectory(EphesoftInputPath);
            if (!Directory.Exists(Path.Combine(EphesoftInputPath, "Input")))
                Directory.CreateDirectory(Path.Combine(EphesoftInputPath, "Input"));
        }

        private void MoveLoanDocumentToDelete(ExportLoan loan)
        {
            try
            {
                string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, loan.TenantSchema);
                string loanFile = loan.TenantSchema + "_" + loan.LoanID.ToString() + Path.GetExtension(loan.FileName);

                var fileSerach = Directory.GetFiles(tenantFolder, loanFile, SearchOption.AllDirectories).ToArray();

                if (fileSerach.Length > 0)
                {
                    string fileName = fileSerach[0];
                    FileInfo fileInfo = new FileInfo(fileName);
                    string outPath = Path.Combine(Path.Combine(tenantFolder, "Deleted"), DateTime.Now.ToString("yyyyMMdd"));
                    CheckAndCreateFolders(outPath);
                    string newFileName = Path.Combine(outPath, fileInfo.Name);
                    File.Move(fileName, newFileName);
                    File.Move(newFileName, Path.ChangeExtension(newFileName, ".del"));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void MoveLoanDocumentToEphesoft(ExportLoan loan, EphesoftExportDataAccess dataAccess, bool isDeletOrgFile)
        {
            try
            {
                string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, loan.TenantSchema);
                string loanFile = loan.TenantSchema + "_" + loan.LoanID.ToString() + Path.GetExtension(loan.FileName);
                string _file = loan.TenantSchema + "_" + loan.LoanID.ToString();
                var fileSerach = Directory.GetFiles(tenantFolder, loanFile, SearchOption.AllDirectories).ToArray();

                if (fileSerach.Length > 0)
                {
                    string fileName = fileSerach[0];
                    FileInfo fileInfo = new FileInfo(fileName);

                    //Extract if the file is zipped
                    if (Array.Exists(extArry, e => e == fileInfo.Extension))
                    {
                        var newfilename = ExtractToFolder(fileName, tenantFolder);
                        if (!newfilename.Equals(string.Empty))
                        {
                            fileName = newfilename;
                            fileInfo = new FileInfo(fileName);
                        }
                    }

                    string outPath = Path.Combine(Path.Combine(tenantFolder, "Output"), DateTime.Now.ToString("yyyyMMdd"));
                    //Create necessary folders
                    CheckAndCreateFolders(outPath);

                    //Lock file before copy
                    string lockFileName = Path.ChangeExtension(fileName, ".lck");
                    File.Move(fileName, lockFileName);

                    //copy file to Ephesoft folder
                    //File.Copy(lockFileName, Path.Combine(Path.Combine(EphesoftInputPath, "Input"), fileInfo.Name));
                 
                    try
                    {
                        if (UseGhostScript)
                        {
                            string folderPath = Path.Combine(Path.Combine(EphesoftInputPath, _file));

                            if (!Directory.Exists(folderPath))
                                Directory.CreateDirectory(folderPath);

                            Int32 tiffGenerated = CreateTiffBatchFolder(lockFileName, folderPath, _file);

                            if (tiffGenerated == TIFF_CREATED)
                                Directory.Move(folderPath, Path.Combine(Path.Combine(EphesoftInputPath, "Input", _file)));
                            else
                            {
                                if (Directory.Exists(folderPath))
                                    Directory.Delete(folderPath, true);

                                //copy file to Ephesoft folder
                                File.Copy(lockFileName, Path.Combine(Path.Combine(EphesoftInputPath, "Input"), fileInfo.Name));
                            }
                        }
                        else
                        {
                            //copy file to Ephesoft folder
                            File.Copy(lockFileName, Path.Combine(Path.Combine(EphesoftInputPath, "Input"), fileInfo.Name));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (isDeletOrgFile)
                    {
                        //Delete the orginal file based on config
                        File.Delete(lockFileName);
                    }
                    else
                    {
                        //Move to Output Folder
                        File.Move(lockFileName, Path.Combine(outPath, Path.ChangeExtension(fileInfo.Name, ".don")));
                    }

                    //Update Loan status as Pending IDC
                    dataAccess.UpdateLoanStatus(loan.LoanID);
                }
                else
                {
                    //Update Loan status as Pending IDC
                    dataAccess.UpdateLoanStatus(loan.LoanID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int CreateTiffBatchFolder(string originalFile, string ephsoftTempPath, string fileName)
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
            if (isRunning)
                proc.Kill();
            return isRunning ? -1 : proc.ExitCode;
            //return proc.ExitCode;
        }

        private int MoveMissingDocumentToEphesoft(string tenantName, bool isDeletOrgFile, int documentCount)
        {
            int totalExportedDocument = 0;

            try
            {


                string tenantFolder = Path.Combine(IntellaLendLoanUploadPath, tenantName);

                //string[] inputFiles = ;
                foreach (string file in Directory.GetFiles(Path.Combine(tenantFolder, "Input"), "*_*_*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".lck")))
                {

                    if (documentCount <= 0)
                        return totalExportedDocument;

                    string fileName = file;
                    FileInfo fileInfo = new FileInfo(fileName);

                    //Extract if the file is zipped
                    if (Array.Exists(extArry, e => e == fileInfo.Extension))
                    {
                        var newfilename = ExtractToFolder(fileName, tenantFolder);
                        if (!newfilename.Equals(string.Empty))
                        {
                            fileName = newfilename;
                            fileInfo = new FileInfo(fileName);
                        }
                    }

                    string outPath = Path.Combine(Path.Combine(tenantFolder, "Output"), DateTime.Now.ToString("yyyyMMdd"));
                    //Create necessary folders
                    CheckAndCreateFolders(outPath);

                    //Lock file before copy
                    string lockFileName = Path.ChangeExtension(fileName, ".lck");
                    File.Move(fileName, lockFileName);
                    //copy file to Ephesoft folder
                    File.Copy(lockFileName, Path.Combine(Path.Combine(EphesoftInputPath, "Input"), fileInfo.Name));

                    if (isDeletOrgFile)
                    {
                        //Delete the orginal file based on config
                        File.Delete(lockFileName);
                    }
                    else
                    {
                        //Move to Output Folder
                        File.Move(lockFileName, Path.Combine(outPath, Path.ChangeExtension(fileInfo.Name, ".don")));
                    }

                    documentCount--;
                    totalExportedDocument++;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return totalExportedDocument;
        }

        private string ExtractToFolder(string zipFile, string zipPath)
        {
            List<string> extractedFiles = new List<string>();
            using (ZipArchive archive = ZipFile.OpenRead(zipFile))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string extractedFile = Path.Combine(zipPath, entry.FullName);
                    entry.ExtractToFile(extractedFile);
                    extractedFiles.Add(extractedFile);
                }
            }

            return extractedFiles.Count > 0 ? extractedFiles[0] : string.Empty;
        }

    }
}
