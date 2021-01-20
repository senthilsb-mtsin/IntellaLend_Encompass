using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class TenantConfigDataAccess
    {
        #region Private variables

        private static string TableSchema;
        private static string TenantSchema = "T1";

        #endregion

        #region Constructor

        public TenantConfigDataAccess() { }

        public TenantConfigDataAccess(string tableschema)
        {
            TableSchema = tableschema;
        }

        #endregion

        #region Public Methods

        public object AddTenantConfigType(CustomerConfig tenantconfig)
        {
            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    if (!db.CustomerConfig.Any(x => x.ConfigKey.Equals(tenantconfig.ConfigKey)))
                    {
                        db.CustomerConfig.Add(tenantconfig);
                        db.SaveChanges();
                        trans.Commit();
                        return true;
                    }
                }
            }

            return false;
        }

        public List<CustomerConfig> GetAllTenantConfigTypes(Int64 CustomerID)
        {

            List<CustomerConfig> custconfig = null;

            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    custconfig = db.CustomerConfig.Where(c => c.CustomerID == CustomerID && c.ConfigKey != CustomerConfiguration.SEARCH_FILTER).ToList();
                }

                return custconfig;
            }
        }

        public bool UpdateTenantConfigType(CustomerConfig reqtenantconfig)
        {

            using (var db = new DBConnect(TableSchema))
            {
                if (db.CustomerConfig.Any(x => x.ConfigID.Equals(reqtenantconfig.ConfigID)))
                {
                    CustomerConfig updateCustConfig = db.CustomerConfig.AsNoTracking().Single(c => c.ConfigID.Equals(reqtenantconfig.ConfigID));
                    updateCustConfig.ConfigValue = reqtenantconfig.ConfigValue;
                    updateCustConfig.Active = reqtenantconfig.Active;
                    updateCustConfig.ModifiedOn = DateTime.Now;
                    db.Entry(updateCustConfig).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<AppConfig> GetAllBoxSettingsConfig()
        {
            List<AppConfig> appConfigLists = null;

            using (var db = new DBConnect(TableSchema))
            {
                appConfigLists = db.AppConfig.AsNoTracking().ToList();
            }

            return appConfigLists;
        }

        public List<AuditDescriptionConfig> GetAllAuditConfig()
        {
            List<AuditDescriptionConfig> auditConfig = null;

            using (var db = new DBConnect(TableSchema))
            {
                auditConfig = db.AuditDescriptionConfig.AsNoTracking().ToList();
            }

            return auditConfig;
        }

        public object GetAllcategoryLists()
        {
             object listCategory;
            using (var db = new DBConnect(TableSchema))
            {
                listCategory = (from catlst in db.CategoryLists.AsNoTracking()
                           // join checklist in db.CheckListDetailMaster.AsNoTracking() on catlst.Category equals checklist.Category into checkCateList
                           // from mappedcatlist in checkCateList.DefaultIfEmpty()
                            select new 
                            {
                                CategoryID = catlst.CategoryID,
                                Category = catlst.Category,
                                Active = catlst.Active,
                                CreatedOn = catlst.CreatedOn,
                                ModifiedOn = catlst.ModifiedOn,
                                IsMappedCheckList = (db.CheckListDetailMaster.Where(x=>x.Category == catlst.Category).FirstOrDefault() !=null) ? true :false
                            }).Distinct().ToList();
              //  listCategory = db.CategoryLists.AsNoTracking().ToList();
            }
            return listCategory;
        }

        public List<ReportMasterData> GetReportMaster()
        {
            List<ReportMasterData> _reportMaster = null;
            //var _reportMaster = "";
            using (var db = new DBConnect(TableSchema))
            {
                _reportMaster = (from rm in db.ReportMaster.AsNoTracking()
                                 join rtm in db.ReviewTypeMaster.AsNoTracking() on rm.ReviewTypeID equals rtm.ReviewTypeID
                                 select new
                                 {
                                     ReportMasterID = rm.ReportMasterID,
                                     ReportName = rm.ReportName,
                                     ReviewTypeID = rm.ReviewTypeID,
                                     ReviewTypeName  = rtm.ReviewTypeName
                                 }).ToList().Select(r => new ReportMasterData { ReportMasterID = r.ReportMasterID,ReportName = r.ReportName,ReviewTypeID = r.ReviewTypeID,ReviewTypeName=r.ReviewTypeName }).ToList();
            }
            return _reportMaster;
        }

        public List<ReportConfig> GetReportConfig()
        {
            List<ReportConfig> reportConfig = null;
            using (var db = new DBConnect(TableSchema))
            {
                //  reportConfig = db.ReportConfig.AsNoTracking().Select(r => new ReportConfig { ReportID = r.ReportID,ReportMasterID = r.ReportMasterID,DocumentName = r.DocumentName}).ToList();
                reportConfig = db.ReportConfig.AsNoTracking().ToList();
            }
            return reportConfig;
        }

        public string GetReviewTypeName(Int64 ReviewTypeID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                return db.ReviewTypeMaster.Where(x => x.ReviewTypeID == ReviewTypeID).Select(r => r.ReviewTypeName).ToString();
            }

        }

        public List<DocumentTypeMaster> GetDocsList()
        {
            List<DocumentTypeMaster> docsList = null;
            using (var db = new DBConnect(TableSchema))
            {
                docsList = db.DocumentTypeMaster.AsNoTracking().ToList();
            }
            return docsList;
        }

        public bool UpdateAuditConfig(AuditDescriptionConfig auditConfig)
        {

            using (var db = new DBConnect(TableSchema))
            {
                auditConfig.ModifiedOn = DateTime.Now;
                db.Entry(auditConfig).State = EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }
        
        public bool SaveandUpdateCategory(CategoryLists categoryList)
        {
            using (var db = new DBConnect(TableSchema))
            {
                CategoryLists _categoryLists = db.CategoryLists.Where(c => c.CategoryID == categoryList.CategoryID).FirstOrDefault();
               
                    _categoryLists.Category = categoryList.Category;
                    _categoryLists.Active = categoryList.Active;
                    _categoryLists.ModifiedOn = DateTime.Now;
                    db.Entry(_categoryLists).State = EntityState.Modified;
                    db.SaveChanges();
                }            
            return true;                
        }

        public bool SaveCategoryGroup(string category,bool active)
        {
            CategoryLists _newcategory = new CategoryLists();
            using (var db = new DBConnect(TableSchema))
            {
                _newcategory.Category = category;
                _newcategory.Active = active;
                _newcategory.CreatedOn = DateTime.Now;
                _newcategory.ModifiedOn = DateTime.Now;
                db.CategoryLists.Add(_newcategory);
                db.SaveChanges();
            }
            return true;
        }

        public bool SaveReportConfigData(string docName, Int64 MasterID,string ServiceType)
        {
            ReportConfig config = new ReportConfig();
            ReportMaster master = new ReportMaster();
            using (var db = new DBConnect(TableSchema))
            {
                master = db.ReportMaster.AsNoTracking().FirstOrDefault();
                config.ReportMasterID = master.ReportMasterID;
                config.DocumentName = docName;
                config.CreatedOn = DateTime.Now;
                config.ModifiedOn = DateTime.Now;
                db.ReportConfig.Add(config);
                db.SaveChanges();
            }
            using (var db = new DBConnect(TenantSchema))
            {                
                //ReviewTypeMaster review = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeName == ServiceType).FirstOrDefault();
                ReportConfig report = new ReportConfig();
                //if(review != null)
                //{
                    report.ReportMasterID = db.ReportMaster.AsNoTracking().Where(m => m.ReportName == master.ReportName).Select(r => r.ReportMasterID).FirstOrDefault();
                    report.DocumentName = docName;
                    report.CreatedOn = DateTime.Now;
                    report.ModifiedOn = DateTime.Now;
                    db.ReportConfig.Add(report);
                    db.SaveChanges();
                //}                   
            }
           return true;
        }

        public bool DeleteReportConfig(Int64 reportId,string docName)
        {
            ReportConfig config = new ReportConfig();
            using (var db = new DBConnect(TableSchema))
            {
                config = db.ReportConfig.AsNoTracking().Where(c => c.DocumentName.Equals(docName) && c.ReportID.Equals(reportId)).FirstOrDefault();
                db.Entry(config).State = EntityState.Deleted;
                db.ReportConfig.Remove(config);
                db.SaveChanges();
            }
            using (var db = new DBConnect(TenantSchema))
            {
                ReportConfig report = db.ReportConfig.AsNoTracking().Where(c => c.DocumentName.Equals(docName) && config.ReportID.Equals(reportId)).FirstOrDefault();
                if(report != null)
                {
                    //report = db.ReportConfig.AsNoTracking().Where(c => c.DocumentName.Equals(docName) && c.ReportID.Equals(reportId)).FirstOrDefault();
                    db.Entry(report).State = EntityState.Deleted;
                    db.ReportConfig.Remove(report);
                    db.SaveChanges();
                }
            }
            return true;          
        }

        public List<InvestorStipulation> GetInvestorStipulationList()
        {
            List<InvestorStipulation> investor = new List<InvestorStipulation>();
            using (var db = new DBConnect(TableSchema))
            {
                investor = db.InvestorStipulations.AsNoTracking().ToList();
            }
            return investor;
        }

        public List<InvestorStipulation> GetActiveInvestorStipulationList()
        {
            List<InvestorStipulation> investor = new List<InvestorStipulation>();
            using (var db = new DBConnect(TableSchema))
            {
                investor = db.InvestorStipulations.AsNoTracking().Where(i=>i.Active == true).ToList();
            }
            return investor;
        }

        public bool SaveInvestorStipulation(string category, bool active)
        {
            InvestorStipulation investor = new InvestorStipulation();
            using (var db = new DBConnect(TableSchema))
            {
                investor.StipulationCategory = category;
                investor.Active = active;
                investor.CreatedOn = DateTime.Now;
                investor.ModifiedOn = DateTime.Now;
                db.InvestorStipulations.Add(investor);
                db.SaveChanges();
            }
                return true;
        }

        public bool UpdateInvestorStipulation(Int64 id, string category, bool acvtive)
        {
            InvestorStipulation stipulation = new InvestorStipulation();
            using (var db = new DBConnect(TableSchema))
            {
                stipulation = db.InvestorStipulations.AsNoTracking().Where(i => i.StipulationID == id).FirstOrDefault();
                if(stipulation != null)
                {
                    stipulation.StipulationCategory = category;
                    stipulation.Active = acvtive;
                    stipulation.ModifiedOn = DateTime.Now;
                    db.Entry(stipulation).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool BoxSettingsConfig(string ClientId, string ClientSecretId, string boxUserID, bool isUpdate)
        {

            using (var db = new DBConnect(TableSchema))
            {
                using (var trans = db.Database.BeginTransaction())
                {

                    Dictionary<string, string> _configKeys = new Dictionary<string, string>();
                    _configKeys.Add(ConfigConstant.BOXCLIENTID, ClientId);
                    _configKeys.Add(ConfigConstant.BOXCLIENTSECRETID, ClientSecretId);
                    _configKeys.Add(ConfigConstant.BOXUSERID, boxUserID);

                    foreach (string key in _configKeys.Keys)
                    {
                        AppConfig appc = db.AppConfig.AsNoTracking().Where(apc => apc.ConfigKey == key).FirstOrDefault();

                        if (appc != null)
                        {
                            appc.ConfigValue = _configKeys[key];
                            db.Entry(appc).State = EntityState.Modified;
                        }
                        else
                        {
                            db.AppConfig.Add(new AppConfig()
                            {
                                ConfigKey = key,
                                ConfigValue = _configKeys[key]                                
                            });
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                    return true;

                    //if (!isUpdate)
                    //{

                    //if(!db.AppConfig.Any(ac=> ac.ConfigKey== ConfigConstant.BOXCLIENTID))
                    //{
                    //    db.AppConfig.Add(new AppConfig()
                    //    {
                    //        ConfigKey = ConfigConstant.BOXCLIENTID,
                    //        ConfigValue = ClientId,

                    //    });
                    //    db.AppConfig.Add(new AppConfig()
                    //    {
                    //        ConfigKey = ConfigConstant.BOXCLIENTSECRETID,
                    //        ConfigValue = ClientSecretId,

                    //    });

                    //    db.AppConfig.Add(new AppConfig()
                    //    {
                    //        ConfigKey = ConfigConstant.BOXUSERID,
                    //        ConfigValue = boxUserID,

                    //    });
                    //}

                    //}
                    //else
                    //{

                    //AppConfig appc = db.AppConfig.AsNoTracking().Where(apc => apc.ConfigKey == ConfigConstant.BOXCLIENTID || apc.ConfigKey == ConfigConstant.BOXCLIENTID || apc.).FirstOrDefault();

                    //    if (appc.ConfigKey == ConfigConstant.BOXCLIENTID)
                    //    {
                    //        appc.ConfigValue = ClientId;
                    //        db.Entry(appc).State = EntityState.Modified;
                    //    }

                    //    appc = db.AppConfig.AsNoTracking().Where(apc => apc.ConfigKey == ConfigConstant.BOXCLIENTSECRETID).FirstOrDefault();
                    //    if (appc.ConfigKey == ConfigConstant.BOXCLIENTSECRETID)
                    //    {
                    //        appc.ConfigValue = ClientSecretId;
                    //        db.Entry(appc).State = EntityState.Modified;
                    //    }
                    //    appc = db.AppConfig.AsNoTracking().Where(apc => apc.ConfigKey == ConfigConstant.BOXUSERID).FirstOrDefault();
                    //    if (appc.ConfigKey == ConfigConstant.BOXUSERID)
                    //    {
                    //        appc.ConfigValue = boxUserID;
                    //        db.Entry(appc).State = EntityState.Modified;
                    //    }

                    //}

                }

            }

            return false;
        }

        public bool DeleteTenantConfigType(Int64 configID)
        {
            using (var db = new DBConnect(TableSchema))
            {
                CustomerConfig custConfigObj = db.CustomerConfig.AsNoTracking().Where(cc => cc.ConfigID == configID).FirstOrDefault();
                db.Entry(custConfigObj).State = EntityState.Deleted;
                db.CustomerConfig.Remove(custConfigObj);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public CustomerConfig GetConfigValues(Int64 CustomerID, string ConfigKey)
        {
            using (var db = new DBConnect(TableSchema))
            {
                return db.CustomerConfig.AsNoTracking().Where(c => c.CustomerID == CustomerID && c.ConfigKey == ConfigKey).FirstOrDefault();
            }
        }

        public List<CustomerConfig> GetLoanSearchFilterConfigValue(Int64 CustomerID, string ConfigKey)
        {
            List<CustomerConfig> _config = new List<CustomerConfig>();
            using (var db = new DBConnect(TableSchema))
            {
                _config = db.CustomerConfig.AsNoTracking().Where(c => c.ConfigKey == CustomerConfiguration.SEARCH_FILTER).ToList();
                return _config;
            }
            
        }

        public object GetLoanSearchFilterValues(Int64 CustomerID, string ConfigKey)
        {
            List<CustomerConfig> _config = new List<CustomerConfig>();
            Dictionary<string, bool> _custConfig = new Dictionary<string, bool>(); 
            using (var db = new DBConnect(TableSchema))
            {
                _config = db.CustomerConfig.AsNoTracking().Where(c => c.ConfigKey == CustomerConfiguration.SEARCH_FILTER).ToList();

                foreach (var item in _config)
                {
                    _custConfig.Add(item.ConfigValue, item.Active);
                }
                return _custConfig;
            }

        }

        public bool UpdateLoanSearchFilterConfig(Int64 configID,bool status)
        {
           
            CustomerConfig _cust = new CustomerConfig();
            List<CustomerConfig> _custConfig = new List<CustomerConfig>(); 
            using (var db = new DBConnect(TableSchema))
            {
                _custConfig = db.CustomerConfig.AsNoTracking().Where(c => c.ConfigKey == CustomerConfiguration.SEARCH_FILTER && c.Active == true).ToList();
                _cust = db.CustomerConfig.AsNoTracking().Where(c => c.ConfigID == configID).FirstOrDefault();
                if(_cust != null && _custConfig !=null && (_custConfig.Count > 1 || status == true))
                {
                    _cust.Active = status;
                    _cust.ModifiedOn = DateTime.Now;
                    db.Entry(_cust).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public object CheckWebHookSubscriptionEventTypeExist(int eventType)
        {
            bool EventTypeExist = false;
            using (var db = new DBConnect(TableSchema))
            {
                EventTypeExist = db.EWebhookSubscription.AsNoTracking().Any(x => x.EventType == eventType);
            }
            return new { EventTypeExist };
        }

        public object GetFannieMaeCustomerConfig()
        {
            bool FannieMaeCustomerConfig;
            using (var db = new DBConnect(TenantSchema))
            {
                CustomerConfig customerConfig = db.CustomerConfig.AsNoTracking().Where(x => x.ConfigKey.Equals("FannieMae")).FirstOrDefault();
                FannieMaeCustomerConfig = customerConfig != null && customerConfig.ConfigValue.ToLower().Equals("true");
            }
            return new { FannieMaeCustomerConfig };
        }
        #endregion


    }
}
