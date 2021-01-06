using System;
using System.Collections.Generic;

namespace IntellaLend.Constance
{
    public class EmailTemplateConstants
    {
        public static Int64 NewUserTemplate = 1;
        public static Int64 ResetPasswordEmail = 2;
        public static Int64 ChangePasswordEmail = 3;

    }
    public class CustomEmailTemplateConstants
    {
        public static int RuleFindings = 4;


    }


    public class EncompassURLILConstant
    {
        public const string GET_TOKEN_WITH_USER = "/api/Token/GetTokenWithUser";
        public const string GET_TOKEN = "/api/Token/GetToken";
        public const string GET_PREDEFINED_FIELDVALUES = "/api/v1/field/predefined";
    }
    // For Encompass Upload service
    public class EncompassUploadConstant
    {
        public static int UPLOAD_WAITING = 0;
        public static int UPLOAD_PROCESSING = 1;
        public static int UPLOAD_COMPLETE = 2;
        public static int UPLOAD_FAILED = -1;
        public static int UPLOAD_RETRY = 3;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { UPLOAD_WAITING, "Upload Pending" },
            { UPLOAD_PROCESSING, "Upload Processing" },
            { UPLOAD_COMPLETE, "Upload Completed" },
            { UPLOAD_FAILED, "Upload Failed" },
            { UPLOAD_RETRY, "Upload  Retry" },
        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }

    }

    public class LOSImportStatusConstant
    {
        public static int LOS_IMPORT_STAGED = 0;
        public static int LOS_IMPORT_PROCESSING = 1;
        public static int LOS_IMPORT_PROCESSED = 2;
        public static int LOS_IMPORT_FAILED = -1;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { LOS_IMPORT_STAGED, "LOS Import Staged" },
            { LOS_IMPORT_PROCESSING, "LOS Import Processing" },
            { LOS_IMPORT_PROCESSED, "Upload Completed" },
            { LOS_IMPORT_FAILED, "LOS Import Failed" }
        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }

    }
    public class EncompassUploadStagingConstant
    {
        public static int UPLOAD_STAGING_WAITING = 0;
        public static int UPLOAD_STAGING_PROCESSING = 1;
        public static int UPLOAD_STAGING_COMPLETE = 2;
        public static int UPLOAD_STAGING_FAILED = -1;
        public static int UPLOAD_STAGING_RETRY = 3;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { UPLOAD_STAGING_WAITING, "Upload Staging Pending" },
            { UPLOAD_STAGING_PROCESSING, "Upload Staging Processing" },
            { UPLOAD_STAGING_COMPLETE, "Upload Staging Completed" },
            { UPLOAD_STAGING_FAILED, "Upload Staging Failed" },

        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }


    }
    public class EncompassTypeConstant
    {
        public const string ServiceType = "ServiceType";
        public const string LoanType = "LoanType";


    }
    public class Application
    {
        public static string URL = "https://{0}.IntellaLend.com";
    }

    public class EnvironmentConstant
    {
        public static string HTTP = "http";
        public static string HTTPS = "https";
    }

    public class EphesoftRotationConstants
    {
        private static Dictionary<string, Int32> dic = null;

        static EphesoftRotationConstants()
        {
            dic = new Dictionary<string, Int32>()
            {
                { "NORTH", 0 },
                { "EAST", 90 },
                { "SOUTH", 180 },
                { "WEST", 270 }
            };
        }

        public static Dictionary<string, Int32> Direction
        {
            get
            {
                return dic;
            }
        }
    }

    public class CustomerConfiguration
    {
        public static string INCLUDE_LOANTYPE_DOCUMENTS = "Include_LoanType_Documents";
        public static string SEARCH_FILTER = "Search_Filter";

    }

    public class EncompassConstant
    {
        public const string RequestToken = "TokenRequest";
        public const string ValidateToken = "TokenValidate";
        public const string InvalidTokenError = "invalid_grant";
        public const string ApplicationID = "All";
        public const string TokenHeader = "Token";
        public const string TokenTypeHeader = "TokenType";
    }

    public class EncompassConfigConstant
    {
        public const string CLIENT_ID = "client_id";
        public const string CLIENT_SECRET = "client_secret";
        public const string GRANT_TYPE = "grant_type";
        public const string SCOPE = "scope";
        public const string INSTANCE_ID = "instance_id";
        public const string USERNAME = "username";
        public const string PASSWORD = "password";
    }

    public class ApplicationConfiguration
    {
        public static string ENCOMPASS_SERVER = "ENCOMPASS_SERVER";
        public static string ENCOMPASS_USERNAME = "ENCOMPASS_USERNAME";
        public static string ENCOMPASS_PASSWORD = "ENCOMPASS_PASSWORD";
    }

    public class EncompassFieldConstant
    {
        public const string LOAN_NUMBER = "364";
        public const string LOAN_NUMBER_LOOKUP = "Loan.LoanNumber";
        public const string INVESTOR_NAME = "VEND.X263";
        public const string BORROWER_NAME = "Pipeline.BorrowerName";
        public const string SERVICE_TYPE = "Fields.Log.MS.LastCompleted";
        public const string LOAN_TYPE = "1172";

        public const string LOAN_CLOSER = "LoanTeamMember.Name.Closer";
        public const string LOAN_OFFICER = "LoanTeamMember.Name.Loan Officer";
        public const string POST_CLOSER = "LoanTeamMember.Name.Post Closer";
        public const string UNDERWRITER = "LoanTeamMember.Name.Underwriter";

        public const string EMAIL_LOAN_CLOSER = "LoanTeamMember.Email.Closer";
        public const string EMAIL_LOAN_OFFICER = "LoanTeamMember.Email.Loan Officer";
        public const string EMAIL_POST_CLOSER = "LoanTeamMember.Email.Post Closer";
        public const string EMAIL_UNDERWRITER = "LoanTeamMember.Email.Underwriter";
    }

    public class LOSFieldType
    {
        public const string IMPORT = "Import";
        public const string LOOKUP = "Lookup";
        public const string UPDATE = "Update";
        public const string SERVICETYPE = "ServiceType";
        public const string LOANSEARCH = "LoanSearch";
        public const string BORROWER = "Borrower";
    }

    public class EncompassExceptionStatus
    {
        public const Int64 FAILED_LOAN = -1;
        public const Int64 RETRY_LOAN = 0;
        public const Int64 SUCCESSFUL_RETRY = 1;
    }
    public class BoxExceptionStatus
    {
        public const Int32 FAILED_LOAN = -1;
        public const Int32 RETRY_LOAN = 0;
        public const Int32 SUCCESSFUL_RETRY = 1;
    }

    public class LOSLookupMapping
    {
        public static List<string> GetDestinationFields
        {
            get
            {
                return new List<string> {
                    "Loans.CustomerID",
                    "Loans.LoanTypeID",
                    "LoanLOSFields.Closer",
                    "LoanLOSFields.LoanOfficer",
                    "LoanLOSFields.PostCloser",
                    "LoanLOSFields.Underwriter",
                    "LoanLOSFields.EmailCloser",
                    "LoanLOSFields.EmailLoanOfficer",
                    "LoanLOSFields.EmailPostCloser",
                    "LoanLOSFields.EmailUnderwriter"
                };
            }
        }

        public static List<string> GetDestinationSearchFields
        {
            get
            {
                return new List<string> {
                    "LoanSearch.InvestorLoanNumber",
                    "LoanSearch.PropertyAddress",
                    "LoanSearch.AuditDueDate"
                };
            }
        }

        public const string PROCESSEDFIELDID = "CX.DOWNLOADED";

        public const string CUSTOMER = "Loans.CustomerID";
        public const string LoanType = "Loans.LoanTypeID";
        public const string CLOSER = "LoanLOSFields.Closer";
        public const string LOANOFFICER = "LoanLOSFields.LoanOfficer";
        public const string POSTCLOSER = "LoanLOSFields.PostCloser";
        public const string UNDERWRITER = "LoanLOSFields.Underwriter";
        public const string EMAILCLOSER = "LoanLOSFields.EmailCloser";
        public const string EMAILLOANOFFICER = "LoanLOSFields.EmailLoanOfficer";
        public const string EMAILPOSTCLOSER = "LoanLOSFields.EmailPostCloser";
        public const string EMAILUNDERWRITER = "LoanLOSFields.EmailUnderwriter";
    }

    public class FieldMatchTypes
    {
        //
        // Summary:
        //     An exact match (including case) is required.
        public const int Exact = 0;
        //
        // Summary:
        //     An exact match (excluding case) is required.
        public const int CaseInsensitive = 1;
        //
        // Summary:
        //     The field value must start with the specified substring (case insensitive).
        public const int StartsWith = 2;
        //
        // Summary:
        //     The field value must contain the specified substring (case insensitive).
        public const int Contains = 3;
    }

    public class LOSExportStatusConstant
    {
        public static int LOS_LOAN_STAGED = 0;
        public static int LOS_LOAN_PROCESSING = 1;
        public static int LOS_LOAN_PROCESSED = 2;
        public static int LOS_LOAN_ERROR = -1;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            {LOS_LOAN_STAGED,"Staged"},
            {LOS_LOAN_PROCESSING,"Processing" },
            {LOS_LOAN_PROCESSED,"Processed" },
            {LOS_LOAN_ERROR,"Error" }
        };
        public static string GetStatusDescription(Int64 StatusID)
        {
            if (Status.ContainsKey(StatusID))
                return Status[StatusID];
            else
                return "Status Unavailable";
        }
    }

    public class LOSExportStagingStatusConstant
    {
        public static int LOS_CLASSIFICATION_EXCEPTION_PROCESSED = 2;
        public static int LOS_CLASSIFICATION_EXCEPTION_FAILED = -2;
        public static int LOS_CLASSIFICATION_RESULTS_PROCESSED = 3;
        public static int LOS_CLASSIFICATION_RESULTS_FAILED = -3;
        public static int LOS_VALIDATION_EXCEPTION_PROCESSED = 4;
        public static int LOS_VALIDATION_EXCEPTION_FAILED = -4;
        public static int LOS_EXPORT_PROCEESED = 5;
        public static int LOS_EXPORT_FAILED = -5;





        protected static readonly Dictionary<Int64, string> StatusDesc = new Dictionary<long, string>()
        {
            {LOS_CLASSIFICATION_RESULTS_PROCESSED,"Classification Results Processed"},
            {LOS_CLASSIFICATION_RESULTS_FAILED,"Classification Results Failed"},
            {LOS_CLASSIFICATION_EXCEPTION_PROCESSED,"Classification Exception Processed" },
            {LOS_CLASSIFICATION_EXCEPTION_FAILED,"Classification Exception Failed"},
            {LOS_VALIDATION_EXCEPTION_PROCESSED,"Validation Exception Processed"},
            {LOS_VALIDATION_EXCEPTION_FAILED,"Validation Exception Failed"},
            {LOS_EXPORT_PROCEESED,"Export  Processed"},
            {LOS_EXPORT_FAILED,"Export Failed"},



        };
        public static string GetStatusDescription(Int64 StatusID)
        {
            if (StatusDesc.ContainsKey(StatusID))
                return StatusDesc[StatusID];
            else
                return "Status Unavailable";
        }
    }
    public class LOSExportEventConstant
    {
        public static string LOS_CLASSIFICATION_EXCEPTION_EVENT = "Classification Review";
        public static string LOS_CLASSIFICATION_EXCEPTION_EVENT_DESC = "Documents waiting for classification review";
        public static string LOS_CLASSIFICATION_RESULTS_EVENT = "Classification Review Complete";
        public static string LOS_CLASSIFICATION_RESULTS_EVENT_DESC = "Document classification and review completed";
        public static string LOS_VALIDATION_EXCEPTION_EVENT = "Data Validation";
        public static string LOS_VALIDATION_EXCEPTION_EVENT_DESC = "Documents waiting for data validation";
        public static string LOS_EXPORT_EVENT = "IntellaLend Export";
        public static string LOS_EXPORT_EVENT_DESC = "Loan ready for review/QC Audit";
    }

    public class LOSExportFileTypeConstant
    {
        public static int LOS_CLASSIFICATION_EXCEPTION = 1;
        public static int LOS_CLASSIFICATION_RESULTS = 2;
        public static int LOS_VALIDATION_EXCEPTION = 3;
        public static int LOS_LOAN_EXPORT = 4;
        public static int LOS_LOAN_DOC_EXPORT = 5;
        public static int LOS_LOAN_JSON_EXPORT = 6;
        protected static readonly Dictionary<Int64, string> FileType = new Dictionary<long, string>()
        {
            {LOS_CLASSIFICATION_EXCEPTION,"Classification Exception"},
            {LOS_CLASSIFICATION_RESULTS,"Classification Results" },
            {LOS_VALIDATION_EXCEPTION,"Validation Exception" },
            {LOS_LOAN_EXPORT,"Loan Export" },
            {LOS_LOAN_DOC_EXPORT,"Loan Document Export" },
            {LOS_LOAN_JSON_EXPORT,"Loan Detail Export" }
        };

        protected static readonly Dictionary<Int64, Int32> FileTypeErrorStatus = new Dictionary<long, Int32>()
        {
            {LOS_CLASSIFICATION_EXCEPTION,LOSExportStagingStatusConstant.LOS_CLASSIFICATION_EXCEPTION_FAILED},
            {LOS_CLASSIFICATION_RESULTS,LOSExportStagingStatusConstant.LOS_CLASSIFICATION_RESULTS_FAILED },
            {LOS_VALIDATION_EXCEPTION,LOSExportStagingStatusConstant.LOS_VALIDATION_EXCEPTION_FAILED },
            {LOS_LOAN_EXPORT,LOSExportStagingStatusConstant.LOS_EXPORT_FAILED },

        };

        protected static readonly Dictionary<Int64, Int32> FileTypeProcessedStatus = new Dictionary<long, Int32>()
        {
            {LOS_CLASSIFICATION_EXCEPTION,LOSExportStagingStatusConstant.LOS_CLASSIFICATION_RESULTS_PROCESSED},
            {LOS_CLASSIFICATION_RESULTS,LOSExportStagingStatusConstant.LOS_CLASSIFICATION_RESULTS_PROCESSED },
            {LOS_VALIDATION_EXCEPTION,LOSExportStagingStatusConstant.LOS_VALIDATION_EXCEPTION_PROCESSED },
            {LOS_LOAN_EXPORT,LOSExportStagingStatusConstant.LOS_EXPORT_PROCEESED },

        };
        public static Int32 GetFileTypeProcessedStatus(Int32 FileTypeID)
        {
            if (FileTypeProcessedStatus.ContainsKey(FileTypeID))
                return FileTypeProcessedStatus[FileTypeID];
            else
                return 0;
        }

        public static Int32 GetFileTypeErrorStatus(Int32 FileTypeID)
        {
            if (FileTypeErrorStatus.ContainsKey(FileTypeID))
                return FileTypeErrorStatus[FileTypeID];
            else
                return 0;
        }

        public static string GetFileTypeDescription(Int64 FileTypeID)
        {
            if (FileType.ContainsKey(FileTypeID))
                return FileType[FileTypeID];
            else
                return "FileType Unavailable";
        }
    }

    public class StatusConstant
    {
        public static Int64 COMPLETE = 1;
        public static Int64 LOS_LOAN_STAGED = 500;
        public static Int64 PENDING_IDC = 2;
        public static Int64 IDC_COMPLETE = 3;
        public static Int64 PENDING_BOX_DOWNLOAD = 4;
        public static Int64 FAILED_BOX_DOWNLOAD = 5;

        public static Int64 PENDING_ENCOMPASS_DOWNLOAD = 10;
        public static Int64 FAILED_ENCOMPASS_DOWNLOAD = 11;
        public static Int64 READY_FOR_IDC = 6;
        public static Int64 IDC_ERROR = 7;
        public static Int64 MOVE_TO_PROCESSING_QUEUE = 8;
        public static Int64 PENDING_LOS_DOWNLOAD = 9;
        public static Int64 LOAN_DELETED = 999;

        public static Int32 LOAN_TYPE_NOT_FOUND = 900;
        //Pending Status
        public static Int64 PENDING_AUDIT = 91;

        //Loan Purge Status
        public static Int64 LOAN_PURGED = 100;
        public static Int64 LOAN_EXPIRED = 101;
        public static Int64 PURGE_WAITING = 102;
        public static Int64 PURGE_FAILED = 103;

        //Loan Export Status
        public static Int64 EXPORT_WAITING = 104;
        public static Int64 EXPORT_FAILED = 105;
        public static Int64 LOAN_EXPORTED = 106;

        //Ephesoft Status
        public static Int64 PENDING_OCR = 201;
        public static Int64 DOCUMENT_CLASSIFICATION = 202;
        public static Int64 FIELD_EXTRACTION = 203;
        public static Int64 OCR_COMPLETED = 204;
        public static Int64 MISSINGDOC_ERROR = 205;

        //Export Batch Loans Status
        public static int FILE_CREATION_SUCCESS = 1;
        public static int FILE_CREATION_FAILED = -1;
        public static int FILE_READY = 0;
        public static Int64 JOB_WAITING = 301;
        public static Int64 PROCESSING_JOB = 302;
        public static Int64 JOB_ERROR = 303;
        public static Int64 JOB_EXPORTED = 304;
        public static Int64 JOB_DELETED = 305;

        //Ephesoft File Removed Status
        public static Int64 DELETE_FILE_READY = 0;
        public static Int64 DELETE_FILE_PENDING = -1;
        public static Int64 DELETE_FILE_SUCCESS = -2;
        public static Int64 DELET_FILE_ERROR = -3;


        //Ephesoft Loan Processing Status
        public static int MOVED_TO_IDC = 401;
        public static int RUNNING = 402;
        public static int CLASSIFICATION_WAITING = 403;
        public static int FIELD_EXTRACTION_WAITING = 404;
        public static int IDC_DELETED = 405;
        public static int EXTRACTION_COMPLETED = 406;
        public static int IDCERROR = 407;
        public static int SUBSTATUS_OCR_ERROR = 408;

        //KPI ConfigType Constants
        public static int NOCONFIG = 0;
        public static int DAILY = 1;
        public static int WEEKLY = 2;
        public static int MONTHLY = 3;
        public static int YEARLY = 4;

        public static readonly Dictionary<Int64, string> KPIConfig = new Dictionary<long, string>()
        {
            { NOCONFIG, "NoConfig" },
            { DAILY, "Daily" },
            { WEEKLY,"Weekly" },
            { MONTHLY,"Monthly" },
            { YEARLY,"Yearly" },
        };
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            //{ COMPLETE, "Complete Audit" },
            { COMPLETE, "Complete Audit" },
            { LOS_LOAN_STAGED, "LOS Export Staged" },
            { READY_FOR_IDC,"Ready for IDC" },
            { PENDING_IDC, "Pending IDC Processing" },
            { IDC_COMPLETE, "IDC Completed" },
            {LOAN_DELETED,"Loan Deleted" },
            { PENDING_AUDIT, "Ready for Audit" },
            { PENDING_BOX_DOWNLOAD, "Pending Box Import" },
            { FAILED_BOX_DOWNLOAD, "Box Download Failed" },
            { PENDING_OCR, "Pending IDC Processing" },
            { IDC_ERROR, "IDC Error" },
            { MOVE_TO_PROCESSING_QUEUE, "Pending IDC Import" },
            { LOAN_PURGED, "Purged" },
            { LOAN_EXPIRED, "Expired" },
            { PURGE_WAITING, "Pending Purge" },
            { PURGE_FAILED, "Failed Purge" },
            { EXPORT_WAITING, "Export Waiting" },
            { EXPORT_FAILED, "Export Failed" },
            { LOAN_EXPORTED, "Loan Exported" },
            { DOCUMENT_CLASSIFICATION, "Document Classification" },
            { FIELD_EXTRACTION, "Field Extraction" },
            { DELETE_FILE_READY, "Delete Ephesoft Output Folder Ready" },
            { DELETE_FILE_PENDING, "Delete Ephesoft Output Folder Pending" },
            { DELETE_FILE_SUCCESS, "Delete Ephesoft Output Folder Success" },
            { DELET_FILE_ERROR, "Delete Ephesoft Output Folder Error" },
            { JOB_WAITING, "Job Export Waiting"},
            { PROCESSING_JOB, "Job Export Processing" },
            { JOB_ERROR, "Job Export Failed" },
            { JOB_EXPORTED, "Job Exported" },
            { JOB_DELETED, "Job Deleted" },
            { MOVED_TO_IDC, "Moved To IDC"},
            { RUNNING, "Running"},
            { CLASSIFICATION_WAITING, "Ready For IDC Classification"},
            { FIELD_EXTRACTION_WAITING, "Ready For IDC Validation"},
            { IDC_DELETED, "IDC Deleted"},
            { EXTRACTION_COMPLETED, "Pending Rules Evaluation"},
            { IDCERROR, "Error"},
            { PENDING_LOS_DOWNLOAD , "Pending LOS Download" },
            { PENDING_ENCOMPASS_DOWNLOAD, "Pending Encompass Download" },
            { FAILED_ENCOMPASS_DOWNLOAD, "Failed Encompass Download" },
            { SUBSTATUS_OCR_ERROR,"IDC Error"}
        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            if (Status.ContainsKey(StatusID))
                return Status[StatusID];
            else
                return "Status Unavailable";
        }
    }

    public class ErrorCodeConstant
    {
        //Import Failed Error Codes
        public static Int32 LOANTYPE_UNAVAILABLE = 1001;
        public static Int32 MOVE_TO_PROCESSING_QUEUE_FAILED = 1002;
        public static Int32 MOVE_TO_IDC_FAILED = 1003;
        public static Int32 FAILED_TO_IMPORT = 1004;
        public static Int32 ERROR = 407;


        protected static readonly Dictionary<Int32, string> Status = new Dictionary<Int32, string>()
        {
            {LOANTYPE_UNAVAILABLE,"LoanType Unavailable" },
            {MOVE_TO_PROCESSING_QUEUE_FAILED,"Move to Processing Queue Failed" },
            {MOVE_TO_IDC_FAILED,"Import to IDC Failed" },
            {FAILED_TO_IMPORT,"IDC Error" },
            {ERROR,"Error in IDC"}
        };
        public static string GetStatusDescription(Int32 StatusID)
        {
            if (Status.ContainsKey(StatusID))
                return Status[StatusID];
            else
                return "Error Code Unavailable";
        }

    }
    public class EphesoftConfigConstant
    {
        public static string NEW = "Moved To IDC";
        public static string RUNNING = "Running";
        public static string READY_FOR_REVIEW = "Ready For IDC Review";
        public static string READY_FOR_VALIDATION = "Ready For IDC Validation";
        public static string DELETED = "IDC Deleted";
        public static string FINISHED = "Extraction Completed";
        public static string ERROR = "IDC Error";
    }
    public class AuditConfigConstant
    {
        public static string USERNAME = "@UserName";
        public static string OLDDOCUMENTTYPE = "@OldDocumentType";
        public static string NEWDOCUMENTTYPE = "@NewDocumentType";
        public static string FIRSTNAME = "@FirstName";
        public static string LASTNAME = "@LastName";
        public static string REVERIFICATIONNAME = "@ReverificationName";
        public static string DOCUMENTTYPENAME = "@DocumentTypeName";
        public static string LOANSTATUS = "@LoanStatus";
        public static string LOANTYPENAME = "@loanTypeName";
        public static string ASSIGNEDBY = "@Assignedby";
        public static string ASSIGNEDTO = "@AssignedTo";
        public static string Role = "@Role";
        public static string LOANSTATUSDESC = "@LoanStatusDesc";

        public static Int64 LOAN_PURGED_BY_USER = 1;
        public static Int64 PURGE_WAITING_BY_USER = 2;
        public static Int64 EXPORT_WAITING_BY_USER = 3;
        public static Int64 LOAN_TYPE_ASSIGNED_BY = 4;
        public static Int64 LOAN_PICKED_BY_THE_USER = 5;
        public static Int64 REVERIFICATION_BEEN_INITIATED_BY = 6;
        public static Int64 UPDATED_LOAN_TYPE_FROM_LOS = 7;
        public static Int64 UPDATED_LOAN_TYPE_FROM_QCIQ = 7;
        public static Int64 UPDATED_EPHESOFTBATCHINSTANCEID = 8;
        public static Int64 UPLOADED_FROM_INTELLALEND = 9;
        public static Int64 UPLOADED_FROM_BOX = 10;
        public static Int64 LOAN_SEARCH_PURGED_BY_USER = 11;
        public static Int64 LOAN_DETAILS_UPDATED_BY = 12;
        public static Int64 DOCUMENT_TYPE_UPDATED = 13;
        public static Int64 DOCUMENT_TYPE_VERSION_INCREMENTED = 14;
        public static Int64 MANUAL_QUESTIONER_UPDATED = 15;
        public static Int64 DOCUMENTS_COUNT_UPDATED = 16;
        public static Int64 MISSING_DOCUMENT_UPLOADED = 17;
        public static Int64 COMPLETED_BY = 18;
        public static Int64 LOAN_MOVED_TO = 19;
        public static Int64 LOAN_STATUS_MODIFIED = 20;
        public static Int64 BOX_FILE_DOWNLOADED = 21;
        public static Int64 BOX_FILE_DOWNLOAD_FAILED = 22;
        public static Int64 EXPORTED_TO_EPHESOFT = 23;
        public static Int64 LOAN_MODIFIED = 24;
        public static Int64 LOAN_PURGED = 25;
        public static Int64 LOAN_EXPORTED = 26;
        public static Int64 SEARCH_FIELDS_INSERTED = 27;
        public static Int64 SEARCH_FIELDS_MODIFIED = 28;
        public static Int64 LOAN_DETAIL_UPDATED_BY_SYSTEM = 29;
        public static Int64 LOAN_DETAIL_INSERTED = 30;
        public static Int64 MISSING_DOCUMENT_FROM_SYSTEM = 31;
        public static Int64 LOANTYPE_SET_BY_SYSTEM = 32;
        public static Int64 CREATED_LOAN_PDF = 33;
        public static Int64 EXPORT_TO_EPHESOFT_FAILED = 34;
        public static Int64 EXPORT_TO_QUEUE_FAILED = 35;
        public static Int64 EPHESOFT_OUTPUT_FOLDER_MOVED = 36;
        public static Int64 EPHESOFT_OUTPUT_FOLDER_DELETED = 37;
        public static Int64 ERROR_DELETING_EPHESOFT_OUTPUT = 38;
        public static Int64 OCR_Accuracy_Calculated = 39;
        public static Int64 EPHESOFTOCRACCURACY_UPDATED = 40;
        public static Int64 ClassificationAccuracy_Updated = 41;
        public static Int64 Document_Level_EphesoftOCRAccuracy_Updated = 42;
        public static Int64 UPLOADED_FROM_ENCOMPASS = 43;
        public static Int64 ASSIGN_LOAN_TO_USER = 44;
        public static Int64 REVERT_TO_READY_FOR_AUDIT = 45;
        public static Int64 STIPULATION_CATEGORY = 46;
        public static Int64 Obsoleted = 47;
        public static Int64 Document_Obsoleted_Reverted = 48;
        public static Int64 STATUS_UPDATED_BY_SYSTEM = 49;
        public static Int64 REVERIFICATION_DELETED_BY = 50;
        public static Int64 UPLOADED_FROM_LOS = 51;


    }

    public class RoleConstant
    {
        public const string UNDERWRITER = "Underwriter";
        public const string POST_CLOSER = "Post-Closer";
        public static Int64 SYSTEM_ADMINISTRATOR = 1;
        public static Int64 QUALITY_CONTROL_MANAGER = 2;
        public static Int64 DATA_ENTRY = 3;
        public static Int64 QUALITY_CONTROL_AUDITOR = 4;
        public static Int64 POST_CLOSER_ROLEID = 10007;


        public static Int64 Common_Role_Urls = 0;
        protected static readonly Dictionary<Int64, string> Role = new Dictionary<long, string>()
        {
            { SYSTEM_ADMINISTRATOR, "System Administrator" },
            { QUALITY_CONTROL_MANAGER, "Quality Control Manager" },
            { DATA_ENTRY, "Data Entry" },
            { QUALITY_CONTROL_AUDITOR, "Quality Control Auditor" },


        };

        public static string GetRoleDescription(Int64 RoleId)
        {
            if (Role.ContainsKey(RoleId))
                return Role[RoleId];
            else
                return "Role Unavailable";
        }
    }

    public class DocumentLevelConstant
    {

        public static Int32 CRITICAL = 11;
        public static Int32 NON_CRITICAL = 0;

        protected static readonly Dictionary<Int32, string> DocumentLevel = new Dictionary<Int32, string>()
        {
            { CRITICAL, "Critical" },
            { NON_CRITICAL, "Non-Critical" }
        };

        protected static readonly Dictionary<Int32, string> DocumentLevelIcons = new Dictionary<int, string>()
        {
            { CRITICAL, "fa fa-times-circle fa-stack-2x fa-icon-size fa-stack-two"},
            { NON_CRITICAL, "fa fa-exclamation-circle fa-stack-2x fa-icon-size fa-stack-two"}
        };

        protected static readonly Dictionary<Int32, string> DocumentLevelIconColor = new Dictionary<int, string>()
        {
            { CRITICAL, "txt-themeRed"},
            { NON_CRITICAL, "txt-warm"}
        };

        public static string GetDocumentLevelDescription(Int32 DocLevel)
        {
            if (DocumentLevel.ContainsKey(DocLevel))
                return DocumentLevel[DocLevel];
            else
                return "Status Unavailable";
        }

        public static string GetDocumentLevelIcons(Int32 DocLevel)
        {
            if (DocumentLevelIcons.ContainsKey(DocLevel))
                return DocumentLevelIcons[DocLevel];
            else
                return string.Empty;
        }

        public static string GetDocumentLevelIconColor(Int32 DocLevel)
        {
            if (DocumentLevelIconColor.ContainsKey(DocLevel))
                return DocumentLevelIconColor[DocLevel];
            else
                return string.Empty;
        }
    }

    public class ImportStagingConstant
    {

        public static Int32 Staged = 0;
        public static Int32 MovedToDone = 1;
        public static Int32 Error = -1;

        protected static readonly Dictionary<Int32, string> ImportStageLevel = new Dictionary<Int32, string>()
        {
            { Staged, "Staged" },
            { MovedToDone, "Moved To Done" },
             { Error, "Error" }
        };


        public static string GetDocumentLevelDescription(Int32 DocLevel)
        {
            if (ImportStageLevel.ContainsKey(DocLevel))
                return ImportStageLevel[DocLevel];
            else
                return "Status Unavailable";
        }

    }

    public class ReportTypeConstant
    {
        public const string MISSING_CRITICAL_DOCUMENT = "MissingCriticalDocument";
        public const string IDC_DATAENTRY_WORKLOAD = "IDCDataEntryWorkload";
        public const string TOP_OF_THE_HOUSE = "TopOftheHouse";
        public const string DOCUMENT_RETENTION_MONITORING = "DocumentRetentionMonitoring";
        public const string OCR_EXTRACTION_REPORT = "OCRExtractionReport";
        public const string DATAENTRY_WORKLOAD = "DataEntryWorkload";
        public const string MISSING_RECORD_LOANS = "MissingRecordedLoans";
        public const string LOANQC_INDEX = "LoanIQcIndex";
        public const string LOAN_INVESTOR_STIPULATIONS = "LoanInvestorstipulations";
        public const string KPI_GOAL_CONFIGURATION = "KpiGoalConfiguration";
        public const string KPI_USER_GROUP_CONFIGURATION = "KpiUserGroupConfiguration";
        public const string LOAN_FAILED_RULES = "LoanFailedRules";
        public const string CRITICAL_RULES_FAILED = "CriticalRulesFailed";
    }

    public class BoxDownloadStatusConstant
    {
        public static int DOWNLOAD_PENDING = 0;
        public static int DOWNLOAD_SUCCESS = 1;
        public static int DOWNLOAD_FAILED = 2;

        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { DOWNLOAD_PENDING, "Dowload Pending" },
            { DOWNLOAD_SUCCESS, "Download Success" },
            { DOWNLOAD_FAILED, "IDC Completed" },
        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }
    }



    public class EncompassStatusConstant
    {
        public static int DOWNLOAD_PENDING = 0;
        public static int DOWNLOAD_SUCCESS = 1;
        public static int DOWNLOAD_FAILED = -1;
        public static int DOWNLOAD_RETRY = 2;

        public static int IMPORT_WAITING = -99;
        public static int UPLOAD_PENDING = 0;
        public static int UPLOAD_PROCESSING = 1;
        public static int UPLOAD_SUCCESS = 2;
        public static int UPLOAD_FAILED = -1;
        public static int UPLOAD_RETRY = 3;


        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { DOWNLOAD_PENDING, "Dowload Pending" },
            { DOWNLOAD_SUCCESS, "Download Success" },
            { DOWNLOAD_FAILED, "IDC Completed" },
        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }
    }
    public class DateConstance
    {
        public static string ShortDateFormart = "MM/dd/yyyy";
        public static string LongDateFormart = "MM/dd/yyyy hh:mm:ss tt";
        public static string AuditDateFormat = "yyyy/MM/dd 00:00:00.000";
    }

    public class ConfigConstant
    {
        public static string BOXCLIENTID = "BOX_CLIENTID";
        public static string BOXCLIENTSECRETID = "BOX_CLIENT_SECRETID";
        public static string BOXUSERID = "BOX_USER_NAME";
        public static string PASSWORDEXPIRYDAYS = "Password_Expiry";
        public static string APPLICATIONURL = "Application_URL";
        public static string NOOFATTEMPTSPWD = "NoofAttempt_Password";
        public static string QCIQSTARTDATEENABLED = "QCIQ_STARTDATE_ENABLED";
        public static string PDFFOOTER = "PDF_Footer";
    }

    public class ReportConstant
    {
        public static Int64 MISSING_RECORD_LOANS = 1;
    }
    public class ExportLoanStatusConstant
    {
        public static int JOB_LOAN_WAITING = 0;
        public static int JOB_LOAN_EXPORTED = 1;
        public static int JOB_LOAN_ERROR = -1;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            {JOB_LOAN_WAITING,"Job Loan Waiting"},
            {JOB_LOAN_EXPORTED,"Job Loan Exported" },
            {JOB_LOAN_ERROR,"Job Loan Error" },
        };
        public static string GetStatusDescription(Int64 StatusID)
        {
            if (Status.ContainsKey(StatusID))
                return Status[StatusID];
            else
                return "Status Unavailable";
        }
    }



    public class UploadConstant
    {
        public static Int32 ADHOC = 0;
        public static Int32 BOX = 1;
        public static Int32 ENCOMPASS = 2;
        public static Int32 UNC = 3;
        public static Int32 LOS = 4;
    }

    public class SynchronizeConstant
    {
        public static Int16 Staged = 0;
        public static Int16 Process = 1;
        public static Int16 Completed = 2;
        public static Int16 Failed = -1;
        public static Int16 DefaultVal = 99;

        public static Int16 AllSync = 0;
        public static Int16 ChecklistSync = 1;
        public static Int16 RetrySync = 3;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { Staged, "Retain & Update Waiting" },
            { Process, "Retain & UpdateProcessing" },
            { Completed, "Retain & Update Completed" },
            { Failed, "Retain & Update Failed" }
        };
        public static string GetStatusDescription(Int64 StatusID)
        {
            if (Status.ContainsKey(StatusID))
                return Status[StatusID];
            else
                return "Status Unavailable";

        }
    }

    public class StipulationConstant
    {
        public static Int32 Pending = 1;
        public static Int32 Completed = 2;
        public static Int32 Cancelled = 3;
    }

    public class EncompassLoanAttachmentDownloadConstant
    {
        public static int Loan = 1;
        public static int TrailingDocuments = 2;
        public static int RuleResult = 3;
    }

    public class EncompassDownloadStepConstant
    {
        public const string LoanAttachment = "LoanAttachment";
        public const string UpdateField = "UpdateField";
    }

    public class EncompassDownloadStepStatusConstant
    {
        public const Int64 Waiting = 1;
        public const Int64 Processing = 2;
        public const Int64 Completed = 3;
        public const Int64 Error = -1;
    }

    public class EWebHookStatusConstant
    {
        public static int EWEB_HOOK_STAGED = 0;
        public static int EWEB_HOOK_PROCESSING = 1;
        public static int EWEB_HOOK_PROCESSED = 2;
        public static int EWEB_HOOK_ERROR = -1;
        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { EWEB_HOOK_STAGED, "Staged" },
            { EWEB_HOOK_PROCESSING, "Processing" },
            { EWEB_HOOK_PROCESSED, "Completed" },
            { EWEB_HOOK_ERROR, "Failed" }
        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }

    }

    public class EWebHookEventsLogConstant
    {
        public static int DOCUMENT_LOG = 1;
        public static int MILESTONELOG = 2;

        protected static readonly Dictionary<Int64, string> Status = new Dictionary<long, string>()
        {
            { DOCUMENT_LOG, "Document Log" },
            { MILESTONELOG, "Milestone Log" },

        };

        public static string GetStatusDescription(Int64 StatusID)
        {
            return Status[StatusID];
        }
    }
}