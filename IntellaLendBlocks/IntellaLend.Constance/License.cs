using System;
using System.Collections.Generic;

namespace IntellaLend.Constance
{
    public class License
    {
        public static Dictionary<string, string> CONFIGURATION { get; set; }

        private const string TOTALUSERS = "TOTAL_USERS";
        private const string TOTALCONCURRENT_USERS = "TOTAL_CONCURRENT_USERS";
        private const string LICENSEEXPIRYDATE = "LICENSE_EXPIRYDATE";
        private const string ID = "ID";
        private const string SECRETKEY = "SECRET_KEY";
        private const string LICENSETYPE = "LICENSE_TYPE";
        private const string LICENSECREATEDDATE = "LICENSE_CREATEDDATE";

        public static string LICENSE_ID
        {
            get
            {
                return Convert.ToString(CONFIGURATION[ID]);
            }
        }
        public static string SECRET_KEY
        {
            get
            {
                return Convert.ToString(CONFIGURATION[SECRETKEY]);
            }
        }
        public static string LICENSE_TYPE
        {
            get
            {
                return Convert.ToString(CONFIGURATION[LICENSETYPE]);
            }
        }
        public static DateTime LICENSE_CREATEDDATE
        {
            get
            {
                return Convert.ToDateTime(CONFIGURATION[LICENSECREATEDDATE]);
            }
        }
        public static Int64 TOTAL_USERS
        {
            get
            {
                return Convert.ToInt64(CONFIGURATION[TOTALUSERS]);
            }
        }

        public static Int64 TOTAL_CONCURRENT_USERS
        {
            get
            {
                return Convert.ToInt64(CONFIGURATION[TOTALCONCURRENT_USERS]);
            }
        }
        public static DateTime LICENSE_EXPIRYDATE
        {
            get
            {
                return Convert.ToDateTime(CONFIGURATION[LICENSEEXPIRYDATE]);
            }
        }
    }
}
