import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { CustomerDatatableModel } from '../models/customer-datatable.model';
import { CustomerData } from '../customer.data';
import { JwtHelperService } from '@auth0/angular-jwt';

const jwtHelper = new JwtHelperService();

@Injectable()
export class CustomerService {
    customerMasterTable$ = new Subject<CustomerDatatableModel[]>();

    constructor(
        private _customerData: CustomerData
    ) { }

    private _customerMaster: CustomerDatatableModel[] = [];
    private _currentCustomer: CustomerDatatableModel = new CustomerDatatableModel();

    getCustomerMaster(req: { TableSchema: string }) {
        return this._customerData.GetCustomerMaster(req).subscribe((res) => {
            if (res !== null) {
                const result = jwtHelper.decodeToken(res.Data)['data'];
                this._customerMaster = [...result];
                this.customerMasterTable$.next(this._customerMaster.slice());
            }
        });
    }

    setCurrectCustomer(selectedCustomer: CustomerDatatableModel) {
        this._currentCustomer = selectedCustomer;
    }

    getCustomerMasterTable(): CustomerDatatableModel[] {
        return this._customerMaster.slice();
    }

    getCurrentCustomer() {
        return this._currentCustomer;
    }

    clearCurrentCustomer() {
        this._currentCustomer = new CustomerDatatableModel();
    }
}
