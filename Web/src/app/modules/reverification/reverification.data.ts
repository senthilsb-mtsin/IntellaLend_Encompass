import { APIService } from 'src/app/shared/service/api.service';
import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { Observable } from 'rxjs';
import { ReverificationApiUrlConstant } from '@mts-api-url';

@Injectable()
export class ReverificationDataAccess {
  constructor(private _api: APIService) { }

  GetReverificationData(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(ReverificationApiUrlConstant.GET_REVERIFICATION_LIST);
  }
  GetReverificationTemplate(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(ReverificationApiUrlConstant.GET_REVERIFICATION_TEMPLATE);
  }
  GetLoanDocuments(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.GET_LOAN_DOCUMENTS, _reqBody);
  }
  AddReverificationSubmit(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.ADD_REVERIFICATION, _reqBody);
  }
  EditReverificationSubmit(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.UPDATE_REERIFICATION, _reqBody);
  }
  GetMappedTemplate(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.GET_MAPPED_TEMPLATE, _reqBody);
  }
  SaveReverifyDocMapping(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.SAVE_REVERIFICATION_DOC_MAPPING, _reqBody);
  }
  UpdateReverificationTemplate(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.UPDATE_MAPPING_TEMPLATE, _reqBody);
  }
  CheckReverificationExist(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.CHECK_REVERIFICATION_EXIST, _reqBody);
  }
  CheckReverificationExistForEdit(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ReverificationApiUrlConstant.CHECK_REVERIFICATION_FOREDIT, _reqBody);
  }

}
