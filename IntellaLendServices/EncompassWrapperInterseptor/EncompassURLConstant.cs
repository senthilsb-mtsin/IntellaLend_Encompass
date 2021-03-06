﻿namespace EncompassWrapperInterseptor
{
    public class EncompassURLConstant
    {

        public const string GET_ASSIGNED_LOAN_ATTACHMENTS = "/api/v1/attachment/assigned?loanGuid={0}";
        public const string GET_UNASSIGNED_LOAN_ATTACHMENTS = "/api/v1/attachment/unassgined?loanGuid={0}";
        public const string GET_ALL_LOAN_ATTACHMENT = "api/v1/attachments?loanGuid={0}";
        public const string GET_DOCUMENT_ATTACHMENT = "/api/v1/document/attachment?loanGuid={0}&documentID={1}";
        public const string GET_ATTACHMENT = "/api/v1/attachment?loanGuid={0}&attachmentId={1}";
        public const string REMOVE_ATTACHMENT = "/api/v1/attachment/remove";
        public const string DOWNLOAD_ATTACHMENT = "/api/v1/attachment/download";
        public const string UPLOAD_ATTACHMENT = "/api/v1/attachment/upload/{0}/{1}";
        public const string REMOVE_DOCUMENT_ATTACHMENT = "/api/v1/document/attachment/remove";
        public const string ASSIGN_DOCUMENT_ATTACHMENT = "/api/v1/document/attachment/assign";

        public const string GET_ALL_LOAN_DOCUMENT_WITH_ATTACHMENTS = "/api/v1/document/attachments?loanGuid={0}";
        public const string GET_ALL_LOAN_DOCUMENTS = "/api/v1/documents?loanGuid={0}";
        public const string GET_LOAN_DOCUMENT = "/api/v1/document?loanGuid={0}&documentID={1}";
        public const string ADD_DOCUMENT = "/api/v1/document/create";
        public const string REMOVE_DOCUMENT = "/api/v1/document/delete";

        public const string GET_FIELD_SCHEMA = "/api/v1/field/schema";
        public const string GET_PREDEFINED_FIELDVALUES = "/api/v1/field/predefined";
        public const string UPDATE_PREDEFINED_FIELDS = "/api/v1/field/predefined/update";

        public const string GET_LOAN = "/api/v1/loan?loanGuid={0}";
        public const string QUERY_PIPELINE = "/api/v1/loan/pipeline";
        public const string GET_LOANS = "/api/v1/loans";
        public const string LOCK_LOAN = "/api/v1/loan/lock?loanGuid={0}";
        public const string UNLOCK_LOAN = "/api/v1/loan/unlock?LockID={0}&LoanGUID={1}";
        public const string UPDATE_CUSTOM_FIELD = "/api/v1/loan/customfield/update";

        public const string VALIDATE_TOKEN = "/api/v1/token/validate";
        public const string GET_TOKEN = "/api/v1/token";
        public const string GET_TOKEN_WITH_USER = "/api/v1/token/credentials";

        public const string GET_DOWNLOAD_ATTACHMENT = "/api/v1/attachment/download";
        public const string GET_UNATTACHMENTS = "/api/v1/attachment/unassgined?loanGuid={0}";
        public const string GET_ALLLOANDOCUMENTS = "/api/v1/documents?loanGuid={0}";

        public const string WEBHOOK_CREATE = "api/v1/webhook/create";
        public const string WEBHOOK_DELETE = "api/v1/webhook/delete?subscriptionId={0}";
        public const string WEBHOOK_GETALL_SUBSCRIPTION = "api/v1/webhook";

    }
}

