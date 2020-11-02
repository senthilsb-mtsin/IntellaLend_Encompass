import { Component, OnInit, OnDestroy, ViewChildren, QueryList, ElementRef, ViewChild } from '@angular/core';
import { AddLoanTypeService } from '../../service/add-loantype.service';
import { Subscription } from 'rxjs';
import { DocumentSearchService } from '../../service/document-search.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { DragulaService } from '@mts-dragula';
import { AssignDocumentTypeRequestModel, DocMappingDetail } from '../../models/assign-document-types-request.model';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ConditionGeneralRuleService } from '../../service/condition-general-rule.service';
import { LoanTypeService } from '../../service/loantype.service';
import { NotificationService } from '@mts-notification';
import { DocumentLevelConstant } from '@mts-status-constant';
import { timeStamp } from 'console';

@Component({
  selector: 'mts-assign-document-type',
  styleUrls: ['assign-document-type.component.css'],
  templateUrl: 'assign-document-type.component.html',
  providers: [DocumentSearchService],
})
export class AssignDocumentTypesComponent implements OnInit, OnDestroy {
  ConditionExistdocID = [];
  ConditionExistdocNames = [];
  AllDocTypes: any[] = [];
  AssignedDocTypes: any[] = [];
  FilterAllDocTypes: any[] = [];
  allDocSearchval = '';
  assignedDocSearchval = '';
  DocumentLevel: any;
  _docmappingDetails: DocMappingDetail[];
  DocName: any = '';
  _currentDocId: number;
  ruleSaveButton: boolean;
  ruleFormationValues: string;
  errMsgStyle = '';
  _ruleModalDisabled = false;
  @ViewChildren('allDocs') _allDocChildrens: QueryList<ElementRef>;
  @ViewChildren('assignedDoc') _assignDocChildrens: QueryList<ElementRef>;
  @ViewChild('GeneralREuleModal') GeneralREuleModal: ModalDirective;
  @ViewChild('DocLevelModal') DocLevelModal: ModalDirective;

  _loanType: { Type: string, LoanTypeID: number, LoanTypeName: string, Active: boolean };
  _docid: number;
  _currentDocname: any;
  constructor(
    private _addLoantypeService: AddLoanTypeService,
    private _loanService: LoanTypeService,
    private _documentSearch: DocumentSearchService,
    private dragService: DragulaService,
    private _conditionRuleService: ConditionGeneralRuleService,
    private _notificationService: NotificationService,
  ) { }

  private subscriptions: Subscription[] = [];
  ngOnInit() {
    this.resetPageData();
    this.dragService.dropModel().subscribe((value) => {
      this.onDropModel(value);
    });
    this.dragService.dragend().subscribe(() =>{
      this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
    })
    this._loanType = this._loanService.getLoanType();
    this.subscriptions.push(this._addLoantypeService.GeneralRuleclose.subscribe((res: boolean) => {

      if (res) {
        this.GeneralREuleModal.hide();
        this._notificationService.showSuccess('Condition Updated Successfully');
      } else {
        this.GeneralREuleModal.hide();
        this._notificationService.showSuccess('Failed');
      }
    }));

    this.subscriptions.push(this._conditionRuleService.AssignDocSaved.subscribe((res: boolean) => {
      if (res) {
        this._conditionRuleService.GetCustLoanDocMapping(this._loanType.LoanTypeID, this.AssignedDocTypes);

      }

    }));
    this.subscriptions.push(this._conditionRuleService.GeneralRuleModalShow.subscribe((res: boolean) => {
      if (res) {
        this.GeneralREuleModal.show();
      }

    }));

    this.subscriptions.push(this._conditionRuleService.ruleSave.subscribe((res: boolean) => {
      this.ruleSaveButton = res;
      if (res) {
        this.errMsgStyle = 'red';
      } else {
        this.errMsgStyle = 'green';
      }
    }));
    this.subscriptions.push(this._conditionRuleService.ruleFormula.subscribe((res: string) => {
      this.ruleFormationValues = res;
    }));
    this.subscriptions.push(this._addLoantypeService.getCurrentDocs.subscribe((res: any) => {
      this.resetPageData();
    }));
    this.subscriptions.push(this._conditionRuleService.InitializeForm.subscribe((res: any) => {
      if (res === true) {
        this.ruleFormationValues = '';
      }
    }));
  }
  CancelGeneralREuleModal() {
    this.GeneralREuleModal.hide();
  }
  RemoveGeneralRule() {
    this._addLoantypeService.Removecondition(this._loanType.LoanTypeID, this._docid);

  }
  resetPageData() {
    this.AllDocTypes = this._addLoantypeService.getAllDocTypes();
    this.AssignedDocTypes = this._addLoantypeService.getAssignedDocTypes();
    this.FilterAllDocTypes = this._addLoantypeService.getAllDocTypes();
  }

  MoveFromAllDoc() {
    for (let i = 0; i < this._allDocChildrens['_results'].length; i++) {
      const clist = this._allDocChildrens['_results'][i].nativeElement.classList.toString();
      const docID = parseInt(this._allDocChildrens['_results'][i].nativeElement.attributes['data-docid'].value, 10);
      const index = this.AllDocTypes.findIndex(l => l.DocumentTypeID === docID);
      if (clist.indexOf('SelectHighlight') > -1) {
        this.AssignedDocTypes.push(this.AllDocTypes[index]);
        this.AllDocTypes.splice(index, 1);
        this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
      }
    }
  }

  MoveToAllDoc() {
    this.GetConditionExistDocTypes();
    let _conditionExist;
    for (let i = 0; i < this._assignDocChildrens['_results'].length; i++) {
      const clist = this._assignDocChildrens['_results'][i].nativeElement.classList.toString();
      const docTypeID = parseInt(this._assignDocChildrens['_results'][i].nativeElement.attributes['data-documentid'].value, 10);
      const index = this.AssignedDocTypes.findIndex(l => l.DocumentTypeID === docTypeID);
      if (clist.indexOf('SelectHighlight') > -1) {
        for (let j = 0; j < this.AssignedDocTypes.length; j++) {
          if (this.AssignedDocTypes[j].DocumentTypeID === docTypeID) {
            _conditionExist = this.AssignedDocTypes[j].Condition;
            if (this.ConditionExistdocID.includes(docTypeID)) {
              this._notificationService.showError('Document assigned to a condition');
              this._assignDocChildrens['_results'][i].nativeElement.classList.value = 'col-md-8 draganddrop';
              break;
            } else if (_conditionExist === '') {
              this.AllDocTypes.push(this.AssignedDocTypes[index]);
              this.AssignedDocTypes.splice(index, 1);
              break;
            } else {
              this._notificationService.showError('Document assigned to a condition');
              this._assignDocChildrens['_results'][i].nativeElement.classList.value = 'col-md-8 draganddrop';
              break;

            }
          }
        }

      }
    }
  }

  MoveAllDocumentsToAssignedDocuments() {
    if (this._allDocChildrens['_results'].length > 0) {
      for (let i = 0; i < this._allDocChildrens['_results'].length; i++) {
        const clist = this._allDocChildrens['_results'][i].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
          this._allDocChildrens['_results'][i].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
          this._allDocChildrens['_results'][i].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
      }
      for (let i = 0; i < this._allDocChildrens['_results'].length; i++) {
        const clist = this._allDocChildrens['_results'][i].nativeElement.classList.toString();
        const docID = parseInt(this._allDocChildrens['_results'][i].nativeElement.attributes['data-docid'].value, 10);
        const index = this.AllDocTypes.findIndex(l => l.DocumentTypeID === docID);
        if (clist.indexOf('SelectHighlight') > -1) {
          this.AssignedDocTypes.push(this.AllDocTypes[index]);
          this.AllDocTypes.splice(index, 1);
          this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
        }
      }
    } else {
      for (let index = 0; index < this._assignDocChildrens['_results'].length; index++) {
        const clist = this._assignDocChildrens['_results'][index].nativeElement.classList.toString();
        if (clist.indexOf('SelectHighlight') > -1) {
          this._assignDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
        } else {
          this._assignDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
        }
      }
      for (let i = 0; i < this._assignDocChildrens['_results'].length; i++) {
        const clist = this._assignDocChildrens['_results'][i].nativeElement.classList.toString();
        const docTypeID = parseInt(this._assignDocChildrens['_results'][i].nativeElement.attributes['data-documentid'].value, 10);
        const index = this.AssignedDocTypes.findIndex(l => l.DocumentTypeID === docTypeID);
        if (clist.indexOf('SelectHighlight') > -1) {
          this.AllDocTypes.push(this.AssignedDocTypes[index]);
          this.AssignedDocTypes.splice(index, 1);
          this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
        }
      }
    }
  }

  AddDocumentType(documentID: number) {
    if (documentID > 0) {
      const index = this.AllDocTypes.findIndex(l => l.DocumentTypeID === documentID);
      if (isTruthy(index)) {
        this.AssignedDocTypes.push(this.AllDocTypes[index]);
        if (this.allDocSearchval !== null) {
          const assignDocValue = this.allDocSearchval;
          this.allDocSearchval = '';
          this.AllDocTypes.splice(index, 1);
          this.allDocSearchval = assignDocValue;
        } else {
          this.AllDocTypes.splice(index, 1);
        }
        this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
        // this._conditionRuleService._loanType.LoanTypeID = this._addLoantypeService.LoanTypeId;
      }
    }
  }

  UnassignedDocFilterSearch(search) {
    this.AllDocTypes = this._documentSearch.LoanTypeDocFiltersearch(
      search,
      this.FilterAllDocTypes,
      this.AssignedDocTypes
    );
  }
  SaveGeneralRule() {
    this._addLoantypeService.SetAssignDocuments(this._loanType.LoanTypeID, this._docid);
  }

  setDocSelected(index: number) {

    const clist = this._allDocChildrens['_results'][index].nativeElement.classList.toString();
    if (clist.indexOf('SelectHighlight') > -1) {
      this._allDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
    } else {
      this._allDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
    }
  }

  setDocAssignSelected(index: number) {
    const clist = this._assignDocChildrens['_results'][index].nativeElement.classList.toString();
    if (clist.indexOf('SelectHighlight') > -1) {
      this._assignDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
    } else {
      this._assignDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
    }
  }
  ShowDocLevelModal(DocID: number, docName: any) {
    this._currentDocId = DocID;
    this._currentDocname = docName;
    this.DocLevelModal.show();

  }
  SaveDocLevelValue() {

    this._docmappingDetails = this.AssignedDocTypes.filter(l => l.DocumentTypeID === this._currentDocId);
    if (this._docmappingDetails !== undefined) {
      for (let i = 0; i < this.AssignedDocTypes.length; i++) {
        if (this.AssignedDocTypes[i].DocumentTypeID === this._currentDocId && ($('#DocumentLevel').is(':checked'))) {
          this.AssignedDocTypes[i].DocumentLevel = DocumentLevelConstant.CRITICAL;
          this.DocLevelModal.hide();
          break;

        } else if (this.AssignedDocTypes[i].DocumentTypeID === this._currentDocId && !($('#DocumentLevel').is(':checked'))) {
          this.AssignedDocTypes[i].DocumentLevel = DocumentLevelConstant.NON_CRITICAL;
          this.DocLevelModal.hide();

          break;
        }
      }

      this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
    }
  }
  CloseModal() {
    this._docmappingDetails = this.AssignedDocTypes.filter(l => l.DocumentTypeID === this._currentDocId);
    if (this._docmappingDetails !== undefined) {
      for (let i = 0; i < this.AssignedDocTypes.length; i++) {
        if (this.AssignedDocTypes[i].DocumentTypeID === this._currentDocId && this.AssignedDocTypes[i].DocumentLevel === DocumentLevelConstant.CRITICAL) {
          this.AssignedDocTypes[i].DocumentLevel = DocumentLevelConstant.CRITICAL;
          this.DocLevelModal.hide();
          $('#DocumentLevel').prop('checked', true);

          break;

        } else if (this.AssignedDocTypes[i].DocumentTypeID === this._currentDocId && this.AssignedDocTypes[i].DocumentLevel === DocumentLevelConstant.NON_CRITICAL) {
          $('#DocumentLevel').prop('checked', false);
          this.DocLevelModal.hide();

          break;
        }
      }
    }
    this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
  }
  RemoveDocumentType(docID: number, Name: string, _condition: any) {
    this.GetConditionExistDocTypes();
    if (this.ConditionExistdocNames.includes(Name)) {
      this._notificationService.showError('Document assigned to a condition');

    } else if (_condition === null || _condition === '') {
      const test = _condition.split(',');
      const index = this.AssignedDocTypes.findIndex(l => l.DocumentTypeID === docID);
      if (index !== undefined) {
        this.AllDocTypes.push(this.AssignedDocTypes[index]);
        if (this.assignedDocSearchval !== null) {
          const assignDocValue = this.assignedDocSearchval;
          this.assignedDocSearchval = '';
          this.AssignedDocTypes.splice(index, 1);
          this.assignedDocSearchval = assignDocValue;
        } else {
          this.AssignedDocTypes.splice(index, 1);
        }
        this._addLoantypeService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
      }
    } else {
      this._notificationService.showError('Document assigned to a condition');
    }
  }
  AddCondition(docName: any, _docid: number, _docLevel: number) {
    if (_docLevel === DocumentLevelConstant.CRITICAL) {
      this._docid = _docid;
      this.DocName = docName;
      this._conditionRuleService.setDocumentTypeID(_docid);
      this._loanType = this._loanService.getLoanType();
      this._conditionRuleService.GetCustLoanDocMapping(this._docid, this.AssignedDocTypes);
    }
  }

  ngOnDestroy() {
    this.dragService.destroy('nested-type');
    this.subscriptions.forEach((element) => {
      element.unsubscribe();
    });
  }
  GetConditionExistDocTypes() {
    this.ConditionExistdocNames = [];
    this.ConditionExistdocID = [];
    for (let i = 0; i < this.AssignedDocTypes.length; i++) {
      if (this.AssignedDocTypes[i].Condition !== null && this.AssignedDocTypes[i].Condition !== '') {
        const _conditionDoc = JSON.parse(this.AssignedDocTypes[i].Condition);
        for (let j = 0; j < this.AssignedDocTypes.length; j++) {
          if (_conditionDoc.formula.search(this.AssignedDocTypes[j].Name) !== -1) {
            if (!this.ConditionExistdocNames.includes(this.AssignedDocTypes[j].Name)) {
              this.ConditionExistdocNames.push(this.AssignedDocTypes[j].Name);
              this.ConditionExistdocID.push(this.AssignedDocTypes[j].DocumentTypeID);
            }
          }

        }
      }

    }
  }
  private onDropModel(args) {
    if (args.target.getAttribute('DivType') === 'Assigned') {
      args.el.classList.add('DropHighlight');
    }
  }
}
