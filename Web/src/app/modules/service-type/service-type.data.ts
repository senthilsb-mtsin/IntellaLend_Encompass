import { APIService } from 'src/app/shared/service/api.service';
import { Injectable } from '@angular/core';
import { ServiceTypeApiUrlConstant } from '@mts-api-url';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { AddServiceTypeRequestModel } from './models/add-service-type-request.model';
import { AssignLoanTypesRequestModel } from './models/assign-loan-types-request.model';

@Injectable()
export class ServiceTypeDataAccess {
    constructor(private _api: APIService) { }

    GetServiceTypeList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_ALL_REVIEWTYPE_MASTER, req);
    }

    AddServiceTypeSubmit(req: AddServiceTypeRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.ADD_SERVICETYPE_SUBMIT, req);
    }

    UpdateServiceTypeSubmit(req: AddServiceTypeRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.EDIT_SERVICETYPE_SUBMIT, req);
    }

    GetSysLoanTypes(req: { ReviewTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_SYSTEM_LOAN_TYPES, req);
    }

    SaveLoanMapping(req: AssignLoanTypesRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.SET_SERVICETYPE_LOANTYPES_MAPPING, req);
    }

    GetServicePriorityList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_REVIEWPRIORITY_MASTER, req);
    }

    GetServiceRoleList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_ALL_ROLE_MASTER, req);
    }
}
