using Jose;
using System;
using System.Collections.Generic;

namespace IntellaLendJWTToken
{
    public class JWTTokenHash
    {
        public string HashSet { get; set; }
        public string Schema { get; set; }
    }

    public class JWTToken
    {
        public static string _domain { get; set; }
        public static string _clientID { get; set; }
        public static string _secretKey { get; set; }
        public static Int32 _tokenTimeOut { get; set; }

        //public static JWTTokenHash HashToken = null;

        //public string CreateJWTToken()
        //{
        //    byte[] secretKey = Base64UrlDecode(_secretKey);
        //    DateTime issued = DateTime.Now;
        //    DateTime expire = DateTime.Now.AddMinutes(_tokenTimeOut);

        //    var payload = new Dictionary<string, object>()
        //    {
        //        {"iss", _domain},
        //        {"aud", _clientID},
        //        {"sub", "APIResponse"},
        //        {"iat", ToUnixTime(issued).ToString()},
        //        {"exp", ToUnixTime(expire).ToString()},
        //        {"expMin",_tokenTimeOut.ToString()},
        //        {"data", HashToken }
        //    };
            
        //    return JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        //}
        public string CreateJWTToken(string hashValue, string schema)
        {
            JWTTokenHash HashToken = new JWTTokenHash() { HashSet = hashValue, Schema = schema };
            byte[] secretKey = Base64UrlDecode(_secretKey);
            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddMinutes(_tokenTimeOut);

            var payload = new Dictionary<string, object>()
            {
                {"iss", _domain},
                {"aud", _clientID},
                {"sub", "APIResponse"},
                {"iat", ToUnixTime(issued).ToString()},
                {"exp", ToUnixTime(expire).ToString()},
                {"expMin",_tokenTimeOut.ToString()},
                {"data", HashToken }
            };


            return JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        }

        public string CreateJWTToken(object data)
        {
            byte[] secretKey = Base64UrlDecode(_secretKey);
            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddMinutes(_tokenTimeOut);

            var payload = new Dictionary<string, object>()
            {
                {"data", data }
            };


            return JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
        }


        private static byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default:
                    throw new System.Exception(
             "Illegal base64url string!");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        private static long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }

}
