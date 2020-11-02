import { AppSettings } from '@mts-app-setting';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { DashboardService } from '../../service/dashboard.service';
import { Subscription } from 'rxjs';
import { KPIUserGroupModel } from '../../models/kpi-user-groups.model';
import { KPIGoalDetailsModel } from '../../models/kpi-goal-details.model';
import { NotificationService } from '@mts-notification';

@Component({
    selector: 'mts-kpi-user-groups',
    templateUrl: 'kpi-user-groups.page.html',
    styleUrls: ['kpi-user-groups.page.css']
})

export class KpiUserGroupsComponent implements OnInit, OnDestroy {

    kpiUserGroupClass = '';
    KpiUserGroupDivdisplay = 'none';
    _UserGroupCarousel = [];
    KpiUserGroupData: any = [];

    kpisearch = false;
    ShowKpiUserDetails = false;
    ShowKpiCardDetails = true;
    KpiScoreCardData: any = [];
    _carousel = [];

    KpiPromise: Subscription;
    dateFromTo: any = [];

    KpiDivClass = '';
    KpiDivdisplay = 'none';
    _showCarousel = false;

    //#region  Constructor
    constructor(private _dashboardService: DashboardService, private _notificationService: NotificationService) {
      this._dashboardService.GetRoleMasterList({ TableSchema: AppSettings.TenantSchema });
    }
    //#endregion Constructor

    //#region  Private Variables
    private subscriptions: Subscription[] = [];
    //#endregion Private Variables

    ngOnInit(): void {
        this.subscriptions.push(this._dashboardService.kpiUserGroupData.subscribe((res: KPIUserGroupModel) => {
            this.KpiUserGroupData = res.KpiUserGroupData;
            this._UserGroupCarousel = res.UserGroupCarousel;
            if (this.KpiUserGroupData.length > 0) {

                this._dashboardService.SetUserGroupCarouselItem(this.KpiUserGroupData, this._UserGroupCarousel, this.dateFromTo);
                this.KpiDivClass = '';
                this.KpiDivdisplay = 'block';
                this.kpiUserGroupClass = '';
                this.KpiUserGroupDivdisplay = 'block';
            } else {
                this.KpiDivClass = '';
                this.KpiDivdisplay = 'none';
                this.kpiUserGroupClass = '';
                this.KpiUserGroupDivdisplay = 'none';
            }
        }));

        this.subscriptions.push(this._dashboardService.kpiGoalDetailsData.subscribe((res: KPIGoalDetailsModel) => {
            this.KpiScoreCardData = res.KpiScoreCardData;
            this._carousel = res.Carousel;
            this.ShowKpiUserDetails = true;
        }));
      this.subscriptions.push(this._dashboardService.rolelist$.subscribe((res: any) => {
        this.GetKpiUserGroupDetail();
        this._dashboardService.SetUserGroupCarouselItem(this.KpiUserGroupData, this._UserGroupCarousel, this.dateFromTo);
      }));

    }

    GetKpiUserGroupDetail() {

        this._dashboardService.GetKpiUserGroupDetail(this.KpiUserGroupData, this._UserGroupCarousel);
    }

    SetCarouselItem() {
        this._carousel = [];
        const carouselLength = this.KpiScoreCardData[0].length / 6;
        let cLength = 0, incrementer = 0;
        for (let index = 0; index < carouselLength; index++) {
            let indexLength = 6;
            if (this.KpiScoreCardData[0].length > 6) {
                indexLength = (this.KpiScoreCardData[0].length - incrementer) > 6 ? 6 : this.KpiScoreCardData[0].length - incrementer;
            } else {
                indexLength = this.KpiScoreCardData[0].length;
            }

            const cSet = [];
            for (let i = 0; i < indexLength; i++) {
                if (cLength < this.KpiScoreCardData[0].length) {
                    cSet.push(this.KpiScoreCardData[0][cLength]);
                }
                cLength++;
            }
            incrementer = incrementer + indexLength;
            this._carousel.push({ carouselSet: cSet });
        }
    }

    ScrollDate(prevNext, _CData: any) {
        if ((_CData.ScrollIndex < 0 && prevNext === 'prev') || (_CData.dataValues.length) - 1 === _CData.ScrollIndex && prevNext === 'next') {

        } else {
            if (prevNext === 'prev') {
                if (_CData.ScrollIndex > 0) {
                    const ScrollIndex = _CData.ScrollIndex - 1;
                    if (_CData.ConfigType === _CData.dataValues[ScrollIndex].ConfigType) {
                        _CData.ScrollIndex = _CData.ScrollIndex - 1;
                        const dates = _CData.dataValues[_CData.ScrollIndex];
                        _CData.dateFromTo = dates.Text;
                        const val = dates.id + ',' + dates.Flag + ',' + dates.PeriodFrom + ',' + dates.PeriodTo + ',' + dates.AuditGoalID;
                        this._dashboardService.datechangefunction(val, _CData);
                    } else {
                        this._notificationService.showError('Different Configuration types cannot be viewed');
                    }
                }
            } else {
                if ((_CData.dataValues.length) - 1 >= _CData.ScrollIndex) {
                    const ScrollIndex = _CData.ScrollIndex + 1;
                    if (_CData.ConfigType === _CData.dataValues[ScrollIndex].ConfigType) {
                        _CData.ScrollIndex = _CData.ScrollIndex + 1;
                        const dates = _CData.dataValues[_CData.ScrollIndex];
                        _CData.dateFromTo = dates.Text;
                        const val = dates.id + ',' + dates.Flag + ',' + dates.PeriodFrom + ',' + dates.PeriodTo + ',' + dates.AuditGoalID;
                        this._dashboardService.datechangefunction(val, _CData);
                    } else {
                        this._notificationService.showError('Different Configuration types cannot be viewed');
                    }
                }
            }
        }
    }

    GetKpiLoanDetail(UserID: any, PeriodFrom: any, PeriodTo: any, UserName: any, AchievedGoalCount: any) {
        this._dashboardService.GetKpiLoanDetail(UserID, PeriodFrom, PeriodTo, UserName, AchievedGoalCount);
    }

    GetKpiGoalDetails(values: any, eventType: String, _CData) {
        if (_CData.AchievedGoalCount > 0) {
            this.kpiUserGroupClass = '';
            let dateValue = '';
            this.KpiUserGroupDivdisplay = 'none';
            this.kpisearch = true;
            this.ShowKpiUserDetails = false;
            this.ShowKpiCardDetails = false;
            this.KpiScoreCardData = [];
            dateValue = values.currentTarget.offsetParent.getElementsByClassName('selectedDate')[0].innerText;
            dateValue = dateValue.trim();
            const auditData = _CData.dataValues.filter(a => a.Text === dateValue);
            this._dashboardService.GetKpiGoalDetails(_CData.RoleID, auditData, this._carousel, this.kpisearch, this.ShowKpiUserDetails, this.KpiScoreCardData, this.ShowKpiCardDetails);
            this._dashboardService.SetRoleID(_CData.RoleID);
        } else {
            this._notificationService.showError('No Loans found for these users on the selected dates');
        }
    }

    gotoback() {
        this.kpiUserGroupClass = '';
        this.KpiUserGroupDivdisplay = 'block';
        this.ShowKpiCardDetails = true;
        this.ShowKpiUserDetails = false;
    }

    ngOnDestroy() {
        this.subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
