import { Injectable, EventEmitter } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { DocumentTypeDatatableModel } from '../models/document-type-datatable.model';
import { Subject, Subscription } from 'rxjs';
import { AddDocumentTypeStepModel } from '../models/documenttype-step.model';
import { AddDocumentTypeRequestModel } from '../models/add-document-type-request.model';
import { DocumentDataAccess } from '../documenttype.data';
import { NotificationService } from '@mts-notification';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { CommonService } from 'src/app/shared/common';

const jwtHelper = new JwtHelperService();

@Injectable()

export class DocumentTypeService {
    setNextStep = new Subject<AddDocumentTypeStepModel>();
    _documentTypeRowData: DocumentTypeDatatableModel = new DocumentTypeDatatableModel();
    DocumentTypeData = new Subject<any>();
    documentList: any = [];
    isPrevious = new Subject<any>();
    _documentTypeID = 0;
    DocumentTypeID = new Subject<any>();
    DocumentTypeName = new Subject<any>();
    isLoad = new Subject<any>();
    syncDetailEnable = new Subject<any>();
    IsValidate = false;
    AddDocTypeSteps: any = {
        DocumentType: 1,
        Documentfields: 2
    };
    showLoading = new Subject<boolean>();

    constructor(private _documentDataAccess: DocumentDataAccess, private _commonservice: CommonService, private _route: Router, private location: Location, private _notificationService: NotificationService) {

    }
    private _stepID: Subscription;
    private _isPrevious = false;
    private UpdateDocName = '';

    SetDocumentTypeRowData(inputData: DocumentTypeDatatableModel) {
        this._documentTypeRowData = inputData;
    }
    SyncDocTypes(req: { TableSchema: string }) {
        return this._documentDataAccess.SynDocTypes(req).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                this.syncDetailEnable.next(true);
                this._notificationService.showSuccess('Document Sync Successfully');
                this.showLoading.next(false);

            } else {
                this.syncDetailEnable.next(true);
                this._notificationService.showError('Document Sync Failed');
                this.showLoading.next(false);

            }

        });
    }
    getDocumentTypeRowData(): void {
        this.DocumentTypeData.next(this._documentTypeRowData);
    }
    CheckDocumentAvailable(_req: AddDocumentTypeRequestModel) {
        const input = { DocumentTypeName: _req.DocumentTypeName };
        return this._documentDataAccess.CheckDocumentExistForEdit(JSON.stringify(input)).subscribe(res => {
            if (isTruthy(res)) {
                const data = jwtHelper.decodeToken(res.Data)['data'];
                if (data.length === 0) {
                    this.AddDocumentType(_req);
                } else {
                    this._notificationService.showError('Document Already Available');
                }
            }
        });
    }
    getDocName() {
        this.DocumentTypeName.next(this.UpdateDocName);
    }
    EditDocumentType(_req: any) {
        this.UpdateDocName = _req.documentType.Name;
        return this._documentDataAccess.EditDocumentType(_req).subscribe(
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
    AddDocumentType(_req: AddDocumentTypeRequestModel) {
        return this._documentDataAccess.AddDocumentType(_req).subscribe(
            res => {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                if (isTruthy(result)) {
                    this._documentTypeID = result;
                    this.DocumentTypeID.next(this._documentTypeID);
                    this._notificationService.showSuccess('DocumentType Added Successfully');
                    this._isPrevious = true;
                    this.isPrevious.next(this._isPrevious);
                    this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.Documentfields, 'active complete', 'active'));
                } else {
                    this._notificationService.showError('DocumentType Name already exist');
                }
            });
    }
    CheckDocumentAvailableForEdit(_req: any) {
        const input = { DocumentTypeName: _req.documentType.Name };
        return this._documentDataAccess.CheckDocumentExistForEdit(JSON.stringify(input)).subscribe(res => {
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
    GotoPrevious() {
        this.location.back();
    }
    SetNext() {
        this._isPrevious = true;
        this.isPrevious.next(this._isPrevious);
        this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.LoanTypeMapping, '', 'active complete'));
        this._route.navigate(['view/documenttype']);
        this._notificationService.showSuccess('Updated Successfully');

    }
    SetStepID(stepid: any) {
        this._stepID = stepid;
        this.SetPrevious();
    }
    SetNextForAdd() {
        this._isPrevious = true;
        this.isPrevious.next(this._isPrevious);
        this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.LoanTypeMapping, '', 'active complete'));
        this._route.navigate(['view/documenttype']);
        this._notificationService.showSuccess('Added Successfully');

    }
    SetPrevious() {
        if (this._stepID === this.AddDocTypeSteps.Documentfields) {
            this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.DocumentType, 'active', ''));
        } else if (this._stepID === this.AddDocTypeSteps.LoanTypeMapping) {
            this.setNextStep.next(new AddDocumentTypeStepModel(this.AddDocTypeSteps.Documentfields, 'active complete', 'active '));
        }
    }

    Validate(request: any) {
        if (!isTruthy(request.DocumentTypeName || request.Name) && !isTruthy(request.DocumentDisplayName) && !isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('All Fields are required');
            return this.IsValidate = false;
        }
        if (isTruthy(request.DocumentTypeName || request.Name) && isTruthy(request.DocumentDisplayName || request.DisplayName) && isTruthy(request.DocumentLevel)) {
            return this.IsValidate = true;
        } else if (isTruthy(request.DocumentTypeName || request.Name) && isTruthy(request.DocumentDisplayName || request.DisplayName)) {
            this._notificationService.showError('Document Level is required');
            return this.IsValidate = false;
        } else if (isTruthy(request.DocumentTypeName || request.Name) && isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('Display Name is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DocumentDisplayName || request.DisplayName) && isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('Document Name is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DocumentTypeName || request.Name)) {
            this._notificationService.showError('Display Name and Document Level is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DocumentDisplayName || request.DisplayName)) {
            this._notificationService.showError('Document Name and Document Level is required');
            return this.IsValidate = false;

        } else if (isTruthy(request.DocumentLevel)) {
            this._notificationService.showError('Document Name and Display Name is required');
            return this.IsValidate = false;

        }

    }

}
