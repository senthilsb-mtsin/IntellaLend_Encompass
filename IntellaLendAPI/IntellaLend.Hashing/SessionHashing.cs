using System;
using System.Security.Cryptography;
using System.Text;

namespace IntellaLend.Hashing
{
    public class SessionHashing
    {
        private static StringBuilder hash;
        private static SHA1CryptoServiceProvider sha1Provider;

        public static string Create(Int64 UserID, Int64 RoleID)
        {
            string hashValue = UserID.ToString() + RoleID.ToString();

            hashValue = GetHash(hashValue) + Guid.NewGuid().ToString();

            return GetHash(hashValue);
        }

        public static string Create(Int64 UserID)
        {
            string hashValue = UserID.ToString();

            hashValue = GetHash(hashValue) + Guid.NewGuid().ToString();

            return GetHash(hashValue);
        }

        private static string GetHash(string hashValue)
        {
            hash = new StringBuilder();

            sha1Provider = new SHA1CryptoServiceProvider();

            byte[] bytes = sha1Provider.ComputeHash(new UTF8Encoding().GetBytes(hashValue));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            return hash.ToString();
        }


        public static bool Check(string DBHashing, string RequestHashing)
        {
            return DBHashing.Equals(RequestHashing);
        }
    }
}
