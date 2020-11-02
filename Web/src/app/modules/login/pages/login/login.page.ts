import { Subscription } from 'rxjs';
import { AppSettings } from '@mts-app-setting';
import { environment } from '../../../../../environments/environment';

import { Component, ViewChild, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { trigger, state, style } from '@angular/animations';

import * as CryptoJS from '../../../../../assets/crypto-js';
import { LoginRequest } from '../../models/login-request.model';
import { GetMenuListRequest } from '../../models/get-menu-list.model';
import { LoginService } from '../../services/login.service';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { NotificationService } from '@mts-notification';
import { SelectComponent } from '@mts-select2';
import { SessionHelper } from '@mts-app-session';

@Component({
  moduleId: 'LoginComponent',
  selector: 'mts-login',
  templateUrl: 'login.page.html',
  styleUrls: ['login.page.css'],
  animations: [
    trigger('LoginAnimation', [
      state(
        'up',
        style({
          transform: 'translate(0, -350px)',
          transition: 'transform 0.5s',
        })
      ),
      state(
        'down',
        style({
          transform: 'translate(0, 0px)',
          transition: 'transform 0.5s',
        })
      ),
    ]),
  ],
})
export class LoginComponent implements OnInit {
  @ViewChild('roleSelect') roleSelect;
  @ViewChild(SelectComponent) select: SelectComponent;
  AppVersion: string = environment.applicationVersion;
  validationMsg = '';
  loginState = '';
  roleState = '';
  userId = 0;
  RolevalidationMsg = '';
  ExpriyValidationMsg = 'Password expired kindly reset';
  isExipry = false;
  currentPwd: any;
  newPwd: any;
  confirmPwd: any;
  PassWordExpiry: any = 'down';

  validationUserMsg = '';
  showLoading = false;
  roleItems: { id: number; text: string }[] = [];
  loginForm: FormGroup;

  isValid: any = 'Red';
  isMinLengthValid: any = 'Red';
  isNumberExist: any = 'Red';
  isUpperCaseCharacterExist: any = 'Red';
  isDisabled = true;
  AD_login: boolean = environment.ADAuthentication;
  constructor(
    private _route: Router,
    private formCollection: FormBuilder,
    private _loginService: LoginService,
    private _notificationService: NotificationService
  ) {
    this.loginForm = formCollection.group({
      userName: ['', Validators.required],
      passWord: ['', Validators.required],
    });
  }
  private specialCharRegex = new RegExp(/[-!$%^&*()_+@|~=`{}\[\]:";'<>?,.\/]/);
  private numberCheckRegex = new RegExp(/.*[0-9].*/);
  private upperCaseCheckRegex = new RegExp(/[A-Z]]*/);
  private fullRegex = new RegExp(/^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,20}$/);

  private subscription: Subscription[] = [];

  ngOnInit() {

    this.subscription.push(this._loginService.validationMsg.subscribe((res: string) => {
      this.validationMsg = res;
    }));

    this.subscription.push(this._loginService.showLoading.subscribe((res: boolean) => {
      this.showLoading = res;
    }));

    this.subscription.push(this._loginService.setLoginForm.subscribe((res: boolean) => {
      if (res) {
        this.loginForm.get('userName').setValue('');
        this.loginForm.get('passWord').setValue('');
      }
    }));

    this.subscription.push(this._loginService.loginState.subscribe((res: string) => {
      this.loginState = res;
      this.roleState = res;
    }));

    this.subscription.push(this._loginService.roleItems.subscribe((res) => {
      this.roleItems = res;
    }));
    this.subscription.push(this._loginService.userId.subscribe((res: number) => {
      this.userId = res;
    }));

    this.subscription.push(this._loginService.newUser.subscribe((res: boolean) => {
      if (res) { this._route.navigate(['/newuser']); }
    }));

    this.subscription.push(this._loginService.singleRole.subscribe((res: string) => {
      this.setDefaultRoute(res);
    }));

    this.subscription.push(this._loginService.defaultRoute.subscribe((res: string) => {
      this._route.navigate(['view/' + res]);
    }));
    this.subscription.push(this._loginService.isExpired.subscribe((res: boolean) => {
      this.isExipry = res;
    }));
  }

  RoleStage() {
    this._loginService.loginState.next('down');
  }

  setDefaultRoute(roleID: string) {
    const input = new GetMenuListRequest(
      AppSettings.TenantSchema,
      roleID,
      this.userId
    );
    this._loginService.setDefaultRoute(input);
  }

  setMenuRoute() {
    if (this.roleSelect.activeOption === undefined) {
      this.RolevalidationMsg = 'Please Select a Role ';
    } else {
      this.RolevalidationMsg = '';
      const selectedID = this.roleSelect.activeOption.id;

      if (isTruthy(selectedID)) {
        const input = new GetMenuListRequest(
          AppSettings.TenantSchema,
          selectedID,
          this.userId
        );
        this._loginService.setDefaultRoute(input);
      } else {
        this._notificationService.showError('Role is Required');
      }
    }
  }

  EmptyValidationMsg(ifVal: any) {
    this.RolevalidationMsg = '';
  }

  login(value: Object, valid: any) {
    if (valid) {
      const _reqBody = new LoginRequest(
        AppSettings.TenantSchema,
        value['userName'],
        CryptoJS.MD5(value['passWord']).toString()
      );
      this._loginService.showLoading.next(true);
      this._loginService.loginSubmit(_reqBody);
    } else {
      this.showLoading = false;
      this.validationMsg = '';
      this.validationUserMsg = '';
      if (value['userName'] === '' && value['passWord'] === '') {
        this.validationMsg = 'Username and Password is Required';
      } else if (value['userName'] === '') {
        this.validationUserMsg = 'Username is Required';
      } else if (value['passWord'] === '') {
        this.validationMsg = 'Password is Required';
      }
    }
  }

  submitForm() {
    if (isTruthy(this.confirmPwd)) {
      if (isTruthy(this.newPwd)) {
        if (this.newPwd === this.confirmPwd) {
          const _reqBody = {
            TableSchema: AppSettings.TenantSchema,
            UserID: SessionHelper.UserDetails.UserID,
            CurrentPassword: CryptoJS.MD5(this.currentPwd).toString(),
            NewPassword: CryptoJS.MD5(this.newPwd).toString()
          };
          this._loginService.updateNewPassword(_reqBody);
        } else {
          this._notificationService.showError('Passwords don\'t match');
        }
      } else {
        this._notificationService.showError('Password Required');
      }
    } else {
      this._notificationService.showError('Current Password Required');
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
    if (this.fullRegex.test(this.newPwd) && this.confirmPwd === this.newPwd) {
      this.isDisabled = false;
    } else {
      this.isDisabled = true;
    }
  }

  GotoDashboard() {
    this._route.navigate(['view']);
  }

  ngDestroy() {
    this.subscription.forEach(element => {
      element.unsubscribe();
    });
  }
}
