import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { LoanDataAccess } from '../loantype.data';
import { JwtHelperService } from '@auth0/angular-jwt';
import { LoanTypeDatatableModel } from '../models/loantype-datatable.model';
import { SyncCustomerRequest } from '../models/sync-customer-request.model';
import { AppSettings } from '@mts-app-setting';
import { NotificationService } from '@mts-notification';
import { SyncDetailsRequest } from '../models/sync-details-request.model';

const jwtHelper = new JwtHelperService();

@Injectable()
export class LoanTypeService {
  ischangeRole = new Subject<boolean>();
  setLoanMasterTableData = new Subject<LoanTypeDatatableModel[]>();
  setSyncDetailsTableData = new Subject<SyncDetailsRequest[]>();
  syncModalShow = new Subject<boolean>();
  _syncDetails: SyncDetailsRequest;
  constructor(private _loanTypeData: LoanDataAccess, private _notificationService: NotificationService) { }

  private loanMasterData: LoanTypeDatatableModel[] = [];
  private _loanTypeType: { Type: string, LoanTypeID: number, LoanTypeName: string, Active: boolean } = { Type: 'Add', LoanTypeID: 0, LoanTypeName: '', Active: true };
  GetLoanTypeMaster(): LoanTypeDatatableModel[] {
    return this.loanMasterData.slice();
  }

  getLoanTypeList() {
    return this._loanTypeData.GetLoanTypeList({ TableSchema: AppSettings.SystemSchema }).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this.loanMasterData = data;
      this.setLoanMasterTableData.next(data);
    });
  }
  GetSyncDetails(req: any) {
    return this._loanTypeData.GetSyncDetails(req).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this._syncDetails = data;
      this.setSyncDetailsTableData.next(data);
    });
  }
  SyncLoanType(req: SyncCustomerRequest) {
    this._loanTypeData.SyncLoanType(req).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      if (data) {
        this._notificationService.showSuccess('Synchronize will be done shortly');
        this.getLoanTypeList();
      } else {
        this._notificationService.showError('Synchronize Failed');
      }
    });
  }

  setLoanType(_loanType: { Type: string, LoanTypeID: number, LoanTypeName: string, Active: boolean }) {
    this._loanTypeType = _loanType;
  }

  getLoanType() {
    return this._loanTypeType;
  }
  clearLoanType() {
    this._loanTypeType = { Type: 'Add', LoanTypeID: 0, LoanTypeName: '', Active: true };
  }
}
