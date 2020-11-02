using MTSEntBlocks.DataBlock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntellaLend.EncompassWrapper
{
    public class EncompassConnectorDataAccess
    {
        private static DataTable ConfigTable;

        public static string ENCOMPASS_SERVER
        {
            get
            {
                return (from d in ConfigTable.AsEnumerable()
                        where d["ConfigKey"].ToString() == EncompassConnectorConstant.ENCOMPASS_SERVER
                        select d["ConfigValue"].ToString()).FirstOrDefault().ToString();
            }
        }

        public static string ENCOMPASS_USERNAME
        {
            get
            {
                return (from d in ConfigTable.AsEnumerable()
                        where d["ConfigKey"].ToString() == EncompassConnectorConstant.ENCOMPASS_USERNAME
                        select d["ConfigValue"].ToString()).FirstOrDefault().ToString();
            }
        }

        public static string ENCOMPASS_PASSWORD
        {
            get
            {
                return (from d in ConfigTable.AsEnumerable()
                        where d["ConfigKey"].ToString() == EncompassConnectorConstant.ENCOMPASS_PASSWORD
                        select d["ConfigValue"].ToString()).FirstOrDefault().ToString();
            }
        }

        static EncompassConnectorDataAccess()
        {
            GetAppConfig();
        }

        private static void GetAppConfig()
        {
            ConfigTable = DataAccess.ExecuteSQLDataTable(string.Format("Select ConfigKey, ConfigValue from {0}", EncompassConnectorConstant.ENCOMPASS_CONFIG_TABLE));
        }
    }
}
