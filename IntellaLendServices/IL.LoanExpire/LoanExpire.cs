using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.LoanExpire
{
    public class LoanExpire : IMTSServiceBase
    {
        public void OnStart(string ServiceParam)
        {

        }

        public bool DoTask()
        {
            try
            {
                UpdateRetentionPolicy();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return true;
        }

        private void UpdateRetentionPolicy()
        {
            LoanExpireDataAccess dataAccess = new LoanExpireDataAccess();
            try
            {
                dataAccess.UpdateRetentionPolicyStatus();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }
    }
}
