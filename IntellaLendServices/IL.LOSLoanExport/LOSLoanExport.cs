using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.LOSLoanExport
{
    public class LOSLoanExport : IMTSServiceBase
    {
        private static string ClassificationExceptionPath = string.Empty;
        private static string ClassificationResultsPath = string.Empty;
        private static string ValidationExceptionPath = string.Empty;
        private static string LOSLoanExportPath = string.Empty;
        private static string EphesoftOutputPath = string.Empty;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            LOSLoanExportPath = Params.Find(f => f.Key == "LOSLoanExportPath").Value;
            EphesoftOutputPath = Params.Find(f => f.Key == "EphesoftOutputPath").Value;
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
                var TenantList = LOSLoanExportDataAccess.GetTenantList();
                foreach (var tenant in TenantList)
                {
                    LOSLoanExportDataAccess dataAccess = new LOSLoanExportDataAccess(tenant.TenantSchema);

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

        private void ExportStagingFiles(LOSLoanExportDataAccess dataAccess)
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
                            List<LOSExportFileStagingDetail> _docToExport = dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, LOSExportStatusConstant.LOS_LOAN_PROCESSING);

                            if (_docToExport.Count > 0)
                            {
                                string loanNumber = dataAccess.GetLoanInfo(filedetail.LoanID);

                                if (!string.IsNullOrEmpty(LOSLoanExportPath))
                                {
                                    string loanPath = Path.Combine(LOSLoanExportPath, Path.GetFileNameWithoutExtension(filedetail.FileName));

                                    //filepath = Path.Combine(loanPath, filedetail.FileName);
                                    Logger.WriteTraceLog($"FilePath : {loanPath}");

                                    if (!Directory.Exists(loanPath))
                                        Directory.CreateDirectory(loanPath);

                                    byte[] loanPDF = new ImageMinIOWrapper(dataAccess.TenantSchema).GetLoanPDF(filedetail.LoanID);

                                    foreach (LOSExportFileStagingDetail item in _docToExport.Where(x => x.FileType == LOSExportFileTypeConstant.LOS_LOAN_DOC_EXPORT).ToList())
                                    {
                                        Int32 FileStatus = LOSExportStatusConstant.LOS_LOAN_ERROR;
                                        string FileError = string.Empty;
                                        try
                                        {
                                            byte[] docPDF = CommonUtils.GetDocPDFFromLoan(loanPDF, item.Pages);
                                            File.WriteAllBytes(Path.Combine(loanPath, item.FileName), docPDF);
                                            FileStatus = LOSExportStatusConstant.LOS_LOAN_PROCESSED;
                                        }
                                        catch (Exception ex)
                                        {
                                            FileError = ex.Message;
                                            Exception newEx = new Exception($"Error while processing the document : {item.FileName}, StagingID : {item.LOSExportFileStagingID}, DocID : {item.DocID}", ex);
                                            MTSExceptionHandler.HandleException(ref newEx);
                                        }

                                        dataAccess.UpdateLOSLoanStagingDetailStatus(item.ID, FileStatus, FileError);
                                        item.Status = FileStatus;

                                    }

                                    Int32 Status = LOSExportStatusConstant.LOS_LOAN_ERROR;

                                    if (!_docToExport.Any(x => x.FileType == LOSExportFileTypeConstant.LOS_LOAN_DOC_EXPORT & x.Status != LOSExportStatusConstant.LOS_LOAN_PROCESSED))
                                    {
                                        LOSExportFileStagingDetail _jsonFile = _docToExport.Where(x => x.FileType == LOSExportFileTypeConstant.LOS_LOAN_JSON_EXPORT).FirstOrDefault();

                                        string fileJson = new LOSLoanJSONExport(dataAccess.TenantSchema).ExportJSON(filedetail.LoanID, loanPath).Replace("\\", "\\\\");

                                        Logger.WriteTraceLog($"Writing JSON File");
                                        string filepath = Path.Combine(loanPath, filedetail.FileName);
                                        File.WriteAllText(filepath, fileJson);

                                        dataAccess.UpdateLOSLoanStagingDetailStatus(_jsonFile.ID, LOSExportStatusConstant.LOS_LOAN_PROCESSED);
                                        Status = LOSExportStatusConstant.LOS_LOAN_PROCESSED;
                                    }

                                    Logger.WriteTraceLog($"Updating Staging Record");

                                    dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, Status, Status == LOSExportStatusConstant.LOS_LOAN_ERROR ? "Documents Not Exported" : "");

                                    if (Status == LOSExportStatusConstant.LOS_LOAN_PROCESSED)
                                    {
                                        Logger.WriteTraceLog($"{filedetail.ID}, {filedetail.LoanID}, {filedetail.FileType} - Updating Staging Completed");

                                        Logger.WriteTraceLog($"Ephesoft Output Folder Deletion Started for the LoanID : {filedetail.LoanID}");

                                        long[] _loanId = { filedetail.LoanID };

                                        dataAccess.UpdateLoanCompleteUserDetails(filedetail.LoanID, RoleConstant.SYSTEM_ADMINISTRATOR, 1, "Auto Complete");

                                        dataAccess.DeleteOutputFolder(_loanId, EphesoftOutputPath);

                                        Logger.WriteTraceLog($"Ephesoft Output Folder Deletion Completed");
                                    }
                                    else
                                        Logger.WriteTraceLog($"{filedetail.ID}, {filedetail.LoanID}, {filedetail.FileType} - Ended with Error");

                                }
                                else
                                    dataAccess.UpdateLOSLoanStagingFileStatus(filedetail.ID, LOSExportStatusConstant.LOS_LOAN_ERROR, $"Export path not configured");
                            }
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
