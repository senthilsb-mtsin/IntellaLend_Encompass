export class LoanPurgeApiUrlConstant {
  static GET_ALL_PURGED_LOANS = 'Loan/GetPurgeMonitor';
  static GET_ALL_EXPIRED_LOANS = 'Loan/GetRetentionLoans';
  static RETRY_PURGE = 'Loan/RetryPurge';
  static GET_PURGE_STATUS = 'Master/GetRetentionWorkFlowStatus';
  static GET_PURGE_BATCH_DETAILS = 'Loan/GetPurgeStagingDetails';
  static PURGE_STAGING = 'Loan/PurgeStaging';
  static GET_DASH_EXPIRED_LOANS = 'Loan/GetDashRetentionLoans';
}
