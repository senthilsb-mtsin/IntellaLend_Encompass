import { NotificationService } from './../../../shared/service/notification.service';

import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject, ReplaySubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ExportDataAccess } from '../export.data';
import { LoanExportModel, LoanJobModel } from '../models/loan-export.model';
import { TenantRequestModel } from '../../loan-import/models/tenant-request.model';
import { EncompassExportModel, EncompassSearchExportModel, SearchExportModel } from '../models/encompass.export.model';
import { LosExportdateModel } from '../models/los.export.date.model';
import { ReExportLOSModel } from '../models/retry.los.export.model';

const jwtHelper = new JwtHelperService();

@Injectable()
export class ExportService {
  encompassmonitordata$ = new Subject();
  encompasstagingdata$ = new Subject();
  LosMonitorDetails$ = new Subject();
  LosExportStagingDetails$ = new Subject();
  retryLOSexport = new Subject();
  BatchData$ = new Subject();
  exportmonitordata$ = new Subject();
  DeleteBatch$ = new Subject();
  retryEexport$ = new Subject();
  retryExport$ = new Subject();
  AddbatchCustomer$ = new ReplaySubject();
  AddCustomer = 0;
  constructor(private _exportdata: ExportDataAccess, private _notificationservice: NotificationService) {

  }

  GetCurrentBatchData(input: LoanJobModel) {
    return this._exportdata.GetCurrentBatchData(input).subscribe(
      res => {
        if (res !== null) {
          const BatchData = jwtHelper.decodeToken(res.Data)['data'];
          if (BatchData !== null) {
            this.BatchData$.next(BatchData);
          }
        }
      }
    );
  }
  SearchExportMonitorDetails(input: SearchExportModel) {
    return this._exportdata.SearchExportMonitorDetails(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.exportmonitordata$.next(data);
        }
      });
  }

  GetExportMonitorDetails(input: TenantRequestModel) {
    return this._exportdata.GetExportMonitorDetails(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.exportmonitordata$.next(data);
        }
      }
    );
  }
  SearchEncompassExportDetails(input: EncompassSearchExportModel) {
    return this._exportdata.SearchEncompassExportDetails(input).subscribe(

      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.encompassmonitordata$.next(data);

        }
      }
    );
  }
  RetryLOSexportDetails(input: any) {
    return this._exportdata.RetryLosExportDetails(input).subscribe(

      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.retryLOSexport.next();
          this._notificationservice.showSuccess('Status Successfully Updated');
        }
      }
    );
  }
  ReExportLOSDetails(req: ReExportLOSModel) {
    return this._exportdata.ReExportLOSDetails(req).subscribe(
      res => {
        if (res !== null) {
          const Result = jwtHelper.decodeToken(res.Data)['data'];
          if (Result) {
            this.retryLOSexport.next();
            this._notificationservice.showSuccess('Re-Export initiated successfully');
          } else {
            this._notificationservice.showError('Unable to Re-Export');
          }
        }
      }
    );
  }
  SearchLOSExportDetails(input: LosExportdateModel) {

    return this._exportdata.SearchLosExportDetails(input).subscribe(

      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.LosMonitorDetails$.next(data);

        }
      }
    );
  }

  GetCurrentLOSExportStagingDetails(input: any) {
    return this._exportdata.GetCurrentLosExportDetails(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          this.LosExportStagingDetails$.next(data);

        }
      }
    );
  }

  GetEncompassExportDetails(input: EncompassExportModel) {

    return this._exportdata.GetEncompassExportDetails(input).subscribe(
      res => {
        if (res !== null) {
          const BatchData = jwtHelper.decodeToken(res.Data)['data'];
          if (BatchData !== null) {
            this.encompasstagingdata$.next(BatchData);

          }
        }
      }
    );
  }
  RetryLoanExport(input: LoanExportModel) {
    return this._exportdata.RetryLoanExport(input).subscribe(
      res => {
        if (res !== null) {

          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null) {
            if (data === true) {
              this.retryExport$.next();
              this._notificationservice.showSuccess('Status Successfully Updated');
            }
          }
        }
      });
  }
  DeleteBatch(input: LoanJobModel) {
    return this._exportdata.DeleteBatch(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data === true) {
            this.DeleteBatch$.next();
            this._notificationservice.showSuccess('Job Deleted Successfully');
          }
        } else {
          this._notificationservice.showError('Batch Not Deleted');
        }
      });
  }
  RetryEncompassUploadStaging(input: EncompassExportModel) {
    return this._exportdata.RetryEncompassUploadStaging(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null) {
            if (data === true) {
              this._notificationservice.showSuccess('Status Successfully Updated');
            }
          }
        }
      });
  }
  RetryEncompassExport(input: EncompassExportModel) {
    this._exportdata.RetryEncompassExport(input).subscribe(
      res => {
        if (res !== null) {
          const data = jwtHelper.decodeToken(res.Data)['data'];
          if (data !== null) {
            if (data === true) {
              this.retryEexport$.next();
              this._notificationservice.showSuccess('Status Successfully Updated');
            }
          }
        }
      });
  }

  validate(): boolean {
    return this.AddCustomer === 0 ? (this._notificationservice.showError('Select Customer Name'), true) : false;
  }
}
