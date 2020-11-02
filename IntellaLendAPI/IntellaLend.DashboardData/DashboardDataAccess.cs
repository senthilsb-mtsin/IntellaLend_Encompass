using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntityDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntellaLend.EntityDataHandler
{
    public class DashboardDataAccess
    {
        private string TenantSchema;

        #region Constructor

        public DashboardDataAccess()
        { }

        public DashboardDataAccess(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion


        #region Public Methods

        public List<object> GetNeedsAttention(Int64 CurrentUserID)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {

                var loans = (from L in db.Loan
                             where L.LastAccessedUserID == CurrentUserID && L.Status == StatusConstant.PENDING_AUDIT
                             select new
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = L.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = L.Status,
                                 LoggedUserID = L.LoggedUserID
                             }).ToList().OrderByDescending(l => l.ReceivedDate).Take(5);

                var loanDetails = (from L in loans
                                   join search in db.LoanSearch on L.LoanID equals search.LoanID
                                   join LTM in db.LoanTypeMaster on search.LoanTypeID equals LTM.LoanTypeID
                                   select new
                                   {
                                       LoanID = search.LoanID,
                                       LoanNumber = search.LoanNumber,
                                       LoanTypeID = search.LoanTypeID,
                                       ReceivedDate = search.ReceivedDate,
                                       Status = search.Status,
                                       LoanAmount = search.LoanAmount,
                                       LoanTypeName = LTM.LoanTypeName,
                                       BorrowerName = search.BorrowerName,
                                       StatusDescription = "",
                                       LoggedUserID = L.LoggedUserID
                                   }).ToList();

                List<WorkFlowStatusMaster> wfMaster = new IntellaLendDataAccess().GetWorkFlowMaster();

                lm = (from l in loanDetails.AsEnumerable()
                      join wm in wfMaster on l.Status equals wm.StatusID
                      select new
                      {
                          LoanID = l.LoanID,
                          LoanNumber = l.LoanNumber,
                          LoanTypeID = l.LoanTypeID,
                          ReceivedDate = l.ReceivedDate,
                          Status = l.Status,
                          LoanAmount = l.LoanAmount,
                          LoanTypeName = l.LoanTypeName,
                          BorrowerName = l.BorrowerName,
                          StatusDescription = wm.StatusDescription,
                          LoggedUserID = l.LoggedUserID
                      }).ToList<object>();
            }

            return lm;
        }

        public List<object> GetAuditStatus(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {
                var lsLoanStatus = db.Loan.Where(l => l.CreatedOn >= FromDate && l.CreatedOn < ToDate && l.CustomerID == (IsSystemAdmin ? l.CustomerID : CustomerID))
                                        .GroupBy(l => l.Status)
                                        .Select(g => new
                                        {
                                            name = g.Key,
                                            y = g.Select(l => l.Status).Count(),
                                            drilldown = g.Key
                                        }).ToList();

                lm = (from l in lsLoanStatus
                      select new
                      {
                          name = StatusConstant.GetStatusDescription(l.name),
                          y = l.y,
                          drilldown = String.Format("{0}_{1}", l.drilldown.ToString(), "S")
                      }).ToList<object>();

            }

            return lm;
        }

        public List<object> GetByAuditorChart(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {
                var custAuditors = (from u in db.Users.AsNoTracking()
                                    join ur in db.UserRoleMapping.AsNoTracking() on u.UserID equals ur.UserID
                                    where u.CustomerID == CustomerID && ur.RoleID == RoleConstant.QUALITY_CONTROL_AUDITOR
                                    select new { AuditorUserID = u.UserID }).ToList();


                var lsCustList = (from ca in custAuditors
                                  join l in db.Loan on ca.AuditorUserID equals l.LastAccessedUserID
                                  where l.CreatedOn >= FromDate && l.CreatedOn < ToDate
                                  group l by l.CustomerID into g
                                  select new
                                  {
                                      name = g.Key,
                                      y = g.Select(l => l.CustomerID).Count(),
                                      drilldown = g.Key
                                  }).ToList();


                lm = (from lc in lsCustList
                      join c in db.CustomerMaster.AsNoTracking() on lc.name equals c.CustomerID
                      select new
                      {
                          name = c.CustomerName,
                          y = lc.y,
                          drilldown = String.Format("{0}_{1}", lc.drilldown.ToString(), "C")
                      }).ToList<object>();

            }

            return lm;
        }


        public List<object> GetAuditStatusDrillCustomer(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 DrillStatusID, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {
                var lsCustList = db.Loan.Where(l => l.CreatedOn >= FromDate && l.CreatedOn < ToDate && l.Status == DrillStatusID)
                                        .GroupBy(l => l.CustomerID)
                                        .Select(g => new
                                        {
                                            name = g.Key,
                                            y = g.Select(l => l.CustomerID).Count(),
                                            drilldown = g.Key
                                        }).ToList();

                lm = (from lc in lsCustList
                      join c in db.CustomerMaster on lc.name equals c.CustomerID
                      select new
                      {
                          name = c.CustomerName,
                          y = lc.y,
                          drilldown = String.Format("{0}_{1}", lc.drilldown.ToString(), "C")
                      }).ToList<object>();
            }

            return lm;
        }

        public List<object> GetAuditStatusDrillLoanType(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 DrillStatusID, Int64 DrillCustID, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {

                var lsLoanTypes = db.Loan.Where(l => l.CreatedOn >= FromDate && l.CreatedOn < ToDate && l.Status == DrillStatusID && l.CustomerID == (IsSystemAdmin ? DrillCustID : CustomerID))
                                        .GroupBy(l => l.LoanTypeID)
                                        .Select(g => new
                                        {
                                            name = g.Key,
                                            y = g.Select(l => l.LoanTypeID).Count(),
                                            drilldown = g.Key
                                        }).ToList();

                lm = (from l in lsLoanTypes
                      join lt in db.LoanTypeMaster on l.name equals lt.LoanTypeID
                      select new
                      {
                          name = lt.LoanTypeName,
                          y = l.y,
                          drilldown = String.Format("{0}_{1}", l.drilldown.ToString(), "LT")
                      }).ToList<object>();

            }

            return lm;
        }

        public List<object> GetAuditStatusDrillLoan(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 DrillStatusID, Int64 DrillCustID, Int64 DrillLoanTypeID, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {

                var loans = (from L in db.Loan
                             where L.CreatedOn >= FromDate &&
                                    L.CreatedOn < ToDate &&
                                    L.Status == DrillStatusID &&
                                    L.CustomerID == (IsSystemAdmin ? DrillCustID : CustomerID) &&
                                    L.LoanTypeID == DrillLoanTypeID
                             select new
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = L.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = L.Status,
                                 LoggedUserID = L.LoggedUserID
                             }).ToList().OrderByDescending(l => l.ReceivedDate);

                var loanDetails = (from L in loans
                                   join LTM in db.LoanTypeMaster on L.LoanTypeID equals LTM.LoanTypeID
                                   join search in db.LoanSearch on L.LoanID equals search.LoanID into lu
                                   from ul in lu.DefaultIfEmpty()
                                   select new
                                   {
                                       LoanID = ul?.LoanID ?? L.LoanID,
                                       LoanNumber = ul?.LoanNumber ?? String.Empty,
                                       LoanTypeID = ul?.LoanTypeID ?? L.LoanTypeID,
                                       ReceivedDate = ul?.ReceivedDate ?? L.ReceivedDate,
                                       Status = ul?.Status ?? L.Status,
                                       LoanAmount = ul?.LoanAmount ?? 0m,
                                       LoanTypeName = LTM.LoanTypeName,
                                       BorrowerName = ul?.BorrowerName ?? String.Empty,
                                       StatusDescription = "",
                                       LoggedUserID = L.LoggedUserID
                                   }).ToList();

                List<WorkFlowStatusMaster> wfMaster = new IntellaLendDataAccess().GetWorkFlowMaster();

                lm = (from l in loanDetails.AsEnumerable()
                      join wm in wfMaster on l.Status equals wm.StatusID
                      select new
                      {
                          LoanID = l.LoanID,
                          LoanNumber = l.LoanNumber,
                          LoanTypeID = l.LoanTypeID,
                          ReceivedDate = l.ReceivedDate,
                          Status = l.Status,
                          LoanAmount = l.LoanAmount,
                          LoanTypeName = l.LoanTypeName,
                          BorrowerName = l.BorrowerName,
                          StatusDescription = wm.StatusDescription,
                          LoggedUserID = l.LoggedUserID
                      }).ToList<object>();

            }

            return lm;
        }

        public List<object> GetDrillAuditor(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 DrillCustID, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {

                var custAuditors = (from u in db.Users.AsNoTracking()
                                    join ur in db.UserRoleMapping.AsNoTracking() on u.UserID equals ur.UserID
                                    where u.CustomerID == CustomerID && ur.RoleID == RoleConstant.QUALITY_CONTROL_AUDITOR
                                    select new { AuditorUserID = u.UserID, AuditorUserName = u.LastName + " " + u.FirstName }).ToList();

                var lsCustList = (from ca in custAuditors
                                  join l in db.Loan on ca.AuditorUserID equals l.LastAccessedUserID
                                  where l.CreatedOn >= FromDate && l.CreatedOn < ToDate && l.CustomerID == (IsSystemAdmin ? DrillCustID : CustomerID)
                                  group l by l.LastAccessedUserID into g
                                  select new
                                  {
                                      name = g.Key,
                                      y = g.Select(l => l.LastAccessedUserID).Count(),
                                      drilldown = g.Key
                                  }).ToList();             

                lm = (from l in lsCustList
                      join ca in custAuditors on l.name equals ca.AuditorUserID
                      select new
                      {
                          name = ca.AuditorUserName,
                          y = l.y,
                          drilldown = String.Format("{0}_{1}", l.drilldown.ToString(), "A")
                      }).ToList<object>();

            }

            return lm;
        }

        public List<object> GetDrillLoan(Int64 RoleId, Int64 UserId, Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 DrillCustID, Int64 DrillAuditorID, bool IsSystemAdmin)
        {
            List<object> lm = null;

            using (var db = new DBConnect(TenantSchema))
            {

                var loans = (from L in db.Loan
                             where L.CreatedOn >= FromDate &&
                                    L.CreatedOn < ToDate &&                                   
                                    L.CustomerID == (IsSystemAdmin ? DrillCustID : CustomerID) &&
                                    L.LastAccessedUserID == DrillAuditorID
                             select new
                             {
                                 LoanID = L.LoanID,
                                 LoanNumber = L.LoanNumber,
                                 LoanTypeID = L.LoanTypeID,
                                 ReceivedDate = L.CreatedOn,
                                 Status = L.Status,
                                 LoggedUserID = L.LoggedUserID
                             }).ToList().OrderByDescending(l => l.ReceivedDate);

                var loanDetails = (from L in loans
                                   join LTM in db.LoanTypeMaster on L.LoanTypeID equals LTM.LoanTypeID
                                   join search in db.LoanSearch on L.LoanID equals search.LoanID into lu
                                   from ul in lu.DefaultIfEmpty()
                                   select new
                                   {
                                       LoanID = ul?.LoanID ?? L.LoanID,
                                       LoanNumber = ul?.LoanNumber ?? String.Empty,
                                       LoanTypeID = ul?.LoanTypeID ?? L.LoanTypeID,
                                       ReceivedDate = ul?.ReceivedDate ?? L.ReceivedDate,
                                       Status = ul?.Status ?? L.Status,
                                       LoanAmount = ul?.LoanAmount ?? 0m,
                                       LoanTypeName = LTM.LoanTypeName,
                                       BorrowerName = ul?.BorrowerName ?? String.Empty,
                                       StatusDescription = "",
                                       LoggedUserID = L.LoggedUserID
                                   }).ToList();

                List<WorkFlowStatusMaster> wfMaster = new IntellaLendDataAccess().GetWorkFlowMaster();

                lm = (from l in loanDetails.AsEnumerable()
                      join wm in wfMaster on l.Status equals wm.StatusID
                      select new
                      {
                          LoanID = l.LoanID,
                          LoanNumber = l.LoanNumber,
                          LoanTypeID = l.LoanTypeID,
                          ReceivedDate = l.ReceivedDate,
                          Status = l.Status,
                          LoanAmount = l.LoanAmount,
                          LoanTypeName = l.LoanTypeName,
                          BorrowerName = l.BorrowerName,
                          StatusDescription = wm.StatusDescription,
                          LoggedUserID = l.LoggedUserID
                      }).ToList<object>();

            }

            return lm;
        }

        public object GetAuditKpiGoalConfigDetails(Int64 RoleId, Int64 UserGroupID, DateTime FromDate, DateTime ToDate, bool Flag, Int64 _auditGoalID)
        {
            object data = null;
            using (var db = new DBConnect(TenantSchema))
            {
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
                List<AuditGoalConfigDetails> ListData = new List<AuditGoalConfigDetails>();
                if (Flag == true)
                {
                    AuditKpiGoalConfig _auditkpigoalconfig = db.AuditKpiGoalConfig.AsNoTracking().Where(x => x.UserGroupID == UserGroupID && x.AuditGoalID == _auditGoalID).FirstOrDefault();

                    List<AuditUserKpiGoalConfig> _auditusergroupconfig = db.AuditUserKpiGoalConfig.AsNoTracking().Where(x => x.UserGroupID == UserGroupID && (x.PeriodFrom >= FromDate && x.PeriodTo < ToDate)).ToList();

                    foreach (AuditUserKpiGoalConfig items in _auditusergroupconfig)
                    {
                        DateTime _FromDate = new DateTime(items.PeriodFrom.GetValueOrDefault().Year, items.PeriodFrom.GetValueOrDefault().Month, items.PeriodFrom.GetValueOrDefault().Day, 0, 0, 0);
                        DateTime _ToDate = new DateTime(items.PeriodTo.GetValueOrDefault().Year, items.PeriodTo.GetValueOrDefault().Month, items.PeriodTo.GetValueOrDefault().Day, 0, 0, 0).AddDays(1);

                        List<Loan> AuditKpiloandetails = new List<Loan>();
                        AuditKpiloandetails = (from l in db.Loan.AsNoTracking()
                                               where
                                                      l.CompletedUserID == items.UserID && l.CompletedUserRoleID == RoleId
                                                      && (l.AuditCompletedDate >= _FromDate && l.AuditCompletedDate < _ToDate)
                                                      && (l.Status == StatusConstant.COMPLETE)
                                               select l).Distinct().ToList();


                        List<AuditGoalConfigDetails> Auditkpidata = new List<AuditGoalConfigDetails>();
                        if (AuditKpiloandetails.Count > 0)
                        {
                            Auditkpidata = (from Auditkpi in AuditKpiloandetails
                                            group Auditkpi by new
                                            {
                                                Auditkpi.CompletedUserID,
                                                Auditkpi.Status
                                            } into AuditKpiList
                                            select new AuditGoalConfigDetails
                                            {
                                                UserID = AuditKpiList.Key.CompletedUserID,
                                                UserGroupID = UserGroupID,
                                                AchievedGoalCount = AuditKpiloandetails.Count(),
                                                Goal = _auditkpigoalconfig.Goal,
                                            }).Distinct().ToList();
                        }
                        else
                        {
                            AuditGoalConfigDetails _tempdata = new AuditGoalConfigDetails();
                            _tempdata.UserID = 0;
                            _tempdata.UserGroupID = UserGroupID;
                            _tempdata.AchievedGoalCount = 0;
                            _tempdata.Goal = _auditkpigoalconfig.Goal;
                            Auditkpidata.Add(_tempdata);
                        }
                        foreach (AuditGoalConfigDetails item in Auditkpidata)
                        {
                            ListData.Add(item);
                        }
                    }
                }
                else
                {
                    KpiUserGroupConfig Grplstobj = db.KpiUserGroupConfig.AsNoTracking().Where(x => x.GroupID == UserGroupID).FirstOrDefault();
                    List<KPIGoalConfig> _kpigoalList = db.KPIGoalConfig.Where(a => a.UserGroupID == UserGroupID && (a.PeriodFrom >= FromDate && a.PeriodTo < ToDate)).ToList();
                    foreach (KPIGoalConfig kpiItem in _kpigoalList)
                    {
                        // List<KpiUserConfigList> data = new List<KpiUserConfigList>();
                        DateTime _FromDate = new DateTime(kpiItem.PeriodFrom.GetValueOrDefault().Year, kpiItem.PeriodFrom.GetValueOrDefault().Month, kpiItem.PeriodFrom.GetValueOrDefault().Day, 0, 0, 0);
                        DateTime _ToDate = new DateTime(kpiItem.PeriodTo.GetValueOrDefault().Year, kpiItem.PeriodTo.GetValueOrDefault().Month, kpiItem.PeriodTo.GetValueOrDefault().Day, 0, 0, 0).AddDays(1);

                        List<Loan> Kpiloandetails = new List<Loan>();
                        Kpiloandetails = (from l in db.Loan.AsNoTracking()
                                          where
                                                 l.CompletedUserID == kpiItem.UserID && l.CompletedUserRoleID == RoleId
                                                 && (l.AuditCompletedDate >= _FromDate && l.AuditCompletedDate < _ToDate)
                                                 && (l.Status == StatusConstant.COMPLETE)
                                          select l).Distinct().ToList();
                        List<AuditGoalConfigDetails> kpidata = new List<AuditGoalConfigDetails>();

                        if (Kpiloandetails.Count > 0)
                        {
                            kpidata = (from kpi in Kpiloandetails
                                       group kpi by new
                                       {
                                           kpi.CompletedUserID,
                                           kpi.Status
                                       } into KpiList
                                       select new AuditGoalConfigDetails
                                       {

                                           UserID = KpiList.Key.CompletedUserID,
                                           UserGroupID = Grplstobj.GroupID,
                                           AchievedGoalCount = Kpiloandetails.Count(),
                                           Goal = Grplstobj.Goal,
                                       }).Distinct().ToList();
                        }
                        else
                        {
                            AuditGoalConfigDetails _tempdata = new AuditGoalConfigDetails();
                            _tempdata.UserID = 0;
                            _tempdata.UserGroupID = Grplstobj.GroupID;
                            _tempdata.AchievedGoalCount = 0;
                            _tempdata.Goal = Grplstobj.Goal;
                            kpidata.Add(_tempdata);
                        }
                        //data = data.Count > 0 ? data : kpidata;
                        foreach (AuditGoalConfigDetails item in kpidata)
                        {
                            ListData.Add(item);
                        }

                    }
                }
                data = (from kpi in ListData
                        group kpi by new
                        {
                            kpi.UserID,
                            kpi.UserGroupID
                        } into _KpiList
                        select new AuditGoalConfigDetails
                        {
                            UserID = _KpiList.Key.UserID,
                            UserGroupID = _KpiList.Key.UserGroupID,
                            AchievedGoalCount = ListData.Where(k => k.UserGroupID == _KpiList.Key.UserGroupID).Select(k => k.AchievedGoalCount).Sum(),
                            Goal = ListData.Where(k => k.UserGroupID == _KpiList.Key.UserGroupID).FirstOrDefault().Goal,
                        }).ToList();
            }
            return data;

        }
        #endregion
    }


    class AuditGoalConfigDetails
        {
            public Int64 UserID { get; set; }
            public Int64 AuditGoalID { get; set; }
            public Int64 UserGroupID { get; set; }
            public Int64 RoleID { get; set; }
            public DateTime PeriodFrom { get; set; }
            public DateTime PeriodTo { get; set; }
            public Int64 Goal { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public Int64 AchievedGoalCount { get; set; }
        }
    }
