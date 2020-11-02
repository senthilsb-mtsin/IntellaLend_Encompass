
import { MTSAPIResponse } from '@mts-api-response-model';
import { LoginApiUrlConstant, BlackListApiUrlConstant } from '@mts-api-url';

import { APIService } from '../service/api.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommonApiUrlConstant } from '../constant/api-url-constants/common-api-url.constant';

@Injectable({ providedIn: 'root' })
export class CommonDataAccess {
  constructor(private _api: APIService) { }

  unLockUser(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.Post(LoginApiUrlConstant.UNLOCK_USER, _reqBody);
  }

  postError(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.Post(BlackListApiUrlConstant.ERROR_HANDLER, _reqBody);
  }
  GetReviewTypeList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_ALL_REVIEWTYPE_MASTER, req);
}

  GetAllSysCheckListMastersDatas(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(CommonApiUrlConstant.GET_SYSTEM_CHECKLIST);
  }

  // GetAllSysStackingOrderMastersDatas(): Observable<MTSAPIResponse> {
  //   return this._api.authHttpGet(CommonApiUrlConstant.GET_SYSTEM_STACKINGORDER);
  // }

  // GetSystemDocumentTypes(): Observable<MTSAPIResponse> {
  //   return this._api.authHttpGet(CommonApiUrlConstant.GET_SYSTEM_DOCUMENTTYPES_STACK);
  // }

  GetAllSysCheckListCategories(req: { Tableschema: string }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_SYSTEM_CHECKLIST_CATEGORIES, req);
  }

  GetSysLOSFields(req: { Tableschema: string, SearchCriteria: string }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_SYSTEM_LOS_FIELDS, req);
  }
  GetAllSysLoantypeDatas(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(CommonApiUrlConstant.GET_ALL_LOANTYPES);
  }
  GetCustomerData(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_CUSTOMER_LIST, _reqBody);
  }
  GetSystemDocumentTypes(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(CommonApiUrlConstant.GET_SYSTEM_DOCUMENT_TYPES);
  }
  GetSystemDocumentFieldList(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(CommonApiUrlConstant.GET_SYSTEM_DOCUMENT_TYPE_FIELD_MASTER);
  }
  GetAllSysStackingOrderMastersDatas(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(CommonApiUrlConstant.GET_SYSTEM_STACKINGORDER);

  }
  GetRoleData(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_ROLE_MASTER, _reqBody);
  }
  GetActiveCustomerList(_reqBody: {Tableschema: string}): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_ACTIVE_CUSTOMERLIST, _reqBody);
  }
  GetAllLoantypeMaster(_reqBody: {Tableschema: string}): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_AllLOANTYPE_MASTERS, _reqBody);
  }
  GetParkingSpotData(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(CommonApiUrlConstant.GET_PARKINGSPOT_DETAILS, _reqBody);
  }

}
