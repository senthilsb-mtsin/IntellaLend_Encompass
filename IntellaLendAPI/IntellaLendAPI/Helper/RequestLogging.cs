using IntellaLend.CommonServices;
using MTSEntBlocks.ExceptionBlock.Handlers;
using System;

namespace IntellaLendAPI
{
    public static class RequestLogging
    {
        public static bool Enabled {
            get {
                return RequestLoggingService.CheckEnabled();
            }
        }

        public static void InsertRequestResponseLog(DateTime _start, DateTime _end, string _log)
        {
            try
            {
                RequestLoggingService.InsertRequestResponseLog(_start, _end, _log);
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }            
        }
    }
}