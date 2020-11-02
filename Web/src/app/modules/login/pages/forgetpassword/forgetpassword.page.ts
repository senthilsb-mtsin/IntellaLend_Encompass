
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { ForgetPasswordService } from '../../services/forget-password.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'mts-forgetpassword',
  templateUrl: 'forgetpassword.page.html',
  styleUrls: ['forgetpassword.page.css']
})
export class ForgetpasswordComponent implements OnInit, OnDestroy {

  showThankYouDiv = false;

  UserName: any;
  apiInput: any = { TableSchema: AppSettings.TenantSchema };

  constructor(private route: Router, public _forgetService: ForgetPasswordService) {
  }
  private subscribtions: Subscription[] = [];

  ngOnInit() {
    this.subscribtions.push(this._forgetService.showThankYouDiv.subscribe((res: boolean) => {
      this.showThankYouDiv = res;
    }));
  }

  submitUserNameForm() {
    if (isTruthy(this.UserName)) {
      this.apiInput['UserName'] = this.UserName;
      this._forgetService.submitUserNameForm(this.apiInput);
    }
  }

  ok() {
    this.route.navigate(['']);
  }

  ngOnDestroy() {
    this.subscribtions.forEach(element => {
      element.unsubscribe();
    });
  }
}
