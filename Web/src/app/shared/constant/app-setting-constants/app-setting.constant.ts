export class AppSettings {
  static TenantSchema = 'T1';
  static SystemSchema = 'IL';
  static AuthorityLabelSingular = 'Lender';
  static AuthorityLabelPlural = 'Lenders';
  static dateFormat = 'MM/dd/yyyy';
  static FannieMaeDocName = 'Fannie Mae - 3-2';
  static RuleFannieMaeDocName = 'Fannie Mae - 3.2';
  static FannieMaeDocDisplayName = 'Fannie Mae - 3-2';
  static SessionErrorMsg = false;
  static TenantConfigType = [
    {
      ConfigKey: 'Delete_Loan_Source',
      ConfigValue: 'Delete Loan Source',
      ConfigType: 'checkbox',
    },
    {
      ConfigKey: 'Ephesoft_URL',
      ConfigValue: 'Ephesoft URL',
      ConfigType: 'text',
    },
    {
      ConfigKey: 'SMTP_Setting',
      ConfigValue: 'SMTP Settings',
      ConfigType: 'smtp',
    },
    {
      ConfigKey: 'BOX_Setting',
      ConfigValue: 'Box.com Configuration',
      ConfigType: 'Box',
    },
    {
      ConfigKey: 'Application_URL',
      ConfigValue: 'Application URL',
      ConfigType: 'text',
    },
    {
      ConfigKey: 'Password_Expiry',
      ConfigValue: 'Temp Password Expiry Day(s)',
      ConfigType: 'text',
    },
    {
      ConfigKey: 'NoofAttempt_Password',
      ConfigValue: 'Password Retry Count',
      ConfigType: 'text',
    },
    {
      ConfigKey: 'Audit_Config',
      ConfigValue: 'Audit Description Configuration',
      ConfigType: 'Table',
    },
    {
      ConfigKey: 'Include_LoanType_Documents',
      ConfigValue: 'Include LoanType Documents',
      ConfigType: 'checkbox',
    },
    {
      ConfigKey: 'Category_List',
      ConfigValue: 'Category List',
      ConfigType: 'List',
    },
    {
      ConfigKey: 'Missing_Loans',
      ConfigValue: 'Missing Document Type',
      ConfigType: 'Report',
    },
    {
      ConfigKey: 'Stipulation_Category',
      ConfigValue: 'Stipulation Category',
      ConfigType: 'Stipulation',
    },

    {
      ConfigKey: 'Search_Filter',
      ConfigValue: 'Loan Search Fields',
      ConfigType: 'LoanSearchFilter',
    },
    {
      ConfigKey: 'PDF_Footer',
      ConfigValue: 'PDF Footer',
      ConfigType: 'text',
    },
    {
      ConfigKey: 'Password_Policy',
      ConfigValue: 'Password Policy',
      ConfigType: 'password',
    },
    {
      ConfigKey: 'WebHook_Subscription',
      ConfigValue: 'WebHook Subscription',
      ConfigType: 'WebHook',
    },
    {
      ConfigKey: 'AD_Configuration',
      ConfigValue: 'AD Configuration',
      ConfigType: 'AD',
    }
  ];
  static customerAuthorityLevel = 91;
  static DocumentCriticalStatus = {
    '0': { Color: 'label-success', Label: 'Non-Critical', Value: 0 },
    '11': { Color: 'label-danger', Label: 'Critical', Value: 11 },
  };
}
export class BoxConfigConstant {
  static BOXCLIENTID = 'BOX_CLIENTID';
  static BOXCLIENTSECRETID = 'BOX_CLIENT_SECRETID';
  static BOXREDIRECTURL = 'BOX_REDIRECT_URL';
  static BOXUSERID = 'BOX_USER_NAME';
}
export class EphesoftStatusConstant {
  static READY_FOR_REVIEW = 'READY_FOR_REVIEW';
  static READY_FOR_VALIDATION = 'READY_FOR_VALIDATION';
  static FINISHED = 'FINISHED';
  static ERROR = 'ERROR';
  static DELETED = 'DELETED';
}
export class EncompassExportStatusConstant {
  static UPLOAD_PENDING = 0;
  static UPLOAD_PROCESSING = 1;
  static UPLOAD_SUCCESS = 2;
  static UPLOAD_FAILED = -1;
  static UPLOAD_RETRY = 3;

  static ENCOMPASS_EXPORT_STATUS_DESCRIPTION = {
    '0': 'Export Waiting',
    '1': 'Export Processing',
    '2': 'Export Success',
    '-1': 'Export Error',
    '3': 'Export Retry',
  };

  static ENCOMPASS_EXPORT_LOAN_STATUS_COLOR = {
    '0': 'label-info',
    '1': 'label-primary',
    '2': 'label-success',
    '-1': 'label-danger',
    '3': 'label-default',
  };

  static STATUS_LABEL_ICON = {
    '0': 'label label-info',
    '1': ' label label-primary',
    '2': 'label label-success',
    '-1': 'label label-danger',
    '3': 'label label-default',
  };
}
export class EncompassUploadStagingConstant {
  static UPLOAD_STAGING_WAITING = 0;
  static UPLOAD_STAGING_PROCESSING = 1;
  static UPLOAD_STAGING_COMPLETE = 2;
  static UPLOAD_STAGING_FAILED = -1;

  static ENCOMPASS_UPLOAD_STAGING_STATUS_DESCRIPTION = {
    '0': 'Export  waiting',
    '1': 'Export  Processing',
    '2': 'Export  Success',
    '-1': 'Export  Error',
  };

  static ENCOMPASS_UPLOAD_STAGING_STATUS_COLOR = {
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
}// tslint:disable-next-line: max-classes-per-file
export class WebHookSubscriptionEventTypesConstants {
  static MilestoneLog = 1;
  static DocumentLog = 2;

  static EventTypesDescription = {
    '1': 'Milestone Log',
    '2': 'Document Log'
  };

  static EventTypesDropdown = [
    WebHookSubscriptionEventTypesConstants.MilestoneLog,
    WebHookSubscriptionEventTypesConstants.DocumentLog
  ];
}
export class ADConfigurationConstant {
  static ADDOMAIN = 'AD_Domain';
  static LDAPURL = 'LDAP_url';
}

export class CustomerImportAssignTypeConstant {
  static LENDER_IMPORT = 0;
  static SERVICE_LENDER_IMPORT = 1;
}
