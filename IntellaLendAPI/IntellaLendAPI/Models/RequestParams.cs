using IntellaLend.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace IntellaLendAPI.Models
{
    #region MAS

    public class MASUpdateLOSExportFileStagingRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public int FileType { get; set; }
        public Int64 LoanID { get; set; }
        public string BatchID { get; set; }
        public string BatchClassID { get; set; }
        public string BCName { get; set; }
        public string BatchName { get; set; }
        public List<MASDocument> MASDocumentList { get; set; }

    }


    #endregion MAS

    public class QCIQDBDetailsRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }

    public class MissingDocRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }


    public class QCIQUpdateLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanType { get; set; }
        public DateTime? QCIQStartDate { get; set; }
    }
    public class EncompassUpdateLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanNumber { get; set; }
        public string BorrowerName { get; set; }
    }

    public class EphesoftLoanDetailsRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
        public string BatchID { get; set; }
        public string BatchClassID { get; set; }
        public string BatchClassName { get; set; }
    }



    public class EncompassDocPagesRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocID { get; set; }
    }


    public class EphesoftLoanPageCountRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 PageCount { get; set; }
    }


    public class EphesoftLoanDetailsResponse : IntellaLendResponse
    {
        public string LoanDetailsJson { get; set; }
    }

    public class EncompassDocPagesResponse : IntellaLendResponse
    {
        public bool isEncompassLoan { get; set; }
        public string EncompassDocPages { get; set; }
    }


    public class WebExceptionResponse : IntellaLendResponse
    {
        public bool Status { get; set; }
    }

    public class QCIQDBDetailsResponse : IntellaLendResponse
    {
        public string data { get; set; }
    }

    public class UserDetailsRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public User user { get; set; }
    }

    public class UserRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string UserName { get; set; }
    }
    public class GetUserRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string UserName { get; set; }
    }
    public class GetServiceBasedUserRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string ServiceTypeName { get; set; }
    }
    public class RoleListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 RoleID { get; set; }
        public Int64 UserID { get; set; }
    }

    public class UserListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 RoleID { get; set; }
    }

    public class LockUnlockUserRquest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int32 UserID { get; set; }
        public bool Lock { get; set; }
    }

    public class CommonListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
    }

    public class TokenResponse : IntellaLendResponse
    {
        public string token { get; set; }
        public string data { get; set; }
    }

    public class UserSecurityQuestionRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string NewPassword { get; set; }
        public UserSecurityQuestion SecurityQuestion { get; set; }
    }
    public class CustomerRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CustomerMaster customerMaster { get; set; }
    }
    public class CustomerUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CustomerMaster customerMaster { get; set; }
    }

    public class UserPasswordUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string NewPassword { get; set; }
        public Int64 UserID { get; set; }
        public string CurrentPassword { get; set; }
        public string NoOfOldPassword { get; set; }
    }

    public class UserPasswordResetRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string NewPassword { get; set; }
        public Int64 UserID { get; set; }
        public string CurrentPassword { get; set; }
    }

    public class RequestCustLoanMapping : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
    }

    public class RequestRetentionPurge : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64[] LoanIDs { get; set; }
        public Int64 UserID { get; set; }
        public string UserName { get; set; }

    }
    public class RequestCustLoanReviewMapping : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
    }
    public class RequestCustLoanReviewStackMapping : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public int CustomerID { get; set; }
        public int LoanTypeID { get; set; }
        public int ReviewTypeID { get; set; }
    }
    public class RequestCustLoanReviewCheckListMapping : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string BoxUploadPath { get; set; }
        public string LoanUploadPath { get; set; }
        public bool isRetainUpdate { get; set; }
    }

    public class RequestCheckLoanPath : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string LoanUploadPath { get; set; }
    }

    public class RequestCustLoanReviewDocTypeMapping : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        //public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
    }
    public class RequestCustLoanDocMapping : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
    }

    public class CustReviewLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public bool isSaveEdit { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReviewTypeID { get; set; }
    }

    public class CloneFromSystemRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64[] LoanTypeIDs { get; set; }
        public Int64 ReviewTypeID { get; set; }
    }

    public class UpdateKPIConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ID { get; set; }
        public Int64 Goal { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public Int64 UserGroupGoal { get; set; }
    }

    #region Stacking Order
    public class RequestSearchStackingOrder : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public long StackingOrderID { get; set; }
    }
    public class RequestSaveSearchStackingOrder : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public List<StackingOrderDetailMaster> StackingOrderDetails { get; set; }
        public List<GetStackOrder> StackOrder { get; set; }
        public string StackOrderName { get; set; }
        public bool IsActive { get; set; }
        public int StackOrderID { get; set; }
    }

    public class RequestSaveStackingOrderGroupField : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public List<StackingOrderDocumentFieldMaster> StackingOrderDetails { get; set; }
        public GetStackOrder StackOrder { get; set; }
    }
    public class RequestSystemStackingOrderDetailMaster : IntellaLendRequest
    {
        public Int64 stackingOrderID { get; set; }
    }

    public class RequestSetOrderByField : IntellaLendRequest
    {
        public Int64 DocumentTypeID { get; set; }
        public Int64 FieldID { get; set; }
    }

    public class RequestSetTenantOrderByField : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 DocumentTypeID { get; set; }
        public Int64 FieldID { get; set; }
    }


    #endregion

    #region Customer Config
    public class RequestAddRetention : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public int CustomerID { get; set; }
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class RequestAddMultipleRetention : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public int CustomerID { get; set; }
        //public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public List<CustomerConfigItem> custConfigItems { get; set; }
    }


    public class RequestGetAllRetention : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public bool Active { get; set; }
    }

    public class RequestEditRetention : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CustomerConfig updatecustomerconfiguration { get; set; }
    }

    public class RequestDeleteRetention : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public long CustomerID { get; set; }
    }
    #endregion

    #region Loan Requests

    public class LoanSearchRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [DefaultDateTimeValueAttribute("FromDate")]
        public string FromDateStr { get; set; }
        [DefaultDateTimeValueAttribute("ToDate")]
        public string ToDateStr { get; set; }

        public Int64 CurrentUserID { get; set; }
        public string LoanNumber { get; set; }
        public Int64 LoanType { get; set; }
        public string BorrowerName { get; set; }
        public string LoanAmount { get; set; }
        public Int64 ReviewStatus { get; set; }
        public DateTime? AuditMonthYear { get; set; }
        public Int64 ReviewType { get; set; }
        public Int64 Customer { get; set; }
        public string PropertyAddress { get; set; }
        public string InvestorLoanNumber { get; set; }
        public string PostCloser { get; set; }
        public string LoanOfficer { get; set; }
        public string UnderWriter { get; set; }
        public DateTime AuditDueDate { get; set; }
        public string[] SelectedLoanStatus { get; set; }

    }
    public class ExportToBoxRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }

    public class LoanAuditRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }
    public class DashSearchRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public DateTime AuditMonthYear { get; set; }
    }

    public class DocumentDownloadRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }
        public Int64 DocumentVersion { get; set; }
    }

    public class UpdateLoanNumberRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string LoanDetails { get; set; }
        public string UserName { get; set; }
        public Int64 Type { get; set; }
    }
    public class UpdateLoanHeader : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public LoanHeader LoanDetails { get; set; }
        public string UserName { get; set; }

    }
    public class LoanRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public bool ReRun { get; set; }
        public Int64 CompletedUserRoleID { get; set; }
        public Int64 CompletedUserID { get; set; }
        public string CompleteNotes { get; set; }
        public string UserName { get; set; }
    }

    public class DeleteLoanRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64[] LoanID { get; set; }
    }

    public class LoanDocVersionRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }

    }

    public class ChecklistDownloadRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public List<ChecklistObject> CheckList { get; set; }
    }


    public class LoanQuestionerUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public List<ManualQuestioner> Questioners { get; set; }
        public Int64 CurrentUserID { get; set; }
    }

    public class LoanNotesUpdateRequest : IntellaLendResponse
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string Notes { get; set; }
    }
    public class DocChangeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 OldDocumentID { get; set; }
        public Int64 NewDocumentID { get; set; }
        public int VersionNumber { get; set; }
        public Int64 CurrentUserID { get; set; }
    }

    public class LoanDocUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }
        public Int64 CurrentUserID { get; set; }
        public int VersionNumber { get; set; }
        public List<DocumentLevelFields> DocumentFields { get; set; }
        public List<DataTable> DocumentTables { get; set; }
    }


    public class LoanTypeMonitorUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string UserName { get; set; }

    }

    public class LoanPickUpUserRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int32 PickUpUserID { get; set; }
    }

    public class ImageRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ImageID { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }
        public Int64 PageNo { get; set; }
        public int VersionNumber { get; set; }
        public Boolean ShowAllDocs { get; set; }
        public Int64 LastPageNumber { get; set; }
    }

    public class LoanDocumentRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }
        public int VersionNumber { get; set; }
    }

    public class LoanReverifyDocumentRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }
        public int VersionNumber { get; set; }
    }

    public class RequestPurgeMonitorData : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Int64 WorkFlowStatus { get; set; }
    }
    public class CheckCurrentLoanUserRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 CurrentUserID { get; set; }
    }
    public class RequestLoanPurge : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        //public long BatchCount { get; set; }
        public string UserName { get; set; }
        //public long Status { get; set; }
        public Int64[] LoanID { get; set; }
        public PurgeStaging purgeStaging { get; set; }
    }
    public class RequestRetryLoanPurge : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64[] BatchIDs { get; set; }
        //public Int64[] LoanID { get; set; }
    }
    public class RequestPurgeStagingDetails : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 BatchID { get; set; }

    }

    public class RequestLoanReverification : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }

    public class DeleteLoans : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64[] LoanID { get; set; }
        public string userName { get; set; }

    }
    #endregion

    #region Masters Requests

    public class DocumentTypeDupRequest : IntellaLendRequest
    {
        public string DocumentTypeName { get; set; }
    }

    public class AddDocumentTypeRequest : IntellaLendRequest
    {
        public string DocumentTypeName { get; set; }
        public string DocumentDisplayName { get; set; }
        public string DocumentLevel { get; set; }
        public Int64 ParkingSpotID { get; set; }
    }

    public class UpdateLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public LoanTypeMaster LoanType { get; set; }
    }

    public class SystemLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public SystemLoanTypeMaster LoanType { get; set; }
    }

    public class SyncCustomerLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 SyncLevel { get; set; }
    }

    public class GetLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanTypeID { get; set; }
    }

    public class RequestGetFieldMaster : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64[] DocumentTypeID { get; set; }
    }

    public class RequestGetDocFieldMaster : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        //public Int64 CustomerID { get; set; }
        //public Int64 LoanTypeID { get; set; }
    }
    public class RequestGetCustLoanTypes : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }

    }
    public class UpdateReviewTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public ReviewTypeMaster ReviewType { get; set; }
    }
    public class SystenReviewTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public SystemReviewTypeMaster ReviewType { get; set; }
    }

    public class GetReviewTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ReviewTypeID { get; set; }
    }

    public class UpdateCheckListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CheckListMaster CheckList { get; set; }
    }

    public class GetCheckListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CheckListID { get; set; }
    }


    public class UpdateStackingOrderRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public StackingOrderMaster StackingOrder { get; set; }
    }

    public class GetStackingOrderRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 StackingOrderID { get; set; }
    }

    public class UpdateDocumentTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public DocumentTypeMaster documentType { get; set; }
        public Int64 ParkingSpotID { get; set; }
    }


    public class UpdateDocumentManagerTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public DocumentTypeMaster documentType { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
    }

    public class AddCheckListDetails : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CheckListDetailMaster CheckListDetailMaster { get; set; }
        public RuleMaster rulemasters { get; set; }
        public Int64 LoanTypeID { get; set; }
    }

    public class AddCSysheckListDetails : IntellaLendRequest
    {
        public CheckListDetailMaster CheckListDetailMaster { get; set; }
        public RuleMaster rulemasters { get; set; }
        public Int64 LoanTypeID { get; set; }
    }
    public class RequestCheckListItems : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CheckListDetailID { get; set; }
        public Int64 LoanTypeID { get; set; }
    }
    public class RequestSysCheckListMasters : IntellaLendRequest
    {
        public CheckListMaster checkListMaster { get; set; }
    }


    #endregion

    #region Dashboard Requests

    public class DashboardRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 UserID { get; set; }
        public Int64 RoleID { get; set; }
        public Int64 CustomerID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Type { get; set; }
        public Int64 DrillStatusID { get; set; }
        public Int64 DrillCustomerID { get; set; }
        public Int64 DrillLoanTypeID { get; set; }
        public Int64 DrillAuditorID { get; set; }
    }

    public class AuditKpiRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 UserGroupID { get; set; }
        public Int64 RoleID { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public bool Flag { get; set; }
        public Int64 AuditGoalID { get; set; }
    }
    #endregion

    #region Checklist Request
    public class RequestDeleteCheckListItem : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64[] CheckListDetailsID { get; set; }
        public Int64 LoanTypeID { get; set; }
    }

    public class AddSysCheckList : IntellaLendRequest
    {
        public Int64 CheckListID { get; set; }
        public string CheckListName { get; set; }
        public bool Active { get; set; }
        public Boolean Sync { get; set; }
    }

    public class AssignSysCheckList : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
        public Int64 CheckListID { get; set; }
        public string CheckListName { get; set; }
    }

    public class AssignSysStackingOrder : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
        public Int64 StackingOrderID { get; set; }
        public string StackingOrderName { get; set; }
    }

    public class RequestCloneCheckListItem : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string ModifiedCheckListName { get; set; }
        public Int64[] CheckListDetailsID { get; set; }
        public Int64 LoanTypeID { get; set; }

    }
    public class RequestCheckListItemsValues : IntellaLendRequest
    {
        public Dictionary<string, object> CheckListItemValues { get; set; }
        public string RuleFormula { get; set; }
    }

    public class GetCheckListGroups : IntellaLendRequest
    {
        public string TableSchema { get; set; }
    }

    public class Requestchecklist : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanTypeID { get; set; }
    }

    public class RequestchecklistUpdate : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CheckListID { get; set; }
        public Boolean Sync { get; set; }

    }
    #endregion

    #region Box Upload

    public class BoxTokenValidateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 UserID { get; set; }
        public string AuthCode { get; set; }
    }

    public class BoxGetFileListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 UserID { get; set; }
        public string FolderID { get; set; }
        public int limit { get; set; }
        public int offSet { get; set; }
        public string FileFilter { get; set; }
    }


    public class BoxUploadRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 UserID { get; set; }
        public string AuthCode { get; set; }
        public Int64 ReviewType { get; set; }
        public Int64 LoanType { get; set; }
        public Int64 CustomerID { get; set; }
        //[DefaultDateTimeValue("Now")]
        public DateTime? AuditMonthYear { get; set; }
        public DateTime? AuditDueDate { get; set; }
        public List<BoxItem> BoxItems { get; set; }
        public string FileFilter { get; set; }
    }

    public class BoxUploadSearchRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 UserID { get; set; }
        public Int64 CustomerID { get; set; }
        public int UploadStatus { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [DefaultDateTimeValueAttribute("FromDate")]
        public string FromDateStr { get; set; }
        [DefaultDateTimeValueAttribute("ToDate")]
        public string ToDateStr { get; set; }
    }

    public class BoxUploadResetRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
    }


    public class EphesoftRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string EphesoftBatchInstanceID { get; set; }
        public string EphesoftURL { get; set; }
        public Int64 CustomerID { get; set; }

    }
    //public class BoxItem
    //{
    //    public string BoxID { get; set; }
    //    public string ItemType { get; set; }
    //    public Int32 Priority { get; set; }
    //}
    public class BoxItemsCountsWithDuplicateFileLists
    {
        public Int64 BoxItemCounts { get; set; }

        public object BoxDuplicateFileList { get; set; }

        public bool IsNonDupFileAdded { get; set; }
    }


    #endregion

    #region Tenant Configuration Request

    public class TenantConfigRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public string ConfigKey { get; set; }
    }

    public class BoxSettingsConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string ClientId { get; set; }
        public string ClientSecretId { get; set; }
        public string BoxUserID { get; set; }
        public bool isUpdate { get; set; }
    }

    public class TenantAddConfigRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CustomerConfig TenantConfigType { get; set; }
    }

    public class TenantUpdateConfigRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ConfigID { get; set; }
        public bool Active { get; set; }
    }
    public class SMTPDetailsRequest : IntellaLendRequest
    {
        public SMTPDETAILS SMTPMaster { get; set; }
    }

    public class RequestPasswordPolicy : IntellaLendRequest
    {
        public PasswordPolicy passwordPolicy { get; set; }
    }

    public class CheckWebHookSubscriptionEventTypeExistRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int32 EventType { get; set; }
    }

    #endregion

    #region Document Fields Request

    public class DocumentFieldUpdateRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public DocumentFieldMaster Field { get; set; }
        public Int64 AssignedFieldID { get; set; }
    }

    public class DocumentFieldsRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
    }

    #endregion

    #region QueryAnalyzer Request

    public class QueryAnalyzerRequest : IntellaLendRequest
    {

        public string Querystring { get; set; }
    }

    public class QCIQLoanTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public List<SystemLoanTypeMaster> QCIQReq { get; set; }
    }

    public class QCIQReviewTypeRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public List<SystemReviewTypeMaster> QCIQReq { get; set; }
    }

    #endregion

    #region System Mapping
    public class RequestSaveSystemDocumentLoan : IntellaLendRequest
    {
        public Int64 DocumentTypeID { get; set; }
        public Int64[] LoanTypeIDs { get; set; }
    }

    public class RequestAddDocumentField : IntellaLendRequest
    {
        public Int64 DocumentTypeID { get; set; }
        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }
    }


    public class RequestGetAllAuditConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
    }

    public class GetAppConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
    }

    public class SaveStipulation : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string SCategory { get; set; }
        public bool Active { get; set; }
    }

    public class UpdateStipulation : IntellaLendRequest
    {
        public Int64 ID { get; set; }
        public string TableSchema { get; set; }
        public string SCategory { get; set; }
        public bool Active { get; set; }
    }


    public class RequestUpdateAuditConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public AuditDescriptionConfig AuditConfig { get; set; }
    }

    public class CategoryList : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public CategoryLists categoryList { get; set; }
    }

    public class SaveCategoryList : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string Category { get; set; }
        public bool Active { get; set; }
    }

    public class SaveReportConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 MasterID { get; set; }
        public string docName { get; set; }
        public string ServiceType { get; set; }
    }

    public class RequestDeleteDocumentField : IntellaLendRequest
    {
        public Int64 FieldID { get; set; }
    }

    public class RequestSystemLoanTypes : IntellaLendRequest
    {
        public Int64 ReviewTypeID { get; set; }
    }

    public class RequestAssignedSystemLoanTypes : IntellaLendRequest
    {
        public Int64 DocumentTypeID { get; set; }
    }

    public class RequestSystemDocTypes : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
    }

    public class RequestSetSystemDocTypes : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
        public List<DocMappingDetails> DocMappingDetails { get; set; }
    }
    public class RequestConditionRule : IntellaLendRequest
    {
        public Int64 DocumentTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public string GeneralRuleValues { get; set; }
        public string TableSchema { get; set; }

    }

    public class RequestSaveSystemReviewLoan : IntellaLendRequest
    {
        public Int64 ReviewTypeID { get; set; }
        public Int64[] LoanTypeIDs { get; set; }
    }

    public class RequestMapping : IntellaLendRequest
    {
        public Int64 ReviewTypeID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 CheckListID { get; set; }
        public Int64 StackingOrderID { get; set; }
        public bool IsAdd { get; set; }
    }

    public class RequestLoanMapping : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
        public Int64 CheckListID { get; set; }
        public Int64 StackingOrderID { get; set; }
        public List<ChecklistItemSequence> ChecklistItemSeq { get; set; }
    }

    public class LosType : IntellaLendRequest
    {
        public string LosName { get; set; }
    }

    #endregion

    #region Re-Verification

    public class RequestSaveReverification : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
        public string ReverificationName { get; set; }
        public Int64 TemplateID { get; set; }
    }

    public class RequestReverification : IntellaLendRequest
    {
        public string TableSchema { get; set; }
    }

    public class RequestUpdateReverification : IntellaLendRequest
    {
        public Int64 LoanTypeID { get; set; }
        public Int64 CustomerID { get; set; }
        public string ReverificationName { get; set; }
        public Int64 TemplateID { get; set; }
        public Int64 MappingID { get; set; }
        public Int64 ReverificationID { get; set; }
        public bool Active { get; set; }
        public string TableSchema { get; set; }
    }

    public class RequestReverificationMappedDoc : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 LoanTypeID { get; set; }
        public Int64 ReverificationID { get; set; }
    }

    public class RequestSetReverifyDocTypes : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ReverificationID { get; set; }
        public Int64[] DocTypeIDs { get; set; }
    }

    public class RequesGetReverifyTemplate : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 MappingID { get; set; }
        //public Int64 ReverificationID { get; set; }    
        public Int64 TemplateID { get; set; }
        public Int64 ReverificationID { get; set; }
    }

    public class RequesGetFields : IntellaLendRequest
    {
        public string DocumentName { get; set; }
    }
    public class ReverificationDupRequest : IntellaLendRequest
    {
        public string ReverificationName { get; set; }
        public string TableSchema { get; set; }
    }
    public class RequesTemplateFields : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 MappingID { get; set; }
        public string TemplateFieldJson { get; set; }
    }

    public class RequestDocFields : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 DocumentID { get; set; }
    }

    public class RequestFieldValue : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentField { get; set; }
    }

    public class RequestDownloadReverification : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 ReverificationMappingID { get; set; }
        public Int64 ReverificationID { get; set; }
        public Int64 UserID { get; set; }
        public string TemplateFieldJson { get; set; }
        public string RequiredDocIDs { get; set; }
        public bool IsCoverLetterReq { get; set; }
        public string ReverificationName { get; set; }
    }
    public class RequestDeletedReverification : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanReverificationID { get; set; }
        public Int64 LoanID { get; set; }
        // public Int64 ReverificationMappingID { get; set; }
        //  public Int64 ReverificationID { get; set; }
        //  public Int64 UserID { get; set; }
        //  public string TemplateFieldJson { get; set; }
        //  public string RequiredDocIDs { get; set; }
        //  public bool IsCoverLetterReq { get; set; }
        //public string ReverificationName { get; set; }
        public string UserName { get; set; }
        public string ReverificationName { get; set; }


    }

    public class RequestDownloadInitReverification : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanReverificationID { get; set; }
    }


    public class RequestGetDocumentFieldMaster : IntellaLendRequest
    {
        public Int64 DocumentTypeID { get; set; }
    }

    #endregion

    #region Reporting Module

    public class ReportRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string ReportType { get; set; }
        public ReportModel ReportModel { get; set; }
    }
    public class StuplationReportRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ReviewTypeID { get; set; }
    }
    public class AssignUserRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 AssignedUserID { get; set; }
        public Int64 CurrentUserID { get; set; }
        public string ServiceTypeName { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }

    }

    #endregion

    #region Error Handler

    public class WebConsoleErrorHandlerRequest : IntellaLendRequest
    {
        public string timeStamp { get; set; }
        public string message { get; set; }
        public string stack { get; set; }
        public object Error { get; set; }
    }

    public class WebRequestErrorHandlerRequest : IntellaLendRequest
    {
        public Int32 status { get; set; }
        public string url { get; set; }
        public string timeStamp { get; set; }
    }

    #endregion

    #region RoleMaster
    public class RoleRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public RoleMaster roletype { get; set; }
        public List<MenuMaster> menus { get; set; }
    }

    public class MenuListRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
    }
    public class RoleMenuAccessList : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int32 RoleID { get; set; }
        public bool IsMappedMenuView { get; set; }

    }
    public class RoleMenuActiveList : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 RoleID { get; set; }
        public MenuMaster menus { get; set; }

    }
    #endregion


    #region Private Class

    public sealed class DefaultDateTimeValueAttribute : ValidationAttribute
    {
        private string memberName;

        public DefaultDateTimeValueAttribute(string DestinationMemberName)
        {
            memberName = DestinationMemberName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value.GetType().Name.ToLower() == Type.GetType("System.String").Name.ToLower() && !string.IsNullOrEmpty((string)value) && !string.IsNullOrEmpty(memberName))
            {
                PropertyInfo property = validationContext.ObjectType.GetProperty(memberName);
                DateTime defaultValue = DateTime.ParseExact((string)value, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                property.SetValue(validationContext.ObjectInstance, defaultValue);
            }
            return ValidationResult.Success;
        }
    }


    //[AttributeUsage(AttributeTargets.Property)]
    //public sealed class DefaultDateTimeValueAttribute : ValidationAttribute
    //{
    //    public string DefaultValue { get; set; }

    //    public DefaultDateTimeValueAttribute(string defaultValue)
    //    {
    //        DefaultValue = defaultValue;
    //    }

    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        PropertyInfo property = validationContext.ObjectType.GetProperty(validationContext.MemberName);

    //        // Set default value only if no value is already specified 
    //        if (DefaultValue == "Now")
    //        {
    //            DateTime defaultValue = GetDefaultValue();
    //            property.SetValue(validationContext.ObjectInstance, defaultValue);
    //        }

    //        return ValidationResult.Success;
    //    }

    //    private DateTime GetDefaultValue()
    //    {
    //        // Resolve a named property of DateTime, like "Now" 
    //        if (this.IsProperty)
    //        {
    //            return GetPropertyValue();
    //        }
    //        // Parse an absolute date 
    //        return GetAbsoluteValue();
    //    }

    //    private bool IsProperty
    //        => typeof(DateTime).GetProperties()
    //            .Select(p => p.Name).Contains(this.DefaultValue);

    //    private DateTime GetPropertyValue()
    //    {
    //        var instance = Activator.CreateInstance<DateTime>();
    //        var value = (DateTime)instance.GetType()
    //            .GetProperty(this.DefaultValue)
    //            .GetValue(instance);

    //        return value;
    //    }


    //    private DateTime GetAbsoluteValue()
    //    {
    //        DateTime value;
    //        if (!DateTime.TryParse(this.DefaultValue, out value))
    //        {
    //            return default(DateTime);
    //        }

    //        return value;
    //    }


    //}

    #endregion

    #region Email Tracker
    public class EmailTrackRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ID { get; set; }
        public EmailtrackerModel emailtracker { get; set; }
    }

    public class ResendEmailRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ID { get; set; }
    }
    public class SendEmailRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Attachement { get; set; }
        public string Body { get; set; }
        public Int64 UserID { get; set; }
        public string SendBy { get; set; }
        public Int64 LoanID { get; set; }
        public string AttachmentsName { get; set; }
    }
    #endregion

    #region Loan Export Monitor
    public class ExportMonitorRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 Status { get; set; }
        public Int64 Customer { get; set; }
        public Exportmodel ExportedDate { get; set; }
    }

    public class SearchJobLoanRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 JobID { get; set; }
        public Int64 LoanID { get; set; }
    }
    public class SaveJobLoanRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 JobID { get; set; }
        public Int64 CustomerID { get; set; }
        public Int64 ExportedBy { get; set; }
        public string JobName { get; set; }
        public Boolean CoverLetter { get; set; }
        public Boolean TableOfContent { get; set; }
        public Boolean PasswordProtected { get; set; }
        public string Password { get; set; }
        public string CoverLetterContent { get; set; }
        public List<BatchLoanDetail> BatchLoanDoc { get; set; }
    }

    #endregion

    #region LoanStipulation

    public class LoanStipulationRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public LoanStipulationDetails LoanStipulationDetails { get; set; }
        public string UserName { get; set; }
    }
    #endregion
    #region KpiGoalConfig
    public class AddKpiGoalConfig : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public KpiUserGroupConfig KpiUserGroupConfig { get; set; }
        public List<KPIGoalConfig> KpiGoalConfig { get; set; }
        public AuditKpiGoalConfig AuditKpiConfig { get; set; }
        public bool IsExistNewUserGrp { get; set; }
    }

    public class KpiGoalConfigStaging : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ID { get; set; }
        public Int64 GroupID { get; set; }
        public int ConfigType { get; set; }
        public Int64 Goal { get; set; }
        public int Status { get; set; }
    }
    #endregion

    #region Encompass Parking Spot
    public class AddParkingSpot
    {
        public string TableSchema { get; set; }
        public string ParkingSpotName { get; set; }
        public Boolean Active { get; set; }
        public Int64 ParkingSpotID { get; set; }
    }

    public class GetLOSFields : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string SearchCriteria { get; set; }
    }

    public class LOSType : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public string LosType { get; set; }
    }
    #endregion


    public class UpdateLoanAudit : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public DateTime AuditDueDate { get; set; }
    }

    public class DocumetnObsoleteRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 LoanID { get; set; }
        public Int64 DocumentID { get; set; }
        public Int64 DocumentVersion { get; set; }
        public bool IsObsolete { get; set; }
        public Int64 CurrentUserID { get; set; }
        public string DocName { get; set; }
    }
    public class EncompassExceptionRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 EncompassExceptionID { get; set; }
        public EncompassExceptionModel encompassException { get; set; }

    }
    public class RetryEncompassExceptionRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 EncompassExceptionID { get; set; }
    }
    public class BoxDownloadExceptionRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ID { get; set; }
        public BoxDownloadExceptionModel boxDownloadException { get; set; }

    }
    public class RetryBoxExceptionRequest : IntellaLendRequest
    {
        public string TableSchema { get; set; }
        public Int64 ID { get; set; }

    }
    //Encompass Export
    public class ELoanRequest : IntellaLendRequest
    {
        public EncompassExportmodel EncompassExportedDate { get; set; }

        public string TableSchema { get; set; }
        public Int64 Status { get; set; }
        public Int64 ID { get; set; }
        public Int64 Customer { get; set; }

        public class EDownloadRetryRequest : IntellaLendRequest
        {
            public string TableSchema { get; set; }
            public Int64 ID { get; set; }
            public Int64 LoanID { get; set; }
            public Int64 EDownloadID { get; set; }

        }
        public class ELoanRetryRequest : IntellaLendRequest
        {
            public string TableSchema { get; set; }
            public Int64 ID { get; set; }
            public Int64 LoanID { get; set; }
        }
        public class LosExportMonitorRequest : IntellaLendRequest
        {
            public string TableSchema { get; set; }
            public Int32 Status { get; set; }
            public Int64 Customer { get; set; }
            public Int64 LoanType { get; set; }
            public Int64 ServiceType { get; set; }
            public Exportmodel ExportedDate { get; set; }

        }


        public class CurrentExportLoanRequest : IntellaLendRequest
        {
            public string TableSchema { get; set; }
            public Int64 ID { get; set; }
            public Int64 LoanID { get; set; }
        }

    }
}