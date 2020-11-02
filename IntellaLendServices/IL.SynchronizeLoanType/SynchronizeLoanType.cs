using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;

namespace IL.SynchronizeLoanType
{
    public class SynchronizeLoanType : IMTSServiceBase
    {
        #region Service Start DoTask

        public void OnStart(string ServiceParam)
        {
            // var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();        
        }

        public bool DoTask()
        {
            //bool result = false;
            try
            {
                StartSynchronizeLoanType();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return true;
        }

        #endregion

        #region Private Methods

        private bool StartSynchronizeLoanType()
        {
            var TenantList = SynchronizeLoanTypeDataAccess.GetTenantList();

            foreach (var tenant in TenantList)
            {
                SynchronizeLoanTypeDataAccess _dataAccess = new SynchronizeLoanTypeDataAccess(tenant.TenantSchema);

                //RetainUpdate _retainUpdate = new RetainUpdate(tenant.TenantSchema);

                List<RetainUpdateStaging> _lsWaitingLoanTypes = _dataAccess.GetStagingLoanTypeSync();

                foreach (RetainUpdateStaging _loanType in _lsWaitingLoanTypes)
                {
                    int result = 0;

                    //List<CustReviewLoanMapping> _lsCustReviewLoanMapping = _dataAccess.GetCustReviewLoanMapping(_loanType.LoanTypeID);

                    _dataAccess.UpdateStagingLoanTypeSync(_loanType.ID, SynchronizeConstant.Process);

                    try
                    {
                        LogMessage($"{tenant.TenantSchema}, {_loanType.ID}, {_loanType.LoanTypeID} - Processing");
                        _dataAccess.SynchDealType(tenant.TenantSchema, _loanType.ID, _loanType.LoanTypeID);
                        result = SynchronizeConstant.Completed;
                        LogMessage($"{tenant.TenantSchema}, {_loanType.ID}, {_loanType.LoanTypeID} - Completed");
                    }
                    catch (Exception ex)
                    {
                        result = SynchronizeConstant.Failed;
                        MTSExceptionHandler.HandleException(ref ex);
                    }

                    _dataAccess.UpdateStagingLoanTypeSync(_loanType.ID, result);
                }


            }
            return true;
        }

        public void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

        #endregion
    }
}
