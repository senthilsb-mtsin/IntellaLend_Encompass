import { ConfigRequestModel } from './config-request.model';

export class ConfigAllRequestModel extends ConfigRequestModel {
  TenantConfigType: TenantConfigType;
  constructor(TableSchema: string, _tenantConfigType: TenantConfigType) {
    super(TableSchema);
    this.TenantConfigType = _tenantConfigType;
  }
}
export class TenantConfigType {
  CustomerID = 0;
}
