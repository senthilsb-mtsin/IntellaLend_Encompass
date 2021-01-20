using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;

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
                    string batch_name = string.Empty;
                    if (batchIdentifier != string.Empty)
                    {
                        string sql = "select batch_status, batch_name from batch_instance where identifier='" + batchIdentifier + "'";
                        System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
                        Logger.WriteTraceLog($"GetEphesoftBatchStatus() dt.Rows.Count : {dt.Rows.Count}");
                        if (dt.Rows.Count > 0)
                        {
                            _status = DBNull.Value.Equals(dt.Rows[0]["batch_status"]) ? string.Empty : dt.Rows[0]["batch_status"].ToString();
                            batch_name = DBNull.Value.Equals(dt.Rows[0]["batch_name"]) ? string.Empty : dt.Rows[0]["batch_name"].ToString();
                        }

                        Logger.WriteTraceLog($"GetEphesoftBatchStatus() _status : {_status}");
                        bool tempInsert = false;
                        switch (_status)
                        {
                            case "NEW":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.MOVED_TO_IDC, EphesoftConfigConstant.NEW, item.DocID, ref tempInsert);
                                break;
                            case "RUNNING":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.RUNNING, EphesoftConfigConstant.RUNNING, item.DocID, ref tempInsert);
                                break;
                            case "READY_FOR_REVIEW":
                                {
                                    bool insert = false;
                                    _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.CLASSIFICATION_WAITING, EphesoftConfigConstant.READY_FOR_REVIEW, item.DocID, ref insert);
                                    if (insert)
                                        TriggerMASExportJSON(_dataAccess, item.LoanID, batch_name, batchIdentifier, tenantSchema, LOSExportFileTypeConstant.LOS_CLASSIFICATION_EXCEPTION);
                                    break;
                                }
                            case "READY_FOR_VALIDATION":
                                {
                                    bool insert = false;
                                    _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.FIELD_EXTRACTION_WAITING, EphesoftConfigConstant.READY_FOR_VALIDATION, item.DocID, ref insert);
                                    if (insert)
                                        TriggerMASExportJSON(_dataAccess, item.LoanID, batch_name, batchIdentifier, tenantSchema, LOSExportFileTypeConstant.LOS_VALIDATION_EXCEPTION);
                                    break;
                                }
                            case "DELETED":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.IDC_DELETED, EphesoftConfigConstant.DELETED, item.DocID, ref tempInsert);
                                break;
                            case "FINISHED":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.EXTRACTION_COMPLETED, EphesoftConfigConstant.FINISHED, item.DocID, ref tempInsert);
                                break;
                            case "ERROR":
                                _dataAccess.UpdateLoanSubStatus(item.LoanID, StatusConstant.SUBSTATUS_OCR_ERROR, EphesoftConfigConstant.ERROR, item.DocID, ref tempInsert);
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

        private void TriggerMASExportJSON(LoanStatusUpdateDataAccess _dataAccess, Int64 LoanID, string BatchName, string BatchID, string tenantSchema, Int32 MASExportFileType)
        {
            string baseURL = ConfigurationManager.AppSettings["BASE_URI"] + "EphesoftInterface";
            using (var handler = new HttpClientHandler() { })
            using (var client = new HttpClient(handler))
            {
                EphsoftLOSExportFileStagingRequest request = new EphsoftLOSExportFileStagingRequest();
                request.LoanID = LoanID;
                request.FileType = MASExportFileType;
                request.TableSchema = tenantSchema;
                request.MASDocumentList = new List<MASDocument>();
                request.BatchID = BatchID;
                request.BatchName = BatchName;
                request.BCName = string.Empty;
                string cont = JsonConvert.SerializeObject(request);
                HttpResponseMessage httpres = client.PostAsync(baseURL + "/UpdateLOSExportFileStaging", new StringContent(cont, Encoding.UTF8, "application/json")).Result;
                //dynamic objres = httpres.Content.ReadAsStringAsync().Result;
                //if (objres.ResponseMessage != null && !string.IsNullOrEmpty(objres.ResponseMessage.MessageDesc))
                //    throw new Exception(objres.ResponseMessage.MessageDesc);
            }
        }
    }

    public class PendingOCRBatches
    {
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
        public string BatchInstancesID { get; set; }
    }

    public class EphsoftLOSExportFileStagingRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public int FileType { get; set; }
        public string BatchID { get; set; }
        public string BatchName { get; set; }
        public string BatchClassID { get; set; }
        public string BCName { get; set; }
        public List<MASDocument> MASDocumentList { get; set; }
    }

}
