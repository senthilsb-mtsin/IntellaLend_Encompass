import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/shared/common';
import { KpiGoalconfigService } from '../services/kpi-goal-configs.service';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { KpiUserGroupConfigModel, KPIGoalConfig } from '../models/kpiuserconfig.model';
import { NotificationService } from '@mts-notification';
import { DataTableDirective } from 'angular-datatables';
import { KpiUserGroupTable } from '../models/kpiuserConfigTable.model';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppSettings } from '@mts-app-setting';
import { KPIStatusConstant } from '@mts-status-constant';

@Component({
    selector: 'mts-kpi-goal-configs',
    templateUrl: 'kpi-goal-configs.page.html',
    styleUrls: ['kpi-goal-configs.page.css'],

})
export class KpiGoalConfigsComponent implements OnInit, OnDestroy {
    @ViewChild('confirmModal') confirmModal: ModalDirective;
    @ViewChild('infoModal') _infoModal: ModalDirective;
    _KpiUserGroupConfig: KpiUserGroupConfigModel = new KpiUserGroupConfigModel();
    _kpiUserConfigData = new KpiUserGroupConfigModel();
    previousConfigType = '';
    currentConfigType = '';
    roleItems: any = [];
    PeriodConfigTypes: any = [];
    _kpiUserShow = false;
    _addGoalShow = false;
    isValidate = false;
    UserRoleList: any = [];
    _kpiStagingData: any = [];
    promise: Subscription;
    constructor(private _notifyService: NotificationService, private _kpiGoalService: KpiGoalconfigService) {
    }
    private subscription: Subscription[] = [];
    ngOnInit() {

        this.PeriodConfigTypes = this._kpiGoalService.PeriodConfigTypes;
        this.subscription.push(this._kpiGoalService._KpiUserGroupConfigTable.subscribe((res: any) => {
            this._kpiUserConfigData = res;
            this.SetKpiRowValues();
        }));
        this.subscription.push(this._kpiGoalService.UserRoleList.subscribe((res: any) => {
            this.UserRoleList = res;
        }));
        this.subscription.push(this._kpiGoalService._kpiStagingData.subscribe((res: any) => {
            this._kpiStagingData = res;
        }));

        this.subscription.push(this._kpiGoalService._kpiUserShow.subscribe((res: boolean) => {
            this._kpiUserShow = res;
        }));
        this.subscription.push(this._kpiGoalService._addGoalShow.subscribe((res: boolean) => {
            this._addGoalShow = res;
        }));
        this.subscription.push(this._kpiGoalService.RoleItems.subscribe((res: any[]) => {
            this.roleItems = res;
        }));
        this.GettrolemasterList();
    }

    GettrolemasterList() {
        const _req: any = { TableSchema: AppSettings.TenantSchema };
        this._kpiGoalService.GetAllRoles(_req);
    }
    GetKPIGoalConfigStagingDetails() {
       const inputData = { TableSchema: AppSettings.TenantSchema, GroupID: this._KpiUserGroupConfig.RoleID, ConfigType : this._KpiUserGroupConfig.ConfigType};
        this._kpiGoalService.GetKPIGoalConfigStagingDetails(inputData);

    }
    SetKpiRowValues() {

        this._KpiUserGroupConfig.Goal = this._kpiUserConfigData[0].TotalGoals;
        this._KpiUserGroupConfig.ConfigType = this._kpiUserConfigData[0].ConfigType;
        this._KpiUserGroupConfig.IsValid = this._kpiUserConfigData[0].IsValid;
        if (this._KpiUserGroupConfig.IsValid === false) {
            this._infoModal.show();
        }
    }

    AddKPIConfigStagingDetails() {
        const _checkgoalcount = this._KpiUserGroupConfig.Goal % this.UserRoleList.length;
        if (this._KpiUserGroupConfig.Goal > 0) {
            if (_checkgoalcount === 0) {
                const isValid = this.CheckValidConfiguration();
                if (isValid) {
                    const userGoalCount = this._KpiUserGroupConfig.Goal / this.UserRoleList.length;
                    const inputData = { TableSchema: AppSettings.TenantSchema, GroupID: this._KpiUserGroupConfig.RoleID, ConfigType: this._KpiUserGroupConfig.ConfigType, Goal: this._KpiUserGroupConfig.Goal };
                    this.promise = this._kpiGoalService.AddKPIConfigStagingDetails(inputData);
                    this.confirmModal.hide();
                }
            } else {
                this._notifyService.showError('This Role has ' + this.UserRoleList.length + ' users and the system  could not equally distribute to available users');
                this.confirmModal.hide();
            }
        } else {
            this._notifyService.showError('Please Enter a Valid Goal Count');

        }

    }
    CheckValidConfiguration(): boolean {
        this.isValidate = true;
        const regex = (/^(0|[1-9]\d*)?$/);
        if (this._KpiUserGroupConfig.ConfigType === '0' || this._KpiUserGroupConfig.ConfigType === 0) {
            this._notifyService.showError('Please Select Period');
            this.isValidate = false;
        } else if (this._KpiUserGroupConfig.Goal === '' || this._KpiUserGroupConfig.Goal === 0 || !regex.test(this._KpiUserGroupConfig.Goal)) {
            this._notifyService.showError('Please Enter a Valid Goal Count');
            this.isValidate = false;
        }
        return this.isValidate;
    }

    RoleChange() {
        if (this._KpiUserGroupConfig.RoleID > 0) {
            this._addGoalShow = true;
            this._kpiUserShow = true;
            this._KpiUserGroupConfig.ConfigType = 0;
            this._KpiUserGroupConfig.Goal = '';
            const _userRoleReq: any = { TableSchema: AppSettings.TenantSchema, RoleID: this._KpiUserGroupConfig.RoleID };
            const _kpiStagingReq = { TableSchema: AppSettings.TenantSchema, GroupID: this._KpiUserGroupConfig.RoleID, ConfigType: this._KpiUserGroupConfig.ConfigType };
            this._kpiGoalService.GetUserRoleList(_userRoleReq, _kpiStagingReq);
        } else {
            this._addGoalShow = false;
            this._kpiUserShow = false;
        }

    }

    ValidateConfiguration() {
        if (this._kpiStagingData.length > 0 ) {
            if (this._kpiStagingData[0].ConfigType !== 0) {
                const configData = convertDateTime(this._kpiStagingData[0].CreatedOn);
                const today = new Date();
                const configDate = new Date(configData);
                if (today.getMonth() !== configDate.getMonth() || today.getDay() !== configDate.getDay() || today.getFullYear() !== configDate.getFullYear()) {
                    if (this._kpiStagingData[0].ConfigType !== this._KpiUserGroupConfig.ConfigType) {
                        this.previousConfigType = KPIStatusConstant.ConfigTypeDescription[this._kpiStagingData[0].ConfigType];
                        this.currentConfigType = KPIStatusConstant.ConfigTypeDescription[this._KpiUserGroupConfig.ConfigType];
                        this.confirmModal.show();
                    } else {
                        this.AddKPIConfigStagingDetails();
                    }
                } else {
                    this.AddKPIConfigStagingDetails();
                }
            } else {
                this.AddKPIConfigStagingDetails();
            }
        } else {
            this.AddKPIConfigStagingDetails();
        }
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
