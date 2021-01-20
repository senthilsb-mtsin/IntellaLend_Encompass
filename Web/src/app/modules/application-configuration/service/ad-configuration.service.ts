import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { SmtpSaveRequestModel } from '../models/smtp-request-data.model';
import { NotificationService } from '@mts-notification';
import { ApplicationConfigService } from './application-configuration.service';
import { AppSettings } from '@mts-app-setting';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ConfigRequestModel } from '../models/config-request.model';
import { Adconfigmodel } from '../models/ad-configuration.model';
const jwtHelper = new JwtHelperService();
@Injectable()
export class ADConfigService {
  _ADGroupFields$ = new Subject<any>();
  _ADGroupFields = [];
  ADGroups$ = new Subject();
  showModal$ = new Subject<boolean>();
  yourModal: ModalDirective;
  adconfigdata$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess
    , private _notificationservice: NotificationService
    , private _appconfigservice: ApplicationConfigService) { }
  GetADGroups(ldapUrl: string) {
    const req = { TableSchema: AppSettings.TenantSchema, LDAPUrl: ldapUrl };
    this._appconfigdata.GetADGroups(req).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      if (data !== null) {
        this._ADGroupFields$.next(data);
        this._ADGroupFields = data;
      }
    });
  }
  showModal() {
    this._ADGroupFields$.next(this._ADGroupFields);
  }
  GetADConfig() {
    const req = new ConfigRequestModel(AppSettings.TenantSchema);
    return this._appconfigdata.GetADConfigValue(req).subscribe(
      (res) => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data) {
            this.adconfigdata$.next(data);

          }
        }
      }
    );
  }

  SaveADConfig(adconfig: Adconfigmodel) {
    const req = { TableSchema: AppSettings.TenantSchema, ADDOMAIN: adconfig.AD_Domain, LDAPURL: adconfig.LDAP_url };
    this._appconfigdata.SaveADConfigValue(req).subscribe(
      (res) => {
        if (res !== null) {
          this._notificationservice.showSuccess(
            'AD Configuration Saved Successfully '
          );
        }
      }
    );
  }

}
