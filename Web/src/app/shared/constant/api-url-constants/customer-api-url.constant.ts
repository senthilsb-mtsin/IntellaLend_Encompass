export class CustomerApiUrlConstant {
    static GET_CUSTOMER_MASTER = 'Customer/GetCustomerList';
    static ADD_CUSTOMER = 'Customer/AddCustomer';
    static EDIT_CUSTOMER = 'Customer/EditCustomer';

    static GET_MAPPING_SERVICE_TYPE = 'Mapping/GetCustReviewTypes';
    static CHECK_CUST_REVIEW_MAPPING = 'Mapping/CheckCustReviewMapping';
    static SAVE_CUST_REVIEW_MAPPING = 'Mapping/SaveCustReviewMapping';
    static REMOVE_CUST_REVIEW_MAPPING = 'Mapping/RemoveCustReviewMapping';
    static RETAIN_CUST_REVIEW_MAPPING = 'Mapping/RetainCustReviewMapping';

    static GET_MAPPING_LOAN_TYPE = 'Mapping/GetCustReviewLoanTypes';
    static CHECK_CUST_REVIEW_LOAN_MAPPING = 'Mapping/CheckCustReviewLoanMapping';
    static SAVE_CUST_REVIEW_LOAN_MAPPING = 'Mapping/SaveCustReviewLoanMapping';
    static SAVE_CUST_LOAN_UPLOAD_PATH = 'Mapping/SaveCustLoanUploadPath';
    static CHECK_CUST_LOAN_UPLOAD_PATH = 'Mapping/CheckCustLoanUploadPath';
    static RETAIN_CUST_REVIEW_LOAN_MAPPING = 'Mapping/RetainCustReviewLoanMapping';
    static REMOVE_CUST_REVIEW_LOAN_MAPPING = 'Mapping/RemoveCustReviewLoanMapping';
    static REMOVE_CUST_LOAN_UPLOAD_PATH = 'Mapping/RemoveCustConfigUploadPath';

    static GET_CUST_REVIEW_LOAN_CHECKLIST = 'Mapping/GetCustReviewLoanCheckList';
    static GET_CHECKLIST_DETAIL = 'CheckListItem/SearchCheckListItem';
    static DELETE_CHECKLIST_ITEM = 'CheckListItem/DeleteCheckListItem';
    static CLONE_CHECKLIST_ITEM = 'CheckListItem/CloneCheckListItem';

    static GET_STACKING_ORDER_DETAIL = 'StackingOrder/SearchStackingOrder';
    static SET_TENANT_ORDER_BY_FIELD = 'StackingOrder/SetTenantOrderByField';
    static SET_TENANT_DOC_FIELD_VALUE = 'StackingOrder/SetTenantDocFielValue';
    static SET_DOC_GROUP_FIELD_VALUE = 'StackingOrder/SetDocGroupFieldValue';
    static SAVE_CUSTOMER_STACKING_ORDER_DETAILS = 'StackingOrder/SaveStackingOrderDetails';

    static GET_ALL_CUSTOMER_CONFIG = 'CustConfig/GetAllCustConfig';
    static ADD_CUSTOMER_CONFIG = 'CustConfig/AddMultipleCustConfig';
}
