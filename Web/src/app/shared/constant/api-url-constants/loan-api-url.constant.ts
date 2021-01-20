export class LoanApiUrlConstant {
    static GET_LOAN_DETAILS = 'Loan/GetLoanDetails';
    static CHECK_LOAN_PDF = 'Loan/CheckLoanPDFExists';

    static UPDATE_LOAN_DETAIL = 'Loan/UpdateLoanDetails';
  static UPDATE_LOAN_HEADERS = 'Loan/UpdateLoanHeader';

    static UPDATE_LOAN_AUDIT_DUE_DATE = 'Loan/SaveLoanAuditDueDate';
    static GET_ASSIGNABLE_USER_LIST = 'User/GetUserDetails';
    static ASSIGN_LOAN = 'Loan/AssignUser';
    static SEND_EMAIL_DETAILS = 'Loan/SendEmailDetails';

    static GET_MISSING_DOC_STATUS = 'Loan/GetMissingDocStatus';
    static GET_MISSING_DOC_VERSION = 'Loan/GetMissingDocVersion';
    static GET_EPHESOFT_URL = 'FileUpload/GetEphesoftURL';
    static DOWNLOAD_DOCUMENT_PDF = 'Loan/DownloadDocumentPDF';
    static DOCUMENT_OBESOLETE = 'Loan/DocumentObsolete';

    static GET_LOAN = 'Loan/GetLoan';
    static GET_LOAN_NOTES = 'Loan/GetLoanNotesDetails';
    static GET_USER_MASTER = 'Master/GetUserMasters';
    static UPDATE_LOAN_NOTES = 'Loan/UpdateLoanNotesDetails';

    static GET_LOAN_STIPULATIONS = 'Loan/GetLoanStipulationDetails';
    static SAVE_LOAN_STIPULATION = 'Loan/SaveLoanStipulations';
    static UPDATE_LOAN_STIPULATION = 'Loan/UpdateLoanStipulations';
    static GET_STIPULATION_LIST = 'TenantConfig/GetActiveInvestorStipulationList';

    static GET_LOAN_AUDIT = 'Loan/GetLoanAudit';
    static GET_CURRENT_EMAIL_DATA = 'Loan/GetCurrentData';
    static RETRY_EMAIL = 'Loan/ResendEmail';
    static GET_EMAIL_TRACKER_DATA = 'Loan/GetEmailTrackerByLoanID';

    static GET_LOAN_BASED_REVERIFICATION = 'Loan/GetLoanBasedReverification';
    static GET_LOAN_REVERIFICATION = 'Loan/GetLoanReverification';
    static DOWNLOAD_LOAN_REVERIFICATION = 'Loan/DownloadReverification';
    static DELETE_LOAN_REVERIFICATION = 'Loan/DeletedReverification';
    static GET_LOAN_DOCUMENT_FIELD = 'Loan/GetFieldValue';
    static DOWNLOAD_REVERIFICATION = 'Loan/DownloadLoanReverification';

    static GET_LOAN_EVAL_CHECKLIST = 'Loan/GetEvaluatedChecklist';
    static UPDATE_LOAN_QUESTIONER = 'Loan/UpdateLoanQuestioner';
    static LOAN_COMPLETE = 'Loan/LoanComplete';
    static REVERT_LOAN_COMPLETE = 'Loan/RevertToReadyForAudit';

    static GET_LOAN_IMAGES = 'Loan/GetLoanBase64Images';
    static CHANGE_DOCUMENT_TYPE = 'Loan/ChangeDocumentType';
    static GET_LOAN_DOC_INFO = 'Loan/GetLoanDocInfo';
    static UPDATE_LOAN_DOCUMENT = 'Loan/UpdateLoanDocument';

    static SAVE_EXPORT_BATCH_DETAILS = 'Export/SaveLoanJob';

}
