namespace EncompassWrapperInterseptor
{
    public class EncompassURLConstant
    {
        public const string VALIDATE_TOKEN = "/api/EncompassToken/ValidateToken";
        public const string GET_TOKEN = "/api/EncompassToken/GetToken";
        public const string GET_TOKEN_WITH_USER = "/api/EncompassToken/GetTokenWithUser";

        public const string GET_LOAN = "/api/EncompassLoan/GetLoans";
        public const string GET_UNATTACHMENTS = "/api/EncompassAttachment/GetUnassginedLoanAttachments?loanGuid={0}";
        public const string GET_DOWNLOAD_ATTACHMENT = "/api/EncompassAttachment/DownloadAttachment?loanGuid={0}&attachmentID={1}";
        public const string UPDATE_CUSTOM_FIELD = "/api/EncompassLoan/UpdateLoanCustomField";





        //Encompass Loan Upload calls

        public const string GET_ALL_LOAN_DOCUMENTS = "/api/EncompassDocument/GetAllLoanDocuments?loanGuid={0}";
        //  public const string ADD_DOCUMENT               =  "/api/EncompassDocument/AddDocument";
        public const string GET_LOAN_DOCUMENT = "/api/EncompassDocument/GetLoanDocument?loanGuid={0}&documentID={1}";
        public const string UPLOAD_ATTACHMENT = "/api/UploadAttachment/{0}/{1}";
        public const string ASSIGN_DOCUMENT_ATTACHMENT = "/api/EncompassAttachment/AssignDocumentAttachment";

        //urls added by mani
        public const string GET_ALLLOANDOCUMENTS = "/api/EncompassDocument/GetAllLoanDocuments?loanGuid={0}";
        public const string ADD_DOCUMENT = "/api/EncompassDocument/AddDocument";
        //public const string ASSIGN_DOCUMENT_ATTACHMENTS = "/api/EncompassAttachment/AssignDocumentAttachement";
        public const string GET_PREDEFINED_FIELDVALUES = "/api/EncompassField/GetPreDefinedFieldValues";


    }
}

