import { Component, OnInit, ViewChild } from '@angular/core';
import { EditDocumentFieldModel } from '../../../models/edit-document-field.model';
import { Subscription } from 'rxjs';
import { DocumentFieldService } from '../../../services/documentfield.service';
import { AppSettings } from '@mts-app-setting';
import { DocumentTypeService } from '../../../services/documenttype.service';

@Component({
    selector: 'mts-editfield',
    templateUrl: 'editfield.page.html',
    styleUrls: ['editfield.page.css'],
})
export class EditFieldComponent implements OnInit  {
    DocumentTypeVal: any = 0;
    DocumentTypeName = '';
    Rows: any = {};
    OverrideFieldID = 0;
    AssignedFieldName = '';
    SelectRow: EditDocumentFieldModel = new EditDocumentFieldModel();
    showHide: any = [false, false];
    promise: Subscription;
    constructor(private _docFieldservice: DocumentFieldService, private _docTypeService: DocumentTypeService) {
        this.showHide = [false, true];
        this._docFieldservice.SetHideValue(this.showHide);
    }
    private subscription: Subscription[] = [];

    ngOnInit() {
        this.subscription.push(this._docFieldservice.EditDocFieldData.subscribe((res: any[]) => {
          this.Rows = res[0].Fields;
        }));
        this.subscription.push(this._docFieldservice.SelectedValues.subscribe((res: EditDocumentFieldModel) => {
            this.SelectRow = res;
        }));
        this.subscription.push(this._docTypeService.DocumentTypeName.subscribe((res: any) => {
            this.DocumentTypeName = res;
        }));
        this.subscription.push(this._docFieldservice.ShowHide.subscribe((res: any) => {
            this.showHide = res;
        }));
        this._docFieldservice.GetSelectedRow();
        this._docTypeService.getDocName();
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
            this.AssignedFieldName = sameField.FieldDisplayName;
            this.UpdateField();
        } else {
            this.OverrideFieldID = sameField.FieldID;
            this.AssignedFieldName = sameField.FieldDisplayName;
            this.UpdateField();

        }
    }
    UpdateField() {
        this.showHide = [true, false];
        this._docFieldservice.ShowHide.next(this.showHide);
        const req: any = { TableSchema: AppSettings.SystemSchema, Field: this.SelectRow, AssignedFieldID: this.OverrideFieldID };
        this._docFieldservice.Updatefield(req);

    }
    Hide() {
        this.showHide = [true, false];
        this._docFieldservice.SetHideValue(this.showHide);
        this._docFieldservice._reloadTable.next();
    }
    ngDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
