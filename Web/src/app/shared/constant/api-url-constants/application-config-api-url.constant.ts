export class AppConfigApiUrlConstant {
  // TENANT CONFIG URL
  static GET_ALL_CONFIG_TYPES = 'TenantConfig/GetAllTenantConfigTypes';
  static GET_CONFIG_TYPE_VALUE = 'TenantConfig/GetConfigValues';
  static ADD_CONFIG_TYPE_VALUE = 'TenantConfig/AddTenantConfigType';
  static UPDATE_CONFIG_TYPE_VALUE = 'TenantConfig/UpdateTenantConfigType';
  // LOAN SEARCH URL

  static GET_LOANSEARCH_CONFIG_DATA =
    'TenantConfig/GetLoanSearchFilterConfigValue';
  static UPDATE_LOANSEARCH_CONFIG_DATA =
    'TenantConfig/UpdateLoanSearchFilterConfig';
  static GET_STIPULATION_LIST = 'TenantConfig/GetStipulationList';
  static GET_CATEGORY_DATA_LIST = 'TenantConfig/GetallCategory';
  static UPDATE_STIPULATION_DATA = 'TenantConfig/UpdateInvestorStipulation';
  static SAVE_STIPULATION_DATA = 'TenantConfig/SaveInvestorStipulation';
  static UPDATE_CATEGORY_GROUP = 'TenantConfig/UpdateCategoryGroup';
  static SAVE_CATEGORY_GROUP = 'TenantConfig/SaveCategoryGroup';
  static GET_AUDIT_CONFIG_DATA = 'TenantConfig/GetAllAuditConfig';
  static UPDATE_AUDIT_CONFIG = 'TenantConfig/UpdateAuditConfig';
  static GET_PASSWORD_POLICY = 'IntellaLend/GetPasswordPolicy';
  static SAVE_PASSWORD_POLICY = 'IntellaLend/SavePasswordPolicy';
  // Box-settings url's
  static CHECK_USER_BOX_TOKEN = 'FileUpload/CheckUserBoxToken';
  static GET_BOX_CONFIG_VALUES = 'TenantConfig/BoxSettingsConfig';
  static GET_ALL_BOX_CONFIG = 'TenantConfig/GetAllBoxSettingsConfig';
  static GET_BOX_TOKEN = 'FileUpload/GenrateUserBoxToken';

  // report-master's url
  static GET_MASTER_REPORT_DATA = 'TenantConfig/GetMasterReport';
  static GET_REPORT_DOCS_LIST = 'TenantConfig/GetDocumentsList';
  static SAVE_REPORT_CONFIG = 'TenantConfig/SaveReportConfig';
  static DELETE_REPORT_CONFIG = 'TenantConfig/DeleteReportConfig';

  // SMTP -SETTINGS
  static SAVE_SMTP_DETAILS = 'IntellaLend/SaveAllSMTPDetails';
  static GET_SMTP_DETAILS = 'IntellaLend/GetAllSMPTDetails';

  //WebHook Subscription
  static CHECK_WEBHOOK_SUBSCRIPTION_EVENTTYPE_EXIST = 'TenantConfig/CheckWebHookSubscriptionEventTypeExist'
}
