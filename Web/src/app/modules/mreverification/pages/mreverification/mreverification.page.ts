import { Component, ViewChild, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { AppSettings } from '@mts-app-setting';
import { ManagerReverificationdetailsModel } from '../../models/manager-reverification-details.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { ReverificationService } from 'src/app/modules/reverification/services/reverification.service';
import { ManagerReverificationService } from '../../services/manager-reverification.service';
import { SessionHelper } from '@mts-app-session';

@Component({
    selector: 'mts-mreverification',
    templateUrl: 'mreverification.page.html',
    styleUrls: ['mreverification.page.css']
})
export class ManagerReverificationComponent implements OnInit, AfterViewInit, OnDestroy {

    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    rowSelected = true;
    dtOptions: any = {};
    selectedRow: any = {};
    dTable: any = {};
    showHide: any = [false, false, false];
    promise: Subscription;

    constructor(private _managerReverificationService: ManagerReverificationService, private _route: Router) {
   this.checkPermission('EditManagerReverification', 1);
        this.checkPermission('ViewManagerReverification', 2);
    }
    private subscription: Subscription[] = [];

    checkPermission(component: string, index: number, ): void {
        const URL = 'View\\ManagerReverification\\' + component;
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
    ngOnInit(): void {
        this.subscription.push(this._managerReverificationService.setManagerReverificationTableData.subscribe((res: any) => {
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
                { sTitle: 'Re-verificationID', mData: 'ReverificationID', bVisible: false },
                { sTitle: 'LoanTypeID', mData: 'LoanTypeID', bVisible: false },
                { sTitle: 'CustomerID', mData: 'CustomerID', bVisible: false },
                { sTitle: AppSettings.AuthorityLabelSingular + ' Name', mData: 'CustomerName', sWidth: '30%' },
                { sTitle: 'LoanType Name', mData: 'LoanTypeName', sWidth: '30%' },
                { sTitle: 'Re-verification Name', mData: 'ReverificationName', sWidth: '30%' },
                { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'MappingID', mData: 'MappingID', bVisible: false },
                { sTitle: 'TemplateID', mData: 'TemplateID', bVisible: false },
                { sTitle: 'TemplateFields', mData: 'TemplateFields', bVisible: false }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [6],
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
                }
            ],
            rowCallback: (row: Node, data: ManagerReverificationdetailsModel, index: number) => {
                const self = this;
                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    self.getRowData(row, data);
                });
                return row;
            }
        };
    }
    getRowData(rowIndex: Node, rowData: ManagerReverificationdetailsModel): void {
        this.rowSelected = $(rowIndex).hasClass('selected');
        this._managerReverificationService.SetManagerReverifyRowData(rowData);

    }
    ngAfterViewInit() {
        this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            if (isTruthy(this.dTable)) {
                this.GetManagerReverification();
            }
        });
    }
    GetManagerReverification() {
        const input = { TableSchema: AppSettings.TenantSchema };
        this.promise = this._managerReverificationService.GetManagerReverification(input);
    }
    ShowReverificationModal(modalType: number) {
        if (modalType === 1) {
            this._route.navigate(['view/mreverification/editmreverification']);
        } else if (modalType === 2) {
            this._route.navigate(['view/mreverification/viewmreverification']);
        }
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
