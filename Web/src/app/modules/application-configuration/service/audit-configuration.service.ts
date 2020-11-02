import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { ConfigRequestModel } from '../models/config-request.model';
import { UpdateAuditModel } from '../models/save-audit-config.model';
import { NotificationService } from '@mts-notification';
const jwtHelper = new JwtHelperService();
@Injectable()
export class AuditConfigService {
  auditTableData$ = new Subject();
  isUpdated$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess
    , private _notificationservice: NotificationService) { }

  GetAllAuditConfig(inputs: ConfigRequestModel) {
    return this._appconfigdata.GetAllAuditConfig(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.auditTableData$.next(result);
        }
      }
    );
  }

  UpdateAuditConfig(inputs: UpdateAuditModel) {
    return this._appconfigdata.UpdateAuditConfig(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
            this._notificationservice.showSuccess('Updated Successfully');
            this.isUpdated$.next(result);
          } else {
            this._notificationservice.showWarning('Updated Failed');
          }

        }
      }
    );
  }
}
