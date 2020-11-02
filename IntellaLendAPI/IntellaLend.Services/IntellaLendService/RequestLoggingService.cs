using IntellaLend.SystemData;
using System;

namespace IntellaLend.CommonServices
{
    public static class RequestLoggingService
    {
        public static void InsertRequestResponseLog(DateTime _start, DateTime _end, string _log)
        {
            RequestLoggingDataAccess.InsertRequestResponseLog(_start, _end,  _log);
          }

        public static bool CheckEnabled()
        {
           return RequestLoggingDataAccess.CheckEnabled();
        }
        
    }
}
