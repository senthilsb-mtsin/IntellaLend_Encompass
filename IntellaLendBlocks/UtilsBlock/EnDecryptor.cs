using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MTSEntBlocks.UtilsBlock
{
    public class EnDecryptor
    {
        private static readonly string IntVector;
        private static readonly string SecretKey;
        static EnDecryptor()
        {

            IntVector = ConfigurationManager.AppSettings["IntializationVector"];
            SecretKey = ConfigurationManager.AppSettings["SecretKey"];
        }

        /// <summary>
        /// EncryptAES
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncryptAES(string plainText)
        {
            string cipherText = string.Empty;
            byte[] encrypted;


            string int_vector = IntVector;
            string secret_Key = SecretKey;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Encoding.ASCII.GetBytes(secret_Key);
                aes.IV = Encoding.ASCII.GetBytes(int_vector);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }

            cipherText = Convert.ToBase64String(encrypted);

            return cipherText;
        }

        /// <summary>
        /// DecryptAES
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DecryptAES(string encryptedData)
        {
            string plainText = string.Empty;

            string decrypted = null;
            byte[] cipher = Convert.FromBase64String(encryptedData);
            string int_vector = IntVector;
            string secret_Key = SecretKey;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = Encoding.ASCII.GetBytes(secret_Key);
                aes.IV = Encoding.ASCII.GetBytes(int_vector);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            decrypted = sr.ReadToEnd();
                        }
                    }
                }
            }

            plainText = decrypted;
            return plainText;
        }
    }
}