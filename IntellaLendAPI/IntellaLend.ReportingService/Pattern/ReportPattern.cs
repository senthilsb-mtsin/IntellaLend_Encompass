using IntellaLend.Model;
using IntellaLend.ReportData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntellaLend.ReportingService
{
    #region Dashboard Report Factory  

    #region Missing Critical Document Report

    public abstract class MissingCriticalDocument
    {
        protected string TenantSchema { get; set; }
        protected Int64 CustomerID { get; set; }
        // protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewTypeId { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }

    public class MissingCriticalReport : MissingCriticalDocument
    {
        public MissingCriticalReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.CustomerID = report.CustomerID;
            //.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewTypeId = report.ReviewTypeID;
        }

        public override object GetReport()
        {
            return new ReportDataAccess(this.TenantSchema).GetMissingCriticalDocumentReport(this.CustomerID, this.FromDate, this.ToDate, this.ReviewTypeId);
        }

        public async override Task<object> GetGraph()
        {
            return await new ReportDataAccess(this.TenantSchema).GetMissingCriticalDocumentGraph(this.FromDate, this.ToDate, this.ReviewTypeId);
        }
    }

    #endregion

    #region Data Entry WorkLoad Report

    public abstract class DataEntryWorload
    {
        protected string TenantSchema { get; set; }
        protected Int64 UserID { get; set; }
        //protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewTypeID { get; set; }
        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }

    public class DataEntryWorloadReport : DataEntryWorload
    {
        public DataEntryWorloadReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.UserID = report.UserID;
            //this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewTypeID = report.ReviewTypeID;
        }

        public override object GetReport()
        {
            return new ReportDataAccess(this.TenantSchema).GetDataEntryWorloadReport(this.UserID, this.FromDate, this.ToDate, this.ReviewTypeID);
        }

        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetDataEntryWorloadGraph(this.FromDate, this.ToDate, this.ReviewTypeID);
        }
    }

    #endregion

    #region Top Of the House Report

    public abstract class TopOftheHouse
    {
        protected string TenantSchema { get; set; }
        protected string BarType { get; set; }
        //protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 CustomerId { get; set; }
        protected Int64 ReviewTypeID { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }

    public class TopOftheHouseReport : TopOftheHouse
    {
        public TopOftheHouseReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.BarType = report.BarType;
            //this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.CustomerId = report.CustomerID;
            this.ReviewTypeID = report.ReviewTypeID;
        }

        public override object GetReport()
        {
            object _result = null;

            switch (this.BarType)
            {
                case "Loan Imported":
                    _result = new ReportDataAccess(this.TenantSchema).GetTopOftheHouseReportFromBox(this.CustomerId, this.FromDate, this.ToDate, this.BarType, this.ReviewTypeID);
                    break;
                case "Ready for IDC":
                    _result = new ReportDataAccess(this.TenantSchema).GetTopOftheHouseReportFromBox(this.CustomerId, this.FromDate, this.ToDate, this.BarType, this.ReviewTypeID);
                    break;
                case "Pending IDC":
                    _result = new ReportDataAccess(this.TenantSchema).GetTopOftheHouseReportFromBox(this.CustomerId, this.FromDate, this.ToDate, this.BarType, this.ReviewTypeID);
                    break;
                case "IDC Completed":
                    _result = new ReportDataAccess(this.TenantSchema).GetTopOftheHouseReportFromBox(this.CustomerId, this.FromDate, this.ToDate, this.BarType, this.ReviewTypeID);
                    break;
                case "Complete Audit":
                    _result = new ReportDataAccess(this.TenantSchema).GetTopOftheHouseReportFromBox(this.CustomerId, this.FromDate, this.ToDate, this.BarType, this.ReviewTypeID);
                    break;
                default:
                    break;
            }

            return _result;
        }

        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetTopOftheHouseGraph(this.FromDate, this.ToDate, this.CustomerId, this.ReviewTypeID);
        }
    }

    #endregion

    #region IDC WorkLoad Report

    public abstract class IDCWorkLoad
    {
        protected string TenantSchema { get; set; }
        protected string BarType { get; set; }
        //protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 DataEntryType { get; set; }
        protected Int64 CustomerID { get; set; }
        public abstract Task<object> GetGraph();
    }

    public class IDCWorkLoadReport : IDCWorkLoad
    {
        public IDCWorkLoadReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.BarType = report.BarType;
            //this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.DataEntryType = report.DataEntryType;
            this.CustomerID = report.CustomerID;
        }

        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetIDCWorkLoadGraph(this.FromDate, this.ToDate, this.DataEntryType, this.CustomerID);
        }
    }

    #endregion

    #region Missing Recorded Loans

    public abstract class MissingRecordedLoans
    {
        protected string TenantSchema { get; set; }
        //protected string BarType { get; set; }
        //protected DateTime AuditMontYear { get; set; }
        //protected Int64 CustomerId { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewtypeID { get; set; }
        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }

    public class MissingRecordedLoansReport : MissingRecordedLoans
    {
        public MissingRecordedLoansReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            //this.BarType = report.BarType;
            //this.AuditMontYear = report.AuditMontYear;
            //this.CustomerId = report.CustomerID;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewtypeID = report.ReviewTypeID;
        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetMissingRecordedLoansReport(this.FromDate, this.ToDate, this.ReviewtypeID);
            return _result;
        }

        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetMissingRecordedLoansGraph(this.FromDate, this.ToDate, this.ReviewtypeID);
        }
    }

    #endregion

    #region Document Retention Monitoring

    public abstract class DocumentRetentionMonitoring
    {
        protected string TenantSchema { get; set; }
        //protected Int64 CustomerID { get; set; }
        //protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewTypeID { get; set; }
        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }

    public class DocumentRetentionMonitoringReport : DocumentRetentionMonitoring
    {
        public DocumentRetentionMonitoringReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            //this.CustomerID = report.CustomerID;
            //this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewTypeID = report.ReviewTypeID;
        }

        public override object GetReport()
        {
            return new ReportDataAccess(this.TenantSchema).GetDocumentRetentionMonitoringReport(this.FromDate, this.ToDate, this.ReviewTypeID);
        }

        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetDocumentRetentionMonitoringGraph(this.FromDate, this.ToDate, this.ReviewTypeID);
        }
    }

    #endregion

    #region IDC Extraction Report

    public abstract class OCRExtraction
    {
        protected string TenantSchema { get; set; }
        protected DateTime AuditMontYear { get; set; }
        protected Int64 LoanID { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected int OcrType { get; set; }
        public abstract object GetOCRSecondLayerReport();
        public abstract Task<List<DBReportResultModel>> GetOCRFirstLayerReport();
    }

    public class OCRExtractionReport : OCRExtraction
    {
        public OCRExtractionReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.AuditMontYear = report.AuditMontYear;
            this.LoanID = report.LoanID;
            this.OcrType = report.OCRReportType;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
        }

        //public override object GetOCRFirstLayerReport()
        //{
        //    return new ReportDataAccess(this.TenantSchema).GetOCRFirstLayerReport(this.AuditMontYear);
        //}
        //public override object GetOCRSecondLayerReport()
        //{
        //    return new ReportDataAccess(this.TenantSchema).GetOCRSecondLayerReport(this.LoanID);
        //}
        public override Task<List<DBReportResultModel>> GetOCRFirstLayerReport()
        {
            return new ReportDataAccess(this.TenantSchema).GetOCRFirstLayerReport(this.FromDate, this.ToDate, this.OcrType);
        }
        public override object GetOCRSecondLayerReport()
        {
            return new ReportDataAccess(this.TenantSchema).GetOCRSecondLayerReport(this.LoanID);
        }

    }

    #endregion

    #region CheckList Failed Loan Report
    public abstract class CheckListFailedLoan
    {
        protected string TenantSchema { get; set; }
        //protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewTypeID { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }
    public class CheckListFailedLoanReport : CheckListFailedLoan
    {
        public CheckListFailedLoanReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            //this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewTypeID = report.ReviewTypeID;
        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetCheckListFailedLoansReport(this.FromDate, this.ToDate, this.ReviewTypeID);
            return _result;
        }
        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetCheckListFailedLoansGraph(this.FromDate, this.ToDate, this.ReviewTypeID);
        }

    }
    #endregion

    #region  Loan Investor Stipulations Report
    public abstract class LoanInvestorStipulation
    {
        protected string TenantSchema { get; set; }
        protected DateTime AuditMontYear { get; set; }
        protected bool IsAuditMonthSearch { get; set; }
        protected Int64 ReviewTypeID { get; set; }
        protected Int64 LoanID { get; set; }
        protected Int64 CustomerID { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 StipulationType { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }
    public class LoanInvestorStipulationReport : LoanInvestorStipulation
    {
        public LoanInvestorStipulationReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            //this.AuditMontYear = report.AuditMontYear;
            //this.IsAuditMonthSearch = report.IsAuditMonthSearch;
            this.ReviewTypeID = report.ReviewTypeID;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.CustomerID = report.CustomerID;
            this.StipulationType = report.StipulationType;

        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetLoanInvestorStipulationReport(this.CustomerID, this.ReviewTypeID, this.FromDate, this.ToDate, this.StipulationType);
            return _result;
        }
        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetLoanInvestorStipulationGraph(this.ReviewTypeID, this.FromDate, this.ToDate, this.StipulationType);
        }

    }
    #endregion

    #region KpiGoal Dashboard 
    public abstract class KpiGoalConfiguration
    {
        protected string TenantSchema { get; set; }
        protected Int64 UserID { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected bool Flag { get; set; }
        protected Int64 RoleID { get; set; }
        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }
    public class KpiGoalConfigurationReport : KpiGoalConfiguration
    {
        public KpiGoalConfigurationReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.RoleID = report.RoleID;
            this.UserID = report.UserID;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.Flag = report.Flag;
        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetKpiGoalReportDetails(this.RoleID, this.UserID, this.FromDate, this.ToDate);
            return _result;
        }
        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetKpiGoalDashboard(this.RoleID, this.UserID, this.Flag, this.FromDate, this.ToDate);
        }

    }
    public abstract class KpiUserGroupConfiguration
    {
        protected string TenantSchema { get; set; }
        protected Int64 GroupID { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }
    public class KpiUserGrouponfigurationReport : KpiUserGroupConfiguration
    {
        public KpiUserGrouponfigurationReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.GroupID = report.UserGroupID;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetKpiUserGroupReportDetails(this.GroupID, this.FromDate, this.ToDate);
            return _result;
        }
        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetKpiUserGroupDashboard();
        }

    }
    #endregion

    #region Loan Failed Rules Report
    public abstract class LoanFailedRules
    {
        protected string TenantSchema { get; set; }
        protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewTypeID { get; set; }
        protected Int64 LoanTypeID { get; set; }
        protected int RuleStatus { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }
    public class LoanFailedRulesReport : LoanFailedRules
    {
        public LoanFailedRulesReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewTypeID = report.ReviewTypeID;
            this.LoanTypeID = report.LoanTypeID;
            this.RuleStatus = report.RuleStatus;

        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetLoanFailedRulesReport(this.FromDate, this.ToDate, this.LoanTypeID, this.ReviewTypeID, this.RuleStatus);
            return _result;
        }
        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetLoanFailedRulesGraph(this.FromDate, this.ToDate, this.ReviewTypeID, this.RuleStatus);
        }

    }
    #endregion

    #region Critical Rules Failed Report
    public abstract class CriticalRulesFailed
    {
        protected string TenantSchema { get; set; }
        protected DateTime AuditMontYear { get; set; }
        protected DateTime FromDate { get; set; }
        protected DateTime ToDate { get; set; }
        protected Int64 ReviewTypeID { get; set; }
        protected Int64 LoanTypeID { get; set; }
        protected Int64 CustomerId { get; set; }
        protected string CategoryName { get; set; }

        public abstract object GetReport();
        public abstract Task<object> GetGraph();
    }
    public class CriticalRulesFailedReport : CriticalRulesFailed
    {
        public CriticalRulesFailedReport(string TableSchema, ReportModel report)
        {
            this.TenantSchema = TableSchema;
            this.AuditMontYear = report.AuditMontYear;
            this.FromDate = report.FromDate;
            this.ToDate = report.ToDate;
            this.ReviewTypeID = report.ReviewTypeID;
            this.LoanTypeID = report.LoanTypeID;
            this.CustomerId = report.CustomerID;
            this.CategoryName = report.CategoryName;
        }

        public override object GetReport()
        {
            object _result = null;
            _result = new ReportDataAccess(this.TenantSchema).GetCriticalLoansFailedReport(this.FromDate, this.ToDate, this.LoanTypeID, this.CategoryName, this.CustomerId);
            return _result;
        }
        public override Task<object> GetGraph()
        {
            return new ReportDataAccess(this.TenantSchema).GetCriticalLoansFailedGraph(this.FromDate, this.ToDate, this.ReviewTypeID, this.CustomerId);
        }

    }
    #endregion

    public abstract class DashboardReportFactory
    {
        public abstract MissingCriticalReport GetMissingCriticalDocument(string TableSchema, ReportModel report);
        public abstract TopOftheHouseReport GetTopOftheHouse(string TableSchema, ReportModel report);
        public abstract DocumentRetentionMonitoringReport GetDocumentRetentionMonitoring(string TableSchema, ReportModel report);
        public abstract OCRExtractionReport GetOCRExtraction(string TableSchema, ReportModel report);
        public abstract DataEntryWorloadReport GetDataEntryWorkload(string TableSchema, ReportModel report);
        public abstract MissingRecordedLoansReport GetMissingRecordedLoansReport(string TableSchema, ReportModel report);
        public abstract CheckListFailedLoanReport GetCheckListFailedLoansReport(string TableSchema, ReportModel report);
        public abstract LoanInvestorStipulationReport GetLoanInvestorStipulationReport(string TableSchema, ReportModel report);
        public abstract KpiGoalConfigurationReport GetKpiGoalReportDetails(string TableSchema, ReportModel report);
        public abstract KpiUserGrouponfigurationReport GetKpiUserGroupReportDetails(string TableSchema, ReportModel report);
        public abstract LoanFailedRulesReport LoanFailedRulesReport(string TableSchema, ReportModel report);
        public abstract CriticalRulesFailedReport CriticalRulesFailedReport(string TableSchema, ReportModel report);
        public abstract IDCWorkLoadReport GetIDCWorkLoadReport(string TableSchema, ReportModel report);
    }

    #endregion

    #region Abstract Factory Pattern

    public class AbstratctFactoryReport : DashboardReportFactory
    {
        public override MissingCriticalReport GetMissingCriticalDocument(string TableSchema, ReportModel report)
        {
            return new MissingCriticalReport(TableSchema, report);
        }

        public override TopOftheHouseReport GetTopOftheHouse(string TableSchema, ReportModel report)
        {
            return new TopOftheHouseReport(TableSchema, report);
        }

        public override IDCWorkLoadReport GetIDCWorkLoadReport(string TableSchema, ReportModel report)
        {
            return new IDCWorkLoadReport(TableSchema, report);
        }

        public override DocumentRetentionMonitoringReport GetDocumentRetentionMonitoring(string TableSchema, ReportModel report)
        {
            return new DocumentRetentionMonitoringReport(TableSchema, report);
        }

        public override OCRExtractionReport GetOCRExtraction(string TableSchema, ReportModel report)
        {
            return new OCRExtractionReport(TableSchema, report);
        }

        public override DataEntryWorloadReport GetDataEntryWorkload(string TableSchema, ReportModel report)
        {
            return new DataEntryWorloadReport(TableSchema, report);
        }

        public override MissingRecordedLoansReport GetMissingRecordedLoansReport(string TableSchema, ReportModel report)
        {
            return new MissingRecordedLoansReport(TableSchema, report);
        }
        public override CheckListFailedLoanReport GetCheckListFailedLoansReport(string TableSchema, ReportModel report)
        {
            return new CheckListFailedLoanReport(TableSchema, report);
        }
        public override LoanInvestorStipulationReport GetLoanInvestorStipulationReport(string TableSchema, ReportModel report)
        {
            return new LoanInvestorStipulationReport(TableSchema, report);
        }
        public override KpiGoalConfigurationReport GetKpiGoalReportDetails(string TableSchema, ReportModel report)
        {
            return new KpiGoalConfigurationReport(TableSchema, report);
        }
        public override KpiUserGrouponfigurationReport GetKpiUserGroupReportDetails(string TableSchema, ReportModel report)
        {
            return new KpiUserGrouponfigurationReport(TableSchema, report);
        }

        public override LoanFailedRulesReport LoanFailedRulesReport(string TableSchema, ReportModel report)
        {
            return new LoanFailedRulesReport(TableSchema, report);
        }
        public override CriticalRulesFailedReport CriticalRulesFailedReport(string TableSchema, ReportModel report)
        {
            return new CriticalRulesFailedReport(TableSchema, report);
        }
    }

    #endregion

}
