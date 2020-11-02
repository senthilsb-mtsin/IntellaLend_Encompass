import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { Subscription } from 'rxjs';
import { ServiceTypeConstant } from '@mts-app-setting';
import { ReviewTypeModel } from '../../models/review-type.model';
import { InvestorStipulationModel } from '../../models/investor-stipulation.model';
import * as Highcharts from 'highcharts';
import { formatDate } from '@mts-functions/convert-datetime.function';

@Component({
    selector: 'mts-investor-stipulations',
    templateUrl: 'investor-stipulations.page.html',
    styleUrls: ['investor-stipulations.page.css']
})
export class InvestorStipulationsComponent implements OnInit, AfterViewInit, OnDestroy {

    Highcharts: typeof Highcharts = Highcharts;
    ReviewTypes: ReviewTypeModel[] = [];
    ReviewTypeID: any = ServiceTypeConstant.POST_CLOSING;

    loanStipulationType = 0;
    loanStipulations: any = [{ id: '0', text: 'All' }, { id: '1', text: 'Pending' }, { id: '2', text: 'Completed' }, { id: '3', text: 'Cancelled' }];

    @ViewChild('StipulationdateRange') StipulationdateRange;
    daterangeoptions: any;
    Stipulationdatevalue: any;
    StipulationDate: boolean;
    investorStipulationData: InvestorStipulationModel = new InvestorStipulationModel(false);

    ShowReviewAuditDate = false;
    FromDateLoanInvestor: any;
    ToDateLoanInvestor: any;

    _StipulationChartElement: any;
    _StipulationOptions = {
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
        legend: {
            enabled: false
        },
        tooltip: {

            pointFormat: 'Stipulation Count: <b>{point.y}</b>'
        },
        lang: {
            noData: 'No Data Available'
        },
        xAxis: {
            type: 'category'
        },
        yAxis: {
            title: {
                text: 'Stipulation Count'
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
                        this.InvestorStipulationCLick(e);
                    }
                },
                showInLegend: true
            },
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y}'
                }
            }
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

        this.subscriptions.push(this._dashboardService.investorStipulationData.subscribe((res: InvestorStipulationModel) => {

            this.investorStipulationData = res;
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
        if (this.ShowReviewAuditDate) {
            const fromdate = new Date();
            const todate = new Date();
        }
        this.GetInvestorStipulation(this.StipulationdateRange.dateFrom, this.StipulationdateRange.dateTo);
    }

    GetInvestorStipulation(FromDate, ToDate) {
         this.FromDateLoanInvestor = formatDate(this.StipulationdateRange.dateFrom);
         this.ToDateLoanInvestor = formatDate(this.StipulationdateRange.dateTo);
        this._dashboardService.GetStipulationData(false, FromDate, ToDate, this.ReviewTypeID, this.loanStipulationType, this._StipulationChartElement);
    }

    InvestorStipulationCLick(e: any) {
        this._dashboardService.InvestorStipulationCLick(e, this.ReviewTypeID, this.loanStipulationType, this.StipulationdateRange);
    }

    saveInstance(chartInstances) {
        const onLoad = this.StipulationDate;
        this._StipulationChartElement = chartInstances;
        this._dashboardService.saveInstance(chartInstances, 'InvestorStipulation', onLoad);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
