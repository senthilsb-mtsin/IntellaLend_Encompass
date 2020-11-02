import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { Subscription } from 'rxjs';
import { ServiceTypeConstant, ReportTypeConstant } from '@mts-app-setting';
import { ReviewTypeModel } from '../../models/review-type.model';
import { DocumentRetentionModel } from '../../models/document-retention.model';
import * as Highcharts from 'highcharts';
import { ReportStateModel } from '../../models/report-state.model';

@Component({
    selector: 'mts-document-retention-monitor',
    templateUrl: 'document-retention-monitor.page.html',
    styleUrls: ['document-retention-monitor.page.css']
})
export class DocumentRetentionMonitorComponent implements OnInit, AfterViewInit, OnDestroy {

    Highcharts: typeof Highcharts = Highcharts;
    @ViewChild('RetentionDateRange') RetentionDateRange;
    RetentionDateRangeValues: any;
    daterangeoptions: any;
    _retDocDate: boolean;

    _documentsRetentionMonitoringChartElement: any;

    ReviewTypes: ReviewTypeModel[] = [];
    DRReviewTypeID: any = ServiceTypeConstant.POST_CLOSING;

    documentRetentionData: DocumentRetentionModel = new DocumentRetentionModel(false);

    _documentRetetntionMonitoringOptions = {
        chart: {
            type: 'solidgauge'
        },
        title: '',
        pane: {
            center: ['50%', '85%'],
            size: '140%',
            startAngle: -90,
            endAngle: 90,
            background: {
                innerRadius: '60%',
                outerRadius: '100%',
                shape: 'arc'
            }
        },
        exporting: {
            allowHTML: true,
            chartOptions: {
                title: {
                    text: ReportTypeConstant.DOCUMENT_RETENTION_MONITORING.Description
                }
            },
            enabled: false
        },
        tooltip: {
            enabled: false
        },

        // the value axis
        yAxis: {
            stops: [
                [0.1, '#55BF3B'], // green
                [0.5, '#DDDF0D'], // yellow
                [0.7, '#DF5353'] // red
            ],
            lineWidth: 0,
            minorTickInterval: null,
            tickAmount: 2,
            // title: {
            //     y: -70
            // },
            labels: {
                y: 16
            },
            min: 0,
            max: 100
        },

        plotOptions: {
            solidgauge: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    y: 5,
                    borderWidth: 0,
                    useHTML: true,
                    format: '<b>{point.name}</b>: {point.y}'
                },
                events: {
                    click: (e) => {
                        this.onDocumentRetentionMonitoringPointClick(e);
                    }
                }
            },
            series: {
                cursor: 'pointer'
            }
        },
        credits: {
            enabled: false
        },

        series: [{
            name: '',
            data: [],
            dataLabels: {
                format: '<div  style="text-align:center;cursor:pointer;"><span style="font-size:30px;color:red;">{y}</span><br/>' +
                    '<span style="font-size:15px;color:red">Expired Loan(s)</span></div>'
            }
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
        this.subscriptions.push(this._dashboardService.reviewTypeList.subscribe((res: ReviewTypeModel[]) => {
            this.ReviewTypes = [...res];
        }));

        this.subscriptions.push(this._dashboardService.documentRetentionData.subscribe((res: DocumentRetentionModel) => {

            this.documentRetentionData = res;
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
        this.getMonthYear(this.RetentionDateRange, 'DocumentRetentionMonitoring');
    }

    getMonthYear(val, reportType) {
        this._retDocDate = val.OnInit;
        if (reportType === 'DocumentRetentionMonitoring') {
            if (typeof this._reportState.DocRetention.Data !== 'undefined' && (!(this._reportState.DocRetention.OnLoad) && this._retDocDate)) {
                if (typeof this._documentsRetentionMonitoringChartElement !== 'undefined') {
                    this._documentsRetentionMonitoringChartElement.series[0].setData(this._reportState.DocRetention.Data);
                }
            } else {
                this._dashboardService.getMonthYear(this.RetentionDateRange, reportType, this._documentsRetentionMonitoringChartElement, this.DRReviewTypeID);
            }
        }
    }

    DownloadDocumentRetentionChart() {
        const localChartObject = this._documentsRetentionMonitoringChartElement;
        this._dashboardService.DownloadMissingRecordedLoans(this._documentsRetentionMonitoringChartElement);
        this._documentsRetentionMonitoringChartElement = localChartObject;
    }

    onDocumentRetentionMonitoringPointClick(e: any) {
        this._dashboardService.onDocumentRetentionMonitoringPointClick(e, this.DRReviewTypeID, this.RetentionDateRange);
    }

    saveInstance(chartInstances) {
        const onLoad = this._retDocDate;
        this._documentsRetentionMonitoringChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'DocumentRetentionMonitoring', onLoad);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
