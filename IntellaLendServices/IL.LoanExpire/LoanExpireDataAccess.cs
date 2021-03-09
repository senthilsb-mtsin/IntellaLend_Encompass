using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IL.LoanExpire
{
    public class LoanExpireDataAccess
    {
        #region Private Variables

        private static string SystemSchema = "IL";

        #endregion

        #region Public Methods 

        public void UpdateRetentionPolicyStatus()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                db.Database.ExecuteSqlCommand($"EXEC [{SystemSchema}].[RETENTIONPOLICYSTATUSUPDATE]");
            }
        }
    }

    #endregion
}
