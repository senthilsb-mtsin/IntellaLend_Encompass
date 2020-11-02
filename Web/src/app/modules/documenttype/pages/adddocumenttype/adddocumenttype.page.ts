import { Component, OnInit } from '@angular/core';
import { AddDocumentTypeStepModel } from '../../models/documenttype-step.model';
import { DocumentTypeService } from '../../services/documenttype.service';
import { Subject, Subscription } from 'rxjs';
import { AppSettings } from '@mts-app-setting';
import { CommonService } from 'src/app/shared/common';
import { Location } from '@angular/common';
import { AddDocumentTypeRequestModel } from '../../models/add-document-type-request.model';
import { DocumentFieldService } from '../../services/documentfield.service';
import { NotificationService } from '@mts-notification';
import { CheckDocumentDuplicateRequestModel } from '../../models/check-doc-dup-request.model';

@Component({
    selector: 'mts-adddocumenttype',
    templateUrl: 'adddocumenttype.page.html',
    styleUrls: ['adddocumenttype.page.css'],
})

export class AddDocumentTypeComponent implements OnInit {
    AddDocTypeSteps = this._documentTypeservice.AddDocTypeSteps;
    setNextStep = new Subject<AddDocumentTypeStepModel>();
    DocumentTypeName: string;
    DocumentDisplayName: string;
    DocumentLevel: string = AppSettings.DocumentCriticalStatus[11].Value.toString();
    DocumentTypeID: number;
    ParrkingSpot: any = 0;
    IsValidate = false;
    slideOneTranClass: any = 'transForm';
    stepModel: AddDocumentTypeStepModel = new AddDocumentTypeStepModel(this.AddDocTypeSteps.DocumentType, 'active', '');
    _parkingSpotItems: any[] = [];
    Status: any[] = [];
    isPrevious = false;
    notValidate = false;
    ParkingSpotName = '';
    constructor(private _documentTypeservice: DocumentTypeService, private location: Location,
        private _commonService: CommonService,
        private _notifyService: NotificationService, private _docFieldService: DocumentFieldService) {
        const StatusKeys = Object.keys(AppSettings.DocumentCriticalStatus);
        for (let index = 0; index < StatusKeys.length; index++) {
            const val = AppSettings.DocumentCriticalStatus[StatusKeys[index]];
            this.Status.push({ Label: val.Label, Value: val.Value });
        }

    }

    private subscription: Subscription[] = [];
    ngOnInit(): void {
        this._commonService.GetParkingSpotList(AppSettings.TenantSchema);
        this.subscription.push(this._commonService.SystemParkingSpotItems.subscribe((res: any[]) => {
            this._parkingSpotItems = res;
        }));
        this.subscription.push(this._documentTypeservice.setNextStep.subscribe((res: AddDocumentTypeStepModel) => {
            this.stepModel = res;
        }));
        this.subscription.push(this._documentTypeservice.isPrevious.subscribe((res: any) => {
            this.isPrevious = res;
        }));

    }

    GotoPrevious() {
        this.location.back();
    }

    AddDocumentType() {
        const req = new AddDocumentTypeRequestModel(this.DocumentTypeName, this.DocumentDisplayName, this.DocumentLevel, this.ParrkingSpot);
        this._documentTypeservice.AddDocumentType(req);
    }
    CheckDocumentAvailable() {
        const req = new AddDocumentTypeRequestModel(this.DocumentTypeName, this.DocumentDisplayName, this.DocumentLevel, this.ParrkingSpot);
        this.IsValidate = this._documentTypeservice.Validate(req);
        if (this.IsValidate) {
            this._documentTypeservice.CheckDocumentAvailable(req);
        }
    }
    GetParkingName(id) {
        const tempMas = this._parkingSpotItems.filter(x => x.ID === this.ParrkingSpot);
        if (tempMas.length <= 0) { return ''; }
        this.ParkingSpotName = tempMas[0].ParkingSpotName;

    }

    CheckDocForEdit() {
         this.GetParkingName(this.ParrkingSpot);
        const req = new CheckDocumentDuplicateRequestModel(this._documentTypeservice._documentTypeID, this.DocumentTypeName, this.DocumentDisplayName, this.ParkingSpotName, true, this.DocumentLevel, this.ParrkingSpot);
        const editDocumentTypeData = { TableSchema: AppSettings.SystemSchema, documentType: req, ParkingSpotID: this.ParrkingSpot };
        this._documentTypeservice.CheckDocumentAvailableForEdit(editDocumentTypeData);

 }
    GotoNextStep() {

        if (this.stepModel.stepID === this.AddDocTypeSteps.DocumentType) {
            if (this.notValidate) {
                this.CheckDocForEdit();
            } else {
                this.CheckDocumentAvailable();
            }

        } else if (this.stepModel.stepID === this.AddDocTypeSteps.Documentfields) {
            this._documentTypeservice.SetNextForAdd();

        }
    }

    SetPrevious() {
        if (this.stepModel.stepID === this.AddDocTypeSteps.Documentfields) {
            this.isPrevious = false;
        }
        this.notValidate = true;
        this._documentTypeservice.SetStepID(this.stepModel.stepID);
    }
    ngDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
