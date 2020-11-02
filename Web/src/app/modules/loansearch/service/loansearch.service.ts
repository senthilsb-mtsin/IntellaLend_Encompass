import { Injectable } from '@angular/core';
import { LoanSearchDataAccess } from '../loansearch.data';
import { GetLoanSearchFilterRequest } from '../models/get-loan-search-filters.request';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Subject } from 'rxjs';
import { LoanSearchRequestModel } from '../models/loan-search-request.model';
import { DeleteLoanRequestModel } from '../models/loan-delete-request.model';
import { NotificationService } from '@mts-notification';
import { LoanSearchTableModel } from '../models/loan-search-table.model';
import { AppSettings } from '@mts-app-setting';
import { OrderByPipe } from '@mts-pipe';

const jwtHelper = new JwtHelperService();

@Injectable()
export class LoanSearchService {
  confimModelHide = new Subject<boolean>();
  loanFilterResult = new Subject<any>();
  searchData = new Subject<LoanSearchTableModel[]>();
  loanTypeMaster = new Subject<{ id: any, text: any }[]>();
  reviewTypeMaster = new Subject<{ ReviewTypeID: any, ReviewTypeName: any }[]>();
  customerMaster = new Subject<{ id: any, text: any }[]>();
  workFlowMaster = new Subject<{ id: any, text: any }[]>();

  constructor(
    private _loanSearchData: LoanSearchDataAccess,
    private _notificationService: NotificationService,
    private _orderBy: OrderByPipe
  ) { }

  private _searchData: LoanSearchTableModel[] = [];
  private _searchValues: LoanSearchRequestModel;

  GetLoanSearchFilterConfigValue(req: GetLoanSearchFilterRequest) {
    this._loanSearchData
      .GetLoanSearchFilterConfigValue(req)
      .subscribe((res) => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        this.loanFilterResult.next(result);
      });
  }

  GetLoanTypeMaster() {
    const req = { TableSchema: AppSettings.TenantSchema };
    this._loanSearchData.GetLoanTypeMaster(req).subscribe((res) => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      const commonLoanTypeItems = [];
      if (Result.length > 0) {
        Result.forEach(element => {
          commonLoanTypeItems.push({ id: element.LoanTypeID, text: element.LoanTypeName });
        });
        this._orderBy.transform(commonLoanTypeItems, { property: 'text', direction: 1 });
      }
      this.loanTypeMaster.next(commonLoanTypeItems.slice());
    });
  }

  GetReviewTypeMaster() {
    const req = { TableSchema: AppSettings.TenantSchema };
    this._loanSearchData.GetReviewTypeMaster(req).subscribe((res) => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      const commonServiceItems = [];
      if (Result.length > 0) {
        Result.forEach(element => {
          commonServiceItems.push({ ReviewTypeID: element.ReviewTypeID, ReviewTypeName: element.ReviewTypeName });
        });
        this._orderBy.transform(commonServiceItems, { property: 'ReviewTypeName', direction: 1 });
      }
      this.reviewTypeMaster.next(commonServiceItems.slice());
    });
  }

  GetCustomerMaster() {
    const req = { TableSchema: AppSettings.TenantSchema };
    this._loanSearchData.GetCustomerMaster(req).subscribe((res) => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      const commonCustomerItems = [];
      if (Result.length > 0) {
        Result.forEach(element => {
          commonCustomerItems.push({ id: element.CustomerID, text: element.CustomerName });
        });
        this._orderBy.transform(commonCustomerItems, { property: 'text', direction: 1 });
      }
      this.customerMaster.next(commonCustomerItems.slice());
    });
  }

  GeWorkFlowMaster() {
    this._loanSearchData.GetWorkFlowMaster().subscribe((res) => {
      const Result = jwtHelper.decodeToken(res.Data)['data'];
      const commonWorkflowItems = [];
      if (Result.length > 0) {
        Result.forEach(element => {
          commonWorkflowItems.push({ id: element.StatusID, text: element.StatusDescription });
        });
        this._orderBy.transform(commonWorkflowItems, { property: 'text', direction: 1 });
      }
      this.workFlowMaster.next(commonWorkflowItems.slice());
    });
  }

  searchSubmit(req: LoanSearchRequestModel) {
    this._searchValues = req;
    return this._loanSearchData.searchSubmit(req).subscribe((res) => {
      const loanData = jwtHelper.decodeToken(res.Data)['data'];
      this._searchData = loanData;
      this.searchData.next(this._searchData.slice());
    });
  }

  DeleteLoans(req: DeleteLoanRequestModel) {
    this._loanSearchData.deleteLoan(req).subscribe((res) => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      if (data) {
        this.confimModelHide.next(true);
        this._notificationService.showSuccess('Loan Deleted Successfully');
        this.searchSubmit(this._searchValues);
      } else {
        this._notificationService.showError('Loan Not Deleted');
      }
    });
  }
}
