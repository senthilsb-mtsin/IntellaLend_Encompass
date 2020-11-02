import { Subject, ReplaySubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import {
  GetBoxTokenModel,
  GetBoxValueModel,
  CheckBoxTokenModel,
} from '../models/box-setting.model';
import { ConfigRequestModel } from '../models/config-request.model';
import { NotificationService } from '@mts-notification';
const jwtHelper = new JwtHelperService();
@Injectable()
export class BoxSettingsService {
  boxToken$ = new ReplaySubject(1);
  allBoxSettings$ = new ReplaySubject(1);
  boxValues$ = new Subject();
  isCheckedToken$ = new ReplaySubject(1);

  constructor(private _appconfigdata: ApplicationConfigDataAccess,
    private _noticationservice: NotificationService) { }
  GetBoxToken(inputRequest: GetBoxTokenModel) {
    return this._appconfigdata.GetBoxToken(inputRequest).subscribe(
      (res) => {
        const Result = jwtHelper.decodeToken(res.Data)['data'];
        if (Result !== null) {
          this.boxToken$.next(Result);
          this._noticationservice.showSuccess(
            'Box Settings Saved Successfully'
          );

        }

      }
    );
  }
  GetAllBoxSettings(inputRequest: ConfigRequestModel) {
    return this._appconfigdata.GetAllBoxSettings(inputRequest).subscribe(
      (res) => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.allBoxSettings$.next(Result);
        }
      }
    );
  }
  getboxvalues(inputRequest: GetBoxValueModel) {
    return this._appconfigdata.getboxvalues(inputRequest).subscribe(
      (res) => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.boxValues$.next(Result);
        }
      }
    );
  }
  CheckUserBoxToken(inputRequest: CheckBoxTokenModel) {
    return this._appconfigdata.CheckUserBoxToken(inputRequest).subscribe(
      (res) => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          this.isCheckedToken$.next(Result);
        }
      }
    );
  }
}
