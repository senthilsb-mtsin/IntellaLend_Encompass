import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { RoleTypeApiUrlConstant } from 'src/app/shared/constant/api-url-constants/roletype-api-url.constant';
import { Observable } from 'rxjs';
import { AddRoleTypeRequestModel } from './models/roletype-request.model';
import { RoleTypeRequest } from './models/table-request.model';
import { RoleDetailsRequest } from './models/roletypedetails.model';
import { ChangeRoleRequest } from './models/changerole.model';

@Injectable()
export class RoleTypeDataAccess {

  constructor(private _api: APIService) {

  }

  GetRoleAdminList(InputReq: RoleTypeRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(RoleTypeApiUrlConstant.GET_ALLROLEMASTER, InputReq);
  }
  GetADGroupMasterList(InputReq: RoleTypeRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(RoleTypeApiUrlConstant.GET_ALLADGROUPMASTER, InputReq);
  }
  GetMenuList(InputReq: RoleTypeRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(RoleTypeApiUrlConstant.GET_ALLMENU, InputReq);
  }
  GetAddRoleSubmit(req: AddRoleTypeRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      RoleTypeApiUrlConstant.GET_ADDROLE, req
    );

  }
  GetEditRoleDetails(updaterole: RoleDetailsRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      RoleTypeApiUrlConstant.GET_EDITROLEDETAILS, updaterole
    );
  }

  CheckUserRoleDetails(rolechange: ChangeRoleRequest) {
    return this._api.authHttpPost(
      RoleTypeApiUrlConstant.CheckUserRoleDetails, rolechange
    );
  }
  UpdateRoleDetails(inputData: AddRoleTypeRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(
      RoleTypeApiUrlConstant.GET_UPADETEROLE, inputData
    );
  }

  GetChangeMenuActive(ChangeMenu: ChangeRoleRequest) {
    return this._api.authHttpPost(
      RoleTypeApiUrlConstant.GetChangeMenuActive, ChangeMenu
    );
  }
}
