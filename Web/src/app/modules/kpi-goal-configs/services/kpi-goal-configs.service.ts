import { JwtHelperService } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { KpiGoalConfigsDataAccess } from '../kpi-goal-configs.data';
import { Subject } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings } from '@mts-app-setting';

const jwtHelper = new JwtHelperService();

@Injectable()

export class KpiGoalconfigService {

    UserRoleList = new Subject<any>();
    PeriodConfigTypes: any = [{ id: 1, text: 'Daily' }, { id: 2, text: 'Weekly' }, { id: 3, text: 'Monthly' }, { id: 4, text: 'Yearly' }];
    _KpiUserGroupConfigTable = new Subject<any>();
    _kpiStagingData = new Subject<any>();
    _kpiUserShow = new Subject<any>();
    _addGoalShow = new Subject<any>();
    RoleItems = new Subject<any[]>();

    constructor(private _notificationservice: NotificationService, private _kpiDataAccess: KpiGoalConfigsDataAccess) {

    }
    private _userRoleList: any = [];
    private _kpiUserConfigData: any = [];
    private _StagingData = [];
    private roleItems = [];

    GetUserRoleList(_userRoleReq: any, _kpiStagingReq: any) {
        return this._kpiDataAccess.GetUserRoleList(_userRoleReq).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                this._userRoleList = [];
                if (data.UserList.length > 0) {
                    this._userRoleList = data.UserList;
                }
                this.UserRoleList.next(this._userRoleList.slice());
                this.GetKPIGoalConfigStagingDetails(_kpiStagingReq);
            }

        });
    }
    GetAllRoles(_reqBody: any) {
        return this._kpiDataAccess.GetAllRoles(_reqBody).subscribe(res => {
            if (isTruthy(res)) {
                const _roles = jwtHelper.decodeToken(res.Data)['data'];
                if (_roles.length > 0) {
                    const _index = _roles.findIndex(x => x.RoleName === 'System Administrator');
                    if (_index !== -1) {
                        _roles.splice(_index, 1);
                    }
                    this.roleItems = [];
                    _roles.forEach(element => {
                        if (element.IncludeKpi === true) {
                            this.roleItems.push(element);
                        }
                    });
                    this.RoleItems.next(this.roleItems.slice());
                } else {
                    this.roleItems = [];
                    this.RoleItems.next(this.roleItems);
                }
            }
        });
    }

    AddKPIConfigStagingDetails(_reqBody: any) {
        return this._kpiDataAccess.SaveKPIConfigStagingDetails(_reqBody).subscribe(res => {
            if (isTruthy(res)) {
                const _addkpigoalConfig = jwtHelper.decodeToken(res.Data)['data'];
                if (_addkpigoalConfig.length > 0) {
                    this._StagingData = _addkpigoalConfig;
                    this._kpiStagingData.next(this._StagingData.slice());
                    this._notificationservice.showSuccess('Configuration Saved successfully');
                    this._addGoalShow.next(true);
                    this._kpiUserShow.next(true);
                } else {
                    this._notificationservice.showError('Please check the configuration');
                }
            }
        });
    }

    GetKPIGoalConfigStagingDetails(_reqBody: any) {
        return this._kpiDataAccess.GetKPIGoalConfigStagingDetails(_reqBody).subscribe(res => {
            if (isTruthy(res)) {
                const _kpigoalConfig = jwtHelper.decodeToken(res.Data)['data'];
                this._kpiUserConfigData = [];
                if (_kpigoalConfig.length > 0) {
                    this._kpiUserConfigData = _kpigoalConfig;
                    this._kpiUserShow.next(true);
                    this._addGoalShow.next(true);
                    this._KpiUserGroupConfigTable.next(this._kpiUserConfigData.slice());
                    this._kpiStagingData.next(this._kpiUserConfigData.slice());

                } else {
                    this._notificationservice.showError('User(s) not available');
                    this._kpiUserShow.next(false);
                    this._addGoalShow.next(false);
                }
            }
        });
    }

}
