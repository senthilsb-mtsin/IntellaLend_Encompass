import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LoanTypeMappingModel } from '../../models/loan-type-mapping.model';
import { NotificationService } from '@mts-notification';

@Component({
    selector: 'mts-loan-type-mapping',
    styleUrls: ['loan-type-mapping.page.css'],
    templateUrl: 'loan-type-mapping.page.html'
})
export class CustLoanTypeMappingComponent implements OnInit, OnDestroy {

    LoanTypesMapped: LoanTypeMappingModel[] = [];
    LoanTypeName = '';
    @ViewChild('loanConfirmModal') loanConfirmModal: ModalDirective;
    @ViewChild('loanRetainConfirm') loanRetainConfirm: ModalDirective;

    constructor(
        private _upsertCustomerService: UpsertCustomerService,
        private _notificationService: NotificationService
    ) { }

    private _subscriptions: Subscription[] = [];
    private _loanTypeID = 0;
    private _currtLoanType: LoanTypeMappingModel;
    private _boxUploadPath = '';
    private _loanUploadPath = '';

    ngOnInit() {
        this._subscriptions.push(this._upsertCustomerService.loanTypesMapped$.subscribe((res: LoanTypeMappingModel[]) => {
            this.LoanTypesMapped = res;
        }));
        this._subscriptions.push(this._upsertCustomerService.loanRetainConfirm$.subscribe((res: boolean) => {
            res ? this.loanRetainConfirm.show() : this.loanRetainConfirm.hide();
        }));
        this._subscriptions.push(this._upsertCustomerService.loanConfirmModal$.subscribe((res: boolean) => {
            res ? this.loanConfirmModal.show() : this.loanConfirmModal.hide();
        }));

        this._upsertCustomerService.GetMappedLoanTypes();
    }

    SetLoanType(loanType: LoanTypeMappingModel) {
        this._boxUploadPath = loanType.BoxUploadPath;
        this._loanUploadPath = loanType.LoanUploadPath;
        const regEx = /^\\\\([^\\:\|\[\]\/";<>+=,?* _]+)\\([\u0020-\u0021\u0023-\u0029\u002D-\u002E\u0030-\u0039\u0040-\u005A\u005E-\u007B\u007E-\u00FF]{1,80})(((?:\\[\u0020-\u0021\u0023-\u0029\u002D-\u002E\u0030-\u0039\u0040-\u005A\u005E-\u007B\u007E-\u00FF]{1,255})+?|)(?:\\((?:[\u0020-\u0021\u0023-\u0029\u002B-\u002E\u0030-\u0039\u003B\u003D\u0040-\u005B\u005D-\u007B]{1,255}){1}(?:\:(?=[\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]|\:)(?:([\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]+(?!\:)|[\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]*)(?:\:([\u0001-\u002E\u0030-\u0039\u003B-\u005B\u005D-\u00FF]+)|))|)))|)$/;
        const regEx1 = /^([a-zA-Z]:)?(\\[a-zA-Z0-9_\-]+)+\\?/;
        const RegTest = regEx.test(this._loanUploadPath);
        const RegTest1 = regEx1.test(this._loanUploadPath);
        this._currtLoanType = loanType;
        if (this._loanUploadPath.length === 0) {
            loanType.Mapped = !loanType.Mapped;
            this._loanTypeID = loanType.LoanTypeID;
            this.LoanTypeName = loanType.LoanTypeName;
            if (loanType.Mapped) {
                loanType.loading = true;
                this.CheckCustReviewLoanMapping();
            } else if (loanType.DBMapped) {
                this.loanConfirmModal.show();
            }
        } else if (RegTest || RegTest1) {
            loanType.Mapped = !loanType.Mapped;
            this._loanTypeID = loanType.LoanTypeID;
            this.LoanTypeName = loanType.LoanTypeName;
            if (loanType.Mapped) {
                loanType.loading = true;
                this.CheckCustumerLoanUploadpath();
            } else if (loanType.DBMapped) {
                this.loanConfirmModal.show();
            }

        } else {
            this._currtLoanType.Mapped = true;
            this._notificationService.showError('Loan Upload Path Is Invalid');
        }

    }

    RevertLoanType() {
        if (this._currtLoanType.Mapped) {
            this._currtLoanType.Mapped = false;
            this.loanRetainConfirm.hide();
        }
    }

    SetCustReviewLoanMapping() {
        this._upsertCustomerService.SetCustReviewLoanMapping(this._currtLoanType);
    }

    RevertMappedLoanType() {
        this._currtLoanType.Mapped = (this._currtLoanType.Mapped) ? false : true;
        this.loanConfirmModal.hide();
    }

    RetainLoanMapping() {
        this._upsertCustomerService.RetainLoanTypeMapping(this._currtLoanType);
    }

    RemoveLoanMapping() {
        this._upsertCustomerService.RemoveCustReviewLoanMapping(this._currtLoanType);
    }

    CheckCustumerLoanUploadpath() {
        this._upsertCustomerService.CheckCustumerLoanUploadpath(this._currtLoanType);
    }

    CheckCustReviewLoanMapping() {
        this._upsertCustomerService.CheckCustReviewLoanMapping(this._currtLoanType);
    }

    CancelLoanTypeConfirm() {
        this.loanConfirmModal.hide();
        this._currtLoanType.Mapped = !this._currtLoanType.Mapped;
    }

    GetCheckList(vals: LoanTypeMappingModel) {
        this._upsertCustomerService.SetSelectedLoanType(vals);
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
