using MTSEntityDataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace IntellaLend.Hashing
{
    public static class DBHashing
    {
        public static string Decrypt(DBConnect db, byte[] EncryptedArray)
        {
            var Param = new SqlParameter("ENCRYPTED", SqlDbType.VarBinary);
            Param.Value = EncryptedArray;

            var oParam = new SqlParameter("RESULT", SqlDbType.VarChar, -1)
            {
                Direction = ParameterDirection.Output
            };

            db.Database.ExecuteSqlCommand("[IL].[Decrypt] @ENCRYPTED, @RESULT OUTPUT", Param, oParam);

            string result = oParam.Value == DBNull.Value ? string.Empty : (string)oParam.Value;

            return result;
        }

        public static byte[] Encrypt(DBConnect db, string DecryptedString)
        {
            var Param = new SqlParameter("DECRYPTED", SqlDbType.VarChar);
            Param.Value = DecryptedString;

            var oParam = new SqlParameter("RESULT", SqlDbType.VarBinary, -1)
            {
                Direction = ParameterDirection.Output
            };

            db.Database.ExecuteSqlCommand("[IL].[Encrypt] @DECRYPTED, @RESULT OUTPUT", Param, oParam);

            byte[] result = oParam.Value == DBNull.Value ? new byte[0] : (byte[])oParam.Value;

            return result;
        }
    }
}
