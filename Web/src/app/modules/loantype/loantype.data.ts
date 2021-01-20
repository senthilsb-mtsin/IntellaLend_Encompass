import { Injectable } from '@angular/core';
import { APIService } from 'src/app/shared/service/api.service';
import { MTSAPIResponse } from '@mts-api-response-model';
import { CommonApiUrlConstant, LoanTypeApiUrlConstant } from '@mts-api-url';
import { Observable } from 'rxjs';
import { SyncCustomerRequest } from './models/sync-customer-request.model';
import { AddLoantypeRequestModel } from './models/add-loantype-request.model';
import { AssignDocumentTypeRequestModel } from './models/assign-document-types-request.model';
import { CloneChecklistRequest } from './models/clone-checklist-request.model';
import { AddChecklistGroupModel } from './models/add-checklist-group.model';
import { AssignStackingOrderRequestModel } from './models/assign-stacking-order-request.model';
import { SaveChecklistItem, SaveRuleMasters } from './models/save-checklist-item-request.model';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { AssignDocumentsRuleRowData } from './models/assign-docs-rule.model';

@Injectable()
export class LoanDataAccess {

  constructor(private _api: APIService) {

  }

  GetLoanTypeList(req: { TableSchema: string }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_ALL_LOANTYPE_MASTER, req);
  }

  SyncLoanType(req: SyncCustomerRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SYNC_CUSTOMER_LOANTYPE, req);
  }

  AddLoanTypeSubmit(req: AddLoantypeRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.ADD_LOANTYPE_SUBMIT, req);
  }
  GetSyncDetails(req: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_SYNC_DETAILS, req);
  }
  UpdateLoanTypeSubmit(req: AddLoantypeRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.EDIT_LOANTYPE_SUBMIT, req);
  }

  GetSysDocumentTypes(req: { LoanTypeID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_SYSTEM_DOCUMENT_TYPES, req);
  }

  SetLoanTypeMapping(req: { LoanTypeID: Number, CheckListID: Number, ChecklistItemSeq: any[] }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_LOANTYPE_CHECKLIST_MAPPING, req);
  }

  SaveDocMapping(req: AssignDocumentTypeRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_LOAN_DOC_MAPPING, req);
  }

  GetLoanTypeChecklist(req: { TableSchema: string, LoanTypeID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_LOANTYPE_CHECKLIST, req);
  }

  GetStackingOrderData(req: { TableSchema: string, LoanTypeID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_LOANTYPE_STACKINGORDER, req);
  }

  GetSystemChecklistDetail(req): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_CHECKLISTDETAIL_MASTER, req);
  }

  SaveChecklistItemObject(req: { TableSchema: string, CheckListDetailMaster: SaveChecklistItem, RuleMasters: SaveRuleMasters, LoanTypeID: number }): Observable<MTSAPIResponse> {
    if (req.CheckListDetailMaster.CheckListDetailID > 0 && req.RuleMasters.RuleID > 0) {
      if (isTruthy(req.TableSchema)) {
        return this._api.authHttpPost(LoanTypeApiUrlConstant.UPDATE_CUSTOMER_CHECKLIST_ITEM, req);
      } else {
        return this._api.authHttpPost(LoanTypeApiUrlConstant.UPDATE_CHECKLIST_ITEM, req);
      }
    } else {
      if (isTruthy(req.TableSchema)) {
        return this._api.authHttpPost(LoanTypeApiUrlConstant.ADD_CUSTOMER_CHECKLIST_ITEM, req);
      } else {
        return this._api.authHttpPost(LoanTypeApiUrlConstant.ADD_CHECKLIST_ITEM, req);
      }
    }
  }

  EvaluateRules(req: { RuleFormula: string, CheckListItemValues: any }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.EVALUATE_CHECKLIST_FORMULA, req);
  }

  AssignChecklist(req: CloneChecklistRequest): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.CLONE_CHECKLIST, req);
  }

  SetDocGrpFieldValue(req: { TableSchema: string, StackingOrderDetails: any[], StackOrder: { ID: number, Name: string, StackingOrderFieldName: string } }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_TENANT_DOC_FIELD_VALUE, req);
  }

  SaveSysCheckList(req: AddChecklistGroupModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.ADD_CHECKLIST_GROUP, req);
  }

  GetSystemStackingOrderDetails(req: { StackingOrderID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_STACKING_ORDER_DETAILS, req);
  }

  AssignStackingOrder(req: AssignStackingOrderRequestModel): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.ASSIGN_STACKING_ORDER, req);
  }

  GetStackDocumentTypes(): Observable<MTSAPIResponse> {
    return this._api.authHttpGet(LoanTypeApiUrlConstant.GET_STACKING_DOCUMENT_TYPES);
  }
  GetStackCreateDocs(req: { LoanTypeID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_STACKING_CREATE_DOCS, req);

  }
  SetDocFieldValue(req: { DocumentTypeID: number, FieldID: number }, type: string): Observable<MTSAPIResponse> {
    if (type === 'OrderByField') {
      return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_ORDERBY_DOC_FIELD, req);
    } else {
      return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_DOC_FIELD, req);
    }
  }

  SaveSystemStackingOrder(req): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_STACKING_ORDER, req);
  }

  SaveStackingOrderMapping(req: { LoanTypeID: number, StackingOrderID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SET_STACKING_ORDER_MAPPING, req);
  }

  CloneChecklistItem(req: { CheckListDetailsID: any[], ModifiedCheckListName: string, LoanTypeID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.CLONE_CHECKLIST_ITEM, req);
  }

  DeleteChecklistItems(req: { CheckListDetailsID: any[], LoanTypeID: number }): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.DELETE_CHECKLIST_ITEM, req);
  }
  SaveConditionGeneralRule(_reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.SAVE_CONDITION_GENERALRULE, _reqBody);
  }
  GetCustLoanDocMapping(reqBody: any): Observable<MTSAPIResponse> {
    return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_CUST_LOAN_DOC_CONDITION_MAPPING, reqBody);
  }
  GetSysLoanTypeDocuments(req: { TableSchema: string, customerID: number, loanTypeID: number, LoanTypeID: number }): Observable<MTSAPIResponse> {
    if (isTruthy(req.TableSchema)) {
      return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_CUST_LOAN_DOC_MAPPING, req);
    } else {
      return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_SYSTEM_LOANTYPE_DOCUMENTS, req);
    }
  }
  GetSysAllLoanTypeDocuments(req: { LoanTypeID: number }): Observable<MTSAPIResponse> {
      return this._api.authHttpPost(LoanTypeApiUrlConstant.GET_SYSTEM_LOANTYPE_DOCUMENTS, req);

  }
  GetLosDocFields(req: { TableSchema: string; LOSDocumentId: number; FieldSearchWord: string; }) {
      return this._api.authHttpPost(CommonApiUrlConstant.GET_LOSDOCUMENT_FIELDS, req);
  }
}
