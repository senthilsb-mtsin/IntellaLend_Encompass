import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { AddUpdateStipulationModel } from '../models/stipulation.model';
import { ConfigRequestModel } from '../models/config-request.model';
import { NotificationService } from '@mts-notification';
const jwtHelper = new JwtHelperService();
@Injectable()
export class StipulationCategoryService {
  stipulationTableData$ = new Subject();
  isstipulationchanged$ = new Subject();

  constructor(private _appconfigdata: ApplicationConfigDataAccess,
    private _notificationservice: NotificationService) { }
  GetStipulationList(inputs: ConfigRequestModel) {
    return this._appconfigdata.GetStipulationList(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.stipulationTableData$.next(result);
        }
      }
    );
  }

  UpdateData(input: AddUpdateStipulationModel) {
    return this._appconfigdata.UpdateStipulationData(input).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];

          if (result) {
            this._notificationservice.showSuccess('Updated Successfully');
            this.isstipulationchanged$.next(result);
          } else {
            this._notificationservice.showWarning('Updated Failed');
          }
        }
      }
    );
  }
  SaveData(inputs: AddUpdateStipulationModel) {
    return this._appconfigdata.SaveStipulationData(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];

          if (result) {
            this._notificationservice.showSuccess('Saved Successfully');
            this.isstipulationchanged$.next(result);
          } else {
            this._notificationservice.showWarning('Save Failed');
          }
        }
      }
    );
  }
}
