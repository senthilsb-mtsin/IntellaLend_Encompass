using IntellaLend.Model;
using MTSEntityDataAccess;
using System.Linq;

namespace IntellaLend.WFProxy
{
    public class WFProxyDataAccess
    {
        public static string SystemSchema = "IL";

        public static string WorkFlowURL
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "WORKFLOWURL").FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }

        public static string WorkflowEnvironment
        {
            get
            {
                using (var db = new DBConnect(SystemSchema))
                {
                    AppConfig appConfig = db.AppConfig.AsNoTracking().Where(a => a.ConfigKey == "WORKFLOWENVIRONMENT").FirstOrDefault();

                    if (appConfig != null)
                        return appConfig.ConfigValue;
                }
                return string.Empty;
            }
        }
    }
}
