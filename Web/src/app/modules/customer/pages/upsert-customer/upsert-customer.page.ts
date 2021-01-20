import { Component, OnInit, OnDestroy } from '@angular/core';
import { CustomerService } from '../../services/customer.service';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';
import { CustomerDatatableModel } from '../../models/customer-datatable.model';
import { CustomerWizardStepModel } from '../../models/customer-wizard-steps.model';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { Router } from '@angular/router';
import { AppSettings } from '@mts-app-setting';
import { ValidateZipcodePipe, CharcCheckPipe, CheckDuplicateName } from '@mts-pipe';

@Component({
  selector: 'mts-upsert-customer',
  styleUrls: ['upsert-customer.page.css'],
  templateUrl: 'upsert-customer.page.html',
  providers: [UpsertCustomerService, CharcCheckPipe, ValidateZipcodePipe, CheckDuplicateName]
})
export class UpsertCustomerComponent implements OnInit, OnDestroy {
  slideOneTranClass: any = 'transForm';
  customer: CustomerDatatableModel;
  UpsertCustomerSteps: { Customer: number, ServiceType: number, LoanType: number, Checklist: number, StackingOrder: number, CustomerConfig: number } = this._upsertCustomerService.UpsertCustomerSteps;
  stepModel: CustomerWizardStepModel = new CustomerWizardStepModel(this.UpsertCustomerSteps.Customer, 'active', '', '', '', '', '');
  loading = false;
  AuthorityLabelSingular: string = AppSettings.AuthorityLabelSingular;
  ReviewTypeName = '';
  LoanTypeName = '';

  constructor(
    private _route: Router,
    private _location: Location,
    private _customerService: CustomerService,
    private _upsertCustomerService: UpsertCustomerService
  ) { }

  private _subscriptions: Subscription[] = [];
  private _previousStep = false;

  ngOnInit() {

    this.customer = this._customerService.getCurrentCustomer();

    const currtURL = this._route.url;
    if (currtURL.search('editcustomer') > 0 && this.customer.Type === 'Add') {
      this._route.navigate(['view/customer']);
    }

    this._subscriptions.push(this._upsertCustomerService.setNextStep$.subscribe((res: CustomerWizardStepModel) => {
      this.stepModel = res;
    }));
    this._subscriptions.push(this._upsertCustomerService.Loading$.subscribe((res: boolean) => {
      this.loading = res;
    }));
    this._subscriptions.push(this._upsertCustomerService.CurrentReviewType$.subscribe((res: string) => {
      this.ReviewTypeName = res;
    }));
    this._subscriptions.push(this._upsertCustomerService.CurrentLoanType$.subscribe((res: string) => {
      this.LoanTypeName = res;
    }));
  }

  GotoMaster() {
    this._location.back();
  }

  GotoPreviousStep() {
    this._previousStep = true;
    if (this.stepModel.stepID === this.UpsertCustomerSteps.ServiceType) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.Customer, 'active', '', '', '', '', ''));
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.LoanType) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.ServiceType, 'active complete', 'active', '', '', '', ''));
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.Checklist) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.LoanType, 'active complete', 'active complete', 'active', '', '', ''));
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.StackingOrder) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.Checklist, 'active complete', 'active complete', 'active complete', 'active', '', ''));
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.CustomerConfig) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.StackingOrder, 'active complete', 'active complete', 'active complete', 'active  complete', 'active', ''));
    }
  }

  GotoNextStep() {
    if (this.stepModel.stepID === this.UpsertCustomerSteps.Customer) {
      if (this.customer.Type === 'Add') {
        this.AddCustomeSubmit();
      } else {
        this.EditCustomerSubmit();
      }
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.Checklist) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.StackingOrder, 'active complete', 'active complete', 'active complete', 'active  complete', 'active', ''));
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.StackingOrder) {
      this._upsertCustomerService.setNextStep$.next(new CustomerWizardStepModel(this.UpsertCustomerSteps.CustomerConfig, 'active complete', 'active complete', 'active complete', 'active complete', 'active complete', ''));
    } else if (this.stepModel.stepID === this.UpsertCustomerSteps.CustomerConfig) {
      this.GotoMaster();
    }
  }

  AddCustomeSubmit() {
    if (!this._upsertCustomerService.validate()) {
      this._upsertCustomerService.Loading$.next(true);
      this._upsertCustomerService.AddCustomeSubmit();
    }
  }

  EditCustomerSubmit() {
    if (!this._upsertCustomerService.validate() && !this._upsertCustomerService.CheckDuplicate()) {
      this._upsertCustomerService.Loading$.next(true);
      this._upsertCustomerService.AddCustomeSubmit();
    }
  }

  ngOnDestroy() {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
