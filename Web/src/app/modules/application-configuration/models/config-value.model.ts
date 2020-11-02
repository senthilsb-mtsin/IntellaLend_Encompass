export class ConfigTypeRequestModel {
  TableSchema?: string;
  ConfigKey: any;
  CustomerID: number;
  constructor(CustomerID: number, ConfigKey: any, TableSchema?: string) {
    this.TableSchema = TableSchema;
    this.ConfigKey = ConfigKey;
    this.CustomerID = CustomerID;
  }
}
export class ConfigValueModel extends ConfigTypeRequestModel {
  ConfigValue: string;
  Active: boolean;
  ConfigID?: number;
  constructor(
    CustomerID: number,
    ConfigValue: any,
    Active: boolean,
    ConfigKey: any,
    ConfigID?: number
  ) {
    super(CustomerID, ConfigKey);

    this.ConfigValue = ConfigValue;

    this.Active = Active;
    this.ConfigID = ConfigID;
  }
}
export class AddEditConfigModel {
  TableSchema: string;
  TenantConfigType: ConfigValueModel;
  constructor(TableSchema: string, TenantConfigType: ConfigValueModel) {
    this.TableSchema = TableSchema;
    this.TenantConfigType = TenantConfigType;
  }
}
