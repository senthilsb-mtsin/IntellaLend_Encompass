using IntellaLend.Constance;
using IntellaLend.Model;
using MTS.ServiceBase;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace IL.KPIGoalConfiguration
{
    public class KPIGoalConfiguration : IMTSServiceBase
    {
        private int EphesoftTotalCore = 1;
        private bool logTracing = false;
        private string StartDayOfWeek = string.Empty;
        public void OnStart(string ServiceParam)
        {
            var Params = XDocument.Parse(ServiceParam).Descendants("add").Select(z => new { Key = z.Attribute("key").Value, Value = z.Value }).ToList();
            // TiffProcessingPath = Params.Find(f => f.Key == "TiffProcessingPath").Value;
            //Int32.TryParse(Params.Find(f => f.Key == "EphesoftTotalCore").Value, out EphesoftTotalCore);
            Boolean.TryParse(ConfigurationManager.AppSettings["KPIConfigDebug"].ToLower(), out logTracing);
            StartDayOfWeek = Params.Find(f => f.Key == "KPIStartingDay").Value;
        }
        public bool DoTask()
        {
            try
            {
                return ImportKPIStagingDetails();
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return false;
        }
        public bool ImportKPIStagingDetails()
        {
            KPIGoalConfigDataAccess dataAccess = null;
            try
            {
                var TenantList = KPIGoalConfigDataAccess.GetTenantList();

                foreach (var tenant in TenantList)
                {
                    dataAccess = new KPIGoalConfigDataAccess(tenant.TenantSchema);
                    ProcessKPIGoalConfig(dataAccess, tenant);
                }
                return true;
            }
            catch (Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
                return false;
            }
            return false;
        }
        private void ProcessKPIGoalConfig(KPIGoalConfigDataAccess dataAccess, TenantMaster tenant)
        {
            try
            {
                List<KPIConfigStaging> _kpiConfig = new List<KPIConfigStaging>();
                _kpiConfig = dataAccess.GetConfigStagingDetails();

                //LogMessage
                LogMessage($"Total Staging Count : {_kpiConfig.Count}");
                foreach (KPIConfigStaging item in _kpiConfig)
                {
                    bool result = false;
                    List<UserRoleMapping> _userRoles = dataAccess.GetRoleAssignedUsers(item.GroupID);
                    LogMessage($"GroupID : {item.GroupID},UserCount : {_userRoles.Count}");
                    if (_userRoles != null && _userRoles.Count > 0)
                    {
                        Int64 _checkGoalCount = item.Goal % _userRoles.Count;
                        DateTime today = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        if (_checkGoalCount == 0)
                        {
                            Int64 individualGoalCount = item.Goal / _userRoles.Count;
                            List<KPIGoalConfig> _kpiGoalConfig = new List<KPIGoalConfig>();

                            List<KpiUserGroupConfig> _kpiUserGroupConfig = dataAccess.GetKPIUserGroupConfigDetails(item.GroupID);
                            List<KPIGoalConfig> _lastKPIConfigDetails = new List<KPIGoalConfig>();
                            if (_kpiUserGroupConfig != null && _kpiUserGroupConfig.Count > 0)
                            {
                                _lastKPIConfigDetails = dataAccess.GetKPIConfigDetails(_kpiUserGroupConfig[0].GroupID);
                                if (_kpiUserGroupConfig[0].ConfigType != item.ConfigType)
                                {
                                    List<KpiUserGroupConfig> _checkPreviousDate = _kpiUserGroupConfig.Where(a => today >= a.PeriodFrom && today <= a.PeriodTo).ToList();
                                    if (_checkPreviousDate != null && _checkPreviousDate.Count > 0)
                                    {
                                        for (int i = 0; i < _lastKPIConfigDetails.Count; i++)
                                        {
                                            _lastKPIConfigDetails[i].PeriodTo = today.AddDays(-1);
                                        }
                                        result = true;
                                    }
                                    else
                                    {
                                        result = true;
                                    }
                                }
                                if(!result)
                                result = CheckConfigPeriod(item, today);

                                if(result)
                                dataAccess.InsertAuditUserKPIGoalConfig(_lastKPIConfigDetails, item);
                            }
                            else
                            {
                                    result = CheckConfigPeriod(item, today);
                            }

                            if(_kpiUserGroupConfig.Count == 0 || result)
                            {
                                Dictionary<string, DateTime> _periodFromTo = CalculateDate(item.ConfigType, today, today);
                                Int64 GroupID = dataAccess.InsertKPIUserGroupConfig(item, _periodFromTo);
                                foreach (UserRoleMapping _user in _userRoles)
                                {
                                    _kpiGoalConfig.Add(new KPIGoalConfig
                                    {
                                        UserID = _user.UserID,
                                        UserGroupID = GroupID,
                                        PeriodFrom = _periodFromTo["PeriodFrom"],
                                        PeriodTo = _periodFromTo["PeriodTo"],
                                        Goal = individualGoalCount,
                                        CreatedOn = DateTime.Now,
                                        ModifiedOn = DateTime.Now
                                    });
                                }
                                dataAccess.InsertKPIGoalConfig(_kpiGoalConfig, item, GroupID);
                            }
                            
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
        }
        public Dictionary<string, DateTime> CalculateDate(int _configType,DateTime periodFromDate, DateTime periodToDate)
        {
            Dictionary<string, DateTime> _periodFromTo = new Dictionary<string, DateTime>();
            try
            {
                DateTime periodFrom, periodTo;
                switch (_configType)
                {
                    case 1:
                        periodFrom = periodFromDate;
                        _periodFromTo.Add("PeriodFrom", periodFrom);
                        periodTo = periodToDate;
                        _periodFromTo.Add("PeriodTo", periodTo);


                        break;
                    case 2:
                        periodFrom = periodFromDate;
                        _periodFromTo.Add("PeriodFrom", periodFrom);
                        periodTo = periodFromDate.AddDays(DayOfWeek.Saturday - periodToDate.DayOfWeek);
                        _periodFromTo.Add("PeriodTo", periodTo);

                        break;
                    case 3:
                        periodFrom = periodFromDate;
                        _periodFromTo.Add("PeriodFrom", periodFrom);
                        int totaldays = DateTime.DaysInMonth(periodToDate.Year, periodToDate.Month);
                        periodTo = new DateTime(periodToDate.Year, periodToDate.Month, totaldays);
                        _periodFromTo.Add("PeriodTo", periodTo);

                        break;
                    case 4:
                        periodFrom = periodFromDate;
                        _periodFromTo.Add("PeriodFrom", periodFrom);
                        periodTo = new DateTime(periodToDate.Year, 12, 31);
                        _periodFromTo.Add("PeriodTo", periodTo);
                        break;
                    default:
                        break;
                }
                
            }
            catch(Exception ex)
            {
                MTSExceptionHandler.HandleException(ref ex);
            }
            return _periodFromTo;
        }
        public bool CheckConfigPeriod(KPIConfigStaging _configStaging,DateTime today)
        {
            if (_configStaging.ConfigType == StatusConstant.DAILY)
            {
                return true;
            }
            else if (_configStaging.ConfigType == StatusConstant.WEEKLY)
            {
                string _day = Convert.ToString(today.DayOfWeek);
                if (_day == StartDayOfWeek)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (_configStaging.ConfigType == StatusConstant.MONTHLY)
            {
                if (today.Day == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (_configStaging.ConfigType == StatusConstant.YEARLY)
            {
                int CurrentDay = today.Day;
                int CurrentMonth = today.Month;
                if(CurrentDay == 1 && CurrentMonth ==1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
                return false;
        }
        private void LogMessage(string _msg)
        {
            Logger.WriteTraceLog(_msg);
        }

    }
}

