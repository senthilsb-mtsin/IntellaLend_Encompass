using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Linq;

namespace IntellaLend.AuditData
{
    public class AuditDataAccess
    {
        public static string[] GetAuditDescription(string schema, Int64 auditID)
        {
            string[] auditDesc = {string.Empty, string.Empty };
            using (var db = new DBConnect(schema))
            {
                AuditDescriptionConfig aConfig = db.AuditDescriptionConfig.AsNoTracking().Where(a => a.ConstantID == auditID).FirstOrDefault();

                if (aConfig != null) {
                    auditDesc[0] = aConfig.Description;
                    auditDesc[1] = aConfig.SystemDescription;
                }
            }
            return auditDesc;
        }
    }
}
