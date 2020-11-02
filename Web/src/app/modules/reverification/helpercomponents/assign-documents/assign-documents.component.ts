import { Component, OnInit, OnDestroy, ChangeDetectorRef, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { ReverificationService } from '../../services/reverification.service';
import { DragulaService } from '@mts-dragula';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { DocMasterSearchPipe } from '@mts-pipe';
import { DocumentSearchService } from 'src/app/modules/loantype/service/document-search.service';

@Component({
  selector: 'mts-assign-documents',
  styleUrls: ['assign-documents.component.css'],
  templateUrl: 'assign-documents.component.html',
  providers: [DocMasterSearchPipe, DocumentSearchService],
})
export class AssignDocumentsComponent implements OnInit, OnDestroy {
  @ViewChildren('allDocs') _allDocChildrens: QueryList<ElementRef>;
  @ViewChildren('assignedDoc') _assignDocChildrens: QueryList<ElementRef>;
  LoanTypeID: any = 0;
  MappingID: any = 0;
  allDocSearchval: any = '';
  scrollbarOptions = { theme: 'minimal-dark', scrollbarPosition: 'inside', axis: 'y' };
  AllDocTypes: any = [];
  AssignedDocTypes: any = [];
  AssignedDocFieldTypes: any = [];
  assignedDocSearchval: any = '';
  FilterAllDocTypes: any = [];
  loadDocs = false;
  constructor(
    private _reverificationService: ReverificationService,
    private _documentSearch: DocumentSearchService,
    private dragService: DragulaService,
    private _docmasterPipe: DocMasterSearchPipe
  ) {

  }
  private subscriptions: Subscription[] = [];

  ngOnInit() {
    this.ResetPageData();
    this.dragService.dropModel().subscribe((value) => {
      this.onDropModel(value);
    });

  }
  ResetPageData() {
    this.AllDocTypes = this._reverificationService.getAllDocTypes();
    this.AssignedDocTypes = this._reverificationService.getAssignedDocTypes();
    this.FilterAllDocTypes = this._reverificationService.getAllDocTypes();
  }
  assignDocChange(assignedDocSearchval) {
    this.AssignedDocTypes = this._docmasterPipe.transform(this.AssignedDocTypes, assignedDocSearchval);
    if (assignedDocSearchval === '') {
      this.ResetPageData();

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
        this._reverificationService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
      }
    }
  }
  MoveFromAllDoc() {
    for (let i = 0; i < this._allDocChildrens['_results'].length; i++) {
      const clist = this._allDocChildrens['_results'][i].nativeElement.classList.toString();
      const docID = parseInt(this._allDocChildrens['_results'][i].nativeElement.attributes['data-docid'].value, 10);
      const index = this.AllDocTypes.findIndex(l => l.DocumentTypeID === docID);
      if (clist.indexOf('SelectHighlight') > -1) {
        this.AssignedDocTypes.push(this.AllDocTypes[index]);
        this.AllDocTypes.splice(index, 1);
      }
    }
    this._reverificationService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);

  }

  MoveToAllDoc() {
    for (let i = 0; i < this._assignDocChildrens['_results'].length; i++) {
      const clist = this._assignDocChildrens['_results'][i].nativeElement.classList.toString();
      const docTypeID = parseInt(this._assignDocChildrens['_results'][i].nativeElement.attributes['data-docid'].value, 10);

      const index = this.AssignedDocTypes.findIndex(l => l.DocumentTypeID === docTypeID);
      if (clist.indexOf('SelectHighlight') > -1) {
        this.AllDocTypes.push(this.AssignedDocTypes[index]);
        this.AssignedDocTypes.splice(index, 1);
        this._reverificationService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);
      }
    }

  }

  setDocSelected(index) {
    const clist = this._allDocChildrens['_results'][index].nativeElement.classList.toString();
    if (clist.indexOf('SelectHighlight') > -1) {
      this._allDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
    } else {
      this._allDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
    }

  }

  RemoveDocumentType(docID) {

    const index = this.AssignedDocTypes.findIndex(l => l.DocumentTypeID === docID);
    if (isTruthy(index)) {
      this.AllDocTypes.push(this.AssignedDocTypes[index]);
      if (isTruthy(this.allDocSearchval)) {
        const assignDocValue = this.assignedDocSearchval;
        this.assignedDocSearchval = '';
        this.AssignedDocTypes.splice(index, 1);
        this.assignedDocSearchval = assignDocValue;
      } else {
        this.AssignedDocTypes.splice(index, 1);
      }
      this._reverificationService.setDocTypes(this.AssignedDocTypes, this.AllDocTypes);

    }

  }

  setDocAssignSelected(index) {
    const clist = this._assignDocChildrens['_results'][index].nativeElement.classList.toString();
    if (clist.indexOf('SelectHighlight') > -1) {
      this._assignDocChildrens['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
    } else {
      this._assignDocChildrens['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
    }

  }
  UnassignedDocFilterSearch(search) {
    this.AllDocTypes = this._documentSearch.LoanTypeDocFiltersearch(search, this.FilterAllDocTypes, this.AssignedDocTypes);
  }

  ngOnDestroy() {
    this.dragService.destroy('nested-type');
    this.subscriptions.forEach((element) => {
      element.unsubscribe();
    });
  }

  private onDropModel(args) {
    if (args.target.getAttribute('DivType') === 'Assigned') {
      args.el.classList.add('DropHighlight');
    }
  }
}
