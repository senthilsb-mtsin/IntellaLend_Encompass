using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class TenantConfigService
    {
        #region Private variables

        private static string TableSchema;

        #endregion

        #region Constructor

        public TenantConfigService() { }

        public TenantConfigService(string tableschema)
        {
            TableSchema = tableschema;
        }

        #endregion

        #region Public Methods

        public object AddTenantConfigType(CustomerConfig reqtenantconfig)
        {
            reqtenantconfig.CreatedOn = DateTime.Now;
            reqtenantconfig.ModifiedOn = DateTime.Now;
            return new TenantConfigDataAccess(TableSchema).AddTenantConfigType(reqtenantconfig);

        }

        public List<CustomerConfig> GetAllTenantConfigTypes(Int64 CustomerID)
        {
            return new TenantConfigDataAccess(TableSchema).GetAllTenantConfigTypes(CustomerID);

        }

        public bool UpdateTenantConfigType(CustomerConfig reqtenantconfig)
        {
            return new TenantConfigDataAccess(TableSchema).UpdateTenantConfigType(reqtenantconfig);

        }

        public bool DeleteTenantConfigType(Int64 ConfigID)
        {
            return new TenantConfigDataAccess(TableSchema).DeleteTenantConfigType(ConfigID);

        }

        public CustomerConfig GetConfigValues(Int64 CustomerID, string ConfigKey)
        {
            return new TenantConfigDataAccess(TableSchema).GetConfigValues(CustomerID, ConfigKey);

        }

        public bool BoxSettingsConfig(string username, string password, string boxUserID, bool isUpdate)
        {
            return new TenantConfigDataAccess(TableSchema).BoxSettingsConfig(username,password, boxUserID, isUpdate);
        }

        public List<AppConfig> GetAllBoxSettingsConfig()
        {
            return new TenantConfigDataAccess(TableSchema).GetAllBoxSettingsConfig();
        }

        public List<AuditDescriptionConfig> GetAllAuditConfig()
        {
            return new TenantConfigDataAccess(TableSchema).GetAllAuditConfig();
        }

        public bool UpdateAuditConfig(AuditDescriptionConfig auditConfig)
        {
            return new TenantConfigDataAccess(TableSchema).UpdateAuditConfig(auditConfig);
        }

        public object GetAllcategoryLists()
        {
            return new TenantConfigDataAccess(TableSchema).GetAllcategoryLists();
        }

        public object GetReportMaster()
        {
            var reportMaster = new TenantConfigDataAccess(TableSchema).GetReportMaster();
            var reportConfig = new TenantConfigDataAccess(TableSchema).GetReportConfig();
            return new
            {
                reportMaster,
                reportConfig
            };
        }

        public bool SaveReportConfigData( string docName,  Int64 MasterID,string ServiceType)
        {
            return new TenantConfigDataAccess(TableSchema).SaveReportConfigData(docName, MasterID, ServiceType);
        }

        public bool DeleteReportConfig(Int64 reportID,string docName)
        {
            return new TenantConfigDataAccess(TableSchema).DeleteReportConfig(reportID, docName);
        }

        public bool SaveandUpdateCategory(CategoryLists categoryList)
        {
            return new TenantConfigDataAccess(TableSchema).SaveandUpdateCategory(categoryList);
        }

        public bool SaveCategoryGroup(string category,bool Active)
        {
            return new TenantConfigDataAccess(TableSchema).SaveCategoryGroup(category, Active);
        }

        public List<DocumentTypeMaster> GetDocsList()
        {
            return new TenantConfigDataAccess(TableSchema).GetDocsList();
        }

        public List<InvestorStipulation> GetInvestorStipulationList()
        {
            return new TenantConfigDataAccess(TableSchema).GetInvestorStipulationList();
        }

        public List<InvestorStipulation> GetActiveInvestorStipulationList()
        {
            return new TenantConfigDataAccess(TableSchema).GetActiveInvestorStipulationList();
        }

        public bool SaveInvestorStipulation(string category, bool acvtive)
        {
            return new TenantConfigDataAccess(TableSchema).SaveInvestorStipulation(category, acvtive);
        }

        public bool UpdateInvestorStipulation(Int64 id, string category, bool acvtive)
        {
            return new TenantConfigDataAccess(TableSchema).UpdateInvestorStipulation(id,category, acvtive);
        }
        public List<CustomerConfig> GetLoanSearchFilterConfigValue(Int64 customerID,string configKey)
        {
            return new TenantConfigDataAccess(TableSchema).GetLoanSearchFilterConfigValue(customerID, configKey);
        }

        public object GetLoanSearchFilterValues(Int64 customerID, string configKey)
        {
            return new TenantConfigDataAccess(TableSchema).GetLoanSearchFilterValues(customerID, configKey);
        }

        public bool UpdateLoanSearchFilterConfig(Int64 configID ,bool status)
        {
            return new TenantConfigDataAccess(TableSchema).UpdateLoanSearchFilterConfig(configID, status);
        }
        public object CheckWebHookSubscriptionEventTypeExist(Int32 eventType)
        {
            return new TenantConfigDataAccess(TableSchema).CheckWebHookSubscriptionEventTypeExist(eventType);
        }

        public object GetFannieMaeCustomerConfig()
        {
            return new TenantConfigDataAccess(TableSchema).GetFannieMaeCustomerConfig();
        }
        #endregion
    }
}
