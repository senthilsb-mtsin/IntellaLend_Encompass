import { Component, OnInit } from '@angular/core';
import { Subscription, Subject } from 'rxjs';
import { DocumentTypeService } from '../../services/documenttype.service';
import { DocumentTypeDatatableModel } from '../../models/document-type-datatable.model';
import { CommonService } from 'src/app/shared/common';
import { AppSettings } from '@mts-app-setting';
import { AddDocumentTypeStepModel } from '../../models/documenttype-step.model';
import { Router } from '@angular/router';
import { DocumentFieldService } from '../../services/documentfield.service';
import { NotificationService } from '@mts-notification';
import { AddDocumentTypeRequestModel } from '../../models/add-document-type-request.model';

@Component({
    selector: 'mts-editdocumenttype',
    templateUrl: 'editdocumenttype.page.html',
    styleUrls: ['editdocumenttype.page.css'],
})
export class EditDocumentTypeComponent implements OnInit {

    slideOneTranClass: any = 'transForm';
    Status: any[] = [];
    isPrevious = false;
    _parkingSpotItems: any[] = [];
    _editDocumentTypeData: DocumentTypeDatatableModel = new DocumentTypeDatatableModel();
    AddDocTypeSteps = this._documentTypeService.AddDocTypeSteps;
    AssignedLoans: any[] = [];
    IsValidate = false;
    setNextStep = new Subject<AddDocumentTypeStepModel>();
    stepModel: AddDocumentTypeStepModel = new AddDocumentTypeStepModel(this.AddDocTypeSteps.DocumentType, 'active', '');
    constructor(private _documentTypeService: DocumentTypeService, private _notifyService: NotificationService, private _documentFieldService: DocumentFieldService, private _route: Router, private _commonService: CommonService) {
        const StatusKeys = Object.keys(AppSettings.DocumentCriticalStatus);
        for (let index = 0; index < StatusKeys.length; index++) {
            const val = AppSettings.DocumentCriticalStatus[StatusKeys[index]];
            this.Status.push({ Label: val.Label, Value: val.Value });
        }
    }

    private subscription: Subscription[] = [];

    ngOnInit() {
        this._commonService.GetParkingSpotList(AppSettings.TenantSchema);
        this.subscription.push(this._commonService.SystemParkingSpotItems.subscribe((res: any[]) => {
            this._parkingSpotItems = res;
        }));
        this.subscription.push(this._documentTypeService.setNextStep.subscribe((res: AddDocumentTypeStepModel) => {
            this.stepModel = res;
        }));
        this.subscription.push(this._documentTypeService.isPrevious.subscribe((res: any) => {
            this.isPrevious = res;
        }));

        this._documentTypeService.getDocumentTypeRowData();
        this._editDocumentTypeData = this._documentTypeService._documentTypeRowData;
    }

    GotoPrevious() {
        this._route.navigate(['view/documenttype']);

    }
    EditDocumentType() {
        const editDocumentTypeData = { TableSchema: AppSettings.SystemSchema, documentType: this._editDocumentTypeData, ParkingSpotID: this._editDocumentTypeData.ParkingSpotID };
        this.IsValidate = this._documentTypeService.Validate(this._editDocumentTypeData);
        if (this.IsValidate) {
            this._documentTypeService.EditDocumentType(editDocumentTypeData);
        }
    }
    CheckDocumentAvailable() {
        const editDocumentTypeData = { TableSchema: AppSettings.SystemSchema, documentType: this._editDocumentTypeData, ParkingSpotID: this._editDocumentTypeData.ParkingSpotID };
        this.IsValidate = this._documentTypeService.Validate(this._editDocumentTypeData);
        if (this.IsValidate) {
            this._documentTypeService.CheckDocumentAvailableForEdit(editDocumentTypeData);
        }
    }
    GoToNextStep() {
        if (this.stepModel.stepID === this.AddDocTypeSteps.DocumentType) {
            this.CheckDocumentAvailable();
        } else if (this.stepModel.stepID === this.AddDocTypeSteps.Documentfields) {
            this.isPrevious = true;
            this._documentTypeService.SetNext();
        }
    }

    SetPrevious() {
        if (this.stepModel.stepID === this.AddDocTypeSteps.Documentfields) {
            this.isPrevious = false;
        }
        this._documentTypeService.SetStepID(this.stepModel.stepID);
    }
    ngDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
