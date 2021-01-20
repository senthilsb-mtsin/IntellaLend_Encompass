import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { GetLoanSearchFilterRequest } from './models/get-loan-search-filters.request';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { LoanSearchApiUrlConstant } from 'src/app/shared/constant/api-url-constants/loan-search-api-rul.constant';
import { LoanSearchRequestModel } from './models/loan-search-request.model';
import { DeleteLoanRequestModel } from './models/loan-delete-request.model';
import { FannieMaeApiUrlConstant } from '@mts-api-url';

@Injectable()
export class LoanSearchDataAccess {
  constructor(private _apiService: APIService) { }

  GetLoanSearchFilterConfigValue(
    req: GetLoanSearchFilterRequest
  ): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanSearchApiUrlConstant.GET_LOAN_SEARCH_FILTER_CONFIG,
      req
    );
  }

  GetLoanTypeMaster(req: { TableSchema: string }): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(LoanSearchApiUrlConstant.GET_LOAN_TYPE_MASTER, req);
  }

  GetCustomerMaster(req: { TableSchema: string }): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(LoanSearchApiUrlConstant.GET_CUSTOMER_MASTER, req);
  }

  GetReviewTypeMaster(req: { TableSchema: string }): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(LoanSearchApiUrlConstant.GET_REVIEW_TYPE_MASTER, req);
  }

  GetWorkFlowMaster(): Observable<MTSAPIResponse> {
    return this._apiService.authHttpGet(LoanSearchApiUrlConstant.GET_WORKFLOW_MASTER);
  }

  searchSubmit(req: LoanSearchRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanSearchApiUrlConstant.GET_LOANS,
      req
    );
  }

  deleteLoan(req: DeleteLoanRequestModel): Observable<MTSAPIResponse> {
    return this._apiService.authHttpPost(
      LoanSearchApiUrlConstant.DELETE_LOANS,
      req
    );
  }
  GetFannieMaeCustomerConfig(req: {TableSchema: string}) {
    return this._apiService.authHttpPost(FannieMaeApiUrlConstant.GET_FANNIEMAE_CUSTOMER_CONFIG, req);
  }
}
