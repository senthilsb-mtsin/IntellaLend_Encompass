import { ApplicationConfigService } from './../../service/application-configuration.service';
import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { NotificationService } from '@mts-notification';
import { PasswordPolicyService } from '../../service/password-policy.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'mts-password-policy',
  templateUrl: './password-policy.component.html',
  styleUrls: ['./password-policy.component.css'],
  providers: [PasswordPolicyService],
})
export class PasswordPolicyComponent implements OnInit, AfterViewInit, OnDestroy {
  _passwordPolicy: any = {
    PasswordExpiryDays: '',
    StoreOldPassword: false,
    NoOfOldPassword: '',
  };
  constructor(
    private _passwordservice: PasswordPolicyService,
    private _notificationservice: NotificationService,
    private _appconfigservice: ApplicationConfigService
  ) { }
  private subscription: Subscription[] = [];
  ngOnInit(): void {
    this.subscription.push(
      this._passwordservice.passwordpolicydata$.subscribe((data: any) => {
        if (data !== null) {
          if (data.NoOfOldPassword === 0) {
            data.NoOfOldPassword = '';
          }
          this._passwordPolicy = data;
        }
        this.SetKeyValue('', this._passwordPolicy.StoreOldPassword);
      })
    );
  }
  ngAfterViewInit() {
    this.GetPasswordPolicy();
  }
  SavePasswordPolicy() {
    if (this.ValidateDays()) {
      const input = { PasswordPolicy: this._passwordPolicy };
      this._passwordservice.SavePasswordPolicy(input);
    }
  }

  ValidateDays(): boolean {
    let isMatched = false;
    if (
      this._passwordPolicy.PasswordExpiryDays.toString().match(
        '^[0-9]{1,3}$'
      ) &&
      this._passwordPolicy.PasswordExpiryDays > 0
    ) {
      isMatched = true;
    } else {
      this._notificationservice.showWarning('Enter valid Password expiry days');
    }
    if (this._passwordPolicy.StoreOldPassword) {
      if (
        this._passwordPolicy.NoOfOldPassword.toString().match('^[0-9]{1,2}$') &&
        this._passwordPolicy.NoOfOldPassword > 0 &&
        this._passwordPolicy.NoOfOldPassword <= 10
      ) {
        if (isMatched) {
          isMatched = true;
        }
      } else {
        isMatched = false;
        this._notificationservice.showWarning(
          'Enter valid Number of old password between 1-10'
        );
      }
    }
    return isMatched;
  }
  SetKeyValue(vals: any, isChecked) {
    if (isChecked) {
      document.getElementById('storePassword').classList.add('showinput');
      document.getElementById('oldPasswordSpan').style.display = '';
    } else {
      this._passwordPolicy.NoOfOldPassword = '';
      document.getElementById('storePassword').classList.remove('showinput');
      document.getElementById('oldPasswordSpan').style.display = 'none';
    }
  }
  CancelPasswordPolicy() {
    this._appconfigservice.RefreshConfig();
  }
  GetPasswordPolicy() {
    this._passwordservice.GetPasswordPolicy();
  }
  ngOnDestroy() {
    this.subscription.forEach((element) => {
      element.unsubscribe();
    });
  }
}
