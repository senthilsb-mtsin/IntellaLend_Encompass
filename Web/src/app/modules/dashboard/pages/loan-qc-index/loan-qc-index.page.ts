import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { ServiceTypeConstant, ReportTypeConstant } from 'src/app/shared/constant/app-setting-constants/dashboard-setting.constant';
import { ReviewTypeModel } from '../../models/review-type.model';
import { DashboardService } from '../../service/dashboard.service';
import { Subscription } from 'rxjs';
import { DatePipe } from '@angular/common';
import { AppSettings } from '@mts-app-setting';
import { DashboardGraphRequestModel } from '../../models/dashboard-graph-request.model';
import { DashboardReportModel } from '../../models/dashboard-report.model';
import { NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { CommonService } from 'src/app/shared/common';
import { QualityIndexModel } from '../../models/quality-index.model';

@Component({
    selector: 'mts-loan-qc-index',
    templateUrl: 'loan-qc-index.page.html',
    styleUrls: ['loan-qc-index.page.css']
})
export class LoanQcIndexComponent implements OnInit, AfterViewInit, OnDestroy {

    @ViewChild('QCIndexDateRange') QCIndexDateRange;

    //#region Public Variables
    qualityIndexData: QualityIndexModel = new QualityIndexModel(0, 0, 0, false, '', 0);
    QCReviewTypeID: number = ServiceTypeConstant.POST_CLOSING;
    ReviewTypes: ReviewTypeModel[] = [];
    daterangeoptions: any;
    QCIndexDateRangeValue: any;

    _loanFailedCount = 0;
    _LoanPercentange = 0;

    _totalloancount = 0;
    _showIndex = false;
    _LColor = '';
    _waterPercentage = 0;

    //#endregion Public Variables

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

        this.subscriptions.push(this._dashboardService.qualityIndexData.subscribe((res: QualityIndexModel) => {

            this.qualityIndexData = res;
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
        this.getMonthYear(this.QCIndexDateRange, 'LoanQcIndex');
    }

    getMonthYear(val, reportType) {
        this._dashboardService.getMonthYear(this.QCIndexDateRange, reportType, null, this.QCReviewTypeID);
    }

    getLoanRecords(val: any) {
        this._dashboardService.getLoanRecords(val, this.QCReviewTypeID, this.QCIndexDateRange);
    }

    ngOnDestroy() {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
