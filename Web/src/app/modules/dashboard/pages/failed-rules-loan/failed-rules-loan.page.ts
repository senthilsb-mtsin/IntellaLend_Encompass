import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { ServiceTypeConstant } from '@mts-app-setting';
import { ReviewTypeModel } from '../../models/review-type.model';
import { Subscription } from 'rxjs';
import { FailedRulesLoanModel } from '../../models/failed-rules-loan.model';
import * as Highcharts from 'highcharts';

@Component({
    selector: 'mts-failed-rules-loan',
    templateUrl: 'failed-rules-loan.page.html',
    styleUrls: ['failed-rules-loan.page.css']
})
export class FailedRulesLoanComponent implements OnInit, AfterViewInit, OnDestroy {
    Highcharts: typeof Highcharts = Highcharts;
    @ViewChild('failedRulesDateRange') _failedRulesDateRange;
    failedRulesLoanData: FailedRulesLoanModel = new FailedRulesLoanModel(false);
    FailedRuledatevalue: any;
    daterangeoptions: any;
    _failedRuleDate: boolean;

    ReviewTypes: ReviewTypeModel[] = [];
    _reviewTypeID: number = ServiceTypeConstant.POST_CLOSING;

    _failedRulesChartElement: any;
    _ShowFailedRulesChart = false;

    allErrorRuleTypes: any = [{ id: '-1', text: 'All' }, { id: '0', text: 'Failed' }, { id: '2', text: 'Error' }];
    ruleType = -1;

    _failedRulesOptions = {
        chart: {
            type: 'column'
        },
        plotOptions: {
            column: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false,
                    format: '<b>{point.name}</b>: {point.y}'
                },
                events: {
                    click: (e) => {
                        this.onLoanFailedRulesDocPointClick(e);
                    }
                },
                showInLegend: true
            }
        },
        colors: ['#FF7939', '#FFAF00', '#055CB2', '#1FABDE', '#64AB23', '#1FABDE', '#4D4E53', '#CACAC8', '#E5E6E5', '#9A9B9D'],
        title: {
            text: ''
        },
        credits: {
            enabled: false
        },
        lang: {
            noData: 'No Data Available'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        legend: {
            enabled: false
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Loan Count'
            }
        },
        exporting: {
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
    constructor(private _dashboardService: DashboardService) {
    }
    //#endregion Constructor

    //#region  Private Variables
    private subscriptions: Subscription[] = [];
    //#endregion Private Variables

    ngOnInit(): void {
        this.subscriptions.push(this._dashboardService.reviewTypeList.subscribe((res: ReviewTypeModel[]) => {
            this.ReviewTypes = [...res];
        }));

        this.subscriptions.push(this._dashboardService.failedRulesLoanData.subscribe((res: FailedRulesLoanModel) => {

            this.failedRulesLoanData = res;
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
        this._dashboardService.getReviewTypeList();
        const fromdate = new Date();
        const todate = new Date();
        this.GetLoanFailedRuleDatas(fromdate, todate);
    }

    GetLoanFailedRuleDatas(FromDate, ToDate) {
        this._dashboardService.GetFailedRuleLoans(false, this._failedRulesDateRange, this._reviewTypeID, this.ruleType, this._failedRulesChartElement);
    }

    onLoanFailedRulesDocPointClick(e: any) {
        this._dashboardService.onLoanFailedRulesDocPointClick(e, this._reviewTypeID, this.ruleType, this._failedRulesDateRange);
    }

    saveInstance(chartInstances) {
        const onLoad = this._failedRuleDate;
        this._failedRulesChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'LoanFailedRules', onLoad);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
