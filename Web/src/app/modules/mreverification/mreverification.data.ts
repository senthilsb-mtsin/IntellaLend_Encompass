import { APIService } from 'src/app/shared/service/api.service';
import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { Observable } from 'rxjs';
import { ManagerReverificationApiUrlConstant } from '@mts-api-url';
import { RequestTemplateModel } from './models/request-template.model';

@Injectable()
export class ManagerReverificationDataAccess {
  constructor(private _api: APIService) { }

  GetCustomerReverificationData(_req: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.GET_CUSTOMER_REVERIFICATION, _req);
  }
  GetManagerReverificationTemplate(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(ManagerReverificationApiUrlConstant.GET_MANAGER_TEMPLATE);
  }
  UpdateReverification(_req: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.UPDATE_REVERIFICATION, _req);
  }
  GetLoanDocuments(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.GET_LOAN_DOCUMENTS, _reqBody);
  }
  GetManagerMappedTemplate(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.GET_MAPPED_TEMPLATE, _reqBody);
  }
  CheckReverificationAvailableForEdit (_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.CHECK_MANAGER_REVERIFI, _reqBody);
  }
  SaveManagerReverifyDocMapping(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.SAVE_REVERIFICATION_DOC_MAPPING, _reqBody);
  }
  UpdateReverificationTemplateFields(_reqBody: RequestTemplateModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(ManagerReverificationApiUrlConstant.UPDATE_MAPPING_TEMPLATE_FIELDS, _reqBody);
  }

}
