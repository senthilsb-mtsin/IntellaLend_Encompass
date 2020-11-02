using System;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace EphesoftService
{
    public static class LookupCryption
    {
        private static byte[] key = { 12, 251, 148, 79, 156, 99, 101, 222, 19, 188, 47, 203, 86, 18, 45, 175, 43, 24, 4, 9, 161, 151, 89, 39 };
        private static byte[] iv = { 13, 17, 249, 126, 97, 213, 19, 199 };     

        public static byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                plainText = "0";
            UTF8Encoding utf8encoder = new UTF8Encoding();
            try
            {
                byte[] inputInBytes = utf8encoder.GetBytes(plainText);
                TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
                ICryptoTransform cryptoTransform = tdesProvider.CreateEncryptor(key, iv);
                MemoryStream encryptedStream = new MemoryStream();
                CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write);
                cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
                cryptStream.FlushFinalBlock();
                encryptedStream.Position = 0;
                byte[] result=new byte[encryptedStream.Length];
                encryptedStream.Read(result, 0, result.Length);
                cryptStream.Close();
                return result;
            }
            catch (Exception ex)
            {
                return utf8encoder.GetBytes("0");
            }
        }

        public static string Decrypt(byte[] inputInBytes)
        {
            try
            {
                UTF8Encoding utf8encoder = new UTF8Encoding();
                TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
                ICryptoTransform cryptoTransform = tdesProvider.CreateDecryptor(key, iv);
                MemoryStream decryptedStream = new MemoryStream();
                CryptoStream cryptStream = new CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write);
                cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
                cryptStream.FlushFinalBlock();
                decryptedStream.Position = 0;
                byte[] result=new byte[decryptedStream.Length];
                decryptedStream.Read(result, 0, result.Length);
                cryptStream.Close();
                UTF8Encoding myutf = new UTF8Encoding();
                return myutf.GetString(result);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}