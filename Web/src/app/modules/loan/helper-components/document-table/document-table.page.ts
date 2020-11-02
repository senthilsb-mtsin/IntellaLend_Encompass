import { Component, ViewChild, OnDestroy, OnInit } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { SessionHelper } from '@mts-app-session';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';

@Component({
    selector: 'mts-loan-document-table',
    templateUrl: 'document-table.page.html',
    styleUrls: ['document-table.page.scss']
})
export class LoanDocumentTableComponent implements OnInit, OnDestroy {
    fieldFrmGrp: FormGroup;
    showHide: boolean[] = [false, false, false];
    _docTables: any[] = [];

    constructor(
        private _loanInfoService: LoanInfoService,
        private fb: FormBuilder
    ) {
        this.checkPermission('ReadonlyLoans', 0);
        this.createForm();
    }

    private _subscriptions: Subscription[] = [];
    private _currentForm: any;

    ngOnInit(): void {
        this._subscriptions.push(this._loanInfoService.SetDocField$.subscribe((res: { DocLevelFields: any, TempDocTables: any }) => {
            if (res !== null) {
                this.setFormFields(res.TempDocTables);
            }
        }));
    }

    AddLineItems(val: any) {
        // RowCoordinates
        const actualTableLength = this.fieldFrmGrp.controls.datatables['controls']['length'];
        this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'].push(this.fb.group({ columns: this.fb.array([]), RowCoordinates: new FormControl({ x0: 0, y0: 0, x1: 0, y1: 0 }) }));
        const len = this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'].length;
        const length = this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'][0].controls.columns.controls.length;
        const headerColumnsLength = this.fieldFrmGrp.controls.datatables['controls'][val].controls[0].value.HeaderRow.HeaderColumns.length;
        if (actualTableLength === 0) {
            for (let i = 0; i < len; i++) {
                if (this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'][i].controls.columns.controls.length === 0) {
                    if (length > 0) {
                        for (let j = 0; j < length; j++) {
                            this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'][i].controls.columns['controls'].push(this.fb.group({ Confidence: '', CoordinatesList: '', Name: '', Page: '', Type: '', Value: '' }));
                        }
                    } else {

                        for (let k = 0; k < headerColumnsLength; k++) {
                            this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'][i].controls.columns['controls'].push(this.fb.group({ Confidence: '', CoordinatesList: '', Name: '', Page: '', Type: '', Value: '' }));
                        }
                    }
                }
            }
        } else {
            for (let t = 0; t < len; t++) {
                if (length > 0) {
                    for (let tr = 0; tr < length; tr++) {
                        if (this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows.controls[t].controls.columns.controls.length !== this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows.controls[0].controls.columns.controls.length) {
                            this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'][t].controls.columns['controls'].push(this.fb.group({ Confidence: '', CoordinatesList: '', Name: '', Page: '', Type: '', Value: '' }));
                        }
                    }
                } else {
                    for (let k = 0; k < headerColumnsLength; k++) {
                        this.fieldFrmGrp.controls.datatables['controls'][val].value[0].Rows['controls'][t].controls.columns['controls'].push(this.fb.group({ Confidence: '', CoordinatesList: '', Name: '', Page: '', Type: '', Value: '' }));
                    }
                }
            }
        }
    }

    DeleteLineItem(tbl: any, vals: any) {
        this.fieldFrmGrp.controls.datatables['controls'][tbl].value[0].Rows['controls'].splice(vals, 1);
    }

    setFormFields(TempDocTables) {
        if (TempDocTables.length > 0) {
            const docTableFGs = TempDocTables.map(docTable => {
                return this.createTableGroup(docTable);
            });

            const docTableFormArray = this.fb.array(docTableFGs);
            this.fieldFrmGrp.setControl('datatables', docTableFormArray);
        } else {
            this.fieldFrmGrp.setControl('datatables', this.fb.array([]));
        }

        this.fieldFrmGrp.enable();
        if (this.showHide[0]) {
            this.fieldFrmGrp.disable();
        }

        this._docTables = TempDocTables;
    }

    createTableGroup(table): FormGroup {
        const rowGroups = table.Rows.map(r => {
            return this.fb.group({ columns: this.fb.array(r.RowColumns.map(c => this.fb.group(c))), RowCoordinates: r.RowCoordinates });
        });
        return this.fb.group([{
            Name: table.Name,
            HeaderRow: table.HeaderRow,
            Rows: this.fb.array(rowGroups)
        }]);
    }

    createForm() {
        this.fieldFrmGrp = this.fb.group({
            datatables: this.fb.array([this.createItem()])
        });

        this.fieldFrmGrp.valueChanges.subscribe((form) => {
            this._loanInfoService.SetCurrentForm(form, 'datatables');
        });
    }

    createItem(): FormGroup {
        return this.fb.group({
            Name: '',
            HeaderRow: { HeaderColumns: [{ Name: '' }] },
            Rows: this.fb.array([
                this.fb.group({
                    columns: this.fb.array([])
                })
            ])
        });
    }

    checkPermission(component: string, index: number): void {
        let URL = '';
        if (index === 1 || index === 2) {
            URL = 'View\\LoanSearch\\LoanInfo\\' + component;
        } else {
            URL = 'View\\LoanDetails\\' + component;
        }

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
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }

}
