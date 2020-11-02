import { OnInit, Component, OnDestroy } from '@angular/core';
import { Subscription, Subject } from 'rxjs';
import { CommonService } from 'src/app/shared/common';
import { AppSettings } from '@mts-app-setting';
import { AddDocumentTypeStepModel } from 'src/app/modules/documenttype/models/documenttype-step.model';
import { NotificationService } from '@mts-notification';
import { Router } from '@angular/router';
import { ManagerDocumentsDatatableModel } from '../../models/manager-documents-table.model';
import { ManagerDoctypeService } from '../../services/manage-doctype.service';

@Component({
    selector: 'mts-manageeditdoctype',
    templateUrl: 'manageeditdoctype.page.html',
    styleUrls: ['manageeditdoctype.page.css'],
})
export class ManagerEditDocumentTypeComponent implements OnDestroy, OnInit {

    CustomerItems: any = [];
    LoanTypeItems: any = [];
    _manageEditDocumentTypeData: ManagerDocumentsDatatableModel = new ManagerDocumentsDatatableModel();
    AuthorityLabelSingular = AppSettings.AuthorityLabelSingular;
    slideOneTranClass: any = 'transForm';
    AddDocTypeSteps = this._managerDocservice.AddDocTypeSteps;
    AssignedLoans: any[] = [];
    IsValidate = false;
    Status: any[] = [];
    isPrevious = false;
    existingDocName = '';
    setNextStep = new Subject<AddDocumentTypeStepModel>();
    stepModel: AddDocumentTypeStepModel = new AddDocumentTypeStepModel(this.AddDocTypeSteps.DocumentType, 'active', '');
    constructor(private _managerDocservice: ManagerDoctypeService, private _notifyService: NotificationService, private _commonservice: CommonService, private _route: Router) {
        const StatusKeys = Object.keys(AppSettings.DocumentCriticalStatus);
        for (let index = 0; index < StatusKeys.length; index++) {
            const val = AppSettings.DocumentCriticalStatus[StatusKeys[index]];
            this.Status.push({ Label: val.Label, Value: val.Value });
        }
    }
    private subscription: Subscription[] = [];

    ngOnInit() {

        this._commonservice.GetActiveCustomerList(AppSettings.TenantSchema);
        this._commonservice.GetAllLoantypeMaster(AppSettings.TenantSchema);

        this.subscription.push(this._managerDocservice.setNextStep.subscribe((res: AddDocumentTypeStepModel) => {
            this.stepModel = res;
        }));
        this.subscription.push(this._managerDocservice.isPrevious.subscribe((res: any) => {
            this.isPrevious = res;
        }));
        this.subscription.push(this._commonservice.SystemLoanTypeMaster.subscribe((res: any) => {
            this.LoanTypeItems = res;
        }));
        this.subscription.push(this._commonservice.SystemActiveCustomerMaster.subscribe((res: any) => {
            this.CustomerItems = res;
        }));
        this._manageEditDocumentTypeData = this._managerDocservice._manageDocumentTypeRowData;
        this.existingDocName = this._manageEditDocumentTypeData.Name;
        this._managerDocservice.getDocumentTypeRowData();

    }

    GotoPrevious() {
        this._route.navigate(['view/mdocumenttype']);
        this._managerDocservice.SetLoading();

    }
    EditDocumentType() {
        const editDocumentTypeData = { TableSchema: AppSettings.TenantSchema, documentType: this._manageEditDocumentTypeData, CustomerID: this._managerDocservice.CustomerID, LoanTypeID: this._managerDocservice.LoanTypeID };
        this.IsValidate = this._managerDocservice.ValidateEdit(this._manageEditDocumentTypeData);
        if (this.IsValidate) {
            this._managerDocservice.EditDocumentType(editDocumentTypeData);
        }
    }
    CheckDocumentAvailable() {
        const editDocumentTypeData = { TableSchema: AppSettings.TenantSchema, documentType: this._manageEditDocumentTypeData, CustomerID: this._manageEditDocumentTypeData.CustomerID, LoanTypeID: this._manageEditDocumentTypeData.LoanTypeID };
        this.IsValidate = this._managerDocservice.ValidateEdit(this._manageEditDocumentTypeData);
        if (this.IsValidate) {
            if (this.existingDocName === this._manageEditDocumentTypeData.Name) {
                this._managerDocservice.EditDocumentType(editDocumentTypeData);
            } else {
                this._managerDocservice.CheckDocumentAvailableForEdit(editDocumentTypeData);
            }
        }
    }
    GoToNextStep() {
        if (this.stepModel.stepID === this.AddDocTypeSteps.DocumentType) {
            this.CheckDocumentAvailable();
        } else if (this.stepModel.stepID === this.AddDocTypeSteps.Documentfields) {
            this.isPrevious = true;
            this._managerDocservice.SetNext();
        }
    }

    SetPrevious() {
        if (this.stepModel.stepID === this.AddDocTypeSteps.Documentfields) {
            this.isPrevious = false;
        }
        this._managerDocservice.SetStepID(this.stepModel.stepID);
    }
    ngOnDestroy() {
        this.subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
