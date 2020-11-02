import { NotificationService } from './../../../../shared/service/notification.service';
import { ExportLoanService } from './../../service/export-loan.service';
import { ExportWizardStepModel } from './../../models/Export-wizard-steps.model';
import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { Subscription } from 'rxjs';
import { Location } from '@angular/common';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
  selector: 'mts-export-loan-wizard',
  templateUrl: './export-loan-wizard.page.html',
  styleUrls: ['./export-loan-wizard.page.css'],
  providers: [ExportLoanService]
})
export class ExportLoanWizardComponent implements OnInit, OnDestroy {
  slideOneTranClass: any = 'transForm';
  loanExportSteps = this._exportloanservice.loanExportSteps;
  stepModel: ExportWizardStepModel = new ExportWizardStepModel(this.loanExportSteps.LoanSelect, 'active');
  _previousStep = false;
  @ViewChild('PasswordconfirmModal') PasswordconfirmModal: ModalDirective;
  @ViewChild('coverLetterConfirmModal') _coverLetterconfirmModal: ModalDirective;

  isDisabled = true;
  isUpperCaseCharacterExist = 'Red';
  isNumberExist = 'Red';
  isMinLengthValid = 'Red';
  isValid = 'Red';
  To = '';
  Subject = '';
  bodyContent = '';
  constructor(public _exportloanservice: ExportLoanService,
    private _location: Location,
    private _notificationservice: NotificationService) { }
  private _subscriptions: Subscription[] = [];
  private specialCharRegex = new RegExp(/[-!$%^&*()_+@|~=`{}\[\]:";'<>?,.\/]/);
  private numberCheckRegex = new RegExp(/.*[0-9].*/);
  private upperCaseCheckRegex = new RegExp(/[A-Z]]*/);
  private ToMailRegex = new RegExp('^[_a-z0-9]+(.[_a-z0-9]+)*@[a-z0-9]+(.[a-z0-9]+)*(.[a-z]{2,})$');
  private fullRegex = new RegExp(/^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,20}$/);
  ngOnInit(): void {
    this._subscriptions.push(this._exportloanservice.setNextStep$.subscribe((res: ExportWizardStepModel) => {
      this.stepModel = res;
    }));
    this._subscriptions.push(this._exportloanservice.showmodel$.subscribe((res: string) => {
      if (res === 'password') {
        this.PasswordconfirmModal.show();
      } else if (res === 'cover') {
        this._coverLetterconfirmModal.show();
      }
    }));
  }
  backtoBatch() {
    this._location.back();
  }
  SetNext() {
    this._previousStep = false;
    if (this.stepModel.stepID === this.loanExportSteps.LoanSelect) {
      if (this._exportloanservice.loanvalidate()) {
        this._exportloanservice.setNextStep$.next(new ExportWizardStepModel(this.loanExportSteps.DocumentSelect, 'active complete', 'active'));
      }
    } else if (this.stepModel.stepID === this.loanExportSteps.DocumentSelect) {
      if (this._exportloanservice.SaveDocuments()) {
        this._exportloanservice.setNextStep$.next(new ExportWizardStepModel(this.loanExportSteps.Configuration, 'active complete', 'active complete', 'active'));
      }
    } else if (this.stepModel.stepID === this.loanExportSteps.Configuration) {
      this._exportloanservice.SaveBatchDetails();
      this.backtoBatch();
    }
  }
  SetPrevious() {
    if (this.stepModel.stepID === this.loanExportSteps.DocumentSelect) {
      this._previousStep = true;
      this._exportloanservice.setNextStep$.next(new ExportWizardStepModel(this.loanExportSteps.LoanSelect, 'active'));
    } else if (this.stepModel.stepID === this.loanExportSteps.Configuration) {
      this._exportloanservice.setNextStep$.next(new ExportWizardStepModel(this.loanExportSteps.DocumentSelect, 'active complete', 'active'));
    }
  }
  savePassword() {
    if (this._exportloanservice.Password !== undefined && this._exportloanservice.Password !== '') {
      if (this._exportloanservice.Password === this._exportloanservice.ConfirmPassword) {
        this._exportloanservice.PasswordProtected = true;
        this.PasswordconfirmModal.hide();
      } else {
        this._notificationservice.showError('Passwords don\'t match');
      }
    } else {
      this._notificationservice.showError('Passowrd Required');
    }
  }
  CancelPassword() {
    this.PasswordconfirmModal.hide();
    this._exportloanservice.PasswordProtected = false;
  }
  CheckPasswordPolicy() {
    if (this.specialCharRegex.test(this._exportloanservice.Password)) {
      this.isValid = 'green';
    } else {
      this.isValid = 'red';
    }
    if (this._exportloanservice.Password.length >= 8 && this._exportloanservice.Password.length <= 20) {
      this.isMinLengthValid = 'green';
    } else {
      this.isMinLengthValid = 'red';
    }
    if (this.numberCheckRegex.test(this._exportloanservice.Password)) {
      this.isNumberExist = 'green';
    } else {
      this.isNumberExist = 'red';
    }
    if (this.upperCaseCheckRegex.test(this._exportloanservice.Password)) {
      this.isUpperCaseCharacterExist = 'green';
    } else {
      this.isUpperCaseCharacterExist = 'Red';
    }
    if (this.fullRegex.test(this._exportloanservice.Password)) {
      this.isDisabled = false;
    } else {
      this.isDisabled = true;
    }
  }
  SaveCoverLetterContent() {
    const letterContent = { To: this.To, Subject: this.Subject, Body: this.bodyContent };
    if (!this.CoverLetterValidation()) {
      this._exportloanservice.CoverLetterContent = JSON.stringify(letterContent);
      this._coverLetterconfirmModal.hide();
    }
  }
  CoverLetterValidation(): boolean {
    let returValue = false;
    if (!isTruthy(this.To) || !isTruthy(this.Subject) || !isTruthy(this.bodyContent)) {
      this._notificationservice.showError('All fields are required');
      returValue = true;
    }
    return returValue;
  }
  CancelCoverLetter() {
    this._coverLetterconfirmModal.hide();
    this._exportloanservice.CoverLetter = false;
  }
  ngOnDestroy() {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
