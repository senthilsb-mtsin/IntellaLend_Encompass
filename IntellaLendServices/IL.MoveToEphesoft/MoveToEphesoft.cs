using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IL.MoveToEphesoft
{
    public class MoveToEphesoft : IMTSServiceBase
    {
        // private static string TiffProcessingPath = string.Empty;
        private static string EphesoftInputPath = string.Empty;
        private string BatchStatus = string.Empty;
        private int EphesoftTotalCore = 1;
        private bool logTracing = false;

        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            // TiffProcessingPath = Params.Find(f => f.Key == "TiffProcessingPath").Value;
            EphesoftInputPath = Params.Find(f => f.Key == "EphesoftInputPath").Value;
            BatchStatus = Params.Find(f => f.Key == "BatchStatus").Value;
            Int32.TryParse(Params.Find(f => f.Key == "EphesoftTotalCore").Value, out EphesoftTotalCore);
            Boolean.TryParse(ConfigurationManager.AppSettings["MoveToEphesoftLog"].ToLower(), out logTracing);
        }

        public bool DoTask()
        {
            try
            {
                return MoveProcessedLoans();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;
        }

        #region Private Methods

        private bool MoveProcessedLoans()
        {
            try
            {
                Int32 _ephesoftEmptySlotCount = GetEphesoftSlotCount();
                if (_ephesoftEmptySlotCount > 0)
                {
                    LogMessage($"Ephesoft Empty Slot Count : {_ephesoftEmptySlotCount.ToString()}");
                    List<ExportProcessingQueue> _waitingToBeExported = MoveToEphesoftDataAccess.GetWaitingProcessedBatchCount(_ephesoftEmptySlotCount);
                    LogMessage($"Waiting To Be Exported Count : {_waitingToBeExported.Count.ToString()}");
                    if (_ephesoftEmptySlotCount > 0 && _waitingToBeExported.Count > 0)
                        MoveProcessedLoansToEphesoft(_waitingToBeExported);
                }

            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }

            return true;
        }

        private void MoveProcessedLoansToEphesoft(List<ExportProcessingQueue> _queueLoans)
        {
            foreach (ExportProcessingQueue queueLoan in _queueLoans)
            {
                Int64 LoanID = 0, DocID = 0;
                Int32 Status = 2;
                string ErrorMessage = string.Empty;
                MoveToEphesoftDataAccess.UpdateExportProcessingQueueError(queueLoan.ProcessingQueueID, 99, ErrorMessage); // 99 = Loan Picked Status
                try
                {
                    string fileName = Path.GetFileName(queueLoan.DestinationPath);
                    LoanID = Convert.ToInt64(fileName.Split('_')[1]);
                    if (fileName.Split('_').Length > 2)
                        DocID = Convert.ToInt64(fileName.Split('_')[2]);

                    string _destPath = Path.Combine(EphesoftInputPath, "Input", fileName);
                    //MoveToEphesoftDataAccess.GetEphesoftBatchInputFolder(queueLoan.TenantSchema, Convert.ToInt64(fileName.Split('_')[1]));
                    //LogMessage($"fileName : {fileName}, _destPath: {_destPath}");                  
                    if (!string.IsNullOrEmpty(_destPath))
                        Directory.Move(queueLoan.DestinationPath, _destPath);
                }
                catch (Exception ex)
                {
                    Status = -1;
                    ErrorMessage = ex.Message;
                    MTSExceptionHandler.HandleException(ref ex);
                }

                if (Status == -1)
                    MoveToEphesoftDataAccess.UpdateLoanStatus(queueLoan.TenantSchema, LoanID, StatusConstant.IDC_ERROR, ErrorCodeConstant.MOVE_TO_IDC_FAILED, DocID);
                else
                    MoveToEphesoftDataAccess.UpdateLoanStatus(queueLoan.TenantSchema, LoanID, StatusConstant.PENDING_IDC, DocID);

                MoveToEphesoftDataAccess.UpdateExportProcessingQueueError(queueLoan.ProcessingQueueID, Status, ErrorMessage);
            }
        }

        private int GetEphesoftSlotCount()
        {
            int totalRunningBatch = MoveToEphesoftDataAccess.GetEphesoftPendingBatchCount(BatchStatus);

            if (totalRunningBatch > EphesoftTotalCore)
                return 0;

            return (EphesoftTotalCore - totalRunningBatch);
        }

        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

        #endregion
    }
}
