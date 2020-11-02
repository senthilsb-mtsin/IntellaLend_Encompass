import { Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ApplicationConfigDataAccess } from './../application-configuration.data';
import { Injectable } from '@angular/core';
import { ConfigRequestModel } from '../models/config-request.model';
import { ReportMasterRequestModel } from '../models/report-master-request.model';
import { NotificationService } from '@mts-notification';
const jwtHelper = new JwtHelperService();
@Injectable()
export class ReportMasterService {
  reportMasterdata$ = new Subject();
  getDocumentlist$ = new Subject();
  saveReportConfig$ = new Subject();
  deleteReportConfig$ = new Subject();
  constructor(private _appconfigdata: ApplicationConfigDataAccess,
    private _notificationservice: NotificationService) { }
  GetReportMasterData(inputs: ConfigRequestModel) {
    return this._appconfigdata.GetReportMasterData(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];
          this.reportMasterdata$.next(result);
        }
      }
    );
  }
  GetDocsList(inputs: ConfigRequestModel) {
    return this._appconfigdata.GetDocsList(inputs).subscribe((res) => {
      if (res !== null) {
        const data = jwtHelper.decodeToken(res.Data)['data'];
        this.getDocumentlist$.next(data);
      }
    });
  }
  SaveEditDocs(inputs: ReportMasterRequestModel) {
    return this._appconfigdata.SaveEditDocs(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];

          if (result) {
            this._notificationservice.showSuccess('Saved Successfully');
            this.saveReportConfig$.next(result);

          } else {
            this._notificationservice.showWarning('Save Failed');
          }
        }
      }
    );
  }
  DeleteReportMaster(inputs: ReportMasterRequestModel) {
    return this._appconfigdata.DeleteReportMaster(inputs).subscribe(
      (res) => {
        if (res !== null) {
          const result = jwtHelper.decodeToken(res.Data)['data'];

          if (result) {
            this._notificationservice.showSuccess('Deleted Successfully');

            this.deleteReportConfig$.next(result);
          } else {

            this._notificationservice.showWarning('Save Failed');
          }
        }
      }
    );
  }
}
