using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MTSEntBlocks.DataBlock
{
    public static class DataAccessExceptionHandler
    {
        #region Static Variables

        //private static LogWriter logWriter;
        private static readonly ExceptionPolicyFactory _exceptionPolicyFactory;
        private static readonly ExceptionManager _exceptionManager;

        #endregion

        #region Constructor

        static DataAccessExceptionHandler()
        {
            SetDataLogger.Create();
            _exceptionPolicyFactory = new ExceptionPolicyFactory();
            _exceptionManager = _exceptionPolicyFactory.CreateManager();
        }

        #endregion

        public static bool HandleException(ref System.Exception ex)
        {
            Exception exceptionToThrow = null;
            
            if (_exceptionManager.HandleException(ex, "ExceptionLogAndRethrowPolicy", out exceptionToThrow))
            {
                if (exceptionToThrow == null)
                    return true;
                else
                    return false;
            }

            return false;

            //bool rethrow = false;

            //rethrow = ExceptionPolicy.HandleException(ex, "ExceptionLogAndRethrowPolicy");

            //return rethrow;
        }

    }
}
