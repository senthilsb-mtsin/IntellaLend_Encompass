import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { LoanTypeSearchService } from '../../service/loantype-search.service';
import { AddServiceTypeService } from '../../service/add-service-type.service';
import { Subscription } from 'rxjs';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { LoanTypeMappingModel } from '../../models/loan-type-mapping.model';
import { NotificationService } from '@mts-notification';
import { element } from 'protractor';
import { AssignLoanTypesRequestModel } from '../../models/assign-loan-types-request.model';
import { ServiceTypeModel } from '../../models/service-type.model';

@Component({
    selector: 'mts-assign-loan-type',
    styleUrls: ['assign-loan-type.component.css'],
    templateUrl: 'assign-loan-type.component.html',
    // providers: [LoanTypeSearchService],
})
export class AssignLoanTypesComponent implements OnInit, OnDestroy {

    AllLoanTypes: any[] = [];
    LoanTypesMapped: any[] = [];
    LoanTypeName = '';
    ReviewDetails: ServiceTypeModel;
    AssignedLoanTypes: any[] = [];
    @ViewChild('loanConfirmModal') loanConfirmModal: ModalDirective;
    @ViewChild('loanRetainConfirm') loanRetainConfirm: ModalDirective;

    constructor(
        private _addServiceTypeService: AddServiceTypeService,
        // private _loantypeSearch: LoanTypeSearchService,
        private _notificationService: NotificationService
    ) { }

    private _subscriptions: Subscription[] = [];
    private _loanTypeID = 0;
    private _currtLoanType: LoanTypeMappingModel;

    ngOnInit() {
        this._subscriptions.push(this._addServiceTypeService.loanRetainConfirm$.subscribe((res: boolean) => {
            res ? this.loanRetainConfirm.show() : this.loanRetainConfirm.hide();
        }));
        this._subscriptions.push(this._addServiceTypeService.loanConfirmModal$.subscribe((res: boolean) => {
            res ? this.loanConfirmModal.show() : this.loanConfirmModal.hide();
        }));

        this.AllLoanTypes = this._addServiceTypeService.getAllLoanTypes();
        this.ReviewDetails = this._addServiceTypeService.getCurrentReviewDetails();
        this.AssignedLoanTypes = this._addServiceTypeService.getAssignedLoanTypes();
        this._addServiceTypeService.getSysLoanTypes();
        this._addServiceTypeService.allAssignedLoanTypes.forEach((ele) => {
            this.LoanTypesMapped.push({LoanTypeID: ele.LoanTypeID, LoanTypeName: ele.LoanTypeName, loading: false, DBMapped: ele.DBMapped, Mapped: ele.Mapped });
        });
    }

    SetLoanType(loanType: LoanTypeMappingModel) {
        this._currtLoanType = loanType;
        loanType.Mapped = !loanType.Mapped;
        this._loanTypeID = loanType.LoanTypeID;
        this.LoanTypeName = loanType.LoanTypeName;
        if (loanType.Mapped) {
            loanType.loading = true;
            this.CheckCustReviewLoanMapping(loanType);
        } else if (loanType.DBMapped) {
            this.loanConfirmModal.show();
        }

    }

    RevertLoanType() {
        if (this._currtLoanType.Mapped) {
            this._currtLoanType.Mapped = false;
            this.loanRetainConfirm.hide();
        }
    }

    RevertMappedLoanType() {
        this._currtLoanType.Mapped = (this._currtLoanType.Mapped) ? false : true;
        this.loanConfirmModal.hide();
    }

    RemoveLoanMapping() {
        this._addServiceTypeService.RemoveReviewLoanMapping(this._currtLoanType);
    }

    CheckCustReviewLoanMapping(vals: LoanTypeMappingModel) {
        this._addServiceTypeService.CheckCustReviewLoanMapping(vals);
    }

    CancelLoanTypeConfirm() {
        this.loanConfirmModal.hide();
        this._currtLoanType.Mapped = !this._currtLoanType.Mapped;
    }

    GetLenders(vals: LoanTypeMappingModel) {
        this._addServiceTypeService.SetSelectedLoanType(vals);
    }

    GetCheckList(vals: LoanTypeMappingModel) {
        // this._upsertCustomerService.SetSelectedLoanType(vals);
    }

    ngOnDestroy() {
        this._subscriptions.forEach(ele => {
            ele.unsubscribe();
        });
    }
}
