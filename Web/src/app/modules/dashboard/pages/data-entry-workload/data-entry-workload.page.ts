import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { ServiceTypeConstant, AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { ReportStateModel } from '../../models/report-state.model';
import { DataEntryWorkLoadModel } from '../../models/data-entry-workload.model';

import * as Highcharts from 'highcharts';
import { CustomerModel } from '../../models/customer.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
@Component({
    selector: 'mts-data-entry-work-load',
    templateUrl: 'data-entry-workload.page.html'
})
export class DataEntryWorkLoadReportComponent implements OnInit, AfterViewInit, OnDestroy {
    Highcharts: typeof Highcharts = Highcharts;
    @ViewChild('DEWorkLoad') DEWorkLoadDateRange;
    DEWorkLoadDateRangeValues: any;

    criticalRuleCustomerid: any = -1;
    staticLabel = AppSettings.AuthorityLabelPlural;
    ActiveCustomers: CustomerModel[] = [];

    dataEntryWorkLoadData: DataEntryWorkLoadModel[] = [];
    ocrSelectedvalue: any = 1;
    ocrSelect = [{ id: 1, fieldvalue: 'Ready for Review' }, { id: 2, fieldvalue: 'Ready for Validation' }];
    _dataEntryWorkLoadOptions = {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        lang: {
            thousandsSep: '',
            noData: ''
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
            name: 'Loan Count',
            colorByPoint: true,
            data: [['Série 1', 45.0]]
        }]
    } as any;

    _retDocDate: boolean;
    _dataEntryWorkLoadChartElement: any;
    daterangeoptions: any;
    constructor(private _dashboardService: DashboardService, public _reportState: ReportStateModel) { }

    private subscriptions: Subscription[] = [];

    ngOnInit(): void {
        this.subscriptions.push(this._dashboardService.activeCustomersList.subscribe((res: CustomerModel[]) => {
            this.ActiveCustomers = [...res];
        }));

        this.subscriptions.push(this._dashboardService._DEWorkLoad$.subscribe((res: any[]) => {
            if (res.length > 0) {
                this.dataEntryWorkLoadData = res;
            } else {
                this.dataEntryWorkLoadData = [];
            }
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

        this._dashboardService.getActiveCustomersList();
    }

    saveInstance(chartInstances) {
        const onLoad = this._retDocDate;
        this._dataEntryWorkLoadChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'DataEnryWorlLoad', onLoad);
    }

    ngAfterViewInit() {
        this.getMonthYear(this.DEWorkLoadDateRange, 'DataEnryWorlLoad');
    }

    getMonthYear(val, reportType) {
        this._retDocDate = val.OnInit;
        if (isTruthy(this._reportState.DataEntryWorkLoad) && isTruthy(this._reportState.DataEntryWorkLoad.Data) && (!(this._reportState.DataEntryWorkLoad.OnLoad) && this._retDocDate)) {
            if (typeof this._dataEntryWorkLoadChartElement !== 'undefined') {
                this._dataEntryWorkLoadChartElement.series[0].setData(this._reportState.DataEntryWorkLoad.Data);
            }
        } else {
            this._dashboardService.getMonthYear(this.DEWorkLoadDateRange, reportType, this._dataEntryWorkLoadChartElement, this.criticalRuleCustomerid, this.ocrSelectedvalue);
        }
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
