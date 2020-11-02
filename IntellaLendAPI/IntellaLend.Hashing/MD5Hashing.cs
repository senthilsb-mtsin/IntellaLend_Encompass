using IntellaLend.Constance;
using System;
using System.Security.Cryptography;
using System.Text;

namespace IntellaLend.Hashing
{
    public class MD5Hashing
    {

        private static StringBuilder hash;
        private static MD5CryptoServiceProvider md5provider;

        public static string Create(string Password)
        {
            hash = new StringBuilder();

            md5provider = new MD5CryptoServiceProvider();

            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(Password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            return hash.ToString();
        }


        public static bool Check(string Password, string userPassword, DateTime CreatedOn)
        {          
            return userPassword.Equals(Create(Password + CreatedOn.ToString(DateConstance.LongDateFormart)));
        }
        
    }
}
