using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Linq;

namespace IntellaLend.SystemData
{
    public static class RequestLoggingDataAccess
    {
        private static string SystemSchema = "IL";

        public static void InsertRequestResponseLog(DateTime _start, DateTime _end, string _log)
        {
            //RequestLoggingService.InsertLog(_log);

            using (var db = new DBConnect(SystemSchema))
            {
                db.RequestResponseLogging.Add(new RequestResponseLogging {
                    RequestDateTime = _start,
                    ResponseDateTime = _end,
                    LogXML = _log
                });
                db.SaveChanges();
            }
        }

        public static bool CheckEnabled()
        {
            bool result = false;
            using (var db = new DBConnect(SystemSchema))
            {
                AppConfig _config = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "LOG_REQUEST_RESPONSE").FirstOrDefault();

                if (_config != null)
                    bool.TryParse(_config.ConfigValue, out result);
            }

            return result;
        }
    }
}
