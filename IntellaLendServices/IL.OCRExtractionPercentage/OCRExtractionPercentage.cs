using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace IL.OCRExtractionPercentage
{
    public class OCRExtractionPercentage : IMTSServiceBase
    {
        #region Private Variables


        #endregion

        #region Service Start DoTask

        public void OnStart(string ServiceParam)
        {
            // var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();        
        }

        public bool DoTask()
        {
            bool result = false;

            result = GetExtractionPercentage();

            return result;
        }

        #endregion

        #region Private Methods

        private bool GetExtractionPercentage()
        {
            try
            {
                var TenantList = OCRAccuracyDataAccess.GetTenantList();

                foreach (var tenant in TenantList)
                {
                    OCRAccuracyDataAccess _dataAccess = new OCRAccuracyDataAccess(tenant.TenantSchema);

                    List<GetLoanBatch> _waitingOCRPercentage = _dataAccess.GetWaitingOCRPercentageLoans();

                    //logMessage($"_waitingOCRPercentage : {_waitingOCRPercentage.Count.ToString()}");

                    foreach (GetLoanBatch _loan in _waitingOCRPercentage)
                    {
                        //logMessage($"_loan : {_loan.EphesoftBatchInstanceID.ToString()}");
                        string _sql = string.Empty;
                        string _classificationSql = string.Empty;
                        bool loanUpdated=false;
                        bool loanDetailUpdated=false;
                        bool classificationLoanUpdated = false;
                     

                        try
                        {
                            _sql = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GetLoanExtractionPercentage.sql"));

                            _sql = _sql.Replace("{BATCHINSTANCEID}", _loan.IDCBatchInstanceID.Trim());

                            _classificationSql = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "GetLoanClassificationPercentage.sql"));

                            _classificationSql = _classificationSql.Replace("{BATCHINSTANCEID}", _loan.IDCBatchInstanceID.Trim());

                            DataSet _ds = OCRAccuracyDataAccess.GetOCRAccuracy(_sql);

                            if (_ds.Tables.Count > 1 && _ds.Tables[0].Rows.Count > 0 && _ds.Tables[1].Rows.Count > 0)
                            {
                                //logMessage($"(_ds.Tables.Count > 1 && _ds.Tables[0].Rows.Count > 0 && _ds.Tables[1].Rows.Count > 0) : {(_ds.Tables.Count > 1 && _ds.Tables[0].Rows.Count > 0 && _ds.Tables[1].Rows.Count > 0)}");

                                decimal _percentage = 0m;
                                decimal.TryParse(Convert.ToString(_ds.Tables[0].Rows[0]["BATCH_PERCENTAGE"]), out _percentage);
                                //logMessage($"_loanID : {_loan.LoanID.ToString()}");
                                 loanUpdated = _dataAccess.UpdateLoanEphesoftAccuracy(_loan.LoanId, _percentage);
                                 loanDetailUpdated = false;
                                if (loanUpdated && _ds.Tables[1] != null)
                                    loanDetailUpdated = _dataAccess.UpdateLoanDocumentAccuracy(_loan.LoanId, _ds.Tables[1]);

                               
                                //logMessage($"(loanUpdated && loanDetailUpdated) : {(loanUpdated && loanDetailUpdated)}");

                            }

                            DataSet classificationDs = OCRAccuracyDataAccess.GetOCRAccuracy(_classificationSql);

                            if (classificationDs.Tables.Count > 0  && classificationDs.Tables[0].Rows.Count > 0 )
                            {
                                //logMessage($"(_ds.Tables.Count > 1 && _ds.Tables[0].Rows.Count > 0 && _ds.Tables[1].Rows.Count > 0) : {(_ds.Tables.Count > 1 && _ds.Tables[0].Rows.Count > 0 && _ds.Tables[1].Rows.Count > 0)}");
                               
                                decimal _percentage = 0m;
                                decimal.TryParse(Convert.ToString(classificationDs.Tables[0].Rows[0]["BATCH_PERCENTAGE"]), out _percentage);
                                //logMessage($"_loanID : {_loan.LoanID.ToString()}");
                                classificationLoanUpdated = _dataAccess.UpdateLoanClassificationAccuracy(_loan.LoanId, _percentage);
                                 
                                //logMessage($"(loanUpdated && loanDetailUpdated) : {(loanUpdated && loanDetailUpdated)}");

                            }

                            if (loanUpdated && loanDetailUpdated && classificationLoanUpdated)
                            {
                                //logMessage($"_loan : {_loan.EphesoftBatchInstanceID.ToString()}");
                                _dataAccess.UpdateLoanOCRPercentageStatus(_loan.LoanId);
                            }
                        }
                        catch (Exception ex)
                        {
                            Exception newException = new Exception($"Loan ID : {_loan.LoanId}.", ex);
                            //throw newException;
                        }                        
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
        }

        private void logMessage(string _msg)
        {
            Exception ex = new Exception(_msg);
            MTSExceptionHandler.HandleException(ref ex);
        }

        #endregion
    }
}
