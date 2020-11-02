import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { CustomerModel } from '../../models/customer.model';
import { DashboardService } from '../../service/dashboard.service';
import { Subscription } from 'rxjs';
import { ReviewTypeModel } from '../../models/review-type.model';
import { ServiceTypeConstant, AppSettings } from '@mts-app-setting';
import { HouseLoanModel } from '../../models/house-loan.model';
import * as Highcharts from 'highcharts';
import { ReportStateModel } from '../../models/report-state.model';

@Component({
    selector: 'mts-house-loan-management',
    templateUrl: 'house-loan-management.page.html',
    styleUrls: ['house-loan-management.page.css']
})
export class HouseLoanManagementComponent implements OnInit, AfterViewInit, OnDestroy {

    Highcharts: typeof Highcharts = Highcharts;
    @ViewChild('houseLoanDateRange') houseLoanDateRange;
    houseLoanData: HouseLoanModel = new HouseLoanModel(false);
    houseLoanDateRangevalues: any;
    daterangeoptions: any;
    _topOfHouseDate: boolean;

    staticLabel = AppSettings.AuthorityLabelPlural;

    _topOfHouseAuditMonthYear: any;
    _showtopOfHousePDF = false;

    ActiveCustomers: CustomerModel[] = [];
    customerid = 0;
    ReviewTypes: ReviewTypeModel[] = [];
    topReviewTypeId = ServiceTypeConstant.POST_CLOSING;

    _topOfHouseChartElement: any;
    _topOfHouseOptions = {
        chart: {
            type: 'column',
            inverted: true
        },
        title: {
            text: ''
        },
        colors: ['#F57939', '#055CB2', '#64AB23', '#1FABDE', '#E1ECF6', '#9A9B9D', '#FFAF00', '#CACAC8', '#4D4E53', '#E5E6E5'],
        credits: {
            enabled: false
        },
        exporting: {
            enabled: false
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        lang: {
            noData: 'No Data Available'
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Loan Count'
            }
        },
        plotOptions: {
            column: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '{point.y}'
                },
                events: {
                    click: (e) => {
                        this.onTopOftheHousePointClick(e);
                    }
                },
                showInLegend: true
            }
        },
        legend: {
            enabled: false
        },
        series: [{
            name: 'Loan Count',
            colorByPoint: true,
            data: []
        }],
        drilldown: {
            series: []
        }
    } as any;
    //#region  Constructor
    constructor(private _dashboardService: DashboardService, private _reportState: ReportStateModel) {
    }
    //#endregion Constructor

    //#region  Private Variables
    private subscriptions: Subscription[] = [];
    //#endregion Private Variables

    ngOnInit(): void {
        this.subscriptions.push(this._dashboardService.activeCustomersList.subscribe((res: CustomerModel[]) => {
            this.ActiveCustomers = [...res];
        }));

        this.subscriptions.push(this._dashboardService.reviewTypeList.subscribe((res: ReviewTypeModel[]) => {
            this.ReviewTypes = [...res];
        }));

        this.subscriptions.push(this._dashboardService.houseLoanData.subscribe((res: HouseLoanModel) => {
            this.houseLoanData = res;
        }));

        this.daterangeoptions = {
            theme: 'default',
            previousIsDisable: false,
            nextIsDisable: false,
            range: 'tm',
            dayNames: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            presetNames: ['Today', 'This Month', 'Last Month', 'This Week', 'Last Week', 'This Year', 'Last Year'],
            dateFormat: 'M/d/yyyy',
            outputFormat: 'DD/MM/YYYY',
            startOfWeek: 0,
            display: { to: 'block', tm: 'block', lm: 'block', lw: 'block', tw: 'block', ty: 'none', ly: 'none', custom: 'block', em: 'block' }

        };
    }

    ngAfterViewInit() {
        this._dashboardService.getActiveCustomersList();
        this._dashboardService.getReviewTypeList();
        this._dashboardService.getMonthYear(this.houseLoanDateRange, 'TopOfTheHouse', this._topOfHouseChartElement, this.topReviewTypeId, this.customerid);
    }

    getMonthYear(val, reportType) {
        this._topOfHouseDate = val.OnInit;
        if (typeof this._reportState.TopOfTheHouse.Data !== 'undefined' && (!(this._reportState.TopOfTheHouse.OnLoad) && this._topOfHouseDate)) {

            if (typeof this._topOfHouseChartElement !== 'undefined') {
                this._topOfHouseChartElement.series[0].setData(this._reportState.TopOfTheHouse.Data);
            }
        } else {
            this._topOfHouseAuditMonthYear = val.Value;
            this._dashboardService.getMonthYear(this.houseLoanDateRange, reportType, this._topOfHouseChartElement, this.topReviewTypeId, this.customerid);
        }
    }

    GetTopOftheHouseDocument(onLoad: boolean) {
        this._dashboardService.GetTopOftheHouseDocument(onLoad, this.houseLoanDateRange, this.topReviewTypeId, this.customerid, this._topOfHouseChartElement);
    }

    DownloadTopOftheHouseChart() {

        const localChartObject = this._topOfHouseChartElement;
        this._dashboardService.DownloadTopOftheHouseChart(this._topOfHouseChartElement);
        this._topOfHouseChartElement = localChartObject;
    }

    onTopOftheHousePointClick(e: any) {
        this._dashboardService.onTopOftheHousePointClick(e, this.customerid, this.topReviewTypeId, this.houseLoanDateRange);
    }

    saveInstance(chartInstances) {
        const onLoad = this._topOfHouseDate;
        this._topOfHouseChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'TopOfTheHouse', onLoad);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
