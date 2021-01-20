import { APIService } from 'src/app/shared/service/api.service';
import { Injectable } from '@angular/core';
import { ServiceTypeApiUrlConstant } from '@mts-api-url';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { AddServiceTypeRequestModel } from './models/add-service-type-request.model';
import { AssignLoanTypesRequestModel } from './models/assign-loan-types-request.model';
import { CustomerImportStagingDetailsRequestModel, CustomerImportStagingRequestModel } from '../customer/models/customer-import.model';

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

    GetAssignedLenders(req: {TableSchema: string, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_ASSIGNED_LENDERS, req);
    }

    SaveLoanMapping(req: AssignLoanTypesRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.SET_SERVICETYPE_LOANTYPES_MAPPING, req);
    }
    RemoveLoanMapping(req: AssignLoanTypesRequestModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.REMOVE_SERVICETYPE_LOAN_MAPPING, req);
    }
    SaveReviewLoanLenderMapping(req: any): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.SAVE_REVIEW_LOAN_LENDER_MAPPING, req);
    }
    GetServicePriorityList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_REVIEWPRIORITY_MASTER, req);
    }

    GetServiceRoleList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_ALL_ROLE_MASTER, req);
    }
    CheckCustReviewLoanMapping(req: { TableSchema: string, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.CHECK_CUST_REVIEW_LOAN_MAPPING, req);
    }
    SaveCustReviewLoanMapping(req: { TableSchema: string, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.SAVE_CUST_REVIEW_LOAN_MAPPING, req);
    }
    GetServiceCustomerImportStaging(req: CustomerImportStagingRequestModel) {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_SERVICE_CUSTOMER_IMPORT_STAGING, req);
    }

    GetCustomeImportStagingDetails(req: CustomerImportStagingDetailsRequestModel) {
        return this._api.authHttpPost(ServiceTypeApiUrlConstant.GET_SERVICE_CUSTOMER_IMPORT_STAGING_DETAILS, req);
    }

}
