using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class CustConfigService
    {
        public static string TableSchema;
        private string tableSchema;
        public CustConfigService() { }
        public CustConfigService(string tableschema)
        {
            TableSchema = tableschema;
        }

        public object AddRetention(Int64 customerID, string configKey, string configValue, bool active, DateTime createdOn, DateTime modifiedOn)
        {
            configKey = "Retention_Policy";
            active = true;
            createdOn = DateTime.Now;
            modifiedOn= DateTime.Now;
            return new CustConfigDataAccess(TableSchema).AddRetention(customerID, configKey, configValue, active, createdOn, modifiedOn);
        }

        public object AddMultipleCustConfig(Int64 customerID, List<CustomerConfigItem> custConfigItems)
        {
            return new CustConfigDataAccess(TableSchema).AddMultipleCustConfig(customerID, custConfigItems);
        }

        public object GetAllCustConfig(bool Active)
        {
            return new CustConfigDataAccess(TableSchema).GetAllCustConfig(Active);
        }
        public object GetAllDistinctCustData(bool Active)
        {
            return new CustConfigDataAccess(TableSchema).GetAllDistinctCustConfig(Active);
        }

        public object EditRetention(CustomerConfig updateCustConfig)
        { 
            return new CustConfigDataAccess(TableSchema).EditRetention(updateCustConfig);
        }

        public object DeleteCustomerConfig(long CustomerID)
        {
            return new CustConfigDataAccess(TableSchema).DeleteCustomerConfig(CustomerID);
        }
    }
}
