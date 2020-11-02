import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { Observable } from 'rxjs';
import { MTSAPIResponse } from '@mts-api-response-model';
import { CustomerApiUrlConstant } from 'src/app/shared/constant/api-url-constants/customer-api-url.constant';
import { CustomerDatatableModel } from './models/customer-datatable.model';
import { SaveCustReviewLoanMappingModel } from './models/save-cust-reivew-loan-mapping.model';
import { StackingOrderDetailTable } from './models/stacking-order-detail-table.model';

@Injectable()
export class CustomerData {
    constructor(private _api: APIService) { }

    GetCustomerMaster(req: { TableSchema: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_CUSTOMER_MASTER, req);
    }

    AddCustomeSubmit(req: { TableSchema: string, customerMaster: CustomerDatatableModel }): Observable<MTSAPIResponse> {
        if (req.customerMaster.Type === 'Add') {
            return this._api.authHttpPost(CustomerApiUrlConstant.ADD_CUSTOMER, req);
        } else {
            return this._api.authHttpPost(CustomerApiUrlConstant.EDIT_CUSTOMER, req);
        }
    }

    GetMappedReviewTypes(req: { TableSchema: string, CustomerID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_MAPPING_SERVICE_TYPE, req);
    }

    GetMappedLoanTypes(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_MAPPING_LOAN_TYPE, req);
    }

    CheckCustReviewMapping(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.CHECK_CUST_REVIEW_MAPPING, req);
    }

    CheckCustReviewLoanMapping(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.CHECK_CUST_REVIEW_LOAN_MAPPING, req);
    }

    RemoveCustReviewLoanMapping(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.REMOVE_CUST_REVIEW_LOAN_MAPPING, req);
    }

    RemoveCustLoanUpload(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.REMOVE_CUST_LOAN_UPLOAD_PATH, req);
    }

    SaveCustReviewMapping(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SAVE_CUST_REVIEW_MAPPING, req);
    }

    SaveCustReviewLoanMapping(req: SaveCustReviewLoanMappingModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SAVE_CUST_REVIEW_LOAN_MAPPING, req);
    }

    SaveCustLoanUploadPath(req: SaveCustReviewLoanMappingModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SAVE_CUST_LOAN_UPLOAD_PATH, req);
    }

    CheckCustLoanUploadPath(req: SaveCustReviewLoanMappingModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.CHECK_CUST_LOAN_UPLOAD_PATH, req);
    }

    RetainLoanTypeMapping(req: SaveCustReviewLoanMappingModel): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.RETAIN_CUST_REVIEW_LOAN_MAPPING, req);
    }

    RemoveCustReviewMapping(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SAVE_CUST_REVIEW_MAPPING, req);
    }

    RetainCustReviewMapping(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.RETAIN_CUST_REVIEW_MAPPING, req);
    }

    GetCheckAndStack(req: { TableSchema: string, CustomerID: number, ReviewTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_CUST_REVIEW_LOAN_CHECKLIST, req);
    }

    GetCheckListDetail(req: { TableSchema: string, CheckListDetailID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_CHECKLIST_DETAIL, req);
    }

    DeleteChecklistItems(req: { TableSchema: string, CheckListDetailsID: number[] }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.DELETE_CHECKLIST_ITEM, req);
    }

    CloneChecklistItem(req: { TableSchema: string, CheckListDetailsID: number[], ModifiedCheckListName: string }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.CLONE_CHECKLIST_ITEM, req);
    }

    GetStackingOrderDetails(req: { TableSchema: string, StackingOrderID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_STACKING_ORDER_DETAIL, req);
    }

    SetTenantOrderByField(req: { TableSchema: string, DocumentTypeID: number, FieldID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SET_TENANT_ORDER_BY_FIELD, req);
    }

    SetTenantDocFieldValue(req: { TableSchema: string, DocumentTypeID: number, FieldID: number }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SET_TENANT_DOC_FIELD_VALUE, req);
    }

    SetDocGroupFieldValue(req: { TableSchema: string, StackingOrderDetails: StackingOrderDetailTable[], StackOrder: { ID: number, Name: string, StackingOrderFieldName: string } }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SET_DOC_GROUP_FIELD_VALUE, req);
    }

    SaveCustomerStackingOrder(req: { TableSchema: string, StackOrderID: number, StackOrder: { isGroup: boolean, ID: number, Name: string, StackingOrderFieldName: string }[] }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.SAVE_CUSTOMER_STACKING_ORDER_DETAILS, req);
    }

    GetAllCustomerConfigData(req: { TableSchema: string, Active: boolean }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.GET_ALL_CUSTOMER_CONFIG, req);
    }

    AddCustomerConfigData(req: { TableSchema: string, CustomerID: number, custConfigItems: any }): Observable<MTSAPIResponse> {
        return this._api.authHttpPost(CustomerApiUrlConstant.ADD_CUSTOMER_CONFIG, req);
    }

}
