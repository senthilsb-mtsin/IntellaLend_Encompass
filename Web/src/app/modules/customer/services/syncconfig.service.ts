import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomerDatatableModel } from '../models/customer-datatable.model';
import { CustomerData } from '../customer.data';
import { CustLoanTypeMappingModel } from '../models/sync-config-mapping.model';
import { AppSettings } from '@mts-app-setting';
import { Location } from '@angular/common';
import { NotificationService } from '@mts-notification';

const jwtHelper = new JwtHelperService();

@Injectable()
export class SyncConfigService {
    customerMasterTable$ = new Subject<CustomerDatatableModel[]>();
    loanTypesMapped$ = new Subject<CustLoanTypeMappingModel[]>();

    _customerID: number;
    _customerName: string;

    constructor(
        private _customerData: CustomerData,
        private location: Location,
        private _notificationService: NotificationService
    ) { }
    private _customerMaster: CustomerDatatableModel[] = [];
    private _currentCustomer: CustomerDatatableModel = new CustomerDatatableModel();

    getCustomerMaster() {
        const req = { TableSchema: AppSettings.TenantSchema };
        return this._customerData.GetCustomerMaster(req).subscribe((res) => {
            if (res !== null) {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                this._customerMaster = [...result];
                this.customerMasterTable$.next(this._customerMaster.slice());
            }
        });
    }

    getCustLoanTypeMapping(_customerID: number) {
        const req = { TableSchema: AppSettings.TenantSchema, CustomerID: _customerID};
        return this._customerData.GetCustLoanTypeMapping(req).subscribe((res) => {
            if (res !== null) {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                this.loanTypesMapped$.next(result);
            }
        });
    }

    setCurrectCustomer(selectedCustomer: CustomerDatatableModel) {
        this._customerID = selectedCustomer.CustomerID;
        this._customerName = selectedCustomer.CustomerName;
        this._currentCustomer = selectedCustomer;
    }
    CloseLoanTypeMapping() {
        this.location.back();
      }
      UpdateCustLoanTypeMapping(inputData: CustLoanTypeMappingModel[]) {
        const req = { TableSchema: AppSettings.TenantSchema, lsCustLoanTypeMappings: inputData};
        this._customerData.UpdateCustLoanTypeMapping(req).subscribe(
          res => {
            if (res !== null) {
              const Data = jwtHelper.decodeToken(res.Data)['data'];
              if (Data === true) {
                  this._notificationService.showSuccess('Sync Configuration Updated Successfully');
                  this.CloseLoanTypeMapping();
                } else {
                  this._notificationService.showError('Sync Configuration Update Failed');
                }
            }
          });
      }

    getCurrentCustomer() {
        return this._currentCustomer;
    }

    clearCurrentCustomer() {
        this._currentCustomer = new CustomerDatatableModel();
    }
}
