import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { LayoutDataAccess } from '../layout.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NotificationService } from '@mts-notification';
import { MTSAPIResponse } from '@mts-api-response-model';
import { SessionHelper } from '@mts-app-session';

const jwtHelper = new JwtHelperService();

@Injectable()
export class LayoutService {
  ischangeRole = new Subject<boolean>();
  setLoginRoute = new Subject();
  defaultRoute = new Subject<string>();

  constructor(private _layoutData: LayoutDataAccess, private _notificationService: NotificationService) { }

  CheckCurrentPassword(req: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string }) {
    this._layoutData.CheckCurrentPassword(req).subscribe((res: MTSAPIResponse) => {
      if (jwtHelper.decodeToken(res.Data)['data']) {
        this.updateNewPassword(req);
      } else {
        this._notificationService.showError('Current Password is Wrong');
      }
    });
  }

  setDefaultRoute(_resBody: { TableSchema: string, RoleID: number, UserID: number, ADLogin: boolean }) {
    this._layoutData.getDefaultRouteData(_resBody).subscribe(
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

  updateNewPassword(req: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string }) {
    this._layoutData.UpdateNewPasswordForExpiry(req).subscribe((res) => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      if (data) {
        this.setLoginRoute.next(true);
        this._notificationService.showSuccess('Password Updated Successfully');
      } else { this._notificationService.showError('You have entered your old password'); }
    });
  }
  // private updateNewPassword(req: { TableSchema: string, UserID: number, CurrentPassword: string, NewPassword: string }) {
  //   this._layoutData.updateNewPassword(req).subscribe((res) => {
  //     this._notificationService.showSuccess('Password Updated Successfully');
  //     this.setLoginRoute.next();
  //   });
  // }
}
