import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { LoanPurgeApiUrlConstant } from '@mts-api-url';
import { Observable } from 'rxjs';
import { PurgeMonitorRequest } from './models/get-purge-monitor-request.model';
import { RetentionPurge } from './models/retention-purge-model';
@Injectable()
export class PurgeDataAccess {
  constructor(private _api: APIService) { }

  GetPurgeDetails(req: PurgeMonitorRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      LoanPurgeApiUrlConstant.GET_ALL_PURGED_LOANS,
      req
    );
  }
  GetExpiredLoanList(req: {
    TableSchema: string;
    FromDate: any;
    ToDate: any;
  }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      LoanPurgeApiUrlConstant.GET_ALL_EXPIRED_LOANS,
      req
    );
  }
  LoanPurgeSearchFromDashboard(req: { TableSchema: string; AuditMonthYear: any }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      LoanPurgeApiUrlConstant.GET_DASH_EXPIRED_LOANS,
      req
    );
  }
  RetryPurge(req: {
    TableSchema: string;
    BatchIDs: any;
  }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanPurgeApiUrlConstant.RETRY_PURGE, req);
  }
  GetPurgeStatus(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(LoanPurgeApiUrlConstant.GET_PURGE_STATUS);
  }
  RetentionPurge(inputReq: RetentionPurge): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      LoanPurgeApiUrlConstant.PURGE_STAGING,
      inputReq
    );
  }
  GetPurgeBatchDetails(inputReq: {
    TableSchema: string;
    BatchID: any;
  }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      LoanPurgeApiUrlConstant.GET_PURGE_BATCH_DETAILS,
      inputReq
    );
  }
}
