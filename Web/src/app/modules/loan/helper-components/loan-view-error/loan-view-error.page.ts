import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoanInfoService } from '../../services/loan-info.service';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-loan-view-error',
    templateUrl: 'loan-view-error.page.html',
    styleUrls: ['loan-view-error.page.css']
})
export class LoanViewErrorComponent implements OnInit {

    LoanViewerIssues: any = { CurrentStatus: '', Message: '', BatchID: '', LoanNumber: '' };
    constructor(
        private _loanInfoService: LoanInfoService
    ) { }
    private _subscriptions: Subscription[] = [];
    private _loanViewerIssues: any;

    ngOnInit(): void {
        this._loanViewerIssues = this._loanInfoService.GetLoanViewIssues();
        if (isTruthy(this._loanViewerIssues)) {
            this.LoanViewerIssues.BatchID = this._loanViewerIssues.EphesoftBatchInstanceID;
            this.LoanViewerIssues.CurrentStatus = this._loanViewerIssues.StatusDescription;
            this.LoanViewerIssues.LoanNumber = this._loanViewerIssues.LoanNumber;
            this.LoanViewerIssues.Message = this._loanViewerIssues.Message;
        } else {
            this.LoanViewerIssues.Message = 'Loan Not Found';
        }
    }
}
