import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { NotificationService } from '@mts-notification';
import { ApplicationConfigService } from './application-configuration.service';
const jwtHelper = new JwtHelperService();
@Injectable()
export class PasswordPolicyService {
  passwordpolicydata$ = new Subject();
  issaved$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess
    , private _notificationservice: NotificationService
    , private _appconfigservice: ApplicationConfigService) { }
  GetPasswordPolicy() {
    return this._appconfigdata.GetPasswordPolicy().subscribe(
      (res) => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data) {
            this.passwordpolicydata$.next(data);
          }
        }
      }
    );
  }
  SavePasswordPolicy(inputs: { PasswordPolicy: any }) {
    return this._appconfigdata.SavePasswordPolicy(inputs).subscribe((res) => {
      if (res !== null) {
      //  const data = jwtHelper.decodeToken(res.Data)['data'];
        this._appconfigservice.RefreshConfig();
        this._notificationservice.showSuccess(
          'Password Policy Saved Successfully '
        );

      }
    });
  }
}
