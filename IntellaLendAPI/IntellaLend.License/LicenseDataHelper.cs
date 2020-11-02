using IntellaLend.Model;
using IntellaLendJWTToken;
using MTSEntBlocks.UtilsBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntellaLend.License
{
    public static class LicenseDataHelper
    {
        private static string SystemSchema = "IL";

        public static Dictionary<string, string> GetLicenseConfig()
        {
            Dictionary<string, string> _dic = new Dictionary<string, string>();

            using (var db = new DBConnect(SystemSchema))
            {
                AppConfig _config = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "TENANT_LICENSE").FirstOrDefault();
                if (_config != null)
                {
                    string _decryptedLicense = CommonUtils.EnDecrypt(_config.ConfigValue, true);

                    _dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(_decryptedLicense);
                }
            }
            return _dic;
        }

        public static Int64 GetCuncurrentUserCount(string Schema)
        {
            Int64 _result = 0;

            using (var db = new DBConnect(Schema))
            {
                List<UserSession> _userSessions = db.UserSession.AsNoTracking().Where(u => u.Active).ToList();
                if (_userSessions != null)
                {
                    _result = _userSessions.Where(u => Convert.ToInt32(DateTime.Now.Subtract(u.LastAccessedTime).TotalMinutes) < JWTToken._tokenTimeOut).Count();
                }
            }
            return _result;
        }

        public static string GetLicenseKey()
        {
            string _key = string.Empty;

            using (var db = new DBConnect(SystemSchema))
            {
                AppConfig _config = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "TENANT_LICENSE").FirstOrDefault();
                if (_config != null)
                {
                    _key = _config.ConfigValue;
                }
            }
            return _key;
        }

        public static Int64 GetUserCount(string Schema)
        {
            Int64 _result = 0;

            using (var db = new DBConnect(Schema))
            {
                _result = db.Users.AsNoTracking().ToList().Count();               
            }
            return _result;
        }


    }
}
