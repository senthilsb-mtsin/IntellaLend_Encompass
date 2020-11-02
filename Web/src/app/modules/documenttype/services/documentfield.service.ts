import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject, Subscription } from 'rxjs';
import { AddDocumentFieldRequestModel } from '../models/adddocument-field-request.model';
import { DocumentDataAccess } from '../documenttype.data';
import { NotificationService } from '@mts-notification';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Location } from '@angular/common';
import { EditDocumentFieldModel } from '../models/edit-document-field.model';
import { analyzeAndValidateNgModules } from '@angular/compiler';
import { AppSettings } from '@mts-app-setting';

const jwtHelper = new JwtHelperService();

@Injectable()

export class DocumentFieldService {
  _reloadTable = new Subject<any>();
  FieldId = new Subject<any>();
  setDocFieldTableData = new Subject<any>();
  DeleteConfirmation = new Subject<any>();
  EditDocFieldData = new Subject<any>();
  Doctypes = new Subject<any>();
  DRows = new Subject<any>();
  SelectedValues = new Subject<any>();
  ShowHide = new Subject<any[]>();
  IsValidate = false;
  _docTypes: any[] = [];
  _docfields: any[] = [];
  DocumentTypeName = new Subject<any>();
  constructor(private _documentDataAccess: DocumentDataAccess, private location: Location, private _notificationService: NotificationService) {

  }
  private slectRowData: EditDocumentFieldModel = new EditDocumentFieldModel();
  private DatatableRows: any = {};

  private _docname: any = '';
  AddField(_req: AddDocumentFieldRequestModel) {
    if (!this.validate(_req)) {
      return this._documentDataAccess
        .AddDocumentField(_req)
        .subscribe((res) => {
          if (isTruthy(res)) {
            const _docFieldID = jwtHelper.decodeToken(res.Data)[
              'data'
            ];
            this.FieldId.next(_docFieldID);
            this._notificationService.showSuccess('Field Added Successfully');
          } else {
            this._notificationService.showError('Field Add Failed');
          }
        });
    }
  }
  SetSelectedRow(Values: EditDocumentFieldModel) {
    this.slectRowData = Values;
    this.SelectedValues.next(this.slectRowData);

  }
  GetSelectedRow() {
    this.SelectedValues.next(this.slectRowData);
  }

  GotoPrevious() {
    this.location.back();
  }
  DeleteField(req: any) {
    return this._documentDataAccess
      .DeleteDocumentField(req)
      .subscribe((res) => {
        if (isTruthy(res)) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result === true) {
            this.DeleteConfirmation.next(result);
          } else {
            this._notificationService.showError('Field Remove Failed');
          }
        }
      });
  }
  SetDocName(docName: any) {
    this._docname = docName;
  }
  getDocName() {
    this.DocumentTypeName.next(this._docname);
  }
  GetDocFieldData(_req: any) {
    return this._documentDataAccess.GetDocumentFields(_req)
      .subscribe(res => {
        if (isTruthy(res)) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data.length > 0) {
            this._docfields = data;
            this.EditDocFieldData.next(data.slice());
          }
        }
      });
  }
  Updatefield(_req: any) {
    if (!this.validate(_req)) {
      return this._documentDataAccess.UpdateDocumentField(_req).subscribe(res => {
        if (isTruthy(res)) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
            this._reloadTable.next(true);
            this._notificationService.showSuccess('Field Updated Successfully');
          } else {
            this._notificationService.showError('Field Update Failed');
          }
        }
      });
    }
  }

  LoadDoctypes(_req: any) {
    return this._documentDataAccess.GetDocumentTypesBasedonLoanType(_req).subscribe(
      res => {
        if (isTruthy(res)) {
          const doctypes = jwtHelper.decodeToken(res.Data)['data'];
          if (doctypes.length > 0) {
            this._docTypes = [];
            doctypes.forEach(element => {
              this._docTypes.push({ id: element.DocID, text: element.DocumentName });
            });
            this.Doctypes.next(this._docTypes.slice());
          }
        }
      }
    );
  }
  SetTableRows(DtableRows: any) {
    this.DatatableRows = DtableRows;
  }
  getTableRows() {
    this.DRows.next(this.DatatableRows);
    this.SetTableRows(this.DatatableRows);
  }

  SetHideValue(_hide: any[]) {
    this.ShowHide.next(_hide);
  }

  validate(_req: any): boolean {
    if (isTruthy(_req.DisplayName)) {
      this._notificationService.showError('Field Display Name Required');
      this.IsValidate = true;
    }
    return this.IsValidate;
  }
}
