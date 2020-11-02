import { Component, ViewChild, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionHelper } from '@mts-app-session';
import { AppSettings } from '@mts-app-setting';
import { CommonService } from 'src/app/shared/common';
import { Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { ReverificationService } from '../../services/reverification.service';
import { ReverificationdetailsModel } from '../../models/reverification-details.model';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-reverification',
    templateUrl: 'reverification.page.html',
    styleUrls: ['reverification.page.css'],
})
export class ReverificationComponent implements OnInit, OnDestroy, AfterViewInit {
    LoanTypeMaster: any = [];
    TemplateMaster: any = [];
    showHide: any = [false, false, false];
    documentTypeListMaster: any = [];
    dtOptions: any = {};
    rowSelected = true;
    dTable: any;
    promise: Subscription;

    constructor(
        private _route: Router,
        private _commonservice: CommonService,
        private _reverificationService: ReverificationService

    ) {
        this.checkPermission('AddDocumentType', 0);
        this.checkPermission('EditBtn', 1);
        this.checkPermission('ViewBtn', 2);

    }
    @ViewChild(DataTableDirective, { static: false }) private datatableElement: DataTableDirective;
    private subscription: Subscription[] = [];

    ngOnInit() {
        this.subscription.push(this._reverificationService.setReverificationTableData.subscribe((res: any) => {
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
            aaData: [],
            aoColumns: [
                { sTitle: 'Re-verificationID', mData: 'ReverificationID', bVisible: false },
                { sTitle: 'LoanTypeID', mData: 'LoanTypeID', bVisible: false },
                { sTitle: 'LoanType Name', mData: 'LoanTypeName', sWidth: '45%' },
                { sTitle: 'Re-verification Name', mData: 'ReverificationName', sWidth: '45%' },
                { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
                { sTitle: 'MappingID', mData: 'MappingID', bVisible: false },
                { sTitle: 'TemplateID', mData: 'TemplateID', bVisible: false },
                { sTitle: 'TemplateFields', mData: 'TemplateFields', bVisible: false }
            ],
            aoColumnDefs: [
                {
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
                }
            ],
            rowCallback: (row: Node, data: ReverificationdetailsModel, index: number) => {
                const self = this;
                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    self.getRowData(row, data);
                });
                return row;
            }
        };

    }
    getRowData(rowIndex: Node, rowData: ReverificationdetailsModel): void {
        this.rowSelected = $(rowIndex).hasClass('selected');
        this._reverificationService.SetRowData(rowData);

    }

    ngAfterViewInit() {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
            if (isTruthy(this.dTable)) {
                this.GetReverification();
            }
        });
    }

    GetReverification() {
        this.promise = this._reverificationService.GetReverification();
    }

    ShowReverificationModal(modalType: number) {
        if (modalType === 0) {
            this._route.navigate(['view/reverification/addreverification']);
        } else if (modalType === 1) {
            this._route.navigate(['view/reverification/editreverification']);
        } else if (modalType === 2) {
            this._route.navigate(['view/reverification/viewreverification']);
        }
    }

    checkPermission(component: string, index: number, ): void {
        const URL = 'View\\Reverification\\' + component;
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
