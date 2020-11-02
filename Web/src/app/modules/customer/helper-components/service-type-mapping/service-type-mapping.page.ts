import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { UpsertCustomerService } from '../../services/upsert-customer.service';
import { Subscription } from 'rxjs';
import { ReviewTypeMappingModel } from '../../models/review-type-mapping.model';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
    selector: 'mts-service-type-mapping',
    styleUrls: ['service-type-mapping.page.css'],
    templateUrl: 'service-type-mapping.page.html'
})
export class ServiceTypeMappingComponent implements OnInit, OnDestroy {

    ReviewTypesMapped: ReviewTypeMappingModel[] = [];
    ReviewTypeName = '';
    _currtReviewType: ReviewTypeMappingModel;
    @ViewChild('confirmModal') confirmModal: ModalDirective;
    @ViewChild('retainConfirm') retainConfirm: ModalDirective;

    constructor(
        private _upsertCustomerService: UpsertCustomerService
    ) { }

    private _subscriptions: Subscription[] = [];
    private _reviewTypeID = 0;

    ngOnInit() {
        this._subscriptions.push(this._upsertCustomerService.reviewTypesMapped$.subscribe((res: ReviewTypeMappingModel[]) => {
            this.ReviewTypesMapped = res;
        }));
        this._subscriptions.push(this._upsertCustomerService.retainConfirm$.subscribe((res: boolean) => {
            res ? this.retainConfirm.show() : this.retainConfirm.hide();
        }));
        this._subscriptions.push(this._upsertCustomerService.confirmModal$.subscribe((res: boolean) => {
            res ? this.confirmModal.show() : this.confirmModal.hide();
        }));

        this._upsertCustomerService.GetMappedReviewTypes();
    }

    SetReviewType(reviewType: ReviewTypeMappingModel) {
        reviewType.Mapped = !reviewType.Mapped;
        this._reviewTypeID = reviewType.ReviewTypeID;
        this.ReviewTypeName = reviewType.ReviewTypeName;
        this._currtReviewType = reviewType;
        if (reviewType.Mapped) {
            reviewType.loading = true;
            this.CheckCustReviewMapping();
        } else if (reviewType.DBMapped) {
            this.confirmModal.show();
        }
    }
    SetCustReviewMapping() {
        this._upsertCustomerService.SetCustReviewMapping(this._currtReviewType);
    }

    RetainMapping() {
        this._upsertCustomerService.RetainCustReviewMapping(this._currtReviewType);
    }

    RemoveMapping() {
        this._upsertCustomerService.RemoveCustReviewMapping(this._currtReviewType);
    }

    CheckCustReviewMapping() {
        this._upsertCustomerService.CheckCustReviewMapping(this._currtReviewType);
    }

    RevertMappedReviewType() {
        this._currtReviewType.Mapped = !this._currtReviewType.Mapped;
        this.confirmModal.hide();
    }

    GetLoanTypes(vals: ReviewTypeMappingModel) {
        this._upsertCustomerService.SetSelectedReviewType(vals);
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
