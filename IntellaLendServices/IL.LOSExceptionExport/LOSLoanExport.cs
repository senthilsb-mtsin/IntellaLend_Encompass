using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.LOSExceptionExport
{
    public class LOSExceptionExport : IMTSServiceBase
    {
        private static string ClassificationExceptionPath = string.Empty;
        private static string ClassificationResultsPath = string.Empty;
        private static string ValidationExceptionPath = string.Empty;
        private static string[] ExportPath = { "", "", "" };

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            ClassificationExceptionPath = Params.Find(f => f.Key == "ClassificationExceptionPath").Value;
            ClassificationResultsPath = Params.Find(f => f.Key == "ClassificationResultsPath").Value;
            ValidationExceptionPath = Params.Find(f => f.Key == "ValidationExceptionPath").Value;
            ExportPath[0] = ClassificationExceptionPath;
            ExportPath[1] = ClassificationResultsPath;
            ExportPath[2] = ValidationExceptionPath;
        }

        public bool DoTask()
        {
            try
            {
                ProcessFileCreation();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return true;
        }

        private bool ProcessFileCreation()
        {
            Logger.WriteTraceLog($"Start ProcessFileCreation()");
            try
            {
                var TenantList = LOSExceptionExportDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    LOSExceptionExportDataAccess dataAccess = new LOSExceptionExportDataAccess(tenant.TenantSchema);

                    ExportStagingFiles(dataAccess);
                }
                Logger.WriteTraceLog($"End ProcessFileCreation()");
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return true;
        }

        private void ExportStagingFiles(LOSExceptionExportDataAccess dataAccess)
        {
            try
            {
                Logger.WriteTraceLog($"Start ExportStagingFiles() dataAccess: {dataAccess} ");
                List<LOSExportFileStaging> _stagingfiles = dataAccess.GetLOSLoanStagingFiles();

                if (_stagingfiles != null && _stagingfiles.Count > 0)
                {
                    foreach (var filedetail in _stagingfiles)
                    {
                        try
                        {
                            Logger.WriteTraceLog($"{filedetail.ID}, {filedetail.LoanID}, {filedetail.FileType} - Processing");
                            dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, LOSExportStatusConstant.LOS_LOAN_PROCESSING);

                            string fileexportpath = string.Empty;
                            string filepath = string.Empty;

                            fileexportpath = ExportPath[filedetail.FileType - 1];

                            if (!string.IsNullOrEmpty(fileexportpath))
                            {
                                filepath = Path.Combine(fileexportpath, filedetail.FileName);
                                Logger.WriteTraceLog($"FilePath : {filepath}");

                                if (!Directory.Exists(fileexportpath))
                                    Directory.CreateDirectory(fileexportpath);

                                Logger.WriteTraceLog($"Writing JSON File");
                                File.WriteAllText(filepath, filedetail.FileJson);

                                Logger.WriteTraceLog($"Updating Staging Record");
                                dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, LOSExportStatusConstant.LOS_LOAN_PROCESSED);
                                Logger.WriteTraceLog($"{filedetail.ID}, {filedetail.LoanID}, {filedetail.FileType} - Processed");
                            }
                            else
                                dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, LOSExportStatusConstant.LOS_LOAN_ERROR, $"Export path not configured");
                        }
                        catch (Exception ex)
                        {
                            dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, LOSExportStatusConstant.LOS_LOAN_ERROR, ex.Message);
                            MTSExceptionHandler.HandleException(ref ex);
                        }
                    }
                }
                Logger.WriteTraceLog($"End ExportStagingFiles() dataAccess: {dataAccess} ");
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }
    }
}
