import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ManagerDocumentsDatatableModel } from '../models/manager-documents-table.model';
import { Subject, Subscription } from 'rxjs';
import { ManagerDocTypeDataAccess } from '../managedoctype.data';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { ValidateModel } from '../models/validate.model';
import { Location } from '@angular/common';
import { EditDocumentFieldModel } from '../../documenttype/models/edit-document-field.model';
import { DocumentDataAccess } from '../../documenttype/documenttype.data';
import { AddDocumentTypeStepModel } from '../../documenttype/models/documenttype-step.model';
import { Router } from '@angular/router';
const jwtHelper = new JwtHelperService();

@Injectable()

export class ManagerDoctypeService {
    _manageDocumentTypeRowData: ManagerDocumentsDatatableModel = new ManagerDocumentsDatatableModel();
    setNextStep = new Subject<AddDocumentTypeStepModel>();
    _managerDocData = new Subject<ManagerDocumentsDatatableModel>();
    LoanTypeItems = new Subject();
    IsEdit = new Subject();
    IsView = new Subject();
    TableShow = new Subject();
    _reloadTable = new Subject<any>();
    ShowHide = new Subject<any[]>();
    FieldId = new Subject<any>();
    setDocFieldTableData = new Subject<any>();
    DeleteConfirmation = new Subject<any>();
    EditDocFieldData = new Subject<any[]>();
    Doctypes = new Subject<any>();
    DRows = new Subject<any>();
    SelectedValues = new Subject<any>();
    isPrevious = new Subject<any>();
    DocumentTypeName = new Subject<any>();
    CustomerID = new Subject<any>();
    LoanTypeID = new Subject<any>();
    loading = false;
    AddDocTypeSteps: any = {
        DocumentType: 1,
        Documentfields: 2
    };
    _isPrevious = false;
    constructor(
        private _managerDoctypeDataAccess: ManagerDocTypeDataAccess,
        private _notificationService: NotificationService,
        private _documentDataAccess: DocumentDataAccess,
        private location: Location,
        private _route: Router
    ) {

    }
    private _loanTypeItems: any = [];
    private slectRowData: EditDocumentFieldModel = new EditDocumentFieldModel();
    private DatatableRows: any = {};
    private _docname: any = '';
    private _docfields: any[] = [];
    private _docTypes: any[] = [];
    private _stepID: Subscription;
    private IsValidate = false;
    private UpdateDocName = '';
    private customerId = 0;
    private show = false;
    SetDocumentTypeRowData(inputData: ManagerDocumentsDatatableModel) {
        this._manageDocumentTypeRowData = inputData;
    }
    EditDocumentType(_req: any) {
        this.UpdateDocName = _req.documentType.Name;
        return this._managerDoctypeDataAccess.UpdateDocumentType(_req).subscribe(
            res => {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                if (isTruthy(result)) {
                    if (result === true) {
                        this._notificationService.showSuccess('Document Type Updated Successfully');
                        this.DocumentTypeName.next(this.UpdateDocName);

                    }
                    this._isPrevious = true;
                    this.isPrevious.next(this._isPrevious);
                    this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.Documentfields, 'active complete', 'active'));

                } else {
                    this._notificationService.showError('Document Type Updated Failed');
                }
            });

    }
    CheckDocumentAvailableForEdit(_req: any) {
        const input = { DocumentTypeName: _req.documentType.Name };
        return this._managerDoctypeDataAccess.CheckDocumentExistForEdit(JSON.stringify(input)).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data.length === 1) {
                    if (data[0].DocumentTypeID === _req.documentType.DocumentTypeID) {
                        this.EditDocumentType(_req);

                    } else {
                        this._notificationService.showError('Document Already Available');

                    }

                } else if (data.length === 0) {
                    this.EditDocumentType(_req);
                } else {
                    this._notificationService.showError('Document Already Available');
                }
            }
        });
    }
    getDocumentTypeRowData(): void {
        this._managerDocData.next(this._manageDocumentTypeRowData);
        this.SetDocName(this._manageDocumentTypeRowData.Name);
    }
    SetSelectedRow(Values: EditDocumentFieldModel) {
        this.slectRowData = Values;
        this.SelectedValues.next(this.slectRowData);

    }

    GetSelectedRow() {

        this.SelectedValues.next(this.slectRowData);
    }

    SetDocName(docName: any) {
        this._docname = docName;
    }
    getDocName() {
        this.DocumentTypeName.next(this.UpdateDocName);
    }
    Validate(cusID, loanTypeId): boolean {
        let returValue = false;
        if (!isTruthy(cusID) && (isTruthy(loanTypeId))) {
            this._notificationService.showError('Select Customer ');
            returValue = true;
        } else if ((!isTruthy(loanTypeId)) && (isTruthy(cusID))) {
            this._notificationService.showError('Select LoanTYpe');
            returValue = true;
        }
        return returValue;
    }

    validate(_req: any): boolean {
        if (!isTruthy(_req.Field.DisplayName)) {
            this._notificationService.showError('Field Display Name Required');
            this.IsValidate = true;
        }
        return this.IsValidate;
    }
    GotoPrevious() {
        this.location.back();
    }
    SetLoading() {
        this.loading = true;
    }
    GetManagerDoctypes(_req: ValidateModel) {
        this.CustomerID = _req.CustomerID;
        this.LoanTypeID = _req.LoanTypeID;
        return this._managerDoctypeDataAccess.GetManagerDocTypes(_req).subscribe(res => {
            if (isTruthy(res)) {
                const _managerDocs = jwtHelper.decodeToken(res.Data)['data'];
                this._managerDocData.next(_managerDocs);
            }
        });

    }
    setClick(val: any) {
        this.show = true;
    }
    GetCurrentLoanTypeId() {
        return this.LoanTypeID;
    }
    GetCurrentCustomerId() {
        return this.CustomerID;
    }
    SetHideValue(_hide: any[]) {
        this.ShowHide.next(_hide);
    }

    SetTableRows(DtableRows: any) {
        this.DatatableRows = DtableRows;
    }
    GetLoantypeForCustomer(_req: any) {
        this.customerId = _req.CustomerID;
        if (this.customerId > 0) {
            return this._managerDoctypeDataAccess.GetLoanTypeForCustomer(_req).subscribe(res => {
                if (isTruthy(res)) {
                    this._loanTypeItems = [];
                    const data = jwtHelper.decodeToken(res.Data)['data'];
                    const _loanTypes = jwtHelper.decodeToken(res.Data)['data'];
                    if (_loanTypes.length > 0) {
                        _loanTypes.forEach(element => {
                            this._loanTypeItems.push({ id: element.LoanTypeID, text: element.LoanTypeName });
                        });
                        this.LoanTypeItems.next(this._loanTypeItems.slice());
                    }
                }
            });
        } else if (this.show) {
            this._notificationService.showError('Select Customer');
            this._loanTypeItems = [];
            this.LoanTypeItems.next(this._loanTypeItems.slice());

        }

    }
    SyncRetainUpdateStagings(_req: any) {
        return this._managerDoctypeDataAccess.SyncRetainUpdateStagings(_req).subscribe(res => {
            if (isTruthy(res)) {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                if (result === true) {
                    this._notificationService.showSuccess('Sync Updated Successfully');
                } else {
                    this._notificationService.showError('Sync Update Failed');
                }
            }
        });
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
        if (this.validate(_req)) {
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
    SetNext() {
        this._isPrevious = true;
        this.isPrevious.next(this._isPrevious);
        this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.LoanTypeMapping, '', 'active complete'));
        this._route.navigate(['view/mdocumenttype']);
        this._notificationService.showSuccess('DocumentType Updated Successfully');
        this.SetLoading();

    }
    SetStepID(stepid: any) {
        this._stepID = stepid;
        this.SetPrevious();
    }
    SetPrevious() {
        if (this._stepID === this.AddDocTypeSteps.Documentfields) {
            this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.DocumentType, 'active', ''));
        } else if (this._stepID === this.AddDocTypeSteps.LoanTypeMapping) {
            this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.Documentfields, 'active complete', 'active '));
        }
    }

    ValidateEdit(request: any): boolean {
        if (!isTruthy(request.Name) && !isTruthy(request.DisplayName) && !isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('All Fields are required');
            return this.IsValidate = false;
        }
        if (isTruthy(request.Name) && isTruthy(request.DisplayName) && isTruthy(request.DocumentLevel)) {
            return this.IsValidate = true;
        } else if (isTruthy(request.Name) && isTruthy(request.DisplayName)) {
            this._notificationService.showError('Document Level is required');
            return this.IsValidate = false;
        } else if (isTruthy(request.Name) && isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('Display Name is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DisplayName) && isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('Document Name is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.Name)) {
            this._notificationService.showError('Display Name and Document Level is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DisplayName)) {
            this._notificationService.showError('Document Name and Document Level is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('Document Name and Display Name is required');
            return this.IsValidate = false;

        }

    }

}
