import { NotificationService } from './../../../shared/service/notification.service';
import { PurgeMonitorRequest } from '../models/get-purge-monitor-request.model';
import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject, ReplaySubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { PurgeDataAccess } from '../loanpurge.data';
import { RetentionPurge } from '../models/retention-purge-model';
import { Location } from '@angular/common';

const jwtHelper = new JwtHelperService();

@Injectable()
export class LoanPurgeService {
  purgeMonitorDTabledata = new Subject<any>();
  isRowSelectEnabled = new Subject<boolean>();
  isRowSelectrDeselect = new Subject<boolean>();
  searchData = new Subject<any>();
  getExpiredloandata = new Subject<any>();
  ReviewStatusMaster = new Subject<any>();
  LoanPurgeSearch = new Subject<any>();
  purgeMonitorBatchDetailsDTables = new ReplaySubject<any>();
  isPurgeMonitorTab = new BehaviorSubject<any>(false);
  constructor(private purgedata: PurgeDataAccess, private location: Location, private _notificationservice: NotificationService) { }
  private isPurgeMonitor = false;
  getPurgedLoans(pmInputReq: PurgeMonitorRequest) {
    return this.purgedata.GetPurgeDetails(pmInputReq).subscribe((res) => {
      if (res !== null) {
        const pMSearchData = jwtHelper.decodeToken(res.Data)['data'];
        this.purgeMonitorDTabledata.next(pMSearchData);
        if (pMSearchData.length > 0) {
          this.isRowSelectrDeselect.next(true);
          this.isRowSelectEnabled.next(false);
        } else {
          this._notificationservice.showError('No data available');
          this.isRowSelectEnabled.next(true);
        }
      }
    });
  }

  GetExpiredLoans(pmInputReq: {
    TableSchema: string;
    FromDate: any;
    ToDate: any;
  }) {
    return this.purgedata.GetExpiredLoanList(pmInputReq).subscribe((res) => {
      if (res !== null) {
        const searchData = jwtHelper.decodeToken(res.Data)['data'];
        this.searchData.next(searchData);
        if (searchData.length > 0) {
          this.isRowSelectrDeselect.next(true);
          this.isRowSelectEnabled.next(false);
        } else {
          this.isRowSelectEnabled.next(true);
          this._notificationservice.showError('No data available');
        }
        this.getExpiredloandata.next(searchData);
      }
    });
  }
  LoanPurgeSearchFromDashboard(pmInputReq: {TableSchema: string; AuditMonthYear: any }) {
    return this.purgedata.LoanPurgeSearchFromDashboard(pmInputReq).subscribe((res) => {
      if (res !== null) {
        const searchData = jwtHelper.decodeToken(res.Data)['data'];
        if (searchData.length > 0) {
          this.isRowSelectrDeselect.next(true);
          this.isRowSelectEnabled.next(false);
        } else {
          this.isRowSelectEnabled.next(true);
          this._notificationservice.showError('No data available');
        }
        this.getExpiredloandata.next(searchData);
      }
    });
  }
  GetPurgeStatus() {
    return this.purgedata.GetPurgeStatus().subscribe((res: any) => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this.ReviewStatusMaster.next(data);
      }
    });
  }

  RetryPurge(pmInputReq: { TableSchema: string; BatchIDs: any }) {
    return this.purgedata.RetryPurge(pmInputReq).subscribe((res) => {
      if (res !== null) {
        const retryData = jwtHelper.decodeToken(res.Data)['data'];
        if (retryData !== null || retryData !== undefined || retryData !== '') {
          this.LoanPurgeSearch.next('retrypurge');
        }
      }
    });
  }
  RetentionPurge(inputReq: RetentionPurge) {
    return this.purgedata.RetentionPurge(inputReq).subscribe((res) => {
      if (res !== null) {
        const resData = jwtHelper.decodeToken(res.Data)['data'];
        if (resData === true) {
          this.LoanPurgeSearch.next('retentionpurge');
        }
      }
    });
  }
  GetPurgeBatchDetails(inputReq: { TableSchema: string; BatchID: any }) {
    return this.purgedata.GetPurgeBatchDetails(inputReq).subscribe((res) => {
      if (res !== null) {
        const resData = jwtHelper.decodeToken(res.Data)['data'];
        if (resData !== null) {
          this.purgeMonitorBatchDetailsDTables.next(resData);
        }
      }
    });
  }
  BacktoPurgeMonitor() {
    this.isPurgeMonitorTab.next(true);
    this.location.back();
  }

}
