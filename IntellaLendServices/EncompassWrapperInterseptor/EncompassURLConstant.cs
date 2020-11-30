namespace EncompassWrapperInterseptor
{
    public class EncompassURLConstant
    {

        public const string GET_ASSIGNED_LOAN_ATTACHMENTS = "/api/GetAssignedLoanAttachment?loanGuid={0}";
        public const string GET_UNASSIGNED_LOAN_ATTACHMENTS = "/api/GetUnassignedLoanAttachments?loanGuid={0}";
        public const string GET_ALL_LOAN_ATTACHMENT = "/api/GetAllLoanAttachment?loanGuid={0}";
        public const string GET_DOCUMENT_ATTACHMENT = "/api/GetDocumentAttachment?loanGuid={0}&documentID={1}";
        public const string GET_ATTACHMENT = "/api/GetAttachment?loanGuid={0}&attachmentId={1}";
        public const string REMOVE_ATTACHMENT = "/api/RemoveAttachment";
        public const string DOWNLOAD_ATTACHMENT = "/api/DownloadAttachment";
        public const string UPLOAD_ATTACHMENT = "/api/UploadAttachment/{0}/{1}";
        public const string REMOVE_DOCUMENT_ATTACHMENT = "/api/RemoveDocumentAttachment";
        public const string ASSIGN_DOCUMENT_ATTACHMENT = "/api/AssignDocumentAttachment";

        public const string GET_ALL_LOAN_DOCUMENT_WITH_ATTACHMENTS = "/api/GetAllLoanDocumentWithAttachments?loanGuid={0}";
        public const string GET_ALL_LOAN_DOCUMENTS = "/api/GetAllLoanDocuments?loanGuid={0}";
        public const string GET_LOAN_DOCUMENT = "/api/GetLoanDocument?loanGuid={0}&documentID={1}";
        public const string ADD_DOCUMENT = "/api/AddDocument";
        public const string REMOVE_DOCUMENT = "/api/RemoveDocument";

        public const string GET_FIELD_SCHEMA = "/api/GetFieldSchema";
        public const string GET_PREDEFINED_FIELDVALUES = "/api/GetPreDefinedFieldValues";
        public const string UPDATE_PREDEFINED_FIELDS = "/api/UpdatePredefinedFields";

        public const string GET_LOAN = "/api/GetLoan?loanGuid={0}";
        public const string QUERY_PIPELINE = "/api/QueryPipeLine";
        public const string GET_LOANS = "/api/GetLoans";
        public const string LOCK_LOAN = "/api/LockLoan";
        public const string UNLOCK_LOAN = "/api/UnLockLoan";
        public const string UPDATE_CUSTOM_FIELD = "/api/UpdateLoanCustomField";

        public const string VALIDATE_TOKEN = "/api/Token/ValidateToken";
        public const string GET_TOKEN = "/api/Token/GetToken";
        public const string GET_TOKEN_WITH_USER = "/api/Token/GetTokenWithUser";

    }
}

