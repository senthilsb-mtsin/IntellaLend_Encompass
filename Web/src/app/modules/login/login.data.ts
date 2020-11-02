import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

import { MTSAPIResponse } from '@mts-api-response-model';
import { LoginApiUrlConstant } from '@mts-api-url';

import { LoginRequest } from './models/login-request.model';
import { APIService } from '../../shared/service/api.service';
import { GetMenuListRequest } from './models/get-menu-list.model';

@Injectable()
export class LoginDataAccess {
  constructor(private _api: APIService) { }

  userSubmit(_reqBody: LoginRequest): Observable<MTSAPIResponse> {
    return this._api.Post(LoginApiUrlConstant.LOGIN_SUBMIT, _reqBody);
  }

  getDefaultRouteData(_reqBody: GetMenuListRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoginApiUrlConstant.GET_MENU, _reqBody);
  }

  submitUserNameForm(_reqBody: { TableSchema: string, UserName: string }): Observable<MTSAPIResponse> {
    return this._api.Post(LoginApiUrlConstant.USER_NAME_CHECK, _reqBody);
  }

  submitDirect(_reqBody: { TableSchema: string, UserName: string }): Observable<MTSAPIResponse> {
    return this._api.Post(LoginApiUrlConstant.FORGET_PASSWORD, _reqBody);
  }

  CheckCurrentPassword(_reqBody: { TableSchema: string, UserID: number, CurrentPassword: string }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoginApiUrlConstant.CHECK_CURRENT_PASSWORD, _reqBody);
  }

  UpdateNewPasswordForExpiry(_reqBody: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string,  }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoginApiUrlConstant.UPDATE_NEW_PASSWORD_FOR_EXPIRY, _reqBody);
  }

  submitNewUserForm(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoginApiUrlConstant.SET_SECURITY_QUESTION, _reqBody);
  }
}
