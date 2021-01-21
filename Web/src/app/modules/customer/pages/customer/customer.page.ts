import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { AppSettings, CustImportAssignTypeConstant, CustomerImportAssignTypeConstant } from '@mts-app-setting';
import { CustomerService } from '../../services/customer.service';
import { Subscription } from 'rxjs';
import { CustomerDatatableModel } from '../../models/customer-datatable.model';
import { DataTableDirective } from 'angular-datatables';
import { SessionHelper } from '@mts-app-session';
import { Router } from '@angular/router';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { convertDateTimewithTime } from '@mts-functions/convert-datetime.function';
import { CommonService } from 'src/app/shared/common';

@Component({
    selector: 'mts-customer',
    styleUrls: ['customer.page.css'],
    templateUrl: 'customer.page.html'
})
export class CustomerComponent implements OnInit, OnDestroy, AfterViewInit {
    dtOptions: any = {};
    @ViewChild(DataTableDirective, { static: false }) datatableEl: DataTableDirective;
    AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;
    promise: Subscription;
    showHide: any = [false, false, false];
    rowSelected = true;
    showCustomerImportMoniter = false;

    constructor(
        private _customerService: CustomerService,
        private _commonservice: CommonService,
        private _route: Router
    ) {
        this.checkPermission('AddBtn', 0);
        this.checkPermission('EditBtn', 1);
        this.checkPermission('ViewBtn', 2);
        this.checkPermission('SyncConfigBtn', 2);
    }

    private _subscriptions: Subscription[] = [];
    private dTable: any;

    ngOnInit(): void {
        this._subscriptions.push(this._customerService.customerMasterTable$.subscribe((res: CustomerDatatableModel[]) => {
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();
        }));
        this.dtOptions = {
            displayLength: 10,
            'select': {
                style: 'single',
                info: false
            },
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'CustomerID', mData: 'CustomerID', sClass: 'text-right', bVisible: false },
                { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sWidth: '40%' },
                { sTitle: AppSettings.AuthorityLabelSingular + ' Code', mData: 'CustomerCode', sWidth: '15%' },
                { sTitle: 'State', mData: 'State', sWidth: '15%' },
                { sTitle: 'Country', mData: 'Country', sWidth: '10%' },
                { sTitle: 'Zip Code', mData: 'ZipCode', sWidth: '10%' },
                { mData: 'ModifiedOn', sTitle: 'Modified On', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },

            ],
            aoColumnDefs: [{
                'aTargets': [7],
                'mRender': function (data, type, row) {
                    const statusFlag = data === true ? 'Active' : 'Inactive';
                    const statusColor = {
                        'true': 'label-success',
                        'false': 'label-danger'
                    };
                    return '<label class=\'label ' + statusColor[row['Active']] + ' label-table\'>' + statusFlag + '</label>';
                }
            },
                {
                    'aTargets': [6],
                    'mRender': function (data, type, row) {
                        return isTruthy(data) ? convertDateTimewithTime(data) : '';
                    }
                },

            ],
            rowCallback: (row: Node, data: CustomerDatatableModel, index: number) => {
                const self = this;
                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    self.setRowData(row, data);
                });
                return row;
            }
        };
    }

    checkPermission(component: string, index: number, ): void {
        const URL = 'View\\Customer\\' + component;
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

    ngAfterViewInit() {
        this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            this.promise = this._customerService.getCustomerMaster({ TableSchema: AppSettings.TenantSchema });
        });
    }

    ShowCustomerModal(modalType: number) {
        if (modalType === 0) {
            this._customerService.clearCurrentCustomer();
            this._route.navigate(['view/customer/addcustomer']);
        } else if (modalType === 1) {
            this._route.navigate(['view/customer/editcustomer']);
        } else if (modalType === 2) {
        this._commonservice.setCustomerImportType(CustomerImportAssignTypeConstant.CUSTOMER_IMPORT);
        this._commonservice.setCustImportType(CustImportAssignTypeConstant.CUSTOMER_IMPORT);
            this._route.navigate(['view/customer/import']);
        } else if (modalType === 3) {
            this._route.navigate(['view/customer/syncconfig']);
        }
    }

    setRowData(rowIndex: Node, rowData: CustomerDatatableModel): void {
        rowData.Type = 'Edit';
        this._customerService.setCurrectCustomer(rowData);
        this.rowSelected = $(rowIndex).hasClass('selected');
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
