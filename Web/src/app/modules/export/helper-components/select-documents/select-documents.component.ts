import { Subscription } from 'rxjs';
import { Component, OnInit, QueryList, ElementRef, ViewChildren, OnDestroy } from '@angular/core';
import { ExportLoanService } from '../../service/export-loan.service';
import { AppSettings } from '@mts-app-setting';

@Component({
  selector: 'mts-select-documents',
  templateUrl: './select-documents.component.html',
  styleUrls: ['./select-documents.component.css']
})
export class SelectDocumentsComponent implements OnInit, OnDestroy {
  promise: Subscription;
  _selectLoans: any;
  loanDocuments: any = [];
  loan: any = [];
  DocNotes = [];
  LoanID: any;
  NoData = true;
  checkExistDoc = [];
  _showSelectall = false;
  _selectDocuments: string;
  selectAllDocBtn = true;
  @ViewChildren('allLoan') _allLoan: QueryList<ElementRef>;
  scrollbarOptions = { axis: 'yx', theme: 'minimal-dark', scrollbarPosition: 'inside' };
  constructor(public _exportloanservice: ExportLoanService) { }
  private subscribtion: Subscription[] = [];
  ngOnInit(): void {
    this.subscribtion.push(this._exportloanservice.documentcount$.subscribe((res: boolean) => {
      this._showSelectall = res;
      this.NoData = !res;
    }));
    this.subscribtion.push(this._exportloanservice.selectAllDocBtn$.subscribe((res: boolean) => {
      this.selectAllDocBtn = res;
    }));

    this._exportloanservice.validatedocument();
  }
  SelectLoan(LoanID: any, index) {
    this._selectDocuments = '';
    this._showSelectall = false;
    this.NoData = true;
    this._allLoan['_results'].forEach(element =>
      element.nativeElement.className = 'loandoclist ng-star-inserted',
    );
    const clist = this._allLoan['_results'][index].nativeElement.classList.toString();
    if (clist.indexOf('SelectHighlight') > -1) {
      this._allLoan['_results'][index].nativeElement.className = clist.replace('SelectHighlight', '');
    } else {
      this._allLoan['_results'][index].nativeElement.className = clist + ' ' + 'SelectHighlight';
    }
    let currentLoanDocExist = false;
    const _currentloandocselect = [];
    this._exportloanservice.LoanDetail.forEach(element => {

      if (element.LoanID === LoanID) {
        element.CurrentLoan = true;
        currentLoanDocExist = element.DocumentDetails.length > 0;
        _currentloandocselect.push(element.DocumentDetails.filter(x => x.IsChecked === false));
      } else {
        element.CurrentLoan = false;
      }
    });
    if (!currentLoanDocExist) {
      this.GetLoanDocuments(LoanID);
    } else {

      this.selectAllDocBtn = (_currentloandocselect.length > 0) ? false : true;
      this._showSelectall = true;
      this.NoData = false;
    }
  }

  GetLoanDocuments(LoanID) {
    this.selectAllDocBtn = true;
    const inputReq = { TableSchema: AppSettings.TenantSchema, LoanID: LoanID };
    this._exportloanservice.LoanDetail.DocumentDetails = [];
    this.promise = this._exportloanservice.GetLoanDocuments(inputReq);
  }

  DocumentSelect(LoanID: any) {
    this._exportloanservice.LoanDetail.forEach(x => {
      const checkIsDocSelected = x.DocumentDetails.filter(d => d.IsChecked === true);
      if (typeof checkIsDocSelected !== undefined && checkIsDocSelected.length > 0 && x.LoanID === LoanID) {
        x.IsSelected = true;
      } else {
        x.IsSelected = (x.LoanID === LoanID) ? false : x.IsSelected;
      }
    });
  }
  RemoveLoanList(LoanId: any, _index: any) {
    this._exportloanservice.RemoveLoanList(LoanId, _index);
  }
  SelectAllDoc() {
    if (this.selectAllDocBtn) {
      this._exportloanservice.LoanDetail.forEach(element => {
        if (element.CurrentLoan === true) {
          element.DocumentDetails.forEach(doc => {
            doc.IsChecked = true;
            element.IsSelected = true;
          });
        }
      });
      this.selectAllDocBtn = false;
    } else {
      this._exportloanservice.LoanDetail.forEach(element => {
        if (element.CurrentLoan === true) {
          element.DocumentDetails.forEach(doc => {
            doc.IsChecked = false;
            element.IsSelected = false;
          });
        }
      });
      this.selectAllDocBtn = true;
    }
  }
  ngOnDestroy() {
    this.subscribtion.forEach((element) => {
      element.unsubscribe();
    });
  }
}
