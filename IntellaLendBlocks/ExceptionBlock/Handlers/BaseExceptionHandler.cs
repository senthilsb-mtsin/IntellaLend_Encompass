using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace MTSEntBlocks.ExceptionBlock.Handlers
{
    public static class BaseExceptionHandler
    {

        #region Static Variables

        //private static LogWriter logWriter;
        private static readonly ExceptionPolicyFactory _exceptionPolicyFactory;
        private static readonly ExceptionManager _exceptionManager;

        #endregion

        #region Constructor

        static BaseExceptionHandler()
        {
            SetExceptionLogger.Create();
             _exceptionPolicyFactory = new ExceptionPolicyFactory();
            _exceptionManager = _exceptionPolicyFactory.CreateManager();
        }

        #endregion

        public static Exception HandleException(ref Exception ex)
        {
            Exception exceptionToThrow = null;

            if (_exceptionManager.HandleException(ex, "ExceptionLogAndRethrowPolicy", out exceptionToThrow))
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
            _exceptionManager.HandleException(ex, "ExceptionLogAndRethrowPolicy");
        }

        public static void Write(string msg, string handler)
        {
            Logger.Write(msg, handler);
        }
        
        ////Log Exceptions to DB
        //public static void UpdateExceptionToDB(System.Exception ex)
        //{
        //    try
        //    {
        //        string ApplicationName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        //        string ExceptionMessage = ex.Message;
        //        string StackTrace = ex.StackTrace;

        //        if (ExceptionMessage != null)
        //            ExceptionMessage = ExceptionMessage.Length > 500 ? ExceptionMessage.Substring(0, 500) : ExceptionMessage;

        //        if (StackTrace != null)
        //            StackTrace = StackTrace.Length > 1000 ? StackTrace.Substring(0, 1000) : StackTrace;

        //        MTSEntBlocks.DataBlock.DataAccess.ExecuteNonQuery("XC_ADD_EXCEPTION_LOG", ApplicationName, ExceptionMessage, StackTrace);
        //    }
        //    catch (Exception logException)
        //    {
        //        LogException(ref logException);
        //    }
        //}

        ////Log Exceptions to DB
        //public static void UpdateCustomExceptionToDB(string message, string stacktrace, string appName = null)
        //{
        //    try
        //    {
        //        string ApplicationName = appName;
        //        if (ApplicationName == null)
        //        {
        //            Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        //        }
        //        string ExceptionMessage = message;
        //        string StackTrace = stacktrace;

        //        if (ExceptionMessage != null)
        //            ExceptionMessage = ExceptionMessage.Length > 500 ? ExceptionMessage.Substring(0, 500) : ExceptionMessage;

        //        if (StackTrace != null)
        //            StackTrace = StackTrace.Length > 1000 ? StackTrace.Substring(0, 1000) : StackTrace;

        //        MTSEntBlocks.DataBlock.DataAccess.ExecuteNonQuery("XC_ADD_EXCEPTION_LOG", ApplicationName, ExceptionMessage, StackTrace);
        //    }
        //    catch (Exception logException)
        //    {
        //        LogException(ref logException);
        //    }
        //}
    }
}
