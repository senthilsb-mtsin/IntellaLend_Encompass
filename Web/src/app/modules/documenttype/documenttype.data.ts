import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { MTSAPIResponse } from '@mts-api-response-model';
import { APIService } from '../../shared/service/api.service';
import { AddDocumentFieldRequestModel } from './models/adddocument-field-request.model';
import { AddDocumentTypeRequestModel } from './models/add-document-type-request.model';
import { DocumentTypeApiUrlConstant, DocumentFieldApiUrlConstant } from '@mts-api-url';

@Injectable()
export class DocumentDataAccess {
  constructor(private _api: APIService) {

  }
  AddDocumentField(_reqBody: AddDocumentFieldRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentFieldApiUrlConstant.ADD_DOCUMENT_FIELD, _reqBody);
  }
  AddDocumentType(_reqBody: AddDocumentTypeRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentTypeApiUrlConstant.ADD_DOCUMENT_TYPE, _reqBody);
  }
  EditDocumentType(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentTypeApiUrlConstant.EDIT_DOCUMENT_TYPE, _reqBody);
  }
  DeleteDocumentField(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentFieldApiUrlConstant.DELETE_DOCUMENT_FIELD, _reqBody);
  }
  UpdateDocumentField(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentFieldApiUrlConstant.UPDATE_DOCUMENT_FIELD, _reqBody);
  }
  GetDocumentTypesBasedonLoanType(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentFieldApiUrlConstant.GET_DOCUMENTTYPEBASED_LOANTYPE, _reqBody);
  }
  CheckDocumentExist(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentTypeApiUrlConstant.CHECK_DOCUMENT_EXIST, _reqBody);
  }
  CheckDocumentExistForEdit(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentTypeApiUrlConstant.CHECK_DOCUMENT_DUPFOREDIT, _reqBody);
  }
  SynDocTypes(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentTypeApiUrlConstant.SYNC_DOC_TYPES, _reqBody);
  }
  GetDocumentFields(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(DocumentFieldApiUrlConstant.GET_DOCUMENT_FIELDS, _reqBody);
  }

}
