import { Location } from '@angular/common';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { Subject, Subscription } from 'rxjs';
import { CommonService } from 'src/app/shared/common/common.service';
import { LoanDataAccess } from '../loantype.data';
import { AddChecklistGroupModel } from '../models/add-checklist-group.model';
import { AddLoantypeRequestModel } from '../models/add-loantype-request.model';
import { AssignDocumentTypeRequestModel, DocMappingDetail } from '../models/assign-document-types-request.model';
import { AssignStackingOrderRequestModel } from '../models/assign-stacking-order-request.model';
import { ChecklistItemRowData } from '../models/checklist-items-table.model';
import { CloneChecklistRequest } from '../models/clone-checklist-request.model';
import { LoanTypeWizardStepModel } from '../models/loan-type-wizard-steps.model';
import { CheckListItemNamePipe } from '../pipes/validateCheckListItemName.pipe';
import { CommonRuleBuilderService } from 'src/app/shared/common/common-rule-builder.service';
import { ConditionGeneralRuleService } from './condition-general-rule.service';

const jwtHelper = new JwtHelperService();

@Injectable()
export class AddLoanTypeService {
  setNextStep = new Subject<LoanTypeWizardStepModel>();
  assignedDocs = new Subject<DocMappingDetail[]>();
  checkListType = new Subject<string>();
  stackingOrderBack = new Subject<string>();
  checkListBack = new Subject<string>();
  stackingOrderType = new Subject<string>();
  SysCheckListDetailTableData = new Subject<any[]>();
  EnableEditChecklistFeatures = new Subject<boolean>();
  isRowNotSelected = new Subject<boolean>();
  HideCloneMsgModal = new Subject<boolean>();
  EnableRuleBuilder = new Subject<boolean>();
  StackUnAssignedDocs = new Subject<any[]>();
  SysStackingOrderDetailData = new Subject<{ StackingOrder: any[], StackingOrderGroup: any[], UnAssignedDocTypes: any[] }>();
  AddLoantypeSteps: any = { Loantype: 1, AssignDocumentType: 2, Checklist: 3, StackingOrder: 4 };
  Loading = new Subject<boolean>();
  SetRuleStep = new Subject<{ Step1: boolean, Step2: boolean, Step3: boolean }>();

  ruleBuilderNext = new Subject<boolean>();
  ruleExpression = new Subject<string>();
  RuleAdded = new Subject<boolean>();
  stackAssignedDocs = new Subject<any>();
  GeneralRuleclose = new Subject<boolean>();
  LoanTypeId: any;
  getCurrentDocs = new Subject<boolean>();
  LoanTypeDocuments = new Subject<{
    Documents: { id: number, text: string }[],
    Fields: { DocID: number, FieldID: number, Name: string, DocName: string }[],
    DataTables: { DocID: number, TableName: string, ColumnName: string }[]
  }>();

  constructor(
    private _loanTypeData: LoanDataAccess,
    private _checklistItemValidator: CheckListItemNamePipe,
    private _notificationService: NotificationService,
    private _commonService: CommonService,
    private _conditionRuleservice: ConditionGeneralRuleService,
    private _commonRuleBuilderService: CommonRuleBuilderService, private _location: Location) { }

  private _allDocTypes: any[] = [];
  private _stackUnAssignedDocs: any[] = [];
  private _stackAssignedDocs: any[] = [];

  private _assignedDocTypes: any[] = [];
  private _loanTypeID = 0;
  private _loanTypeName = '';
  private _stackinOrderName = '';
  private _sysCheckListDetailTableData: any[] = [];
  private _sysStackingOrderDetailData: any[] = [];
  private _sysUnAssignedStackingOrder: any[] = [];
  private _sysGroupStackingOrderDetailData: any[] = [];
  private _checkList: { CheckListID: number, CheckListName: string } = { CheckListID: 0, CheckListName: '' };
  private _stackinOrder: { StackingOrderID: number, Description: string } = { StackingOrderID: 0, Description: '' };
  private _docSysStackingOrder: any[] = [];
  private _docGroupStackingOrder: any[] = [];
  private _sysStackingDetails: any[] = [];
  private _stackingOrderType = '';
  private _checkListType = '';
  private _checklistItemRowData: ChecklistItemRowData = new ChecklistItemRowData();
  private _ruleBuilderSteps: { Step1: boolean, Step2: boolean, Step3: boolean };
  private _loanTypeDocumentFields: { DocID: number, FieldID: number, Name: string, DocName: string }[] = [];
  private _loanTypeDocuments: { id: number, text: string }[] = [];
  private _loanTypeDocumentDataTable: { DocID: number, TableName: string, ColumnName: string }[] = [];
  addLoanTypeSubmit(req: AddLoantypeRequestModel) {
    this._loanTypeData.AddLoanTypeSubmit(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if (result.Success) {
            this._loanTypeID = result.LoanTypeID;
            this._commonRuleBuilderService.setCurrentLoanTypeID(this._loanTypeID);
            this._loanTypeName = req.LoanType.LoanTypeName;
            this._notificationService.showSuccess('Loan Type Added Successfully');
            this.getSysDocumentTypes();
          } else {
            this._notificationService.showError('Loan Type Name already exist');
          }
        }
        this.Loading.next(false);
      });
  }

  clearSavedChecklistItem() {
    this._checklistItemRowData = new ChecklistItemRowData();
  }

  updateLoanTypeSubmit(req: AddLoantypeRequestModel) {
    this._loanTypeData.UpdateLoanTypeSubmit(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if (result) {
            this._loanTypeID = req.LoanType.LoanTypeID;
            this._loanTypeName = req.LoanType.LoanTypeName;
            this._commonRuleBuilderService.setCurrentLoanTypeID(this._loanTypeID);
            this._notificationService.showSuccess('Loan Type Updated Successfully');
            this.getSysDocumentTypes();
          } else {
            this._notificationService.showError('Loan Type Name already exist');
          }
        }
        this.Loading.next(false);
      });
  }

  SaveDocMapping(req: AssignDocumentTypeRequestModel) {
    this._loanTypeData.SaveDocMapping(req).subscribe(res => {
      const result = jwtHelper.decodeToken(res.Data)['data'];
      if (result) {
        this.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.Checklist, 'active complete', 'active complete', 'active', ''));
        this._notificationService.showSuccess('Loan Types Mapping Updated Successfully');
      }
      this.Loading.next(false);
    });
  }

  docFieldsInitChange(control: any, currtDocFields: any[], vals: any, field: string, index: number) {
    const tempGeneralEditDocFieldMasters = [];
    control.get(field).setValue('');
    let newVals = '';
    let newValNotEmpty = false;
    if (typeof (vals) !== 'string') {
      if (vals.currentTarget.value !== '' && vals.currentTarget.value !== 'Select') {
        newValNotEmpty = true;
        newVals = vals.target.selectedOptions[0].innerText.replace(/[\s]/g, '');
      }
    } else {
      if (vals !== '' && vals !== 'Select') {
        newValNotEmpty = true;
        newVals = vals.replace(/[\s]/g, '');
      }
    }

    if (newValNotEmpty) {
      for (let i = 0; i < this._loanTypeDocumentFields.slice().length; i++) {
        const trmmiedSpaceDocName = this._loanTypeDocumentFields.slice()[i].DocName.replace(/[\s]/g, '');
        if (newVals === trmmiedSpaceDocName) {
          tempGeneralEditDocFieldMasters.push(this._loanTypeDocumentFields.slice()[i].Name);
        }
      }
      currtDocFields[index] = tempGeneralEditDocFieldMasters;
    } else if (vals === '' || vals === 'Select') {
      control.get(field).setValue('');
      currtDocFields[index] = tempGeneralEditDocFieldMasters;
    }
    if (tempGeneralEditDocFieldMasters.length === 0) {
      this._notificationService.showError('Fields Unavailable');
    }
  }

  SetAssignDocuments(loantypeID: number, DocId: number) {

    for (let i = 0; i < this._assignedDocTypes.length; i++) {
      if (this._assignedDocTypes[i].DocumentTypeID === DocId) {
        this._assignedDocTypes[i].Condition = JSON.stringify(this._conditionRuleservice.ConditionValues.value);
        this.GeneralRuleclose.next(true);
        break;

      }
    }
  }

  Removecondition(loantypeID: number, DocId: number) {

    for (let i = 0; i < this._assignedDocTypes.length; i++) {
      if (this._assignedDocTypes[i].DocumentTypeID === DocId) {
        this._assignedDocTypes[i].Condition = '';
        this.setDocTypes(this._assignedDocTypes, this._allDocTypes);
        this._conditionRuleservice.InitializeForm.next(true);
        this.GeneralRuleclose.next(true);
        break;

      }
    }
  }
  getCurrentLoanTypeID(): number {
    return this._loanTypeID;
  }

  setCurrentLoanType(input: { LoanTypeID: number, LoanTypeName: string }) {
    this._loanTypeID = input.LoanTypeID;
    this._loanTypeName = input.LoanTypeName;
    this._commonRuleBuilderService.setCurrentLoanTypeID(this._loanTypeID);
  }

  getCurrentLoanTypeName(): string {
    return this._loanTypeName;
  }

  setDocSysStackingOrder(docStack: any[], docStackGrp: any[]) {

    this._docSysStackingOrder = docStack;
    this._docGroupStackingOrder = docStackGrp;
  }

  setEditChecklistItem(rowData: ChecklistItemRowData) {
    rowData.LosMatched = isTruthy(rowData.LosIsMatched) && rowData.LosIsMatched === 1;
    if (isTruthy(rowData.RuleJson)) {
      rowData.RuleJsonObject = JSON.parse(rowData.RuleJson);
    }

    this._checklistItemRowData = rowData;
  }

  getEditChecklistItem() {
    return this._checklistItemRowData;
  }

  getLoanTypeChecklist() {
    if (this._loanTypeID > 0) {
      const req = { TableSchema: AppSettings.SystemSchema, LoanTypeID: this._loanTypeID };
      this._loanTypeData.GetLoanTypeChecklist(req).subscribe(res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          this.setSystemChecklist({ CheckListID: result.CheckListID, CheckListName: result.CheckListName });
          this._commonRuleBuilderService.setSystemChecklist({ CheckListID: result.CheckListID, CheckListName: result.CheckListName });
        } else {
          this.checkListBack.next('');
        }
      });
    }
  }
  getSysLoanTypeDocs(_assignDocs: any) {
     this._loanTypeDocuments = [];
    this._loanTypeDocumentFields = [];
    this._loanTypeDocumentDataTable = [];
    _assignDocs.forEach(element => {
      this._loanTypeDocuments.push({ id: element.DocumentTypeID, text: element.Name });
    });
    for (let i = 0; i < _assignDocs.length; i++) {
      for (let j = 0; j < _assignDocs[i].DocumentFieldMasters.length; j++) {
        this._loanTypeDocumentFields.push({ DocID: _assignDocs[i].DocumentFieldMasters[j].DocumentTypeID, FieldID: _assignDocs[i].DocumentFieldMasters[j].FieldID, Name: _assignDocs[i].DocumentFieldMasters[j].Name, DocName: _assignDocs[i].Name });
      }
      for (let j = 0; j < _assignDocs[i].RuleDocumentTables.length; j++) {
        this._loanTypeDocumentDataTable.push({ DocID: _assignDocs[i].RuleDocumentTables[j].DocumentID, TableName: _assignDocs[i].RuleDocumentTables[j].TableName, ColumnName: _assignDocs[i].RuleDocumentTables[j].TableColumnName });
      }
    }
    this.LoanTypeDocuments.next(
      {
        Documents: this._loanTypeDocuments.slice(),
        Fields: this._loanTypeDocumentFields.slice(),
        DataTables: this._loanTypeDocumentDataTable.slice()
      }
    );

  }
  GetSystemLoanDocs() {
    this._loanTypeDocuments = [];
    this._loanTypeDocumentFields = [];
    this._loanTypeDocumentDataTable = [];
    this._assignedDocTypes.forEach(element => {
      this._loanTypeDocuments.push({ id: element.DocumentTypeID, text: element.Name });
    });
    for (let i = 0; i < this._assignedDocTypes.length; i++) {
      for (let j = 0; j < this._assignedDocTypes[i].DocumentFieldMasters.length; j++) {
        this._loanTypeDocumentFields.push({ DocID: this._assignedDocTypes[i].DocumentFieldMasters[j].DocumentTypeID, FieldID: this._assignedDocTypes[i].DocumentFieldMasters[j].FieldID, Name: this._assignedDocTypes[i].DocumentFieldMasters[j].Name, DocName: this._assignedDocTypes[i].Name });
      }
      for (let j = 0; j < this._assignedDocTypes[i].RuleDocumentTables.length; j++) {
        this._loanTypeDocumentDataTable.push({ DocID: this._assignedDocTypes[i].RuleDocumentTables[j].DocumentID, TableName: this._assignedDocTypes[i].RuleDocumentTables[j].TableName, ColumnName: this._assignedDocTypes[i].RuleDocumentTables[j].TableColumnName });
      }
    }
    this.LoanTypeDocuments.next(
      {
        Documents: this._loanTypeDocuments.slice(),
        Fields: this._loanTypeDocumentFields.slice(),
        DataTables: this._loanTypeDocumentDataTable.slice()
      }
    );

  }
  getStackingOrderData() {
    if (this._loanTypeID > 0) {
      const req = { TableSchema: AppSettings.SystemSchema, LoanTypeID: this._loanTypeID };
      this._loanTypeData.GetStackingOrderData(req).subscribe(res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          this.setSystemStackingOrder({ StackingOrderID: result.StackingOrderID, Description: result.Description });
        } else {
          this.stackingOrderBack.next('');
        }
      });
    }
  }

  SaveStackingOrder() {

    let checkFieldName = true;
    this._docSysStackingOrder.forEach(element => {
      if (checkFieldName) {

        const isDupDocument = this._sysStackingDetails.filter(x => x.DocumentTypeID === element.DocumentTypeID);
        if (isDupDocument.length === 0 && element.isGroup === undefined || !element.isGroup) {
          this._sysStackingDetails.push({ isGroup: false, ID: element.DocumentTypeID, Name: 'None', StackingOrderFieldName: '' });
        } else if (isDupDocument.length === 0) {
          const docs = this._docGroupStackingOrder.filter(ele => ele.StackingOrderGroupName === element.StackingOrderGroupName);
          docs.forEach(el => {
            if (el.StackingOrderFieldName !== '' && el.StackingOrderFieldName !== 'Select') {
              checkFieldName = true;
              this._sysStackingDetails.push({ isGroup: true, ID: el.DocumentTypeID, Name: el.StackingOrderGroupName, StackingOrderFieldName: el.StackingOrderFieldName });
            } else {
              this._sysStackingDetails = [];
              checkFieldName = false;
            }
          });
        }
      }
    });

    if (checkFieldName) {
      this.SaveSystemStackingOrder();
    } else {
      this._notificationService.showError('Select Group Sort Field');
    }
  }

  setDocGrpFieldValue(req: { TableSchema: string, StackingOrderDetails: any[], StackOrder: { ID: number, Name: string, StackingOrderFieldName: string } }) {
    this._loanTypeData.SetDocGrpFieldValue(req).subscribe(res => {
      if (res !== null) {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result === null) { this._notificationService.showError('Error Contact Administrator'); }
      } else { this._notificationService.showError('Error Contact Administrator'); }
    });
  }

  getStackUnDocTypes(stackType: string = '') {
    if (isTruthy(this._stackUnAssignedDocs) && this._stackUnAssignedDocs.length > 0) {
      this.StackUnAssignedDocs.next(this._stackUnAssignedDocs.slice());
      return Subscription.EMPTY;
    } else {
      return this.getStackUnAssignedDocs(stackType);
    }
  }
  getStackAssignDocTypes(stackType: string = '') {
    if (isTruthy(this._stackAssignedDocs) && this._stackAssignedDocs.length > 0) {
      this.stackAssignedDocs.next(this._stackAssignedDocs.slice());
      return Subscription.EMPTY;
    } else {
      return this.getStackDocs(stackType);
    }
  }
  getAllDocTypes() {
    return this._allDocTypes.slice();
  }

  getAssignedDocTypes() {
    this.assignedDocs.next(this._assignedDocTypes);
    return this._assignedDocTypes.slice();
  }

  setDocTypes(_assignedDocs: any[], _allDocs: any[]) {
    this._assignedDocTypes = _assignedDocs.slice();
    this._allDocTypes = _allDocs.slice();
    this.assignedDocs.next(_assignedDocs);
    this.getCurrentDocs.next(true);
    this.getSysLoanTypeDocs(this._assignedDocTypes);
  }

  assignChecklist(req: CloneChecklistRequest) {
    req.CheckListID = this._checkList.CheckListID;
    req.LoanTypeID = this._loanTypeID;
    return this._loanTypeData.AssignChecklist(req).subscribe(res => {
      if (res !== null) {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result !== null) {
          this.setSystemChecklist({ CheckListID: Result.CheckListID, CheckListName: req.CheckListName });
          this._commonService.SetSystemChecklistMaster({ CheckListID: Result.CheckListID, CheckListName: req.CheckListName });
          this._commonRuleBuilderService.setSystemChecklist({ CheckListID: Result.CheckListID, CheckListName: req.CheckListName });
          this._checkListType = 'edit';
          this._notificationService.showSuccess('Checklist Assigned Successfully');
          this.checkListType.next(this._checkListType);
          this.EnableEditChecklistFeatures.next(true);
        } else {
          this._notificationService.showError('Cannot able to assign checklist');
        }
      }
    });
  }

  AssignStackingOrder() {

    const req = new AssignStackingOrderRequestModel(this._loanTypeID, this._stackinOrder.StackingOrderID, this._stackinOrderName);
    this._loanTypeData.AssignStackingOrder(req).subscribe(res => {
      if (res !== null) {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result !== null) {
          this._stackinOrder = { StackingOrderID: Result.StackingOrderID, Description: this._stackinOrderName };
          this._notificationService.showSuccess('Stacking Order Assigned Successfully');
          this._stackingOrderType = 'edit';
          this.stackingOrderType.next(this._stackingOrderType);
        }
      } else {
        this._notificationService.showError('Error while creating');
      }
    });
  }

  SaveSysCheckList(req: AddChecklistGroupModel) {
    this._loanTypeData.SaveSysCheckList(req).subscribe(res => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        if (data.length > 0) {
          this.setSystemChecklist({ CheckListID: data[0].CheckListID, CheckListName: req.checkListMaster.CheckListName });
          this._commonService.SetSystemChecklistMaster(this._checkList);
          this._commonRuleBuilderService.setSystemChecklist(this._checkList);
          this.setLoanTypeMapping();
          this._checkListType = 'edit';
          this.checkListType.next(this._checkListType);
          this.EnableEditChecklistFeatures.next(true);
          this._notificationService.showSuccess('Checklist Group Added Successfully');
        } else {
          this._notificationService.showError('Create Checklist Group Failed');
        }
      }
    });
  }

  checkDuplicateChecklistGrp(newName: string) {
    const checkListGrp = this._commonService.GetSystemChecklistMaster(true);
    if (checkListGrp.length > 0 && isTruthy(newName) && isTruthy(this._checkList) && isTruthy(this._checkList.CheckListName)) {
      if (this._checkList.CheckListName.toLowerCase() !== newName.toLowerCase()) {
        const existCheck = checkListGrp.filter(x => x.CheckListName.toString().toLowerCase() === newName.toLowerCase());
        if (existCheck.length > 0) {
          this._notificationService.showError('Checklist Group Already Exists !');
        }
      }
    }
  }

  checkDuplicateStackingOrderGrp(newName: string) {
    this._stackinOrderName = newName;
    const stackOrderGrp = this._commonService.GetSysStackingOrderGroup();
    if (stackOrderGrp.length > 0 && isTruthy(newName)) {
      if (stackOrderGrp.filter(x => x.Description.toUpperCase() === newName.toUpperCase() && x.StackingOrderID !== this._stackinOrder.StackingOrderID).length > 0) {
        this._notificationService.showError('Stacking Order Already Exists !');
        return false;
      } else { return true; }
    }
    return false;
  }

  checkDuplicateChecklist(newName: string) {
    const checkListGrp = this._commonService.GetSystemChecklistMaster(true);
    if (checkListGrp.length > 0 && isTruthy(newName)) {
      const existCheck = checkListGrp.filter(x => x.CheckListName.toString().toLowerCase() === newName.toLowerCase());
      if (existCheck.length > 0) {
        this._notificationService.showError('Checklist Group Already Exists !');
        return false;
      } else {
        return true;
      }
    }
    return false;
  }

  setSystemChecklist(checkList: { CheckListID: number, CheckListName: string }) {
    this._checkList = checkList;
  }

  setSystemStackingOrder(stackingOrder: { StackingOrderID: number, Description: string }) {
    this._stackinOrder = stackingOrder;
  }

  getChecklist() {
    return this._checkList;
  }

  getSystemStackingOrderDetails() {
    if (isTruthy(this._sysStackingOrderDetailData) && this._sysStackingOrderDetailData.length > 0) {
      this.SysStackingOrderDetailData.next({ StackingOrder: this._sysStackingOrderDetailData.slice(), StackingOrderGroup: this._sysGroupStackingOrderDetailData.slice(), UnAssignedDocTypes: this._sysUnAssignedStackingOrder.slice() });
      return Subscription.EMPTY;
    } else {
      const req = { StackingOrderID: this._stackinOrder.StackingOrderID };
      return this._loanTypeData.GetSystemStackingOrderDetails(req).subscribe(res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this._sysStackingOrderDetailData = [];
          data.forEach(element => {
            if (element.StackingOrderGroupDetails.length === 0) {
              this._sysStackingOrderDetailData.push(
                {
                  DocumentTypeID: element.DocumentTypeID,
                  DocumentTypeName: element.DocumentTypeName,
                  SequenceID: element.SequenceID,
                  StackingOrderDetailID: element.StackingOrderDetailID,
                  StackingOrderID: element.StackingOrderID,
                  DocFieldList: element.DocFieldList,
                  OrderByFieldID: element.OrderByFieldID,
                  isGroup: false,
                  DocFieldValueId: element.DocFieldValueId,
                  isComponentName: 'SystemStackingOrder'
                }
              );
            } else {
              let fieldId = 0;
              const dupGroup = this._sysStackingOrderDetailData.filter(el => el.StackingOrderGroupName === element.StackingOrderGroupDetails[0].StackingOrderGroupName);
              element.DocFieldList.forEach(ele => {
                if (ele.DisplayName === element.StackingOrderGroupDetails[0].GroupSortField) {
                  fieldId = ele.FieldID;
                }
              });
              if (dupGroup.length === 0) {
                this._sysStackingOrderDetailData.push(
                  {
                    DocumentTypeID: true,
                    DocumentTypeName: element.DocumentTypeName,
                    SequenceID: element.SequenceID,
                    StackingOrderDetailID: element.StackingOrderDetailID,
                    StackingOrderID: element.StackingOrderID,
                    DocFieldList: element.DocFieldList,
                    OrderByFieldID: fieldId,
                    DocFieldValueId: element.DocFieldValueId,
                    isGroup: true,
                    isContainer: true,
                    StackingOrderGroupName: element.StackingOrderGroupDetails[0].StackingOrderGroupName,
                    isComponentName: 'SystemStackingOrder'
                  });
              }
              this._sysGroupStackingOrderDetailData.push(
                {
                  DocumentTypeID: element.DocumentTypeID,
                  DocumentTypeName: element.DocumentTypeName,
                  SequenceID: element.SequenceID,
                  StackingOrderDetailID: element.StackingOrderDetailID,
                  StackingOrderID: element.StackingOrderID,
                  DocFieldList: element.DocFieldList,
                  OrderByFieldID: element.OrderByFieldID,
                  DocFieldValueId: element.DocFieldValueId,
                  isGroup: true,
                  StackingOrderGroupName: element.StackingOrderGroupDetails[0].StackingOrderGroupName,
                  StackingOrderFieldName: element.StackingOrderGroupDetails[0].GroupSortField,
                  isComponentName: 'SystemStackingOrder'
                }
              );
            }
          });

          this._sysUnAssignedStackingOrder = [];
          this._stackUnAssignedDocs.forEach(element => {
            const docDetails = this._sysGroupStackingOrderDetailData.find(x => x.DocumentTypeID === element.DocumentTypeID);
            if (docDetails === undefined || !(docDetails.isGroup)) {
              if (this._sysStackingOrderDetailData.filter(x => x.DocumentTypeID === element.DocumentTypeID).length === 0) {
                this._sysUnAssignedStackingOrder.push(
                  {
                    DocumentTypeID: element.DocumentTypeID,
                    DocumentTypeName: element.DocumentTypeName,
                    SequenceID: 0,
                    StackingOrderDetailID: 0,
                    StackingOrderID: this._stackinOrder.StackingOrderID,
                    DocFieldList: element.DocFieldList,
                    OrderByFieldID: element.OrderByFieldID,
                    DocFieldValueId: element.DocFieldValueId,
                    StackingOrderGroupName: '',
                    isGroup: false,
                    StackingGroupDocuments: [],
                    StackingOrderFieldName: '',
                    isComponentName: 'SystemStackingOrder'
                  }
                );
              }
            }
          });
          this.setDocSysStackingOrder(this._sysStackingOrderDetailData.slice(), this._sysGroupStackingOrderDetailData.slice());
          this.SysStackingOrderDetailData.next({ StackingOrder: this._sysStackingOrderDetailData.slice(), StackingOrderGroup: this._sysGroupStackingOrderDetailData.slice(), UnAssignedDocTypes: this._sysUnAssignedStackingOrder.slice() });
        }
      });
    }
  }

  getChecklistDetailTable() {
    this._commonRuleBuilderService.setCheckListItems(this._sysCheckListDetailTableData.slice());
    this.SysCheckListDetailTableData.next(this._sysCheckListDetailTableData.slice());
  }

  GoToLoanType() {
    this.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.Loantype, 'active', '', '', ''));
  }

  getSystemChecklistDetailTable() {
    const req = { CheckListID: this._checkList.CheckListID };
    return this._loanTypeData.GetSystemChecklistDetail(req).subscribe(
      res => {
        if (res.Data !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data.length > 0) {
            data.forEach(element => {
              element.LoanType = this._loanTypeName;
            });
            this._sysCheckListDetailTableData = data;
          } else {
            this._sysCheckListDetailTableData = [];
          }
          this._commonRuleBuilderService.setCheckListItems(this._sysCheckListDetailTableData.slice());
          this.SysCheckListDetailTableData.next(this._sysCheckListDetailTableData.slice());
        } else {
          this._notificationService.showError('Error Fetching Checklist Details');
        }
      });
  }

  setDocFieldValue(req: { DocumentTypeID: number, FieldID: number }, type: string) {
    this._loanTypeData.SetDocFieldValue(req, type).subscribe(
      res => {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        if (data) {
          this._notificationService.showSuccess('Updated Successfully');
        } else {
          this._notificationService.showError('Updated Unsuccessfully');
        }
      });
  }

  getStackingOrder() {
    this._stackinOrderName = this._stackinOrder.Description;
    return this._stackinOrder;
  }

  getStackType() {
    return this._stackingOrderType;
  }

  getCheckListType() {
    return this._checkListType;
  }

  setStackType(type: string) {
    this._stackingOrderType = type;
  }

  setChecklistType(type: string) {
    this._checkListType = type;
  }

  CloneChecklistItem(req: { CheckListDetailsID: any[], ModifiedCheckListName: string, LoanTypeID: number }) {
    req.LoanTypeID = this._loanTypeID;
    return this._loanTypeData.CloneChecklistItem(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result !== null) {
          if (result) {
            this.getSystemChecklistDetailTable();
            this._notificationService.showSuccess('Checklist Item Cloned Successfully');
            this.isRowNotSelected.next(true);
            this.HideCloneMsgModal.next(true);
          }
        }
      });
  }

  DeleteChecklistItems(req: { CheckListDetailsID: any[], LoanTypeID: number }) {
    req.LoanTypeID = this._loanTypeID;
    return this._loanTypeData.DeleteChecklistItems(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result) {
          this.getSystemChecklistDetailTable();
          this._notificationService.showSuccess('Checklist Item Deleted Successfully');
          this.isRowNotSelected.next(true);
        } else {
          this._notificationService.showError('Checklist Item Deleted Failed');
        }
      });
  }
  getStackDocs(stackType: string = '') {
    return this._loanTypeData.GetStackCreateDocs({ LoanTypeID: this._loanTypeID }).subscribe(res => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        data.AssignedDocTypes.forEach(element => {
          this._stackAssignedDocs.push(
            {
              DocumentTypeID: element.DocumentTypeID,
              DocumentTypeName: element.Name,
              SequenceID: 0,
              StackingOrderDetailID: 0,
              StackingOrderID: this._stackinOrder.StackingOrderID,
              DocFieldList: element.DocFieldList,
              OrderByFieldID: element.OrderByFieldID,
              StackingOrderGroupName: '',
              StackingOrderFieldName: '',
              isComponentName: 'SystemStackingOrder'
            }
          );
        });
        data.AllDocTypes.forEach(element => {
          this._stackUnAssignedDocs.push(
            {
              DocumentTypeID: element.DocumentTypeID,
              DocumentTypeName: element.Name,
              SequenceID: 0,
              StackingOrderDetailID: 0,
              StackingOrderID: this._stackinOrder.StackingOrderID,
              DocFieldList: element.DocFieldList,
              OrderByFieldID: element.OrderByFieldID,
              StackingOrderGroupName: '',
              StackingOrderFieldName: '',
              isComponentName: 'SystemStackingOrder'
            }
          );
        });
        if (stackType === 'edit') {
          this.setSysAllDocuments();
        }
        this.StackUnAssignedDocs.next(this._stackUnAssignedDocs.slice());
        this.stackAssignedDocs.next(this._stackAssignedDocs.slice());
      }
    });
  }

  private SaveSystemStackingOrder() {

    if (isTruthy(this._stackinOrderName)) {
      if (this.checkDuplicateStackingOrderGrp(this._stackinOrderName)) {
        const inputs = { TableSchema: '', StackOrderName: this._stackinOrderName, IsActive: true, StackOrderID: this._stackinOrder.StackingOrderID, StackOrder: this._sysStackingDetails };
        this._loanTypeData.SaveSystemStackingOrder(inputs).subscribe(res => {
          if (res !== null) {
            const Result = jwtHelper.decodeToken(res.Data)['data'];
            if (Result !== null) {
              if (Result.Success === true) {
                this._stackinOrder.StackingOrderID = Result.StackOrderID;
                this._stackinOrder.Description = this._stackinOrderName;
                this.SaveStackingOrderMapping();
                this._notificationService.showSuccess('Stacking Order Saved Successfully');
              } else {
                this._notificationService.showError('Error while saving');
              }
            } else {
              this._notificationService.showError('Error while saving');
            }
          }
        });
      }
    } else {
      this._notificationService.showError('Enter Stacking Order Name');
    }
  }

  private SaveStackingOrderMapping() {
    const req = { LoanTypeID: this._loanTypeID, StackingOrderID: this._stackinOrder.StackingOrderID };
    this._loanTypeData.SaveStackingOrderMapping(req).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result) {
          this._notificationService.showSuccess('Loan Type - Stacking order Mapped Successfully');
          this.GotoMaster();
        } else {
          this._notificationService.showError('Mapping Failed');
        }
      }
    );
  }

  private GotoMaster() {
    this._location.back();
  }

  private getStackUnAssignedDocs(stackType: string = '') {
    return this._loanTypeData.GetStackDocumentTypes().subscribe(res => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        data.forEach(element => {
          this._stackUnAssignedDocs.push(
            {
              DocumentTypeID: element.DocumentTypeID,
              DocumentTypeName: element.DisplayName,
              SequenceID: 0,
              StackingOrderDetailID: 0,
              StackingOrderID: this._stackinOrder.StackingOrderID,
              DocFieldList: element.DocFieldList,
              OrderByFieldID: element.OrderByFieldID,
              StackingOrderGroupName: '',
              StackingOrderFieldName: '',
              isComponentName: 'SystemStackingOrder'
            }
          );
        });

        if (stackType === 'edit') {
          this.setSysAllDocuments();
        }
        this.StackUnAssignedDocs.next(this._stackUnAssignedDocs.slice());
      }
    });
  }
  private setSysAllDocuments() {
    const tempUnAssigned = [];
    this._stackUnAssignedDocs.forEach(element => {
      const docDetails = this._sysGroupStackingOrderDetailData.find(x => x.DocumentTypeID === element.DocumentTypeID);
      if (docDetails === undefined || !(docDetails.isGroup)) {
        if (this._sysStackingOrderDetailData.filter(x => x.DocumentTypeID === element.DocumentTypeID).length === 0) {
          tempUnAssigned.push(
            {
              DocumentTypeID: element.DocumentTypeID,
              DocumentTypeName: element.DocumentTypeName,
              SequenceID: 0,
              StackingOrderDetailID: 0,
              StackingOrderID: this._stackinOrder.StackingOrderID,
              DocFieldList: element.DocFieldList,
              OrderByFieldID: element.OrderByFieldID,
              DocFieldValueId: element.DocFieldValueId,
              StackingOrderGroupName: '',
              isGroup: false,
              StackingGroupDocuments: [],
              StackingOrderFieldName: '',
              isComponentName: 'SystemStackingOrder'
            }
          );
        }
      }
    });

    this.SysStackingOrderDetailData.next({ StackingOrder: this._sysStackingOrderDetailData.slice(), StackingOrderGroup: this._sysGroupStackingOrderDetailData.slice(), UnAssignedDocTypes: tempUnAssigned.slice() });

  }
  private getSysDocumentTypes() {

    this._loanTypeData.GetSysDocumentTypes({ LoanTypeID: this._loanTypeID }).subscribe(
      res => {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        this._allDocTypes = result.AllDocTypes;
        this._assignedDocTypes = result.AssignedDocTypes;
        this.LoanTypeId = this._loanTypeID;
        this.setNextStep.next(new LoanTypeWizardStepModel(this.AddLoantypeSteps.AssignDocumentType, 'active complete', 'active', '', ''));
      }
    );
  }
  private setLoanTypeMapping() {
    const req = { LoanTypeID: this._loanTypeID, CheckListID: this._checkList.CheckListID, ChecklistItemSeq: [] };
    this._loanTypeData.SetLoanTypeMapping(req).subscribe(res => {
      if (res !== null) {
        const result = jwtHelper.decodeToken(res.Data)['data'];
        if (result) {
          this._notificationService.showSuccess('Loan Type Mapped Successfully');
        } else {
          this._notificationService.showError('LoanType - Checklist mapping failed');
        }
      }
    });
  }

}
