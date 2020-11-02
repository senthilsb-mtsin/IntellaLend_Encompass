using IntellaLend.Constance;
using IntellaLend.EntityDataHandler;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class DashboardService
    {
        private string TenantSchema;

        #region Constructor

        public DashboardService()
        { }

        public DashboardService(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods

        public List<object> GetNeedsAttention(Int64 CurrentUserID)
        {
            return new DashboardDataAccess(TenantSchema).GetNeedsAttention(CurrentUserID);
        }

        public List<object> GetAuditStatus(Int64 RoleID, Int64 CurrentUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate)
        {
            bool IsSystemAdmin = RoleID.Equals(RoleConstant.SYSTEM_ADMINISTRATOR);
            return new DashboardDataAccess(TenantSchema).GetAuditStatus(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, IsSystemAdmin);
        }

        public List<object> GetAuditStatusDrill(Int64 RoleID, Int64 CurrentUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, string Type, Int64 DrillStatusID, Int64 DrillCustID, Int64 DrillLoanTypeID)
        {
            bool IsSystemAdmin = RoleID.Equals(RoleConstant.SYSTEM_ADMINISTRATOR);

            if (Type.Equals("S"))
            {
                if (IsSystemAdmin)
                    return new DashboardDataAccess(TenantSchema).GetAuditStatusDrillCustomer(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillStatusID, IsSystemAdmin);
                else
                    return new DashboardDataAccess(TenantSchema).GetAuditStatusDrillLoanType(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillStatusID, DrillCustID, IsSystemAdmin);
            }
            else if (Type.Equals("C"))
                return new DashboardDataAccess(TenantSchema).GetAuditStatusDrillLoanType(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillStatusID, DrillCustID, IsSystemAdmin);
            else if (Type.Equals("LT"))
                return new DashboardDataAccess(TenantSchema).GetAuditStatusDrillLoan(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillStatusID, DrillCustID, DrillLoanTypeID, IsSystemAdmin);

            return null;
        }

        public List<object> GetByAuditorChart(Int64 RoleID, Int64 CurrentUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate)
        {
            bool IsSystemAdmin = RoleID.Equals(RoleConstant.SYSTEM_ADMINISTRATOR);
            return new DashboardDataAccess(TenantSchema).GetByAuditorChart(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, IsSystemAdmin);
        }

        public List<object> GetByAuditorDrillChart(Int64 RoleID, Int64 CurrentUserID, Int64 CustomerID, DateTime FromDate, DateTime ToDate, string Type, Int64 DrillCustID, Int64 DrillAuditorID)
        {
            bool IsSystemAdmin = RoleID.Equals(RoleConstant.SYSTEM_ADMINISTRATOR);

            //if (Type.Equals("S"))
            //{
            //    if (IsSystemAdmin)
            //        return new DashboardDataAccess(TenantSchema).GetAuditStatusDrillCustomer(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillStatusID, IsSystemAdmin);
            //    else
            //        return new DashboardDataAccess(TenantSchema).GetAuditStatusDrillLoanType(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillStatusID, DrillCustID, IsSystemAdmin);
            //}
            //else
            if (Type.Equals("C"))
                return new DashboardDataAccess(TenantSchema).GetDrillAuditor(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillCustID, IsSystemAdmin);
            else if (Type.Equals("A"))
                return new DashboardDataAccess(TenantSchema).GetDrillLoan(RoleID, CurrentUserID, CustomerID, FromDate, ToDate, DrillCustID, DrillAuditorID, IsSystemAdmin);

            return null;
        }
        
        public object GetAuditKpiGoalConfigDetails(Int64 RoleId,Int64 UserGroupID, DateTime FromDate, DateTime ToDate,bool Flag,Int64 auditGoalID)
        {
            return new DashboardDataAccess(TenantSchema).GetAuditKpiGoalConfigDetails(RoleId, UserGroupID, FromDate, ToDate, Flag, auditGoalID);
        }
        #endregion

    }
}
