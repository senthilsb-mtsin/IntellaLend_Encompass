
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { AppSettings } from '@mts-app-setting';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { Subscription } from 'rxjs';
import { SessionHelper } from '@mts-app-session';
import * as CryptoJS from '../../../../../assets/crypto-js';
import { LoginService } from '../../services/login.service';
import { NotificationService } from '@mts-notification';

@Component({
    selector: 'mts-newuser',
    templateUrl: 'new-user.page.html',
    styleUrls: ['new-user.page.css']
})
export class NewUserComponent implements OnInit, OnDestroy {
    password: any = '';
    isValid: any = 'Red';
    isMinLengthValid: any = 'Red';
    isNumberExist: any = 'Red';
    isUpperCaseCharacterExist: any = 'Red';
    isDisabled = true;

    constructor(
        private _route: Router, private _loginService: LoginService, private _notificationService: NotificationService) { }

    private specialCharRegex = new RegExp(/[-!$%^&*()_+@|~=`{}\[\]:";'<>?,.\/]/);
    private numberCheckRegex = new RegExp(/.*[0-9].*/);
    private upperCaseCheckRegex = new RegExp(/[A-Z]]*/);
    private fullRegex = new RegExp(/^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,20}$/);
    private _subscription: Subscription;
    ngOnInit() {
        this._subscription = this._loginService.RouteNavigate.subscribe(res => {
            this._route.navigate(['']);
        });
    }

    CheckPasswordPolicy() {
        if (this.specialCharRegex.test(this.password)) {
            this.isValid = 'green';
        } else {
            this.isValid = 'red';
        }
        if (this.password.length >= 8 && this.password.length <= 20) {
            this.isMinLengthValid = 'green';
        } else {
            this.isMinLengthValid = 'red';
        }
        if (this.numberCheckRegex.test(this.password)) {
            this.isNumberExist = 'green';
        } else {
            this.isNumberExist = 'red';
        }
        if (this.upperCaseCheckRegex.test(this.password)) {
            this.isUpperCaseCharacterExist = 'green';
        } else {
            this.isUpperCaseCharacterExist = 'Red';
        }
        if (this.fullRegex.test(this.password)) {
            this.isDisabled = false;
        } else {
            this.isDisabled = true;
        }
    }

    submitForm() {
        const user = SessionHelper.UserDetails;
        if (isTruthy(this.password)) {
            const req = { TableSchema: AppSettings.TenantSchema, NewPassword: CryptoJS.MD5(this.password).toString(), SecurityQuestion: { UserID: user.UserID } };
            this._loginService.submitNewUserForm(req);
        } else {
            this._notificationService.showError('Password  Required');
        }
    }

    ngOnDestroy() {
        this._subscription.unsubscribe();
    }
}
