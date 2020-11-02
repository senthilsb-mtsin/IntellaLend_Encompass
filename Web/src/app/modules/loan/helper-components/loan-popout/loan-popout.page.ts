import { OnInit, OnDestroy, Component, AfterViewInit } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { LoanInfoService } from '../../services/loan-info.service';
import { SessionHelper } from '@mts-app-session';
import { Loan } from '../../models/loan-details.model';
import { Subscription } from 'rxjs';

@Component({
    selector: 'mts-loan-popout',
    templateUrl: 'loan-popout.page.html',
    styleUrls: ['loan-popout.page.css'],
    providers: [LoanInfoService]
})
export class LoanPopOutComponent implements OnInit, OnDestroy, AfterViewInit {
    showHide: any[] = [false];
    loan: Loan = new Loan();
    viewFotterClass = 'page-content-wrapper';
    mainViewClass = 'page-container';
    promise: Subscription;
    soWidth = 'col-md-3 p0';
    imgDivWidth = 'col-md-8';
    fieldWidth = 'col-md-1';

    constructor(
        public _notificationService: NotificationService,
        public _loanInfoService: LoanInfoService) {
        this.checkPermission('ReadonlyLoans', 0);
        this.loan = JSON.parse(localStorage.getItem('loan_details'));
        this._loanInfoService.SetLoan(this.loan);
        this._loanInfoService.SetLoanPageInfo(this.loan.LoanHeaderPopOutInfo);
        this._loanInfoService.SetLoanViewDocument(this.loan.doc);
        this._loanInfoService.LoanDetails$.next(this.loan);
    }

    private _subscriptions: Subscription[] = [];

    ngAfterViewInit(): void {
        setTimeout(() => {
            this._loanInfoService.LoanPopOutSOHeight$.next('SOPopOutHeight');
            this._loanInfoService.LoanPopOutFieldHeight$.next('FieldPopOutHeight');
            this._loanInfoService.LoanPopOutImageViewerHeight$.next('calc(100vh - 85px)');
            this._loanInfoService.ShowDocumentDetailView$.next(true);
        }, 300);
    }

    ngOnInit(): void {

        this._subscriptions.push(this._loanInfoService.FIELDToggle$.subscribe((res: boolean) => {
            this.fieldWidth = this.fieldWidth === 'col-md-3' ? 'col-md-1' : 'col-md-3';
            this.CheckDivWidth();
        }));

        this._subscriptions.push(this._loanInfoService.STACKToggle$.subscribe((res: boolean) => {
            this.soWidth = this.soWidth === 'col-md-3 p0' ? 'col-md-1 p0' : 'col-md-3 p0';
            this.CheckDivWidth();
        }));

    }

    CheckDivWidth() {
        if (this.soWidth === 'col-md-1 p0' && this.fieldWidth === 'col-md-1') {
            this.imgDivWidth = 'col-md-10';
        } else if (this.soWidth === 'col-md-1 p0' && this.fieldWidth === 'col-md-3') {
            this.imgDivWidth = 'col-md-8';
        } else if (this.soWidth === 'col-md-3 p0' && this.fieldWidth === 'col-md-1') {
            this.imgDivWidth = 'col-md-8';
        } else if (this.soWidth === 'col-md-3 p0' && this.fieldWidth === 'col-md-3') {
            this.imgDivWidth = 'col-md-6';
        }
        setTimeout(() => {
            this._loanInfoService.ImageFit$.next(true);
        }, 300);
    }

    typeCheck(val): boolean {
        return (val === undefined);
    }

    checkPermission(component: string, index: number, ): void {
        const URL = 'View\\LoanDetails\\' + component;
        const AccessCheck = false;
        const AccessUrls = SessionHelper.RoleDetails.URLs;

        if (AccessCheck !== null) {
            AccessUrls.forEach(element => {
                if (element.URL === URL) {
                    this.showHide[index] = true;
                    return false;
                }
            });
        }
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
