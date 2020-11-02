using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntBlocks.UtilsBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace IL.LoanJobsExport
{
    public class LoanJobsExport : IMTSServiceBase
    {
        private static string JobExportPath = string.Empty;
        private string BatchStatus = string.Empty;
        private int EphesoftTotalCore = 1;
        private string TemplatePath = string.Empty;
        private bool logTracing = false;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            // TiffProcessingPath = Params.Find(f => f.Key == "TiffProcessingPath").Value;
            JobExportPath = Params.Find(f => f.Key == "BatchExportPath").Value;
            TemplatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "CoverLetterTemplate", "CoverLetterTemplate.html");
            //BatchStatus = Params.Find(f => f.Key == "BatchStatus").Value;
            //Int32.TryParse(Params.Find(f => f.Key == "EphesoftTotalCore").Value, out EphesoftTotalCore);
            Boolean.TryParse(ConfigurationManager.AppSettings["LoanJobExport"].ToLower(), out logTracing);
        }

        public bool DoTask()
        {
            try
            {
                return ProcessBatchLoans();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;
        }
        private bool ProcessBatchLoans()
        {
            LoanJobExportDataAccess dataAccess = null;
            try
            {
                var TenantList = LoanJobExportDataAccess.GetTenantList();

                foreach (var tenant in TenantList)
                {
                    dataAccess = new LoanJobExportDataAccess(tenant.TenantSchema);
                    // Int32 _batchCountToExport = GetDocumentExportCount();
                    //if (_batchCountToExport > 0)
                    ExportJobDocuments(dataAccess, tenant);
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

        private void ExportJobDocuments(LoanJobExportDataAccess dataAccess, TenantMaster tenant)
        {
            List<LoanJobExport> _loanJob = dataAccess.GetLoanBatches();

            LogMessage($"Job Files to be Exported : {_loanJob.Count.ToString()}");
            Int64 _jobId = 0;
            try
            {
                foreach (var _batchDetails in _loanJob)
                {
                    string jobLevelErrorMessage = "";
                    try
                    {
                        _jobId = _batchDetails.JobID;
                        dataAccess.UpdateLoanJobExport(_batchDetails.JobID, StatusConstant.PROCESSING_JOB, string.Empty);
                        List<LoanJobExportDetail> _loanJobExportDetail = dataAccess.GetLoanBatchExportDetails(_batchDetails.JobID);
                        string CurrentDateFolder = DateTime.Now.ToString("MMddyyyy");
                        string ExportPath = Path.Combine(JobExportPath, CurrentDateFolder, _batchDetails.JobName);
                        string coverLetterFilePath = Path.Combine(ExportPath, _batchDetails.JobName + "_CoverLetter" + ".pdf");
                        if (_batchDetails.CoverLetter)
                        {
                            //var _coverLetterContent = JsonConvert.DeserializeObject(_batchDetails.CoverLetterContent);
                            if(string.IsNullOrEmpty(_batchDetails.ExportPath))
                            {
                                CreateCoverLetter(ExportPath, coverLetterFilePath, _batchDetails);
                            }
                            
                        }
                        foreach (var _loanJobDocs in _loanJobExportDetail)
                        {
                            string LoanLevelErrorMessage = "";
                            try
                            {
                                string loanDetails = dataAccess.GetLoanDetailsObject(_loanJobDocs.LoanID);
                                Batch _batchDocuments = JsonConvert.DeserializeObject<Batch>(loanDetails);
                                string docDetails = _loanJobDocs.LoanDocumentConfig;//dataAccess.GetDocDetailsObject(loanId);
                                List<DocumentDetails> docs = JsonConvert.DeserializeObject<List<DocumentDetails>>(docDetails);
                                docs = docs.Distinct().ToList();
                                // List<TOCDetails> _tocDetails = new List<TOCDetails>();
                                //List< PageDetailsAndDescription >_pageDetails = new List<PageDetailsAndDescription>();
                                Dictionary<string, List<int>> _pageDetailDictionary = new Dictionary<string, List<int>>();
                                List<int[]> _docPageArray = new List<int[]>();
                                foreach (var item in docs)
                                {
                                    Documents doc = _batchDocuments.Documents.Where(a => a.DocumentTypeID == item.DocumentTypeID && item.VersionNumber == a.VersionNumber).FirstOrDefault();
                                    if (doc != null)
                                    {
                                        if (doc.Pages != null && doc.Pages.Count > 0)
                                        {
                                            string docName = "";
                                            int isVersioned = docs.FindAll(a => a.DocumentTypeID == item.DocumentTypeID).Count();
                                            if (isVersioned > 1)
                                            {
                                                docName = doc.Description + " - V" + doc.VersionNumber;
                                            }
                                            else
                                            {
                                                docName = doc.Description;
                                            }
                                            if (!_pageDetailDictionary.ContainsKey(docName))
                                            {
                                            LogMessage($"DocType : {doc.Type},PageNumber : {doc.Pages.Count},VersionNumber : {doc.VersionNumber}");
                                            string typeOfDoc = doc.Pages[0].GetType().Name;
                                            string[] docPages = null;
                                            List<int> _pages = new List<int>();
                                            object page = null;

                                            for (int i = 0; i < doc.Pages.Count; i++)
                                            {
                                                docPages = doc.Pages[i].Split(new string[] { "PG" }, StringSplitOptions.None);
                                                if (docPages.Length > 1)
                                                {
                                                    page = docPages[1];
                                                }
                                                else
                                                {
                                                    page = docPages[0];
                                                }
                                                _pages.Add(Convert.ToInt32(page));
                                            }
                                            int _isVersioned = docs.FindAll(a => a.DocumentTypeID == item.DocumentTypeID).Count();
                                            if (_isVersioned > 1)
                                            {
                                                _pageDetailDictionary.Add(doc.Description + " - V" + doc.VersionNumber, _pages);
                                            }
                                            else
                                            {
                                                _pageDetailDictionary.Add(doc.Description, _pages);
                                            }

                                                _pageDetailDictionary[docName] = _pages;

                                                LogMessage($"DocumentNamePlaceHolder - {docName} : [{String.Join(",",_pages)}]");
                                            }

                                            //_docPageArray.Add(_pages);
                                            //docPages = doc.Pages[0].Split(new string[] { "PG" }, StringSplitOptions.None);
                                            //if (docPages.Length > 1)
                                            //{
                                            //    page = docPages[1];
                                            //}
                                            //else
                                            //{
                                            //    page = doc.Pages[0];
                                            //}

                                            //_tocDetails.Add(new TOCDetails
                                            //{
                                            //    StartingPage = (Convert.ToInt32(page) + 1),
                                            //    Type = doc.Description
                                            //});
                                        }
                                        else
                                        {
                                            LoanLevelErrorMessage += ($"Page Number Not Found For DocumentTypeID  :" + doc.DocumentTypeID);
                                            throw new Exception(LoanLevelErrorMessage);
                                        }
                                    }
                                    else
                                    {
                                        LoanLevelErrorMessage += ($"Document Not Found  :" + item.DocumentTypeID);
                                        throw new Exception(LoanLevelErrorMessage);
                                    }
                                    //doc.Type
                                }
                                byte[] originalLoanPDF = dataAccess.GetLoanPDF(_loanJobDocs.LoanID);
                                string _footerText = dataAccess.GetPDFFooterName();
                                if (!string.IsNullOrEmpty(_footerText) && originalLoanPDF != null)
                                {
                                    originalLoanPDF = CommonUtils.CreateHeaderFooterPDF(originalLoanPDF, _footerText);
                                }
                                ReArrangePDF _reArrange = CommonUtils.ReArrrange(_pageDetailDictionary, originalLoanPDF);

                                if (originalLoanPDF.Length > 0)
                                {
                                    byte[] TocPdf = null;
                                    string loanNumber = dataAccess.GetLoanNumber(_loanJobDocs.LoanID);
                                    //byte[] TocPdf = CommonUtils.CreateTocPDF("Index", originalLoanPDF, _tocDetails);

                                    if (_batchDetails.PasswordProtected || _batchDetails.TableOfContent)
                                    {
                                        TocPdf = CommonUtils.CreateConfiguredPDF(_reArrange.TocPDF, _batchDetails, _reArrange._tocDetails);

                                    }

                                    else
                                    {
                                        TocPdf = _reArrange.TocPDF;
                                    }
                                    string timeStamp = DateTime.Now.ToString("MMddyyyyHHmmssFFF");
                                    string fileName = "";
                                    string filePath = "";
                                    fileName = loanNumber + "_" + timeStamp + ".pdf";
                                    if (!string.IsNullOrEmpty(_batchDetails.ExportPath))
                                    {
                                        ExportPath = _batchDetails.ExportPath;
                                        filePath = Path.Combine(_batchDetails.ExportPath, fileName);
                                    }
                                    else
                                    {
                                        filePath = Path.Combine(ExportPath, fileName);
                                    }
                                    LogMessage($"Checking File Path :{filePath}");
                                    if (Directory.Exists(ExportPath))
                                    {
                                        File.WriteAllBytes(filePath, TocPdf);
                                    }
                                    else
                                    {
                                        Directory.CreateDirectory(ExportPath);
                                        File.WriteAllBytes(filePath, TocPdf);
                                    }
                                    dataAccess.UpdateLoanExportDetailStatus(_loanJobDocs.LoanID, _loanJobDocs.JobID, fileName, filePath, StatusConstant.FILE_CREATION_SUCCESS);
                                }
                                else
                                {
                                    LoanLevelErrorMessage += ($"Image Not Found For LoanID  :" + _loanJobDocs.LoanID);
                                    throw new Exception(LoanLevelErrorMessage);
                                }

                            }
                            catch (Exception ex)
                            {
                                //dataAccess.UpdateLoanBatchExportErrorStatus(_batchDetails.JobID,StatusConstant.FILE_CREATION_FAILED);

                                dataAccess.UpdateLoanExportDetailStatus(_loanJobDocs.LoanID, _loanJobDocs.JobID, ex);
                                MTSExceptionHandler.HandleException(ref ex);
                                continue;
                            }
                        }
                        int _loanExportCount = dataAccess.CheckAllLoansExported(_batchDetails.JobID);

                        if (_loanExportCount > 0 && _loanExportCount == _batchDetails.LoanCount)
                        {
                            if (string.IsNullOrEmpty(_batchDetails.ExportPath))
                            {
                                dataAccess.UpdateLoanJobExport(_batchDetails.JobID, StatusConstant.JOB_EXPORTED, ExportPath);
                            }
                            else
                            {
                                dataAccess.UpdateLoanJobExport(_batchDetails.JobID, StatusConstant.JOB_EXPORTED, _batchDetails.ExportPath);
                            }
                        }
                        else if (_loanExportCount > 0 && _loanExportCount < _batchDetails.LoanCount)
                        {
                            jobLevelErrorMessage = $"Associated Loans Are Partially Exported For Job ID : {_batchDetails.JobName}";
                            dataAccess.UpdateLoanBatchExportErrorStatus(_batchDetails.JobID, StatusConstant.JOB_ERROR, jobLevelErrorMessage, ExportPath);
                        }
                        else
                        {
                            jobLevelErrorMessage = "Loan(s) failed to exported";
                            dataAccess.UpdateLoanBatchExportErrorStatus(_batchDetails.JobID, StatusConstant.JOB_ERROR, jobLevelErrorMessage);
                        }

                        //int _loanPendingCount =  dataAccess.CheckAllLoansExported(_batchDetails.JobID);
                        //  if (_loanPendingCount == 0)
                        //  {
                        //      dataAccess.UpdateLoanJobExport(_batchDetails.JobID, StatusConstant.JOB_EXPORTED, ExportPath);

                        //  }
                        //  else
                        //  {
                        //      jobLevelErrorMessage += ($"Associated Loans Are Partially Exported For Job ID :" + _batchDetails.JobName);
                        //      throw new Exception(jobLevelErrorMessage); 
                        //  }
                    }
                    catch (Exception ex)
                    {

                        //docLevelErrorMessage += ($"Job Failed" + _batchDetails.JobID + ",");
                        dataAccess.UpdateLoanBatchExportErrorStatus(_batchDetails.JobID, StatusConstant.JOB_ERROR, ex);
                        MTSExceptionHandler.HandleException(ref ex);
                        continue;

                    }
                }
            }
            catch (Exception exception)
            {
                dataAccess.UpdateLoanBatchExportErrorStatus(_jobId, StatusConstant.JOB_ERROR, exception);
            }
        }

        private void CreateCoverLetter(string exportPath, string coverLetterPath, LoanJobExport _batch)
        {
            string htmlTemplate = File.ReadAllText(TemplatePath);
            StringBuilder sb = new StringBuilder();

            string jonString = _batch.CoverLetterContent.Replace("\\n", "<br>");
            JobConfigDetails _coverLetterContent = JsonConvert.DeserializeObject<JobConfigDetails>(jonString);
            htmlTemplate = htmlTemplate.Replace("@To", _coverLetterContent.To);
            htmlTemplate = htmlTemplate.Replace("@Subject", _coverLetterContent.Subject);
            htmlTemplate = htmlTemplate.Replace("@Body", _coverLetterContent.Body);
            string[] sbElements = htmlTemplate.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < sbElements.Length - 1; i++)
            {
                sb.Append(sbElements[i]);
            }

            byte[] coverLetterPdf = CommonUtils.CreateCoverLetter(sb, _batch);

            if (Directory.Exists(exportPath))
            {
                File.WriteAllBytes(coverLetterPath, coverLetterPdf);
            }
            else
            {
                Directory.CreateDirectory(exportPath);
                File.WriteAllBytes(coverLetterPath, coverLetterPdf);
            }

        }

        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }
    }
}
