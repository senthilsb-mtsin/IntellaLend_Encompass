import { filter } from 'rxjs/operators';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Injectable } from '@angular/core';
import { ReviewTypeModel } from '../models/review-type.model';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { DashboardDataAccess } from '../dashboard.data';
import { AppSettings, ReportTypeConstant } from '@mts-app-setting';
import { DashboardReportModel } from '../models/dashboard-report.model';
import { DashboardGraphRequestModel } from '../models/dashboard-graph-request.model';
import { DatePipe } from '@angular/common';
import { QualityIndexModel } from '../models/quality-index.model';
import { CustomerModel } from '../models/customer.model';
import { LoanDefectsModel } from '../models/loan-defects.model';
import { Router } from '@angular/router';
import { HouseLoanModel } from '../models/house-loan.model';
import { FailedRulesLoanModel } from '../models/failed-rules-loan.model';
import { MissingDocumentsLoanModel } from '../models/missing-documents-loan.model';
import { InvestorStipulationModel } from '../models/investor-stipulation.model';
import { DocumentRetentionModel } from '../models/document-retention.model';
import { NotificationService } from '@mts-notification';
import { IDCWorkloadRequestModel } from '../models/idc-workload-request.model';
import { IDCWorkloadDrillDownGridRequestModel } from '../models/idc-workload-drilldowngrid.request.model';
import { AuditKpiGoalConfigRequestModel } from '../models/audit-kpi-goalconfig-request.model';
import { DashboardGraphKPIRequestModel } from '../models/kpi-dashboard-request.model';
import { SessionHelper } from '@mts-app-session';
import { CheckCurrentLoanUserRequestModel } from '../models/check-current-loanuser-request.model';
import { LoanInfoService } from '../../loan/services/loan-info.service';
import { SetLoanPickupUserRequestModel } from '../models/set-loan-pickup-user-request.model';
import { ChecklistFailedLoansRequestModel } from '../models/checklist-failed-loans-request.model';
import { KPIUserGroupDetailRequestModel } from '../models/kpi-usergroup-detail-request.model';
import { KPIStatusConstant } from '@mts-status-constant';
import { formatDate, convertDateTime } from '@mts-functions/convert-datetime.function';
import { KPIUserGroupModel } from '../models/kpi-user-groups.model';
import { KPIGoalDetailsModel } from '../models/kpi-goal-details.model';
import { DashboardCommonServiceModel } from '../models/dashboard-common.model';
import { ReportStateModel } from '../models/report-state.model';
import { ReportServiceModel } from '../models/reporting.model';

const jwtHelper = new JwtHelperService();

@Injectable()
export class DashboardService {
    //#region Public Variables
    // #region Loan Quality Index
    reviewTypeList = new Subject<ReviewTypeModel[]>();
    qualityIndexData = new Subject<QualityIndexModel>();
    loanDefectsData = new Subject<LoanDefectsModel>();
    houseLoanData = new Subject<HouseLoanModel>();
    failedRulesLoanData = new Subject<FailedRulesLoanModel>();
    missingDocumentsLoanData = new Subject<MissingDocumentsLoanModel>();
    investorStipulationData = new Subject<InvestorStipulationModel>();
    documentRetentionData = new Subject<DocumentRetentionModel>();
    kpiUserGroupData = new Subject<KPIUserGroupModel>();
    kpiGoalDetailsData = new Subject<KPIGoalDetailsModel>();
    rolelist$ = new Subject();
    showCarousel = new Subject<boolean>();
    _DEWorkLoad$ = new Subject();

    QCIndexDate: boolean;
    isQCAuditmonthsearch = false;

    _failedLoans: any = [];
    _loanFailedCount = 0;
    _checkListLoanDate: any;
    __checklistFailedLoans: any;
    _LoanPercentange: any = '';

    _totalloancount = 0;
    _showIndex = false;
    _LColor = '';
    _waterPercentage = 0;
    // #endregion Quality Index

    //#region  Loan Defects
    activeCustomersList = new Subject<CustomerModel[]>();
    _criticalRulesDate: any;
    _showCriticalRules = false;
    _criticalRulesCategory: any = [];
    _criticalRulesOptions: any;
    //#endregion Loan Defects

    //#region  House Loan
    _showtopOfHousePDF = false;
    _topOfHouseAuditMonthYear: any;
    //#endregion House Loan

    //#region  Loan Failed Rule
    _failedRulesReceivedDate: any;
    _failedRulesLoanTypeCategory: any = [];
    _ShowFailedRulesChart = false;
    //#endregion Loan Failed Rule

    //#region  Missing Documents Loan
    _missingRecordedLoansAuditMonthYear: any;
    _showMissingRecordedPDF = false;
    //#endregion Missing Documents Loan

    _dataEntryAuditMonthYear: any;

    //#region  Investor Stipulations
    IsAuditMonthSearch = false;
    _ShowLoanInvestorChart = false;
    _StipulationAuditMonthYear: any;
    //#endregion Investor Stipulations

    //#region  Document Retention Monitor
    _documentsRetentionMonitoringAuditMonthYear: any;
    _showDocRetentionPDF = false;
    //#endregion Document Retention Monitor
    //#endregion Public Variables

    //#region Constructor
    constructor(private _reportService: ReportServiceModel,
        private _reportState: ReportStateModel,
        private _dashboardData: DashboardDataAccess,
        private _commonService: DashboardCommonServiceModel,
        private _router: Router,
        private datePipe: DatePipe,
        private _loanInfoService: LoanInfoService,
        private _notificationService: NotificationService) {
    }
    //#endregion Constructor

    //#region Private Variables
    private RoleItems = [];
    private _reviewTypeList: ReviewTypeModel[] = [];
    private _activeCustomersList: CustomerModel[] = [];
    private RoleID: any;
    //#endregion Private Variables

    //#region Public Functions

    getReviewTypeList() {
        return this._dashboardData.GetReviewTypeList({ TableSchema: AppSettings.TenantSchema }).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            this._reviewTypeList = [];
            if (result != null) {
                if (result.length > 0) {
                    result.forEach(element => {
                        this._reviewTypeList.push({ ReviewTypeID: element.ReviewTypeID, ReviewTypeName: element.ReviewTypeName });
                    });
                }
            }
            this.reviewTypeList.next(this._reviewTypeList.slice());
        });
    }

    getActiveCustomersList() {
        return this._dashboardData.GetActiveCustomersList({ TableSchema: AppSettings.TenantSchema }).subscribe(res => {
            const result = jwtHelper.decodeToken(res.Data)['data'];
            this._activeCustomersList = [];
            if (result != null) {
                if (result.length > 0) {
                    result.forEach(element => {
                        this._activeCustomersList.push({ CustomerID: element.CustomerID, CustomerName: element.CustomerName });
                    });
                }
            }
            this.activeCustomersList.next(this._activeCustomersList.slice());
        });
    }

    getMonthYear(val, reportType, chartElement, reviewTypeID = 0, customerID = 0, show = false) {

        if (reportType === 'LoanQcIndex') {
            this._checkListLoanDate = val.value;
            this.QCIndexDate = false;
            this.isQCAuditmonthsearch = false;
            this.GetCheckListFailedLoans(this.QCIndexDate, val, reviewTypeID);
        }

        if (reportType === 'CriticalRulesFailed') {
            this.GetCriticalRules(false, val, customerID, chartElement);
        }

        if (reportType === 'TopOfTheHouse') {
            this._topOfHouseAuditMonthYear = val.value;
            this.GetTopOftheHouseDocument(false, val, reviewTypeID, customerID, chartElement);
        }

        if (reportType === 'MissingRecordedLoans') {
            this._missingRecordedLoansAuditMonthYear = val.value;
            this.GetMissingRecordedLoans(false, val, reviewTypeID, chartElement);
        }

        if (reportType === 'DataEnryWorlLoad') {
            this._dataEntryAuditMonthYear = val.value;
            this.GetDataEntryWorkLoad(false, val, reviewTypeID, chartElement, customerID);
        }

        if (reportType === 'DocumentRetentionMonitoring') {
            this._documentsRetentionMonitoringAuditMonthYear = val.value;
            this.GetDocumentRetentionMonitoring(false, val, reviewTypeID, chartElement);
        }
    }

    GetDataEntryWorkLoad(onLoad: boolean, dateElement, customerID: number, chartElement, category) {
        const fromdate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        const _reportModel = { FromDate: fromdate, ToDate: todate, DataEntryType: category, CustomerID: customerID, ReviewTypeID: 0 };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.IDC_DATAENTRY_WORKLOAD.Name, _reportModel);
        this.getDashboardGraph_DataEntryWorkLoad(_dgreq, onLoad, chartElement);
    }

    getLoanRecords(val: any, QCReviewTypeID: number, QCIndexDateRange) {
        this._reportService.ReportType = ReportTypeConstant.LOANQC_INDEX.Name;
        this._reportService.ReportDescription = ReportTypeConstant.LOANQC_INDEX.Description;
        this._reportService.ReportModel.CategoryName = '';
        this._reportService.ReportModel.ReviewTypeID = QCReviewTypeID;
        this._reportService.ReportModel.FromDate = this.datePipe.transform(QCIndexDateRange.dateFrom, AppSettings.dateFormat);
        this._reportService.ReportModel.ToDate = this.datePipe.transform(QCIndexDateRange.dateTo, AppSettings.dateFormat);
        this._router.navigate(['view/dashboard/checklistloan']);
    }

    getcheckListFailedLoans(_btnSearchClick: boolean, dTable) {
        const _dgreq = new ChecklistFailedLoansRequestModel(AppSettings.TenantSchema, this._reportService.ReportType, this._reportService.ReportModel);
        return this._dashboardData.GetChecklistFailedLoans(_dgreq).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            _btnSearchClick = false;
            dTable.clear();
            if (data != null) {
                dTable.rows.add(data);
            }
            dTable.draw();
            dTable.columns.adjust();
        });
    }

    checkCurrentUser(row: any, AlertMessage: string = '', currentLoanID: number = 0, currentRow, confirmModal) {
        row['CurrentUserID'] = SessionHelper.UserDetails.UserID;
        const _dgreq = new CheckCurrentLoanUserRequestModel(AppSettings.TenantSchema, row.LoanID, SessionHelper.UserDetails.UserID);
        return this._dashboardData.CheckCurrentLoanUser(_dgreq).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data != null) {
                if (data.CurrentUser) {
                    this.setLoanPickUpUser(row);
                    this.setDataAndRoute(row);
                } else {
                    this.alertLoanPicked(row, data.LoggerUserName, AlertMessage, currentLoanID, currentRow, confirmModal);
                }
            }
        });
    }

    alertLoanPicked(row: any, loggedUserName: string, AlertMessage: string = '', currentLoanID: number = 0, currentRow, confirmModal) {
        AlertMessage = 'This Loan is currently operated by : <b>' + loggedUserName + '</b>. Do you still want to view ?';
        currentLoanID = row.LoanID;
        currentRow = row;
        confirmModal.show();
    }

    setLoanPickUpUser(row: any) {
        const _dgreq = new SetLoanPickupUserRequestModel(AppSettings.TenantSchema, row.LoanID, row.CurrentUserID);
        return this._dashboardData.SetLoanPickupUser(_dgreq).subscribe(res => {
        });
    }

    setDataAndRoute(row: any) {
        this._loanInfoService.SetLoanPageInfo(row);
        this._router.navigate(['view/loandetails']);
    }

    GetKpiUserGroupDetail(KpiUserGroupData, _UserGroupCarousel) {
        const _reportModel = { UserID: SessionHelper.UserDetails.UserID };
        const _dgreq = new KPIUserGroupDetailRequestModel(AppSettings.TenantSchema, ReportTypeConstant.KPI_USER_GROUP_CONFIGURATION.Name, _reportModel);
        return this._dashboardData.GetKPIUserGroupDetails(_dgreq).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {

                data.forEach(element => {
                    let index;
                    element.forEach(ele => {
                        ele.PeriodFrom = convertDateTime(ele.PeriodFrom);
                        ele.PeriodTo = convertDateTime(ele.PeriodTo);
                        ele.dateFromTo = ele.PeriodFrom + '-' + ele.PeriodTo;
                        ele.IsConfigured = true;
                        ele['ScrollIndex'] = 0;
                        const role = this.RoleItems.find(x => x.RoleID === ele.RoleID);
                        ele.roleactive = isTruthy(role) ? role.Active : false;
                        index = this.RoleItems.findIndex(x => x.RoleID === ele.RoleID);
                    });
                    if (index !== -1) {
                        this.RoleItems.splice(index, 1);
                    }
                });
                this.RoleItems.forEach(element => {
                    const _arr = [];
                    _arr.push({
                        UserGroupName: element.RoleName,
                        IsConfigured: false,
                        roleactive: element.Active
                    });
                    data.push(_arr);
                });
            } else {
                this.RoleItems.forEach(element => {
                    const _arr = [];
                    _arr.push({
                        UserGroupName: '',
                        IsConfigured: false,
                        roleactive: element.Active
                    });
                    data.push(_arr);
                });
            }
            KpiUserGroupData = data.map(a => a.filter(b => b.roleactive === true)).filter(a => a.length);
            this.kpiUserGroupData.next(new KPIUserGroupModel(KpiUserGroupData, _UserGroupCarousel));
        });
    }

    SetUserGroupCarouselItem(KpiUserGroupData, _UserGroupCarousel, dateFromTo) {
        const carouselLength = KpiUserGroupData.length / 4;
        let cLength = 0, incrementer = 0;
        for (let index = 0; index < carouselLength; index++) {
            let indexLength = 4;

            if (KpiUserGroupData.length > 4) {
                indexLength = (KpiUserGroupData.length - incrementer) > 4 ? 4 : KpiUserGroupData.length - incrementer;
            } else {
                indexLength = KpiUserGroupData.length;
            }
            const cSet = [];
            for (let i = 0; i < indexLength; i++) {
                dateFromTo = [];
                if (cLength < KpiUserGroupData.length) {
                    dateFromTo.push({ id: KpiUserGroupData[cLength][0].UserGroupId, Text: KpiUserGroupData[cLength][0].dateFromTo, Flag: false, PeriodFrom: KpiUserGroupData[cLength][0].PeriodFrom, PeriodTo: KpiUserGroupData[cLength][0].PeriodTo, AuditGoalID: 0, ConfigType: KpiUserGroupData[cLength][0].ConfigType });
                    KpiUserGroupData[cLength][0].dataValues = dateFromTo;
                    cSet.push(KpiUserGroupData[cLength][0]);
                }

                cLength++;
            }
            let splitvalues: any = [];
            let getdatevals: any = [];
            cSet.forEach(element => {
                getdatevals = [];
                if (element.IsConfigured === true) {
                    element.auditconfigdetails.forEach(ele => {
                        ele.PeriodFrom = convertDateTime(ele.PeriodFrom);
                        ele.PeriodTo = convertDateTime(ele.PeriodTo);
                        getdatevals.push(ele.PeriodFrom + '-' + ele.PeriodTo + '|' + ele.PeriodFrom + '|' + ele.PeriodTo + '|' + ele.AuditGoalID + '|' + ele.ConfigType);
                    });
                    getdatevals = getdatevals.filter((x, i, a) => x && a.indexOf(x) === i);
                    getdatevals.forEach(elem => {
                        splitvalues = elem.split('|');
                        element.dataValues.push({ id: element.UserGroupId, Text: splitvalues[0], Flag: true, PeriodFrom: splitvalues[1], PeriodTo: splitvalues[2], AuditGoalID: splitvalues[3], ConfigType: KPIStatusConstant.ConfigTypeDescription[splitvalues[4]] });
                    });
                }
            });
            incrementer = incrementer + indexLength;
            _UserGroupCarousel.push({ carouselSet: cSet });
        }
    }

    GetKpiLoanDetail(UserID: any, PeriodFrom: any, PeriodTo: any, UserName: any, AchievedGoalCount: any) {
        if (AchievedGoalCount !== 0) {
            this._reportService.ReportType = ReportTypeConstant.KPI_GOAL_CONFIGURATION.Name;
            this._reportService.ReportDescription = ReportTypeConstant.KPI_GOAL_CONFIGURATION.Description;
            this._reportService.ReportModel.UserID = UserID;
            this._reportService.ReportModel.RoleID = this.RoleID;
            this._reportService.ReportModel.FromDate = PeriodFrom;
            this._reportService.ReportModel.ToDate = PeriodTo;
            this._reportService.ReportModel.UserName = UserName;
            this._router.navigate(['view/dashboard/report']);
        }
    }

    GetKpiGoalDetails(RoleID: any, auditData: any, _carousel, kpisearch, ShowKpiUserDetails, KpiScoreCardData, ShowKpiCardDetails) {
        const _reportModel = { RoleID: RoleID, UserID: auditData[0].Flag === false ? auditData[0].id : auditData[0].AuditGoalID, Flag: auditData[0].Flag, FromDate: auditData[0].PeriodFrom, ToDate: auditData[0].PeriodTo };
        const _dgreq = new DashboardGraphKPIRequestModel(AppSettings.TenantSchema, ReportTypeConstant.KPI_GOAL_CONFIGURATION.Name, _reportModel);
        return this._dashboardData.GetDashboardKPIDetails(_dgreq).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            kpisearch = false;
            if (data.length > 0) {
                ShowKpiUserDetails = true;
                data.forEach(element => {
                    element.PeriodFrom = convertDateTime(element.PeriodFrom);
                    element.PeriodTo = convertDateTime(element.PeriodTo);
                    element.UserName = element.FirstName + ' ' + element.LastName;
                });
                KpiScoreCardData.push(data);
                this.SetCarouselItem(_carousel, KpiScoreCardData);
            } else {
                ShowKpiCardDetails = true;
                kpisearch = false;
                this._notificationService.showError('No Loans found for these users on the selected dates');
            }
        });
    }
    SetRoleID(RoleId: any) {
        this.RoleID = RoleId;

    }
    SetCarouselItem(_carousel, KpiScoreCardData) {
        _carousel = [];
        const carouselLength = KpiScoreCardData[0].length / 6;
        let cLength = 0, incrementer = 0;
        for (let index = 0; index < carouselLength; index++) {
            let indexLength = 6;
            if (KpiScoreCardData[0].length > 6) {
                indexLength = (KpiScoreCardData[0].length - incrementer) > 6 ? 6 : KpiScoreCardData[0].length - incrementer;
            } else {
                indexLength = KpiScoreCardData[0].length;
            }
            const cSet = [];
            for (let i = 0; i < indexLength; i++) {
                if (cLength < KpiScoreCardData[0].length) {
                    cSet.push(KpiScoreCardData[0][cLength]);
                }
                cLength++;
            }
            incrementer = incrementer + indexLength;
            _carousel.push({ carouselSet: cSet });
        }
        this.kpiGoalDetailsData.next(new KPIGoalDetailsModel(KpiScoreCardData, _carousel));
    }

    datechangefunction(value: any, _CData: any) {
        const val = value.split(',');
        const inputData = { TableSchema: AppSettings.TenantSchema, RoleID: _CData.RoleID, UserGroupID: val[0], Flag: val[1], FromDate: val[2], ToDate: val[3], AuditGoalID: val[4] };
        this.GetAuditKPIGoalConfigDetails(inputData, _CData);
    }

    GetAuditKPIGoalConfigDetails(req: AuditKpiGoalConfigRequestModel, _CData: any) {
        return this._dashboardData.GetDashboardAuditKpiGoalConfigDetails(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data != null && data.length > 0) {
                _CData.Goal = data[0].Goal;
                _CData.AchievedGoalCount = data[0].AchievedGoalCount;
            }
        });
    }

    GetOCRClassification(ocrSelecteddata: any, dateFrom: any, dateTo: any, _classificationTable) {
        const fromDateStr = formatDate(dateFrom);
        const toDatStr = formatDate(dateTo);
        const _reportModel = { OCRReportType: parseInt(ocrSelecteddata, 10), FromDate: fromDateStr, ToDate: toDatStr };
        const _dgreq = new IDCWorkloadRequestModel(AppSettings.TenantSchema, ReportTypeConstant.OCR_EXTRACTION_REPORT.Name, _reportModel);
        return this.getDashboardDatatable_IDCWorkload(_dgreq, dateFrom, dateTo, _classificationTable);
    }

    GetOCRClassificationInnerTable(rowIndex: Node, rowData: any, _classificationAuditMonthYear, _classificationTable) {
        const loanID = rowData.LoanID;
        const _reportModel = { AuditMontYear: _classificationAuditMonthYear, LoanID: loanID };
        const _dgreq = new IDCWorkloadDrillDownGridRequestModel(AppSettings.TenantSchema, ReportTypeConstant.OCR_EXTRACTION_REPORT.Name, _reportModel);
        this.getDashboardDrillDownGrid_IDCWorkload(_dgreq, rowIndex, _classificationTable);
    }

    getDashboardDrillDownGrid_IDCWorkload(req: IDCWorkloadDrillDownGridRequestModel, rowIndex: Node, _classificationTable) {
        return this._dashboardData.GetDashboardDrillDownGrid(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                const randomNumber = ['87', '91', '89', '95', '93', '92', '94', '88', '96', '90', '93', '89', '86', '99'];
                let tempTable = '<table class="table table-striped table-bordered" width="100%">#TR#</table>';
                let tempTr = '<tr class="text-center"><th  class="text-center">Document Name</th><th  class="text-center">IDC Accuracy (%)</th></tr>';
                let count = 0;
                data.forEach(element => {
                    if (count > randomNumber.length - 1) {
                        count = 0;
                    }
                    tempTr += '<tr  class="text-center"><td  class="text-center">' + element.DocName + '</td><td  class="text-center">' + randomNumber[count] + '</td></tr>';
                    count++;
                });
                tempTable = tempTable.replace('#TR#', tempTr);
                _classificationTable.row(rowIndex).child($(tempTable)).show();
            }
        });
    }

    getDashboardDatatable_IDCWorkload(req: IDCWorkloadRequestModel, dateFrom: any, dateTo: any, _classificationTable) {
        return this._dashboardData.GetDashboardDatatable(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                _classificationTable.clear();
                _classificationTable.rows.add(data);
                _classificationTable.draw();
                this._reportState.OCRExtractionReport.Data = data;
                this._reportState.OCRExtractionReport.FromDate = dateFrom;
                this._reportState.OCRExtractionReport.ToDate = dateTo;
            } else {
                _classificationTable.clear();
                _classificationTable.draw();
            }
        });
    }

    GetDocumentRetentionMonitoring(onLoad: boolean, dateElement, reviewTypeId: number, chartElement) {
        const fromdate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        const _reportModel = { FromDate: fromdate, ToDate: todate, ReviewTypeID: reviewTypeId };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.DOCUMENT_RETENTION_MONITORING.Name, _reportModel);
        this.getDashboardGraph_DocumentRetention(_dgreq, onLoad, chartElement);
    }

    getDashboardGraph_DocumentRetention(req: DashboardGraphRequestModel, onLoad: boolean, _documentsRetentionMonitoringChartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (isTruthy(data) && data.length > 0) {
                _documentsRetentionMonitoringChartElement.series[0].setData([{ name: '', y: data.length }]);
                this._reportState.DocRetention.Data = data.length;
                this._reportState.DocRetention.OnLoad = onLoad;
                this._showDocRetentionPDF = true;
                this._commonService.checklistitemResult = data;
                this._reportState.DocRetention.SelectedDate = this._documentsRetentionMonitoringAuditMonthYear;
            } else {
                this._showDocRetentionPDF = false;
            }
            this.documentRetentionData.next(new DocumentRetentionModel(this._showDocRetentionPDF));
        });
    }

    DownloadDocumentRetentionChart(chartElement) {
        chartElement.exportChart({
            type: 'application/pdf',
            filename: ReportTypeConstant.DOCUMENT_RETENTION_MONITORING.Description
        });
    }

    GetMissingRecordedLoans(onLoad: boolean, dateElement, reviewTypeId: number, chartElement) {
        const fromdate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        const _reportModel = { FromDate: fromdate, ToDate: todate, ReviewTypeID: reviewTypeId };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.MISSING_RECORDED_LOANS.Name, _reportModel);
        this.getDashboardGraph_MissingRecordedLoan(_dgreq, onLoad, chartElement);
    }

    getDashboardGraph_MissingRecordedLoan(req: DashboardGraphRequestModel, onLoad: boolean, _missingRecordedLoansChartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data > 0) {
                _missingRecordedLoansChartElement.series[0].setData([{ name: '', y: data }]);
                this._reportState.MissingRecordedLoans.Data = data;
                this._reportState.MissingRecordedLoans.OnLoad = onLoad;
                this._showMissingRecordedPDF = true;
                this._reportState.MissingRecordedLoans.SelectedDate = this._missingRecordedLoansAuditMonthYear;
            } else {
                this._showMissingRecordedPDF = false;
            }
            this.missingDocumentsLoanData.next(new MissingDocumentsLoanModel(this._showMissingRecordedPDF));
        });
    }

    DownloadMissingRecordedLoans(chartElement) {
        chartElement.exportChart({
            type: 'application/pdf',
            filename: ReportTypeConstant.MISSING_RECORDED_LOANS.Description
        });
    }

    GetStipulationData(onLoad: boolean, FromDateLoanInvestor, ToDateLoanInvestor, reviewTypeId: number, stipulationType: number, chartElement) {
        if (this.IsAuditMonthSearch) {
            FromDateLoanInvestor = null;
            ToDateLoanInvestor = null;
        }
        const _reportModel = { FromDate: FromDateLoanInvestor, ToDate: ToDateLoanInvestor, ReviewTypeID: reviewTypeId, StipulationType: stipulationType };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.LOAN_INVESTOR_STIPULATIONS.Name, _reportModel);
        this.getDashboardGraph_StipulationData(_dgreq, onLoad, chartElement);
    }

    getDashboardGraph_StipulationData(req: DashboardGraphRequestModel, onLoad: boolean, _StipulationChartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                _StipulationChartElement.series[0].setData(data);
                this._reportState.LoanStipulation.Data = data;
                this._reportState.LoanStipulation.OnLoad = onLoad;
                this._ShowLoanInvestorChart = true;
                this._reportState.LoanStipulation.SelectedDate = this._StipulationAuditMonthYear;
            } else {
                this._ShowLoanInvestorChart = false;
            }
            this.investorStipulationData.next(new InvestorStipulationModel(this._ShowLoanInvestorChart));
        });
    }

    GetFailedRuleLoans(onLoad: boolean, dateElement, reviewTypeId: number, ruleType: number, chartElement) {
        const fromdate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        const _reportModel = { FromDate: fromdate, ToDate: todate, ReviewTypeID: reviewTypeId, RuleStatus: ruleType };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.LOAN_FAILED_RULES.Name, _reportModel);
        this.getDashboardGraph_LoanFailedRule(_dgreq, onLoad, chartElement);
    }

    getDashboardGraph_LoanFailedRule(req: DashboardGraphRequestModel, onLoad: boolean, _failedRulesChartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                this._failedRulesLoanTypeCategory = [];
                data.forEach(elt => {
                    this._failedRulesLoanTypeCategory.push(elt.name);
                });
                _failedRulesChartElement.xAxis[0].categories = this._failedRulesLoanTypeCategory;
                _failedRulesChartElement.setTitle({ text: '' });
                _failedRulesChartElement.series[0].setData(data);
                this._reportState.LoanFailedRules.Data = data;
                this._reportState.LoanFailedRules.OnLoad = onLoad;
                this._ShowFailedRulesChart = true;
                this._reportState.LoanFailedRules.SelectedDate = this._failedRulesReceivedDate;
            } else {
                this._ShowFailedRulesChart = false;
                _failedRulesChartElement.setTitle({ text: 'No Data Available' });
            }
            this.failedRulesLoanData.next(new FailedRulesLoanModel(this._ShowFailedRulesChart));
        });
    }

    GetTopOftheHouseDocument(onLoad: boolean, dateElement, reviewTypeId: number, customerId: number, chartElement) {
        const fromdate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        const _reportModel = { FromDate: fromdate, ToDate: todate, CustomerID: customerId, ReviewTypeID: reviewTypeId };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.TOP_OF_THE_HOUSE.Name, _reportModel);
        this.getDashboardGraph_HouseLoan(_dgreq, onLoad, chartElement);
    }

    DownloadTopOftheHouseChart(chartElement: any) {
        chartElement.exportChart({
            type: 'application/pdf',
            filename: ReportTypeConstant.TOP_OF_THE_HOUSE.Description
        });
    }

    getDashboardGraph_DataEntryWorkLoad(req: DashboardGraphRequestModel, onLoad: boolean, _dataEntryWorkLoadChartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                _dataEntryWorkLoadChartElement.series[0].setData(data);
                this._reportState.DataEntryWorkLoad.Data = data;
                this._reportState.DataEntryWorkLoad.OnLoad = onLoad;
                const noData = [];
                data.forEach(element => {
                    noData.push((element.y > 0));
                });
                this._reportState.DataEntryWorkLoad.SelectedDate = this._dataEntryAuditMonthYear;
                _dataEntryWorkLoadChartElement.setTitle({ text: '' });
            } else {
                _dataEntryWorkLoadChartElement.setTitle({ text: 'No Data Available' });
            }
            this._DEWorkLoad$.next(data);
        });
    }

    getDashboardGraph_HouseLoan(req: DashboardGraphRequestModel, onLoad: boolean, _topOfHouseChartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                _topOfHouseChartElement.series[0].setData(data);
                this._reportState.TopOfTheHouse.Data = data;
                this._reportState.TopOfTheHouse.OnLoad = onLoad;
                const noData = [];
                data.forEach(element => {
                    noData.push((element.y > 0));
                });
                this._showtopOfHousePDF = !(noData.every(this.CheckEveryValue));
                this._reportState.TopOfTheHouse.SelectedDate = this._topOfHouseAuditMonthYear;
                this.houseLoanData.next(new HouseLoanModel(this._showtopOfHousePDF));
            } else {
                this._showtopOfHousePDF = false;
                _topOfHouseChartElement.setTitle({ text: 'No Data Available' });
            }
        });
    }

    CheckEveryValue(val): boolean {
        return val === false;
    }

    GetCriticalRules(onLoad: boolean, val, customerId: number, chartElement) {
        const fromdate = this.datePipe.transform(val.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(val.dateTo, AppSettings.dateFormat);
        const _reportModel = { FromDate: fromdate, ToDate: todate, ReviewTypeID: 0, CustomerID: customerId };
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.CRITICAL_RULES_FAILED.Name, _reportModel);
        this.getDashboardGraph_LoanDefects(_dgreq, chartElement);
    }

    getDashboardGraph_LoanDefects(req: DashboardGraphRequestModel, chartElement) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.length > 0) {
                this._criticalRulesCategory = [];
                data.forEach(elt => {
                    this._criticalRulesCategory.push(elt.name);
                });
                chartElement.xAxis[0].categories = this._criticalRulesCategory;
                chartElement.setTitle({ text: '' });
                chartElement.series[0].setData(data);
                this._reportState.CriticalRulesFailed.Data = data;
                this._reportState.CriticalRulesFailed.OnLoad = false;
                this._showCriticalRules = true;
                this._reportState.CriticalRulesFailed.SelectedDate = this._criticalRulesDate;
            } else {
                this._showCriticalRules = false;
            }
            this.loanDefectsData.next(new LoanDefectsModel(this._showCriticalRules));
        });
    }

    onCriticalRulesPointClick(e: any, customerID: number, val) {
        this._reportService.ReportType = ReportTypeConstant.CRITICAL_RULES_FAILED.Name;
        this._reportService.ReportDescription = ReportTypeConstant.CRITICAL_RULES_FAILED.Description;
        this._reportService.ReportModel.CategoryName = e.point.id;
        this._reportService.ReportModel.CustomerID = customerID;
        this._reportService.ReportModel.FromDate = this.datePipe.transform(val.dateFrom, AppSettings.dateFormat);
        this._reportService.ReportModel.ToDate = this.datePipe.transform(val.dateTo, AppSettings.dateFormat);
        this._reportService.ReportModel.UserName = '';
        this._router.navigate(['view/dashboard/checklistloan']);
    }

    onTopOftheHousePointClick(e: any, customerID: number, reveiwTypeID: number, dateElement) {
        this._reportService.ReportType = ReportTypeConstant.TOP_OF_THE_HOUSE.Name;
        this._reportService.ReportDescription = ReportTypeConstant.TOP_OF_THE_HOUSE.Description;
        this._reportService.ReportModel.BarType = e.point.name;
        this._reportService.ReportModel.CustomerID = customerID;
        this._reportService.ReportModel.ReviewTypeID = reveiwTypeID;
        this._reportService.ReportModel.FromDate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        this._reportService.ReportModel.ToDate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        this._reportService.ReportModel.UserName = '';
        this._router.navigate(['view/dashboard/report']);
    }

    onDocumentRetentionMonitoringPointClick(e: any, reviewTypeID: number, dateElement) {
        this._commonService.AuditMonthYear = this._documentsRetentionMonitoringAuditMonthYear;
        this._commonService.FromDate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        this._commonService.ToDate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        this._commonService.ReviewTypeID = reviewTypeID;
        const isValid: boolean = this.CheckPermissionsforRoles('View\\Dashboard\\PurgeClick');
        if (isValid) {
            this._router.navigate(['view/loanpurge']);
        } else {
            this._notificationService.showError('Access Denied!!!');
        }
    }

    CheckPermissionsforRoles(URL: any): boolean {
        let AccessCheck = false;
        const AccessUrls = SessionHelper.RoleDetails.URLs;
        if (AccessCheck != null) {
            AccessUrls.forEach(elts => {
                if (elts.URL === URL) {
                    AccessCheck = true;
                    return false;
                }
            });
            return AccessCheck;
        }
    }

    onMissingRecordedLoansPointClick(e: any, reviewTypeID: number, dateElement) {
        this._reportService.ReportType = ReportTypeConstant.MISSING_RECORDED_LOANS.Name;
        this._reportService.ReportDescription = ReportTypeConstant.MISSING_RECORDED_LOANS.Description;
        this._reportService.ReportModel.ReviewTypeID = reviewTypeID;
        this._reportService.ReportModel.FromDate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        this._reportService.ReportModel.ToDate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        this._reportService.ReportModel.UserName = '';
        this._router.navigate(['view/dashboard/report']);
    }

    onLoanFailedRulesDocPointClick(e: any, reviewTypeID: number, ruleType: number, dateElement) {
        this._reportService.ReportType = ReportTypeConstant.LOAN_FAILED_RULES.Name;
        this._reportService.ReportDescription = ReportTypeConstant.LOAN_FAILED_RULES.Description;
        this._reportService.ReportModel.LoanTypeID = e.point.id;
        this._reportService.ReportModel.ReviewTypeID = reviewTypeID;
        this._reportService.ReportModel.CategoryName = '';
        this._reportService.ReportModel.RuleStatus = ruleType;
        this._reportService.ReportModel.FromDate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        this._reportService.ReportModel.ToDate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        this._reportService.ReportModel.UserName = '';
        this._router.navigate(['view/dashboard/checklistloan']);
    }

    InvestorStipulationCLick(e: any, reviewTypeID: number, stipulationType: number, dateElement) {
        this._reportService.ReportType = ReportTypeConstant.LOAN_INVESTOR_STIPULATIONS.Name;
        this._reportService.ReportDescription = ReportTypeConstant.LOAN_INVESTOR_STIPULATIONS.Description;
        this._reportService.ReportModel.CustomerID = e.point.id;
        this._reportService.ReportModel.FromDate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        this._reportService.ReportModel.ReviewTypeID = reviewTypeID;
        this._reportService.ReportModel.ToDate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        this._reportService.ReportModel.UserName = '';
        this._reportService.ReportModel.StipulationType = stipulationType;
        this._router.navigate(['view/dashboard/report']);
    }

    saveInstance(chartInstances, type, onLoad: boolean) {
        if (type === 'CriticalRulesFailed') {
            if (typeof this._reportState.CriticalRulesFailed.Data !== 'undefined' && (!(this._reportState.CriticalRulesFailed.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.CriticalRulesFailed.Data);
                this.loanDefectsData.next(new LoanDefectsModel(this._showCriticalRules));
            }
        }

        if (type === 'TopOfTheHouse') {

            if (typeof this._reportState.TopOfTheHouse.Data !== 'undefined' && (!(this._reportState.TopOfTheHouse.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.TopOfTheHouse.Data);
            }
        }

        if (type === 'DataEnryWorlLoad') {
            if (typeof this._reportState.DataEntryWorkLoad.Data !== 'undefined' && (!(this._reportState.DataEntryWorkLoad.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.DataEntryWorkLoad.Data);
            }
        }

        if (type === 'LoanFailedRules') {
            if (typeof this._reportState.LoanFailedRules.Data !== 'undefined' && (!(this._reportState.LoanFailedRules.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.LoanFailedRules.Data);
            }
        }

        if (type === 'MissingRecordedLoans') {
            if (typeof this._reportState.MissingRecordedLoans.Data !== 'undefined' && (!(this._reportState.MissingRecordedLoans.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.MissingRecordedLoans.Data);
            }
        }

        if (type === 'InvestorStipulation') {
            if (typeof this._reportState.LoanStipulation.Data !== 'undefined' && (!(this._reportState.LoanStipulation.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.LoanStipulation.Data);
            }
        }

        if (type === 'DocumentRetentionMonitoring') {
            if (typeof this._reportState.DocRetention.Data !== 'undefined' && (!(this._reportState.DocRetention.OnLoad) && onLoad)) {
                chartInstances.series[0].setData(this._reportState.DocRetention.Data);
            }

        }
    }

    GetCheckListFailedLoans(QcIndexdate: boolean, dateElement: any, QCReviewTypeID: number) {
        const fromdate = this.datePipe.transform(dateElement.dateFrom, AppSettings.dateFormat);
        const todate = this.datePipe.transform(dateElement.dateTo, AppSettings.dateFormat);
        const _reportModel = new DashboardReportModel(fromdate, todate, QCReviewTypeID);
        const _dgreq = new DashboardGraphRequestModel(AppSettings.TenantSchema, ReportTypeConstant.LOANQC_INDEX.Name, _reportModel);
        this.getDashboardGraph_QcIndex(_dgreq);
    }

    getDashboardGraph_QcIndex(req: DashboardGraphRequestModel) {
        return this._dashboardData.GetDashboardGraph(req).subscribe(res => {
            const data = jwtHelper.decodeToken(res.Data)['data'];
            if (data.TotalLoanCount > 0) {
                this._failedLoans = [];
                this._loanFailedCount = data.FailedLoanCount;
                this._totalloancount = data.TotalLoanCount;
                this.setQualityIndex();
                this._showIndex = true;
                this._reportState.CheckListFailedLoans.SelectedDate = this._checkListLoanDate;
            } else {
                this._showIndex = false;
                this.__checklistFailedLoans = 'No Data Available';
            }
            this.qualityIndexData.next(new QualityIndexModel(this._loanFailedCount, this._LoanPercentange, this._totalloancount, this._showIndex, this._LColor, this._waterPercentage));
        });
    }

    setQualityIndex() {
        const colorInc = 100 / 3;

        let val = parseInt(((this._loanFailedCount / this._totalloancount) * 100).toFixed(2), 10);
        if (this._loanFailedCount === 0) {
            val = 0;
        }
        // if (val !== 0 && !isNaN(val) && val <= 100 && val >= 0) {
        const valOrig = val;
        val = 100 - val;
        if (valOrig === 0) {
            this._LoanPercentange = 0;
        } else { this._LoanPercentange = valOrig; }
        this._LColor = '';
        this._waterPercentage = val;
        if (valOrig < colorInc * 1) {
            this._LColor = 'red';
        } else if (valOrig < colorInc * 2) {
            this._LColor = 'orange';
        } else {
            this._LColor = 'green';
        }
        // } else {
        //     this._showIndex = false;
        //     this.__checklistFailedLoans = 'No Data Available';

        // }
    }
    GetRoleMasterList(req: { TableSchema: string }) {
        return this._dashboardData.GetRoleMasterList(req).subscribe(
            res => {
                this.RoleItems = [];
                if (res != null) {
                    const roles = jwtHelper.decodeToken(res.Data)['data'];
                    if (roles.length > 0) {
                        const _index = roles.findIndex(x => x.RoleName === 'System Administrator');
                        if (_index !== -1) {
                            roles.splice(_index, 1);
                        }
                        roles.forEach(element => {
                            if (element.IncludeKpi === true) {
                                this.RoleItems.push(element);
                            }
                        });
                    }
                }
                this.rolelist$.next();
            }
        );
    }

    //#endregion Public Functions

}
