import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { CustomerModel } from '../../models/customer.model';
import { Subscription } from 'rxjs';
import { DashboardService } from '../../service/dashboard.service';
import { LoanDefectsModel } from '../../models/loan-defects.model';
import * as Highcharts from 'highcharts';
import { ReportStateModel } from '../../models/report-state.model';

@Component({
    selector: 'mts-loan-defects',
    templateUrl: 'loan-defects.page.html',
    styleUrls: ['loan-defects.page.css']
})
export class LoanDefectsComponent implements OnInit, AfterViewInit, OnDestroy {

    Highcharts: typeof Highcharts = Highcharts;

    @ViewChild('criticalRulesDateRange') _criticalRulesDateRange;

    loanDefectsData: LoanDefectsModel = new LoanDefectsModel(false);
    criticalRulesDateRangeValues: any;
    daterangeoptions: any;

    criticalRuleCustomerid: any = 0;
    staticLabel = AppSettings.AuthorityLabelPlural;
    ActiveCustomers: CustomerModel[] = [];
    _criticalRulesDate: any;
    _criticalRulesChartElement: any;

    _criticalRulesOptions = {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        lang: {
            thousandsSep: ''
        },
        title:
        {
            text: ''
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.y}</b>'
        },
        exporting: {
            enabled: false
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: false,
                    format: '<b>{point.name}</b>: {point.y}'
                },
                events: {
                    click: (e) => {
                        this.onCriticalRulesPointClick(e);
                    }
                },
                showInLegend: true
            }
        },
        legend: {
            align: 'right',
            layout: 'vertical',
            verticalAlign: 'middle',
            x: 0,
            y: 0,
            labelFormatter: function () {
                return this.name + ' ' + '(' + this.y + ')';
            }
        },
        credits: {
            enabled: false
        },
        yAxis: {
            title: {
                text: 'Rule'
            }
        },
        series: [{
            name: 'Rule Count',
            colorByPoint: true,
            data: [['Série 1', 45.0]]
        }]
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

        this.subscriptions.push(this._dashboardService.loanDefectsData.subscribe((res: LoanDefectsModel) => {

            this.loanDefectsData = res;
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
        this.getMonthYear(this._criticalRulesDateRange, 'CriticalRulesFailed');
    }

    getMonthYear(val, reportType) {
        this._criticalRulesDate = val.OnInit;

        if (typeof this._reportState.CriticalRulesFailed.Data !== 'undefined' && (!(this._reportState.CriticalRulesFailed.OnLoad) && this._criticalRulesDate)) {

            if (typeof this._criticalRulesChartElement !== 'undefined') {
                this._criticalRulesChartElement.series[0].setData(this._reportState.CriticalRulesFailed.Data);
            }
        } else {

            this._dashboardService.getMonthYear(this._criticalRulesDateRange, reportType, this._criticalRulesChartElement, 0, this.criticalRuleCustomerid);
        }
    }

    GetCriticalRules(onLoad: boolean) {

        this._dashboardService.GetCriticalRules(onLoad, this._criticalRulesDateRange, this.criticalRuleCustomerid, this._criticalRulesChartElement);
    }

    onCriticalRulesPointClick(e: any) {

        this._dashboardService.onCriticalRulesPointClick(e, this.criticalRuleCustomerid, this._criticalRulesDateRange);
    }

    saveInstance(chartInstances) {

        const onLoad = this._criticalRulesDate;
        this._criticalRulesChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'CriticalRulesFailed', onLoad);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
