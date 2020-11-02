import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NgDateRangePickerOptions, NgDateRangePickerComponent } from '@mts-daterangepicker/ng-daterangepicker.component';
import { NotificationService } from '@mts-notification';
import { AppSettings } from '@mts-app-setting';
import { DataTableDirective } from 'angular-datatables';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { KPIStatusConstant } from '@mts-status-constant';
import { KpiUserGroupConfigModel, AuditKpiGoalConfig, KPIGoalConfig } from '../../models/kpiuserconfig.model';
import { KpiGoalconfigService } from '../../services/kpi-goal-configs.service';
import { convertDateTime } from '@mts-functions/convert-datetime.function';

@Component({
    selector: 'mts-kpi-user-config',
    templateUrl: 'kpi-user-config.page.html',
    styleUrls: ['kpi-user-config.page.css'],

})
export class KpiUserConfigComponent implements OnInit, OnDestroy, AfterViewInit {
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    _kpiUserConfigData = new KpiUserGroupConfigModel();
    dTable: any = {};
    dtOptions: any = {};
    _kpiStagingData: any = [];
    rowSelected = true;
    constructor(private _notifyService: NotificationService,
        private _kpiGoalService: KpiGoalconfigService) {

    }
    private subscription: Subscription[] = [];

    ngOnInit() {
        this.subscription.push(this._kpiGoalService._KpiUserGroupConfigTable.subscribe((res: any) => {
            this._kpiUserConfigData = res;
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();

        }));
        this.subscription.push(this._kpiGoalService._kpiStagingData.subscribe((res: any) => {
            this._kpiStagingData = res;
            this.SetTableValue(this._kpiStagingData);
        }));

        this.dtOptions = {
            displayLength: 5,
            'select': {
                style: 'single',
                info: false,
                selector: 'td:not(:last-child)'
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Configuration Type', mData: 'ConfigType', sClass: 'text-center' },
                { sTitle: 'User Name', mData: 'UserName' },
                { sTitle: 'Goal', mData: 'Goal', sClass: 'text-center' },
                { sTitle: 'CreatedOn', 'type': 'date', mData: 'CreatedOn', bVisible: false },
            ],
            aoColumnDefs: [
                {
                    'aTargets': [3],
                    'mRender': function (date) {
                        if (isTruthy(date)) {
                            return convertDateTime(date);
                        } else {
                            return date;
                        }
                    }
                },
                {
                    'aTargets': [0],
                    'mRender': function (data) {
                        if (isTruthy(data)) {
                            if (data === 0) {
                                return 'No Config';
                            } else if (data === 1) {
                                return 'Daily';
                            } else if (data === 2) {
                                return 'Weekly';
                            } else if (data === 3) {
                                return 'Monthly';
                            } else if (data === 4) {
                                return 'Yearly';
                            }
                        } else {
                            return '';
                        }
                    }
                }
            ],
            rowCallback: (row: Node, data: KpiUserGroupConfigModel, index: number) => {
                const self = this;
                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    self.GetRowData(row, data);
                });
                return row;
            }
        };

    }
    ngAfterViewInit() {
        this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
            if (isTruthy(this.dTable)) {
                this.dTable = dtInstance;
            }
        });
    }
    GetRowData(rowIndex: Node, rowData: KpiUserGroupConfigModel): void {
        this.rowSelected = $(rowIndex).hasClass('selected');
    }

    SetTableValue(_data: any) {
        if (isTruthy(_data > 0)) {
            this.dTable.clear();
            this.dTable.rows.add(_data);
            this.dTable.draw();
        }
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
