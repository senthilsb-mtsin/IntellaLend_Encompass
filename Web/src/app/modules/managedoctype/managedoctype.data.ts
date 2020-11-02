import { APIService } from 'src/app/shared/service/api.service';
import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { Observable } from 'rxjs';
import { ManagerDocTypeApiUrlConstant } from '@mts-api-url';

@Injectable()
export class ManagerDocTypeDataAccess {
  constructor(private _api: APIService) {

  }
  GetManagerDocTypes(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerDocTypeApiUrlConstant.GET_DOCTYPE_CUST_LOAN, _reqBody);
  }
  GetLoanTypeForCustomer(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerDocTypeApiUrlConstant.GET_LOANTYPE_FORCUSTOMER, _reqBody);
  }
  UpdateDocumentField(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerDocTypeApiUrlConstant.UPDATE_DOCUMENT_FIELD, _reqBody);
  }
  UpdateDocumentType(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerDocTypeApiUrlConstant.UPDATE_MANAGER_DOCTYPE, _reqBody);
  }
  CheckDocumentExistForEdit(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerDocTypeApiUrlConstant.CHECK_DOCUMENT_DUPFOREDIT, _reqBody);
  }
}
