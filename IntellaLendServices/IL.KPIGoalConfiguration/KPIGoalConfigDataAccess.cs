using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace IL.KPIGoalConfiguration
{
    class KPIGoalConfigDataAccess
    {
        #region Private Variables

        private static string TenantSchema;
        private static string SystemSchema = "IL";

        #endregion

        #region Constructor

        public KPIGoalConfigDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods

        public static List<TenantMaster> GetTenantList()
        {
            using (var db = new DBConnect(SystemSchema))
            {
                List<TenantMaster> _tenantMs = db.TenantMaster.ToList();

                List<AppConfig> _appConfig = db.AppConfig.ToList();

                List<Int64> _ints = new List<long>();

                foreach (TenantMaster a in _tenantMs)
                {
                    foreach (AppConfig app in _appConfig)
                    {
                        if (a.ID == app.ID)
                            _ints.Add(a.ID);
                    }
                }

                _ints = _tenantMs.Where(a => _appConfig.Any(b => b.ID == a.ID)).Select(t => t.ID).ToList();
                return db.TenantMaster.Where(m => m.Active == true).ToList();
            }
        }
        public List<KPIConfigStaging> GetConfigStagingDetails()
        {
            List<KPIConfigStaging> _configStaging = new List<KPIConfigStaging>();
            using (var db = new DBConnect(TenantSchema))
            {
                _configStaging = db.KPIConfigStaging.AsNoTracking().Where(k => k.Status == false).ToList();
                return _configStaging;
            }

        }
        public KPIConfigStaging GetStagingDetail(Int64 GroupID)
        {
            KPIConfigStaging _configStaging = new KPIConfigStaging();
            using (var db = new DBConnect(TenantSchema))
            {
                _configStaging = db.KPIConfigStaging.AsNoTracking().Where(k => k.Status == false && k.GroupID == GroupID).FirstOrDefault();
                return _configStaging;
            }

        }
        public List<UserRoleMapping> GetRoleAssignedUsers(Int64 RoleID)
        {
            List<UserRoleMapping> _userRoles = new List<UserRoleMapping>();
            using (var db = new DBConnect(TenantSchema))
            {
                _userRoles = db.UserRoleMapping.AsNoTracking().Where(u => u.RoleID == RoleID).ToList();
                return _userRoles;
            }
        }
        public List<KPIGoalConfig> GetKPIConfigDetails(Int64 GroupID)
        {
            List<KPIGoalConfig> _kpiGoalConfig = new List<KPIGoalConfig>();
            using (var db = new DBConnect(TenantSchema))
            {
                _kpiGoalConfig = db.KPIGoalConfig.AsNoTracking().Where(u => u.UserGroupID == GroupID).ToList();
                return _kpiGoalConfig;
            }
        }

        public List<KpiUserGroupConfig> GetKPIUserGroupConfigDetails(Int64 RoleID)
        {
            List<KpiUserGroupConfig> _kpiUserConfig = new List<KpiUserGroupConfig>();
            using (var db = new DBConnect(TenantSchema))
            {
                _kpiUserConfig = db.KpiUserGroupConfig.AsNoTracking().Where(u => u.RoleID == RoleID).ToList();
                return _kpiUserConfig;
            }
        }
        public void InsertAuditUserKPIGoalConfig(List<KPIGoalConfig> _lastKPIConfigDetails, KPIConfigStaging _kpiConfigs)
        {
            List<KPIGoalConfig> _kpiGoalConfig = new List<KPIGoalConfig>();
            using (var db = new DBConnect(TenantSchema))
            {
                foreach (var item in _lastKPIConfigDetails)
                {
                    db.AuditUserKpiGoalConfig.Add(new AuditUserKpiGoalConfig()
                    {
                        UserID = item.UserID,
                        UserGroupID = item.UserGroupID,
                        Goal = item.Goal,
                        PeriodFrom = item.PeriodFrom,
                        PeriodTo = item.PeriodTo,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });
                }
                db.SaveChanges();
            }
        }
        public Int64 InsertKPIUserGroupConfig(KPIConfigStaging _kpiConfig, Dictionary<string, DateTime> _periodFromTo)
        {
            List<KPIGoalConfig> _kpiGoalConfig = new List<KPIGoalConfig>();
            KpiUserGroupConfig _kpiUserGroup = null;
            AuditUserKpiGoalConfig _auditConfig = null;
            
            using (var db = new DBConnect(TenantSchema))
            {
                 _kpiUserGroup = db.KpiUserGroupConfig.AsNoTracking().Where(k => k.RoleID == _kpiConfig.GroupID).FirstOrDefault();
             
                if (_kpiUserGroup != null)
                {
                    _auditConfig = db.AuditUserKpiGoalConfig.AsNoTracking().Where(a => a.UserGroupID == _kpiUserGroup.GroupID).OrderByDescending(a => a.ID).FirstOrDefault();
                    AuditKpiGoalConfig _audiKPIGoalConfig = db.AuditKpiGoalConfig.Add(new AuditKpiGoalConfig()
                    {
                        Goal = _kpiUserGroup.Goal,
                        UserGroupID = _kpiUserGroup.GroupID,
                        RoleID = _kpiUserGroup.RoleID,
                        ConfigType = _kpiUserGroup.ConfigType,
                        PeriodFrom = Convert.ToDateTime(_auditConfig.PeriodFrom),
                        PeriodTo = Convert.ToDateTime(_auditConfig.PeriodTo),
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    });

                    _kpiUserGroup.Goal = _kpiConfig.Goal;
                    _kpiUserGroup.ConfigType = _kpiConfig.ConfigType;
                    _kpiUserGroup.PeriodFrom = _periodFromTo["PeriodFrom"];
                    _kpiUserGroup.PeriodTo = _periodFromTo["PeriodTo"];
                    _kpiUserGroup.ModifiedOn = DateTime.Now;
                    db.Entry(_kpiUserGroup).State = EntityState.Modified;

                    db.SaveChanges();

                    return _kpiUserGroup.GroupID;
                }
                else
                {
                    KpiUserGroupConfig _KpiUserGroupConfig = db.KpiUserGroupConfig.Add(new KpiUserGroupConfig()
                    {
                        RoleID = _kpiConfig.GroupID,
                        Goal = _kpiConfig.Goal,
                        PeriodFrom = _periodFromTo["PeriodFrom"],
                        PeriodTo = _periodFromTo["PeriodTo"],
                        ConfigType = _kpiConfig.ConfigType,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                    });
                    db.SaveChanges();
                    return _KpiUserGroupConfig.GroupID;
                }
            }

        }
        public void InsertKPIGoalConfig(List<KPIGoalConfig> _KPIGoalConfig, KPIConfigStaging _kpiGoalStaging, Int64 UserGroupID)
        {
            using (var db = new DBConnect(TenantSchema))
            {
                List<KPIGoalConfig> _OldKPIGoalConfig = db.KPIGoalConfig.AsNoTracking().Where(k => k.UserGroupID == UserGroupID).ToList();
                if (_OldKPIGoalConfig.Count > 0)
                {
                    //db.KPIGoalConfig.RemoveRange(_OldKPIGoalConfig);
                    foreach (var item in _OldKPIGoalConfig)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                }
                foreach (var item in _KPIGoalConfig)
                {
                    db.KPIGoalConfig.Add(item);
                }
                db.SaveChanges();

            }
        }



        #endregion
    }
}

