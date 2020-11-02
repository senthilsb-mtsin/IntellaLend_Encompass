export class LOSExportStatusConstant {
    static  LOS_LOAN_STAGED = 0;
    static  LOS_LOAN_POCESSING = 1;
    static  LOS_LOAN_PROCESSED = 2;
    static  LOS_LOAN_ERROR = -1;

    static LOS_EXPORT_STATUS_DESCRIPTION = {
     '0': 'Staged',
     '1': 'Processing',
     '2': 'Processed',
     '-1': 'Error',

   };
   static LOS_EXPORT_STATUS_COLOR = {
    '0': 'label-info',
    '1': 'label-primary',
    '2': 'label-success',
    '-1': 'label-danger',
   };

   static STATUS_LABEL_ICON = {
      '0': 'label label-info',
      '1': ' label label-primary',
      '2': 'label label-success',
      '-1': 'label label-danger',
   };
  }
  export class LOSExportStagingStatusConstant {
    static  LOS_LOAN_STAGED = 0;
    static  LOS_LOAN_POCESSING = 1;
    static  LOS_CLASSIFICATION_EXCEPTION_PROCESSED = 2;
    static  LOS_CLASSIFICATION_EXCEPTION_FAILED = -2;
    static  LOS_CLASSIFICATION_RESULTS_PROCESSED = 3;
    static  LOS_CLASSIFICATION_RESULTS_FAILED = -3;
    static  LOS_VALIDATION_EXCEPTION_PROCESSED = 4;
    static  LOS_VALIDATION_EXCEPTION_FAILED = -4;
    static  LOS_EXPORT_PROCEESED = 5;
    static  LOS_EXPORT_FAILED = -5;

   static LOS_EXPORT_STAGING_STATUS_DESCRIPTION = {
     '0': 'Staged',
     '1': 'Processing',
     '2': 'Classification Exception Processed',
     '-2': 'Classification Exception Failed',
      '3': 'Classification Results Processed',
     '-3': 'Classification Results Failed',
      '4': 'Validation Exception Processed',
     '-4': 'Validation Exception Failed',
     '5': 'Export  Processed',
     '-5': 'Export Failed',

   };
    static LOS_EXPORT__STAGING_STATUS_COLOR = {
    '0': 'label-info',
    '1': ' label-default',
    '2': 'label-primary',
    '3': 'label-primary',
    '4': 'label-primary',
    '5': 'label-success',
    '-1': 'label-danger',
    '-2': 'label-danger',
    '-3': 'label-danger',
    '-4': 'label-danger',
    '-5': 'label-danger',

   };
   }
   export class ExportLoanStatusConstant {
    static JOBLOANWAITING = 0;
    static JOBLOANPROCESS = 1;
    static JOBLOANERROR = -1;
    static EXPRT_LOAN_STATUS_DESCRIPTION = {
      '0': 'Job Loan Waiting',
      '1': 'Job Loan Exported',
      '-1': 'Job Loan Error',
    };

    static EXPORT_LOAN_STATUS_COLOR = {
      '0': 'label-info',
      '1': 'label-success',
      '-1': 'label-warning',
    };

    static STATUS_LABEL_ICON = {
      '0': 'label label-info',
      '1': 'label label-success',
      '-1': 'label label-warning',
    };
  }
