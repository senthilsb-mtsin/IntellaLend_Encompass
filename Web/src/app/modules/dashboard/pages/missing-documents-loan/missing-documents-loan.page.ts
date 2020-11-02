import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { Subscription } from 'rxjs';
import { ServiceTypeConstant, ReportTypeConstant } from '@mts-app-setting';
import { ReviewTypeModel } from '../../models/review-type.model';
import { MissingDocumentsLoanModel } from '../../models/missing-documents-loan.model';
import * as Highcharts from 'highcharts';
import { ReportStateModel } from '../../models/report-state.model';
const HighchartsMore = require('highcharts/highcharts-more.src');
HighchartsMore(Highcharts);
const HC_solid_gauge = require('highcharts/modules/solid-gauge.src');
HC_solid_gauge(Highcharts);
const hcExport = require('highcharts/modules/exporting');
hcExport(Highcharts);

@Component({
    selector: 'mts-missing-documents-loan',
    templateUrl: 'missing-documents-loan.page.html',
    styleUrls: ['missing-documents-loan.page.css']
})
export class MissingDocumentsLoanComponent implements OnInit, AfterViewInit, OnDestroy {

    Highcharts: typeof Highcharts = Highcharts;
    @ViewChild('MissingDocsdaterange') MissingDocsdaterange;
    _missingRecordedLoansDate: boolean;
    _missingRecordedLoansChartElement: any;

    missingDocumentsLoanData: MissingDocumentsLoanModel = new MissingDocumentsLoanModel(false);

    ReviewTypes: ReviewTypeModel[] = [];
    MRReviewTypesID: any = ServiceTypeConstant.POST_CLOSING;

    MissingDocs: any;
    daterangeoptions: any;
    _missingRecordedLoansOptions = {
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
                    text: ReportTypeConstant.MISSING_RECORDED_LOANS.Description
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
                        this.onMissingRecordedLoansPointClick(e);
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
                    '<span style="font-size:15px;color:red">Missing Loan(s)</span></div>'
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

        this.subscriptions.push(this._dashboardService.missingDocumentsLoanData.subscribe((res: MissingDocumentsLoanModel) => {

            this.missingDocumentsLoanData = res;
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
        this.getMonthYear(this.MissingDocsdaterange, 'MissingRecordedLoans');
    }

    getMonthYear(val, reportType) {

        this._missingRecordedLoansDate = val.OnInit;
        if (typeof this._reportState.MissingRecordedLoans.Data !== 'undefined' && (!(this._reportState.MissingRecordedLoans.OnLoad) && this._missingRecordedLoansDate)) {

            if (typeof this._missingRecordedLoansChartElement !== 'undefined') {
                this._missingRecordedLoansChartElement.series[0].setData(this._reportState.MissingRecordedLoans.Data);
            }
        } else {
            this._dashboardService.getMonthYear(this.MissingDocsdaterange, reportType, this._missingRecordedLoansChartElement, this.MRReviewTypesID);
        }
    }

    DownloadMissingRecordedLoans() {

        const localChartObject = this._missingRecordedLoansChartElement;
        this._dashboardService.DownloadMissingRecordedLoans(this._missingRecordedLoansChartElement);
        this._missingRecordedLoansChartElement = localChartObject;
    }

    onMissingRecordedLoansPointClick(e: any) {
        this._dashboardService.onMissingRecordedLoansPointClick(e, this.MRReviewTypesID, this.MissingDocsdaterange);
    }

    saveInstance(chartInstances) {

        const onLoad = this._missingRecordedLoansDate;
        this._missingRecordedLoansChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'MissingRecordedLoans', onLoad);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
