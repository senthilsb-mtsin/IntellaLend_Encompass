import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AppSettings } from '@mts-app-setting';
import { ResetExpiredPassword } from '../../models/reset-expired-password.model';
import { UserDatatableModel } from '../../models/user-datatable.model';
import { DataTableDirective } from 'angular-datatables';
import { SessionHelper } from '@mts-app-session';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { convertDateTime } from '@mts-functions/convert-datetime.function';
import { GetSchemaRequest } from '../../models/get-schema-request..model';

@Component({
    selector: 'mts-user',
    templateUrl: 'user.page.html',
    styleUrls: ['user.page.css'],
})

export class UserComponent implements OnInit, OnDestroy, AfterViewInit {

    UserData = new UserDatatableModel();
    showHide: any = [false, false, false];
    dtOptions: any = {};
    rowSelected = true;
    promise: Subscription;
    constructor(
        private _route: Router,
        private _userService: UserService

    ) {
        this.checkPermission('AddBtn', 0);
        this.checkPermission('EditBtn', 1);
        this.checkPermission('ViewBtn', 2);
    }

    private dTable: any;
    @ViewChild(DataTableDirective, { static: false }) private datatableElement: DataTableDirective;
    private subscription: Subscription[] = [];

    ngOnInit() {

        this.subscription.push(this._userService.setUserTableData.subscribe((res: any) => {
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();
        }));

        this.dtOptions = {
            displayLength: 10,
            aaData: [],
            'select': {
                style: 'single',
                info: false,
                selector: 'td:not(:last-child)'
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'UserID', mData: 'UserID', bVisible: false },
                { sTitle: 'Email ID', mData: 'UserName', sWidth: '18%' },
                { sTitle: 'Created Date', 'type': 'date', mData: 'CreatedOn', sClass: 'text-center', sWidth: '12%' },
                { sTitle: 'First Name', mData: 'FirstName', sWidth: '25%' },
                { sTitle: 'LastModified', 'type': 'date', mData: 'LastModified', bVisible: false },
                { sTitle: 'Last Name', mData: 'LastName', sWidth: '25%' },
                { sTitle: 'Locked', mData: 'Locked', sWidth: '8%' },
                { sTitle: 'MiddleName', mData: 'MiddleName', bVisible: false },
                { sTitle: 'Password', mData: 'Password', bVisible: false },
                { sTitle: 'Status', mData: 'Status', sClass: 'text-right', bVisible: false },
                { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'Reset Password', mData: 'UserID', sClass: 'text-center', sWidth: '10%' }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [2, 4],
                    'mRender': function (date) {
                        if (date !== null && date !== '') {
                            return convertDateTime(date);
                        } else {
                            return date;
                        }
                    }
                },
                {
                    'aTargets': [10],
                    'mRender': function (data, type, row) {

                        let statusFlag = '';
                        if (data === true) {
                            statusFlag = 'Active';
                        } else {
                            statusFlag = 'Inactive';
                        }
                        const statusColor = {
                            'true': 'label-success',
                            'false': 'label-danger'
                        };

                        return '<label class=\'label ' + statusColor[row['Active']] + ' label-table\'>' + statusFlag + '</label></td>';
                    }
                },
                {
                    'aTargets': [11],
                    'mRender': function (data, type, row) {
                        if (data !== null) {
                            return '<span style="cursor:pointer" class="material-icons resetpwd">settings_backup_restore</span>';
                        }
                        return '<a></a>';
                    }
                }
            ],
            rowCallback: (row: Node, data: UserDatatableModel, index: number) => {
                const self = this;
                $('td .resetpwd', row).unbind('click');
                $('td .resetpwd', row).bind('click', () => {
                    self.ResetExpiredPassword(row, data);
                });

                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    self.getRowData(row, data);
                });
                return row;
            }
        };
    }

    ngAfterViewInit() {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            if (isTruthy(this.dTable)) {
               this.getUserList();
            }
        });
    }

    getRowData(rowIndex: Node, rowData: UserDatatableModel): void {
        this.rowSelected = $(rowIndex).hasClass('selected');
        this._userService.SetRowData(rowData);
    }

    ResetExpiredPassword(row: Node, rowData: UserDatatableModel): void {
        const input = new ResetExpiredPassword(AppSettings.TenantSchema, rowData.UserName);
        this._userService.ResetExpiredPassword(input);
    }

    ShowUserModal(modalType: number) {
        if (modalType === 0) {
            this._route.navigate(['view/user/adduser']);
        } else if (modalType === 1) {
            this._route.navigate(['view/user/edituser']);
        } else if (modalType === 2) {
            this._route.navigate(['view/user/viewuser']);
        }
    }

    getUserList() {
        const input = new GetSchemaRequest(
            AppSettings.TenantSchema,
        );
      this.promise = this._userService.GetUserList(input);
    }

    checkPermission(component: string, index: number): void {
        const URL = 'View\\User\\' + component;
        const AccessCheck = false;
        const AccessUrls = SessionHelper.RoleDetails.URLs;
        if (AccessCheck !== null) {
            AccessUrls.forEach(element => {
                if (element.URL === URL) {
                    this.showHide[index] = true;
                    return false;
                }
            });
        }
    }

    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
