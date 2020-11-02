export class LoanTypeApiUrlConstant {
  static GET_ALL_LOANTYPE_MASTER = 'Master/GetAllLoanTypeMaster';
  static SYNC_CUSTOMER_LOANTYPE = 'Mapping/SyncCustomerLoanType';
  static GET_SYNC_DETAILS = 'Mapping/GetSyncLoanDetails';
  static ADD_LOANTYPE_SUBMIT = 'Master/AddLoanType';
  static EDIT_LOANTYPE_SUBMIT = 'Master/UpdateLoanType';
  static GET_SYSTEM_DOCUMENT_TYPES = 'IntellaLend/GetSystemDocuments';
  static SET_LOAN_DOC_MAPPING = 'IntellaLend/SetLoanDocTypeMapping';
  static GET_CHECKLISTDETAIL_MASTER = 'CheckListItem/GetAllCheckListItems';
  static GET_LOANTYPE_CHECKLIST = 'IntellaLend/GetCheckList';

  static SET_TENANT_DOC_FIELD_VALUE = 'StackingOrder/SetTenantDocGroupFieldValue';
  static CLONE_CHECKLIST = 'CheckListItem/AssignChecklist';
  static ADD_CHECKLIST_GROUP = 'CheckListItem/AddSysCheckList';
  static SET_LOANTYPE_CHECKLIST_MAPPING = 'IntellaLend/SetLoanCheckMapping';

  static GET_STACKING_CREATE_DOCS = 'StackingOrder/GetLoanStackDocs';
  static GET_STACKING_DOCUMENT_TYPES = 'StackingOrder/GetStackSystemDocumentTypes';
  static GET_STACKING_ORDER_DETAILS = 'StackingOrder/GetSystemStackingOrderDetailMaster';
  static ASSIGN_STACKING_ORDER = 'CheckListItem/AssignStackingOrder';
  static SET_DOC_FIELD = 'StackingOrder/SetDocFieldValue';
  static SET_ORDERBY_DOC_FIELD = 'StackingOrder/SetDocFieldValue';
  static SET_STACKING_ORDER = 'StackingOrder/SaveSystemStackingOrderDetails';
  static SET_STACKING_ORDER_MAPPING = 'IntellaLend/SetLoanStackMapping';
  static GET_LOANTYPE_STACKINGORDER = 'IntellaLend/GetStackingOrder';
  static SAVE_CONDITION_GENERALRULE = 'IntellaLend/SaveConditionGeneralRule';
  static CLONE_CHECKLIST_ITEM = 'CheckListItem/CloneSysCheckListItem';
  static DELETE_CHECKLIST_ITEM = 'CheckListItem/DeleteSysCheckListItem';
  static GET_SYSTEM_LOANTYPE_DOCUMENTS = 'StackingOrder/GetSystemDocumentTypes';
  static GET_CUST_LOAN_DOC_MAPPING = 'Mapping/CustLoanDocTypeMapping';
  static GET_CUST_LOAN_DOC_CONDITION_MAPPING = 'Mapping/GetCustLoanDocTypeMapping';

  static ADD_CHECKLIST_ITEM = 'CheckListItem/SaveSysCheckListDetails';
  static UPDATE_CHECKLIST_ITEM = 'CheckListItem/UpdateSysCheckListDetails';
  static EVALUATE_CHECKLIST_FORMULA = 'CheckListItem/TestCheckListItemValues';

  static ADD_CUSTOMER_CHECKLIST_ITEM = 'CheckListItem/CheckListDetails';
  static UPDATE_CUSTOMER_CHECKLIST_ITEM = 'CheckListItem/UpdateCheckListDetails';
}
