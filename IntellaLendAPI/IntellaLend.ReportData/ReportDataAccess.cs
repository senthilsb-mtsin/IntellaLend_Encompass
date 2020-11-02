using IntellaLend.Constance;
using IntellaLend.Model;
using MTSEntBlocks.DataBlock;
using MTSEntBlocks.ExceptionBlock.Handlers;
using MTSEntBlocks.LoggerBlock;
using MTSEntityDataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IntellaLend.ReportData
{
    public class ReportDataAccess
    {
        private static string TableSchema = string.Empty;

        public ReportDataAccess(string tenantSchema)
        {
            TableSchema = tenantSchema;
        }

        #region Grid Report

        //Result should be in type List<ReportResultModel>
        public object GetTopOftheHouseReportFromBox(Int64 CustomerId, DateTime FromDate, DateTime ToDate, string type, Int64 ReviewTypeID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            List<Loan> _boxLoanLs = new List<Loan>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {

                if (CustomerId == 0)
                {
                    //_boxLoanLs = db.Loan.AsNoTracking().Where(l => l.AuditMonthYear == AuditMonthYear && l.FromBox).ToList();
                    _boxLoanLs = db.Loan.AsNoTracking().Where(l => l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.ReviewTypeID == ReviewTypeID).ToList();
                }
                else
                {
                    //_boxLoanLs = db.Loan.AsNoTracking().Where(l => l.AuditMonthYear == AuditMonthYear && l.FromBox && l.CustomerID == CustomerId).ToList();
                    _boxLoanLs = db.Loan.AsNoTracking().Where(l => l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.CustomerID == CustomerId && l.ReviewTypeID == ReviewTypeID).ToList();
                }

                //List<Loan> _boxLoanLs = db.Loan.AsNoTracking().Where(l => l.AuditMonthYear == AuditMontYear && l.FromBox).ToList();
                //List<Loan> _boxLoanLs = db.Loan.AsNoTracking().Where(l => l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.ReviewTypeID == ReviewTypeID && l.CustomerID == CustomerId).ToList();

                switch (type)
                {
                    case "Loan Imported":
                        _report = GetReportModel(db, _boxLoanLs);
                        break;
                    case "Ready for IDC":
                        {
                            List<Loan> _readyForIDC = _boxLoanLs.Where(b => (b.Status == StatusConstant.READY_FOR_IDC || b.Status == StatusConstant.PENDING_BOX_DOWNLOAD)).ToList();
                            _report = GetReportModel(db, _readyForIDC);
                            break;
                        }
                    case "Pending IDC":
                        {
                            List<Loan> _pendingIDC = _boxLoanLs.Where(i => i.Status == StatusConstant.PENDING_IDC).ToList();
                            _report = GetReportModel(db, _pendingIDC);
                            break;
                        }
                    case "IDC Completed":
                        {
                            List<Loan> _IDCCompleted = _boxLoanLs.Where(i => (i.Status == StatusConstant.PENDING_AUDIT && i.SubStatus == StatusConstant.EXTRACTION_COMPLETED)).ToList();
                            _report = GetReportModel(db, _IDCCompleted);
                            break;
                        }
                    case "Complete Audit":
                        {
                            List<Loan> _Auditcomplete = _boxLoanLs.Where(i => (i.Status == StatusConstant.COMPLETE)).ToList();
                            _report = GetReportModel(db, _Auditcomplete);
                            break;
                        }
                    default:
                        break;
                }
            }

            return _report;
        }

        public object GetMissingRecordedLoansReport(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                List<Int64> _distLoanIDs = db.LoanReporting.AsNoTracking().Where(x => x.AddToReport == true).Select(x => x.LoanID).Distinct().ToList();

                List<Loan> _loans = (from l in db.Loan.AsNoTracking()
                                     join lr in _distLoanIDs on l.LoanID equals lr
                                     select l).Where(a => a.CreatedOn > FromDate && a.CreatedOn < ToDate && a.ReviewTypeID == ReviewTypeID
                                     && (a.Status != StatusConstant.LOAN_DELETED && a.Status != StatusConstant.LOAN_EXPIRED && a.Status != StatusConstant.LOAN_PURGED)).ToList<Loan>();
                _report = GetReportModel(db, _loans);
            }
            return _report;
        }


        //Result should be in type List<ReportResultModel>
        public object GetMissingCriticalDocumentReport(Int64 CustomerID, DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);

            using (var db = new DBConnect(TableSchema))
            {

                List<Loan> _loans = (from l in db.Loan.AsNoTracking()
                                     join ld in db.LoanDetail.AsNoTracking() on l.LoanID equals ld.LoanID
                                     where l.ReviewTypeID == ReviewTypeID && l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.CustomerID == CustomerID && ld.MissingCriticalDocCount > 0
                                     && (l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED)
                                     select l).ToList<Loan>();

                _report = GetReportModel(db, _loans);
            }

            return _report;
        }

        //Result should be in type List<ReportResultModel>
        public object GetDocumentRetentionMonitoringReport(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);

            using (var db = new DBConnect(TableSchema))
            {
                List<Loan> _loans = db.Loan.AsNoTracking().Where(l => l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.Status == StatusConstant.LOAN_EXPIRED && l.ReviewTypeID == ReviewTypeID).ToList();

                _report = GetReportModel(db, _loans);
            }
            return _report;
        }

        /* public List<ReportResultModel> GetOCRFirstLayerReport(DateTime AuditMontYear)
         {
             List<ReportResultModel> _report = new List<ReportResultModel>();

             using (var db = new DBConnect(TableSchema))
             {
                 List<Loan> _loans = (from l in db.Loan.AsNoTracking()
                                      join ld in db.LoanDetail.AsNoTracking() on l.LoanID equals ld.LoanID
                                      where l.AuditMonthYear == AuditMontYear
                                      select l).ToList();

                 // Prakash - Commentted from Demo
                 // List< Loan > _loans = db.Loan.AsNoTracking().Where(l => l.AuditMonthYear == AuditMontYear).ToList();

                 _report = GetReportModel(db, _loans);
             }

             return _report;
         }*/

        public async Task<List<DBReportResultModel>> GetOCRFirstLayerReport(DateTime fromDate, DateTime toDate, int OCRType)
        {
            Logger.WriteTraceLog($"GetOCRFirstLayerReport(): fromDate: {fromDate}, toDate : {toDate}, OCRType : {OCRType}");
            fromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 0, 0, 0).AddDays(1);
            Logger.WriteTraceLog($"Formatted fromDate: {fromDate}, toDate : {toDate}, OCRType : {OCRType}");
            List<DBReportResultModel> _report = new List<DBReportResultModel>();
            using (var db = new DBConnect(TableSchema))
            {
                await Task.Run(() =>
                {
                    SqlParameter _fromDate = new SqlParameter("@FROMDATE", fromDate);
                    SqlParameter _toDate = new SqlParameter("@TODATE", toDate);
                    SqlParameter _OCRType = new SqlParameter("@OCRTYPE", OCRType);
                    _report = db.ReportResultModel.SqlQuery($"[{TableSchema}].DASHBOARD_IDC_REPORT @FROMDATE,@TODATE,@OCRTYPE", _fromDate, _toDate, _OCRType).ToList();

                    foreach (var idcfields in _report)
                    {
                        if (idcfields != null && !string.IsNullOrEmpty(idcfields.EphesoftReviewerName) && !string.IsNullOrEmpty(idcfields.IDCReviewDuration))
                        {
                            List<string> nameLs = idcfields.EphesoftReviewerName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                            var _idcRD = idcfields.IDCReviewDuration.Split('|').Where(x => Convert.ToString(x) != "").Select(x => x.Trim()).ToList();
                            List<DateTime> durationLs = _idcRD.Where(x => x != string.Empty).Select(a => DateTime.ParseExact(a, "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
                            if (durationLs.Count > 0)
                            {
                                idcfields.MaxIDCReviewDuration = durationLs.Max().ToString("HH:mm:ss");
                                int maxTimeIndex = durationLs.IndexOf(durationLs.Max());

                                if (maxTimeIndex >= nameLs.Count)
                                    maxTimeIndex = nameLs.Count - 1;

                                idcfields.MaxEphesoftReviewerName = nameLs[maxTimeIndex].Trim();
                            }
                        }

                        if (idcfields != null && !string.IsNullOrEmpty(idcfields.EphesoftValidatorName) && !string.IsNullOrEmpty(idcfields.IDCValidationDuration))
                        {
                            List<string> nameLs = idcfields.EphesoftValidatorName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                            var _idcVDT = idcfields.IDCValidationDuration.Split('|').Where(x => Convert.ToString(x) != "").Select(x => x.Trim()).ToList();
                            List<DateTime> durationLs = _idcVDT.Where(x => x != "").Select(a => DateTime.ParseExact(a, "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();

                            if (durationLs != null && durationLs.Count > 0)
                            {
                                idcfields.MaxIDCValidationDuration = durationLs.Max().ToString("HH:mm:ss");
                                int maxTimeIndex = durationLs.IndexOf(durationLs.Max());

                                if (maxTimeIndex >= nameLs.Count)
                                    maxTimeIndex = nameLs.Count - 1;

                                idcfields.MaxEphesoftValidatorName = nameLs[maxTimeIndex].Trim();
                            }
                        }
                    }
                    Logger.WriteTraceLog($"_report.Count : {_report.Count}");
                    Logger.WriteTraceLog($"After generating report data");
                });
            }

            return _report;
        }
        public object GetOCRSecondLayerReport(Int64 LoanID)
        {
            List<object> _report = new List<object>();
            using (var db = new DBConnect(TableSchema))
            {
                LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

                if (_loanDetail != null)
                {
                    Batch _batch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);

                    var batchDocs = from docs in _batch.Documents
                                    group docs by docs.DocumentTypeID into docGroup
                                    select docGroup.OrderByDescending(p => p.VersionNumber).First();

                    _report = (from b in batchDocs
                               select new
                               {
                                   DocName = b.Type,
                                   Confidence = b.DocumentExtractionAccuracy
                               }).AsEnumerable().Select(d => new
                               {
                                   DocName = d.DocName,
                                   Percentage = string.IsNullOrEmpty(d.Confidence) ? "NA" : d.Confidence
                               }).ToList<object>();
                }
            }

            return _report;
        }
        //public object GetOCRSecondLayerReport(Int64 LoanID)
        //{
        //    List<object> _report = new List<object>();
        //    List<object> _report1 = new List<object>();
        //    //string connectIonString= ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        //    //DynamicDataAccess.ExecuteSQLDataSet(connectIonString, sqlScript); ;
        //    //string sql = "select * from MTS_AUTO_VALIDATION_SKIP";
        //    //using(var db2 = new DataAccess2("EphesoftUtilityConnectionName").ExecuteDataTable(sql)) { }
        //    //System.Data.DataTable dtable = new DataAccess2("EphesoftUtilityConnectionName").ExecuteDataTable(sql);
        //    System.Data.DataTable dtable = new DataAccess2("EphesoftUtilityConnectionName").GetDataTable("GET_AUTO_VALIDATION_SKIP_DOCS");
        //    using (var db = new DBConnect(TableSchema))
        //    {
        //        LoanDetail _loanDetail = db.LoanDetail.AsNoTracking().Where(l => l.LoanID == LoanID).FirstOrDefault();

        //        if (_loanDetail != null)
        //        {
        //            Batch _batch = JsonConvert.DeserializeObject<Batch>(_loanDetail.LoanObject);

        //            var batchDocs = from docs in _batch.Documents
        //                            group docs by docs.DocumentTypeID into docGroup
        //                            select docGroup.OrderByDescending(p => p.VersionNumber).First();
        //            _report = (
        //                from b in batchDocs
        //                join System.Data.DataRow d in dtable.Rows on b.Type equals (string)d["DOCUMENT_NAME"]
        //                select new
        //                {
        //                    DocName = b.Type,
        //                    Confidence = b.DocumentExtractionAccuracy
        //                }).AsEnumerable().Select(d => new
        //                {
        //                    DocName = d.DocName,
        //                    Percentage = string.IsNullOrEmpty(d.Confidence) ? "NA" : d.Confidence
        //                }).ToList<object>();
        //        }
        //    }
        //    return _report;
        //}
        //Result should be in type List<ReportResultModel>
        public List<ReportResultModel> GetDataEntryWorloadReport(Int64 UserID, DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();

            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);


            using (var db = new DBConnect(TableSchema))
            {
                List<Loan> _loans = (from l in db.Loan.AsNoTracking()
                                     join u in db.Users.AsNoTracking() on l.LastAccessedUserID equals u.UserID
                                     join r in db.UserRoleMapping.AsNoTracking() on l.LastAccessedUserID equals r.UserID
                                     where l.ReviewTypeID == ReviewTypeID && l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.LastAccessedUserID == UserID && l.Status == StatusConstant.COMPLETE && r.RoleID == RoleConstant.DATA_ENTRY
                                     select l).ToList();

                _report = GetReportModel(db, _loans);
            }
            return _report;
        }

        public object GetCheckListFailedLoansReport(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                List<Loan> loans = (from lca in db.LoanChecklistAudit.AsNoTracking()
                                    join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                                    join c in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals c.CheckListDetailID
                                    where l.ReviewTypeID == ReviewTypeID && l.CreatedOn > FromDate && l.CreatedOn < ToDate
                                    && lca.Result == false && c.Rule_Type == 0 && l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED
                                    select l
                           ).Distinct().ToList();
                _report = GetReportModel(db, loans);
            }
            return _report;
        }
        public object GetLoanInvestorStipulationReport(Int64 CustomerID, Int64 ReviewTypeID, DateTime FromDate, DateTime ToDate, Int64 StipulationType)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            object data = null;

            using (var db = new DBConnect(TableSchema))
            {
                //if (IsAuditMonthSearch == true)
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
                //{

                List<Loan> loans = (from ls in db.LoanStipulation.AsNoTracking()
                                    join l in db.Loan.AsNoTracking() on ls.LoanID equals l.LoanID
                                    where
                                     ls.CustomerID == CustomerID && ls.ReviewTypeID == ReviewTypeID
                                     && ((ls.RecievedDate >= FromDate && ls.RecievedDate < ToDate) && (ls.StipulationStatus == StipulationType || StipulationType == 0))
                                     && (l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED)

                                    select l).ToList();
                //List<Loan> loans = (from ls in db.LoanStipulation.AsNoTracking()
                //                    join l in db.Loan.AsNoTracking() on ls.LoanID equals l.LoanID
                //                    where
                //                    (ls.RecievedDate > FromDate && ls.RecievedDate < ToDate) && StipulationType != 0 ? ls.StipulationStatus == StipulationType : StipulationType == 0
                //                    select l
                //               ).Distinct().Where(l=>l.ReviewTypeID == ReviewTypeID && l.CustomerID == CustomerID).ToList();
                _report = GetReportModel(db, loans);

            }
            return _report;

        }


        public object GetKpiGoalReportDetails(Int64 RoleId, Int64 UserId, DateTime FromDate, DateTime ToDate)
        {
            object data = null;
            FromDate = (new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0));
            ToDate = (new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)).AddDays(1);
            List<ReportResultModel> _report = new List<ReportResultModel>();
            List<KpiUserGroupConfig> Grplstobj = new List<KpiUserGroupConfig>();

            using (var db = new DBConnect(TableSchema))
            {

                List<Loan> loans = (from kpi in db.KPIGoalConfig.AsNoTracking()
                                    join l in db.Loan.AsNoTracking() on kpi.UserID equals l.CompletedUserID
                                    where kpi.UserID == UserId && l.CompletedUserRoleID == RoleId &&
                                    (l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate)
                                    && l.Status == StatusConstant.COMPLETE
                                    select l).Distinct().ToList();

                List<Loan> Auditkpiloans = (from Auditkpi in db.AuditUserKpiGoalConfig.AsNoTracking()
                                            join l in db.Loan.AsNoTracking() on Auditkpi.UserID equals l.CompletedUserID
                                            where Auditkpi.UserID == UserId && l.CompletedUserRoleID == RoleId &&
                                            (l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate)
                                             && l.Status == StatusConstant.COMPLETE
                                            select l).Distinct().ToList();
                if (loans.Count > 0)
                {
                    _report = GetReportModel(db, loans);

                }
                else if (Auditkpiloans.Count > 0)
                {
                    _report = GetReportModel(db, Auditkpiloans);

                }


            }
            return _report;
        }
        public object GetKpiUserGroupReportDetails(Int64 GroupID, DateTime FromDate, DateTime ToDate)
        {
            object data = null;
            return data;
        }

        public object GetLoanFailedRulesReport(DateTime FromDate, DateTime ToDate, Int64 LoanTypeID, Int64 ReviewTypeID, int RuleStatus)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                List<Loan> loans = (from lca in db.LoanChecklistAudit.AsNoTracking()
                                    join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                                    join cd in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals cd.CheckListDetailID
                                    where l.CreatedOn > FromDate && l.CreatedOn < ToDate
                                    && lca.LoanTypeID == LoanTypeID && lca.ReviewTypeID == ReviewTypeID && cd.Rule_Type == 0
                                          && ((RuleStatus == -1 && lca.Result == false)
                                          || (RuleStatus == 0 && lca.ErrorMessage.Length == 0 && lca.Result == false)
                                          || (RuleStatus == 2 && lca.ErrorMessage.Length > 0 && lca.Result == false))
                                          && (l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED)
                                    select l
                           ).Distinct().ToList();
                _report = GetReportModel(db, loans, RuleStatus);
            }
            return _report;
        }
        public object GetCriticalLoansFailedReport(DateTime FromDate, DateTime ToDate, Int64 LoanTypeID, string categoryName, Int64 customerID)
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                var loans = (from lca in db.LoanChecklistAudit.AsNoTracking()
                             join c in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals c.CheckListDetailID
                             join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                             where (l.CreatedOn > FromDate && l.CreatedOn < ToDate
                             && l.Status == StatusConstant.PENDING_AUDIT && lca.Result == false && c.Rule_Type == 0 && c.Category == categoryName)
                             group lca by new { lca.LoanID, lca.CustomerID } into g
                             select new
                             {
                                 Loan = g.Key.LoanID, // g["loan"],
                                 CustomerID = g.Key.CustomerID,
                                 Count = g.Count()
                             }

                           ).Where(l => l.CustomerID == customerID || customerID == 0).ToList();
                List<Loan> _loans = new List<Loan>();
                Dictionary<Int64, Int64> _count = new Dictionary<long, long>();
                foreach (var item in loans)
                {
                    Loan _loan = db.Loan.AsNoTracking().Where(l => l.LoanID == item.Loan).FirstOrDefault();
                    if (_loan != null)
                    {
                        _loans.Add(_loan);
                        _count[_loan.LoanID] = item.Count;
                    }
                }

                _report = GetReportModel(db, _loans, -1, _count, categoryName);
            }
            return _report;
        }
        #endregion

        #region Graph Report

        public async Task<object> GetTopOftheHouseGraph(DateTime DateFrom, DateTime DateTo, Int64 CustomerId, Int64 ReviewTypeID)
        {
            List<object> lm = new List<object>();
            DateFrom = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
            DateTo = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                List<Loan> _boxLoanLs = new List<Loan>();
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                //if (CustomerId == 0)
                //{
                //_boxLoanLs = db.Loan.AsNoTracking().Where(l => l.AuditMonthYear == AuditMonthYear && l.FromBox).ToList();
                //if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Audit_Due_Date")
                //{
                //    _boxLoanLs = (from l in db.Loan.AsNoTracking()
                //                  join ls in db.LoanSearch.AsNoTracking() on l.LoanID equals ls.LoanID
                //                  where l.ReviewTypeID == ReviewTypeID
                //                    && ((ls.AuditDueDate != null) && ls.AuditDueDate > DateFrom && ls.AuditDueDate < DateTo)
                //                    && (CustomerId == 0 || l.CustomerID == CustomerId)
                //                  select l).ToList<Loan>();
                //}
                // else if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                {
                    _boxLoanLs = (from l in db.Loan.AsNoTracking()
                                  select l).Where(a => a.CreatedOn > DateFrom && a.CreatedOn < DateTo && a.ReviewTypeID == ReviewTypeID && (CustomerId == 0 || a.CustomerID == CustomerId)).ToList<Loan>();
                }
                //_boxLoanLs = db.Loan.AsNoTracking().Where(l => l.CreatedOn > DateFrom && l.CreatedOn < DateTo && l.ReviewTypeID == ReviewTypeID).ToList();
                //}
                //else
                //{
                //    //_boxLoanLs = db.Loan.AsNoTracking().Where(l => l.AuditMonthYear == AuditMonthYear && l.FromBox && l.CustomerID == CustomerId).ToList();
                //    _boxLoanLs = db.Loan.AsNoTracking().Where(l => l.CreatedOn > DateFrom && l.CreatedOn < DateTo && l.CustomerID == CustomerId && l.ReviewTypeID == ReviewTypeID).ToList();
                //}


                List<Loan> _readyForIDC = _boxLoanLs.Where(b => (b.Status == StatusConstant.READY_FOR_IDC || b.Status == StatusConstant.PENDING_BOX_DOWNLOAD)).ToList();

                List<Loan> _pendingIDC = _boxLoanLs.Where(i => i.Status == StatusConstant.PENDING_IDC).ToList();

                List<Loan> _IDCCompleted = _boxLoanLs.Where(i => i.Status == StatusConstant.PENDING_AUDIT && i.SubStatus == StatusConstant.EXTRACTION_COMPLETED).ToList();

                List<Loan> _AuditComplete = _boxLoanLs.Where(i => (i.Status == StatusConstant.COMPLETE)).ToList();

                await Task.Run(() =>
                {
                    lm.Add(new
                    {
                        name = "Loan Imported",
                        y = _boxLoanLs.Count(),
                        drilldown = "Loan Imported"
                    });

                    lm.Add(new
                    {
                        name = "Ready for IDC",
                        y = _readyForIDC.Count(),
                        drilldown = "Ready for IDC"
                    });

                    lm.Add(new
                    {
                        name = "Pending IDC",
                        y = _pendingIDC.Count(),
                        drilldown = "Pending IDC"
                    });

                    lm.Add(new
                    {
                        name = "IDC Completed",
                        y = _IDCCompleted.Count(),
                        drilldown = "IDC Completed"
                    });
                    lm.Add(new
                    {
                        name = "Complete Audit",
                        y = _AuditComplete.Count(),
                        drilldown = "Complete Audit"
                    });
                });
            }

            return lm;
        }

        public async Task<object> GetIDCWorkLoadGraph(DateTime DateFrom, DateTime DateTo, Int64 DataEntryType, Int64 CustomerID)
        {
            List<object> lm = new List<object>();
            DateFrom = new DateTime(DateFrom.Year, DateFrom.Month, DateFrom.Day, 0, 0, 0);
            DateTo = new DateTime(DateTo.Year, DateTo.Month, DateTo.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {

                var loans = (from idc in db.IDCFields.AsNoTracking()
                             join l in db.Loan.AsNoTracking() on idc.LoanID equals l.LoanID
                             where (CustomerID == -1 || l.CustomerID == CustomerID)
                                 && (l.CreatedOn > DateFrom && l.CreatedOn < DateTo)
                                 && idc.IDCBatchInstanceID != null
                                 && idc.IDCBatchInstanceID != ""
                             select new
                             {
                                 CustomerID = l.CustomerID,
                                 BatchID = idc.IDCBatchInstanceID
                             }).ToList();

                List<string> batchIdentifier = loans.Select(x => "'" + x.BatchID + "'").Distinct().ToList();

                string type = DataEntryType == 1 ? "READY_FOR_REVIEW" : "READY_FOR_VALIDATION";
                string sql = $"select batch_status, identifier from batch_instance where batch_status = '{type}' AND identifier in ({string.Join(",", batchIdentifier)}) ";
                System.Data.DataTable dt = new DataAccess2("EphesoftConnectionName").ExecuteDataTable(sql);
                Dictionary<Int64, Int32> customerBatchCount = new Dictionary<long, int>();
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var batchDetail = loans.Where(x => x.BatchID == dr["identifier"].ToString()).FirstOrDefault();
                        if (batchDetail != null)
                        {
                            if (customerBatchCount.ContainsKey(batchDetail.CustomerID))
                                customerBatchCount[batchDetail.CustomerID] = customerBatchCount[batchDetail.CustomerID] + 1;
                            else
                                customerBatchCount[batchDetail.CustomerID] = 1;
                        }
                    }
                }

                await Task.Run(() =>
                {
                    foreach (var _customerID in customerBatchCount.Keys)
                    {
                        CustomerMaster cus = db.CustomerMaster.AsNoTracking().Where(x => x.CustomerID == _customerID).FirstOrDefault();
                        if (cus != null)
                        {
                            lm.Add(new
                            {
                                y = customerBatchCount[_customerID],
                                name = cus.CustomerName,
                                drilldown = cus.CustomerName,
                                id = cus.CustomerName
                            });
                        }
                    }
                });
            }

            return lm;
        }


        public async Task<object> GetMissingRecordedLoansGraph(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            Int64 loanCount = 0;
            FromDate = (new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0));
            ToDate = (new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                await Task.Run(() =>
                {
                    List<Int64> _distLoanIDs = new List<long>();
                    if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                    {
                        _distLoanIDs = (from lr in db.LoanReporting.AsNoTracking()
                                        join a in db.Loan.AsNoTracking() on lr.LoanID equals a.LoanID
                                        where lr.AddToReport == true && (a.CreatedOn > FromDate && a.CreatedOn < ToDate) && a.ReviewTypeID == ReviewTypeID
                                                         && (a.Status != StatusConstant.LOAN_DELETED && a.Status != StatusConstant.LOAN_EXPIRED && a.Status != StatusConstant.LOAN_PURGED)
                                        select a.LoanID).Distinct().ToList();
                    }
                    loanCount = _distLoanIDs.Count();
                });
            }
            return loanCount;
        }
        class missLoanObject
        {
            public Int64 LoanID { get; set; }
            public string CustomerName { get; set; }
            public Int64 CustomerID { get; set; }
            public Int64 StipulationStatus { get; set; }
        }
        public async Task<List<object>> GetMissingCriticalDocumentGraph(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            var lm = new List<object>();

            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);

            using (var db = new DBConnect(TableSchema))
            {
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                List<missLoanObject> _loanIDs = new List<missLoanObject>();
                if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                {
                    _loanIDs = (from l in db.Loan.AsNoTracking()
                                join ld in db.LoanDetail.AsNoTracking() on l.LoanID equals ld.LoanID
                                join ls in db.LoanSearch.AsNoTracking() on l.LoanID equals ls.LoanID
                                join c in db.CustomerMaster.AsNoTracking() on l.CustomerID equals c.CustomerID
                                where l.ReviewTypeID == ReviewTypeID
                                && (l.CreatedOn > FromDate && l.CreatedOn < ToDate)
                                && ld.MissingCriticalDocCount > 0
                                && l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED
                                select new missLoanObject
                                {
                                    LoanID = l.LoanID,
                                    CustomerName = c.CustomerName,
                                    CustomerID = l.CustomerID
                                }).ToList();

                }



                await Task.Run(() =>
                {
                    lm = (from c in _loanIDs
                          group c by new
                          {
                              c.CustomerID,
                              c.CustomerName
                          } into gcs
                          select new
                          {
                              name = gcs.Key.CustomerName,
                              drilldown = gcs.Key.CustomerID,
                              id = gcs.Key.CustomerID,
                              y = gcs.Count(),
                          }).ToList<object>();
                });

            }

            return lm;
        }

        public async Task<object> GetDocumentRetentionMonitoringGraph(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            object _loan = null;
            FromDate = (new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0));
            ToDate = (new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0)).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                await Task.Run(() =>
                {
                    if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                    {

                        _loan = (from l in db.Loan.AsNoTracking()
                                 join cus in db.CustomerMaster on l.CustomerID equals cus.CustomerID
                                 join lty in db.LoanTypeMaster on l.LoanTypeID equals lty.LoanTypeID
                                 join ls in db.LoanSearch on l.LoanID equals ls.LoanID
                                 where l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.Status == StatusConstant.LOAN_EXPIRED && l.ReviewTypeID == ReviewTypeID
                                 select new
                                 {
                                     LoanID = l.LoanID,
                                     LoanNumber = l.LoanNumber,
                                     LoanType = lty.LoanTypeName,
                                     BorrowerName = ls.BorrowerName,
                                     CustomerName = cus.CustomerName,
                                     Status = l.Status
                                 }).ToList();

                    }
                });

                //  _loanCount = db.Loan.AsNoTracking().Where(l => l.CreatedOn > FromDate && l.CreatedOn < ToDate && l.Status == StatusConstant.LOAN_EXPIRED && l.ReviewTypeID == ReviewTypeID).ToList().Count();
            }
            return _loan;
        }

        public async Task<object> GetDataEntryWorloadGraph(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            List<GraphObject> dataEntryGraphData = new List<GraphObject>();

            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);

            using (var db = new DBConnect(TableSchema))
            {
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                //if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Audit_Due_Date")
                //{
                //    dataEntryGraphData = (from l in db.Loan.AsNoTracking()
                //                          join r in db.UserRoleMapping.AsNoTracking() on l.LastAccessedUserID equals r.UserID
                //                          join ls in db.LoanSearch.AsNoTracking() on l.LoanID equals ls.LoanID
                //                          where l.ReviewTypeID == ReviewTypeID
                //                          && ((ls.AuditDueDate != null) && ls.AuditDueDate > FromDate && ls.AuditDueDate < ToDate)
                //                          && l.Status == StatusConstant.COMPLETE && r.RoleID == RoleConstant.DATA_ENTRY
                //                          group l by new { l.LastAccessedUserID } into g
                //                          select new GraphObject()
                //                          {
                //                              name = "",
                //                              y = g.Count(),
                //                              drilldown = g.Key.LastAccessedUserID,
                //                              id = g.Key.LastAccessedUserID
                //                          }
                //                      ).ToList<GraphObject>();
                //}
                // else if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                await Task.Run(() =>
                {
                    if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                    {
                        dataEntryGraphData = (from l in db.Loan.AsNoTracking()
                                              join r in db.UserRoleMapping.AsNoTracking() on l.LastAccessedUserID equals r.UserID
                                              join ls in db.LoanSearch.AsNoTracking() on l.LoanID equals ls.LoanID
                                              where l.ReviewTypeID == ReviewTypeID
                                              && l.CreatedOn > FromDate && l.CreatedOn < ToDate
                                              && l.Status == StatusConstant.COMPLETE && r.RoleID == RoleConstant.DATA_ENTRY
                                              group l by new { l.LastAccessedUserID } into g
                                              select new GraphObject()
                                              {
                                                  name = "",
                                                  y = g.Count(),
                                                  drilldown = g.Key.LastAccessedUserID,
                                                  id = g.Key.LastAccessedUserID
                                              }
                                         ).ToList<GraphObject>();
                    }

                    foreach (var item in dataEntryGraphData)
                    {
                        User _user = db.Users.AsNoTracking().Where(us => us.UserID == item.id).FirstOrDefault();

                        if (_user != null)
                            item.name = $"{_user.LastName} {_user.FirstName}";
                    }
                });

            }

            return dataEntryGraphData;
        }

        public async Task<object> GetCheckListFailedLoansGraph(DateTime FromDate, DateTime ToDate, Int64 ReviewTypeID)
        {
            Int64 LoanCount = 0;
            Int64 TotalLoanCount = 0;

            //ToDate = ToDate.AddDays(1);
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);

            using (var db = new DBConnect(TableSchema))
            {
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                //if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Audit_Due_Date")
                //{
                //    LoanCount = (from lca in db.LoanChecklistAudit.AsNoTracking()
                //                 join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                //                 join ls in db.LoanSearch.AsNoTracking() on l.LoanID equals ls.LoanID
                //                 where l.ReviewTypeID == ReviewTypeID
                //                  && ((ls.AuditDueDate != null) && ls.AuditDueDate > FromDate && ls.AuditDueDate < ToDate)
                //                 && lca.Result == false
                //                 select l
                //           ).Distinct().Count();
                //}
                //else
                //if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                await Task.Run(() =>
                {
                    if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                    {
                        var rules = (from lca in db.LoanChecklistAudit.AsNoTracking()
                                     join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                                     join c in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals c.CheckListDetailID
                                     where l.ReviewTypeID == ReviewTypeID && (l.CreatedOn > FromDate && l.CreatedOn < ToDate) && c.Rule_Type == 0 // && lca.Result == false
                                     && (l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED)
                                     select lca
                               //).Distinct().Count();
                               ).ToList();

                        //TotalLoanCount = db.Loan.Where(x => x.ReviewTypeID == ReviewTypeID && (x.CreatedOn > FromDate && x.CreatedOn < ToDate)).Count();
                        TotalLoanCount = rules.ToList().Count;
                        LoanCount = rules.Where(x => x.Result == true).ToList().Count;
                    }
                });

            }

            return new { FailedLoanCount = LoanCount, TotalLoanCount = TotalLoanCount };
        }

        public async Task<object> GetLoanInvestorStipulationGraph(Int64 ReviewTypeID, DateTime FromDate, DateTime ToDate, Int64 StipulationType)
        {
            List<object> StipulationData = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                List<missLoanObject> _loanstipulation = new List<missLoanObject>();
                List<Loan> _investorloan = new List<Loan>();
                ReviewTypeMaster _reviewtypemster = db.ReviewTypeMaster.AsNoTracking().Where(x => x.ReviewTypeID == ReviewTypeID).FirstOrDefault();
                if (_reviewtypemster != null && _reviewtypemster.SearchCriteria == "Received_Date")
                {

                    _loanstipulation = (from ls in db.LoanStipulation.AsNoTracking()
                                        join c in db.CustomerMaster.AsNoTracking() on ls.CustomerID equals c.CustomerID
                                        join l in db.Loan.AsNoTracking() on ls.LoanID equals l.LoanID
                                        where
                                        ls.ReviewTypeID == ReviewTypeID &&
                                       (ls.RecievedDate >= FromDate && ls.RecievedDate < ToDate)
                                       && (l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED)
                                        select new missLoanObject
                                        {
                                            LoanID = ls.LoanID,
                                            CustomerName = c.CustomerName,
                                            CustomerID = ls.CustomerID,
                                            StipulationStatus = ls.StipulationStatus
                                        }).Where(l => l.StipulationStatus == StipulationType || StipulationType == 0).ToList();
                }

                await Task.Run(() =>
                {
                    StipulationData = (from ls in _loanstipulation
                                       group ls by new
                                       {
                                           ls.CustomerID,
                                           ls.CustomerName
                                       } into GLstipulation
                                       select new
                                       {
                                           name = GLstipulation.Key.CustomerName,
                                           drilldown = GLstipulation.Key.CustomerID,
                                           id = GLstipulation.Key.CustomerID,
                                           y = GLstipulation.Count(),
                                       }).ToList<object>();
                });
            }
            return StipulationData;

        }

        public async Task<object> GetKpiGoalDashboard(Int64 RoleID, Int64 UserGroupID, bool Flag, DateTime FromDate, DateTime ToDate)
        {
            List<KPIGoalConfig> _kpicongigList = new List<KPIGoalConfig>();
            AuditKpiGoalConfig _auditList = new AuditKpiGoalConfig();
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);

            List<object> lstobj = new List<object>();
            List<object> loancount = new List<object>();
            await Task.Run(() =>
            {
                using (var db = new DBConnect(TableSchema))
                {
                    _kpicongigList = db.KPIGoalConfig.AsNoTracking().Where(x => x.UserGroupID == UserGroupID).ToList();
                    if (Flag == true)
                    {
                        _auditList = db.AuditKpiGoalConfig.AsNoTracking().Where(a => a.AuditGoalID == UserGroupID && a.PeriodFrom >= FromDate && a.PeriodTo < ToDate).FirstOrDefault();
                        List<UserRoleMapping> userRole = db.UserRoleMapping.AsNoTracking().Where(x => x.RoleID == _auditList.RoleID).ToList();

                        foreach (UserRoleMapping user in userRole)
                        {
                            List<Loan> loans = db.Loan.AsNoTracking().Where(l => l.CompletedUserRoleID == user.RoleID && l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate).ToList();
                            if (loans.Count > 0)
                            {
                                loancount.Add(loans);
                            }
                        }
                        if (loancount.Count > 0)
                        {
                            foreach (UserRoleMapping map in userRole)
                            {

                                var loandetails = (from l in db.Loan.AsNoTracking()
                                                   where
                                                          l.CompletedUserRoleID == map.RoleID
                                                          && (l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate)
                                                          && (l.Status == StatusConstant.COMPLETE || l.Status == StatusConstant.LOAN_PURGED || l.Status == StatusConstant.LOAN_EXPIRED || l.Status == StatusConstant.PURGE_WAITING
                                                               || l.Status == StatusConstant.PURGE_WAITING || l.Status == StatusConstant.PURGE_FAILED || l.Status == StatusConstant.EXPORT_WAITING || l.Status == StatusConstant.EXPORT_FAILED || l.Status == StatusConstant.LOAN_EXPORTED)
                                                   select l).Count();

                                var data = new
                                {
                                    UserID = map.UserID,
                                    FirstName = db.Users.Where(x => x.UserID == map.UserID).FirstOrDefault().FirstName,
                                    LastName = db.Users.Where(x => x.UserID == map.UserID).FirstOrDefault().LastName,
                                    AchievedGoalCount = loandetails,
                                    GoalCount = _auditList.Goal / userRole.Count(),
                                    PeriodFrom = _auditList.PeriodFrom,
                                    PeriodTo = _auditList.PeriodTo

                                };
                                if (data != null)
                                {
                                    lstobj.Add(data);
                                }
                            }
                        }

                    }
                    else
                    {
                        if (_kpicongigList.Count > 0)
                        {
                            foreach (KPIGoalConfig item in _kpicongigList)
                            {
                                FromDate = new DateTime(item.PeriodFrom.GetValueOrDefault().Year, item.PeriodFrom.GetValueOrDefault().Month, item.PeriodFrom.GetValueOrDefault().Day, 0, 0, 0);
                                ToDate = new DateTime(item.PeriodTo.GetValueOrDefault().Year, item.PeriodTo.GetValueOrDefault().Month, item.PeriodTo.GetValueOrDefault().Day, 0, 0, 0).AddDays(1);
                                List<Loan> loans = db.Loan.AsNoTracking().Where(l => l.CompletedUserID == item.UserID && l.CompletedUserRoleID == RoleID && l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate).ToList();
                                if (loans.Count > 0)
                                {
                                    loancount.Add(loans);
                                }
                            }
                            if (loancount.Count > 0)
                            {
                                foreach (KPIGoalConfig item in _kpicongigList)
                                {
                                    FromDate = new DateTime(item.PeriodFrom.GetValueOrDefault().Year, item.PeriodFrom.GetValueOrDefault().Month, item.PeriodFrom.GetValueOrDefault().Day, 0, 0, 0);
                                    ToDate = new DateTime(item.PeriodTo.GetValueOrDefault().Year, item.PeriodTo.GetValueOrDefault().Month, item.PeriodTo.GetValueOrDefault().Day, 0, 0, 0).AddDays(1);

                                    List<Loan> loans = db.Loan.AsNoTracking().Where(l => l.CompletedUserRoleID == RoleID && l.CompletedUserID == item.UserID && l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate).ToList();
                                    var Kpiloandetails = (from l in db.Loan.AsNoTracking()
                                                          where
                                                                 l.CompletedUserID == item.UserID &&
                                                                 l.CompletedUserRoleID == RoleID
                                                                 && (l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate)
                                                                 && (l.Status == StatusConstant.COMPLETE || l.Status == StatusConstant.LOAN_PURGED || l.Status == StatusConstant.LOAN_EXPIRED || l.Status == StatusConstant.PURGE_WAITING
                                                                      || l.Status == StatusConstant.PURGE_WAITING || l.Status == StatusConstant.PURGE_FAILED || l.Status == StatusConstant.EXPORT_WAITING || l.Status == StatusConstant.EXPORT_FAILED || l.Status == StatusConstant.LOAN_EXPORTED)
                                                          select l).ToList();
                                    var data = new
                                    {

                                        UserID = item.UserID,
                                        FirstName = db.Users.Where(x => x.UserID == item.UserID).FirstOrDefault().FirstName,
                                        LastName = db.Users.Where(x => x.UserID == item.UserID).FirstOrDefault().LastName,
                                        AchievedGoalCount = Kpiloandetails.Count(),
                                        GoalCount = _kpicongigList.Where(x => x.UserID == item.UserID).FirstOrDefault().Goal,
                                        PeriodFrom = _kpicongigList.Where(x => x.UserID == item.UserID).FirstOrDefault().PeriodFrom,
                                        PeriodTo = _kpicongigList.Where(x => x.UserID == item.UserID).FirstOrDefault().PeriodTo,

                                    };
                                    if (data != null)
                                    {
                                        lstobj.Add(data);
                                    }

                                }
                            }

                        }
                    }
                }
            });
            return lstobj;
        }
        public async Task<object> GetKpiUserGroupDashboard()
        {
            List<object> FinalObj = new List<object>();
            using (var db = new DBConnect(TableSchema))
            {
                List<KpiUserGroupConfig> Grplstobj = new List<KpiUserGroupConfig>();
                List<KpiUserConfigList> KPIList = new List<KpiUserConfigList>();
                Grplstobj = db.KpiUserGroupConfig.AsNoTracking().ToList();  // Current Kpi Group Config
                foreach (KpiUserGroupConfig userGroupCOnfig in Grplstobj)
                {
                    KPIList = new List<KpiUserConfigList>();
                    List<KPIGoalConfig> _kpigoalList = db.KPIGoalConfig.Where(a => a.UserGroupID == userGroupCOnfig.GroupID).ToList();

                    List<AuditKpiGoalConfig> _allAuditGroupList = db.AuditKpiGoalConfig.Where(a => a.UserGroupID == userGroupCOnfig.GroupID).OrderByDescending(a => a.PeriodFrom).ToList();
                    List<AuditKpiGoalConfig> _auditGroupList = new List<AuditKpiGoalConfig>();
                    var _distinctAudit = (from p in _allAuditGroupList
                                          group p by new { p.PeriodFrom, p.PeriodTo }
                    into groupList
                                          select new AuditKpiGoalConfig
                                          {
                                              PeriodFrom = groupList.Key.PeriodFrom,
                                              PeriodTo = groupList.Key.PeriodTo
                                          }).ToList();

                    foreach (var item in _distinctAudit)
                    {
                        _auditGroupList.Add(_allAuditGroupList.Where(a => a.PeriodFrom == item.PeriodFrom && a.PeriodTo == item.PeriodTo).OrderByDescending(aa => aa.AuditGoalID).FirstOrDefault());
                    }

                    await Task.Run(() =>
                    {

                        foreach (KPIGoalConfig kpiItem in _kpigoalList)
                        {
                            DateTime FromDate = new DateTime(kpiItem.PeriodFrom.GetValueOrDefault().Year, kpiItem.PeriodFrom.GetValueOrDefault().Month, kpiItem.PeriodFrom.GetValueOrDefault().Day, 0, 0, 0);
                            DateTime ToDate = new DateTime(kpiItem.PeriodTo.GetValueOrDefault().Year, kpiItem.PeriodTo.GetValueOrDefault().Month, kpiItem.PeriodTo.GetValueOrDefault().Day, 0, 0, 0).AddDays(1);

                            List<Loan> Kpiloandetails = new List<Loan>();
                            Kpiloandetails = (from l in db.Loan.AsNoTracking()
                                              where
                                                     l.CompletedUserRoleID == userGroupCOnfig.RoleID
                                                     && (l.AuditCompletedDate >= FromDate && l.AuditCompletedDate < ToDate)
                                                     && (l.Status == StatusConstant.COMPLETE)
                                              select l).Distinct().ToList();
                            List<KpiUserConfigList> kpidata = new List<KpiUserConfigList>();

                            if (Kpiloandetails.Count > 0)
                            {
                                kpidata = (from kpi in Kpiloandetails
                                           group kpi by new
                                           {
                                               kpi.CompletedUserID,
                                               kpi.CompletedUserRoleID,
                                               kpi.Status
                                           } into KpiList
                                           select new KpiUserConfigList
                                           {

                                               UserID = KpiList.Key.CompletedUserID,
                                               RoleID = KpiList.Key.CompletedUserRoleID,
                                               UserGroupId = userGroupCOnfig.GroupID,
                                               FirstName = db.Users.Where(x => x.UserID == KpiList.Key.CompletedUserID).FirstOrDefault().FirstName,
                                               LastName = db.Users.Where(x => x.UserID == KpiList.Key.CompletedUserID).FirstOrDefault().LastName,
                                               UserGroupName = db.Roles.Where(x => x.RoleID == KpiList.Key.CompletedUserRoleID).FirstOrDefault().RoleName,
                                               AchievedGoalCount = Kpiloandetails.Count(),
                                               Goal = userGroupCOnfig.Goal,
                                               PeriodFrom = userGroupCOnfig.PeriodFrom,
                                               PeriodTo = userGroupCOnfig.PeriodTo,
                                               ConfigType = StatusConstant.KPIConfig[userGroupCOnfig.ConfigType]
                                           }).Distinct().ToList();
                            }
                            else
                            {
                                KpiUserConfigList _tempdata = new KpiUserConfigList();
                                _tempdata.UserID = 0;
                                _tempdata.RoleID = userGroupCOnfig.RoleID;
                                _tempdata.UserGroupId = userGroupCOnfig.GroupID;
                                _tempdata.FirstName = "";
                                _tempdata.LastName = "";
                                _tempdata.UserGroupName = db.Roles.Where(x => x.RoleID == userGroupCOnfig.RoleID).FirstOrDefault().RoleName;
                                _tempdata.AchievedGoalCount = Kpiloandetails.Count();
                                _tempdata.Goal = userGroupCOnfig.Goal;
                                _tempdata.PeriodFrom = userGroupCOnfig.PeriodFrom;
                                _tempdata.PeriodTo = userGroupCOnfig.PeriodTo;
                                _tempdata.ConfigType = StatusConstant.KPIConfig[userGroupCOnfig.ConfigType];
                                kpidata.Add(_tempdata);
                            }

                            foreach (KpiUserConfigList item in kpidata)
                            {
                                if (!KPIList.Any(l => l.RoleID == item.RoleID))
                                {
                                    KPIList.Add(item);
                                }
                            }


                            var obdatas = (from kpi in KPIList
                                           group kpi by new
                                           {
                                               kpi.UserID,
                                               kpi.RoleID,
                                               kpi.UserGroupId
                                           } into _KpiList
                                           select new KpiUserConfigList
                                           {
                                               UserID = _KpiList.Key.UserID,
                                               UserGroupId = _KpiList.Key.UserGroupId,
                                               UserGroupName = db.Roles.AsNoTracking().Where(x => x.RoleID == _KpiList.Key.RoleID).FirstOrDefault().RoleName,
                                               AchievedGoalCount = KPIList.Where(k => k.UserGroupId == _KpiList.Key.UserGroupId).Select(k => k.AchievedGoalCount).Sum(),
                                               Goal = KPIList.Where(k => k.UserGroupId == _KpiList.Key.UserGroupId).FirstOrDefault().Goal,
                                               RoleID = _KpiList.Key.RoleID,
                                               PeriodFrom = KPIList.Where(k => k.UserGroupId == _KpiList.Key.UserGroupId).FirstOrDefault().PeriodFrom,
                                               PeriodTo = KPIList.Where(k => k.UserGroupId == _KpiList.Key.UserGroupId).FirstOrDefault().PeriodTo,
                                               auditconfigdetails = _auditGroupList,
                                               ConfigType = KPIList.Where(k => k.UserGroupId == _KpiList.Key.UserGroupId).FirstOrDefault().ConfigType
                                           }).Distinct().ToList();
                            if (obdatas.Count > 0)
                            {
                                FinalObj.Add(obdatas);
                            }
                        }
                    });
                }
            }

            return FinalObj;
        }
        //private List<AuditGoalConfigDetails> getAuditUserGroupGoalConfig(AuditKpiGoalConfig _auditgoalconfig)
        //{
        //    List<AuditGoalConfigDetails> AuditKPIList = new List<AuditGoalConfigDetails>();
        //    using (var db = new DBConnect(TableSchema))
        //    {
        //        List<AuditUserKpiGoalConfig> _auditusergroupconfig = db.AuditUserKpiGoalConfig.AsNoTracking().Where(x => x.UserGroupID == _auditgoalconfig.UserGroupID).ToList();
        //        foreach (AuditUserKpiGoalConfig items in _auditusergroupconfig)
        //        {
        //            DateTime FromDate = new DateTime(items.PeriodFrom.GetValueOrDefault().Year, items.PeriodFrom.GetValueOrDefault().Month, items.PeriodFrom.GetValueOrDefault().Day, 0, 0, 0);
        //            DateTime ToDate = new DateTime(items.PeriodTo.GetValueOrDefault().Year, items.PeriodTo.GetValueOrDefault().Month, items.PeriodTo.GetValueOrDefault().Day, 0, 0, 0).AddDays(1);

        //            List<Loan> AuditKpiloandetails = new List<Loan>();
        //            AuditKpiloandetails = (from l in db.Loan.AsNoTracking()
        //                                   where
        //                                          l.AssignedUserID == items.UserID
        //                                          && (l.CreatedOn >= FromDate && l.CreatedOn < ToDate)
        //                                          && (l.Status == StatusConstant.COMPLETE)
        //                                   select l).Distinct().ToList();
        //            List<AuditGoalConfigDetails> Auditkpidata = new List<AuditGoalConfigDetails>();

        //            if (AuditKpiloandetails.Count > 0)
        //            {
        //                Auditkpidata = (from Auditkpi in AuditKpiloandetails
        //                           group Auditkpi by new
        //                           {
        //                               Auditkpi.AssignedUserID,
        //                               Auditkpi.Status
        //                           } into AuditKpiList
        //                           select new AuditGoalConfigDetails
        //                           {
        //                               UserID = AuditKpiList.Key.AssignedUserID,
        //                               RoleID = _auditgoalconfig.RoleID,
        //                               UserGroupID = _auditgoalconfig.UserGroupID,
        //                               AchievedGoalCount = AuditKpiloandetails.Count(),
        //                               Goal = _auditgoalconfig.Goal,
        //                               PeriodFrom = _auditgoalconfig.PeriodFrom,
        //                               PeriodTo = _auditgoalconfig.PeriodTo,
        //                           }).Distinct().ToList();
        //            }
        //            foreach (AuditGoalConfigDetails item in Auditkpidata)
        //            {
        //                if (!AuditKPIList.Any(x => x.RoleID == item.RoleID))
        //                {
        //                    AuditKPIList.Add(item);
        //                }

        //            }
        //        }
        //    }
        //    return AuditKPIList;
        //}
        public async Task<object> GetLoanFailedRulesGraph(DateTime FromDate, DateTime ToDate, Int64 reviewTypeID, int RuleStatus)
        {

            List<object> lm = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                await Task.Run(() =>
                {

                    var _rules = (from lca in db.LoanChecklistAudit.AsNoTracking()
                                  join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                                  join cd in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals cd.CheckListDetailID
                                  join lt in db.LoanTypeMaster.AsNoTracking() on lca.LoanTypeID equals lt.LoanTypeID
                                  where l.CreatedOn > FromDate && l.CreatedOn <= ToDate && lca.ReviewTypeID == reviewTypeID && cd.Rule_Type == 0
                                  && ((RuleStatus == -1 && lca.Result == false)
                                  || (RuleStatus == 0 && lca.ErrorMessage.Length == 0 && lca.Result == false)
                                  || (RuleStatus == 2 && lca.ErrorMessage.Length > 0 && lca.Result == false))
                                  && (l.Status != StatusConstant.LOAN_DELETED && l.Status != StatusConstant.LOAN_EXPIRED && l.Status != StatusConstant.LOAN_PURGED)
                                  select new
                                  {
                                      LoanID = l.LoanID,
                                      LoanTypeName = lt.LoanTypeName,
                                      LoanTypeID = lt.LoanTypeID
                                  }).Distinct().ToList();

                    lm = (from r in _rules
                          group r by new
                          {
                              r.LoanTypeID,
                              r.LoanTypeName

                          } into gcs
                          select new
                          {
                              name = gcs.Key.LoanTypeName,
                              drilldown = gcs.Key.LoanTypeID,
                              id = gcs.Key.LoanTypeID,
                              y = gcs.Count(),
                          }).ToList<object>();
                });
                return lm;
            }
        }

        public async Task<object> GetCriticalLoansFailedGraph(DateTime FromDate, DateTime ToDate, Int64 reviewTypeID, Int64 customerID)
        {

            List<object> lm = null;
            FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 0, 0, 0).AddDays(1);
            using (var db = new DBConnect(TableSchema))
            {
                await Task.Run(() =>
                {

                    var _rules = (from lca in db.LoanChecklistAudit.AsNoTracking()
                                  join l in db.Loan.AsNoTracking() on lca.LoanID equals l.LoanID
                                  join c in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals c.CheckListDetailID
                                  //join lt in db.LoanTypeMaster.AsNoTracking() on lca.LoanTypeID equals lt.LoanTypeID
                                  where l.Status == StatusConstant.PENDING_AUDIT && l.CreatedOn > FromDate && l.CreatedOn <= ToDate && c.Rule_Type == 0 && lca.Result == false
                                  select new
                                  {
                                      LoanID = l.LoanID,
                                      CategoryName = c.Category,
                                      CustomerID = l.CustomerID,
                                      CheckListDetailID = c.CheckListDetailID,
                                      //LoanTypeName = lt.LoanTypeName,
                                      //LoanTypeID = lt.LoanTypeID
                                  }).Where(l => l.CustomerID == customerID || customerID == 0).Distinct().ToList();

                    lm = (from r in _rules
                          group r.CategoryName by r.CategoryName
                           into gcs
                          select new
                          {
                              name = gcs.Key,
                              drilldown = gcs.Key,
                              id = gcs.Key,
                              y = gcs.Count(),
                          }).ToList<object>();

                });

                return lm;
            }
        }
        #endregion

        #region Private Report Methods

        private List<ReportResultModel> GetReportModel(DBConnect db, List<Loan> _loans, int _ruleStatus = -1, Dictionary<Int64, Int64> _checklistCategoryCount = null, string categoryName = "")
        {
            List<ReportResultModel> _report = new List<ReportResultModel>();
            List<ReportConfig> _reportConfig = db.ReportConfig.AsNoTracking().ToList();
            foreach (Loan l in _loans)
            {
                try
                {
                    LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == l.LoanID).FirstOrDefault();
                    ReportResultModel _rptLoan = new ReportResultModel();
                    List<LoanChecklistAudit> lsCheckAudit = (from lca in db.LoanChecklistAudit.AsNoTracking()
                                                             join cd in db.CheckListDetailMaster.AsNoTracking() on lca.ChecklistDetailID equals cd.CheckListDetailID
                                                             where lca.LoanID == l.LoanID
                                                             && ((_ruleStatus == -1 && cd.Rule_Type == 0 && lca.Result == false)
                                                             || (_ruleStatus == 0 && lca.ErrorMessage.Length == 0 && lca.Result == false && cd.Rule_Type == 0)
                                                             || (_ruleStatus == 2 && lca.ErrorMessage.Length > 0 && lca.Result == false && cd.Rule_Type == 0))
                                                             select lca).ToList();

                    List<LoanReporting> _loanReporting = db.LoanReporting.AsNoTracking().Where(x => x.LoanID == l.LoanID && x.AddToReport == true).ToList();
                    var idcfields = db.IDCFields.AsNoTracking().Where(x => x.LoanID == l.LoanID).FirstOrDefault();
                    var _loanLOSFields = db.LoanLOSFields.AsNoTracking().Where(x => x.LoanID == l.LoanID).FirstOrDefault();
                    _rptLoan.LoanID = l.LoanID;
                    _rptLoan.OCRAccuracyCalculated = idcfields != null ? idcfields.OCRAccuracyCalculated != null ? idcfields.OCRAccuracyCalculated.Value : false : false;
                    _rptLoan.LoanNumber = string.IsNullOrEmpty(l.LoanNumber) ? string.Empty : l.LoanNumber;
                    _rptLoan.StatusDescription = StatusConstant.GetStatusDescription(l.Status);
                    _rptLoan.AuditMonthYear = l.AuditMonthYear != null ? l.AuditMonthYear.ToString("MMMM", CultureInfo.InvariantCulture) + " " + l.AuditMonthYear.Year.ToString() : "";
                    _rptLoan.CustomerName = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == l.CustomerID).FirstOrDefault().CustomerName;
                    _rptLoan.ServiceTypeName = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == l.ReviewTypeID).FirstOrDefault().ReviewTypeName;
                    _rptLoan.LoanTypeName = string.Empty;
                    _rptLoan.DataEntryName = string.Empty;
                    _rptLoan.StatusID = l.Status;
                    _rptLoan.PageCount = l.PageCount;
                    _rptLoan.OCRAccuracy = idcfields != null ? idcfields.IDCOCRAccuracy == null ? "0.00" : idcfields.IDCOCRAccuracy.ToString() : "0.00";
                    _rptLoan.ClassificationAccuracy = idcfields != null ? idcfields.ClassificationAccuracy == null ? "0.00" : idcfields.ClassificationAccuracy.Value.ToString() : "0.00";
                    _rptLoan.EphesoftReviewerName = idcfields != null ? string.IsNullOrEmpty(idcfields.IDCReviewerName) ? string.Empty : idcfields.IDCReviewerName : string.Empty;
                    _rptLoan.EphesoftValidatorName = idcfields != null ? string.IsNullOrEmpty(idcfields.IDCValidatorName) ? string.Empty : idcfields.IDCValidatorName : string.Empty;
                    _rptLoan.ReviewCompletionDate = idcfields != null ? idcfields.IDCLevelOneCompletionDate : null;
                    _rptLoan.ValidationCompletionDate = idcfields != null ? idcfields.IDCLevelTwoCompletionDate : null;
                    _rptLoan.AuditCompletionDate = idcfields != null ? idcfields.IDCCompletionDate : null;
                    _rptLoan.LoanDuration = string.IsNullOrEmpty(l.LoanDuration) ? string.Empty : l.LoanDuration;
                    _rptLoan.EphesoftBatchInstanceID = idcfields != null ? idcfields.IDCBatchInstanceID : string.Empty;
                    _rptLoan.IDCReviewDuration = idcfields != null ? idcfields.IDCLevelOneDuration : string.Empty;
                    _rptLoan.IDCValidationDuration = idcfields != null ? idcfields.IDCLevelTwoDuration : string.Empty;
                    _rptLoan.LoanOfficer = _loanLOSFields != null ? _loanLOSFields.LoanOfficer : string.Empty;
                    _rptLoan.UnderWritter = _loanLOSFields != null ? _loanLOSFields.Underwriter : string.Empty;
                    _rptLoan.Closer = _loanLOSFields != null ? _loanLOSFields.Closer : string.Empty;
                    _rptLoan.PostCloser = _loanLOSFields != null ? _loanLOSFields.PostCloser : string.Empty;
                    _rptLoan.Investor = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == l.CustomerID).FirstOrDefault().CustomerName;
                    _rptLoan.CriticalRulesCount = _checklistCategoryCount != null ? _checklistCategoryCount.Where(c => c.Key == l.LoanID).Select(c => c.Value).FirstOrDefault() : 0;
                    _rptLoan.CategoryName = string.IsNullOrEmpty(categoryName) ? string.Empty : categoryName;
                    _rptLoan.FailedRulesCount = lsCheckAudit.Count;
                    _rptLoan.LoanAmount = _loanSearch != null ? _loanSearch.LoanAmount : 0;
                    _rptLoan.BorrowerName = _loanSearch != null ? string.IsNullOrEmpty(_loanSearch.BorrowerName) ? string.Empty : _loanSearch.BorrowerName : string.Empty;
                    _rptLoan.AssignUserID = l.AssignedUserID;
                    _rptLoan.LoanAuditCompleteDate = l.AuditCompletedDate != null ? l.AuditCompletedDate : null;
                    _rptLoan.ReceivedDate = l.CreatedOn != null ? l.CreatedOn : null;

                    if (l.CreatedOn != null)
                    {
                        DateTime _createdOn = l.CreatedOn.GetValueOrDefault(DateTime.Now);
                        _rptLoan.NoofDaysAged = Convert.ToInt64((DateTime.Now - _createdOn).TotalDays);
                    }

                    if (l.LoanTypeID != 0)
                        _rptLoan.LoanTypeName = db.LoanTypeMaster.AsNoTracking().Where(lt => lt.LoanTypeID == l.LoanTypeID).FirstOrDefault().LoanTypeName;

                    if (l.LastAccessedUserID != 0)
                    {
                        User _user = db.Users.AsNoTracking().Where(u => u.UserID == l.LastAccessedUserID).FirstOrDefault();

                        List<UserRoleMapping> _userRoles = db.UserRoleMapping.AsNoTracking().Where(u => u.UserID == l.LastAccessedUserID).ToList();

                        _rptLoan.DataEntryName = $"{_user.LastName} {_user.FirstName}";
                        _rptLoan.AuditorName = _userRoles.Count == 1 && _userRoles.FirstOrDefault().RoleID == RoleConstant.QUALITY_CONTROL_AUDITOR ? $"{_user.LastName} {_user.FirstName}" : string.Empty;
                    }

                    if (idcfields != null && !string.IsNullOrEmpty(idcfields.IDCReviewerName) && !string.IsNullOrEmpty(idcfields.IDCLevelOneDuration))
                    {
                        List<string> nameLs = idcfields.IDCReviewerName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                        // List<DateTime> durationLs = l.IDCReviewDuration.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(a => DateTime.ParseExact(a.Trim(), "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
                        var _idcRD = idcfields.IDCLevelOneDuration.Split('|').Where(x => Convert.ToString(x) != "").Select(x => x.Trim()).ToList();
                        List<DateTime> durationLs = _idcRD.Where(x => x != string.Empty).Select(a => DateTime.ParseExact(a, "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
                        if (durationLs.Count > 0)
                        {
                            _rptLoan.MaxIDCReviewDuration = durationLs.Max().ToString("HH:mm:ss");
                            int maxTimeIndex = durationLs.IndexOf(durationLs.Max());

                            if (maxTimeIndex >= nameLs.Count)
                                maxTimeIndex = nameLs.Count - 1;

                            _rptLoan.MaxEphesoftReviewerName = nameLs[maxTimeIndex].Trim();
                        }
                    }

                    if (idcfields != null && !string.IsNullOrEmpty(idcfields.IDCValidatorName) && !string.IsNullOrEmpty(idcfields.IDCLevelTwoDuration))
                    {
                        List<string> nameLs = idcfields.IDCValidatorName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                        // List<DateTime> durationLs = l.IDCValidationDuration.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(a => DateTime.ParseExact(a.Trim(), "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
                        var _idcVDT = idcfields.IDCLevelTwoDuration.Split('|').Where(x => Convert.ToString(x) != "").Select(x => x.Trim()).ToList();
                        List<DateTime> durationLs = _idcVDT.Where(x => x != "").Select(a => DateTime.ParseExact(a, "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();

                        if (durationLs != null && durationLs.Count > 0)
                        {
                            _rptLoan.MaxIDCValidationDuration = durationLs.Max().ToString("HH:mm:ss");
                            int maxTimeIndex = durationLs.IndexOf(durationLs.Max());

                            if (maxTimeIndex >= nameLs.Count)
                                maxTimeIndex = nameLs.Count - 1;

                            _rptLoan.MaxEphesoftValidatorName = nameLs[maxTimeIndex].Trim();
                        }
                    }
                    if (_loanReporting.Count > 0)
                    {

                        foreach (LoanReporting _loanReport in _loanReporting)
                        {
                            ReportConfig _rC = _reportConfig.Find(x => x.ReportID == _loanReport.ReportID);
                            if (_rC != null)
                            {
                                _rptLoan.MissingDocumentNames += _rC.DocumentName + ",<br>";
                            }
                            else
                            {
                                _rptLoan.MissingDocumentNames += string.Empty;
                            }

                        }
                    }
                    else
                    {
                        _rptLoan.MissingDocumentNames = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(_rptLoan.MissingDocumentNames))
                    {
                        int len = _rptLoan.MissingDocumentNames.LastIndexOf(',');
                        _rptLoan.MissingDocumentNames = _rptLoan.MissingDocumentNames.Remove(len, 1);
                    }
                    _report.Add(_rptLoan);
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }
            return _report;
        }

        private List<DBReportResultModel> GetReportModelIDCReport(DBConnect db, List<Loan> _loans, int _ruleStatus = -1, Dictionary<Int64, Int64> _checklistCategoryCount = null, string categoryName = "")
        {
            Logger.WriteTraceLog($"Inside GetReportModelIDCReport()");
            List<DBReportResultModel> _report = new List<DBReportResultModel>();
            //List<ReportConfig> _reportConfig = db.ReportConfig.AsNoTracking().ToList();
            foreach (Loan l in _loans)
            {
                try
                {
                    Logger.WriteTraceLog($"Current LOANID : {l.LoanID}");
                    LoanSearch _loanSearch = db.LoanSearch.AsNoTracking().Where(ls => ls.LoanID == l.LoanID).FirstOrDefault();
                    DBReportResultModel _rptLoan = new DBReportResultModel();
                    // Logger.WriteTraceLog($"After _loanSearch");
                    var idcfields = db.IDCFields.AsNoTracking().Where(x => x.LoanID == l.LoanID).FirstOrDefault();
                    // Logger.WriteTraceLog($"After idcfields");
                    _rptLoan.LoanID = l.LoanID;
                    _rptLoan.CustomerName = db.CustomerMaster.AsNoTracking().Where(c => c.CustomerID == l.CustomerID).FirstOrDefault().CustomerName;
                    // Logger.WriteTraceLog($"After CustomerName {_rptLoan.CustomerName}");
                    _rptLoan.ServiceTypeName = db.ReviewTypeMaster.AsNoTracking().Where(r => r.ReviewTypeID == l.ReviewTypeID).FirstOrDefault().ReviewTypeName;
                    // Logger.WriteTraceLog($"After ReviewTypeMaster {_rptLoan.ServiceTypeName}");
                    _rptLoan.EphesoftBatchInstanceID = idcfields != null ? idcfields.IDCBatchInstanceID : string.Empty;
                    _rptLoan.LoanNumber = string.IsNullOrEmpty(l.LoanNumber) ? string.Empty : l.LoanNumber;
                    if (l.LoanTypeID != 0)
                        _rptLoan.LoanTypeName = db.LoanTypeMaster.AsNoTracking().Where(lt => lt.LoanTypeID == l.LoanTypeID).FirstOrDefault().LoanTypeName;

                    // Logger.WriteTraceLog($"After LoanTypeName {_rptLoan.LoanTypeName}");
                    _rptLoan.ReviewCompletionDate = idcfields != null ? idcfields.IDCLevelOneCompletionDate : null;
                    _rptLoan.ValidationCompletionDate = idcfields != null ? idcfields.IDCLevelTwoCompletionDate : null;
                    _rptLoan.AuditCompletionDate = idcfields != null ? idcfields.IDCCompletionDate : null;
                    _rptLoan.LoanDuration = string.IsNullOrEmpty(l.LoanDuration) ? string.Empty : l.LoanDuration;
                    _rptLoan.EphesoftReviewerName = idcfields != null ? string.IsNullOrEmpty(idcfields.IDCReviewerName) ? string.Empty : idcfields.IDCReviewerName : string.Empty;
                    _rptLoan.EphesoftValidatorName = idcfields != null ? string.IsNullOrEmpty(idcfields.IDCValidatorName) ? string.Empty : idcfields.IDCValidatorName : string.Empty;
                    _rptLoan.IDCReviewDuration = idcfields != null ? idcfields.IDCLevelOneDuration : string.Empty;
                    _rptLoan.IDCValidationDuration = idcfields != null ? idcfields.IDCLevelTwoDuration : string.Empty;
                    if (l.LastAccessedUserID != 0)
                    {

                        User _user = db.Users.AsNoTracking().Where(u => u.UserID == l.LastAccessedUserID).FirstOrDefault();
                        // Logger.WriteTraceLog($"After LastAccessedUserID {_user.UserID}");
                        List<UserRoleMapping> _userRoles = db.UserRoleMapping.AsNoTracking().Where(u => u.UserID == l.LastAccessedUserID).ToList();
                        // Logger.WriteTraceLog($"After _userRoles");
                        _rptLoan.AuditorName = _userRoles.Count == 1 && _userRoles.FirstOrDefault().RoleID == RoleConstant.QUALITY_CONTROL_AUDITOR ? $"{_user.LastName} {_user.FirstName}" : string.Empty;
                    }
                    _rptLoan.PageCount = l.PageCount;
                    _rptLoan.ClassificationAccuracy = idcfields != null ? idcfields.ClassificationAccuracy == null ? "0.00" : idcfields.ClassificationAccuracy.Value.ToString() : "0.00";
                    _rptLoan.OCRAccuracy = idcfields != null ? idcfields.IDCOCRAccuracy == null ? "0.00" : idcfields.IDCOCRAccuracy.ToString() : "0.00";
                    // Logger.WriteTraceLog($"Before _userRoles 
                    if (idcfields != null && !string.IsNullOrEmpty(idcfields.IDCReviewerName) && !string.IsNullOrEmpty(idcfields.IDCLevelOneDuration))
                    {
                        // Logger.WriteTraceLog($"Inside IF IDCReviewerName {idcfields.IDCReviewerName}");
                        List<string> nameLs = idcfields.IDCReviewerName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                        // Logger.WriteTraceLog($"After nameLs {nameLs.Count}");
                        var _idcRD = idcfields.IDCLevelOneDuration.Split('|').Where(x => Convert.ToString(x) != "").Select(x => x.Trim()).ToList();
                        List<DateTime> durationLs = _idcRD.Where(x => x != string.Empty).Select(a => DateTime.ParseExact(a, "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
                        // Logger.WriteTraceLog($"After durationLs {durationLs.Count}");
                        if (durationLs.Count > 0)
                        {
                            _rptLoan.MaxIDCReviewDuration = durationLs.Max().ToString("HH:mm:ss");
                            // Logger.WriteTraceLog($"After MaxIDCReviewDuration {_rptLoan.MaxIDCReviewDuration}");
                            int maxTimeIndex = durationLs.IndexOf(durationLs.Max());

                            if (maxTimeIndex >= nameLs.Count)
                                maxTimeIndex = nameLs.Count - 1;

                            _rptLoan.MaxEphesoftReviewerName = nameLs[maxTimeIndex].Trim();
                            // Logger.WriteTraceLog($"After MaxEphesoftReviewerName {_rptLoan.MaxEphesoftReviewerName}");
                        }
                        // Logger.WriteTraceLog($"Completed IF IDCReviewerName {idcfields.IDCReviewerName}");
                    }

                    if (idcfields != null && !string.IsNullOrEmpty(idcfields.IDCValidatorName) && !string.IsNullOrEmpty(idcfields.IDCLevelTwoDuration))
                    {
                        // Logger.WriteTraceLog($"Completed IF IDCValidatorName {idcfields.IDCValidatorName}");
                        List<string> nameLs = idcfields.IDCValidatorName.Split('|').Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
                        // Logger.WriteTraceLog($"After nameLs {nameLs.Count}");
                        var _idcVDT = idcfields.IDCLevelTwoDuration.Split('|').Where(x => Convert.ToString(x) != "").Select(x => x.Trim()).ToList();
                        List<DateTime> durationLs = _idcVDT.Where(x => x != "").Select(a => DateTime.ParseExact(a, "HH:mm:ss", CultureInfo.InvariantCulture)).ToList();
                        // Logger.WriteTraceLog($"After durationLs {durationLs.Count}");
                        if (durationLs != null && durationLs.Count > 0)
                        {
                            _rptLoan.MaxIDCValidationDuration = durationLs.Max().ToString("HH:mm:ss");
                            // Logger.WriteTraceLog($"After MaxIDCValidationDuration {_rptLoan.MaxIDCValidationDuration}");
                            int maxTimeIndex = durationLs.IndexOf(durationLs.Max());

                            if (maxTimeIndex >= nameLs.Count)
                                maxTimeIndex = nameLs.Count - 1;

                            _rptLoan.MaxEphesoftValidatorName = nameLs[maxTimeIndex].Trim();
                            // Logger.WriteTraceLog($"After MaxEphesoftValidatorName {_rptLoan.MaxEphesoftValidatorName}");
                        }
                        // Logger.WriteTraceLog($"Completed IF IDCValidatorName {idcfields.IDCValidatorName}");
                    }
                    _report.Add(_rptLoan);

                    // Logger.WriteTraceLog($"Loan Added");
                }
                catch (Exception ex)
                {
                    Logger.WriteTraceLog($"Current LOANID Error : {l.LoanID}");
                    MTSExceptionHandler.HandleException(ref ex);
                    throw ex;
                }
            }
            Logger.WriteTraceLog($"Total Report Data : {_report.Count}");
            return _report;
        }


        #endregion

    }

    class GraphObject
    {
        public string name { get; set; }
        public Int64 y { get; set; }
        public Int64 drilldown { get; set; }
        public Int64 id { get; set; }
    }
    class KpiUserConfigList
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserGroupName { get; set; }
        public Int64 AchievedGoalCount { get; set; }
        public Int64 Goal { get; set; }
        public Int64 UserGroupId { get; set; }
        public Int64 RoleID { get; set; }
        public Int64 UserID { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public List<AuditKpiGoalConfig> auditconfigdetails { get; set; }
        public string ConfigType { get; set; }
    }


    class AuditGoalConfigDetails
    {
        // public Int64 UserID { get; set; }
        public Int64 AuditGoalID { get; set; }
        public Int64 UserGroupID { get; set; }
        public Int64 RoleID { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodTo { get; set; }
        public Int64 Goal { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        //  public Int64 AchievedGoalCount { get; set; }
    }
}
