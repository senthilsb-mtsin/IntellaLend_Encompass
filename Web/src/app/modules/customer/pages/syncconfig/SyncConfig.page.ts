import { Component, OnInit, AfterViewInit, ViewChild, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { CustLoanTypeMappingModel } from '../../models/sync-config-mapping.model';
import { CustomerService } from '../../services/customer.service';
import { SyncConfigService } from '../../services/syncconfig.service';

@Component({
  selector: 'mts-syncconfig',
  templateUrl: 'SyncConfig.page.html',
  styleUrls: ['SyncConfig.page.css'],
})
export class SyncConfigComponent implements OnInit, AfterViewInit, OnDestroy {
  CustLoanTypeMappingModel;
  CustLoanTypeMapped: CustLoanTypeMappingModel[] = [];
  _loanTypeID: number;
  _documentID: number;
  promise: Subscription;
  _customerName: string;

  constructor(
    private _customerService: CustomerService,
    private _syncConfigService: SyncConfigService) { }
  private _subscriptions: Subscription[] = [];

  ngAfterViewInit(): void {
  }
  ngOnInit(): void {
    this._customerName = this._customerService.getCurrentCustomer().CustomerName;
    this.promise = this._syncConfigService.getCustLoanTypeMapping(this._customerService.getCurrentCustomer().CustomerID);

    this._subscriptions.push(this._syncConfigService.loanTypesMapped$.subscribe((res: CustLoanTypeMappingModel[]) => {
      this.CustLoanTypeMapped = res;
    }));

  }

  SetLoanTypeSync(custLoanType: CustLoanTypeMappingModel) {
    custLoanType.LoanTypeSync = !custLoanType.LoanTypeSync;
  }

  SetDocumentTypeSync(custLoanType: CustLoanTypeMappingModel) {
    custLoanType.DocumentTypeSync = !custLoanType.DocumentTypeSync;
  }

  CloseLoanTypeMapping() {
    this._syncConfigService.CloseLoanTypeMapping();
  }
  SaveCustLoanTypeMapping() {
    this._syncConfigService.UpdateCustLoanTypeMapping(this.CustLoanTypeMapped);
  }
  ngOnDestroy() {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
