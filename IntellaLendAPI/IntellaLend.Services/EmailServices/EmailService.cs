using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.CommonServices.EmailServices
{
    public class EmailService
    {
        private string TenantSchema;

        #region Constructor

        public EmailService()
        { }

        public EmailService(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        public LOSImportStaging GetImportStagingDetails (Int64 importStagingID)
        {  
            return new IntellaLendDataAccess(TenantSchema).GetLOSImportStagingDetail(importStagingID);
        }
    }
}
