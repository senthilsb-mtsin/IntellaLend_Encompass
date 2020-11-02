import { Component, ViewChild, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoanExportWizardStepModel } from '../../models/loan-export-wizard-step.model';
import { LoanExportModel } from '../../models/loan-export.mode';
import { LoanInfoService } from '../../services/loan-info.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Location } from '@angular/common';
import { NotificationService } from '@mts-notification';
import { SessionHelper } from '@mts-app-session';
import { AppSettings } from '@mts-app-setting';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'mts-loan-detail-export',
    templateUrl: 'loan-detail-export.page.html',
    styleUrls: ['loan-detail-export.page.css']
})
export class LoanDetailExportComponent implements OnInit, OnDestroy {
    @ViewChild('coverLetterConfirmModal') _coverLetterconfirmModal: ModalDirective;
    @ViewChild('PasswordconfirmModal') PasswordconfirmModal: ModalDirective;
    stepModel: LoanExportWizardStepModel = new LoanExportWizardStepModel(0, 'active', '');
    slideOneTranClass: any = 'transForm';
    loanExportData: LoanExportModel;
    _btnSearchClick = false;
    _showSelectall = true;
    NoData = false;
    selectAllDocBtn = true;
    _selectDocuments = '';
    To = '';
    Subject = '';
    bodyContent = '';
    isValid: any = 'Red';
    isMinLengthValid: any = 'Red';
    isNumberExist: any = 'Red';
    isUpperCaseCharacterExist: any = 'Red';
    isDisabled = true;

    constructor(
        private _loanInfoService: LoanInfoService,
        private _location: Location,
        private _notificationService: NotificationService
    ) { }

    private _subscriptions: Subscription[] = [];
    private specialCharRegex = new RegExp(/[-!$%^&*()_+@|~=`{}\[\]:";'<>?,.\/]/);
    private numberCheckRegex = new RegExp(/.*[0-9].*/);
    private upperCaseCheckRegex = new RegExp(/[A-Z]]*/);
    private fullRegex = new RegExp(/^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,20}$/);

    ngOnInit(): void {
        this.loanExportData = new LoanExportModel(0, '', '', 0, false, false, false, '', '', '', []);
        this.SetLoanExportData();

        this._subscriptions.push(this._loanInfoService.LoanExportBack$.subscribe((res: boolean) => {
            if (res) { this._location.back(); }
        }));

    }

    CancelPassword() {
        this.PasswordconfirmModal.hide();
        this.loanExportData.PasswordProtected = false;
    }

    savePassword() {
        if (this.loanExportData.Password !== undefined && this.loanExportData.Password !== '') {
            if (this.loanExportData.Password === this.loanExportData.ConfirmPassword) {
                this.loanExportData.PasswordProtected = true;
                this.PasswordconfirmModal.hide();
            } else {
                this._notificationService.showError('Passwords don\'t match');
            }
        } else {
            this._notificationService.showError('Passowrd Required');
        }
    }

    CheckPasswordPolicy() {
        if (this.specialCharRegex.test(this.loanExportData.Password)) {
            this.isValid = 'green';
        } else {
            this.isValid = 'red';
        }

        if (this.loanExportData.Password.length >= 8 && this.loanExportData.Password.length <= 20) {
            this.isMinLengthValid = 'green';
        } else {
            this.isMinLengthValid = 'red';
        }

        if (this.numberCheckRegex.test(this.loanExportData.Password)) {
            this.isNumberExist = 'green';
        } else {
            this.isNumberExist = 'red';
        }
        if (this.upperCaseCheckRegex.test(this.loanExportData.Password)) {
            this.isUpperCaseCharacterExist = 'green';
        } else {
            this.isUpperCaseCharacterExist = 'Red';
        }
        if (this.fullRegex.test(this.loanExportData.Password)) {
            this.isDisabled = false;
        } else {
            this.isDisabled = true;
        }
    }

    CancelCoverLetter() {
        this._coverLetterconfirmModal.hide();
        this.loanExportData.CoverLetter = false;
    }

    SaveCoverLetterContent() {
        const letterContent = { To: this.To, Subject: this.Subject, Body: this.bodyContent };
        if (!this.CoverLetterValidation()) {
            this.loanExportData.CoverLetterContent = JSON.stringify(letterContent);
            this._coverLetterconfirmModal.hide();
        }
    }

    CoverLetterValidation(): boolean {
        let returValue = false;
        if (!isTruthy(this.To) || !isTruthy(this.Subject) || !isTruthy(this.bodyContent)) {
            this._notificationService.showError('All fields are required');
            returValue = true;
        }
        return returValue;
    }

    ShowCoverLetter() {
        if (this.loanExportData.CoverLetter) {
            this._coverLetterconfirmModal.show();
        }
    }

    SetLoanExportData() {
        this.loanExportData.LoanDetail.push({
            LoanID: this._loanInfoService.GetLoan().LoanID,
            LoanNumber: (isTruthy(this._loanInfoService.GetLoanTableDetails().LoanNumber)) ? this._loanInfoService.GetLoanTableDetails().LoanNumber : this._loanInfoService.GetLoan().LoanID,
            IsSelected: false,
            Documents: '',
            CurrentLoan: false,
            DocumentDetails: []
        });
        this.loanExportData.LoanDetail.forEach(dc => {
            dc.CurrentLoan = true;
            this._loanInfoService.GetLoan().loanDocuments.forEach(element => {
                if (element.DocumentLevelIcon === 'Success' && !element.Obsolete) {
                    dc.DocumentDetails.push({
                        IsChecked: false,
                        DocumentTypeId: element.DocID,
                        VersionNumber: element.VersionNumber,
                        DocumentTypeName:
                            (this._loanInfoService.CheckMoreThanOnce(element.DocName) ? element.DocName + ' -V' + (element.FieldOrderBy === '' ? (element.VersionNumber).toString()
                                : this._loanInfoService.GetFieldVersionNumber(element.DocName, element.FieldOrderVersion)) : element.DocName)
                    });
                }
            });
        });
    }

    SaveDocuments() {
        const LoanDocCount = [];
        this.loanExportData.LoanDetail.forEach(element => {
            const detail = element.DocumentDetails.filter(d => d.IsChecked === true);
            if (typeof detail !== 'undefined' && detail.length > 0) {
                const docstring = [];
                detail.forEach(dd => {
                    docstring.push(
                        {
                            DocumentTypeId: dd.DocumentTypeId,
                            VersionNumber: dd.VersionNumber
                        });
                });
                element.Documents = JSON.stringify(docstring);
                LoanDocCount.push(element.LoanID);
            }
        });
        if (LoanDocCount.length <= 0) {
            this._notificationService.showError('Select Atleast one Document');
        } else if (LoanDocCount.length === this.loanExportData.LoanDetail.length) {
            this.stepModel = new LoanExportWizardStepModel(1, 'active complete', 'active');
        } else {
            this._notificationService.showError('Document is not mapped with selected loan');
        }
    }

    setNext(nextclass) {
        if (nextclass === 1) {
            this.SaveDocuments();
        }
        if (nextclass === 2) {
            this.SaveBatchDetails();
        }
    }

    SetPasswordConfig(isactive: any) {
        if (isactive === true) {
            this.loanExportData.Password = '';
            this.loanExportData.ConfirmPassword = '';
            this.PasswordconfirmModal.show();
        } else {
            this.loanExportData.PasswordProtected = false;
            this.loanExportData.Password = '';
            this.loanExportData.ConfirmPassword = '';
        }
    }

    SaveBatchDetails() {
        const req = {
            TableSchema: AppSettings.TenantSchema,
            JobName: this.loanExportData.JobName,
            ExportedBy: SessionHelper.UserDetails.UserID,
            CustomerID: this.loanExportData.CustomerID,
            CoverLetter: this.loanExportData.CoverLetter,
            TableOfContent: this.loanExportData.TableOfContent,
            PasswordProtected: this.loanExportData.PasswordProtected,
            Password: this.loanExportData.Password,
            CoverLetterContent: this.loanExportData.CoverLetterContent,
            BatchLoanDoc: this.loanExportData.LoanDetail
        };
        this._loanInfoService.SaveLoanBatchDetails(req);
    }

    setPrevious(prevDiv) {
        if (prevDiv === 0) {
            this.stepModel = new LoanExportWizardStepModel(0, 'active', '');
        }
    }

    backtoBatch() {
        this._location.back();
    }

    DocumentSelect(LoanID: any) {
        this.loanExportData.LoanDetail.forEach(x => {
            const checkIsDocSelected = x.DocumentDetails.filter(d => d.IsChecked === true);
            if (isTruthy(checkIsDocSelected) && checkIsDocSelected.length > 0 && x.LoanID === LoanID) {
                x.IsSelected = true;
            } else {
                x.IsSelected = (x.LoanID === LoanID) ? false : x.IsSelected;
            }
        });
    }

    SelectAllDoc() {
        this._selectDocuments = '';
        if (this.selectAllDocBtn) {
            this.loanExportData.LoanDetail.forEach(element => {
                if (element.CurrentLoan === true) {
                    element.DocumentDetails.forEach(doc => {
                        doc.IsChecked = true;
                        element.IsSelected = true;
                    });
                }
            });

            this.selectAllDocBtn = false;
        } else {
            this.loanExportData.LoanDetail.forEach(element => {
                if (element.CurrentLoan === true) {
                    element.DocumentDetails.forEach(doc => {
                        doc.IsChecked = false;
                        element.IsSelected = false;
                    });
                }
            });
            this.selectAllDocBtn = true;
        }
    }

    ngOnDestroy(): void {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
