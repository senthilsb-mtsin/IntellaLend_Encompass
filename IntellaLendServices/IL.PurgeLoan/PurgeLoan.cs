using IntellaLend.Constance;
using IntellaLend.MinIOWrapper;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IL.PurgeLoan
{
    public class PurgeLoan : IMTSServiceBase
    {
        private static string exportConfigKey;
        private static string[] csvHeaders;
        private static string ephesoftOutputFolderPath = string.Empty;
        private static bool exportPDFInPurge = false;


        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            ephesoftOutputFolderPath = Params.Find(f => f.Key == "EphesoftOutputPath").Value;
            Boolean.TryParse(Params.Find(f => f.Key == "ExportPDFInPurge").Value, out exportPDFInPurge);
            exportConfigKey = "Export_Path";
            csvHeaders = new string[] { "LoanNumber", "BorrowerName", "SSN", "CustomerName", "LoanTypeName", "ServiceTypeName", "ReceivedDate", "AuditMonthYear" };
        }


        public bool DoTask()
        {
            try
            {
                List<TenantMaster> TenantLists = PurgeLoanDataAccess.GetAllTenants();
                foreach (TenantMaster tenantLists in TenantLists)
                {
                    StartPurgeLoan(tenantLists.TenantSchema);
                    DeleteAuditCompleteFiles(tenantLists.TenantSchema);
                }
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;
        }

        public void StartPurgeLoan(string tenantSchema)
        {
            PurgeLoanDataAccess _purgeLoanData = null;
            try
            {
                _purgeLoanData = new PurgeLoanDataAccess(tenantSchema);
                List<PurgeStaging> BatchIDS = _purgeLoanData.GetAllBatchIDS();
                foreach (PurgeStaging _batch in BatchIDS)
                {
                    List<bool> _lsStatus = new List<bool>();
                    List<PurgeStagingDetails> psd = _purgeLoanData.GetBatchDetails(_batch.BatchID);
                    foreach (PurgeStagingDetails psDeatils in psd)
                    {
                        try
                        {
                            Loan loan = _purgeLoanData.GetLoan(psDeatils.LoanID);

                            if (loan != null)
                            {
                                string ExportPath = _purgeLoanData.GetExportPath(loan.CustomerID, exportConfigKey);

                                string pdfExportPath = _purgeLoanData.GetPDFPath(loan.LoanID);

                                byte[] loanCSV = GetCSV(loan, _purgeLoanData);

                                bool isPDFCSVExported = MovePDFCSVFiles(ExportPath, pdfExportPath, loanCSV, loan.LoanID, psDeatils.BatchID, psDeatils.Status, _purgeLoanData);

                                _lsStatus.Add(isPDFCSVExported);

                                if (!isPDFCSVExported)
                                    throw new Exception("Files Not Created");
                            }
                            else
                                throw new Exception("Cannot find Loan");
                        }
                        catch (Exception ex)
                        {
                            _purgeLoanData.SetExceptionMessage(psDeatils.BatchID, psDeatils.LoanID, psDeatils.Status == StatusConstant.PURGE_WAITING ? StatusConstant.PURGE_FAILED : StatusConstant.EXPORT_FAILED, ex.Message);
                            MTSExceptionHandler.HandleException(ref ex);
                        }
                    }

                    Int64 status = _batch.Status == StatusConstant.PURGE_WAITING ? StatusConstant.LOAN_PURGED : StatusConstant.LOAN_EXPORTED;

                    if (_lsStatus.Where(f => f == false).ToList().Count > 0)
                        status = _batch.Status == StatusConstant.PURGE_WAITING ? StatusConstant.PURGE_FAILED : StatusConstant.EXPORT_FAILED;

                    _purgeLoanData.PSDLoanPurged(_batch.BatchID, status);

                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        private bool MovePDFCSVFiles(string exportPath, string pdfExportPath, byte[] loanCSV, long loanID, long batchID, Int64 Status, PurgeLoanDataAccess _purgeLoanData)
        {
            try
            {
                string loanPath = Path.Combine(exportPath, loanID.ToString());
                string _csvFile = Path.Combine(loanPath, loanID.ToString() + ".csv");

                string _pdfFile = Path.Combine(loanPath, $"{loanID.ToString()}.pdf");


                if (Directory.Exists(loanPath))
                    Directory.Delete(loanPath, true);

                //Prakash - Configuration added as said by Prasad in Email. Email Subject : "FW: Download Button" on "Nov 28, 2018, 3:44 PM"
                if (exportPDFInPurge)
                    Directory.CreateDirectory(loanPath);

                if (Status == StatusConstant.PURGE_WAITING)
                {
                    if (exportPDFInPurge)
                    {
                        if (File.Exists(_pdfFile))
                            File.Delete(_pdfFile);

                        File.WriteAllBytes(_pdfFile, _purgeLoanData.GetPDFBytes(loanID));
                    }

                    _purgeLoanData.DeleteLoanPDF(loanID);

                    bool isLoanImageDeleted = _purgeLoanData.DeleteLoanImages(loanID);

                    if (isLoanImageDeleted)
                    {
                        new ImageMinIOWrapper(_purgeLoanData.TenantSchema).DeleteBucket(loanID);

                        Int64 outPut = _purgeLoanData.PurgeLoanRecords(loanID);
                        {
                            bool psdStatus = _purgeLoanData.PSDLoanPurged(batchID, loanID, StatusConstant.LOAN_PURGED);
                            bool loanStatus = _purgeLoanData.LoanPurged(loanID, StatusConstant.LOAN_PURGED);
                            bool loanSearchStatus = _purgeLoanData.LoanSearchPurged(loanID, StatusConstant.LOAN_PURGED);

                            return (loanStatus && loanSearchStatus && psdStatus);
                        }

                    }

                }
                else if (Status == StatusConstant.EXPORT_WAITING)
                {
                    if (!Directory.Exists(loanPath))
                        Directory.CreateDirectory(loanPath);

                    if (File.Exists(_csvFile))
                        File.Delete(_csvFile);

                    File.WriteAllBytes(_csvFile, loanCSV);

                    if (File.Exists(_pdfFile))
                        File.Delete(_pdfFile);

                    File.WriteAllBytes(_pdfFile, _purgeLoanData.GetPDFBytes(loanID));

                    bool psdStatus = _purgeLoanData.PSDLoanPurged(batchID, loanID, StatusConstant.LOAN_EXPORTED);
                    bool loanStatus = _purgeLoanData.LoanPurged(loanID, StatusConstant.LOAN_EXPORTED);
                    bool loanSearchStatus = _purgeLoanData.LoanSearchPurged(loanID, StatusConstant.LOAN_EXPORTED);

                    return (loanStatus && loanSearchStatus && psdStatus);
                    //    _purgeLoanData.PSDLoanPurged(batchID, StatusConstant.LOAN_EXPORTED);
                    //else
                    //    throw new Exception("Status Not Updated");
                    // return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        static byte[] GetCSV(Loan loan, PurgeLoanDataAccess _purgeLoanData)
        {
            StringBuilder sb = new StringBuilder();

            if (exportPDFInPurge)
            {
                LoanSearch loanSearch = _purgeLoanData.GetLoanSearchDetails(loan.LoanID);
                string customerName = _purgeLoanData.GetCustomerName(loan.CustomerID);
                string loanTypeName = _purgeLoanData.GetLoanTypeName(loan.LoanTypeID);
                string reviewTypeName = _purgeLoanData.GetReviewTypeName(loan.ReviewTypeID);
                DateTime? dt = null;
                List<string> csvData = new List<string>();
                sb.AppendLine(String.Join(",", csvHeaders));
                csvData.Add(loanSearch != null ? loanSearch.LoanNumber : string.Empty);
                csvData.Add(loanSearch != null ? loanSearch.BorrowerName : string.Empty);
                csvData.Add(loanSearch != null ? loanSearch.SSN : string.Empty);
                csvData.Add(customerName);
                csvData.Add(loanTypeName);
                csvData.Add(loanSearch != null && loanSearch.ReceivedDate != null ? loanSearch.ReceivedDate.GetValueOrDefault().ToString("MM/dd/yyyy") : string.Empty);
                csvData.Add(reviewTypeName);
                csvData.Add(loanSearch != null && loan.AuditMonthYear != null ? loan.AuditMonthYear.ToString("MM/yyyy") : string.Empty);
                sb.AppendLine(String.Join(",", csvData));
            }

            return Encoding.ASCII.GetBytes(sb.ToString());
        }

        public void DeleteAuditCompleteFiles(string tenantSchema)
        {
            PurgeLoanDataAccess _purgeLoanData = null;
            try
            {
                _purgeLoanData = new PurgeLoanDataAccess(tenantSchema);
                List<GetLoanBatch> BatchIDS = _purgeLoanData.GetAllAuditCompleteBatchIDS();
                foreach (GetLoanBatch _lBID in BatchIDS)
                {
                    try
                    {
                        bool isFileRemoved = DeleteEphesoftFiles(ephesoftOutputFolderPath, _lBID.IDCBatchInstanceID, _purgeLoanData, _lBID.LoanId);
                    }
                    catch (Exception ex)
                    {
                        _purgeLoanData.UpdateEphesoftFolderStatus(_lBID.LoanId, StatusConstant.DELET_FILE_ERROR);
                        Exception exc = new Exception($"Exception Occured for the LoanID-{_lBID.LoanId.ToString()}", ex);
                        MTSExceptionHandler.HandleException(ref exc);
                    }
                }
            }
            catch (Exception ex)
            {

                MTSExceptionHandler.HandleException(ref ex);
            }
        }

        private bool DeleteEphesoftFiles(string pdfPath, string ephesoftBatchInstanceID, PurgeLoanDataAccess _purgeLoanData, Int64 _loanID)
        {
            try
            {
                bool isFolderDeleted = false;
                bool isFolderPendingStatus = false;
                if (pdfPath != string.Empty)
                {
                    string actualPath = Path.Combine(pdfPath, "Output", ephesoftBatchInstanceID);
                    if (actualPath != null)
                    {
                        if (Directory.Exists(actualPath))
                        {
                            isFolderPendingStatus = _purgeLoanData.UpdateEphesoftFolderStatus(_loanID, StatusConstant.DELETE_FILE_PENDING);
                            if (isFolderPendingStatus)
                            {
                                Directory.Delete(actualPath, true);
                                isFolderDeleted = _purgeLoanData.UpdateEphesoftFolderStatus(_loanID, StatusConstant.DELETE_FILE_SUCCESS);
                                return isFolderDeleted;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

}
