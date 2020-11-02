import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from '../application-configuration.data';
import { Injectable } from '@angular/core';
import { ConfigTypeRequestModel } from '../models/config-value.model';
import { UpdateLoanSearchModel } from '../models/custom-loan-search.model';
import { NotificationService } from '@mts-notification';
const jwtHelper = new JwtHelperService();
@Injectable()
export class CustomLoansearchService {
  searchtabledata$ = new Subject();
  isupdated$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess,
    private _notificationservice: NotificationService) { }

  GetLoanSearchFilterConfigValue(inputs: ConfigTypeRequestModel): any {
    return this._appconfigdata.GetLoanSearchFilterConfigValue(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.searchtabledata$.next(result);
        }
      }
    );
  }

  UpdateLoanSearchFilterStatus(inputs: UpdateLoanSearchModel): any {
    return this._appconfigdata.UpdateLoanSearchFilterStatus(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          if (result) {
            this._notificationservice.showSuccess('Updated Successfully');
            this.isupdated$.next(result);
          } else {

            this._notificationservice.showWarning(
              'Atleast select one search criteria'
            );
          }

        }
      }
    );
  }
}
