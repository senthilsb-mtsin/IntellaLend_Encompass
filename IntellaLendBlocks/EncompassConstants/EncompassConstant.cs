namespace EncompassWrapperConstants
{
    public class EncompassConstant
    {
        public const string RequestToken = "TokenRequest";
        public const string ValidateToken = "TokenValidate";
        public const string InvalidTokenError = "invalid_grant";
        public const string ApplicationID = "All";
    }

    public class EncompassFieldOperator
    {
        public const string AND = "and";
    }

    public class EncompassExportStatus
    {
        public const string SUCCESS = "success";
        public const string RUNNING = "running";
        public const string FAILED = "failed";
    }

    public class EncompassFieldMatchType
    {
        public const string Exact = "Exact";
        public const string CaseInsensitive = "CaseInsensitive";
        public const string StartsWith = "StartsWith";
        public const string Contains = "Contains";
    }
}
