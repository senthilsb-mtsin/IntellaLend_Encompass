import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { APIService } from '../../shared/service/api.service';
import { ResetExpiredPassword } from './models/reset-expired-password.model';
import { GetSchemaRequest } from './models/get-schema-request..model';
import { UserApiUrlConstant } from '@mts-api-url';
import { UserDatatableModel } from './models/user-datatable.model';

@Injectable()
export class UserDataAccess {
  constructor(private _api: APIService) { }

  getUserData(_reqBody: GetSchemaRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(UserApiUrlConstant.GET_USER_LIST, _reqBody);
  }
  getCustomerData(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(UserApiUrlConstant.GET_CUSTOMER_LIST, _reqBody);
  }
  AddUserData(_reqBody: UserDatatableModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(UserApiUrlConstant.ADD_USER, _reqBody);
  }
  EditUserData(_reqBody: UserDatatableModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(UserApiUrlConstant.UPDATE_USER, _reqBody);
  }
  getReviewtypeData(_reqBody: GetSchemaRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(UserApiUrlConstant.GET_REVIEWTYPE_MASTER, _reqBody);
  }
  getResetPassword(_reqBody: ResetExpiredPassword): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(UserApiUrlConstant.RESET_EXPIRED_PASSWORD, _reqBody);
  }
}
