using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace MTSEntBlocks.ExceptionBlock.Handlers
{
    public static class UIExceptionHandler
    {

        #region Static Variables

        //private static LogWriter logWriter;
        private static readonly ExceptionPolicyFactory _exceptionPolicyFactory;
        private static readonly ExceptionManager _exceptionManager;

        #endregion

        #region Constructor

        static UIExceptionHandler()
        {
            SetExceptionLogger.Create();
            _exceptionPolicyFactory = new ExceptionPolicyFactory();
            _exceptionManager = _exceptionPolicyFactory.CreateManager();
        }

        #endregion

        public static Exception HandleException(ref Exception ex)
        {
            Exception exceptionToThrow = null;

            if (_exceptionManager.HandleException(ex, "UIExceptionLogAndRethrowPolicy", out exceptionToThrow))
            {
                if (exceptionToThrow == null)
                    return ex;
                else
                    return exceptionToThrow;
            }

            return exceptionToThrow;
        }

        public static void LogException(ref Exception ex)
        {
            _exceptionManager.HandleException(ex, "UIExceptionLogAndRethrowPolicy");
        }

        public static void Write(string msg, string handler)
        {
            Logger.Write(msg, handler);
        }

        //public static bool HandleException(ref System.Exception ex)
        //{
        //    return true;
        //    //bool rethrow = false;
        //    //try
        //    //{
        //    //    if (ex is MTSException)
        //    //    {
        //    //        // DA BL exception has already been logged / handled
        //    //    }
        //    //    else
        //    //    {
        //    //        rethrow = ExceptionPolicy.HandleException(ex, "UserInterfacePolicy");
        //    //    }
        //    //}
        //    //catch (Exception exp)
        //    //{
        //    //    ex = exp;
        //    //}
        //    //return rethrow;
        //}
    }
}
