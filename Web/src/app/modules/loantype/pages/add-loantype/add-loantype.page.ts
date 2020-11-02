import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subscription } from 'rxjs';
import { LoanTypeService } from '../../service/loantype.service';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AddLoantypeRequestModel } from '../../models/add-loantype-request.model';
import { AppSettings } from '@mts-app-setting';
import { NotificationService } from '@mts-notification';
import { AssignDocumentTypeRequestModel, DocMappingDetail } from '../../models/assign-document-types-request.model';
import { LoanTypeWizardStepModel } from '../../models/loan-type-wizard-steps.model';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';

@Component({
  selector: 'mts-add-loan-type',
  templateUrl: 'add-loantype.page.html',
  styleUrls: ['add-loantype.page.css'],
  providers: [AddLoanTypeService, CommonRuleBuilderService]
})
export class AddLoanTypeComponent implements OnInit, OnDestroy {
  slideOneTranClass: any = 'transForm';
  LoanTypeName = '';
  LoanTypeActive = true;
  createChecklistType = '';
  createStackingOrder = '';
  AssignedDocTypes: any[] = [];
  AddLoantypeSteps = this._addLoanTypeService.AddLoantypeSteps;
  stackOrderType = '';
  checkListType = '';
  stepModel: LoanTypeWizardStepModel = new LoanTypeWizardStepModel(this.AddLoantypeSteps.Loantype, 'active', '', '', '');
  loading = false;
  DocMappingDetails: DocMappingDetail[] = [];
  constructor(
    private _route: Router,
    private _location: Location,
    private _loanService: LoanTypeService,
    private _addLoanTypeService: AddLoanTypeService,
    private _notificationService: NotificationService) { }

  private _previousStep = false;
  private subscriptions: Subscription[] = [];
  private _loanType: { Type: string, LoanTypeID: number, LoanTypeName: string, Active: boolean };

  ngOnInit() {
    this._loanType = this._loanService.getLoanType();

    const currtURL = this._route.url;
    if (currtURL.search('editloantype') > 0 && this._loanType.Type === 'Add') {
      this._route.navigate(['view/loantype']);
    }

    this.subscriptions.push(this._addLoanTypeService.setNextStep.subscribe((res: LoanTypeWizardStepModel) => {
      this.stepModel = res;
    }));
    this.subscriptions.push(this._addLoanTypeService.assignedDocs.subscribe((res: any[]) => {
      this.AssignedDocTypes = [...res];
    }));
    this.subscriptions.push(this._addLoanTypeService.checkListBack.subscribe((res: string) => {
      this.createChecklistType = res;
    }));
    this.subscriptions.push(this._addLoanTypeService.stackingOrderBack.subscribe((res: string) => {
      this.createStackingOrder = res;
    }));
    this.subscriptions.push(this._addLoanTypeService.stackingOrderType.subscribe((res: string) => {
      this.stackOrderType = res;
    }));
    this.subscriptions.push(this._addLoanTypeService.checkListType.subscribe((res: string) => {
      this.checkListType = res;
    }));
    this.subscriptions.push(this._addLoanTypeService.Loading.subscribe((res: boolean) => {
      this.loading = res;
    }));

    if (this._loanType.Type !== 'Add') {
      this.LoanTypeName = this._loanType.LoanTypeName;
      this.LoanTypeActive = this._loanType.Active;
      this._previousStep = true;
      this._addLoanTypeService.setCurrentLoanType({ LoanTypeID: this._loanType.LoanTypeID, LoanTypeName: this.LoanTypeName });
      this._addLoanTypeService.getLoanTypeChecklist();
      this._addLoanTypeService.getStackingOrderData();
      this.createChecklistType = 'createEdit';
      this.createStackingOrder = 'createEdit';
      this._addLoanTypeService.setChecklistType('edit');
      this._addLoanTypeService.setStackType('edit');
    }
  }

  showChecklist(divType: string, type: string) {
    if (type === 'checklist') {
      this.createChecklistType = divType;
      this._addLoanTypeService.setChecklistType(divType);
    } else if (type === 'stackingorder') {
      this.createStackingOrder = divType;
      this._addLoanTypeService.setStackType(divType);
    }
  }

  CreateCheckList() { }

  AddLoanTypeSubmit() {
    if (this.isValid()) {
      this._addLoanTypeService.Loading.next(true);
      const req = new AddLoantypeRequestModel(AppSettings.TenantSchema, 0, this.LoanTypeName, 0, this.LoanTypeActive);
      this._addLoanTypeService.addLoanTypeSubmit(req);
    }
  }

  isValid(): boolean {
    let tempLoanType = '';
    if (isTruthy(this.LoanTypeName)) {
      tempLoanType = this.LoanTypeName.replace(/[\s]/g, '');
    } else {
      this._notificationService.showError('Loan Type Name Required');
      return false;
    }
    const obj = this._loanService.GetLoanTypeMaster().filter(l => l.LoanTypeName === tempLoanType);

    if (this._previousStep) {
      if (typeof obj !== 'undefined' && obj.length === 0) {
        return true;
      } else if (obj.length > 0 && obj[0].LoanTypeName === this._addLoanTypeService.getCurrentLoanTypeName()) {
        return true;
      } else if (obj.length > 0 && obj[0].LoanTypeName === tempLoanType) {
        this._notificationService.showError('Loan Type Already Exists');
        return false;
      }
    } else {
      if (typeof obj !== 'undefined' && obj.length === 0) {
        return true;
      } else if (obj.length > 0 && obj[0].LoanTypeName === tempLoanType) {
        this._notificationService.showError('Loan Type Already Exists');
        return false;
      }
    }
  }

  GotoNextStep() {
    if (this.stepModel.stepID === this.AddLoantypeSteps.Loantype) {
      if (!this._previousStep) {
        this.AddLoanTypeSubmit();
      } else {
        this.EditLoanTypeSubmit();
      }
    } else if (this.stepModel.stepID === this.AddLoantypeSteps.AssignDocumentType) {
      this.SaveDocMapping();
    } else if (this.stepModel.stepID === this.AddLoantypeSteps.Checklist) {
      this.IsChecklistCreated();
    } else if (this.stepModel.stepID === this.AddLoantypeSteps.StackingOrder) {
      this.SaveStackingOrder();
    }
  }

  GotoPreviousStep() {
    this._previousStep = true;
    if (this.stepModel.stepID === this.AddLoantypeSteps.AssignDocumentType) {
      this._addLoanTypeService.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.Loantype, 'active', '', '', ''));
    } else if (this.stepModel.stepID === this.AddLoantypeSteps.Checklist) {
      this._addLoanTypeService.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.AssignDocumentType, 'active complete', 'active', '', ''));
    } else if (this.stepModel.stepID === this.AddLoantypeSteps.StackingOrder) {
      this._addLoanTypeService.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.Checklist, 'active complete', 'active complete', 'active', ''));
    }
  }

  EditLoanTypeSubmit() {
    if (this.isValid()) {
      this._addLoanTypeService.Loading.next(true);
      const req = new AddLoantypeRequestModel(AppSettings.TenantSchema, this._addLoanTypeService.getCurrentLoanTypeID(), this.LoanTypeName, 0, this.LoanTypeActive);
      this._addLoanTypeService.updateLoanTypeSubmit(req);
      this._previousStep = false;
    }
  }

  SaveStackingOrder() {
    this._addLoanTypeService.SaveStackingOrder();
  }

  IsChecklistCreated() {
    const checkList = this._addLoanTypeService.getChecklist();
    if (isTruthy(checkList) && checkList.CheckListID > 0) {
      this._addLoanTypeService.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.StackingOrder, 'active complete', 'active complete', 'active complete', 'active'));
    } else {
      this._notificationService.showError('Create/Clone Checklist Group');
    }
  }

  SaveDocMapping() {
    this._addLoanTypeService.Loading.next(true);
    this.DocMappingDetails = this.AssignedDocTypes;
     const req = new AssignDocumentTypeRequestModel(this._addLoanTypeService.getCurrentLoanTypeID(), this.DocMappingDetails);
    this._addLoanTypeService.SaveDocMapping(req);
  }

  GotoMaster() {
    this._location.back();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
