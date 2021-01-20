import { Component, ViewChild, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataTableDirective } from 'angular-datatables';
import { Router } from '@angular/router';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { EditDocumentFieldModel } from 'src/app/modules/documenttype/models/edit-document-field.model';
import { ManagerDoctypeService } from '../../services/manage-doctype.service';

@Component({
    selector: 'mts-editmdocfield',
    templateUrl: 'editmdocfield.page.html',
    styleUrls: ['editmdocfield.page.css'],
})
export class ManagerEditDocumentFieldComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild('confirmModal') confirmModal: ModalDirective;
    @ViewChild(DataTableDirective) datatableEl: DataTableDirective;
    SelectRow: EditDocumentFieldModel = new EditDocumentFieldModel();
    isTitleEnabled = true;
    isEditEnabled = false;
    DocumentTypeName = '';
    docTypeItems: any = [];
    _editDocFieldData: any = [];
    dTable: any = {};
    dtOptions: any = {};
    showHide: any = [false, false];
    _reLoad: any = false;
    promise: Subscription;
    constructor(private _mdocService: ManagerDoctypeService, private _route: Router) {
        this.LoadDoctypes();
        this.showHide = [true, false];
        this._mdocService.SetHideValue(this.showHide);
    }
    private subscription: Subscription[] = [];
    ngOnInit() {
        this.subscription.push(this._mdocService.Doctypes.subscribe((res: any) => {
            this.docTypeItems = res;
        }));
        this.subscription.push(this._mdocService.ShowHide.subscribe((res: any) => {
            this.showHide = res;
        }));
        this.subscription.push(this._mdocService._reloadTable.subscribe((res: any) => {
            this.GetDocFieldData();
        }));

        this.dtOptions = {
            displayLength: 5,
            aaData: [],
            'select': false,
            'iDisplayLength': 10,
            'aLengthMenu': [[5, 10, 25, 50, -1], [5, 10, 25, 50, 'All']],
            aoColumns: [
                { sTitle: 'Field Name', mData: 'Name' },
                { sTitle: 'Field Display Name', mData: 'DisplayName' },
                { sTitle: 'Active', mData: 'Active' },
                { sTitle: 'Edit', mData: 'DocumentTypeID', sClass: 'text-center', bVisible: true },
                { sTitle: '', mData: 'FieldID', bVisible: false }
            ],
            aoColumnDefs: [
                {
                    'aTargets': [2],
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
                        return '<span style=\'cursor: pointer;\' class=\'editField material-icons txt-warm\'>rate_review</span>';
                    }
                }

            ],
            rowCallback: (row: Node, data: EditDocumentFieldModel, index: number) => {
                const self = this;
                $('td .editField', row).unbind('click');
                $('td .editField', row).bind('click', () => {
                    self.EditDocumentTypeField(data);
                });
                return row;
            }

        };
        this.GetDocFieldData();

    }
    ngAfterViewInit() {
        this.subscription.push(this._mdocService.EditDocFieldData.subscribe((res: any) => {
            this.datatableEl.dtInstance.then((dtInstance: DataTables.Api) => {
                this.dTable = dtInstance;
                this.dTable.clear();
                this.dTable.rows.add(res[0].Fields);
                this.dTable.draw();
            });
        }));
    }
    LoadDoctypes() {
        const req: any = { TableSchema: AppSettings.SystemSchema };
        this.docTypeItems = [];
        this._mdocService.LoadDoctypes(req);
    }
    EditDocumentTypeField(data: EditDocumentFieldModel) {
  this.isEditEnabled = true;
        this.showHide = [false, true];
        this._mdocService.SetHideValue(this.showHide);
        this.docTypeItems.forEach(element => {
            if (element.id === this._mdocService._manageDocumentTypeRowData.DocumentTypeID) {
                this.DocumentTypeName = element.text;
                this._mdocService.SetDocName(this.DocumentTypeName);
                return;
            }
        });
        this.SelectRow = data;
        this.SelectRow.OrderBy = false;
        if (data.DocOrderByField === 'Desc') {
            this.SelectRow.OrderBy = true;
        }
        this._mdocService.SetSelectedRow(this.SelectRow);
    }

    SetTableRowData() {
        const dTableRows = this.dTable.rows().data();
        this._mdocService.SetTableRows(dTableRows);
    }

    GetDocFieldData() {
        let docid = [];
        docid = [this._mdocService._manageDocumentTypeRowData.DocumentTypeID];
        const inputData: any = { TableSchema: AppSettings.TenantSchema, DocumentTypeID: docid };
        this.promise = this._mdocService.GetDocFieldData(inputData);

    }
    ReloadTableEmit(e) {
        if (e) {
            this.GetDocFieldData();
            this.dTable.clear();
            this.dTable.rows.add(this._editDocFieldData[0].Fields);
            this.dTable.draw();
        }
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
