import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionHelper } from '@mts-app-session';
import { AppSettings } from '@mts-app-setting';
import { CommonService } from 'src/app/shared/common';
import { Subscription } from 'rxjs';
import { DataTableDirective } from 'angular-datatables';
import { DocumentTypeService } from '../../services/documenttype.service';
import { DocumentTypeDatatableModel } from '../../models/document-type-datatable.model';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-documenttype',
    templateUrl: 'documenttype.page.html',
    styleUrls: ['documenttype.page.css'],
})
export class DocumentTypeComponent implements OnInit, AfterViewInit {

    showHide: any = [false, false, false];
    documentTypeListMaster: any = [];
    dtOptions: any = {};
    rowSelected = true;
    promise: Subscription;
    constructor(
        private _route: Router,
        private _commonservice: CommonService,
        private _documentTypeService: DocumentTypeService

    ) {
        this.checkPermission('AddDocumentType', 0);
        this.checkPermission('EditBtn', 1);
        this.checkPermission('ViewBtn', 2);

    }

    private subscription: Subscription;
    private dTable: any;
    @ViewChild(DataTableDirective, { static: false }) private datatableElement: DataTableDirective;

    ngOnInit() {
        this.subscription = this._commonservice.SystemDocumentTypeMaster.subscribe((res: any) => {
            this.dTable.clear();
            this.dTable.rows.add(res);
            this.dTable.draw();

        });

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
                { sTitle: 'Document Name', mData: 'Name', sWidth: '30%' },
                { sTitle: 'Document Display Name', mData: 'DisplayName', sWidth: '30%' },
                { sTitle: 'EFolder Name', mData: 'ParkingSpotName', sWidth: '38%' },
                { sTitle: 'Document Level', mData: 'DocumentLevel', sClass: 'text-center', sWidth: '14%', bVisible: false },
                { sTitle: 'Active/Inactive', mData: 'Active', sClass: 'text-center', sWidth: '10%' },
                { mData: 'DocumentLevel', bVisible: false },
                { mData: 'ParkingSpotID', bVisible: false },
            ],
            aoColumnDefs: [{
                'aTargets': [5],
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
                'aTargets': [4],
                'mRender': function (data, type, row) {
                    return '<label class=\'label ' + AppSettings.DocumentCriticalStatus[data].Color + ' label-table\'>' + AppSettings.DocumentCriticalStatus[data].Label + '</label></td>';
                }
            }],
            rowCallback: (row: Node, data: DocumentTypeDatatableModel, index: number) => {
                const self = this;
                $('td', row).unbind('click');
                $('td', row).bind('click', () => {
                    return self.getDocumentTypeRowData(row, data);
                });
                return row;
            }

        };
    }

    ngAfterViewInit() {
        this.datatableElement.dtInstance.then((dtInstance: DataTables.Api) => {
            this.dTable = dtInstance;
           this.promise = this._commonservice.GetSystemDocumentTypes();
        });
    }

    getDocumentTypeRowData(rowIndex: Node, rowData: DocumentTypeDatatableModel): void {
        this.rowSelected = $(rowIndex).hasClass('selected');
        this._documentTypeService.SetDocumentTypeRowData(rowData);

    }
    ShowDocumentTypeModal(modalType: number) {
     if (modalType === 0) {
            this._route.navigate(['view/documenttype/adddocumenttype']);
        } else if (modalType === 1) {
            this._route.navigate(['view/documenttype/editdocumenttype']);
        } else if (modalType === 2) {
            this._route.navigate(['view/documenttype/viewdocumenttype']);
        }
    }

    checkPermission(component: string, index: number, ): void {
        const URL = 'View\\DocumentType\\' + component;
        const AccessCheck = false;
        const AccessUrls = SessionHelper.RoleDetails.URLs;
        if (isTruthy(AccessCheck)) {
            AccessUrls.forEach(element => {
                if (element.URL === URL) {
                    this.showHide[index] = true;
                    return false;
                }
            });
        }
    }

    ngDestroy() {
        this.subscription.unsubscribe();
    }

}
