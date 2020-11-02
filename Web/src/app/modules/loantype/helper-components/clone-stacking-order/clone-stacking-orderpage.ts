import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonService } from 'src/app/shared/common/common.service';
import { Subscription } from 'rxjs';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { NotificationService } from '@mts-notification';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-clone-stacking-order',
    styleUrls: ['clone-stacking-order.page.css'],
    templateUrl: 'clone-stacking-order.page.html',
})
export class CloneStackingOrderComponent implements OnInit, OnDestroy {
    stackingOrderList: any[] = [];
    stackDropValue: any = 0;
    sysEditStackingOrderName = '';
    assignedDocSearchval = '';
    docSysStackingOrder: any[] = [];
    docGroupStackingOrder: any[] = [];
    showDocuments = false;
    promise: Subscription;
    stackOrderType = 'clone';
    constructor(
        private _commonService: CommonService,
        private _notificationService: NotificationService,
        private _addLoanTypeService: AddLoanTypeService
    ) { }

    private _subscription: Subscription[] = [];

    ngOnInit() {

        this.stackingOrderList = this._commonService.GetSysStackingOrderGroup();
        this.stackOrderType = this._addLoanTypeService.getStackType();
        this._subscription.push(this._commonService.SystemStackingOrderMaster.subscribe(
            (res: any[]) => {
                this.stackingOrderList = res;
            }
        ));

        this._subscription.push(this._addLoanTypeService.SysStackingOrderDetailData.subscribe(
            (res: { StackingOrder: any[], StackingOrderGroup: any[], UnAssignedDocTypes: any[] }) => {
                this.docSysStackingOrder = res.StackingOrder;
                this.docGroupStackingOrder = res.StackingOrderGroup;
            }
        ));

        this._subscription.push(this._addLoanTypeService.stackingOrderType.subscribe((res: string) => {
            this.stackOrderType = res;
        }));

    }

    AssignStackingOrder() {
        if (isTruthy(this.stackDropValue) && this.stackDropValue.StackingOrderID !== 0 && this.sysEditStackingOrderName !== '') {
            if (this.stackingOrderList.filter(x => x.Description.toUpperCase() === this.sysEditStackingOrderName.toUpperCase()).length === 0) {
                this._addLoanTypeService.AssignStackingOrder();
            } else {
                this._notificationService.showError('Stacking Order Already Exists !');
            }
        }
    }

    SysStackingOrderRedundancyCheck(vals: string) {
        this._addLoanTypeService.checkDuplicateStackingOrderGrp(vals);
    }

    EditStackingMaster() {
        if (this.stackDropValue !== 0 && this.stackDropValue.StackingOrderID !== 0) {
            this.sysEditStackingOrderName = this.stackDropValue.Description;
            this._addLoanTypeService.setSystemStackingOrder(this.stackDropValue);
            this.showDocuments = true;
            this.promise = this._addLoanTypeService.getSystemStackingOrderDetails();
        } else {
            this.sysEditStackingOrderName = '';
            this._addLoanTypeService.setSystemStackingOrder({ StackingOrderID: 0, Description: '' });
            this._addLoanTypeService.SysStackingOrderDetailData.next({ StackingOrder: [], StackingOrderGroup: [], UnAssignedDocTypes: [] });
            this.showDocuments = false;
            this._notificationService.showError('Select Stacking Order');
        }
    }

    CreateClose() {
        this._addLoanTypeService.stackingOrderBack.next('');
    }

    ngOnDestroy() {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
    }
}
