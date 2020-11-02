import { Component, OnInit, OnDestroy } from '@angular/core';
import { SessionHelper } from '@mts-app-session';
import { LoanImportService } from '../../services/loan-import.service';
import { Subscription } from 'rxjs';
import { GetBoxTokenModel } from 'src/app/modules/application-configuration/models/box-setting.model';
import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'mts-loan-import',
  templateUrl: './loan-import.page.html',
  styleUrls: ['./loan-import.page.css']
})
export class LoanImportComponent implements OnInit, OnDestroy {
  showHide: any = [false, false];
  importTab = 'loan_importMonitor';
  promise: Subscription;
  userDetails: any;
  constructor(private _loanImportService: LoanImportService,
    private aroute: ActivatedRoute, ) {
    this.userDetails = SessionHelper.UserDetails;
    this.checkPermission('Adhoc', 0);
    this.checkPermission('Box.Com', 1);

  }
  private BoxAuthCode: any;
  private _getTokenCall = false;
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    if (this.showHide[0]) {
      this.importTab = 'ad_loanImport';
    } else if (!this.showHide[0] && this.showHide[1]) {
      this.importTab = 'loan_importBox';
    }
    this.subscription.push(
      this._loanImportService.enableLoanMonitor$.subscribe(
        (result: any) => {
          if (result) {
            this.importTab = 'loan_importMonitor';
          }
        }
      )
    );
    if (this.showHide[1] && !this._getTokenCall) {
      this.GetBoxToken();
    }
  }
  checkPermission(component: string, index: number): void {
    const URL = 'View\\BoxUpload\\' + component;
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
  GetBoxToken() {
    this._getTokenCall = true;
    if (isTruthy(this.aroute.queryParams['value'].code)) {
      this.BoxAuthCode = this.aroute.queryParams['value'].code;
      const inputRequest = new GetBoxTokenModel(AppSettings.TenantSchema, SessionHelper.UserDetails.UserID, this.BoxAuthCode);
      this._loanImportService.GetBoxTokenImport(inputRequest);
    }
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
