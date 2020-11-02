import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AppSettings } from '@mts-app-setting';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { EditDocumentFieldModel } from 'src/app/modules/documenttype/models/edit-document-field.model';
import { ManagerDoctypeService } from '../../services/manage-doctype.service';

@Component({
    selector: 'mts-editmfield',
    templateUrl: 'editmfield.page.html',
    styleUrls: ['editmfield.page.css'],
})
export class ManagerEditFieldComponent implements OnInit, OnDestroy {
    @ViewChild('confirmModal') confirmModal: ModalDirective;
    DocumentTypeVal: any = 0;
    DocumentTypeName = '';
    Rows: any = {};
    OverrideFieldID = 0;
    AssignedFieldName = '';
    SelectRow: EditDocumentFieldModel = new EditDocumentFieldModel();
    showHide: any = [false, false];
    promise: Subscription;
    constructor(private _mdocService: ManagerDoctypeService) {
        this.showHide = [false, true];
        this._mdocService.SetHideValue(this.showHide);
    }
    private subscription: Subscription[] = [];

    ngOnInit() {
        this.subscription.push(this._mdocService.EditDocFieldData.subscribe((res: any[]) => {
            this.Rows = res[0].Fields;
        }));
        this.subscription.push(this._mdocService.SelectedValues.subscribe((res: EditDocumentFieldModel) => {
            this.SelectRow = res;
        }));
        this.subscription.push(this._mdocService.DocumentTypeName.subscribe((res: any) => {
            this.DocumentTypeName = res;
        }));
        this.subscription.push(this._mdocService.ShowHide.subscribe((res: any) => {
            this.showHide = res;
        }));
        this._mdocService.GetSelectedRow();
        this._mdocService.getDocName();
    }
    EditDocumentTypeFieldSubmit() {
        const dTableRows = this.Rows;
        let sameField = { result: true, FieldID: 0, FieldDisplayName: '' };
        for (let index = 0; index < dTableRows.length; index++) {
            if (dTableRows[index].DocOrderByField === 'Desc') {
                sameField = { result: this.SelectRow.FieldID === dTableRows[index].FieldID, FieldID: dTableRows[index].FieldID, FieldDisplayName: dTableRows[index].DisplayName };
                break;
            }
        }
        if (sameField.result) {
            this.OverrideFieldID = sameField.FieldID;
            this.UpdateField();
        } else {
            this.OverrideFieldID = sameField.FieldID;
            this.AssignedFieldName = sameField.FieldDisplayName;
            this.UpdateField();
        }
    }

    UpdateField() {
        this.showHide = [true, false];
        this._mdocService.ShowHide.next(this.showHide);
        const req: any = { TableSchema: AppSettings.TenantSchema, Field: this.SelectRow, AssignedFieldID: this.OverrideFieldID };
        this._mdocService.Updatefield(req);

    }
    Hide() {
        this.showHide = [true, false];
        this._mdocService.ShowHide.next(this.showHide);
        this._mdocService._reloadTable.next(true);
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
