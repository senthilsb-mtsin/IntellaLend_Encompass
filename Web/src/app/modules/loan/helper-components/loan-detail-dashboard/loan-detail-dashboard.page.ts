import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
  selector: 'mts-loan-detail-dashboard',
  templateUrl: 'loan-detail-dashboard.page.html',
  styleUrls: ['loan-detail-dashboard.page.scss']
})
export class LoanDetailDashboardComponent implements OnInit, OnDestroy {
  fieldDivWidth: any = 'FIELD_Width';
  FIELD_textShow: any = 'FIELD_textHide';
  imgWidth: any = '';
  imgDivWidth: any = 'width95';

  tabHeaders: any[] = [
    { id: 'tab1', name: 'Checklist', badge: false, icon: 'Lh30 fa fa-list' },
    { id: 'tab2', name: 'Re-Verification', badge: true, icon: 'Lh30 fa fa-check' },
    { id: 'tab3', name: 'Notes', badge: false, icon: 'Lh30 fa fa-sticky-note-o' },
    { id: 'tab4', name: 'Loan Stipulation', badge: false, icon: 'Lh30 fa fa-history' },
    { id: 'tab5', name: 'History', badge: false, icon: 'Lh30 fa fa-file-text-o' }
  ];
  selectedTab = this.tabHeaders[0].id;
  showImage = false;
  ShowDocumentDetailView = false;
  CurrentDocName = '';

  constructor(
    private _loanInfoService: LoanInfoService
  ) { }

  private _subscriptions: Subscription[] = [];
  private myWindow: any;

  ngOnInit(): void {
    this._subscriptions.push(this._loanInfoService.ShowDocumentDetailView$.subscribe((res: boolean) => {
      this.ShowDocumentDetailView = res;
      this.CurrentDocName = this._loanInfoService.GetLoanViewDocument()._currentDocName;
      this._loanInfoService.FIELDToggle$.next(true);
    }));

    this._subscriptions.push(this._loanInfoService.FIELDToggle$.subscribe((res: boolean) => {
      if (!this.ShowDocumentDetailView) {
        this.FIELD_textShow = 'FIELD_textHide FI_sub_icon';
        this.fieldDivWidth = 'FIELD_Width';
        this.imgDivWidth = 'width95';
      } else { this.FIELD_Toggle(); }
    }));
  }

  showTracker() {
    this._loanInfoService.ShowDocumentDetailView$.next(false);
    this._loanInfoService.FIELDToggle$.next(true);
    this._loanInfoService.SOToggle$.next(true);
  }

  popOut() {
    // this._loan.lastPageNumber = 0;
    // this._loan.checkListState = false;
    // this._loan.pageNumberArray = [];
    // this._loan.SingleimageState = true;
    if (isTruthy(this.myWindow)) {
      if (!this.myWindow.closed) {
        this.myWindow.close();
      }
    }
    const loanDetails = {
      LoanID: this._loanInfoService.GetLoan().LoanID,
      loanStackingOrder: this._loanInfoService.GetLoan().loanStackingOrder,
      loanGroupDocuments: this._loanInfoService.GetLoanDocuments(),
      loanDocuments: this._loanInfoService.GetLoan().loanDocuments,
      doc: this._loanInfoService.GetLoanViewDocument(),
      showDownload: true,
      LoanNumber: this._loanInfoService.GetLoanTableDetails().LoanNumber,
      LoanHeaderPopOutInfo: this._loanInfoService.GetLoanTableDetails()
    };

    localStorage.setItem('loan_details', JSON.stringify(loanDetails));

    this.myWindow = window.open('loanpopout', 'Win1', 'width=2000,height=750,channelmode=1,scrollbars=1,status=0,titlebar=0,toolbar=0,resizable=1');
  }

  FIELD_Toggle() {
    if (this.FIELD_textShow === '') {
      this.FIELD_textShow = 'FIELD_textHide FI_sub_icon';
      this.fieldDivWidth = 'FIELD_Width';
      this.imgDivWidth = 'width95';
    } else {
      this.fieldDivWidth = '';
      this.FIELD_textShow = '';
      this.imgDivWidth = '';
    }
    setTimeout(() => {
      this._loanInfoService.ImageFit$.next(true);
    }, 300);
  }

  ngOnDestroy(): void {
    this._subscriptions.forEach(element => {
      element.unsubscribe();
    });
  }
}
