using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Configuration;
using MTSEntBlocks.UtilsBlock;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;
using IntellaLend.Constance;
using System.Web;
using System.Reflection;

namespace IL.LoanBatchExport
{
    public class LoanBatchExport : IMTSServiceBase
    {
        private static string BatchExportPath = string.Empty;
        private string BatchStatus = string.Empty;
        private int EphesoftTotalCore = 1;
        private string TemplatePath = string.Empty;
        private bool logTracing = false;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            // TiffProcessingPath = Params.Find(f => f.Key == "TiffProcessingPath").Value;
            BatchExportPath = Params.Find(f => f.Key == "BatchExportPath").Value;
            TemplatePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "CoverLetterTemplate", "CoverLetterTemplate.html");
            //BatchStatus = Params.Find(f => f.Key == "BatchStatus").Value;
            //Int32.TryParse(Params.Find(f => f.Key == "EphesoftTotalCore").Value, out EphesoftTotalCore);
             Boolean.TryParse(ConfigurationManager.AppSettings["LoanBatchExport"].ToLower(), out logTracing);
        }

        public bool DoTask()
        {
            try
            {
                return ProcessBatchLoans();
            }
            catch (Exception ex)
            {
                BaseExceptionHandler.HandleException(ref ex);
            }
            return false;
        }
        private bool ProcessBatchLoans()
        {
            LoanBatchExportDataAccess dataAccess = null;
            try
            {
                var TenantList = LoanBatchExportDataAccess.GetTenantList();

                foreach (var tenant in TenantList)
                {
                    dataAccess = new LoanBatchExportDataAccess(tenant.TenantSchema);
                    // Int32 _batchCountToExport = GetDocumentExportCount();
                    //if (_batchCountToExport > 0)
                    ExportBatchDocuments(dataAccess, tenant);
                }
                return true;
            }
            catch (Exception ex)
            {
                BaseExceptionHandler.HandleException(ref ex);
                return false;
            }
            return false;
        }

        private void ExportBatchDocuments(LoanBatchExportDataAccess dataAccess, TenantMaster tenant)
        {
            List<IntellaLend.Model.LoanBatchExport> _loanBatch = dataAccess.GetLoanBatches();

            LogMessage($"Batch Files to be Exported : {_loanBatch.Count.ToString()}");
            Int64 _batchId = 0;
            try
            {
                foreach (var _batchDetails in _loanBatch)
                {
                    _batchId = _batchDetails.BatchID;
                    dataAccess.UpdateLoanBatchExport(_batchDetails.BatchID, StatusConstant.PROCESSING_BATCH, string.Empty);
                    List<LoanBatchExportDetail> _loanBatchExportDetail = dataAccess.GetLoanBatchExportDetails(_batchDetails.BatchID);
                    string ExportPath = Path.Combine(BatchExportPath, _batchDetails.BatchName);
                    string coverLetterFilePath = Path.Combine(ExportPath, _batchDetails.BatchName + "_CoverLetter" + ".pdf");
                    if (_batchDetails.CoverLetter)
                    {
                        //var _coverLetterContent = JsonConvert.DeserializeObject(_batchDetails.CoverLetterContent);
                        CreateCoverLetter(ExportPath,coverLetterFilePath, _batchDetails);
                    }
                    foreach (var _loanBatchDocs in _loanBatchExportDetail)
                    {
                        string loanDetails = dataAccess.GetLoanDetailsObject(_loanBatchDocs.LoanID);
                        Batch _batchDocuments = JsonConvert.DeserializeObject<Batch>(loanDetails);
                        string docDetails = _loanBatchDocs.LoanDocumentConfig;//dataAccess.GetDocDetailsObject(loanId);
                        List<DocumentDetails> docs = JsonConvert.DeserializeObject<List<DocumentDetails>>(docDetails);

                        List<TOCDetails> _tocDetails = new List<TOCDetails>();
                        foreach (var item in docs)
                        {
                            Documents doc = _batchDocuments.Documents.Where(a => a.DocumentTypeID == item.DocumentTypeID && item.VersionNumber == a.VersionNumber).FirstOrDefault();

                            _tocDetails.Add(new TOCDetails
                            {
                                StartingPage = (Convert.ToInt32(doc.Pages[0]) + 1),
                                Type = doc.Type
                            });
                            //doc.Type
                        }

                        byte[] originalLoanPDF = dataAccess.GetLoanPDF(_loanBatchDocs.LoanID);
                        string loanNumber = dataAccess.GetLoanNumber(_loanBatchDocs.LoanID);
                        //byte[] TocPdf = CommonUtils.CreateTocPDF("Index", originalLoanPDF, _tocDetails);

                        byte[] TocPdf = CommonUtils.CreateConfiguredPDF(originalLoanPDF, _batchDetails, _tocDetails);

                        
                        string filePath = Path.Combine(ExportPath, loanNumber + ".pdf");
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
                        dataAccess.UpdateLoanExportDetailStatus(_loanBatchDocs.LoanID, filePath);
                    }
                    dataAccess.UpdateLoanBatchExport(_batchDetails.BatchID, StatusConstant.BATCH_EXPORTED, BatchExportPath);
                }
            }
            catch (Exception exception)
            {
                dataAccess.UpdateLoanBatchExportErrorStatus(_batchId, StatusConstant.BATCH_ERROR, exception);
            }
        }

        private void CreateCoverLetter(string exportPath,string coverLetterPath, IntellaLend.Model.LoanBatchExport _batch)
        {
            string htmlTemplate = File.ReadAllText(TemplatePath);
            StringBuilder sb = new StringBuilder();
           
            string jonString = _batch.CoverLetterContent.Replace("\\n", "<br>");
            BatchConfigDetails _coverLetterContent = JsonConvert.DeserializeObject<BatchConfigDetails>(jonString);
            htmlTemplate= htmlTemplate.Replace("@To", _coverLetterContent.To);
            htmlTemplate = htmlTemplate.Replace("@Subject", _coverLetterContent.Subject);
            htmlTemplate= htmlTemplate.Replace("@Body", _coverLetterContent.Body);
            string [] sbElements = htmlTemplate.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i=0 ;i < sbElements.Length -1;i++)
            {
                sb.Append(sbElements[i]);
            }
            
            byte[] coverLetterPdf = CommonUtils.CreateCoverLetter(sb, _batch);

            if(Directory.Exists(exportPath))
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
            if (logTracing)
            {
                Exception ex = new Exception(_msg);
                BaseExceptionHandler.HandleException(ref ex);
            }
        }
    }
}
