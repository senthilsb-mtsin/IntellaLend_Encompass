import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { SmtpSaveRequestModel } from '../models/smtp-request-data.model';
import { NotificationService } from '@mts-notification';
import { ApplicationConfigService } from './application-configuration.service';
const jwtHelper = new JwtHelperService();
@Injectable()
export class SMTPService {
  isSaved$ = new Subject();
  smtpDetails$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess,
    private _notificationservice: NotificationService
    , private _appconfigservice: ApplicationConfigService) { }
  SaveSMTPSubmit(Inputs: SmtpSaveRequestModel) {
    return this._appconfigdata.SaveSMTPSubmit(Inputs).subscribe((res) => {
      if (res !== null) {
       // const data = jwtHelper.decodeToken(res.Data)['data'];
        this._appconfigservice.RefreshConfig();
        this._notificationservice.showSuccess(
          'SMTP Details Updated Successfully '
        );
      }
    });
  }
  GetAllSMPTDetails() {
    return this._appconfigdata.GetAllSMPTDetails().subscribe(
      (res) => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.smtpDetails$.next(data[0]);
        }
      }
    );
  }
  CloseSmtp() {
    this._appconfigservice.RefreshConfig();

  }
}
