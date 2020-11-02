using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;

namespace IL.LoanStatusUpdate
{
    public class LoanStatusUpdate : IMTSServiceBase
    {
        private bool logTracing = false;
        private static string IntellaLendLoanUploadPath = string.Empty;
        public void OnStart(string ServiceParam)
        {
            //var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            //IntellaLendLoanUploadPath = Params.Find(f => f.Key == "IntellaLendLoanUploadPath").Value;
            // Boolean.TryParse(ConfigurationManager.AppSettings["LoanStatusUpdate"].ToLower(), out logTracing);
        }

        public bool DoTask()
        {
            try
            {
                Logger.WriteTraceLog("DoTask()");
                List<TenantMaster> TenantLists = LoanStatusUpdateDataAccess.GetAllTenants();
                foreach (TenantMaster tenantLists in TenantLists)
                {
                    Logger.WriteTraceLog($"DoTask() tenantLists : {tenantLists.TenantSchema}");
                    GetEphesoftBatchStatus(tenantLists.TenantSchema);
                    Logger.WriteTraceLog($"DoTask() Completed tenantLists : {tenantLists.TenantSchema}");

                }
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;

        }

        public void GetEphesoftBatchStatus(string tenantSchema)
        {
            try
            {
                LoanStatusUpdateDataAccess _dataAccess = new LoanStatusUpdateDataAccess(tenantSchema);
                List<PendingOCRBatches> loan = _dataAccess.GetPendingIDCLoans();
                Logger.WriteTraceLog($"GetEphesoftBatchStatus() loan.count : {loan.Count}");
                foreach (var item in loan)
                {
                    Logger.WriteTraceLog($"GetEphesoftBatchStatus() Loan : {item.LoanID} , BatchID : {item.BatchInstancesID}");
                    string batchIdentifier = item.BatchInstancesID; //_dataAccess.GetBatchIdentifier(item.LoanID);
                    string _status = string.Empty;
                    if (batchIdentifier != string.Empty)
                    {
                        string sql = "select batch_status from batch_instance where identifier='" + batchIdentifier + "'";
                        System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
                        Logger.WriteTraceLog($"GetEphesoftBatchStatus() dt.Rows.Count : {dt.Rows.Count}");
                        if (dt.Rows.Count > 0)
                        {
                            _status = DBNull.Value.Equals(dt.Rows[0][0]) ? string.Empty : dt.Rows[0][0].ToString();
                        }

                        Logger.WriteTraceLog($"GetEphesoftBatchStatus() _status : {_status}");
                        switch (_status)
                        {
                            case "NEW":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.MOVED_TO_IDC, EphesoftConfigConstant.NEW, item.DocID);
                                break;
                            case "RUNNING":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.RUNNING, EphesoftConfigConstant.RUNNING, item.DocID);
                                break;
                            case "READY_FOR_REVIEW":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.CLASSIFICATION_WAITING, EphesoftConfigConstant.READY_FOR_REVIEW, item.DocID);
                                break;
                            case "READY_FOR_VALIDATION":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.FIELD_EXTRACTION_WAITING, EphesoftConfigConstant.READY_FOR_VALIDATION, item.DocID);
                                break;
                            case "DELETED":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.IDC_DELETED, EphesoftConfigConstant.DELETED, item.DocID);
                                break;
                            case "FINISHED":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.EXTRACTION_COMPLETED, EphesoftConfigConstant.FINISHED, item.DocID);
                                break;
                            case "ERROR":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.SUBSTATUS_OCR_ERROR, EphesoftConfigConstant.ERROR, item.DocID);
                                break;
                            default:
                                break;
                        }

                        Logger.WriteTraceLog($"GetEphesoftBatchStatus() Updated : {item.LoanID} , BatchID : {item.BatchInstancesID}");
                    }

                }
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }
    }

    public class PendingOCRBatches
    {
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
        public string BatchInstancesID { get; set; }
    }
}
