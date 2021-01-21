export class StatusConstant {
  static COMPLETE = 1;
  static PENDING_IDC = 2;
  static IDC_COMPLETE = 3;
  static PENDING_BOX_DOWNLOAD = 4;
  static FAILED_BOX_DOWNLOAD = 5;
  static READY_FOR_IDC = 6;
  static IDC_ERROR = 7;
  static LOAN_TYPE_NOT_FOUND = 900;
  static MOVE_TO_PROCESSING_QUEUE = 8;
  static PENDING_ENCOMPASS_DOWNLOAD = 10;
  static FAILED_ENCOMPASS_DOWNLOAD = 11;
  static PENDING_AUDIT = 91;
  static LOAN_PURGED = 100;
  static LOAN_EXPIRED = 101;
  static PURGE_WAITING = 102;
  static PURGE_FAILED = 103;
  static EXPORT_WAITING = 104;
  static EXPORT_FAILED = 105;
  static LOAN_EXPORTED = 106;
  static PENDING_OCR = 201;
  static DOCUMENT_CLASSIFICATION = 202;
  static FIELD_EXTRACTION = 203;
  static DELETE_LOAN = 999;
  static JOB_WAITING = 301;
  static PROCESSING_JOB = 302;
  static JOB_ERROR = 303;
  static JOB_EXPORTED = 304;
  static JOB_DELETED = 305;
  static MOVED_TO_IDC = 401;
  static RUNNING = 402;
  static CLASSIFICATION_WAITING = 403;
  static FIELD_EXTRACTION_WAITING = 404;
  static IDC_DELETED = 405;
  static EXTRACTION_COMPLETED = 406;
  static ERROR = 407;
  static SUBSTATUS_OCR_ERROR = 408; // ephesoft  error
  static FAILED_TO_IMPORT = 1004;
  static LOANTYPE_UNAVAILABLE = 1001;
  static LOAN_WAITING = 0;
  static LOS_LOAN_EXPORT_STAGED = 500;
  static STATUS_DESCRIPTION = {
    '1': 'Export Complete',
    '2': 'Pending IDC Processing',
    '3': 'IDC Completed',
    '4': 'Pending Box Import',
    '5': 'Box Download Failed',
    '6': 'Ready for IDC',
    '7': 'IDC Error',
    '8': 'Pending IDC Import',
    '10': 'Pending Encompass Download',
    '11': 'Failed Encompass Download',
    '91': 'Ready for Audit',
    '100': 'Loan Purged',
    '101': 'Expired',
    '102': 'Pending Purge',
    '103': 'Failed Purge',
    '105': 'Export Failed',
    '201': 'Pending IDC',
    '106': 'Loan Exported',
    '104': ' Export Waiting',
    '202': 'Document Classification',
    '203': 'Field Extraction',
    '999': 'Loan Deleted',
    '1004': 'IDC Error',
    '1002': 'Move to Processing Queue Failed',
    '1003': 'Import to IDC Failed',
    '1001': 'LoanType Unavailable',
    '900': 'LoanType Unavailable',
    '301': 'Job Export Waiting',
    '302': 'Job Export Processing',
    '303': 'Job Export Failed',
    '304': 'Job Exported',
    '305': 'Job Deleted',
    '401': 'Moved To IDC',
    '402': 'Running',
    // "403": "Classification Waiting",
    // "404": "Field Extraction Waiting",
    '403': 'Ready For IDC Review',
    '404': 'Ready For IDC Validation',
    '405': 'IDC Deleted',
    // "406": "Extraction Completed",
    '406': 'Pending Rules Evaluation',
    '407': 'Error',
    '408': 'IDC Error',
    '0': 'Loan Waiting',
    '500': 'Export Waiting',
  };
  static PRIORITY_LEVEL = {
    '0': 'Priority Unavailable',
    '1': 'Critical',
    '2': 'High',
    '3': 'Medium',
    '4': 'Low'
  };
  static IDC_STATUS_ICON = {
    '401': 'move_to_inbox',
    '402': 'directions_bike',
    '403': 'rate_review',
    '404': 'assignment',
    '405': 'delete',
    '406': 'event_available',
    '407': 'error',
  };

  static STATUS_COLOR = {
    '1': 'label-success',
    '2': 'label-info',
    '3': 'label-default',
    '4': 'label-default',
    '5': 'label-danger',
    '6': 'label-info',
    '7': 'label-danger',
    '8': 'label-info',
    '10': 'label-primary',
    '11': 'label-danger',
    '91': 'label-primary',
    '100': 'label-success',
    '102': 'label-warning',
    '103': 'label-danger',
    '101': 'label-warning',
    '201': 'label-warning',
    '202': 'label-warning',
    '203': 'label-warning',
    '106': 'label-success',
    '104': 'label-warning',
    '105': 'label-danger',
    '999': 'label-danger',
    '301': 'label-info',
    '302': 'label-primary',
    '303': 'label-warning',
    '304': 'label-success',
    '305': 'label-danger',
    '401': 'label-info',
    '402': 'label-primary',
    '403': 'label-warning',
    '404': 'label-success',
    '405': 'label-danger',
    '406': 'label-success',
    '407': 'label-danger',
    '408': 'label-danger',
    '0': 'label-info',
    '1004': 'label-danger',
    '500': 'label-info',
  };

  static STATUS_LABEL_ICON = {
    '1': 'label label-success',
    '2': 'label label-info',
    '3': 'label label-default',
    '4': 'label label-default',
    '5': 'label label-danger',
    '6': 'label label-info',
    '7': 'label label-danger',
    '8': 'label label-info',
    '10': 'label label-primary',
    '11': 'label label-danger',
    '91': 'label label-primary',
    '100': 'label label-success',
    '101': 'label label-warning',
    '201': 'label label-warning',
    '202': 'label label-warning',
    '203': 'label label-warning',
    '104': 'label label-warning',
    '106': 'label label-success',
    '105': 'label label-danger',
    '999': 'label label-danger',
    '301': 'label label-info',
    '302': 'label label-primary',
    '303': 'label label-warning',
    '304': 'label label-success',
    '305': 'label label-danger',
    '401': 'label label-info',
    '402': 'label label-primary',
    '403': 'label label-warning',
    '404': 'label label-success',
    '405': 'label label-danger',
    '406': 'label label-success',
    '407': 'label label-danger',
    '408': 'label label-danger',
    '1004': 'label label-danger',
    '0': 'label label-info',
    '500': 'label label-info',
  };
}
