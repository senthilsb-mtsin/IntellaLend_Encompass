import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceTypeService } from '../../service/service-type.service';
import { AddServiceTypeService } from '../../service/add-service-type.service';
import { NotificationService } from '@mts-notification';
import { ServiceTypeWizardStepModel } from '../../models/service-type-wizard-steps.model';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AppSettings } from '@mts-app-setting';
import { AddServiceTypeRequestModel } from '../../models/add-service-type-request.model';
import { AssignLoanTypesRequestModel } from '../../models/assign-loan-types-request.model';
import { ServiceTypePriorityModel } from '../../models/service-type-priority.model';
import { ServiceTypeModel } from '../../models/service-type.model';
import { ServiceTypeRoleModel } from '../../models/service-type-role.model';

@Component({
  selector: 'mts-add-service-type',
  templateUrl: 'add-service-type.page.html',
  styleUrls: ['add-service-type.page.css'],
})

export class AddServiceTypeComponent implements OnInit, OnDestroy {
  //#region Public Variables
  slideOneTranClass: any = 'transForm';
  ServiceTypeName = '';
  AssignedLoanTypes: number[] = [];
  AddServiceTypeSteps = this._addServiceTypeService.AddServiceTypeSteps;
  stepModel: ServiceTypeWizardStepModel = new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.ServiceType, 'active', '');
  loading = false;
  priorityList: ServiceTypePriorityModel[] = [];
  ReviewDetails: ServiceTypeModel;
  RoleItems: ServiceTypeRoleModel[] = [];
  _serviceType: { Type: string; ServiceTypeID: number; ServiceTypeName: string; };
  //#endregion Public Variables

  //#region  Constructor
  constructor(
    private _route: Router,
    private _location: Location,
    private _serviceTypeService: ServiceTypeService,
    private _addServiceTypeService: AddServiceTypeService,
    private _notificationService: NotificationService) { }
  //#endregion Constructor

  //#region  Private Variables
  private _previousStep = false;
  private subscriptions: Subscription[] = [];

  //#endregion Private Variables

  //#region Public Methods
  ngOnInit() {
    this._serviceType = this._serviceTypeService.getServiceType();
    this.ReviewDetails = this._addServiceTypeService.getCurrentReviewDetails();

    const currtURL = this._route.url;
    if (currtURL.search('editreviewtype') > 0 && this._serviceType.Type === 'Add') {
      this._route.navigate(['view/reviewtype']);
    }

    this.subscriptions.push(this._addServiceTypeService.setNextStep.subscribe((res: ServiceTypeWizardStepModel) => {
      this.stepModel = res;
    }));
    this.subscriptions.push(this._addServiceTypeService.assignedLoanTypes.subscribe((res: number[]) => {
      this.AssignedLoanTypes = [...res];
    }));
    this.subscriptions.push(this._addServiceTypeService.Loading.subscribe((res: boolean) => {
      this.loading = res;
    }));

    this.subscriptions.push(this._addServiceTypeService.priorityList.subscribe((res: ServiceTypePriorityModel[]) => {
      this.priorityList = [...res];
    }));

    this.subscriptions.push(this._addServiceTypeService.roleList.subscribe((res: ServiceTypeRoleModel[]) => {
      this.RoleItems = [...res];
    }));

    if (this._serviceType.Type !== 'Add') {
      this.ServiceTypeName = this._serviceType.ServiceTypeName;
      this._previousStep = true;
      this._addServiceTypeService.setCurrentServiceType({ ServiceTypeID: this._serviceType.ServiceTypeID, ServiceTypeName: this.ServiceTypeName });
    }
  }

  AddServiceTypeSubmit() {
    if (this.isValid()) {
      this._addServiceTypeService.Loading.next(true);
      const req = new AddServiceTypeRequestModel(AppSettings.TenantSchema, this.ReviewDetails);
      this._addServiceTypeService.addServiceTypeSubmit(req);
    }
  }

  isValid(): boolean {
    let tempServiceType = '';
    if (isTruthy(this.ReviewDetails.ReviewTypeName)) {
      tempServiceType = this.ReviewDetails.ReviewTypeName.replace(/[\s]/g, '');
    } else {
      this._notificationService.showError('Service Type Name Required');
      return false;
    }
    if ((this.ReviewDetails.ReviewTypePriority === 0) || (this.ReviewDetails.ReviewTypePriority === null)) {
      this._notificationService.showError('Priority Required');
      return false;
    }
    const obj = this._serviceTypeService.GetServiceTypeMaster().filter(l => l.ReviewTypeName === tempServiceType);
    if (this._previousStep) {
      if (typeof obj !== 'undefined' && obj.length === 0) {
        return true;
      } else if (obj.length > 0 && obj[0].ReviewTypeName === this._addServiceTypeService.getCurrentServiceTypeName()) {
        return true;
      } else if (obj.length > 0 && obj[0].ReviewTypeName === tempServiceType) {
        this._notificationService.showError('Service Type Already Exists');
        return false;
      }
    } else {
      if (typeof obj !== 'undefined' && obj.length === 0) {
        return true;
      } else if (obj.length > 0 && obj[0].ReviewTypeName === tempServiceType) {
        this._notificationService.showError('Service Type Already Exists');
        return false;
      }
    }
  }

  GotoNextStep() {
    if (this.stepModel.stepID === this.AddServiceTypeSteps.ServiceType) {
      if (!this._previousStep) {
        this.AddServiceTypeSubmit();
      } else {
        this.EditServiceTypeSubmit();
      }
    } else if (this.stepModel.stepID === this.AddServiceTypeSteps.AssignLoanType) {
      this.SaveLoanMapping();
    }
  }

  GotoPreviousStep() {
    this._previousStep = true;
    if (this.stepModel.stepID === this.AddServiceTypeSteps.AssignLoanType) {
      this._addServiceTypeService.setNextStep.next(new ServiceTypeWizardStepModel(this.AddServiceTypeSteps.ServiceType, 'active', ''));
    }
  }

  EditServiceTypeSubmit() {
    if (this.isValid()) {
      this._addServiceTypeService.Loading.next(true);
      const req = new AddServiceTypeRequestModel(AppSettings.TenantSchema, this.ReviewDetails);
      this._addServiceTypeService.updateServiceTypeSubmit(req);
      this._previousStep = false;
    }
  }

  SaveLoanMapping() {
    this._addServiceTypeService.Loading.next(true);
    const req = new AssignLoanTypesRequestModel(this.ReviewDetails.ReviewTypeID, this.AssignedLoanTypes.filter((v, i, a) => a.indexOf(v) === i).slice());
    this._addServiceTypeService.SaveLoanMapping(req);
  }

  GotoMaster() {
    this._location.back();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
  //#endregion Public Methods
}
