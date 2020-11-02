import { Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { SessionHelper } from '@mts-app-session';
import { LayoutService } from '../../service/layout.service';
import { AppSettings } from '@mts-app-setting';
import { NotificationService } from '@mts-notification';

import * as CryptoJS from '../../../../../assets/crypto-js';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
    selector: 'mts-resetpassword',
    templateUrl: 'reset-password.page.html',
    styleUrls: ['reset-password.page.css'],
})
export class ResetPasswordComponent implements OnInit, OnDestroy {

    currentPwd: any;
    newPwd: any;
    confirmPwd: any;
    ValidationMsg: String = '';
    isValid: any = 'Red';
    isMinLengthValid: any = 'Red';
    isNumberExist: any = 'Red';
    isUpperCaseCharacterExist: any = 'Red';
    isDisabled = true;
    apiInput: any = { TableSchema: AppSettings.TenantSchema, UserID: 0, CurrentPassword: '', NewPassword: '' };
    user: any = SessionHelper.UserDetails;
    constructor(private router: Router, private _layoutServie: LayoutService, private _notifyService: NotificationService) {

    }

    private specialCharRegex = new RegExp(/[-!$%^&*()_+@|~=`{}\[\]:";'<>?,.\/]/);
    private numberCheckRegex = new RegExp(/.*[0-9].*/);
    private upperCaseCheckRegex = new RegExp(/[A-Z]]*/);
    private fullRegex = new RegExp(/^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,20}$/);

    private _subscription: Subscription[] = [];

    ngOnInit() {
        this._subscription.push(this._layoutServie.setLoginRoute.subscribe(res => {
            this.router.navigate(['view/login']);
        }));
    }

    submitForm() {
        if (isTruthy(this.confirmPwd)) {
            if (isTruthy(this.newPwd)) {
                if (this.newPwd === this.confirmPwd) {
                    this.apiInput['UserID'] = this.user.UserID;
                    this.apiInput['CurrentPassword'] = CryptoJS.MD5(this.currentPwd).toString();
                    this.apiInput['NewPassword'] = CryptoJS.MD5(this.newPwd).toString();
                    this._layoutServie.CheckCurrentPassword(this.apiInput);
                } else {
                    this._notifyService.showError('Passwords don\'t match');
                }
            } else {
                this._notifyService.showError('Password Required');
            }
        } else {
            this._notifyService.showError('Current Password Required');
        }
    }

    CheckPasswordPolicy() {
        if (this.specialCharRegex.test(this.newPwd)) {
            this.isValid = 'green';
        } else {
            this.isValid = 'red';
        }

        if (this.newPwd.length >= 8 && this.newPwd.length <= 20) {
            this.isMinLengthValid = 'green';
        } else {
            this.isMinLengthValid = 'red';
        }

        if (this.numberCheckRegex.test(this.newPwd)) {
            this.isNumberExist = 'green';
        } else {
            this.isNumberExist = 'red';
        }
        if (this.upperCaseCheckRegex.test(this.newPwd)) {
            this.isUpperCaseCharacterExist = 'green';
        } else {
            this.isUpperCaseCharacterExist = 'Red';
        }
        if (this.fullRegex.test(this.newPwd)) {
            this.isDisabled = false;
        } else {
            this.isDisabled = true;
        }
    }

    ngOnDestroy(): void {
        this._subscription.forEach(element => {
            element.unsubscribe();
        });
    }

}
