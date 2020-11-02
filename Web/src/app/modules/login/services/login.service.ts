import { MTSAPIResponse } from '@mts-api-response-model';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoginDataAccess } from '../login.data';
import { LoginRequest } from '../models/login-request.model';
import { SessionHelper } from '@mts-app-session';
import { GetMenuListRequest } from '../models/get-menu-list.model';
import { NotificationService } from '@mts-notification';
import { environment } from 'src/environments/environment';
import { Console } from 'console';

const jwtHelper = new JwtHelperService();

@Injectable()
export class LoginService {
  // Login Page
  validationMsg = new Subject<string>();
  showLoading = new Subject<boolean>();
  loginState = new Subject<string>();
  roleItems = new Subject<{ id: number; text: string }[]>();
  userId = new Subject<number>();
  newUser = new Subject<boolean>();
  singleRole = new Subject<string>();
  defaultRoute = new Subject<string>();
  isExpired = new Subject<boolean>();
  RouteNavigate = new Subject();
  setLoginForm = new Subject<boolean>();
  AD_login: boolean = environment.ADAuthentication;
  constructor(private loginData: LoginDataAccess, private _notificationService: NotificationService) { }

  private _roleItems: { id: number; text: string }[] = [];
  private _userId = 0;

  loginSubmit(_reqBody: LoginRequest) {

    this.loginData.userSubmit(_reqBody).subscribe((res: MTSAPIResponse) => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this.showLoading.next(false);
        if (data !== null) {
          if (data.Success ) {
            const user = data.User;
            this.validationMsg.next('');
            if (user.Active) {
              if (!user.Locked) {
                localStorage.setItem('userDetails', res.Data);
                SessionHelper.setSessionVariables();
                if (user.Status === 0) {
                  this.newUser.next(true);
                } else {
                  this.isExpired.next(false);
                  if (data.ExpiryDays && !this.AD_login) {
                    this.isExpired.next(data.ExpiryDays);
                    this.loginState.next('up');
                  } else {
                    this._userId = user.UserID;
                    this.userId.next(this._userId);

                    SessionHelper.setSessionVariables();

                    if (user.UserRoleMapping.length > 1) {
                      this._roleItems = [];
                      user.UserRoleMapping.forEach((element) => {
                        this._roleItems.push({
                          id: element.RoleID,
                          text: element.RoleName,
                        });
                      });

                      this.roleItems.next(this._roleItems.slice());
                      this.loginState.next('up');
                    } else {
                      this.singleRole.next(user.UserRoleMapping[0].RoleID);
                    }
                  }
                }
              } else {
                this.showLoading.next(false);
                this.validationMsg.next('User Already Logged-In/Locked');
              }
            } else {
              this.showLoading.next(false);
              this.validationMsg.next('User is Inactive');
            }
          } else {
            if (data.Message === 'User Null' && data.Locked === true) {
              this.validationMsg.next('User Locked for No of Attempts');
            } else if (data.Message === 'User Null' && data.Locked === false) {
              this.validationMsg.next('Wrong Username or Password');
            } else if (data.Message === 'Password Expired') {
              this.isExpired.next(true);
              this.loginState.next('up');
              // this.validationMsg.next('Password Expired, Please Contact Administrator');
            } else if (data.Message === 'License Not Valid') {
              this.validationMsg.next('License Expired');
            } else if (data.Message === 'Login success but no roles assigned') {
              this.validationMsg.next('');
              this._notificationService.showError('No Role(s) Available')
            } else {
              this.validationMsg.next(data.Message);
            }
            this.showLoading.next(false);
          }
        }
      } else { this._notificationService.showError('Error Contact Administrator'); }
    });
  }

  setDefaultRoute(_resBody: GetMenuListRequest) {
    this.loginData.getDefaultRouteData(_resBody).subscribe(
      (res: MTSAPIResponse) => {
        const MenuList = jwtHelper.decodeToken(res.Data)['data'];
        if (MenuList !== null) {
          localStorage.setItem('roleDetails', res.Data);
          SessionHelper.setSessionVariables();
          this.defaultRoute.next(MenuList.StartPage);
        }
      }
    );
  }

  submitNewUserForm(req: any) {
    this.loginData.submitNewUserForm(req).subscribe(
      res => {
        if (res.Token !== null) {
          this._notificationService.showSuccess('Password Has Been Updated Successfully');
          this.RouteNavigate.next(true);
        }
      }
    );
  }

  updateNewPassword(req: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string }) {
    this.loginData.UpdateNewPasswordForExpiry(req).subscribe((res) => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      if (data) {
        this.setLoginForm.next(true);
        this.loginState.next('down');
      } else { this._notificationService.showError('You have entered your old password'); }
    });
    // this.loginData.CheckCurrentPassword(req).subscribe((res) => {
    //   this._notificationService.showSuccess('Password Updated Successfully');
    // });
  }
}
