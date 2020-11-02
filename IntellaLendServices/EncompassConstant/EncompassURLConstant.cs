namespace EncompassWrapperConstants
{
    public class EncompassURLConstant
    {
        #region Token Authentication

        public const string GET_TOKEN = "oauth2/v1/token";
        public const string TOKEN_INTROSPECTION = "oauth2/v1/token/introspection";

        #endregion

        #region Loan Controller

        public const string GET_LOAN = "encompass/v1/loans/{0}";
        public const string GET_LOANS = "encompass/v1/loanPipeline?cursorType=randomAccess&limit={0}";

        public const string LOCK_LOAN = "encompass/v1/resourceLocks?view=id";
        public const string UNLOCK_LOAN = "encompass/v1/resourceLocks/{0}?resourceType={1}&resourceId={2}";

        public const string UPDATE_CUSTOM_FIELD = "encompass/v1/loans/{0}";

        #endregion

        #region Field Controller

        public const string GET_FIELD_SCHEMA = "encompass/v1/schema/loan/contractGenerator";
        public const string GET_PREDEFINED_FIELD_VALUE = "encompass/v1/loans/{0}/fieldReader";
        public const string UPDATE_LOAN_FIELD = "encompass/v1/loans/{0}?view=id";

        //public const string LOCK_LOAN = "encompass/v1/resourceLocks?view=id";
        //public const string UNLOCK_LOAN = "encompass/v1/resourceLocks/{0}?resourceType={1}&resourceId={2}";

        //public const string UPDATE_CUSTOM_FIELD = "encompass/v1/loans/{0}";

        #endregion

        #region Attachment Controller

        public const string GET_LOAN_ATTACHMENTS = "encompass/v1/loans/{0}/attachments";
        public const string GET_LOAN_DOCUMENT_ATTACHMENTS = "encompass/v1/loans/{0}/documents/{1}/attachments";

        public const string EXPORT_JOB_REQUEST = "efolder/v1/exportjobs";
        public const string EXPORT_JOB_REQUEST_STATUS = "efolder/v1/exportjobs/{0}";

        public const string UPLOAD_ATTACHMENT_REQUEST = "encompass/v1/loans/{0}/attachments/url?view=id";
        public const string REMOVE_DOCUMENT_ATTACHMENT = "encompass/v1/loans/{0}/documents/{1}/attachments?action=remove";
        public const string ADD_DOCUMENT_ATTACHMENT = "encompass/v1/loans/{0}/documents/{1}/attachments?action=add";


        #endregion

        #region Document Controller

        public const string GET_LOAN_ALL_DOCUMENTS = "encompass/v1/loans/{0}/documents";
        public const string GET_LOAN_DOCUMENT = "encompass/v1/loans/{0}/documents/{1}";

        public const string ADD_DOCUMENT = "encompass/v1/loans/{0}/documents?view=id";

        #endregion
    }

}
