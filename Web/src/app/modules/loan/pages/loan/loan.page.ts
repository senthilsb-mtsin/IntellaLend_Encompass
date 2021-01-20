import { Component, OnInit, OnDestroy } from '@angular/core';
import { LoanInfoService } from '../../services/loan-info.service';
import { Subscription } from 'rxjs';
import { LoanSearchTableModel } from 'src/app/modules/loansearch/models/loan-search-table.model';
import { Location } from '@angular/common';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Loan } from '../../models/loan-details.model';
import { NotificationService } from '@mts-notification';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { StatusConstant } from '@mts-status-constant';

@Component({
    selector: 'mts-loan-detail',
    templateUrl: 'loan.page.html',
    styleUrls: ['loan.page.css'],
})
export class LoanComponent implements OnInit, OnDestroy {

    loanSearchTableData: LoanSearchTableModel = new LoanSearchTableModel();
    LoanFound = false;
    Loan: Loan = new Loan();
    Promise: Subscription;
    ShowErrorDiv = false;
    imgWidth: any = '';
    SO_width: any = '';
    imgDivWidth: any = 'width95';
    ShowDocumentDetailView = false;

    constructor(
        private _loanInfoService: LoanInfoService,
        private _location: Location,
        private _route: Router,
        private _activeRoute: ActivatedRoute,
    ) {
        this._activeRoute.params.subscribe((params: Params) => {
            if (isTruthy(params) && isTruthy(params['id'])) {
                if (params['id'] !== 'loanviewerror') {
                    this._encryptedLoanGUID = params['id'];
                    this.Promise = this._loanInfoService.GetLoanInfo(this._encryptedLoanGUID);
                    this._loanInfoService.IsDirectLink = true;
                }
            } else {
                this.setLoanDetails();
                this._loanInfoService.IsDirectLink = false;
            }
        });
    }

    private _subscriptions: Subscription[] = [];
    private _encryptedLoanGUID = '';

    setLoanDetails() {
        this.loanSearchTableData = this._loanInfoService.GetLoanTableDetails();
        if (!isTruthy(this.loanSearchTableData) || !isTruthy(this.loanSearchTableData.LoanID)) {
            this._location.back();
        } else {
            this.GetLoanDetails();
        }
    }

    ngOnInit() {

        this._subscriptions.push(this._loanInfoService.LoanInfo$.subscribe((res: LoanSearchTableModel) => {
            if (isTruthy(res) && (res['StatusID'] === StatusConstant.PENDING_AUDIT || res['StatusID'] === StatusConstant.COMPLETE)) {
                this._loanInfoService.SetLoanPageInfo(res);
                this.LoanFound = true;
                this.setLoanDetails();
            } else {
                if (isTruthy(res)) {
                    res.Message = 'Loan not in viewable status';
                }

                this._loanInfoService.SetLoanViewIssues(res);
                this._route.navigate(['view/loandetails/loanviewerror']);
            }
        }));

        this._subscriptions.push(this._loanInfoService.LoanDetails$.subscribe((res: Loan) => {
            if (isTruthy(res) && res.LoanID === this.loanSearchTableData.LoanID) {
                this.LoanFound = true;
                this.Loan = res;
            } else if (res !== null && res.LoanID === 0) {
                this.ShowErrorDiv = true;
            }
        }));

        this._subscriptions.push(this._loanInfoService.ChangeDocumentType$.subscribe((res: any) => {
            this.Promise = this._loanInfoService.ChangeDocumentType(res);
        }));

        this._subscriptions.push(this._loanInfoService.SaveAndRevaluate$.subscribe((res: any) => {
            if (res) {
                this.Promise = this._loanInfoService.saveNReevaluate();
            }
        }));

        this._subscriptions.push(this._loanInfoService.CompleteLoan$.subscribe((res: any) => {
            if (res) {
                this.Promise = this._loanInfoService.CompleteLoan();
            }
        }));

        this._subscriptions.push(this._loanInfoService.ShowDocumentDetailView$.subscribe((res: boolean) => {
            this.ShowDocumentDetailView = res;
        }));

        this._subscriptions.push(this._loanInfoService.SO_width$.subscribe((res: string) => {
            this.SO_width = res;
        }));

        this._subscriptions.push(this._loanInfoService.STACKToggle$.subscribe((res: boolean) => {
            this.SO_Toggle();
        }));
    }

    SO_Toggle() {
        this.imgWidth = this.imgWidth === '' ? 'DOC_width' : '';
        if (!this.ShowDocumentDetailView) {
            this.imgWidth = '';
        }
    }

    GetLoanDetails() {
        if (this.loanSearchTableData.LoanID > 0) {
            this.Promise = this._loanInfoService.GetLoanDetails();
        } else {
            this.ShowErrorDiv = true;
        }
    }

    SOMouseLeaveToggle() {

        // if (this.showImgDiv && this.stackHidden) {
        //     this.SO_textShow = 'SO_textHIde';
        //     this.SO_TitleHide = 'SO_textShow';
        //     //this.SO_sub_icon = 'SO_sub_icon';
        //     this.SO_Icon = 'SO_Icon';
        //     this.SO_fullWidth = 'SO_fullWidth';
        //     this.imgWidth = 'DOC_width';
        //     this._stackDiv.nativeElement.style = '';
        //     this.SO_width = 'SO_width';
        //     this._panelShow = false;
        //     this.SO_CustHeight = 'soCustHeight332';
        // }
    }

    SOMouseEnterToggle() {
        // if (this.showImgDiv && this.stackHidden) {
        //     //this.SO_sub_icon = '';
        //     this.SO_Icon = '';
        //     this.SO_fullWidth = '';
        //     this.imgWidth = '';
        //     this.SO_width = '';
        //     this.SO_textShow = '';
        //     this.SO_TitleHide = '';
        //     //this.stackHidden = false;
        //     this._panelShow = true;
        //     this.SO_CustHeight = 'soCustHeight368';
        // }
    }

    back() {
        this._location.back();
    }

    ngOnDestroy() {
        this._subscriptions.forEach(element => {
            element.unsubscribe();
        });
    }
}
