import { Injectable } from '@angular/core';
import { ServiceTypeDataAccess } from '../service-type.data';
import { NotificationService } from '@mts-notification';
import { Subject } from 'rxjs';
import { AppSettings } from '@mts-app-setting';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ServiceTypeModel } from '../models/service-type.model';

const jwtHelper = new JwtHelperService();
@Injectable()
export class ServiceTypeService {

  setServiceTypeMasterTableData = new Subject<ServiceTypeModel[]>();

  constructor(private _serviceTypeData: ServiceTypeDataAccess, private _notificationService: NotificationService) { }

  private ServiceTypeMasterData: ServiceTypeModel[] = [];
  private _serviceTypeType: { Type: string, ServiceTypeID: number, ServiceTypeName: string } = { Type: 'Add', ServiceTypeID: 0, ServiceTypeName: '' };

  GetServiceTypeMaster(): ServiceTypeModel[] {
    return this.ServiceTypeMasterData.slice();
  }

  getServiceTypeList() {
    return this._serviceTypeData.GetServiceTypeList({ TableSchema: AppSettings.TenantSchema }).subscribe(res => {
      const data = jwtHelper.decodeToken(res.Data)['data'];
      this.ServiceTypeMasterData = data;
      this.setServiceTypeMasterTableData.next(this.ServiceTypeMasterData.slice());
    });
  }

  setServiceType(_serviceType: { Type: string, ServiceTypeID: number, ServiceTypeName: string }) {
    this._serviceTypeType = _serviceType;
  }

  getServiceType() {
    return this._serviceTypeType;
  }

  clearServiceType() {
    this._serviceTypeType = { Type: 'Add', ServiceTypeID: 0, ServiceTypeName: '' };
  }
}
