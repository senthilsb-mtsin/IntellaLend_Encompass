using IntellaLend.Constance;
using IntellaLend.Model;
using System.Threading.Tasks;

namespace IntellaLend.ReportingService
{
    public class ReportingService
    {
        private static string TableSchema = string.Empty;

        public ReportingService(string tenantSchema)
        {
            TableSchema = tenantSchema;
        }

        public object GetDrilldownGrid(string _reportType, ReportModel _reportParams)
        {
            object _report;
            AbstratctFactoryReport _factory = new AbstratctFactoryReport();

            switch (_reportType)
            {
                case ReportTypeConstant.MISSING_CRITICAL_DOCUMENT:
                    _report = _factory.GetMissingCriticalDocument(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.TOP_OF_THE_HOUSE:
                    _report = _factory.GetTopOftheHouse(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.DOCUMENT_RETENTION_MONITORING:
                    _report = _factory.GetDocumentRetentionMonitoring(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.OCR_EXTRACTION_REPORT:
                    _report = _factory.GetOCRExtraction(TableSchema, _reportParams).GetOCRSecondLayerReport();
                    break;
                case ReportTypeConstant.DATAENTRY_WORKLOAD:
                    _report = _factory.GetDataEntryWorkload(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.MISSING_RECORD_LOANS:
                    _report = _factory.GetMissingRecordedLoansReport(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.LOANQC_INDEX:
                    _report = _factory.GetCheckListFailedLoansReport(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.LOAN_INVESTOR_STIPULATIONS:
                    _report = _factory.GetLoanInvestorStipulationReport(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.KPI_GOAL_CONFIGURATION:
                    _report = _factory.GetKpiGoalReportDetails(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.LOAN_FAILED_RULES:
                    _report = _factory.LoanFailedRulesReport(TableSchema, _reportParams).GetReport();
                    break;
                case ReportTypeConstant.CRITICAL_RULES_FAILED:
                    _report = _factory.CriticalRulesFailedReport(TableSchema, _reportParams).GetReport();
                    break;
                default:
                    _report = null;
                    break;
            }

            return _report;
        }

        public async Task<object> GetDashboardGraph(string _reportType, ReportModel _reportParams)
        {
            object _report;
            AbstratctFactoryReport _factory = new AbstratctFactoryReport();

            switch (_reportType)
            {
                case ReportTypeConstant.MISSING_CRITICAL_DOCUMENT:
                    _report = await _factory.GetMissingCriticalDocument(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.IDC_DATAENTRY_WORKLOAD:
                    _report = await _factory.GetIDCWorkLoadReport(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.TOP_OF_THE_HOUSE:
                    _report = await _factory.GetTopOftheHouse(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.DOCUMENT_RETENTION_MONITORING:
                    _report = await _factory.GetDocumentRetentionMonitoring(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.OCR_EXTRACTION_REPORT:
                    _report = await _factory.GetOCRExtraction(TableSchema, _reportParams).GetOCRFirstLayerReport();
                    break;
                case ReportTypeConstant.DATAENTRY_WORKLOAD:
                    _report = await _factory.GetDataEntryWorkload(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.MISSING_RECORD_LOANS:
                    _report = await _factory.GetMissingRecordedLoansReport(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.LOANQC_INDEX:
                    _report = await _factory.GetCheckListFailedLoansReport(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.LOAN_INVESTOR_STIPULATIONS:
                    _report = await _factory.GetLoanInvestorStipulationReport(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.KPI_GOAL_CONFIGURATION:
                    _report = await _factory.GetKpiGoalReportDetails(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.KPI_USER_GROUP_CONFIGURATION:
                    _report = await _factory.GetKpiUserGroupReportDetails(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.LOAN_FAILED_RULES:
                    _report = await _factory.LoanFailedRulesReport(TableSchema, _reportParams).GetGraph();
                    break;
                case ReportTypeConstant.CRITICAL_RULES_FAILED:
                    _report = await _factory.CriticalRulesFailedReport(TableSchema, _reportParams).GetGraph();
                    break;
                default:
                    _report = null;
                    break;
            }

            return _report;
        }
    }
}
