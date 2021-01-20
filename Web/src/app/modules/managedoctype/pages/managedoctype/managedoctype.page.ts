import { Component, ViewChild, AfterViewInit, OnInit, OnDestroy } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { Subscription } from 'rxjs';
import { CommonService } from 'src/app/shared/common';
import { AppSettings } from '@mts-app-setting';
import { ManagerDoctypeService } from '../../services/manage-doctype.service';
import { ManagerDocumentsDatatableModel } from '../../models/manager-documents-table.model';
import { SessionHelper } from '@mts-app-session';
import { Router } from '@angular/router';
import { ValidateModel } from '../../models/validate.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';

@Component({
    selector: 'mts-managedoctype',
    templateUrl: 'managedoctype.page.html',
    styleUrls: ['managedoctype.page.css'],
})
export class ManagerDocumentTypeComponent implements OnInit, AfterViewInit, OnDestroy {
    dtOptions: any = {};
    rowSelected = true;
    syncEnable = false;
    promise: Subscription;
    addStatus: any = [];
    CustomerItems: any = [];
    LoanTypeItems: any = [];
    CustomerID: any;
    LoanTypeID: any;
    LoanTypevalue: any = 0;
    Customervalue: any = 0;
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    showHide: any = [false, false, false];
    IsTableShow = true;
    isloading = false;
    click: boolean;
    constructor(private _commonservice: CommonService,
        private _managerDocservice: ManagerDoctypeService,
        private _route: Router,
        private _notificationService: NotificationService
    ) {
        const StatusKeys = Object.keys(AppSettings.DocumentCriticalStatus);

        for (let index = 0; index < StatusKeys.length; index++) {
            const val = AppSettings.DocumentCriticalStatus[StatusKeys[index]];
            this.addStatus.push({ Label: val.Label, Value: val.Value });
        }
        this.checkPermission('EditBtn', 0);
        this.checkPermission('ViewBtn', 1);
    }

    private subscription: Subscription[] = [];
    private dTable: any;
    @ViewChild(DataTableDirective, { static: false }) private datatableElement: DataTableDirective;

    private checkedOnce = false;
    private checkedLoanTypeOnce = false;

    checkPermission(component: string, index: number, ): void {
        const URL = 'View\\ManagerDocumentType\\' + component;
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
    ngOnInit() {
        this._commonservice.GetActiveCustomerList(AppSettings.TenantSchema);
        this.subscription.push(this._commonservice.SystemActiveCustomerMaster.subscribe((res: any) => {
            this.CustomerItems = res;
        }));
        this.subscription.push(this._managerDocservice._managerDocData.subscribe((res: any) => {
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();

        }));

        this.subscription.push(this._managerDocservice.LoanTypeItems.subscribe((res: any) => {
            this.LoanTypeItems = res;
        }));

        this.dtOptions = {
            displayLength: 10,
            aaData: [],
            'iDisplayLength': 10,
            'select': {
                style: 'single',
                info: false
            },
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'DocumentTypeID', mData: 'DocumentTypeID', sClass: 'text-right', bVisible: false },
                { sTitle: 'Document Name', mData: 'Name', sWidth: '38%' },
                { sTitle: 'Document Display Name', mData: 'DisplayName', sWidth: '38%' },
                { sTitle: 'Document Level', mData: 'DocumentLevel', sClass: 'text-center', sWidth: '14%', bVisible: false },
                { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
                { sTitle: AppSettings.AuthorityLabelSingular + ' ID', mData: 'CustomerID', bVisible: false },
                { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sClass: 'text-center', sWidth: '38%' },
                { sTitle: 'LoanType ID', mData: 'LoanTypeID', bVisible: false },
                { sTitle: 'LoanType Name', mData: 'LoanTypeName', sClass: 'text-center', sWidth: '38%' },
                { mData: 'DocumentLevel', bVisible: false }
            ],
            aoColumnDefs: [{
                'aTargets': [4],
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
                'aTargets': [3],
                'mRender': function (data, type, row) {
                    return '<label class=\'label ' + AppSettings.DocumentCriticalStatus[data].Color + ' label-table\'>' + AppSettings.DocumentCriticalStatus[data].Label + '</label></td>';
                }
            }],
            rowCallback: (row: Node, data: ManagerDocumentsDatatableModel, index: number) => {
                const self = this;
                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    self.getRowData(row, data);
                });
                return row;
            }

        };
    }
    getRowData(rowIndex: Node, rowData: ManagerDocumentsDatatableModel): void {
        this.rowSelected = $(rowIndex).hasClass('selected');
        this._managerDocservice.SetDocumentTypeRowData(rowData);

    }
    ShowDocumentTypeModal(modalType: number) {
        if (modalType === 1) {
            this._route.navigate(['view/mdocumenttype/manageeditdoctype']);
        }
        if (modalType === 2) {
            this._route.navigate(['view/mdocumenttype/manageviewdoctype']);
        }
    }
    GetManagerDoctypes() {
        if (this.Customervalue === 0 && this.LoanTypevalue === 0 && this._managerDocservice.loading) {
            this.Customervalue = this._managerDocservice.CustomerID;
            this.LoanTypevalue = this._managerDocservice.LoanTypeID;
            if (this.Customervalue !== 0 || this.Customervalue !== undefined) {
                this.OnCustomerChange(this.Customervalue);

            }
        }
        const inputData: ValidateModel = { TableSchema: AppSettings.TenantSchema, CustomerID: this.Customervalue, LoanTypeID: this.LoanTypevalue };
        if ((this.Customervalue === undefined || this.Customervalue === 0 || this.Customervalue === '0') && (this.LoanTypevalue === undefined || this.LoanTypevalue === 0 || this.LoanTypevalue === '0' ) && (this.click !== undefined)) {
            this.promise = this._managerDocservice.GetManagerDoctypes(inputData);
            this._notificationService.showError('Select Customer');
            this.syncEnable = false;
        } else if ((this.LoanTypevalue === undefined || this.LoanTypevalue === 0 || this.LoanTypevalue === '0') && (this.Customervalue !== '0' || this.Customervalue !== undefined || this.Customervalue !== 0)  && (this.click !== undefined)) {
            this.promise = this._managerDocservice.GetManagerDoctypes(inputData);
            this._notificationService.showError('Select LoanType');
            this.syncEnable = false;

        } else if ((this.LoanTypevalue !== undefined || this.LoanTypevalue !== '0') && (this.Customervalue !== '0' || this.Customervalue !== undefined)) {
            this.promise = this._managerDocservice.GetManagerDoctypes(inputData);
            this.syncEnable = true;
        }
    }
    SyncLoantype() {
        if (this.LoanTypevalue !== 0 && this.Customervalue !== 0) {
            const req: any = { TableSchema: AppSettings.TenantSchema, LoanTypeID: this._managerDocservice.LoanTypeID };
            this._managerDocservice.SyncRetainUpdateStagings(req);
        }

    }
    CheckTrigger() {
        this.click = true;
        this._managerDocservice.setClick(true);
    }
    OnCustomerChange(val: any) {
        this.CustomerID = val;
        const req: any = { TableSchema: AppSettings.TenantSchema, CustomerID: this.CustomerID };
        this._managerDocservice.GetLoantypeForCustomer(req);

    }
    ngAfterViewInit() {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            this.GetManagerDoctypes();
        });
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
