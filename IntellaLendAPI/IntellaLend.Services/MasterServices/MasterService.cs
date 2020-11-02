using IntellaLend.EntityDataHandler;
using IntellaLend.Model;
using System;
using System.Collections.Generic;

namespace IntellaLend.CommonServices
{
    public class MasterService
    {
        private static string TenantSchema;

        #region Constructor

        public MasterService()
        { }

        public MasterService(string tenantSchema)
        {
            TenantSchema = tenantSchema;
        }

        #endregion

        #region Public Methods        

        public List<RoleMaster> GetRoleMaster()
        {
            return new MasterDataAccess(TenantSchema).GetRoleMaster();
        }
        public List<RoleMasterADGroup> GetAllRoleMasterList()
        {
            return new MasterDataAccess(TenantSchema).GetAllRoleMasterList();
        }

        public List<ADGroupMasters> GetAllADGroupMasterList()
        {
            return new MasterDataAccess(TenantSchema).GetAllADGroupMasterList();
        }

        public List<CustomerMaster> GetCustomerList()
        {
            return new MasterDataAccess(TenantSchema).GetCustomerList();
        }


        public List<ReviewPriorityMaster> GetReviewPriorityMaster()
        {
            return new MasterDataAccess(TenantSchema).GetReviewPriorityMaster();
        }
        public List<EncompassParkingSpot> GetEncompassParkingSpot()
        {
            return new MasterDataAccess().GetEncompassParkingSpot();
        }

        #region ReviewType

        public List<SystemReviewTypeMaster> GetReviewTypeMaster(bool activeFilter)
        {
            return new MasterDataAccess(TenantSchema).GetReviewTypeMaster(activeFilter);
        }

        public ReviewTypeMaster GetReviewType(Int64 ReviewTypeID)
        {
            return new MasterDataAccess(TenantSchema).GetReviewType(ReviewTypeID);
        }

        public bool UpdateReviewType(ReviewTypeMaster ReviewType)
        {
            return new MasterDataAccess(TenantSchema).UpdateReviewType(ReviewType);
        }

        public object AddReviewType(SystemReviewTypeMaster ReviewType)
        {
            return new IntellaLendDataAccess(TenantSchema).AddObjectReviewType(ReviewType);
        }

        #endregion
        
        #region CheckList
        public List<CheckListMaster> GetCheckListMaster(bool activeFilter)
        {
            return new MasterDataAccess(TenantSchema).GetCheckListMaster(activeFilter);
        }

        public CheckListMaster GetCheckList(Int64 CheckListID)
        {
            return new MasterDataAccess(TenantSchema).GetCheckList(CheckListID);
        }

        public bool UpdateCheckList(CheckListMaster CheckList)
        {
            return new MasterDataAccess(TenantSchema).UpdateCheckList(CheckList);
        }

        public bool AddCheckList(CheckListMaster CheckList)
        {
            return new MasterDataAccess(TenantSchema).AddCheckList(CheckList);
        }
        #endregion
        
        #region StackingOrder
        public List<StackingOrderMaster> GetStackingOrderMaster(bool activeFilter)
        {
            return new MasterDataAccess(TenantSchema).GetStackingOrderMaster(activeFilter);
        }

        public StackingOrderMaster GetStackingOrder(Int64 StackingOrderID)
        {
            return new MasterDataAccess(TenantSchema).GetStackingOrder(StackingOrderID);
        }

        public bool UpdateStackingOrder(StackingOrderMaster StackingOrder)
        {
            return new MasterDataAccess(TenantSchema).UpdateStackingOrder(StackingOrder);
        }

        public bool AddStackingOrder(StackingOrderMaster StackingOrder)
        {
            return new MasterDataAccess(TenantSchema).AddStackingOrder(StackingOrder);
        }
        #endregion

        #region LoanType
        public object GetLoanTypeMaster(bool activeFilter)
        {
            return new MasterDataAccess(TenantSchema).GetLoanTypeMaster(activeFilter);
        }

        public LoanTypeMaster GetLoanType(Int64 LoanTypeID)
        {
            return new MasterDataAccess(TenantSchema).GetLoanType(LoanTypeID);
        }

        public bool UpdateLoanType(LoanTypeMaster loanType)
        {
            return new MasterDataAccess(TenantSchema).UpdateLoanType(loanType);
        }

        public object AddLoanType(SystemLoanTypeMaster loanType)
        {
            return new IntellaLendDataAccess(TenantSchema).AddSingleLoanType(loanType);
        }
        #endregion

        public bool UpdateDocumentField(DocumentFieldMaster Field, Int64 AssignedFieldID)
        {
            return new MasterDataAccess(TenantSchema).UpdateDocumentField(Field, AssignedFieldID);
        }

        public bool CheckDocumentDup(string DocumentTypeName)
        {
            return new MasterDataAccess().CheckDocumentDup(DocumentTypeName);
        }
        public List<DocumentTypeMaster> CheckDocumentDupForEdit(string DocumentTypeName)
        {
            return new MasterDataAccess().CheckDocumentDupForEdit(DocumentTypeName);
        }
      
        public Int64 AddDocumentType(string DocumentTypeName, string DocumentDisplayName, Int32 DocumentLevel, Int32 EncompassParkingSpotID)
        {
            
return new MasterDataAccess().AddDocumentType(DocumentTypeName, DocumentDisplayName, DocumentLevel,EncompassParkingSpotID);
        }

        public List<SecurityQuestionMasters> GetSecurityQuestionList()
        {
            return new MasterDataAccess(TenantSchema).GetSecurityQuestionList();
        }

        public List<WorkFlowStatusMaster> GetWorkFlowMaster()
        {
            return new IntellaLendDataAccess().GetWorkFlowMaster();
        }

        public List<WorkFlowStatusMaster> GetSearchWorkFlowSatus()
        {
            return new IntellaLendDataAccess().GetSearchWorkFlowSatus();
        }
        

        public List<object> GetDocumentTypeMaster(bool activeFilter)
        {
            return new MasterDataAccess(TenantSchema).GetDocumentTypeMaster(activeFilter);
        }
        public List<DocumentTypeMaster> GetActiveDocumentTypeMaster()
        {
            return new MasterDataAccess(TenantSchema).GetActiveDocumentTypeMaster();
        }
        
        public bool AddDocumentType(DocumentTypeMaster docTypeMaster)
        {
            return new MasterDataAccess(TenantSchema).AddDocumentType(docTypeMaster);
        }
        

        public bool UpdateDocumentType(DocumentTypeMaster docTypeMaster,Int64 ParkingSpotID)
        {
            return new MasterDataAccess(TenantSchema).UpdateDocumentType(docTypeMaster, ParkingSpotID);
        }

        public List<WorkFlowStatusMaster> GetRetentionWorkFlowStatus()
        {
            return new IntellaLendDataAccess().GetRetentionWorkFlowStatus();
        }

        public object GetActiveDocumentTypeMasterWithCustandLoan(Int64 CustomerID, Int64 LoanTypeID)
        {
            return new MasterDataAccess(TenantSchema).GetActiveDocumentTypeMasterWithCustandLoan(CustomerID,LoanTypeID);
        }

        public bool UpdateManagerDocumentType(DocumentTypeMaster documentType,Int64 CustomerID,Int64 LoanTypeID)
        {
            return new MasterDataAccess(TenantSchema).UpdateManagerDocumentType(documentType, CustomerID, LoanTypeID);
        }
        public bool AddManagerDocumentType(DocumentTypeMaster documentType, Int64 CustomerID, Int64 LoanTypeID)
        {
            return new MasterDataAccess(TenantSchema).AddManagerDocumentType(documentType, CustomerID, LoanTypeID);
        }

        public List<DocumentTypeMaster> CheckManagerDocumentDupForEdit(string DocumentTypeName)
        {
            return new MasterDataAccess().CheckManagerDocumentDupForEdit(DocumentTypeName);
        }
        public List<User> GetUserMasters()
        {
            return new MasterDataAccess(TenantSchema).GetUserMasters();
        }

        #region RoleMaster
        public object AddRoleDetails(RoleMaster roletype,List<MenuMaster> menus)
        {
            return new IntellaLendDataAccess(TenantSchema).AddRoleDetails(roletype, menus);
        }
        public object UpdateRoleDetails(RoleMaster roletype,List<MenuMaster> menus)
        {
            return new IntellaLendDataAccess(TenantSchema).UpdateRoleDetails(roletype, menus);
        }
        public object ChecUserRoleDetails(Int64 RoleID)
        {
            return new IntellaLendDataAccess(TenantSchema).ChecUserRoleDetails(RoleID);
        }

        #endregion

        #region Kpi User List
        public object GetUserRoleList (Int64 RoleID)
        {
            return new IntellaLendDataAccess(TenantSchema).GetUserRoleList(RoleID);
        }
        public object SaveKpiConfigurationDetails(KpiUserGroupConfig kpiUserGroupConfig, List<KPIGoalConfig> kpiconfig, bool IsExistNewUserGrp)
        {
            return new IntellaLendDataAccess(TenantSchema).SaveKpiConfigurationDetails(kpiUserGroupConfig, kpiconfig, IsExistNewUserGrp);
        }
        public object SaveKpiConfigurationDetails(Int64 groupID,int configType,Int64 goal)
        {
            return new IntellaLendDataAccess(TenantSchema).SaveKpiConfigurationDetails(groupID, configType, goal);
        }
        public object UpdateKPIConfigStagingData(Int64 ID,Int64 groupID, int configType, Int64 goal, int status)
        {
            return new IntellaLendDataAccess(TenantSchema).UpdateKPIConfigStagingData(ID,groupID, configType, goal);
        }
        public object GetKPIGoalConfigStagingDetails(Int64 groupID, int configType)
        {
            return new IntellaLendDataAccess(TenantSchema).GetKPIGoalConfigStagingDetails(groupID, configType);
        }
        

        #endregion

        #region Encompass Parking Spot
        public object AddParkingSpot(string ParkSpotName , Boolean Active)
        {
            return new MasterDataAccess().AddParkingSpot(ParkSpotName, Active);
        }
        public object UpdateParkingSpot(string ParkSpotName, Boolean Active,Int64 ParkingSpotID)
        {
            return new MasterDataAccess().UpdateParkingSpot(ParkSpotName, Active,ParkingSpotID);
        }
        
        #endregion
        #endregion
    }
}
