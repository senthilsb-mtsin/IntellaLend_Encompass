import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { APIService } from '../../shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { LoginApiUrlConstant } from '@mts-api-url';

@Injectable()
export class LayoutDataAccess {
    constructor(private _api: APIService) { }

    CheckCurrentPassword(_reqBody: { TableSchema: string, UserID: number, CurrentPassword: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(LoginApiUrlConstant.CHECK_CURRENT_PASSWORD, _reqBody);
    }

    getDefaultRouteData(_reqBody: { TableSchema: string, RoleID: number, UserID: number, ADLogin: boolean }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(LoginApiUrlConstant.GET_MENU, _reqBody);
    }

    updateNewPassword(_reqBody: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(LoginApiUrlConstant.UPDATE_NEW_PASSWORD, _reqBody);
    }

    UpdateNewPasswordForExpiry(_reqBody: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string, }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(LoginApiUrlConstant.UPDATE_NEW_PASSWORD_FOR_EXPIRY, _reqBody);
    }
}
