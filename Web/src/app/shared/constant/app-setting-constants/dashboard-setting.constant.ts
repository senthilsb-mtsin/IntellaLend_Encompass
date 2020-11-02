export class ServiceTypeConstant {
    static POST_CLOSING_AUDIT = 'Post-Closing Audit';
    static POST_CLOSING = 1;
}

export class ReportTypeConstant {
    static MISSING_CRITICAL_DOCUMENT = { Name: 'MissingCriticalDocument', Description: 'Missing Critical Document' };
    static TOP_OF_THE_HOUSE = { Name: 'TopOftheHouse', Description: 'Top of the House Loan Management' };
    static DOCUMENT_RETENTION_MONITORING = { Name: 'DocumentRetentionMonitoring', Description: 'Document Retention Monitoring' };
    static OCR_EXTRACTION_REPORT = { Name: 'OCRExtractionReport', Description: 'IDC Extraction Report' };
    static DATAENTRY_WORKLOAD = { Name: 'DataEntryWorkload', Description: 'Data Entry Workload' };
    static IDC_DATAENTRY_WORKLOAD = { Name: 'IDCDataEntryWorkload', Description: 'Data Entry Workload' };
    static MISSING_RECORDED_LOANS = { Name: 'MissingRecordedLoans', Description: 'Loans With Missing Documents' };
    static LOANQC_INDEX = { Name: 'LoanIQcIndex', Description: ' Loan Qc Index' };
    static LOAN_INVESTOR_STIPULATIONS = { Name: 'LoanInvestorstipulations', Description: 'Loan Investor Stipulations' };
    static KPI_GOAL_CONFIGURATION = { Name: 'KpiGoalConfiguration', Description: 'Kpi Goal Loans' };
    static KPI_USER_GROUP_CONFIGURATION = { Name: 'KpiUserGroupConfiguration', Description: 'Kpi User Group Configuration' };
    static LOAN_FAILED_RULES = { Name: 'LoanFailedRules', Description: ' Loan Files With Failed Rules' };
    static CRITICAL_RULES_FAILED = { Name: 'CriticalRulesFailed', Description: 'Loan Defects By Rule Category' };
}
