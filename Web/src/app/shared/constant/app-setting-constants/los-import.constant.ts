export class LOSImportStatusConstant {
    static  LOS_IMPORT_STAGED = 0;
    static  LOS_IMPORT_PROCESSING = 1;
    static  LOS_IMPORT_PROCESSED = 2;
    static  LOS_IMPORT_FAILED = -1;

    static LOS_IMPORT_STATUS_DESCRIPTION = {
      '0': 'LOS Import Staged',
      '1': 'LOS Import Processing',
      '2': 'Upload Completed',
      '-1': 'LOS Import Failed',

    };
    static LOS_IMPORT_STATUS_COLOR = {
     '0': 'label-info',
     '1': 'label-primary',
     '2': 'label-success',
     '-1': 'label-danger',
    };

    static LOS_STATUS_LABEL_ICON = {
     '0': 'label label-info',
       '1': ' label label-primary',
       '2': 'label label-success',
       '-1': 'label label-danger',
    };
 }
